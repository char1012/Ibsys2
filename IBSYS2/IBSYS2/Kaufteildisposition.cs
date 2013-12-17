using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data.OleDb;



namespace IBSYS2
{
    public partial class Kaufteildisposition : UserControl
    {
        private OleDbConnection myconn;

        // Datenweitergabe:
        int aktPeriode;
        int[] auftraege = new int[12];
        int[] direktverkaeufe = new int[3];
        int[,] sicherheitsbest = new int[30, 5];
        int[,] produktion = new int[30, 2];
        int[,] produktionProg = new int[3, 5];
        int[,] prodReihenfolge = new int[30, 2];
        int[,] kapazitaet = new int[15, 5];
        int[,] kaufauftraege = new int[29, 6];

        public Kaufteildisposition()
        {
            InitializeComponent();
            continue_btn.Enabled = true; // false, wenn Zellen geleert werden
            setValues();
        }

        public Kaufteildisposition(int aktPeriode, int[] auftraege, int[] direktverkaeufe, int[,] sicherheitsbest,
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

            InitializeComponent();
            continue_btn.Enabled = true; // false, wenn Zellen geleert werden

            Boolean bereitsBerechnet = false;
            for (int i = 0; i < kaufauftraege.GetLength(0); i++)
            {
                if (kaufauftraege[i, 1] > 0)
                {
                    bereitsBerechnet = true;
                    break;
                }
            }
            // wenn bereits Werte vorhanden sind, Felder fuellen
            // die ersten drei Spalten trotzdem nochmal berechnen
            if (bereitsBerechnet == true)
            {
                // Werte simulieren
                int periode = aktPeriode - 1;
                //Produktion der P-Teile fuer die aktuelle und drei weitere Perioden

                // DB-Verbindung herstellen
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

                // Mitteilung einblenden
                ProcessMessage message = new ProcessMessage();
                message.Show(this);
                message.Location = new Point(500, 300);
                message.Update();
                this.Enabled = false;

                // Spalte Diskont
                //1.  Dicountmengen ermitteln
                int a = 0;
                double[,] teildaten = new double[29, 6];
                cmd.CommandText = @"SELECT Teilenummer, Startteilewert, Diskontmenge, Bestellkosten, Wiederbeschaffunszeit, Abweichung FROM Teil WHERE Diskontmenge > 0 ORDER BY Teilenummer ASC;";
                OleDbDataReader dbReader = cmd.ExecuteReader();
                while (dbReader.Read())
                {
                    teildaten[a, 0] = Convert.ToInt32(dbReader["Teilenummer"]);
                    teildaten[a, 1] = Convert.ToInt32(dbReader["Diskontmenge"]);
                    teildaten[a, 2] = Convert.ToInt32(dbReader["Bestellkosten"]);
                    teildaten[a, 3] = Convert.ToDouble(dbReader["Wiederbeschaffunszeit"]);
                    teildaten[a, 4] = Convert.ToDouble(dbReader["Abweichung"]);
                    teildaten[a, 5] = Convert.ToDouble(dbReader["Startteilewert"]);
                    a++;
                }
                dbReader.Close();
                // 2. Zellen fuellen
                for (int i = 0; i < teildaten.GetLength(0); ++i)
                {
                    int k = i + 1;
                    this.Controls.Find("D" + k.ToString(), true)[0].Text = teildaten[i, 1].ToString();
                }

                // Methode calculateBestand rufen
                int[,] bestand = calculateBestand(periode);

                // Methode calculateVerbrauch rufen
                int[,] verbrauch = calculateVerbrauch(produktionProg);

                // berechnen, wie lange das Lager noch reicht
                double[,] reichweite = calculateReichweite(bestand, verbrauch);

                for (int i = 0; i < kaufauftraege.GetLength(0); ++i)
                {
                    double zeit = teildaten[i, 3] + teildaten[i, 4];
                    int k = i + 1;

                    // Spalte Mindestmenge fuellen
                    int durchschnitt = (verbrauch[i, 1] + verbrauch[i, 2] + verbrauch[i, 3] + verbrauch[i, 4]) / 4;
                    //int mindestbestellwert = Convert.ToInt32(durchschnitt * zeit);
                    int mindestbestellwert = Convert.ToInt32(Math.Ceiling((durchschnitt * zeit) / 5.0) * 5);
                    this.Controls.Find("M" + k.ToString(), true)[0].Text = mindestbestellwert.ToString();

                    // Spalte optimale Bestellmenge fuellen
                    // Wurzel von (200 * Jahresbedarf * Bestellkosten) / (Einstandspreis * LHS)
                    int jahresbedarf = 52 * durchschnitt;
                    double optimaleMenge = Math.Round(Math.Sqrt((200 * jahresbedarf * teildaten[i, 2]) / (teildaten[i, 5] * 30)));
                    this.Controls.Find("O" + k.ToString(), true)[0].Text = optimaleMenge.ToString();

                    if (kaufauftraege[i, 4] != 0 || kaufauftraege[i, 5] != 0)
                    {
                        this.Controls.Find("BM" + k.ToString(), true)[0].Text = kaufauftraege[i, 4].ToString();
                        String bestellart = "";
                        if (kaufauftraege[i, 5] == 4)
                        {
                            bestellart = "E";
                        }
                        else if (kaufauftraege[i, 5] == 5)
                        {
                            bestellart = "N";
                        }
                        this.Controls.Find("B" + k.ToString(), true)[0].Text = bestellart;
                    }
                }

                message.Close();
                this.Enabled = true;
            }
            // sonst Werte berechnen
            else
            {
                setValues();
            }
        }

