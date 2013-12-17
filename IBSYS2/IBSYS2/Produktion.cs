using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IBSYS2
{
    public partial class Produktion : UserControl
    {
        private OleDbConnection myconn;
        private char[] digits = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
        private String sprache = "de";

        // Datenweitergabe:
        int aktPeriode;
        int[] auftraege = new int[12];
        double[,] direktverkaeufe = new double[3, 4];
        int[,] sicherheitsbest = new int[30, 5];
        int[,] produktion = new int[30, 2];
        int[,] produktionProg = new int[3, 5];
        int[,] prodReihenfolge = new int[30, 2];
        int[,] kapazitaet = new int[15, 5];
        int[,] kaufauftraege = new int[29, 6];
        
        int periode;

        List<int> sicherheitsbe = new List<int>();

        List<int> lagerbestand = new List<int>();
        List<int> bearbeitung = new List<int>();
        List<int> wartelisteAr = new List<int>();
        List<int> wartelisteMa = new List<int>();

        // Array fuer berechnete Produktionsmengen
        int[,] berProduktion = new int[30, 2];
        int[,] backupProduktion = new int[30, 2];

        public int[,] BackupProduktion
        {
            get { return backupProduktion; }
            set { backupProduktion = value; }
        }

        public Produktion()
        {
            InitializeComponent();
            continue_btn.Enabled = false;

            string databasename = @"Provider=Microsoft.ACE.OLEDB.12.0; Data Source=IBSYS_DB.accdb";
            myconn = new OleDbConnection(databasename);

            for (int i = 3; i < sicherheitsbest.GetLength(0); i++) // bei 3 anfangen, weil dort die E-Teile anfangen
            {
                sicherheitsbe.Add(sicherheitsbest[i, 1]);
            }

            System.Windows.Forms.ToolTip ToolTipDE = new System.Windows.Forms.ToolTip();
            System.Windows.Forms.ToolTip ToolTipEN = new System.Windows.Forms.ToolTip();
            if (pic_de.SizeMode != PictureBoxSizeMode.Normal & sprache == "de")
            {
                ToolTipEN.RemoveAll();
                ToolTipDE.SetToolTip(this.pictureBox7, Sprachen.DE_PR_INFO);
            }
            else
            {
                ToolTipDE.RemoveAll();
                ToolTipEN.SetToolTip(this.pictureBox7, Sprachen.EN_PR_INFO);
            }

            berechneProduktion();
            ProduktionETeile();
        }

        public Produktion(int[,] sicherheitsbe)
        {
            this.sicherheitsbest = sicherheitsbe;
        }

        public Produktion(int aktPeriode, int[] auftraege, double[,] direktverkaeufe, int[,] sicherheitsbest,
            int[,] produktion, int[,] produktionProg, int[,] prodReihenfolge, int[,] kapazitaet, int[,] kaufauftraege,
            String sprache)
        {
            this.sprache = sprache;
            this.aktPeriode = aktPeriode;
            if (auftraege != null)
            {
                this.auftraege = auftraege;
            }
            if (direktverkaeufe != null)
            {
                this.direktverkaeufe = direktverkaeufe;
            }
            if (sicherheitsbest != null)
            {
                this.sicherheitsbest = sicherheitsbest;
            }
            if (produktion != null)
            {
                this.produktion = produktion;
            }
            if (produktionProg != null)
            {
                this.produktionProg = produktionProg;
            }
            if (prodReihenfolge != null)
            {
                this.prodReihenfolge = prodReihenfolge;
            }
            if (kapazitaet != null)
            {
                this.kapazitaet = kapazitaet;
            }
            if (kaufauftraege != null)
            {
                this.kaufauftraege = kaufauftraege;
            }

            // var UserControl kapa= new Kapazitaetsplan();
            InitializeComponent();
            continue_btn.Enabled = false;
            sprachen();

            string databasename = @"Provider=Microsoft.ACE.OLEDB.12.0; Data Source=IBSYS_DB.accdb";
            myconn = new OleDbConnection(databasename);

            for (int i = 3; i < sicherheitsbest.GetLength(0); i++) // bei 3 anfangen, weil dort die E-Teile anfangen
            {
                sicherheitsbe.Add(sicherheitsbest[i, 1]);
            }

            System.Windows.Forms.ToolTip ToolTipDE = new System.Windows.Forms.ToolTip();
            System.Windows.Forms.ToolTip ToolTipEN = new System.Windows.Forms.ToolTip();
            if (pic_de.SizeMode != PictureBoxSizeMode.Normal & sprache != "en")
            {
                ToolTipEN.RemoveAll();
                ToolTipDE.SetToolTip(this.pictureBox7, Sprachen.DE_PR_INFO);
            }
            else
            {
                ToolTipDE.RemoveAll();
                ToolTipEN.SetToolTip(this.pictureBox7, Sprachen.EN_PR_INFO);
            }

            // aktPeriode = aktuelle Periode, periode = Periode aus XML (letzte Periode)
            periode = aktPeriode - 1;

            Boolean bereitsBerechnet = false;
            for (int i = 0; i < produktion.GetLength(0); i++)
            {
                if (produktion[i, 1] > 0)
                {
                    bereitsBerechnet = true;
                    break;
                }
            }
            // Wenn produktion bereits Werte enthaelt, sollen diese in berProduktion eingespeist werden
            if (bereitsBerechnet == true)
            {
                berProduktion = produktion; // fuer die E-Teile
                textBox1.Text = produktionProg[0, 1].ToString();
                textBox2.Text = produktionProg[1, 1].ToString();
                textBox3.Text = produktionProg[2, 1].ToString();
                textBox6.Text = produktionProg[0, 2].ToString();
                textBox4.Text = produktionProg[1, 2].ToString();
                textBox5.Text = produktionProg[2, 2].ToString();
                textBox7.Text = produktionProg[0, 3].ToString();
                textBox8.Text = produktionProg[1, 3].ToString();
                textBox9.Text = produktionProg[2, 3].ToString();
                textBox10.Text = produktionProg[0, 4].ToString();
                textBox11.Text = produktionProg[1, 4].ToString();
                textBox12.Text = produktionProg[2, 4].ToString();
            }
            // sonst neu berechnen
            else
            {
                berechneProduktion();
                ProduktionETeile();
            }
        }

        private void check()
        {
            bool weiter = true;
            for (int i = 1; i <= 12; ++i)
            {
                if (this.Controls.Find("textBox" + i.ToString(), true)[0].Text == "" || this.Controls.Find("textBox" + i.ToString(), true)[0].ForeColor == Color.Red)
                {
                    weiter = false;
                    
                }
                else
                {
                    continue;
                }
            }
            if(weiter == true)
            {
                continue_btn.Enabled = true;
            }
            else
            {
                continue_btn.Enabled = false;
            }
        }

        private void berechneProduktion()
        {
            //für aktuelle Periode
            double p1 = auftraege[0] + direktverkaeufe[0, 1];
            double p2 = auftraege[1] + direktverkaeufe[1, 1];
            double p3 = auftraege[2] + direktverkaeufe[2, 1];

            //+ eingabe Sicherheitsbestand 
            double sp1 = sicherheitsbest[0, 1];
            double sp2 = sicherheitsbest[1, 1];
            double sp3 = sicherheitsbest[2, 1];

            //- Lagerbestand Vorperiode 
            int lagerbestandp1 = Daten("1", "Bestand", "Teilenummer_FK", "Lager", periode);
            int lagerbestandp2 = Daten("2", "Bestand", "Teilenummer_FK", "Lager", periode);
            int lagerbestandp3 = Daten("3", "Bestand", "Teilenummer_FK", "Lager", periode);
            
            //- Aufträge in Warteschlange 
            int WartelisteMap1 = Daten("1", "Menge", "Fehlteil_Teilenummer_FK", "Warteliste_Material", periode);
            int WartelisteMap2 = Daten("2", "Menge", "Fehlteil_Teilenummer_FK", "Warteliste_Material", periode);
            int WartelisteMap3 = Daten("3", "Menge", "Fehlteil_Teilenummer_FK", "Warteliste_Material", periode);
            int WartelisteAr1 = Daten("1", "Menge", "Teilenummer_FK", "Warteliste_Arbeitsplatz", periode);
            int WartelisteAr2 = Daten("2", "Menge", "Teilenummer_FK", "Warteliste_Arbeitsplatz", periode);
            int WartelisteAr3 = Daten("3", "Menge", "Teilenummer_FK", "Warteliste_Arbeitsplatz", periode);
            
            //- Aufträge in Bearbeitung
            int Bearbeitungp1 = Daten("1", "Menge", "Teilenummer_FK", "Bearbeitung", periode);
            int Bearbeitungp2 = Daten("2", "Menge", "Teilenummer_FK", "Bearbeitung", periode);
            int Bearbeitungp3 = Daten("3", "Menge", "Teilenummer_FK", "Bearbeitung", periode);

           // Eingabe Aufträge + eingabe Sicherheitsbestand - Lagerbestand Vorperiode - Aufträge in Warteschlange - Aufträge in Bearbeitung
            string prod1 = Convert.ToInt32(p1 + sp1 - lagerbestandp1 - WartelisteAr1 - WartelisteMap1 - Bearbeitungp1).ToString();
            string prod2 = Convert.ToInt32(p2 + sp2 - lagerbestandp2 - WartelisteAr2 - WartelisteMap2 - Bearbeitungp2).ToString();
            string prod3 = Convert.ToInt32(p3 + sp3 - lagerbestandp3 - WartelisteAr3 - WartelisteMap3 - Bearbeitungp3).ToString();
            
            if (prod1.StartsWith("-"))
            {
                textBox1.Text = "0";
            }
            else
            {
                textBox1.Text = prod1;
            }
            if (prod2.StartsWith("-"))
            {
                textBox2.Text = "0";
            }
            else
            {
                textBox2.Text = prod2;
            }
            if (prod3.StartsWith("-"))
            {
                textBox3.Text = "0";
            }
            else
            {
                textBox3.Text = prod3;
            }

            berProduktion[0, 0] = 1;
            berProduktion[0, 1] = Convert.ToInt32(prod1);
            berProduktion[1, 0] = 2;
            berProduktion[1, 1] = Convert.ToInt32(prod2);
            berProduktion[2, 0] = 3;
            berProduktion[2, 1] = Convert.ToInt32(prod3);

            #region Produktion der Prognosen
            double prognose1p1 = auftraege[3];
            double prognose1p2 = auftraege[4];
            double prognose1p3 = auftraege[5];
            double prognose2p1 = auftraege[6];
            double prognose2p2 = auftraege[7];
            double prognose2p3 = auftraege[8];
            double prognose3p1 = auftraege[9];
            double prognose3p2 = auftraege[10];
            double prognose3p3 = auftraege[11];

            string prognosep1 = Convert.ToInt32((prognose1p1 + prognose2p1 + prognose3p1) / 3 * 1.1).ToString();
            if (prognosep1.StartsWith("-"))
            {
                textBox6.Text = "0";
                textBox7.Text = "0";
                textBox10.Text = "0";
            }
            else
            {
                textBox6.Text = prognosep1;
                textBox7.Text = prognosep1;
                textBox10.Text = prognosep1;
            }

            string prognosep2 = Convert.ToInt32((prognose1p2 + prognose2p2 + prognose3p2) / 3 * 1.1).ToString();
            if (prognosep2.StartsWith("-"))
            {
                textBox4.Text = "0";
                textBox8.Text = "0";
                textBox11.Text = "0";
            }
            else
            {
                textBox4.Text = prognosep2;
                textBox8.Text = prognosep2;
                textBox11.Text = prognosep2;
            }

            string prognosep3 = Convert.ToInt32((prognose1p3 + prognose2p3 + prognose3p3) / 3 * 1.1).ToString();
            if (prognosep3.StartsWith("-"))
            {
                textBox5.Text = "0";
                textBox9.Text = "0";
                textBox12.Text = "0";
            }
            else
            {
                textBox5.Text = prognosep3;
                textBox9.Text = prognosep3;
                textBox12.Text = prognosep3; 
            }

            #endregion

        }

        public int[,] ProduktionETeile()
        {
            int p26;
            int p51;
            int p16;
            int p17;
            int p50;
            int p4;
            int p10;
            int p49;
            int p7;
            int p13;
            int p18;

            int p56;
            int p55;
            int p5;
            int p11;
            int p54;
            int p8;
            int p14;
            int p19;

            int p31;
            int p30;
            int p6;
            int p12;
            int p29;
            int p9;
            int p15;
            int p20;
            #region DB
            string databasename = @"Provider=Microsoft.ACE.OLEDB.12.0; Data Source=IBSYS_DB.accdb";
            myconn = new OleDbConnection(databasename);
            OleDbCommand cmd = new OleDbCommand();
            cmd.CommandType = CommandType.Text;
            cmd.Connection = myconn;

            try
            {
                myconn.Open();
            }
            catch (Exception)
            {
                if (pic_de.SizeMode == PictureBoxSizeMode.StretchImage)
                {
                    System.Windows.Forms.MessageBox.Show("DB-Verbindung wurde nicht ordnugnsgemäß geschlossen bei der letzten Verwendung, Verbindung wird neu gestartet, bitte haben Sie einen Moment Geduld.");
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("DB connection was not closed correctly, connection will be restarted, please wait a moment.");
                }
                myconn.Close();
                myconn.Open();
            }
            #endregion

            #region Daten aus DB
            int a = 0;
            List<List<int>> lagerbestand = new List<List<int>>();
            cmd.CommandText = @"SELECT Teilenummer_FK, Bestand FROM Lager WHERE periode = " + periode + ";";
            OleDbDataReader dbReader = cmd.ExecuteReader();
            while (dbReader.Read())
            {
                lagerbestand.Add(new List<int>());
                lagerbestand[a].Add(Convert.ToInt32(dbReader["Teilenummer_FK"]));
                lagerbestand[a].Add(Convert.ToInt32(dbReader["Bestand"]));
                ++a;
            }
            dbReader.Close();

            a = 0;
            List<List<int>> warteliste_arbeitsplatz = new List<List<int>>();
            cmd.CommandText = @"SELECT Teilenummer_FK, Menge FROM Warteliste_Arbeitsplatz WHERE Periode = " + periode + ";";
            dbReader = cmd.ExecuteReader();
            while (dbReader.Read())
            {
                warteliste_arbeitsplatz.Add(new List<int>());
                warteliste_arbeitsplatz[a].Add(Convert.ToInt32(dbReader["Teilenummer_FK"]));
                warteliste_arbeitsplatz[a].Add(Convert.ToInt32(dbReader["Menge"]));
                ++a;
            }
            dbReader.Close();

            a = 0;
            List<List<int>> warteliste_material = new List<List<int>>();
            cmd.CommandText = @"SELECT Fehlteil_Teilenummer_FK, Menge FROM Warteliste_Material WHERE Periode = " + periode + ";";
            dbReader = cmd.ExecuteReader();
            while (dbReader.Read())
            {
                warteliste_material.Add(new List<int>());
                warteliste_material[a].Add(Convert.ToInt32(dbReader["Fehlteil_Teilenummer_FK"]));
                warteliste_material[a].Add(Convert.ToInt32(dbReader["Menge"]));
                ++a;
            }
            dbReader.Close();

            a = 0;
            List<List<int>> bearbeitung = new List<List<int>>();
            cmd.CommandText = @"SELECT Teilenummer_FK, Menge FROM Bearbeitung WHERE Periode = " + periode + ";";
            dbReader = cmd.ExecuteReader();
            while (dbReader.Read())
            {
                bearbeitung.Add(new List<int>());
                bearbeitung[a].Add(Convert.ToInt32(dbReader["Teilenummer_FK"]));
                bearbeitung[a].Add(Convert.ToInt32(dbReader["Menge"]));
                ++a;
            }
            dbReader.Close();
            #endregion

            #region Daten zur Berechnung
            p26 = auftraege[0] + Convert.ToInt32(direktverkaeufe[0, 1]) + sicherheitsbest[20, 1];
            p51 = auftraege[0] + Convert.ToInt32(direktverkaeufe[0, 1]) + sicherheitsbest[26, 1];

            p16 = p51 + sicherheitsbest[15, 1];
            p17 = p51 + sicherheitsbest[16, 1];
            p50 = p51 + sicherheitsbest[25, 1];

            p4 = p50 + sicherheitsbest[3, 1];
            p10 = p50 + sicherheitsbest[9, 1];
            p49 = p50 + sicherheitsbest[24, 1];

            p7 = p49 + sicherheitsbest[6, 1];
            p13 = p49 + sicherheitsbest[12, 1];
            p18 = p49 + sicherheitsbest[17, 1];

            p56 = auftraege[1] + Convert.ToInt32(direktverkaeufe[1, 1]) + sicherheitsbest[29, 1];

            p55 = p56 + sicherheitsbest[28, 1];

            p5 = p55 + sicherheitsbest[4, 1];
            p11 = p55 + sicherheitsbest[10, 1];
            p54 = p55 + sicherheitsbest[27, 1];

            p8 = p54 + sicherheitsbest[7, 1];
            p14 = p54 + sicherheitsbest[13, 1];
            p19 = p54 + sicherheitsbest[18, 1];

            p31 = auftraege[2] + Convert.ToInt32(direktverkaeufe[2, 1]) + sicherheitsbest[23, 1];

            p30 = p31 + sicherheitsbest[22, 1];

            p6 = p30 + sicherheitsbest[5, 1];
            p12 = p30 + sicherheitsbest[11, 1];
            p29 = p30 + sicherheitsbest[21, 1];

            p9 = p29 + sicherheitsbest[8, 1];
            p15 = p29 + sicherheitsbest[14, 1];
            p20 = p29 + sicherheitsbest[19, 1];
            #endregion
            int[] teilenummer = new int[]{26,51,16,17,50,4,10,49,7,13,18,56,
                55,5,11,54,8,14,19,31,30,6,12,29,9,15,20};

            for (int i = 0; i < teilenummer.Count(); i++)
            {
                #region Bearbeitung
                            for (int e = 0; e < bearbeitung.Count; e++)
                            {
                                if (bearbeitung[e][0] == teilenummer[0])
                                {
                                    p26 -= bearbeitung[e][1];
                                }
                                if (bearbeitung[e][0] == teilenummer[1])
                                {
                                    p51 -= bearbeitung[e][1];
                                }
                                if (bearbeitung[e][0] == teilenummer[2])
                                {
                                    p16 -= bearbeitung[e][1];
                                }
                                if (bearbeitung[e][0] == teilenummer[3])
                                {
                                    p17 -= bearbeitung[e][1];
                                }
                                if (bearbeitung[e][0] == teilenummer[4])
                                {
                                    p50 -= bearbeitung[e][1];
                                }
                                if (bearbeitung[e][0] == teilenummer[5])
                                {
                                    p4 -= bearbeitung[e][1];
                                }
                                if (bearbeitung[e][0] == teilenummer[6])
                                {
                                    p10 -= bearbeitung[e][1];
                                }
                                if (bearbeitung[e][0] == teilenummer[7])
                                {
                                    p49 -= bearbeitung[e][1];
                                }
                                if (bearbeitung[e][0] == teilenummer[8])
                                {
                                    p7 -= bearbeitung[e][1];
                                }
                                if (bearbeitung[e][0] == teilenummer[9])
                                {
                                    p13 -= bearbeitung[e][1];
                                }
                                if (bearbeitung[e][0] == teilenummer[10])
                                {
                                    p18 -= bearbeitung[e][1];
                                }
                                if (bearbeitung[e][0] == teilenummer[11])
                                {
                                    p56 -= bearbeitung[e][1];
                                }
                                if (bearbeitung[e][0] == teilenummer[12])
                                {
                                    p55 -= bearbeitung[e][1];
                                }
                                if (bearbeitung[e][0] == teilenummer[13])
                                {
                                    p5 -= bearbeitung[e][1];
                                }
                                if (bearbeitung[e][0] == teilenummer[14])
                                {
                                    p11 -= bearbeitung[e][1];
                                }
                                if (bearbeitung[e][0] == teilenummer[15])
                                {
                                    p54 -= bearbeitung[e][1];
                                }
                                if (bearbeitung[e][0] == teilenummer[16])
                                {
                                    p8 -= bearbeitung[e][1];
                                }
                                if (bearbeitung[e][0] == teilenummer[17])
                                {
                                    p14 -= bearbeitung[e][1];
                                }
                                if (bearbeitung[e][0] == teilenummer[18])
                                {
                                    p19 -= bearbeitung[e][1];
                                }
                                if (bearbeitung[e][0] == teilenummer[19])
                                {
                                    p31 -= bearbeitung[e][1];
                                }
                                if (bearbeitung[e][0] == teilenummer[20])
                                {
                                    p30 -= bearbeitung[e][1];
                                }
                                if (bearbeitung[e][0] == teilenummer[21])
                                {
                                    p6 -= bearbeitung[e][1];
                                }
                                if (bearbeitung[e][0] == teilenummer[22])
                                {
                                    p12 -= bearbeitung[e][1];
                                }
                                if (bearbeitung[e][0] == teilenummer[23])
                                {
                                    p29 -= bearbeitung[e][1];
                                }
                                if (bearbeitung[e][0] == teilenummer[24])
                                {
                                    p9 -= bearbeitung[e][1];
                                }
                                if (bearbeitung[e][0] == teilenummer[25])
                                {
                                    p15 -= bearbeitung[e][1];
                                }
                                if (bearbeitung[e][0] == teilenummer[26])
                                {
                                    p20 -= bearbeitung[e][1];
                                }
                            }
                            #endregion
                #region Lagerbestand
                            for (int l = 0; l < lagerbestand.Count; l++)
                            {
                                if (lagerbestand[l][0] == teilenummer[0])
                                {
                                    p26 -= lagerbestand[l][1];
                                }
                                if (lagerbestand[l][0] == teilenummer[1])
                                {
                                    p51 -= lagerbestand[l][1];
                                }
                                if (lagerbestand[l][0] == teilenummer[2])
                                {
                                    p16 -= lagerbestand[l][1];
                                }
                                if (lagerbestand[l][0] == teilenummer[3])
                                {
                                    p17 -= lagerbestand[l][1];
                                }
                                if (lagerbestand[l][0] == teilenummer[4])
                                {
                                    p50 -= lagerbestand[l][1];
                                }
                                if (lagerbestand[l][0] == teilenummer[5])
                                {
                                    p4 -= lagerbestand[l][1];
                                }
                                if (lagerbestand[l][0] == teilenummer[6])
                                {
                                    p10 -= lagerbestand[l][1];
                                }
                                if (lagerbestand[l][0] == teilenummer[7])
                                {
                                    p49 -= lagerbestand[l][1];
                                }
                                if (lagerbestand[l][0] == teilenummer[8])
                                {
                                    p7 -= lagerbestand[l][1];
                                }
                                if (lagerbestand[l][0] == teilenummer[9])
                                {
                                    p13 -= lagerbestand[l][1];
                                }
                                if (lagerbestand[l][0] == teilenummer[10])
                                {
                                    p18 -= lagerbestand[l][1];
                                }
                                if (lagerbestand[l][0] == teilenummer[11])
                                {
                                    p56 -= lagerbestand[l][1];
                                }
                                if (lagerbestand[l][0] == teilenummer[12])
                                {
                                    p55 -= lagerbestand[l][1];
                                }
                                if (lagerbestand[l][0] == teilenummer[13])
                                {
                                    p5 -= lagerbestand[l][1];
                                }
                                if (lagerbestand[l][0] == teilenummer[14])
                                {
                                    p11 -= lagerbestand[l][1];
                                }
                                if (lagerbestand[l][0] == teilenummer[15])
                                {
                                    p54 -= lagerbestand[l][1];
                                }
                                if (lagerbestand[l][0] == teilenummer[16])
                                {
                                    p8 -= lagerbestand[l][1];
                                }
                                if (lagerbestand[l][0] == teilenummer[17])
                                {
                                    p14 -= lagerbestand[l][1];
                                }
                                if (lagerbestand[l][0] == teilenummer[18])
                                {
                                    p19 -= lagerbestand[l][1];
                                }
                                if (lagerbestand[l][0] == teilenummer[19])
                                {
                                    p31 -= lagerbestand[l][1];
                                }
                                if (lagerbestand[l][0] == teilenummer[20])
                                {
                                    p30 -= lagerbestand[l][1];
                                }
                                if (lagerbestand[l][0] == teilenummer[21])
                                {
                                    p6 -= lagerbestand[l][1];
                                }
                                if (lagerbestand[l][0] == teilenummer[22])
                                {
                                    p12 -= lagerbestand[l][1];
                                }
                                if (lagerbestand[l][0] == teilenummer[23])
                                {
                                    p29 -= lagerbestand[l][1];
                                }
                                if (lagerbestand[l][0] == teilenummer[24])
                                {
                                    p9 -= lagerbestand[l][1];
                                }
                                if (lagerbestand[l][0] == teilenummer[25])
                                {
                                    p15 -= lagerbestand[l][1];
                                }
                                if (lagerbestand[l][0] == teilenummer[26])
                                {
                                    p20 -= lagerbestand[l][1];
                                }
                            }
                            #endregion
                #region Wartelisten
                            for (int k = 0; k < warteliste_material.Count; k++)
                            {
                                if (warteliste_material[k][0] == teilenummer[0])
                                {
                                    p26 -= warteliste_material[k][1];
                                }
                                if (warteliste_material[k][0] == teilenummer[1])
                                {
                                    p51 -= warteliste_material[k][1];
                                }
                                if (warteliste_material[k][0] == teilenummer[2])
                                {
                                    p16 -= warteliste_material[k][1];
                                }
                                if (warteliste_material[k][0] == teilenummer[3])
                                {
                                    p17 -= warteliste_material[k][1];
                                }
                                if (warteliste_material[k][0] == teilenummer[4])
                                {
                                    p50 -= warteliste_material[k][1];
                                }
                                if (warteliste_material[k][0] == teilenummer[5])
                                {
                                    p4 -= warteliste_material[k][1];
                                }
                                if (warteliste_material[k][0] == teilenummer[6])
                                {
                                    p10 -= warteliste_material[k][1];
                                }
                                if (warteliste_material[k][0] == teilenummer[7])
                                {
                                    p49 -= warteliste_material[k][1];
                                }
                                if (warteliste_material[k][0] == teilenummer[8])
                                {
                                    p7 -= warteliste_material[k][1];
                                }
                                if (warteliste_material[k][0] == teilenummer[9])
                                {
                                    p13 -= warteliste_material[k][1];
                                }
                                if (warteliste_material[k][0] == teilenummer[10])
                                {
                                    p18 -= warteliste_material[k][1];
                                }
                                if (warteliste_material[k][0] == teilenummer[11])
                                {
                                    p56 -= warteliste_material[k][1];
                                }
                                if (warteliste_material[k][0] == teilenummer[12])
                                {
                                    p55 -= warteliste_material[k][1];
                                }
                                if (warteliste_material[k][0] == teilenummer[13])
                                {
                                    p5 -= warteliste_material[k][1];
                                }
                                if (warteliste_material[k][0] == teilenummer[14])
                                {
                                    p11 -= warteliste_material[k][1];
                                }
                                if (warteliste_material[k][0] == teilenummer[15])
                                {
                                    p54 -= warteliste_material[k][1];
                                }
                                if (warteliste_material[k][0] == teilenummer[16])
                                {
                                    p8 -= warteliste_material[k][1];
                                }
                                if (warteliste_material[k][0] == teilenummer[17])
                                {
                                    p14 -= warteliste_material[k][1];
                                }
                                if (warteliste_material[k][0] == teilenummer[18])
                                {
                                    p19 -= warteliste_material[k][1];
                                }
                                if (warteliste_material[k][0] == teilenummer[19])
                                {
                                    p31 -= warteliste_material[k][1];
                                }
                                if (warteliste_material[k][0] == teilenummer[20])
                                {
                                    p30 -= warteliste_material[k][1];
                                }
                                if (warteliste_material[k][0] == teilenummer[21])
                                {
                                    p6 -= warteliste_material[k][1];
                                }
                                if (warteliste_material[k][0] == teilenummer[22])
                                {
                                    p12 -= warteliste_material[k][1];
                                }
                                if (warteliste_material[k][0] == teilenummer[23])
                                {
                                    p29 -= warteliste_material[k][1];
                                }
                                if (warteliste_material[k][0] == teilenummer[24])
                                {
                                    p9 -= warteliste_material[k][1];
                                }
                                if (warteliste_material[k][0] == teilenummer[25])
                                {
                                    p15 -= warteliste_material[k][1];
                                }
                                if (warteliste_material[k][0] == teilenummer[26])
                                {
                                    p20 -= warteliste_material[k][1];
                                }
                            }
                            for (int m = 0; m < warteliste_arbeitsplatz.Count; m++)
                            {
                                if (warteliste_arbeitsplatz[m][0] == teilenummer[0])
                                {
                                    p26 -= warteliste_arbeitsplatz[m][1];
                                }
                                if (warteliste_arbeitsplatz[m][0] == teilenummer[1])
                                {
                                    p51 -= warteliste_arbeitsplatz[m][1];
                                }
                                if (warteliste_arbeitsplatz[m][0] == teilenummer[2])
                                {
                                    p16 -= warteliste_arbeitsplatz[m][1];
                                }
                                if (warteliste_arbeitsplatz[m][0] == teilenummer[3])
                                {
                                    p17 -= warteliste_arbeitsplatz[m][1];
                                }
                                if (warteliste_arbeitsplatz[m][0] == teilenummer[4])
                                {
                                    p50 -= warteliste_arbeitsplatz[m][1];
                                }
                                if (warteliste_arbeitsplatz[m][0] == teilenummer[5])
                                {
                                    p4 -= warteliste_arbeitsplatz[m][1];
                                }
                                if (warteliste_arbeitsplatz[m][0] == teilenummer[6])
                                {
                                    p10 -= warteliste_arbeitsplatz[m][1];
                                }
                                if (warteliste_arbeitsplatz[m][0] == teilenummer[7])
                                {
                                    p49 -= warteliste_arbeitsplatz[m][1];
                                }
                                if (warteliste_arbeitsplatz[m][0] == teilenummer[8])
                                {
                                    p7 -= warteliste_arbeitsplatz[m][1];
                                }
                                if (warteliste_arbeitsplatz[m][0] == teilenummer[9])
                                {
                                    p13 -= warteliste_arbeitsplatz[m][1];
                                }
                                if (warteliste_arbeitsplatz[m][0] == teilenummer[10])
                                {
                                    p18 -= warteliste_arbeitsplatz[m][1];
                                }
                                if (warteliste_arbeitsplatz[m][0] == teilenummer[11])
                                {
                                    p56 -= warteliste_arbeitsplatz[m][1];
                                }
                                if (warteliste_arbeitsplatz[m][0] == teilenummer[12])
                                {
                                    p55 -= warteliste_arbeitsplatz[m][1];
                                }
                                if (warteliste_arbeitsplatz[m][0] == teilenummer[13])
                                {
                                    p5 -= warteliste_arbeitsplatz[m][1];
                                }
                                if (warteliste_arbeitsplatz[m][0] == teilenummer[14])
                                {
                                    p11 -= warteliste_arbeitsplatz[m][1];
                                }
                                if (warteliste_arbeitsplatz[m][0] == teilenummer[15])
                                {
                                    p54 -= warteliste_arbeitsplatz[m][1];
                                }
                                if (warteliste_arbeitsplatz[m][0] == teilenummer[16])
                                {
                                    p8 -= warteliste_arbeitsplatz[m][1];
                                }
                                if (warteliste_arbeitsplatz[m][0] == teilenummer[17])
                                {
                                    p14 -= warteliste_arbeitsplatz[m][1];
                                }
                                if (warteliste_arbeitsplatz[m][0] == teilenummer[18])
                                {
                                    p19 -= warteliste_arbeitsplatz[m][1];
                                }
                                if (warteliste_arbeitsplatz[m][0] == teilenummer[19])
                                {
                                    p31 -= warteliste_arbeitsplatz[m][1];
                                }
                                if (warteliste_arbeitsplatz[m][0] == teilenummer[20])
                                {
                                    p30 -= warteliste_arbeitsplatz[m][1];
                                }
                                if (warteliste_arbeitsplatz[m][0] == teilenummer[21])
                                {
                                    p6 -= warteliste_arbeitsplatz[m][1];
                                }
                                if (warteliste_arbeitsplatz[m][0] == teilenummer[22])
                                {
                                    p12 -= warteliste_arbeitsplatz[m][1];
                                }
                                if (warteliste_arbeitsplatz[m][0] == teilenummer[23])
                                {
                                    p29 -= warteliste_arbeitsplatz[m][1];
                                }
                                if (warteliste_arbeitsplatz[m][0] == teilenummer[24])
                                {
                                    p9 -= warteliste_arbeitsplatz[m][1];
                                }
                                if (warteliste_arbeitsplatz[m][0] == teilenummer[25])
                                {
                                    p15 -= warteliste_arbeitsplatz[m][1];
                                }
                                if (warteliste_arbeitsplatz[m][0] == teilenummer[26])
                                {
                                    p20 -= warteliste_arbeitsplatz[m][1];
                                }
                            }
                            #endregion
            }
            berProduktion[3, 0] = 4;
            berProduktion[3, 1] = p4;
            berProduktion[4, 0] = 5;
            berProduktion[4, 1] = p5;
            berProduktion[5, 0] = 6;
            berProduktion[5, 1] = p6;
            berProduktion[6, 0] = 7;
            berProduktion[6, 1] = p7;
            berProduktion[7, 0] = 8;
            berProduktion[7, 1] = p8;
            berProduktion[8, 0] = 9;
            berProduktion[8, 1] = p9;
            berProduktion[9, 0] = 10;
            berProduktion[9, 1] = p10;
            berProduktion[10, 0] = 11;
            berProduktion[10, 1] = p11;
            berProduktion[11, 0] = 12;
            berProduktion[11, 1] = p12;
            berProduktion[12, 0] = 13;
            berProduktion[12, 1] = p13;
            berProduktion[13, 0] = 14;
            berProduktion[13, 1] = p14;
            berProduktion[14, 0] = 15;
            berProduktion[14, 1] = p15;
            berProduktion[15, 0] = 16;
            berProduktion[15, 1] = p16;
            berProduktion[16, 0] = 17;
            berProduktion[16, 1] = p17;
            berProduktion[17, 0] = 18;
            berProduktion[17, 1] = p18;
            berProduktion[18, 0] = 19;
            berProduktion[18, 1] = p19;
            berProduktion[19, 0] = 20;
            berProduktion[19, 1] = p20;
            berProduktion[20, 0] = 26;
            berProduktion[20, 1] = p26;
            berProduktion[21, 0] = 29;
            berProduktion[21, 1] = p29;
            berProduktion[22, 0] = 30;
            berProduktion[22, 1] = p30;
            berProduktion[23, 0] = 31;
            berProduktion[23, 1] = p31;
            berProduktion[24, 0] = 49;
            berProduktion[24, 1] = p49;
            berProduktion[25, 0] = 50;
            berProduktion[25, 1] = p50;
            berProduktion[26, 0] = 51;
            berProduktion[26, 1] = p51;
            berProduktion[27, 0] = 54;
            berProduktion[27, 1] = p54;
            berProduktion[28, 0] = 55;
            berProduktion[28, 1] = p55;
            berProduktion[29, 0] = 56;
            berProduktion[29, 1] = p56;

            return berProduktion;

        }

        private int Daten(string teilenummer_FK, string spalte, string spalte1, string tabelle, int periode)
        {
            OleDbCommand cmd = new OleDbCommand();
            cmd.CommandType = CommandType.Text;
            cmd.Connection = myconn;
            try
            {
                myconn.Open();
            }
            catch (Exception)
            {
                myconn.Close();
                myconn.Open();
            }
            cmd.CommandText = @"SELECT * FROM " + tabelle + " WHERE " + spalte1 + " = " + teilenummer_FK + " AND Periode = " + periode;
            OleDbDataReader dr = cmd.ExecuteReader();
            int laa = 0;
            while (dr.Read())
            {
                laa = Convert.ToInt32(dr[spalte]);
                return laa;
            }
            dr.Close();
            myconn.Close();
            return laa;
        }

        #region textBoxen
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                textBox1.ForeColor = Color.Red;
            }
            else
            {
                textBox1.ForeColor = Color.Black;
                bool okay = true;
                
                foreach (char c in textBox1.Text.ToCharArray())
                {
                    
                    if (!digits.Contains<char>(c))
                    {
                        textBox1.ForeColor = Color.Red;
                        okay = false;
                        break;
                    }
                }
                if (okay == true)
                {
                    textBox1.ForeColor = Color.Black;;
                }
            }
            check();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (textBox2.Text == "")
            {
                textBox2.ForeColor = Color.Red;
            }
            else
            {
                textBox2.ForeColor = Color.Black;
                bool okay = true;

                foreach (char c in textBox2.Text.ToCharArray())
                {

                    if (!digits.Contains<char>(c))
                    {
                        textBox2.ForeColor = Color.Red;
                        okay = false;
                        break;
                    }
                }
                if (okay == true)
                {
                    textBox2.ForeColor = Color.Black;
                }
            }
            check();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if (textBox3.Text == "")
            {
                textBox3.ForeColor = Color.Red;
            }
            else
            {
                textBox3.ForeColor = Color.Black;
                bool okay = true;

                foreach (char c in textBox3.Text.ToCharArray())
                {

                    if (!digits.Contains<char>(c))
                    {
                        textBox3.ForeColor = Color.Red;
                        okay = false;
                        break;
                    }
                }
                if (okay == true)
                {
                    textBox3.ForeColor = Color.Black;
                }
            }
            check();
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            if (textBox6.Text == "")
            {
                textBox6.ForeColor = Color.Red;
            }
            else
            {
                textBox6.ForeColor = Color.Black;
                bool okay = true;

                foreach (char c in textBox6.Text.ToCharArray())
                {

                    if (!digits.Contains<char>(c))
                    {
                        textBox6.ForeColor = Color.Red;
                        okay = false;
                        break;
                    }
                }
                if (okay == true)
                {
                    textBox6.ForeColor = Color.Black;
                }
            }
            check();
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            if (textBox4.Text == "")
            {
                textBox4.ForeColor = Color.Red;
            }
            else
            {
                textBox4.ForeColor = Color.Black;
                bool okay = true;

                foreach (char c in textBox4.Text.ToCharArray())
                {

                    if (!digits.Contains<char>(c))
                    {
                        textBox4.ForeColor = Color.Red;
                        okay = false;
                        break;
                    }
                }
                if (okay == true)
                {
                    textBox4.ForeColor = Color.Black;
                }
            }
            check();
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            if (textBox5.Text == "")
            {
                textBox5.ForeColor = Color.Red;
            }
            else
            {
                textBox5.ForeColor = Color.Black;
                bool okay = true;

                foreach (char c in textBox5.Text.ToCharArray())
                {

                    if (!digits.Contains<char>(c))
                    {
                        textBox5.ForeColor = Color.Red;
                        okay = false;
                        break;
                    }
                }
                if (okay == true)
                {
                    textBox5.ForeColor = Color.Black;
                }
            }
            check();
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            if (textBox7.Text == "")
            {
                textBox7.ForeColor = Color.Red;
            }
            else
            {
                textBox7.ForeColor = Color.Black;
                bool okay = true;

                foreach (char c in textBox7.Text.ToCharArray())
                {

                    if (!digits.Contains<char>(c))
                    {
                        textBox7.ForeColor = Color.Red;
                        okay = false;
                        break;
                    }
                }
                if (okay == true)
                {
                    textBox7.ForeColor = Color.Black;
                }
            }
            check();
        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            if (textBox8.Text == "")
            {
                textBox8.ForeColor = Color.Red;
            }
            else
            {
                textBox8.ForeColor = Color.Black;
                bool okay = true;

                foreach (char c in textBox8.Text.ToCharArray())
                {

                    if (!digits.Contains<char>(c))
                    {
                        textBox8.ForeColor = Color.Red;
                        okay = false;
                        break;
                    }
                }
                if (okay == true)
                {
                    textBox8.ForeColor = Color.Black;
                }
            }
            check();
        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {
            if (textBox9.Text == "")
            {
                textBox9.ForeColor = Color.Red;
            }
            else
            {
                textBox9.ForeColor = Color.Black;
                bool okay = true;

                foreach (char c in textBox9.Text.ToCharArray())
                {

                    if (!digits.Contains<char>(c))
                    {
                        textBox9.ForeColor = Color.Red;
                        okay = false;
                        break;
                    }
                }
                if (okay == true)
                {
                    textBox9.ForeColor = Color.Black;
                }
            }
            check();
        }

        private void textBox10_TextChanged(object sender, EventArgs e)
        {
            if (textBox10.Text == "")
            {
                textBox10.ForeColor = Color.Red;
            }
            else
            {
                textBox10.ForeColor = Color.Black;
                bool okay = true;

                foreach (char c in textBox10.Text.ToCharArray())
                {

                    if (!digits.Contains<char>(c))
                    {
                        textBox10.ForeColor = Color.Red;
                        okay = false;
                        break;
                    }
                }
                if (okay == true)
                {
                    textBox10.ForeColor = Color.Black;
                }
            }
            check();
        }

        private void textBox11_TextChanged(object sender, EventArgs e)
        {
            if (textBox11.Text == "")
            {
                textBox11.ForeColor = Color.Red;
            }
            else
            {
                textBox11.ForeColor = Color.Black;
                bool okay = true;

                foreach (char c in textBox11.Text.ToCharArray())
                {

                    if (!digits.Contains<char>(c))
                    {
                        textBox11.ForeColor = Color.Red;
                        okay = false;
                        break;
                    }
                }
                if (okay == true)
                {
                    textBox11.ForeColor = Color.Black;
                }
            }
            check();
        }

        private void textBox12_TextChanged(object sender, EventArgs e)
        {
            if (textBox12.Text == "")
            {
                textBox12.ForeColor = Color.Red;
            }
            else
            {
                textBox12.ForeColor = Color.Black;
                bool okay = true;
                
                foreach (char c in textBox12.Text.ToCharArray())
                {
                    
                    if (!digits.Contains<char>(c))
                    {
                        textBox12.ForeColor = Color.Red;
                        okay = false;
                        break;
                    }
                }
                if (okay == true)
                {
                    textBox12.ForeColor = Color.Black;
                }
            }
            check();
        } 

        #endregion

        #region Navigation

        private void continue_btn_Click(object sender, EventArgs e)
        {
            for (int i = 1; i <= 12; ++i)
            {
                if (this.Controls.Find("textBox" + i.ToString(), true)[0].Text == "0")
                {
                    string message;
                    if (pic_de.SizeMode != PictureBoxSizeMode.Normal & sprache != "en")
                    {
                        message = "Sie haben mindestens an einer Stelle angegeben, dass Sie nichts produzieren wollen. Sind Sie sich sicher?";
                    }
                    else
                    {
                        message = "At one point you want to produce anything. Are you sure?";
                    }

                    string caption;
                    if (pic_de.SizeMode != PictureBoxSizeMode.Normal & sprache != "en")
                    {
                        caption = "Sind Sie sich sicher?";
                    }
                    else
                    {
                        caption = "Are you sure?";
                    }
                    MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                    DialogResult result;

                    result = MessageBox.Show(message, caption, buttons);

                    if (result == DialogResult.Yes)
                    {
                        // Datenweitergabe

                        produktion = berProduktion; // alle Produktionsmengen

                        // P1, P2 und P3 nochmal auslesen
                        produktion[0, 1] = Convert.ToInt32(textBox1.Text);
                        produktion[1, 1] = Convert.ToInt32(textBox2.Text);
                        produktion[2, 1] = Convert.ToInt32(textBox3.Text);

                        produktionProg[0, 0] = 1;
                        produktionProg[0, 1] = Convert.ToInt32(textBox1.Text);
                        produktionProg[0, 2] = Convert.ToInt32(textBox6.Text);
                        produktionProg[0, 3] = Convert.ToInt32(textBox7.Text);
                        produktionProg[0, 4] = Convert.ToInt32(textBox10.Text);
                        produktionProg[1, 0] = 2;
                        produktionProg[1, 1] = Convert.ToInt32(textBox2.Text);
                        produktionProg[1, 2] = Convert.ToInt32(textBox4.Text);
                        produktionProg[1, 3] = Convert.ToInt32(textBox8.Text);
                        produktionProg[1, 4] = Convert.ToInt32(textBox11.Text);
                        produktionProg[2, 0] = 3;
                        produktionProg[2, 1] = Convert.ToInt32(textBox3.Text);
                        produktionProg[2, 2] = Convert.ToInt32(textBox5.Text);
                        produktionProg[2, 3] = Convert.ToInt32(textBox9.Text);
                        produktionProg[2, 4] = Convert.ToInt32(textBox12.Text);

                        this.Controls.Clear();
                        UserControl prodreihe = new Produktionsreihenfolge(aktPeriode, auftraege, direktverkaeufe,
                            sicherheitsbest, produktion, produktionProg, prodReihenfolge, kapazitaet, kaufauftraege, sprache);
                        this.Controls.Add(prodreihe);
                        break;
                    }
                    break;
                }
                else
                {
                    if (i == 12)
                    {
                        // Datenweitergabe

                        produktion = berProduktion; // alle Produktionsmengen

                        // P1, P2 und P3 nochmal auslesen
                        produktion[0, 1] = Convert.ToInt32(textBox1.Text);
                        produktion[1, 1] = Convert.ToInt32(textBox2.Text);
                        produktion[2, 1] = Convert.ToInt32(textBox3.Text);

                        produktionProg[0, 0] = 1;
                        produktionProg[0, 1] = Convert.ToInt32(textBox1.Text);
                        produktionProg[0, 2] = Convert.ToInt32(textBox6.Text);
                        produktionProg[0, 3] = Convert.ToInt32(textBox7.Text);
                        produktionProg[0, 4] = Convert.ToInt32(textBox10.Text);
                        produktionProg[1, 0] = 2;
                        produktionProg[1, 1] = Convert.ToInt32(textBox2.Text);
                        produktionProg[1, 2] = Convert.ToInt32(textBox4.Text);
                        produktionProg[1, 3] = Convert.ToInt32(textBox8.Text);
                        produktionProg[1, 4] = Convert.ToInt32(textBox11.Text);
                        produktionProg[2, 0] = 3;
                        produktionProg[2, 1] = Convert.ToInt32(textBox3.Text);
                        produktionProg[2, 2] = Convert.ToInt32(textBox5.Text);
                        produktionProg[2, 3] = Convert.ToInt32(textBox9.Text);
                        produktionProg[2, 4] = Convert.ToInt32(textBox12.Text);

                        this.Controls.Clear();
                        UserControl prodreihe = new Produktionsreihenfolge(aktPeriode, auftraege, direktverkaeufe,
                            sicherheitsbest, produktion, produktionProg, prodReihenfolge, kapazitaet, kaufauftraege, sprache);
                        this.Controls.Add(prodreihe);
                    }
                    else { continue; }
                }
            }

        }

        private void back_Click(object sender, EventArgs e)
        {
            // Datenweitergabe

            produktion = berProduktion; // alle Produktionsmengen

            // P1, P2 und P3 nochmal auslesen
            produktion[0, 1] = Convert.ToInt32(textBox1.Text);
            produktion[1, 1] = Convert.ToInt32(textBox2.Text);
            produktion[2, 1] = Convert.ToInt32(textBox3.Text);

            produktionProg[0, 0] = 1;
            produktionProg[0, 1] = Convert.ToInt32(textBox1.Text);
            produktionProg[0, 2] = Convert.ToInt32(textBox6.Text);
            produktionProg[0, 3] = Convert.ToInt32(textBox7.Text);
            produktionProg[0, 4] = Convert.ToInt32(textBox10.Text);
            produktionProg[1, 0] = 2;
            produktionProg[1, 1] = Convert.ToInt32(textBox2.Text);
            produktionProg[1, 2] = Convert.ToInt32(textBox4.Text);
            produktionProg[1, 3] = Convert.ToInt32(textBox8.Text);
            produktionProg[1, 4] = Convert.ToInt32(textBox11.Text);
            produktionProg[2, 0] = 3;
            produktionProg[2, 1] = Convert.ToInt32(textBox3.Text);
            produktionProg[2, 2] = Convert.ToInt32(textBox5.Text);
            produktionProg[2, 3] = Convert.ToInt32(textBox9.Text);
            produktionProg[2, 4] = Convert.ToInt32(textBox12.Text);

            this.Controls.Clear();
            UserControl sicherheit = new Sicherheitsbestand(aktPeriode, auftraege, direktverkaeufe,
                sicherheitsbest, produktion, produktionProg, prodReihenfolge, kapazitaet, kaufauftraege, sprache);
            this.Controls.Add(sicherheit);
        }

        private void label6_Click(object sender, EventArgs e)
        {
            // Datenweitergabe

            produktion = berProduktion; // alle Produktionsmengen

            // P1, P2 und P3 nochmal auslesen
            produktion[0, 1] = Convert.ToInt32(textBox1.Text);
            produktion[1, 1] = Convert.ToInt32(textBox2.Text);
            produktion[2, 1] = Convert.ToInt32(textBox3.Text);

            produktionProg[0, 0] = 1;
            produktionProg[0, 1] = Convert.ToInt32(textBox1.Text);
            produktionProg[0, 2] = Convert.ToInt32(textBox6.Text);
            produktionProg[0, 3] = Convert.ToInt32(textBox7.Text);
            produktionProg[0, 4] = Convert.ToInt32(textBox10.Text);
            produktionProg[1, 0] = 2;
            produktionProg[1, 1] = Convert.ToInt32(textBox2.Text);
            produktionProg[1, 2] = Convert.ToInt32(textBox4.Text);
            produktionProg[1, 3] = Convert.ToInt32(textBox8.Text);
            produktionProg[1, 4] = Convert.ToInt32(textBox11.Text);
            produktionProg[2, 0] = 3;
            produktionProg[2, 1] = Convert.ToInt32(textBox3.Text);
            produktionProg[2, 2] = Convert.ToInt32(textBox5.Text);
            produktionProg[2, 3] = Convert.ToInt32(textBox9.Text);
            produktionProg[2, 4] = Convert.ToInt32(textBox12.Text);

            this.Controls.Clear();
            UserControl sicherheit = new Sicherheitsbestand(aktPeriode, auftraege, direktverkaeufe,
                sicherheitsbest, produktion, produktionProg, prodReihenfolge, kapazitaet, kaufauftraege, sprache);
            this.Controls.Add(sicherheit);
        }

        private void label5_Click(object sender, EventArgs e)
        {
            // Datenweitergabe

            produktion = berProduktion; // alle Produktionsmengen

            // P1, P2 und P3 nochmal auslesen
            produktion[0, 1] = Convert.ToInt32(textBox1.Text);
            produktion[1, 1] = Convert.ToInt32(textBox2.Text);
            produktion[2, 1] = Convert.ToInt32(textBox3.Text);

            produktionProg[0, 0] = 1;
            produktionProg[0, 1] = Convert.ToInt32(textBox1.Text);
            produktionProg[0, 2] = Convert.ToInt32(textBox6.Text);
            produktionProg[0, 3] = Convert.ToInt32(textBox7.Text);
            produktionProg[0, 4] = Convert.ToInt32(textBox10.Text);
            produktionProg[1, 0] = 2;
            produktionProg[1, 1] = Convert.ToInt32(textBox2.Text);
            produktionProg[1, 2] = Convert.ToInt32(textBox4.Text);
            produktionProg[1, 3] = Convert.ToInt32(textBox8.Text);
            produktionProg[1, 4] = Convert.ToInt32(textBox11.Text);
            produktionProg[2, 0] = 3;
            produktionProg[2, 1] = Convert.ToInt32(textBox3.Text);
            produktionProg[2, 2] = Convert.ToInt32(textBox5.Text);
            produktionProg[2, 3] = Convert.ToInt32(textBox9.Text);
            produktionProg[2, 4] = Convert.ToInt32(textBox12.Text);

            this.Controls.Clear();
            UserControl import = new ImportPrognose(aktPeriode, auftraege, direktverkaeufe,
                sicherheitsbest, produktion, produktionProg, prodReihenfolge, kapazitaet, kaufauftraege, sprache);
            this.Controls.Add(import);
        } 
        #endregion

        private void ETeile_Click(object sender, EventArgs e)
        {
            backupProduktion = berProduktion;
            Produktion_ETeile eteile = new Produktion_ETeile(berProduktion, sicherheitsbest, sprache);
            eteile.Show();
        }

        public void vonProduktionEteile(int[,] beProd)
        {
            this.berProduktion = beProd;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            berechneProduktion();
        }

        public void sprachen()
        {
            if (pic_en.SizeMode == PictureBoxSizeMode.StretchImage | sprache == "en")
            {
                //EN Brotkrumenleiste
                lbl_Startseite.Text = (Sprachen.EN_LBL_STARTSEITE);
                lbl_Sicherheitsbestand.Text = (Sprachen.EN_LBL_SICHERHEITSBESTAND);
                lbl_Produktion.Text = (Sprachen.EN_LBL_PRODUKTION);
                lbl_Produktionsreihenfolge.Text = (Sprachen.EN_LBL_PRODUKTIONSREIHENFOLGE);
                lbl_Kapazitaetsplan.Text = (Sprachen.EN_LBL_KAPATITAETSPLAN);
                lbl_Kaufteiledisposition.Text = (Sprachen.EN_LBL_KAUFTEILEDISPOSITION);
                lbl_Ergebnis.Text = (Sprachen.EN_LBL_ERGEBNIS);

                //EN Buttons
                continue_btn.Text = (Sprachen.EN_BTN_CONTINUE);
                button1.Text = (Sprachen.EN_BTN_DEFAULT);
                back.Text = (Sprachen.EN_BTN_BACK);
                ETeile.Text = (Sprachen.EN_BTN_ETEILE);


                //EN Groupboxen
                groupBox1.Text = (Sprachen.EN_PR_GROUPBOX1);

                //DE Labels
                aktuellePer.Text = (Sprachen.EN_LBL_IP_AKTUELLE_PERIODE);
                PeriodeX.Text = (Sprachen.EN_LBL_IP_PERIODEX);
                PeriodeX1.Text = (Sprachen.EN_LBL_IP_PERIODEX1);
                PeriodeX2.Text = (Sprachen.EN_LBL_IP_PERIODEX2);

                //DE Tooltip
                System.Windows.Forms.ToolTip ToolTipP = new System.Windows.Forms.ToolTip();
                ToolTipP.SetToolTip(this.pictureBox7, Sprachen.EN_PR_INFO);

            }
            else
            {
                //DE Brotkrumenleiste
                lbl_Sicherheitsbestand.Text = (Sprachen.DE_LBL_SICHERHEITSBESTAND);
                lbl_Startseite.Text = (Sprachen.DE_LBL_STARTSEITE);
                lbl_Produktionsreihenfolge.Text = (Sprachen.DE_LBL_PRODUKTIONSREIHENFOLGE);
                lbl_Produktion.Text = (Sprachen.DE_LBL_PRODUKTION);
                lbl_Kapazitaetsplan.Text = (Sprachen.DE_LBL_KAPATITAETSPLAN);
                lbl_Kaufteiledisposition.Text = (Sprachen.DE_LBL_KAUFTEILEDISPOSITION);
                lbl_Ergebnis.Text = (Sprachen.DE_LBL_ERGEBNIS);

                //DE Buttons
                continue_btn.Text = (Sprachen.DE_BTN_CONTINUE);
                button1.Text = (Sprachen.DE_BTN_DEFAULT);
                back.Text = (Sprachen.DE_BTN_BACK);
                ETeile.Text = (Sprachen.DE_BTN_ETEILE);


                //DE Groupboxen
                groupBox1.Text = (Sprachen.DE_PR_GROUPBOX1);

                //DE Labels
                aktuellePer.Text = (Sprachen.DE_LBL_IP_AKTUELLE_PERIODE);
                PeriodeX.Text = (Sprachen.DE_LBL_IP_PERIODEX);
                PeriodeX1.Text = (Sprachen.DE_LBL_IP_PERIODEX1);
                PeriodeX2.Text = (Sprachen.DE_LBL_IP_PERIODEX2);

                //DE Tooltip
                System.Windows.Forms.ToolTip ToolTipP = new System.Windows.Forms.ToolTip();
                ToolTipP.SetToolTip(this.pictureBox7, Sprachen.DE_PR_INFO);
            }
        }

        private void pic_en_Click(object sender, EventArgs e)
        {
            pic_en.SizeMode = PictureBoxSizeMode.StretchImage;
            pic_de.SizeMode = PictureBoxSizeMode.Normal;
            sprachen();
            sprache = "en";
        }

        private void pic_de_Click(object sender, EventArgs e)
        {
            pic_de.SizeMode = PictureBoxSizeMode.StretchImage;
            pic_en.SizeMode = PictureBoxSizeMode.Normal;
            sprachen();
            sprache = "de";
        }

        private void lbl_Produktionsreihenfolge_Click(object sender, EventArgs e)
        {
            if (continue_btn.Enabled == true)
            {
                // Datenweitergabe

                produktion = berProduktion; // alle Produktionsmengen

                // P1, P2 und P3 nochmal auslesen
                produktion[0, 1] = Convert.ToInt32(textBox1.Text);
                produktion[1, 1] = Convert.ToInt32(textBox2.Text);
                produktion[2, 1] = Convert.ToInt32(textBox3.Text);

                produktionProg[0, 0] = 1;
                produktionProg[0, 1] = Convert.ToInt32(textBox1.Text);
                produktionProg[0, 2] = Convert.ToInt32(textBox6.Text);
                produktionProg[0, 3] = Convert.ToInt32(textBox7.Text);
                produktionProg[0, 4] = Convert.ToInt32(textBox10.Text);
                produktionProg[1, 0] = 2;
                produktionProg[1, 1] = Convert.ToInt32(textBox2.Text);
                produktionProg[1, 2] = Convert.ToInt32(textBox4.Text);
                produktionProg[1, 3] = Convert.ToInt32(textBox8.Text);
                produktionProg[1, 4] = Convert.ToInt32(textBox11.Text);
                produktionProg[2, 0] = 3;
                produktionProg[2, 1] = Convert.ToInt32(textBox3.Text);
                produktionProg[2, 2] = Convert.ToInt32(textBox5.Text);
                produktionProg[2, 3] = Convert.ToInt32(textBox9.Text);
                produktionProg[2, 4] = Convert.ToInt32(textBox12.Text);

                this.Controls.Clear();
                UserControl prodreihe = new Produktionsreihenfolge(aktPeriode, auftraege, direktverkaeufe,
                    sicherheitsbest, produktion, produktionProg, prodReihenfolge, kapazitaet, kaufauftraege, sprache);
                this.Controls.Add(prodreihe);
            }
        }
    }

}