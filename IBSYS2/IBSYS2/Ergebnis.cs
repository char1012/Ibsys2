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
    public partial class Ergebnis : UserControl
    {
        private OleDbConnection myconn;
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

        // lokale Information
        int periode;
        int[] storevalues;

        public Ergebnis()
        {
            InitializeComponent();
            result();
        }

        public Ergebnis(int aktPeriode, int[] auftraege, double[,] direktverkaeufe, int[,] sicherheitsbest,
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

            InitializeComponent();
            sprachen();

            // Mitteilung einblenden
            ProcessMessage message = new ProcessMessage(sprache);
            message.Show(this);
            message.Location = new Point(500, 300);
            message.Update();
            this.Enabled = false;

            result();

            // Einkaufsauftraege
            tableLayoutPanel1.Controls.Clear();
            tableLayoutPanel1.RowStyles.Clear();
            tableLayoutPanel1.RowCount = kaufauftraege.GetLength(0);
            tableLayoutPanel1.AutoScroll = true;
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < kaufauftraege.GetLength(0); y++)
                {
                    Label label = new Label();

                    if (x == 0)
                    {
                        label.Text = kaufauftraege[y, 0].ToString();
                    }
                    else if (x == 1)
                    {
                        label.Text = kaufauftraege[y, 4].ToString();
                    }
                    else
                    {
                        int bestellart = kaufauftraege[y, 5];
                        if (bestellart == 4)
                        {
                            label.Text = "E";
                        }
                        else if (bestellart == 5)
                        {
                            label.Text = "N";
                        }
                        else
                        {
                            label.Text = "";
                        }
                    }

                    tableLayoutPanel1.Controls.Add(label, x, y);
                }
            }
            
            // Produktionsauftraege
            tableLayoutPanel2.Controls.Clear();
            tableLayoutPanel2.RowStyles.Clear();
            tableLayoutPanel2.RowCount = prodReihenfolge.GetLength(0);
            tableLayoutPanel2.AutoScroll = true;
            for (int x = 0; x < 2; x++)
            {
                for (int y = 0; y < prodReihenfolge.GetLength(0); y++)
                {
                    Label label = new Label();

                    if (x == 0)
                    {
                        label.Text = prodReihenfolge[y, 0].ToString();
                    }
                    else if (x == 1)
                    {
                        label.Text = prodReihenfolge[y, 1].ToString();
                    }

                    tableLayoutPanel2.Controls.Add(label, x, y);
                }
            }

            // Kapazitaet
            tableLayoutPanel3.Controls.Clear();
            tableLayoutPanel3.RowStyles.Clear();
            tableLayoutPanel3.RowCount = kapazitaet.GetLength(0);
            tableLayoutPanel3.AutoScroll = true;
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < kapazitaet.GetLength(0); y++)
                {
                    Label label = new Label();

                    if (x == 0)
                    {
                        label.Text = kapazitaet[y, 0].ToString();
                    }
                    else if (x == 1)
                    {
                        label.Text = kapazitaet[y, 4].ToString();
                    }
                    else
                    {
                        label.Text = kapazitaet[y, 3].ToString();
                    }

                    tableLayoutPanel3.Controls.Add(label, x, y);
                }
            }

            message.Close();
            this.Enabled = true;
        }

        public void result()
        {
            periode = aktPeriode - 1;

            storevalues = calculateStorevalue(periode, auftraege, direktverkaeufe, produktion);

            if (storevalues[1] >= 250000)
            {
                textBox2.ForeColor = Color.Red;
            }

            // Strings formatieren
            String s1 = storevalues[0].ToString();
            int count1 = s1.Length;
            if (count1 > 3)
            {
                String neu1 = "";
                if (count1 == 6)
                {
                    neu1 += s1.Substring(0, 3);
                    neu1 += ".";
                    neu1 += s1.Substring(3, 3);
                }
                else if (count1 == 5)
                {
                    neu1 += s1.Substring(0, 2);
                    neu1 += ".";
                    neu1 += s1.Substring(3, 3);
                }
                else if (count1 == 4)
                {
                    neu1 += s1.Substring(0, 1);
                    neu1 += ".";
                    neu1 += s1.Substring(3, 3);
                }
                textBox1.Text = neu1;
            }

            String s2 = storevalues[1].ToString();
            int count2 = s2.Length;
            if (count1 > 3)
            {
                String neu2 = "";
                if (count2 == 6)
                {
                    neu2 += s2.Substring(0, 3);
                    neu2 += ".";
                    neu2 += s2.Substring(3, 3);
                }
                else if (count2 == 5)
                {
                    neu2 += s2.Substring(0, 2);
                    neu2 += ".";
                    neu2 += s2.Substring(3, 3);
                }
                else if (count2 == 4)
                {
                    neu2 += s2.Substring(0, 1);
                    neu2 += ".";
                    neu2 += s2.Substring(3, 3);
                }
                textBox1.Text = neu2;
            }

            String s3 = storevalues[2].ToString();
            int count3 = s3.Length;
            if (count3 > 3)
            {
                String neu3 = "";
                if (count3 == 6)
                {
                    neu3 += s3.Substring(0, 3);
                    neu3 += ".";
                    neu3 += s3.Substring(3, 3);
                }
                else if (count3 == 5)
                {
                    neu3 += s3.Substring(0, 2);
                    neu3 += ".";
                    neu3 += s3.Substring(3, 3);
                }
                else if (count3 == 4)
                {
                    neu3 += s3.Substring(0, 1);
                    neu3 += ".";
                    neu3 += s3.Substring(3, 3);
                }
                textBox1.Text = neu3;
            }
        }

        private int[] calculateStorevalue(int periode, int[] auftraege, double[,] direktverkaeufe, int[,] produktion)
        {
            // Array fuer Anfangslagerwert, Endlagerwert und Mittelwert
            int[] storevalue = new int[3];
            // Array fuer Teilewerte
            double[,] teilewerte = new double[59,2];
            
            // DB-Verbindung
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
                myconn.Close();
                myconn.Open();
            }

            // a) Anfangslagerwert aus der DB lesen
            cmd.CommandText = @"SELECT Aktueller_Lagerbestand FROM Informationen WHERE Periode = " + periode + ";";
            OleDbDataReader dbReader = cmd.ExecuteReader();
            while (dbReader.Read()) // hier sollte nur eine Zeile herauskommen
            {
                storevalue[0] = Convert.ToInt32(dbReader["Aktueller_Lagerbestand"]);
            }
            dbReader.Close();

            // b) geschaetzter Endlagerwert berechnen

            // Gesamtwert und einzelne Tageswerte berechnen (fuer Mittelwert-Berechnung)
            int endwert = storevalue[0];
            int[] tageswerte = new int[5]{0,0,0,0,0};

            // Teilewert ermitteln
            if (aktPeriode > 1)
            {
                cmd.CommandText = @"SELECT Teilenummer_FK, Teilewert FROM Lager WHERE Periode = " + periode + ";";
                dbReader = cmd.ExecuteReader();
                int n = 0;
                while (dbReader.Read())
                {
                    teilewerte[n, 0] = Convert.ToDouble(dbReader["Teilenummer_FK"]);
                    teilewerte[n, 1] = Convert.ToDouble(dbReader["Teilewert"]);
                    n++;
                }
                dbReader.Close();
            }
            else
            {
                cmd.CommandText = @"SELECT Teilenummer, Startbestand FROM Teil;";
                dbReader = cmd.ExecuteReader();
                int n = 0;
                while (dbReader.Read())
                {
                    teilewerte[n, 0] = Convert.ToDouble(dbReader["Teilenummer"]);
                    teilewerte[n, 1] = Convert.ToDouble(dbReader["Startbestand"]);
                    n++;
                }
                dbReader.Close();
            }

            // 1. eingehende Bestellungen dazurechnen
            // weil sowohl im xml als auch in der Bestellliste keine Ankunftsdaten vorhanden sind,
            // gehe ich davon aus, dass alle ausstehenden Bestellungen in dieser Periode ankommen und
            // die neuen Bestellungen nicht in dieser Periode ankommen
            double wertBestellungen = 0;
            List<List<int>> bestellungen = new List<List<int>>();
            cmd.CommandText = @"SELECT Teilenummer_FK, Menge FROM Bestellung WHERE Periode = " + periode
                    + " AND Eingegangen = False;";
            dbReader = cmd.ExecuteReader();
            n = 0;
            while (dbReader.Read())
            {
                bestellungen.Add(new List<int>());
                bestellungen[n].Add(Convert.ToInt32(dbReader["Teilenummer_FK"]));
                bestellungen[n].Add(Convert.ToInt32(dbReader["Menge"]));
                n++;
            }
            dbReader.Close();
            for (int i = 0; i < bestellungen.Count; i++)
            {
                for (int no = 0; no < teilewerte.GetLength(0); no++)
                {
                    if (teilewerte[no, 0] == bestellungen[i][0])
                    {
                        wertBestellungen += (bestellungen[i][1] * teilewerte[no, 1]);
                    }
                }
            }

            // 2. fertiggestellte Erzeugnisse dazurechnen (auf Basis der Planproduktion)
            // der Einfachheit halber: gesamter Produktionswert berechnen und dann gleichmaessig auf 5 Tage verteilen
            double wertProduktion = 0;
            for (int i = 0; i < produktion.GetLength(0); i++)
            {
                for (int no = 0; no < teilewerte.GetLength(0); no++)
                {
                    if (teilewerte[no, 0] == produktion[i, 0])
                    {
                        wertProduktion += (produktion[i, 1] * teilewerte[no, 1]);
                    }
                }
            }
            int[,] produktion2 = new int[30, 2];
            cmd.CommandText = @"SELECT Teilenummer_FK, Menge FROM Warteliste_Arbeitsplatz WHERE Periode = " + periode + ";";
            dbReader = cmd.ExecuteReader();
            n = 0;
            while (dbReader.Read())
            {
                for (int no = 0; no < teilewerte.GetLength(0); no++)
                {
                    if (teilewerte[no, 0] == Convert.ToInt32(dbReader["Teilenummer_FK"]))
                    {
                        wertProduktion += (Convert.ToInt32(dbReader["Menge"]) * teilewerte[no, 1]);
                        produktion2[n, 0] = Convert.ToInt32(dbReader["Teilenummer_FK"]);
                        produktion2[n, 1] = Convert.ToInt32(dbReader["Menge"]);
                        n++;
                    }
                }
            }
            dbReader.Close();
            cmd.CommandText = @"SELECT Erzeugnis_Teilenummer_FK, Menge FROM Warteliste_Material WHERE Periode = " + periode + ";";
            dbReader = cmd.ExecuteReader();
            while (dbReader.Read())
            {
                for (int no = 0; no < teilewerte.GetLength(0); no++)
                {
                    if (teilewerte[no, 0] == Convert.ToInt32(dbReader["Erzeugnis_Teilenummer_FK"]))
                    {
                        wertProduktion += (Convert.ToInt32(dbReader["Menge"]) * teilewerte[no, 1]);
                        produktion2[n, 0] = Convert.ToInt32(dbReader["Erzeugnis_Teilenummer_FK"]);
                        produktion2[n, 1] = Convert.ToInt32(dbReader["Menge"]);
                        n++;
                    }
                }
            }
            dbReader.Close();
            cmd.CommandText = @"SELECT Teilenummer_FK, Menge FROM Bearbeitung WHERE Periode = " + periode + ";";
            dbReader = cmd.ExecuteReader();
            while (dbReader.Read())
            {
                for (int no = 0; no < teilewerte.GetLength(0); no++)
                {
                    if (teilewerte[no, 0] == Convert.ToInt32(dbReader["Teilenummer_FK"]))
                    {
                        wertProduktion += (Convert.ToInt32(dbReader["Menge"]) * teilewerte[no, 1]);
                        produktion2[n, 0] = Convert.ToInt32(dbReader["Teilenummer_FK"]);
                        produktion2[n, 1] = Convert.ToInt32(dbReader["Menge"]);
                        n++;
                    }
                }
            }
            dbReader.Close();

            // 3. verkaufte Endprodukte abziehen (unter Annahme, dass Verkauf planmaessig stattfindet)
            // 1/5 der Endprodukte gehen pro Tag ab
            double wertVerkaeufe = 0;
            for(int i = 0; i < 3; i++) // nur die ersten drei in auftraege
            {
                for (int no = 0; no < teilewerte.GetLength(0); no++)
                {
                    if (teilewerte[no,0] == (i+1))
                    {
                        wertVerkaeufe += (auftraege[i] * teilewerte[no,1]); // Menge mit Wert multiplizieren und dazurechnen
                        wertVerkaeufe += (direktverkaeufe[i, 1] * teilewerte[no, 1]);
                    }
                }
            }

            // 4. verwendete E- und K-Teile abziehen (auf Basis der Planproduktion)
            double wertVerwendung = 0;
            int prod1 = 0;
            int prod2 = 0;
            int prod3 = 0;
            for (int no = 0; no < produktion.GetLength(0); no++)
            {
                if (produktion[no, 0] == 1)
                    prod1 = produktion[no, 1];
                else if (produktion[no, 0] == 2)
                    prod2 = produktion[no, 1];
                else if (produktion[no, 0] == 3)
                    prod3 = produktion[no, 1];
            }
            // K-Teile
            cmd.CommandText = @"SELECT K_Teil, P1, P2, P3 FROM Verwendung;";
            dbReader = cmd.ExecuteReader();
            while (dbReader.Read())
            {
                for (int no = 0; no < teilewerte.GetLength(0); no++)
                {
                    if (teilewerte[no,0] == Convert.ToInt32(dbReader["K_Teil"]))
                    {
                        int menge = (Convert.ToInt32(dbReader["P1"]) * prod1)
                            + (Convert.ToInt32(dbReader["P2"]) * prod2) 
                            + (Convert.ToInt32(dbReader["P3"]) * prod3);
                        wertVerwendung += (menge * teilewerte[no, 1]);
                    }
                }
            }
            dbReader.Close();
            // E-Teile
            cmd.CommandText = @"SELECT E_Teil_FK, Produziert_FK FROM VerwendungETeile;";
            dbReader = cmd.ExecuteReader();
            while (dbReader.Read())
            {
                for (int no = 0; no < teilewerte.GetLength(0); no++)
                {
                    if (teilewerte[no, 0] == Convert.ToInt32(dbReader["E_Teil_FK"]))
                    {
                        int menge = 0;
                        // herausfinden, wieviel vom aktuellen Teil verwendet wird
                        // dafuer die Produktionsmenge fuer Produziert_FK herausfinden
                        for (int i = 0; i < produktion.GetLength(0); i++)
                        {
                            if (produktion[i, 0] == Convert.ToInt32(dbReader["Produziert_FK"]))
                            {
                                menge += produktion[i, 1]; // jedes Teil fliesst genau einmal ein
                            }
                        }
                        // Produktion der Warteliste_Arbeitsplatz, Warteliste_Material, Bearbeitung
                        for (int i = 0; i < produktion2.GetLength(0); i++)
                        {
                            if (produktion2[i, 0] == Convert.ToInt32(dbReader["Produziert_FK"]))
                            {
                                menge += produktion2[i, 1]; // jedes Teil fliesst genau einmal ein
                            }
                        }
                        wertVerwendung += (menge * teilewerte[no, 1]);
                    }
                }
            }
            dbReader.Close();

            // 5. Zusammenrechnen
            // tageswerte berechnen
            for (int i = 0; i < tageswerte.Length; i++)
            {
                tageswerte[i] = Convert.ToInt32(tageswerte[i] + (wertBestellungen / 5) + (wertProduktion / 5) 
                    - (wertVerkaeufe / 5) - (wertVerwendung / 5));
            }
            // tageswerte berichtigen (bis jetzt nur der Zugang pro Tag, ab jetzt der totale Wert)
            tageswerte[0] += storevalue[0];
            for (int i = 1; i < tageswerte.Length; i++)
            {
                tageswerte[i] += tageswerte[i-1];
            }
            // endgueltigen Wert festhalten
            endwert = Convert.ToInt32(endwert + wertBestellungen + wertProduktion - wertVerkaeufe - wertVerwendung);
            storevalue[1] = endwert;

            // c) geschatzter Mittelwert berechnen - wichtig, weil sprungfixe Kosten aus Basis des Mittelwertes berechnet werden
            storevalue[2] = tageswerte.Sum() / 5;  

            return storevalue;
        }

        public void sprachen()
        {
            if (pic_en.SizeMode == PictureBoxSizeMode.StretchImage | sprache != "de")
            {
                //EN Brotkrumenleiste
                lbl_Startseite.Text = (Sprachen.EN_LBL_STARTSEITE);
                lbl_Sicherheitsbestand.Text = (Sprachen.EN_LBL_SICHERHEITSBESTAND);
                lbl_Produktionsreihenfolge.Text = (Sprachen.EN_LBL_PRODUKTIONSREIHENFOLGE);
                lbl_Produktion.Text = (Sprachen.EN_LBL_PRODUKTION);
                lbl_Kapazitaetsplan.Text = (Sprachen.EN_LBL_KAPATITAETSPLAN);
                lbl_Kaufteiledisposition.Text = (Sprachen.EN_LBL_KAUFTEILEDISPOSITION);
                lbl_Ergebnis.Text = (Sprachen.EN_LBL_ERGEBNIS);

                //EN Buttons
                End_btn.Text = (Sprachen.EN_BTN_XML_EXPORT);
                back_btn.Text = (Sprachen.DE_BTN_BACK);

                //EN Groupboxen
                groupBox2.Text = (Sprachen.EN_ER_GROUPBOX2);
                groupBox3.Text = (Sprachen.EN_ER_GROUPBOX3);
                groupBox4.Text = (Sprachen.EN_ER_GROUPBOX4);
                Lagerwerte.Text = (Sprachen.EN_ER_LAGERWERT);

                //EN Label
                label1.Text = (Sprachen.EN_ER_TEIL);
                label3.Text = (Sprachen.EN_ER_MENGE);
                label5.Text = (Sprachen.EN_ER_BESTART);
                label2.Text = (Sprachen.EN_ER_TEIL);
                label4.Text = (Sprachen.EN_ER_MENGE);
                label6.Text = (Sprachen.EN_ER_ARBEITSPLATZ);
                label7.Text = (Sprachen.EN_ER_SCHICHTEN);
                label8.Text = (Sprachen.EN_ER_UEBERSTUNDEN);
                label9.Text = (Sprachen.EN_ER_DAY);
                Lageranfangswert.Text = (Sprachen.EN_ER_ANFANGSWERT);
                Lagerzwischenwert.Text = (Sprachen.EN_ER_MITTELWERT);
                Lagerendwert.Text = (Sprachen.EN_ER_ENDWERT);

            }
            else
            {
                //DE Brotkrumenleiste
                lbl_Sicherheitsbestand.Text = (Sprachen.DE_LBL_SICHERHEITSBESTAND);
                lbl_Startseite.Text = (Sprachen.DE_LBL_STARTSEITE);
                lbl_Produktionsreihenfolge.Text = (Sprachen.EN_LBL_PRODUKTIONSREIHENFOLGE);
                lbl_Produktion.Text = (Sprachen.DE_LBL_PRODUKTION);
                lbl_Kapazitaetsplan.Text = (Sprachen.DE_LBL_KAPATITAETSPLAN);
                lbl_Kaufteiledisposition.Text = (Sprachen.DE_LBL_KAUFTEILEDISPOSITION);
                lbl_Ergebnis.Text = (Sprachen.DE_LBL_ERGEBNIS);

                //DE Buttons
                End_btn.Text = (Sprachen.DE_BTN_XML_EXPORT);
                back_btn.Text = (Sprachen.DE_BTN_BACK);

                //DE Groupboxen
                groupBox2.Text = (Sprachen.DE_ER_GROUPBOX2);
                groupBox3.Text = (Sprachen.DE_ER_GROUPBOX3);
                groupBox4.Text = (Sprachen.DE_ER_GROUPBOX4);
                Lagerwerte.Text = (Sprachen.DE_ER_LAGERWERT);

                //EN Label
                label1.Text = (Sprachen.DE_ER_TEIL);
                label3.Text = (Sprachen.DE_ER_MENGE);
                label5.Text = (Sprachen.DE_ER_BESTART);
                label2.Text = (Sprachen.DE_ER_TEIL);
                label4.Text = (Sprachen.DE_ER_MENGE);
                label6.Text = (Sprachen.DE_ER_ARBEITSPLATZ);
                label7.Text = (Sprachen.DE_ER_SCHICHTEN);
                label8.Text = (Sprachen.DE_ER_UEBERSTUNDEN);
                label9.Text = (Sprachen.DE_ER_DAY);
                Lageranfangswert.Text = (Sprachen.DE_ER_ANFANGSWERT);
                Lagerzwischenwert.Text = (Sprachen.DE_ER_MITTELWERT);
                Lagerendwert.Text = (Sprachen.DE_ER_ENDWERT);
            }
        }

        private void pic_en_Click(object sender, EventArgs e)
        {
            pic_en.SizeMode = PictureBoxSizeMode.StretchImage;
            pic_de.SizeMode = PictureBoxSizeMode.Normal;
            sprache = "en";
            sprachen();
        }

        private void pic_de_Click(object sender, EventArgs e)
        {
            pic_de.SizeMode = PictureBoxSizeMode.StretchImage;
            pic_en.SizeMode = PictureBoxSizeMode.Normal;
            sprache = "de";
            sprachen();
        }

        private void back_btn_Click(object sender, EventArgs e)
        {
            this.Controls.Clear();
            UserControl kaufteile = new Kaufteildisposition(aktPeriode, auftraege, direktverkaeufe,
                sicherheitsbest, produktion, produktionProg, prodReihenfolge, kapazitaet, kaufauftraege, sprache);
            this.Controls.Add(kaufteile);
        }

        private void lbl_Startseite_Click(object sender, EventArgs e)
        {
            this.Controls.Clear();
            UserControl import = new ImportPrognose(aktPeriode, auftraege, direktverkaeufe,
                sicherheitsbest, produktion, produktionProg, prodReihenfolge, kapazitaet, kaufauftraege, sprache);
            this.Controls.Add(import);
        }

        private void lbl_Sicherheitsbestand_Click(object sender, EventArgs e)
        {
            this.Controls.Clear();
            UserControl sicherheit = new Sicherheitsbestand(aktPeriode, auftraege, direktverkaeufe,
                sicherheitsbest, produktion, produktionProg, prodReihenfolge, kapazitaet, kaufauftraege, sprache);
            this.Controls.Add(sicherheit);
        }

        private void lbl_Produktion_Click(object sender, EventArgs e)
        {
            this.Controls.Clear();
            UserControl prod = new Produktion(aktPeriode, auftraege, direktverkaeufe,
                sicherheitsbest, produktion, produktionProg, prodReihenfolge, kapazitaet, kaufauftraege, sprache);
            this.Controls.Add(prod);
        }

        private void lbl_Produktionsreihenfolge_Click(object sender, EventArgs e)
        {
            this.Controls.Clear();
            UserControl prodreihenfolge = new Produktionsreihenfolge(aktPeriode, auftraege, direktverkaeufe,
                sicherheitsbest, produktion, produktionProg, prodReihenfolge, kapazitaet, kaufauftraege, sprache);
            this.Controls.Add(prodreihenfolge);
        }

        private void lbl_Kapazitaetsplan_Click(object sender, EventArgs e)
        {
            this.Controls.Clear();
            UserControl kapplan = new Kapazitaetsplan(aktPeriode, auftraege, direktverkaeufe,
                sicherheitsbest, produktion, produktionProg, prodReihenfolge, kapazitaet, kaufauftraege, sprache);
            this.Controls.Add(kapplan);
        }

        private void lbl_Kaufteiledisposition_Click(object sender, EventArgs e)
        {
            this.Controls.Clear();
            UserControl kaufteile = new Kaufteildisposition(aktPeriode, auftraege, direktverkaeufe,
                sicherheitsbest, produktion, produktionProg, prodReihenfolge, kapazitaet, kaufauftraege, sprache);
            this.Controls.Add(kaufteile);
        }

        private void End_btn_Click(object sender, EventArgs e)
        {
            // Mitteilung einblenden
            ProcessMessage message = new ProcessMessage(sprache);
            message.Show(this);
            message.Location = new Point(500, 300);
            message.Update();
            this.Enabled = false;

            // TODO - ExportXMLClass aufrufen
            try
            {
                System.Windows.Forms.FolderBrowserDialog objDialog = new FolderBrowserDialog();
                if (objDialog.ShowDialog(this) == DialogResult.OK)
                {
                    String pfad = objDialog.SelectedPath;
                    MessageBox.Show("Neuer Pfad: " + objDialog.SelectedPath);
                    ExportXMLClass exp = new ExportXMLClass();
                    exp.XMLExport(pfad,kaufauftraege, prodReihenfolge, kapazitaet, auftraege, direktverkaeufe);
                    MessageBox.Show("Die Datei wurde exportiert und ist verfügbar unter: "+pfad, "Export erfolgt");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("" + ex);
            }
            message.Close();
            this.Enabled = true;

            // TODO - Speicherort fuer XML-Datei auswaehlen lassen
        }
    }
}