        public void setValues()
        {
            // Werte simulieren
            int periode = aktPeriode - 1;
            //Produktion der P-Teile fuer die aktuelle und drei weitere Perioden

            // DB-Verbindung herstellen
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

            // Mitteilung einblenden
            ProcessMessage message = new ProcessMessage();
            message.Show(this);
            message.Location = new Point(500, 300);
            message.Update();
            this.Enabled = false;

            // Spalte Diskont
            //1.  Dicountmengen ermitteln
            int a = 0;
            double[,] teildaten = new double[29,6];
            cmd.CommandText = @"SELECT Teilenummer, Startteilewert, Diskontmenge, Bestellkosten, Wiederbeschaffunszeit, Abweichung FROM Teil WHERE Diskontmenge > 0 ORDER BY Teilenummer ASC;";
            OleDbDataReader dbReader = cmd.ExecuteReader();
            while (dbReader.Read())
            {
                teildaten[a, 0] = Convert.ToInt32(dbReader["Teilenummer"]);
                teildaten[a, 1] = Convert.ToInt32(dbReader["Diskontmenge"]);
                teildaten[a, 2] = Convert.ToInt32(dbReader["Bestellkosten"]);
                teildaten[a, 3] = Convert.ToDouble(dbReader["Wiederbeschaffunszeit"]);
                teildaten[a, 4] = Convert.ToDouble(dbReader["Abweichung"]);
                teildaten[a, 5] = Convert.ToDouble(dbReader["Startteilewert"]);
                a++;
            }
            dbReader.Close();
            // 2. Zellen fuellen
            for (int i = 0; i < teildaten.GetLength(0); ++i)
            {
                int k = i + 1;
                this.Controls.Find("D" + k.ToString(), true)[0].Text = teildaten[i,1].ToString();
            }

            // Methode calculateBestand rufen
            int[,] bestand = calculateBestand(periode);

            // Methode calculateVerbrauch rufen
            int[,] verbrauch = calculateVerbrauch(produktionProg);

            // berechnen, wie lange das Lager noch reicht
            double[,] reichweite = calculateReichweite(bestand, verbrauch);

            // Spalte Bestellart fuellen
            String[,] bestellart = new String[29,2];
            for (int i = 0; i < bestellart.GetLength(0); ++i)
            {
                bestellart[i, 0] = teildaten[i, 0].ToString();
                String bestellartString = "";
                double zeit = teildaten[i, 3] + teildaten[i, 4];
                if (reichweite[i, 1] - zeit <= 0)
                {
                    bestellartString = "E";
                }
                else if (reichweite[i, 1] - zeit <= 1)
                {
                    bestellartString = "N";
                }
                bestellart[i, 1] = bestellartString;
                int k = i + 1;
                this.Controls.Find("B" + k.ToString(), true)[0].Text = bestellartString;

                // Spalte Mindestmenge fuellen
                int durchschnitt = (verbrauch[i, 1] + verbrauch[i, 2] + verbrauch[i, 3] + verbrauch[i, 4]) / 4;
                //int mindestbestellwert = Convert.ToInt32(durchschnitt * zeit);
                int mindestbestellwert = Convert.ToInt32(Math.Ceiling((durchschnitt * zeit) / 5.0) * 5);
                this.Controls.Find("M" + k.ToString(), true)[0].Text = mindestbestellwert.ToString();

                // Spalte optimale Bestellmenge fuellen
                // Wurzel von (200 * Jahresbedarf * Bestellkosten) / (Einstandspreis * LHS)
                int jahresbedarf = 52 * durchschnitt;
                double optimaleMenge = Math.Round(Math.Sqrt((200 * jahresbedarf * teildaten[i, 2]) / (teildaten[i, 5] * 30)));
                this.Controls.Find("O" + k.ToString(), true)[0].Text = optimaleMenge.ToString();

                // nur wenn etwas in Spalte Bestellart steht, die folgenden fuellen:
                if (bestellartString != "")
                {
                    // Spalte Bestellmenge fuellen
                    double bestellmenge = 0;

                    // Diskont-Menge
                    int diskont = Convert.ToInt32(teildaten[i, 1]);

                    // Lagerwerte zum Vergleich
                    double lagerMindest = 0;
                    if (mindestbestellwert >= diskont)
                    {
                        lagerMindest = mindestbestellwert * (teildaten[i, 5] / 100 * 90);
                    }
                    else
                    {
                        lagerMindest = mindestbestellwert * teildaten[i, 5];
                    }
                    double lagerOptimal = 0;
                    if (optimaleMenge >= diskont)
                    {
                        lagerOptimal = optimaleMenge * (teildaten[i, 5] / 100 * 90);
                    }
                    else
                    {
                        lagerOptimal = optimaleMenge * teildaten[i, 5];
                    }
                    double lagerDiskont = diskont * (teildaten[i, 5] / 100 * 90);

                    // optimale Bestellmenge außer die Diskontmenge ist hoeher und insgesamt günstiger
                    if ((diskont > optimaleMenge) && (lagerDiskont ==
                            Math.Min(Math.Min(lagerMindest, lagerOptimal), lagerDiskont)))
                    {
                        bestellmenge = diskont;
                    }
                    else
                    {
                        bestellmenge = optimaleMenge;
                    }

                    // die Bestellmenge darf nicht kleiner sein als die Mindestbestellmenge
                    if (mindestbestellwert > bestellmenge)
                    {
                        bestellmenge = mindestbestellwert;
                    }
                    
                    this.Controls.Find("BM" + k.ToString(), true)[0].Text = bestellmenge.ToString();
                }
            }

            message.Close();
            this.Enabled = true;
        }

