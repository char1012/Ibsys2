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

        // Datenweitergabe:
        int aktPeriode;
        int[] auftraege = new int[12];
        int[] direktverkaeufe = new int[3];
        int[,] sicherheitsbest = new int[30, 2];
        int[,] produktion = new int[30, 2];
        int[,] produktionProg = new int[3, 5];
        int[,] prodReihenfolge = new int[30, 2];
        int[,] kapazitaet = new int[14, 5];
        int[,] kaufauftraege = new int[29, 6];
        
        int periode;

        List<int> sicherheitsbe = new List<int>();

        List<int> lagerbestand = new List<int>();
        List<int> bearbeitung = new List<int>();
        List<int> wartelisteAr = new List<int>();
        List<int> wartelisteMa = new List<int>();

        // Array fuer berechnete Produktionsmengen
        int[,] berProduktion = new int[30, 2];

        public Produktion()
        {
           // var UserControl kapa= new Kapazitaetsplan();
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
            if (pic_de.SizeMode != PictureBoxSizeMode.Normal)
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
        }

        public Produktion(int aktPeriode, int[] auftraege, int[] direktverkaeufe, int[,] sicherheitsbest,
            int[,] produktion, int[,] produktionProg, int[,] prodReihenfolge, int[,] kapazitaet, int[,] kaufauftraege)
        {
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

            string databasename = @"Provider=Microsoft.ACE.OLEDB.12.0; Data Source=IBSYS_DB.accdb";
            myconn = new OleDbConnection(databasename);

            for (int i = 3; i < sicherheitsbest.GetLength(0); i++) // bei 3 anfangen, weil dort die E-Teile anfangen
            {
                sicherheitsbe.Add(sicherheitsbest[i, 1]);
            }

            System.Windows.Forms.ToolTip ToolTipDE = new System.Windows.Forms.ToolTip();
            System.Windows.Forms.ToolTip ToolTipEN = new System.Windows.Forms.ToolTip();
            if (pic_de.SizeMode != PictureBoxSizeMode.Normal)
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

            berechneProduktion();
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
            double p1 = auftraege[0] + direktverkaeufe[0];
            double p2 = auftraege[1] + direktverkaeufe[1];
            double p3 = auftraege[2] + direktverkaeufe[2];

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

            // TODO alle Produktionsmengen berechnen und berProduktion befuellen

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
                    string message = "Sie haben mindestens an einer Stelle angegeben, dass Sie nichts produzieren wollen. Sind Sie sich sicher?";
                    string caption = "Sind Sie sich sicher?";
                    MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                    DialogResult result;

                    result = MessageBox.Show(message, caption, buttons);

                    if (result == DialogResult.Yes)
                    {
                        // Datenweitergabe

                        produktion = berProduktion; // alle Produktionsmengen
                        // wegen fehlender E-Teile, simulieren:
                        // dieser Teil kommt also spaeter weg
                        produktion[0, 0] = 1;
                        produktion[0, 1] = 90; // Teil p1 mit 90 Stueck Produktion
                        produktion[1, 0] = 2;
                        produktion[1, 1] = 190;
                        produktion[2, 0] = 3;
                        produktion[2, 1] = 160;
                        produktion[3, 0] = 4;
                        produktion[3, 1] = 60;
                        produktion[4, 0] = 5;
                        produktion[4, 1] = 160;
                        produktion[5, 0] = 6;
                        produktion[5, 1] = 0;
                        produktion[6, 0] = 7;
                        produktion[6, 1] = 50;
                        produktion[7, 0] = 8;
                        produktion[7, 1] = 150;
                        produktion[8, 0] = 9;
                        produktion[8, 1] = 0;
                        produktion[9, 0] = 10;
                        produktion[9, 1] = 60;
                        produktion[10, 0] = 11;
                        produktion[10, 1] = 160;
                        produktion[11, 0] = 12;
                        produktion[11, 1] = 0;
                        produktion[12, 0] = 13;
                        produktion[12, 1] = 50;
                        produktion[13, 0] = 14;
                        produktion[13, 1] = 150;
                        produktion[14, 0] = 15;
                        produktion[14, 1] = 0;
                        produktion[15, 0] = 16;
                        produktion[15, 1] = 20 + 130 + 90;
                        produktion[16, 0] = 17;
                        produktion[16, 1] = 20 + 130 + 90;
                        produktion[17, 0] = 18;
                        produktion[17, 1] = 50;
                        produktion[18, 0] = 19;
                        produktion[18, 1] = 150;
                        produktion[19, 0] = 20;
                        produktion[19, 1] = 0;
                        produktion[20, 0] = 26;
                        produktion[20, 1] = 50 + 160 + 130;
                        produktion[21, 0] = 29;
                        produktion[21, 1] = 0;
                        produktion[22, 0] = 30;
                        produktion[22, 1] = 0;
                        produktion[23, 0] = 31;
                        produktion[23, 1] = 70;
                        produktion[24, 0] = 49;
                        produktion[24, 1] = 60;
                        produktion[25, 0] = 50;
                        produktion[25, 1] = 70;
                        produktion[26, 0] = 51;
                        produktion[26, 1] = 80;
                        produktion[27, 0] = 54;
                        produktion[27, 1] = 160;
                        produktion[28, 0] = 55;
                        produktion[28, 1] = 170;
                        produktion[29, 0] = 56;
                        produktion[29, 1] = 180;

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
                            sicherheitsbest, produktion, produktionProg, prodReihenfolge, kapazitaet, kaufauftraege);
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
                        // wegen fehlender E-Teile, simulieren:
                        // dieser Teil kommt also spaeter weg
                        produktion[0, 0] = 1;
                        produktion[0, 1] = 90; // Teil p1 mit 90 Stueck Produktion
                        produktion[1, 0] = 2;
                        produktion[1, 1] = 190;
                        produktion[2, 0] = 3;
                        produktion[2, 1] = 160;
                        produktion[3, 0] = 4;
                        produktion[3, 1] = 60;
                        produktion[4, 0] = 5;
                        produktion[4, 1] = 160;
                        produktion[5, 0] = 6;
                        produktion[5, 1] = 0;
                        produktion[6, 0] = 7;
                        produktion[6, 1] = 50;
                        produktion[7, 0] = 8;
                        produktion[7, 1] = 150;
                        produktion[8, 0] = 9;
                        produktion[8, 1] = 0;
                        produktion[9, 0] = 10;
                        produktion[9, 1] = 60;
                        produktion[10, 0] = 11;
                        produktion[10, 1] = 160;
                        produktion[11, 0] = 12;
                        produktion[11, 1] = 0;
                        produktion[12, 0] = 13;
                        produktion[12, 1] = 50;
                        produktion[13, 0] = 14;
                        produktion[13, 1] = 150;
                        produktion[14, 0] = 15;
                        produktion[14, 1] = 0;
                        produktion[15, 0] = 16;
                        produktion[15, 1] = 20 + 130 + 90;
                        produktion[16, 0] = 17;
                        produktion[16, 1] = 20 + 130 + 90;
                        produktion[17, 0] = 18;
                        produktion[17, 1] = 50;
                        produktion[18, 0] = 19;
                        produktion[18, 1] = 150;
                        produktion[19, 0] = 20;
                        produktion[19, 1] = 0;
                        produktion[20, 0] = 26;
                        produktion[20, 1] = 50 + 160 + 130;
                        produktion[21, 0] = 29;
                        produktion[21, 1] = 0;
                        produktion[22, 0] = 30;
                        produktion[22, 1] = 0;
                        produktion[23, 0] = 31;
                        produktion[23, 1] = 70;
                        produktion[24, 0] = 49;
                        produktion[24, 1] = 60;
                        produktion[25, 0] = 50;
                        produktion[25, 1] = 70;
                        produktion[26, 0] = 51;
                        produktion[26, 1] = 80;
                        produktion[27, 0] = 54;
                        produktion[27, 1] = 160;
                        produktion[28, 0] = 55;
                        produktion[28, 1] = 170;
                        produktion[29, 0] = 56;
                        produktion[29, 1] = 180;

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
                            sicherheitsbest, produktion, produktionProg, prodReihenfolge, kapazitaet, kaufauftraege);
                        this.Controls.Add(prodreihe);
                    }
                    else { continue; }
                }
            }

        }

        private void back_Click(object sender, EventArgs e)
        {
            this.Controls.Clear();
            UserControl sicherheit = new Sicherheitsbestand();
            this.Controls.Add(sicherheit);
        }

        private void label6_Click(object sender, EventArgs e)
        {
            this.Controls.Clear();
            UserControl sicherheit = new Sicherheitsbestand();
            this.Controls.Add(sicherheit);
        }

        private void label5_Click(object sender, EventArgs e)
        {
            this.Controls.Clear();
            UserControl import = new ImportPrognose();
            this.Controls.Add(import);
        } 
        #endregion

        private void ETeile_Click(object sender, EventArgs e)
        {
            // TODO hier zusaetzlich berProduktion uebergeben
            new Produktion_ETeile(periode, textBox1.Text, textBox2.Text, textBox3.Text, sicherheitsbe).Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            berechneProduktion();
        }

        public void sprachen()
        {
            if (pic_en.SizeMode == PictureBoxSizeMode.StretchImage)
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
        }

        private void pic_de_Click(object sender, EventArgs e)
        {
            pic_de.SizeMode = PictureBoxSizeMode.StretchImage;
            pic_en.SizeMode = PictureBoxSizeMode.Normal;
            sprachen(); 
        }

        private void lbl_Produktionsreihenfolge_Click(object sender, EventArgs e)
        {
            if (continue_btn.Enabled == true)
            {
                // Datenweitergabe

                produktion = berProduktion; // alle Produktionsmengen
                // wegen fehlender E-Teile, simulieren:
                // dieser Teil kommt also spaeter weg
                produktion[0, 0] = 1;
                produktion[0, 1] = 90; // Teil p1 mit 90 Stueck Produktion
                produktion[1, 0] = 2;
                produktion[1, 1] = 190;
                produktion[2, 0] = 3;
                produktion[2, 1] = 160;
                produktion[3, 0] = 4;
                produktion[3, 1] = 60;
                produktion[4, 0] = 5;
                produktion[4, 1] = 160;
                produktion[5, 0] = 6;
                produktion[5, 1] = 0;
                produktion[6, 0] = 7;
                produktion[6, 1] = 50;
                produktion[7, 0] = 8;
                produktion[7, 1] = 150;
                produktion[8, 0] = 9;
                produktion[8, 1] = 0;
                produktion[9, 0] = 10;
                produktion[9, 1] = 60;
                produktion[10, 0] = 11;
                produktion[10, 1] = 160;
                produktion[11, 0] = 12;
                produktion[11, 1] = 0;
                produktion[12, 0] = 13;
                produktion[12, 1] = 50;
                produktion[13, 0] = 14;
                produktion[13, 1] = 150;
                produktion[14, 0] = 15;
                produktion[14, 1] = 0;
                produktion[15, 0] = 16;
                produktion[15, 1] = 20 + 130 + 90;
                produktion[16, 0] = 17;
                produktion[16, 1] = 20 + 130 + 90;
                produktion[17, 0] = 18;
                produktion[17, 1] = 50;
                produktion[18, 0] = 19;
                produktion[18, 1] = 150;
                produktion[19, 0] = 20;
                produktion[19, 1] = 0;
                produktion[20, 0] = 26;
                produktion[20, 1] = 50 + 160 + 130;
                produktion[21, 0] = 29;
                produktion[21, 1] = 0;
                produktion[22, 0] = 30;
                produktion[22, 1] = 0;
                produktion[23, 0] = 31;
                produktion[23, 1] = 70;
                produktion[24, 0] = 49;
                produktion[24, 1] = 60;
                produktion[25, 0] = 50;
                produktion[25, 1] = 70;
                produktion[26, 0] = 51;
                produktion[26, 1] = 80;
                produktion[27, 0] = 54;
                produktion[27, 1] = 160;
                produktion[28, 0] = 55;
                produktion[28, 1] = 170;
                produktion[29, 0] = 56;
                produktion[29, 1] = 180;

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
                    sicherheitsbest, produktion, produktionProg, prodReihenfolge, kapazitaet, kaufauftraege);
                this.Controls.Add(prodreihe);
            }
        }
    }

}