        // Methode, um Bestand (Anfangsbest. + eingeh. Best. - noch zu entnehmen) zu ermitteln
        private int[,] calculateBestand(int periode)
        {
            int[,] teile = new int[29, 2];

            OleDbCommand cmd = new OleDbCommand();
            cmd.CommandType = CommandType.Text;
            cmd.Connection = myconn;
            OleDbCommand cmd2 = new OleDbCommand();
            cmd2.CommandType = CommandType.Text;
            cmd2.Connection = myconn;
            OleDbCommand cmd3 = new OleDbCommand();
            cmd3.CommandType = CommandType.Text;
            cmd3.Connection = myconn;

            // 1. Anfangsbestand aus DB lesen
            int a = 0;
            cmd.CommandText = @"SELECT Teilenummer_FK, Bestand FROM Lager WHERE Teilenummer_FK IN (SELECT Teilenummer FROM Teil WHERE Diskontmenge > 0) AND Periode = " + periode + " ORDER BY Teilenummer_FK ASC;";
            OleDbDataReader dbReader = cmd.ExecuteReader();
            while (dbReader.Read())
            {
                teile[a, 0] = Convert.ToInt32(dbReader["Teilenummer_FK"]);
                teile[a, 1] += Convert.ToInt32(dbReader["Bestand"]);
                a++;
            }
            dbReader.Close();

            // 2. noch eingehende Bestellungen aus der DB lesen
            cmd.CommandText = @"SELECT Teilenummer_FK, Menge FROM Bestellung WHERE Eingegangen = False AND Periode = " + periode + " ORDER BY Teilenummer_FK ASC;";
            dbReader = cmd.ExecuteReader();
            while (dbReader.Read())
            {
                for (int i = 0; i < teile.GetLength(0); i++)
                {
                    if (teile[i, 0] == Convert.ToInt32(dbReader["Teilenummer_FK"]))
                    {
                        teile[i, 1] += Convert.ToInt32(dbReader["Menge"]);
                    }
                }
            }
            dbReader.Close();

            // 3. noch zu entnehmen berechnen
            for (int i = 0; i < teile.GetLength(0); i++)
            {
                cmd.CommandText = @"SELECT Arbeitszeit_Erzeugnis_FK, Anzahl FROM Kaufteil_Arbeitszeit_Erzeugnis WHERE Kaufteil_Teilenummer_FK = " + teile[i,0] + ";";
                dbReader = cmd.ExecuteReader();
                while (dbReader.Read())
                {
                    cmd2.CommandText = @"SELECT Erzeugnis_Teilenummer_FK, Arbeitsplatz_FK, Reihenfolge FROM Arbeitsplatz_Erzeugnis WHERE ID = " + dbReader["Arbeitszeit_Erzeugnis_FK"] + ";";
                    OleDbDataReader dbReader2 = cmd2.ExecuteReader();
                    while (dbReader2.Read()) // hier sollte eine Zeile herauskommen
                    {
                        OleDbDataReader dbReader3;

                        // wenn es bei Reihenfolge = 1 einfliesst, muss nur der aktuelle Platz beruecksichtigt werden
                        int[] plaetze = new int[Convert.ToInt32(dbReader2["Reihenfolge"])];
                        plaetze[0] = Convert.ToInt32(dbReader2["Arbeitsplatz_FK"]);
                        a = 1;
                        // wenn die Reihenfolge > 1, muessen alle Plaetze beachtet werden, die kleiner der RF sind
                        if (Convert.ToInt32(dbReader2["Reihenfolge"]) > 1)
                        {
                            cmd3.CommandText = @"SELECT Arbeitsplatz_FK FROM Arbeitsplatz_Erzeugnis WHERE Erzeugnis_Teilenummer_FK = " + dbReader2["Erzeugnis_Teilenummer_FK"] + " AND Reihenfolge < " + dbReader2["Reihenfolge"] + ";";
                            dbReader3 = cmd3.ExecuteReader();
                            while (dbReader3.Read())
                            {
                                plaetze[a] = Convert.ToInt32(dbReader3["Arbeitsplatz_FK"]);
                                a++;
                            }
                            dbReader3.Close();
                        }

                        // fuer diese Arbeitsplaetze muss nun Warteliste_Arbeitsplatz geprueft werden
                        for (int no = 0; no < plaetze.Length; no++)
                        {
                            cmd3.CommandText = @"SELECT Menge FROM Warteliste_Arbeitsplatz WHERE Arbeitsplatz_FK = " + plaetze[no] + " AND Teilenummer_FK = " + dbReader2["Erzeugnis_Teilenummer_FK"] + " AND Periode = " + periode + ";";
                            dbReader3 = cmd3.ExecuteReader();
                            while (dbReader3.Read())
                            {
                                // Menge mit Anzahl multiplizieren und von Bestand abziehen
                                int menge = Convert.ToInt32(dbReader3["Menge"]) * Convert.ToInt32(dbReader["Anzahl"]);
                                teile[i, 1] -= menge;
                            }
                            dbReader3.Close();

                            // die Teile in Bearbeitung nicht fuer den Platz beachten, in den es einfliesst
                            if(no >= 1)
                            {
                                cmd3.CommandText = @"SELECT Menge FROM Bearbeitung WHERE Arbeitsplatz_FK = " + plaetze[no] + " AND Teilenummer_FK = " + dbReader2["Erzeugnis_Teilenummer_FK"] + " AND Periode = " + periode + ";";
                                dbReader3 = cmd3.ExecuteReader();
                                while (dbReader3.Read())
                                {
                                    // Menge mit Anzahl multiplizieren und von Bestand abziehen
                                    int menge = Convert.ToInt32(dbReader3["Menge"]) * Convert.ToInt32(dbReader["Anzahl"]);
                                    teile[i, 1] -= menge;
                                }
                                dbReader3.Close();
                            }
                        }

                        // pruefen, ob Erzeugnis_Teilenummer_FK in Warteliste_Material zu finden ist
                        cmd3.CommandText = @"SELECT Menge FROM Warteliste_Material WHERE Erzeugnis_Teilenummer_FK = " + dbReader2["Erzeugnis_Teilenummer_FK"] + " AND Periode = " + periode + ";";
                        dbReader3 = cmd3.ExecuteReader();
                        while (dbReader3.Read())
                        {
                            // Menge mit Anzahl multiplizieren und von Bestand abziehen
                            int menge = Convert.ToInt32(dbReader3["Menge"]) * Convert.ToInt32(dbReader["Anzahl"]);
                            teile[i, 1] -= menge;
                        }
                        dbReader3.Close();
                    }
                    dbReader2.Close();
                }
                dbReader.Close();
            }

            return teile;
        }

        private int[,] calculateVerbrauch(int[,] produktionProg)
        {
            int[,] verbrauch = new int[29, 5];

            OleDbCommand cmd = new OleDbCommand();
            cmd.CommandType = CommandType.Text;
            cmd.Connection = myconn;

            int a = 0;
            int[,] verwendung = new int[29,4];
            cmd.CommandText = @"SELECT K_Teil, P1, P2, P3 FROM Verwendung ORDER BY K_Teil ASC;";
            OleDbDataReader dbReader = cmd.ExecuteReader();
            while (dbReader.Read())
            {
                verwendung[a, 0] = Convert.ToInt32(dbReader["K_Teil"]);
                verwendung[a, 1] = Convert.ToInt32(dbReader["P1"]);
                verwendung[a, 2] = Convert.ToInt32(dbReader["P2"]);
                verwendung[a, 3] = Convert.ToInt32(dbReader["P3"]);
                a++;
            }

            for (int i = 0; i < verbrauch.GetLength(0); i++)
            {
                verbrauch[i, 0] = verwendung[i, 0];
                verbrauch[i, 1] = // Verbrauch aktuelle Periode
                    (produktionProg[0, 1] * verwendung[i, 1]) + (produktionProg[1, 1] * verwendung[i, 2])
                    + (produktionProg[2, 1] * verwendung[i, 3]);
                verbrauch[i, 2] = // Verbrauch Periode akt+1
                    (produktionProg[0, 2] * verwendung[i, 1]) + (produktionProg[1, 2] * verwendung[i, 2])
                    + (produktionProg[2, 2] * verwendung[i, 3]);
                verbrauch[i, 3] = // Verbrauch Periode akt+2
                    (produktionProg[0, 3] * verwendung[i, 1]) + (produktionProg[1, 3] * verwendung[i, 2])
                    + (produktionProg[2, 3] * verwendung[i, 3]);
                verbrauch[i, 4] = // Verbrauch Periode akt+3
                    (produktionProg[0, 4] * verwendung[i, 1]) + (produktionProg[1, 4] * verwendung[i, 2])
                    + (produktionProg[2, 4] * verwendung[i, 3]);
            }

            return verbrauch;
        }

        private double[,] calculateReichweite(int[,] bestand, int[,] verbrauch)
        {
            double[,] reichweite = new double[29, 2];

            for (int i = 0; i < bestand.GetLength(0); i++)
            {
                reichweite[i, 0] = bestand[i, 0];
                double teilBestand = bestand[i, 1];
                double teilReichweite = 0;
                for (int no = 1; no < verbrauch.GetLength(1); no++)
                {
                    if (teilBestand < verbrauch[i, no])
                    {
                        teilReichweite = (no - 1) + (teilBestand / verbrauch[i, no]);
                        break;
                    }
                    else if (no == verbrauch.GetLength(1) - 1)
                    {
                        // wegen Teil 24 kontrollieren, ob es auch eine 5. Periode reichen wuerde
                        // Durchschnitt der Perioden errechnen
                        int durchschnitt = (verbrauch[i, 1] + verbrauch[i, 2] + verbrauch[i, 3] + verbrauch[i, 4]) / 4;
                        if ((teilBestand - verbrauch[i, no]) < durchschnitt)
                        {
                            teilReichweite = 4;
                        }
                        else
                        {
                            teilReichweite = 5;
                        }
                    }
                    else
                    {
                        teilBestand -= verbrauch[i, no];
                    }
                }
                reichweite[i, 1] = teilReichweite;
            }

            return reichweite;
        }

        private void continue_btn_Click(object sender, EventArgs e)
        {
            // Datenweitergabe

            // Werte aus TextBoxen in kapazitaet auslesen
            for (int i = 0; i < kaufauftraege.GetLength(0); ++i)
            {
                int k = i + 1;

                String wert = this.Controls.Find("label" + k.ToString(), true)[0].Text;
                if (wert == "")
                {
                    kaufauftraege[i, 0] = 0;
                }
                else
                {
                    kaufauftraege[i, 0] = Convert.ToInt32(wert);
                }

                wert = this.Controls.Find("D" + k.ToString(), true)[0].Text;
                if (wert == "")
                {
                    kaufauftraege[i, 1] = 0;
                }
                else
                {
                    kaufauftraege[i, 1] = Convert.ToInt32(wert);
                }

                wert = this.Controls.Find("M" + k.ToString(), true)[0].Text;
                if (wert == "")
                {
                    kaufauftraege[i, 2] = 0;
                }
                else
                {
                    kaufauftraege[i, 2] = Convert.ToInt32(wert);
                }

                wert = this.Controls.Find("O" + k.ToString(), true)[0].Text;
                if (wert == "")
                {
                    kaufauftraege[i, 3] = 0;
                }
                else
                {
                    kaufauftraege[i, 3] = Convert.ToInt32(wert);
                }

                wert = this.Controls.Find("BM" + k.ToString(), true)[0].Text;
                if (wert == "")
                {
                    kaufauftraege[i, 4] = 0;
                }
                else
                {
                    kaufauftraege[i, 4] = Convert.ToInt32(wert);
                }

                String bestellart = this.Controls.Find("B" + k.ToString(), true)[0].Text;
                if (bestellart == "E")
                {
                    kaufauftraege[i, 5] = 4;
                }
                else if (bestellart == "N")
                {
                    kaufauftraege[i, 5] = 5;
                }
                else
                {
                    kaufauftraege[i, 5] = 0;
                }
            }

            this.Controls.Clear();
            UserControl ergebnis = new Ergebnis(aktPeriode, auftraege, direktverkaeufe,
                sicherheitsbest, produktion, produktionProg, prodReihenfolge, kapazitaet, kaufauftraege);
            this.Controls.Add(ergebnis);
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
                back_btn.Text = (Sprachen.EN_BTN_BACK);

                //EN Groupboxen
                groupBox1.Text = (Sprachen.EN_KD_GROUPBOX1);
                
                //EN Labels
                /*
                lbl_menge1.Text = (Sprachen.EN_LBL_KD_MENGE);
                lbl_menge2.Text = (Sprachen.EN_LBL_KD_MENGE);
                lbl_menge3.Text = (Sprachen.EN_LBL_KD_MENGE);
                lbl_bestellart1.Text = (Sprachen.EN_LBL_KD_BESTELLART);
                lbl_bestellart2.Text = (Sprachen.EN_LBL_KD_BESTELLART);
                lbl_bestellart3.Text = (Sprachen.EN_LBL_KD_BESTELLART);
                */

                //EN Tooltip
                System.Windows.Forms.ToolTip ToolTipEN = new System.Windows.Forms.ToolTip();
                ToolTipEN.SetToolTip(this.pictureBox7, Sprachen.EN_KD_INFO);

            }
            else
            {
                //DE Brotkrumenleiste
                lbl_Startseite.Text = (Sprachen.DE_LBL_STARTSEITE);
                lbl_Sicherheitsbestand.Text = (Sprachen.DE_LBL_SICHERHEITSBESTAND);
                lbl_Produktion.Text = (Sprachen.DE_LBL_PRODUKTION);
                lbl_Produktionsreihenfolge.Text = (Sprachen.DE_LBL_PRODUKTIONSREIHENFOLGE);
                lbl_Kapazitaetsplan.Text = (Sprachen.DE_LBL_KAPATITAETSPLAN);
                lbl_Kaufteiledisposition.Text = (Sprachen.DE_LBL_KAUFTEILEDISPOSITION);
                lbl_Ergebnis.Text = (Sprachen.DE_LBL_ERGEBNIS);

                //DE Buttons
                continue_btn.Text = (Sprachen.EN_BTN_CONTINUE);
                back_btn.Text = (Sprachen.DE_BTN_BACK);

                //DE Groupboxen
                groupBox1.Text = (Sprachen.DE_KD_GROUPBOX1);

                //DE Labels
                /*
                lbl_menge1.Text = (Sprachen.DE_LBL_KD_MENGE);
                lbl_menge2.Text = (Sprachen.DE_LBL_KD_MENGE);
                lbl_menge3.Text = (Sprachen.DE_LBL_KD_MENGE);
                lbl_bestellart1.Text = (Sprachen.DE_LBL_KD_BESTELLART);
                lbl_bestellart2.Text = (Sprachen.DE_LBL_KD_BESTELLART);
                lbl_bestellart3.Text = (Sprachen.DE_LBL_KD_BESTELLART);
                */

                //DE Tooltip
                System.Windows.Forms.ToolTip ToolTipDE = new System.Windows.Forms.ToolTip();
                ToolTipDE.SetToolTip(this.pictureBox7, Sprachen.DE_KD_INFO);
                
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

        private void back_btn_Click(object sender, EventArgs e)
        {
            // Datenweitergabe

            // Werte aus TextBoxen in kapazitaet auslesen
            for (int i = 0; i < kaufauftraege.GetLength(0); ++i)
            {
                int k = i + 1;

                String wert = this.Controls.Find("label" + k.ToString(), true)[0].Text;
                if (wert == "")
                {
                    kaufauftraege[i, 0] = 0;
                }
                else
                {
                    kaufauftraege[i, 0] = Convert.ToInt32(wert);
                }

                wert = this.Controls.Find("D" + k.ToString(), true)[0].Text;
                if (wert == "")
                {
                    kaufauftraege[i, 1] = 0;
                }
                else
                {
                    kaufauftraege[i, 1] = Convert.ToInt32(wert);
                }

                wert = this.Controls.Find("M" + k.ToString(), true)[0].Text;
                if (wert == "")
                {
                    kaufauftraege[i, 2] = 0;
                }
                else
                {
                    kaufauftraege[i, 2] = Convert.ToInt32(wert);
                }

                wert = this.Controls.Find("O" + k.ToString(), true)[0].Text;
                if (wert == "")
                {
                    kaufauftraege[i, 3] = 0;
                }
                else
                {
                    kaufauftraege[i, 3] = Convert.ToInt32(wert);
                }

                wert = this.Controls.Find("BM" + k.ToString(), true)[0].Text;
                if (wert == "")
                {
                    kaufauftraege[i, 4] = 0;
                }
                else
                {
                    kaufauftraege[i, 4] = Convert.ToInt32(wert);
                }

                String bestellart = this.Controls.Find("B" + k.ToString(), true)[0].Text;
                if (bestellart == "E")
                {
                    kaufauftraege[i, 5] = 4;
                }
                else if (bestellart == "N")
                {
                    kaufauftraege[i, 5] = 5;
                }
                else
                {
                    kaufauftraege[i, 5] = 0;
                }
            }

            this.Controls.Clear();
            UserControl kapplan = new Kapazitaetsplan(aktPeriode, auftraege, direktverkaeufe,
                sicherheitsbest, produktion, produktionProg, prodReihenfolge, kapazitaet, kaufauftraege);
            this.Controls.Add(kapplan);
        }

        private void lbl_Startseite_Click(object sender, EventArgs e)
        {
            // Datenweitergabe

            // Werte aus TextBoxen in kapazitaet auslesen
            for (int i = 0; i < kaufauftraege.GetLength(0); ++i)
            {
                int k = i + 1;

                String wert = this.Controls.Find("label" + k.ToString(), true)[0].Text;
                if (wert == "")
                {
                    kaufauftraege[i, 0] = 0;
                }
                else
                {
                    kaufauftraege[i, 0] = Convert.ToInt32(wert);
                }

                wert = this.Controls.Find("D" + k.ToString(), true)[0].Text;
                if (wert == "")
                {
                    kaufauftraege[i, 1] = 0;
                }
                else
                {
                    kaufauftraege[i, 1] = Convert.ToInt32(wert);
                }

                wert = this.Controls.Find("M" + k.ToString(), true)[0].Text;
                if (wert == "")
                {
                    kaufauftraege[i, 2] = 0;
                }
                else
                {
                    kaufauftraege[i, 2] = Convert.ToInt32(wert);
                }

                wert = this.Controls.Find("O" + k.ToString(), true)[0].Text;
                if (wert == "")
                {
                    kaufauftraege[i, 3] = 0;
                }
                else
                {
                    kaufauftraege[i, 3] = Convert.ToInt32(wert);
                }

                wert = this.Controls.Find("BM" + k.ToString(), true)[0].Text;
                if (wert == "")
                {
                    kaufauftraege[i, 4] = 0;
                }
                else
                {
                    kaufauftraege[i, 4] = Convert.ToInt32(wert);
                }

                String bestellart = this.Controls.Find("B" + k.ToString(), true)[0].Text;
                if (bestellart == "E")
                {
                    kaufauftraege[i, 5] = 4;
                }
                else if (bestellart == "N")
                {
                    kaufauftraege[i, 5] = 5;
                }
                else
                {
                    kaufauftraege[i, 5] = 0;
                }
            }

            this.Controls.Clear();
            UserControl import = new ImportPrognose(aktPeriode, auftraege, direktverkaeufe,
                sicherheitsbest, produktion, produktionProg, prodReihenfolge, kapazitaet, kaufauftraege);
            this.Controls.Add(import);
        }

        private void lbl_Sicherheitsbestand_Click(object sender, EventArgs e)
        {
            // Datenweitergabe

            // Werte aus TextBoxen in kapazitaet auslesen
            for (int i = 0; i < kaufauftraege.GetLength(0); ++i)
            {
                int k = i + 1;

                String wert = this.Controls.Find("label" + k.ToString(), true)[0].Text;
                if (wert == "")
                {
                    kaufauftraege[i, 0] = 0;
                }
                else
                {
                    kaufauftraege[i, 0] = Convert.ToInt32(wert);
                }

                wert = this.Controls.Find("D" + k.ToString(), true)[0].Text;
                if (wert == "")
                {
                    kaufauftraege[i, 1] = 0;
                }
                else
                {
                    kaufauftraege[i, 1] = Convert.ToInt32(wert);
                }

                wert = this.Controls.Find("M" + k.ToString(), true)[0].Text;
                if (wert == "")
                {
                    kaufauftraege[i, 2] = 0;
                }
                else
                {
                    kaufauftraege[i, 2] = Convert.ToInt32(wert);
                }

                wert = this.Controls.Find("O" + k.ToString(), true)[0].Text;
                if (wert == "")
                {
                    kaufauftraege[i, 3] = 0;
                }
                else
                {
                    kaufauftraege[i, 3] = Convert.ToInt32(wert);
                }

                wert = this.Controls.Find("BM" + k.ToString(), true)[0].Text;
                if (wert == "")
                {
                    kaufauftraege[i, 4] = 0;
                }
                else
                {
                    kaufauftraege[i, 4] = Convert.ToInt32(wert);
                }

                String bestellart = this.Controls.Find("B" + k.ToString(), true)[0].Text;
                if (bestellart == "E")
                {
                    kaufauftraege[i, 5] = 4;
                }
                else if (bestellart == "N")
                {
                    kaufauftraege[i, 5] = 5;
                }
                else
                {
                    kaufauftraege[i, 5] = 0;
                }
            }

            this.Controls.Clear();
            UserControl sicherheit = new Sicherheitsbestand(aktPeriode, auftraege, direktverkaeufe,
                sicherheitsbest, produktion, produktionProg, prodReihenfolge, kapazitaet, kaufauftraege);
            this.Controls.Add(sicherheit);
        }

        private void lbl_Produktion_Click(object sender, EventArgs e)
        {
            // Datenweitergabe

            // Werte aus TextBoxen in kapazitaet auslesen
            for (int i = 0; i < kaufauftraege.GetLength(0); ++i)
            {
                int k = i + 1;

                String wert = this.Controls.Find("label" + k.ToString(), true)[0].Text;
                if (wert == "")
                {
                    kaufauftraege[i, 0] = 0;
                }
                else
                {
                    kaufauftraege[i, 0] = Convert.ToInt32(wert);
                }

                wert = this.Controls.Find("D" + k.ToString(), true)[0].Text;
                if (wert == "")
                {
                    kaufauftraege[i, 1] = 0;
                }
                else
                {
                    kaufauftraege[i, 1] = Convert.ToInt32(wert);
                }

                wert = this.Controls.Find("M" + k.ToString(), true)[0].Text;
                if (wert == "")
                {
                    kaufauftraege[i, 2] = 0;
                }
                else
                {
                    kaufauftraege[i, 2] = Convert.ToInt32(wert);
                }

                wert = this.Controls.Find("O" + k.ToString(), true)[0].Text;
                if (wert == "")
                {
                    kaufauftraege[i, 3] = 0;
                }
                else
                {
                    kaufauftraege[i, 3] = Convert.ToInt32(wert);
                }

                wert = this.Controls.Find("BM" + k.ToString(), true)[0].Text;
                if (wert == "")
                {
                    kaufauftraege[i, 4] = 0;
                }
                else
                {
                    kaufauftraege[i, 4] = Convert.ToInt32(wert);
                }

                String bestellart = this.Controls.Find("B" + k.ToString(), true)[0].Text;
                if (bestellart == "E")
                {
                    kaufauftraege[i, 5] = 4;
                }
                else if (bestellart == "N")
                {
                    kaufauftraege[i, 5] = 5;
                }
                else
                {
                    kaufauftraege[i, 5] = 0;
                }
            }

            this.Controls.Clear();
            UserControl prod = new Produktion(aktPeriode, auftraege, direktverkaeufe,
                sicherheitsbest, produktion, produktionProg, prodReihenfolge, kapazitaet, kaufauftraege);
            this.Controls.Add(prod);
        }

        private void lbl_Produktionsreihenfolge_Click(object sender, EventArgs e)
        {
            // Datenweitergabe

            // Werte aus TextBoxen in kapazitaet auslesen
            for (int i = 0; i < kaufauftraege.GetLength(0); ++i)
            {
                int k = i + 1;

                String wert = this.Controls.Find("label" + k.ToString(), true)[0].Text;
                if (wert == "")
                {
                    kaufauftraege[i, 0] = 0;
                }
                else
                {
                    kaufauftraege[i, 0] = Convert.ToInt32(wert);
                }

                wert = this.Controls.Find("D" + k.ToString(), true)[0].Text;
                if (wert == "")
                {
                    kaufauftraege[i, 1] = 0;
                }
                else
                {
                    kaufauftraege[i, 1] = Convert.ToInt32(wert);
                }

                wert = this.Controls.Find("M" + k.ToString(), true)[0].Text;
                if (wert == "")
                {
                    kaufauftraege[i, 2] = 0;
                }
                else
                {
                    kaufauftraege[i, 2] = Convert.ToInt32(wert);
                }

                wert = this.Controls.Find("O" + k.ToString(), true)[0].Text;
                if (wert == "")
                {
                    kaufauftraege[i, 3] = 0;
                }
                else
                {
                    kaufauftraege[i, 3] = Convert.ToInt32(wert);
                }

                wert = this.Controls.Find("BM" + k.ToString(), true)[0].Text;
                if (wert == "")
                {
                    kaufauftraege[i, 4] = 0;
                }
                else
                {
                    kaufauftraege[i, 4] = Convert.ToInt32(wert);
                }

                String bestellart = this.Controls.Find("B" + k.ToString(), true)[0].Text;
                if (bestellart == "E")
                {
                    kaufauftraege[i, 5] = 4;
                }
                else if (bestellart == "N")
                {
                    kaufauftraege[i, 5] = 5;
                }
                else
                {
                    kaufauftraege[i, 5] = 0;
                }
            }

            this.Controls.Clear();
            UserControl prodreihenfolge = new Produktionsreihenfolge(aktPeriode, auftraege, direktverkaeufe,
                sicherheitsbest, produktion, produktionProg, prodReihenfolge, kapazitaet, kaufauftraege);
            this.Controls.Add(prodreihenfolge);
        }

        private void lbl_Kapazitaetsplan_Click(object sender, EventArgs e)
        {
            // Datenweitergabe

            // Werte aus TextBoxen in kapazitaet auslesen
            for (int i = 0; i < kaufauftraege.GetLength(0); ++i)
            {
                int k = i + 1;

                String wert = this.Controls.Find("label" + k.ToString(), true)[0].Text;
                if (wert == "")
                {
                    kaufauftraege[i, 0] = 0;
                }
                else
                {
                    kaufauftraege[i, 0] = Convert.ToInt32(wert);
                }

                wert = this.Controls.Find("D" + k.ToString(), true)[0].Text;
                if (wert == "")
                {
                    kaufauftraege[i, 1] = 0;
                }
                else
                {
                    kaufauftraege[i, 1] = Convert.ToInt32(wert);
                }

                wert = this.Controls.Find("M" + k.ToString(), true)[0].Text;
                if (wert == "")
                {
                    kaufauftraege[i, 2] = 0;
                }
                else
                {
                    kaufauftraege[i, 2] = Convert.ToInt32(wert);
                }

                wert = this.Controls.Find("O" + k.ToString(), true)[0].Text;
                if (wert == "")
                {
                    kaufauftraege[i, 3] = 0;
                }
                else
                {
                    kaufauftraege[i, 3] = Convert.ToInt32(wert);
                }

                wert = this.Controls.Find("BM" + k.ToString(), true)[0].Text;
                if (wert == "")
                {
                    kaufauftraege[i, 4] = 0;
                }
                else
                {
                    kaufauftraege[i, 4] = Convert.ToInt32(wert);
                }

                String bestellart = this.Controls.Find("B" + k.ToString(), true)[0].Text;
                if (bestellart == "E")
                {
                    kaufauftraege[i, 5] = 4;
                }
                else if (bestellart == "N")
                {
                    kaufauftraege[i, 5] = 5;
                }
                else
                {
                    kaufauftraege[i, 5] = 0;
                }
            }

            this.Controls.Clear();
            UserControl kapplan = new Kapazitaetsplan(aktPeriode, auftraege, direktverkaeufe,
                sicherheitsbest, produktion, produktionProg, prodReihenfolge, kapazitaet, kaufauftraege);
            this.Controls.Add(kapplan);
        }

        private void lbl_Ergebnis_Click(object sender, EventArgs e)
        {
            // Datenweitergabe

            // Werte aus TextBoxen in kapazitaet auslesen
            for (int i = 0; i < kaufauftraege.GetLength(0); ++i)
            {
                int k = i + 1;

                String wert = this.Controls.Find("label" + k.ToString(), true)[0].Text;
                if (wert == "")
                {
                    kaufauftraege[i, 0] = 0;
                }
                else
                {
                    kaufauftraege[i, 0] = Convert.ToInt32(wert);
                }

                wert = this.Controls.Find("D" + k.ToString(), true)[0].Text;
                if (wert == "")
                {
                    kaufauftraege[i, 1] = 0;
                }
                else
                {
                    kaufauftraege[i, 1] = Convert.ToInt32(wert);
                }

                wert = this.Controls.Find("M" + k.ToString(), true)[0].Text;
                if (wert == "")
                {
                    kaufauftraege[i, 2] = 0;
                }
                else
                {
                    kaufauftraege[i, 2] = Convert.ToInt32(wert);
                }

                wert = this.Controls.Find("O" + k.ToString(), true)[0].Text;
                if (wert == "")
                {
                    kaufauftraege[i, 3] = 0;
                }
                else
                {
                    kaufauftraege[i, 3] = Convert.ToInt32(wert);
                }

                wert = this.Controls.Find("BM" + k.ToString(), true)[0].Text;
                if (wert == "")
                {
                    kaufauftraege[i, 4] = 0;
                }
                else
                {
                    kaufauftraege[i, 4] = Convert.ToInt32(wert);
                }

                String bestellart = this.Controls.Find("B" + k.ToString(), true)[0].Text;
                if (bestellart == "E")
                {
                    kaufauftraege[i, 5] = 4;
                }
                else if (bestellart == "N")
                {
                    kaufauftraege[i, 5] = 5;
                }
                else
                {
                    kaufauftraege[i, 5] = 0;
                }
            }

            this.Controls.Clear();
            UserControl ergebnis = new Ergebnis(aktPeriode, auftraege, direktverkaeufe,
                sicherheitsbest, produktion, produktionProg, prodReihenfolge, kapazitaet, kaufauftraege);
            this.Controls.Add(ergebnis);
        }

    }
}
