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
        // Tooltip:
        // Die Mindestmenge zeigt, wieviel Sie mindestens bestellen muessen, dass die Menge für die Wiederbeschaffungszeit reicht.
        // Die optimale Bestellmenge richtet sich nach der Formel zur optimalen Bestellmenge und stellt die kostengünstigste Bestellmenge dar.
        // Sie müssen jedoch beachten, dass diese Formel nicht die sprungfixen Lagerhaltungskosten einkalkuliert.
        // Als Bestellart können Sie entweder N(ormal) oder E(xpress) eingeben.

        private OleDbConnection myconn;
        private String sprache = "de";
        private char[] digits = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
        private char[] letters = new char[] { 'E', 'N' };
        bool tBM1 = true, tBM2 = true, tBM3 = true, tBM4 = true, tBM5 = true, tBM6 = true, tBM7 = true, tBM8 = true, tBM9 = true, tBM10 = true, tBM11 = true, tBM12 = true, tBM13 = true, tBM14 = true, tBM15 = true, tBM16 = true, tBM17 = true, tBM18 = true, tBM19 = true, tBM20 = true, tBM21 = true, tBM22 = true, tBM23 = true, tBM24 = true, tBM25 = true, tBM26 = true, tBM27 = true, tBM28 = true, tBM29 = true, tB1 = true, tB2 = true, tB3 = true, tB4 = true, tB5 = true, tB6 = true, tB7 = true, tB8 = true, tB9 = true, tB10 = true, tB11 = true, tB12 = true, tB13 = true, tB14 = true, tB15 = true, tB16 = true, tB17 = true, tB18 = true, tB19 = true, tB20 = true, tB21 = true, tB22 = true, tB23 = true, tB24 = true, tB25 = true, tB26 = true, tB27 = true, tB28 = true, tB29 = true;
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

        public Kaufteildisposition()
        {
            InitializeComponent();
            continue_btn.Enabled = true; // false, wenn Zellen geleert werden
            setValues();
        }

        public Kaufteildisposition(int aktPeriode, int[] auftraege, double[,] direktverkaeufe, int[,] sicherheitsbest,
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
            continue_btn.Enabled = true; // false, wenn Zellen geleert werden
            sprachen();

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
                    myconn.Close();
                    myconn.Open();
                }

                // Mitteilung einblenden
                ProcessMessage message = new ProcessMessage(sprache);
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
                    else
                    {
                        // um eventuell vorhandene Werte zu loeschen
                        this.Controls.Find("BM" + k.ToString(), true)[0].Text = "";
                        this.Controls.Find("B" + k.ToString(), true)[0].Text = "";
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
                myconn.Close();
                myconn.Open();
            }

            // Mitteilung einblenden
            ProcessMessage message = new ProcessMessage(sprache);
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
                else
                {
                    // um eventuell vorhandene Werte zu loeschen
                    bestellartString = "";
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
                else
                {
                    // um eventuell vorhandene Werte zu loeschen
                    this.Controls.Find("BM" + k.ToString(), true)[0].Text = "";
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

            OleDbDataReader dbReader;

            // 1. Anfangsbestand aus DB lesen
            int a = 0;
            if (aktPeriode > 1)
            {
                cmd.CommandText = @"SELECT Teilenummer_FK, Bestand FROM Lager WHERE Teilenummer_FK IN (SELECT Teilenummer FROM Teil WHERE Diskontmenge > 0) AND Periode = " + periode + " ORDER BY Teilenummer_FK ASC;";
                dbReader = cmd.ExecuteReader();
                while (dbReader.Read())
                {
                    teile[a, 0] = Convert.ToInt32(dbReader["Teilenummer_FK"]);
                    teile[a, 1] += Convert.ToInt32(dbReader["Bestand"]);
                    a++;
                }
                dbReader.Close();
            }
            else
            {
                cmd.CommandText = @"SELECT Teilenummer, Startbestand FROM Teil WHERE Art = 'K' ORDER BY Teilenummer ASC;";
                dbReader = cmd.ExecuteReader();
                while (dbReader.Read())
                {
                    teile[a, 0] = Convert.ToInt32(dbReader["Teilenummer"]);
                    teile[a, 1] += Convert.ToInt32(dbReader["Startbestand"]);
                    a++;
                }
                dbReader.Close();
            }

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
            bool t = true;
            for (int i = 0; i < kaufauftraege.GetLength(0); ++i)
            {
                if (kaufauftraege[i, 4] != 0 & kaufauftraege[i, 5] == 0)
                {
                    t = false;
                }
                else if (kaufauftraege[i, 4] == 0 & kaufauftraege[i, 5] != 0)
                {
                    t = false;
                }
            }

            if (t == true)
            {
                this.Controls.Clear();
                UserControl ergebnis = new Ergebnis(aktPeriode, auftraege, direktverkaeufe,
                    sicherheitsbest, produktion, produktionProg, prodReihenfolge, kapazitaet, kaufauftraege, sprache);
                this.Controls.Add(ergebnis);
            }
            else
            {
                MessageBox.Show("Ein Wert fehlt! Bitte prüfen Sie Ihre Eingaben.");
            }
        }

        public void sprachen()
        {
            if (pic_en.SizeMode == PictureBoxSizeMode.StretchImage | sprache != "de")
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
                default_btn.Text = (Sprachen.EN_BTN_DEFAULT);

                //EN Groupboxen
                groupBox1.Text = (Sprachen.EN_KD_GROUPBOX1);
                
                //EN Labels
                labelDiskont1.Text = (Sprachen.EN_LBL_KD_DISKONT);
                labelMM1.Text = (Sprachen.EN_LBL_KD_MM);
                labelOP1.Text = (Sprachen.EN_LBL_KD_OP);
                labelBM1.Text = (Sprachen.EN_LBL_KD_BM);
                labelBA1.Text = (Sprachen.EN_LBL_KD_BA);

                labelDiskont2.Text = (Sprachen.EN_LBL_KD_DISKONT);
                labelMM2.Text = (Sprachen.EN_LBL_KD_MM);
                labelOP2.Text = (Sprachen.EN_LBL_KD_OP);
                labelBM2.Text = (Sprachen.EN_LBL_KD_BM);
                labelBA2.Text = (Sprachen.EN_LBL_KD_BA);

                labelDiskont3.Text = (Sprachen.EN_LBL_KD_DISKONT);
                labelMM3.Text = (Sprachen.EN_LBL_KD_MM);
                labelOP3.Text = (Sprachen.EN_LBL_KD_OP);
                labelBM3.Text = (Sprachen.EN_LBL_KD_BM);
                labelBA3.Text = (Sprachen.EN_LBL_KD_BA);

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
                continue_btn.Text = (Sprachen.DE_BTN_CONTINUE);
                back_btn.Text = (Sprachen.DE_BTN_BACK);
                default_btn.Text = (Sprachen.DE_BTN_DEFAULT);

                //DE Groupboxen
                groupBox1.Text = (Sprachen.DE_KD_GROUPBOX1);

                //DE Labels
                labelDiskont1.Text = (Sprachen.DE_LBL_KD_DISKONT);
                labelMM1.Text = (Sprachen.DE_LBL_KD_MM);
                labelOP1.Text = (Sprachen.DE_LBL_KD_OP);
                labelBM1.Text = (Sprachen.DE_LBL_KD_BM);
                labelBA1.Text = (Sprachen.DE_LBL_KD_BA);

                labelDiskont2.Text = (Sprachen.DE_LBL_KD_DISKONT);
                labelMM2.Text = (Sprachen.DE_LBL_KD_MM);
                labelOP2.Text = (Sprachen.DE_LBL_KD_OP);
                labelBM2.Text = (Sprachen.DE_LBL_KD_BM);
                labelBA2.Text = (Sprachen.DE_LBL_KD_BA);

                labelDiskont3.Text = (Sprachen.DE_LBL_KD_DISKONT);
                labelMM3.Text = (Sprachen.DE_LBL_KD_MM);
                labelOP3.Text = (Sprachen.DE_LBL_KD_OP);
                labelBM3.Text = (Sprachen.DE_LBL_KD_BM);
                labelBA3.Text = (Sprachen.DE_LBL_KD_BA);

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
            sprache = "en";
        }

        private void pic_de_Click(object sender, EventArgs e)
        {
            pic_de.SizeMode = PictureBoxSizeMode.StretchImage;
            pic_en.SizeMode = PictureBoxSizeMode.Normal;
            sprachen();
            sprache = "de";
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
                sicherheitsbest, produktion, produktionProg, prodReihenfolge, kapazitaet, kaufauftraege, sprache);
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
                sicherheitsbest, produktion, produktionProg, prodReihenfolge, kapazitaet, kaufauftraege, sprache);
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
                sicherheitsbest, produktion, produktionProg, prodReihenfolge, kapazitaet, kaufauftraege, sprache);
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
                sicherheitsbest, produktion, produktionProg, prodReihenfolge, kapazitaet, kaufauftraege, sprache);
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
                sicherheitsbest, produktion, produktionProg, prodReihenfolge, kapazitaet, kaufauftraege, sprache);
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
                sicherheitsbest, produktion, produktionProg, prodReihenfolge, kapazitaet, kaufauftraege, sprache);
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
                sicherheitsbest, produktion, produktionProg, prodReihenfolge, kapazitaet, kaufauftraege, sprache);
            this.Controls.Add(ergebnis);
        }

        private void BM1_TextChanged(object sender, EventArgs e)
        {
                BM1.ForeColor = Color.Black;
                bool okay = true;
                //neuer Text darf nur Zeichen aus der Liste digits (in der Klasse deklariert)
                foreach (char c in BM1.Text.ToCharArray())
                {
                    //sobald es ein unpassendes Zeichen gibt, aufhoeren und Fehlermeldung ausgeben
                    if (!digits.Contains<char>(c))
                    {
                        BM1.ForeColor = Color.Red;
                        okay = false;
                        tBM1 = false;
                        continue_btn.Enabled = false;
                        back_btn.Enabled = false;
                        break;
                    }

                }
                if (okay == true)
                {
                    BM1.ForeColor = Color.Black;
                    tBM1 = true;
                    if (tB1 & tB2 & tB3 & tB4 & tB5 & tB6 & tB7 & tB8 & tB9 & tB10 & tB11 & tB12 & tB13 & tB14 & tB15 & tB16 & tB17 & tB18 & tB19 & tB20 & tB21 & tB22 & tB23 & tB24 & tB25 & tB26 & tB27 & tB28 & tB29 & tBM1 & tBM2 & tBM3 & tBM4 & tBM5 & tBM6 & tBM7 & tBM8 & tBM9 & tBM10 & tBM11 & tBM12 & tBM13 & tBM14 & tBM15 & tBM16 & tBM17 & tBM18 & tBM19 & tBM20 & tBM21 & tBM22 & tBM23 & tBM24 & tBM25 & tBM26 & tBM27 & tBM28 & tBM29)
                    {
                        continue_btn.Enabled = true;
                        back_btn.Enabled = true;
                    }
                    else
                    {
                        continue_btn.Enabled = false;
                        back_btn.Enabled = false;
                    }
                }
            
        }

        private void BM2_TextChanged(object sender, EventArgs e)
        {
                BM2.ForeColor = Color.Black;
                bool okay = true;
                //neuer Text darf nur Zeichen aus der Liste digits (in der Klasse deklariert)
                foreach (char c in BM2.Text.ToCharArray())
                {
                    //sobald es ein unpassendes Zeichen gibt, aufhoeren und Fehlermeldung ausgeben
                    if (!digits.Contains<char>(c))
                    {
                        BM2.ForeColor = Color.Red;
                        okay = false;
                        tBM2 = false;
                        continue_btn.Enabled = false;
                        back_btn.Enabled = false;
                        break;
                    }
                }
                if (okay == true)
                {
                    BM2.ForeColor = Color.Black;
                    tBM2 = true;
                    if (tBM1 & tBM2 & tBM3 & tBM4 & tBM5 & tBM6 & tBM7 & tBM8 & tBM9 & tBM10 & tBM11 & tBM12 & tBM13 & tBM14 & tBM15 & tBM16 & tBM17 & tBM18 & tBM19 & tBM20 & tBM21 & tBM22 & tBM23 & tBM24 & tBM25 & tBM26 & tBM27 & tBM28 & tBM29)
                    {
                        continue_btn.Enabled = true;
                        back_btn.Enabled = true;
                    }
                    else
                    {
                        continue_btn.Enabled = false;
                        back_btn.Enabled = false;
                    }
                }
            
        }

        private void BM3_TextChanged(object sender, EventArgs e)
        {
                BM3.ForeColor = Color.Black;
                bool okay = true;
                //neuer Text darf nur Zeichen aus der Liste digits (in der Klasse deklariert)
                foreach (char c in BM3.Text.ToCharArray())
                {
                    //sobald es ein unpassendes Zeichen gibt, aufhoeren und Fehlermeldung ausgeben
                    if (!digits.Contains<char>(c))
                    {
                        BM3.ForeColor = Color.Red;
                        okay = false;
                        tBM3 = false;
                        continue_btn.Enabled = false;
                        back_btn.Enabled = false;
                        break;
                    }
                }
                if (okay == true)
                {
                    BM3.ForeColor = Color.Black;
                    tBM3 = true;
                    if (tB1 & tB2 & tB3 & tB4 & tB5 & tB6 & tB7 & tB8 & tB9 & tB10 & tB11 & tB12 & tB13 & tB14 & tB15 & tB16 & tB17 & tB18 & tB19 & tB20 & tB21 & tB22 & tB23 & tB24 & tB25 & tB26 & tB27 & tB28 & tB29 & tBM1 & tBM2 & tBM3 & tBM4 & tBM5 & tBM6 & tBM7 & tBM8 & tBM9 & tBM10 & tBM11 & tBM12 & tBM13 & tBM14 & tBM15 & tBM16 & tBM17 & tBM18 & tBM19 & tBM20 & tBM21 & tBM22 & tBM23 & tBM24 & tBM25 & tBM26 & tBM27 & tBM28 & tBM29)
                    {
                        continue_btn.Enabled = true;
                        back_btn.Enabled = true;
                    }
                    else
                    {
                        continue_btn.Enabled = false;
                        back_btn.Enabled = false;
                    }
                }
            
        }

        private void BM4_TextChanged(object sender, EventArgs e)
        {
            if (BM4.Text == "")
            {
                continue_btn.Enabled = false;
                back_btn.Enabled = false;
                tBM4 = false;
            }
            else
            {
                BM4.ForeColor = Color.Black;
                bool okay = true;
                //neuer Text darf nur Zeichen aus der Liste digits (in der Klasse deklariert)
                foreach (char c in BM4.Text.ToCharArray())
                {
                    //sobald es ein unpassendes Zeichen gibt, aufhoeren und Fehlermeldung ausgeben
                    if (!digits.Contains<char>(c))
                    {
                        BM4.ForeColor = Color.Red;
                        okay = false;
                        tBM4 = false;
                        continue_btn.Enabled = false;
                        back_btn.Enabled = false;
                        break;
                    }
                }
                if (okay == true)
                {
                    BM4.ForeColor = Color.Black;
                    tBM4 = true;
                    if (tB1 & tB2 & tB3 & tB4 & tB5 & tB6 & tB7 & tB8 & tB9 & tB10 & tB11 & tB12 & tB13 & tB14 & tB15 & tB16 & tB17 & tB18 & tB19 & tB20 & tB21 & tB22 & tB23 & tB24 & tB25 & tB26 & tB27 & tB28 & tB29 & tBM1 & tBM2 & tBM3 & tBM4 & tBM5 & tBM6 & tBM7 & tBM8 & tBM9 & tBM10 & tBM11 & tBM12 & tBM13 & tBM14 & tBM15 & tBM16 & tBM17 & tBM18 & tBM19 & tBM20 & tBM21 & tBM22 & tBM23 & tBM24 & tBM25 & tBM26 & tBM27 & tBM28 & tBM29)
                    {
                        continue_btn.Enabled = true;
                        back_btn.Enabled = true;
                    }
                    else
                    {
                        continue_btn.Enabled = false;
                        back_btn.Enabled = false;
                    }
                }
            }
        }

        private void BM5_TextChanged(object sender, EventArgs e)
        {
                BM5.ForeColor = Color.Black;
                bool okay = true;
                //neuer Text darf nur Zeichen aus der Liste digits (in der Klasse deklariert)
                foreach (char c in BM5.Text.ToCharArray())
                {
                    //sobald es ein unpassendes Zeichen gibt, aufhoeren und Fehlermeldung ausgeben
                    if (!digits.Contains<char>(c))
                    {
                        BM5.ForeColor = Color.Red;
                        okay = false;
                        tBM5 = false;
                        continue_btn.Enabled = false;
                        back_btn.Enabled = false;
                        break;
                    }
                }
                if (okay == true)
                {
                    BM5.ForeColor = Color.Black;
                    tBM5 = true;
                    if (tB1 & tB2 & tB3 & tB4 & tB5 & tB6 & tB7 & tB8 & tB9 & tB10 & tB11 & tB12 & tB13 & tB14 & tB15 & tB16 & tB17 & tB18 & tB19 & tB20 & tB21 & tB22 & tB23 & tB24 & tB25 & tB26 & tB27 & tB28 & tB29 & tBM1 & tBM2 & tBM3 & tBM4 & tBM5 & tBM6 & tBM7 & tBM8 & tBM9 & tBM10 & tBM11 & tBM12 & tBM13 & tBM14 & tBM15 & tBM16 & tBM17 & tBM18 & tBM19 & tBM20 & tBM21 & tBM22 & tBM23 & tBM24 & tBM25 & tBM26 & tBM27 & tBM28 & tBM29)
                    {
                        continue_btn.Enabled = true;
                        back_btn.Enabled = true;
                    }
                    else
                    {
                        continue_btn.Enabled = false;
                        back_btn.Enabled = false;
                    }
                }
            
        }

        private void BM6_TextChanged(object sender, EventArgs e)
        {
                BM6.ForeColor = Color.Black;
                bool okay = true;
                //neuer Text darf nur Zeichen aus der Liste digits (in der Klasse deklariert)
                foreach (char c in BM6.Text.ToCharArray())
                {
                    //sobald es ein unpassendes Zeichen gibt, aufhoeren und Fehlermeldung ausgeben
                    if (!digits.Contains<char>(c))
                    {
                        BM6.ForeColor = Color.Red;
                        okay = false;
                        tBM6 = false;
                        continue_btn.Enabled = false;
                        back_btn.Enabled = false;
                        break;
                    }
                }
                if (okay == true)
                {
                    BM6.ForeColor = Color.Black;
                    tBM6 = true;
                    if (tB1 & tB2 & tB3 & tB4 & tB5 & tB6 & tB7 & tB8 & tB9 & tB10 & tB11 & tB12 & tB13 & tB14 & tB15 & tB16 & tB17 & tB18 & tB19 & tB20 & tB21 & tB22 & tB23 & tB24 & tB25 & tB26 & tB27 & tB28 & tB29 & tBM1 & tBM2 & tBM3 & tBM4 & tBM5 & tBM6 & tBM7 & tBM8 & tBM9 & tBM10 & tBM11 & tBM12 & tBM13 & tBM14 & tBM15 & tBM16 & tBM17 & tBM18 & tBM19 & tBM20 & tBM21 & tBM22 & tBM23 & tBM24 & tBM25 & tBM26 & tBM27 & tBM28 & tBM29)
                    {
                        continue_btn.Enabled = true;
                        back_btn.Enabled = true;
                    }
                    else
                    {
                        continue_btn.Enabled = false;
                        back_btn.Enabled = false;
                    }
                }
            
        }

        private void BM7_TextChanged(object sender, EventArgs e)
        {
                BM7.ForeColor = Color.Black;
                bool okay = true;
                //neuer Text darf nur Zeichen aus der Liste digits (in der Klasse deklariert)
                foreach (char c in BM7.Text.ToCharArray())
                {
                    //sobald es ein unpassendes Zeichen gibt, aufhoeren und Fehlermeldung ausgeben
                    if (!digits.Contains<char>(c))
                    {
                        BM7.ForeColor = Color.Red;
                        okay = false;
                        tBM7 = false;
                        continue_btn.Enabled = false;
                        back_btn.Enabled = false;
                        break;
                    }
                }
                if (okay == true)
                {
                    BM7.ForeColor = Color.Black;
                    tBM7 = true;
                    if (tB1 & tB2 & tB3 & tB4 & tB5 & tB6 & tB7 & tB8 & tB9 & tB10 & tB11 & tB12 & tB13 & tB14 & tB15 & tB16 & tB17 & tB18 & tB19 & tB20 & tB21 & tB22 & tB23 & tB24 & tB25 & tB26 & tB27 & tB28 & tB29 & tBM1 & tBM2 & tBM3 & tBM4 & tBM5 & tBM6 & tBM7 & tBM8 & tBM9 & tBM10 & tBM11 & tBM12 & tBM13 & tBM14 & tBM15 & tBM16 & tBM17 & tBM18 & tBM19 & tBM20 & tBM21 & tBM22 & tBM23 & tBM24 & tBM25 & tBM26 & tBM27 & tBM28 & tBM29)
                    {
                        continue_btn.Enabled = true;
                        back_btn.Enabled = true;
                    }
                    else
                    {
                        continue_btn.Enabled = false;
                        back_btn.Enabled = false;
                    }
                }
            
        }

        private void BM8_TextChanged(object sender, EventArgs e)
        {
                BM8.ForeColor = Color.Black;
                bool okay = true;
                //neuer Text darf nur Zeichen aus der Liste digits (in der Klasse deklariert)
                foreach (char c in BM8.Text.ToCharArray())
                {
                    //sobald es ein unpassendes Zeichen gibt, aufhoeren und Fehlermeldung ausgeben
                    if (!digits.Contains<char>(c))
                    {
                        BM8.ForeColor = Color.Red;
                        okay = false;
                        tBM8 = false;
                        continue_btn.Enabled = false;
                        back_btn.Enabled = false;
                        break;
                    }
                }
                if (okay == true)
                {
                    BM8.ForeColor = Color.Black;
                    tBM8 = true;
                    if (tB1 & tB2 & tB3 & tB4 & tB5 & tB6 & tB7 & tB8 & tB9 & tB10 & tB11 & tB12 & tB13 & tB14 & tB15 & tB16 & tB17 & tB18 & tB19 & tB20 & tB21 & tB22 & tB23 & tB24 & tB25 & tB26 & tB27 & tB28 & tB29 & tBM1 & tBM2 & tBM3 & tBM4 & tBM5 & tBM6 & tBM7 & tBM8 & tBM9 & tBM10 & tBM11 & tBM12 & tBM13 & tBM14 & tBM15 & tBM16 & tBM17 & tBM18 & tBM19 & tBM20 & tBM21 & tBM22 & tBM23 & tBM24 & tBM25 & tBM26 & tBM27 & tBM28 & tBM29)
                    {
                        continue_btn.Enabled = true;
                        back_btn.Enabled = true;
                    }
                    else
                    {
                        continue_btn.Enabled = false;
                        back_btn.Enabled = false;
                    }
                }
            
        }

        private void BM9_TextChanged(object sender, EventArgs e)
        {
                BM9.ForeColor = Color.Black;
                bool okay = true;
                //neuer Text darf nur Zeichen aus der Liste digits (in der Klasse deklariert)
                foreach (char c in BM9.Text.ToCharArray())
                {
                    //sobald es ein unpassendes Zeichen gibt, aufhoeren und Fehlermeldung ausgeben
                    if (!digits.Contains<char>(c))
                    {
                        BM9.ForeColor = Color.Red;
                        okay = false;
                        tBM9 = false;
                        continue_btn.Enabled = false;
                        back_btn.Enabled = false;
                        break;
                    }
                }
                if (okay == true)
                {
                    BM9.ForeColor = Color.Black;
                    tBM9 = true;
                    if (tB1 & tB2 & tB3 & tB4 & tB5 & tB6 & tB7 & tB8 & tB9 & tB10 & tB11 & tB12 & tB13 & tB14 & tB15 & tB16 & tB17 & tB18 & tB19 & tB20 & tB21 & tB22 & tB23 & tB24 & tB25 & tB26 & tB27 & tB28 & tB29 & tBM1 & tBM2 & tBM3 & tBM4 & tBM5 & tBM6 & tBM7 & tBM8 & tBM9 & tBM10 & tBM11 & tBM12 & tBM13 & tBM14 & tBM15 & tBM16 & tBM17 & tBM18 & tBM19 & tBM20 & tBM21 & tBM22 & tBM23 & tBM24 & tBM25 & tBM26 & tBM27 & tBM28 & tBM29)
                    {
                        continue_btn.Enabled = true;
                        back_btn.Enabled = true;
                    }
                    else
                    {
                        continue_btn.Enabled = false;
                        back_btn.Enabled = false;
                    }
                }
            
        }

        private void BM10_TextChanged(object sender, EventArgs e)
        {
                BM10.ForeColor = Color.Black;
                bool okay = true;
                //neuer Text darf nur Zeichen aus der Liste digits (in der Klasse deklariert)
                foreach (char c in BM10.Text.ToCharArray())
                {
                    //sobald es ein unpassendes Zeichen gibt, aufhoeren und Fehlermeldung ausgeben
                    if (!digits.Contains<char>(c))
                    {
                        BM10.ForeColor = Color.Red;
                        okay = false;
                        tBM10 = false;
                        continue_btn.Enabled = false;
                        back_btn.Enabled = false;
                        break;
                    }
                }
                if (okay == true)
                {
                    BM10.ForeColor = Color.Black;
                    tBM10 = true;
                    if (tB1 & tB2 & tB3 & tB4 & tB5 & tB6 & tB7 & tB8 & tB9 & tB10 & tB11 & tB12 & tB13 & tB14 & tB15 & tB16 & tB17 & tB18 & tB19 & tB20 & tB21 & tB22 & tB23 & tB24 & tB25 & tB26 & tB27 & tB28 & tB29 & tBM1 & tBM2 & tBM3 & tBM4 & tBM5 & tBM6 & tBM7 & tBM8 & tBM9 & tBM10 & tBM11 & tBM12 & tBM13 & tBM14 & tBM15 & tBM16 & tBM17 & tBM18 & tBM19 & tBM20 & tBM21 & tBM22 & tBM23 & tBM24 & tBM25 & tBM26 & tBM27 & tBM28 & tBM29)
                    {
                        continue_btn.Enabled = true;
                        back_btn.Enabled = true;
                    }
                    else
                    {
                        continue_btn.Enabled = false;
                        back_btn.Enabled = false;
                    }
                }
            
        }

        private void BM11_TextChanged(object sender, EventArgs e)
        {
                BM11.ForeColor = Color.Black;
                bool okay = true;
                //neuer Text darf nur Zeichen aus der Liste digits (in der Klasse deklariert)
                foreach (char c in BM11.Text.ToCharArray())
                {
                    //sobald es ein unpassendes Zeichen gibt, aufhoeren und Fehlermeldung ausgeben
                    if (!digits.Contains<char>(c))
                    {
                        BM11.ForeColor = Color.Red;
                        okay = false;
                        tBM11 = false;
                        continue_btn.Enabled = false;
                        back_btn.Enabled = false;
                        break;
                    }
                }
                if (okay == true)
                {
                    BM11.ForeColor = Color.Black;
                    tBM11 = true;
                    if (tB1 & tB2 & tB3 & tB4 & tB5 & tB6 & tB7 & tB8 & tB9 & tB10 & tB11 & tB12 & tB13 & tB14 & tB15 & tB16 & tB17 & tB18 & tB19 & tB20 & tB21 & tB22 & tB23 & tB24 & tB25 & tB26 & tB27 & tB28 & tB29 & tBM1 & tBM2 & tBM3 & tBM4 & tBM5 & tBM6 & tBM7 & tBM8 & tBM9 & tBM10 & tBM11 & tBM12 & tBM13 & tBM14 & tBM15 & tBM16 & tBM17 & tBM18 & tBM19 & tBM20 & tBM21 & tBM22 & tBM23 & tBM24 & tBM25 & tBM26 & tBM27 & tBM28 & tBM29)
                    {
                        continue_btn.Enabled = true;
                        back_btn.Enabled = true;
                    }
                    else
                    {
                        continue_btn.Enabled = false;
                        back_btn.Enabled = false;
                    }
                }
            
        }

        private void BM12_TextChanged(object sender, EventArgs e)
        {
                BM12.ForeColor = Color.Black;
                bool okay = true;
                //neuer Text darf nur Zeichen aus der Liste digits (in der Klasse deklariert)
                foreach (char c in BM12.Text.ToCharArray())
                {
                    //sobald es ein unpassendes Zeichen gibt, aufhoeren und Fehlermeldung ausgeben
                    if (!digits.Contains<char>(c))
                    {
                        BM12.ForeColor = Color.Red;
                        okay = false;
                        tBM12 = false;
                        continue_btn.Enabled = false;
                        back_btn.Enabled = false;
                        break;
                    }
                }
                if (okay == true)
                {
                    BM12.ForeColor = Color.Black;
                    tBM12 = true;
                    if (tB1 & tB2 & tB3 & tB4 & tB5 & tB6 & tB7 & tB8 & tB9 & tB10 & tB11 & tB12 & tB13 & tB14 & tB15 & tB16 & tB17 & tB18 & tB19 & tB20 & tB21 & tB22 & tB23 & tB24 & tB25 & tB26 & tB27 & tB28 & tB29 & tBM1 & tBM2 & tBM3 & tBM4 & tBM5 & tBM6 & tBM7 & tBM8 & tBM9 & tBM10 & tBM11 & tBM12 & tBM13 & tBM14 & tBM15 & tBM16 & tBM17 & tBM18 & tBM19 & tBM20 & tBM21 & tBM22 & tBM23 & tBM24 & tBM25 & tBM26 & tBM27 & tBM28 & tBM29)
                    {
                        continue_btn.Enabled = true;
                        back_btn.Enabled = true;
                    }
                    else
                    {
                        continue_btn.Enabled = false;
                        back_btn.Enabled = false;
                    }
                }
            
        }

        private void BM13_TextChanged(object sender, EventArgs e)
        {
                BM13.ForeColor = Color.Black;
                bool okay = true;
                //neuer Text darf nur Zeichen aus der Liste digits (in der Klasse deklariert)
                foreach (char c in BM13.Text.ToCharArray())
                {
                    //sobald es ein unpassendes Zeichen gibt, aufhoeren und Fehlermeldung ausgeben
                    if (!digits.Contains<char>(c))
                    {
                        BM13.ForeColor = Color.Red;
                        okay = false;
                        tBM13 = false;
                        continue_btn.Enabled = false;
                        back_btn.Enabled = false;
                        break;
                    }
                }
                if (okay == true)
                {
                    BM13.ForeColor = Color.Black;
                    tBM13 = true;
                    if (tB1 & tB2 & tB3 & tB4 & tB5 & tB6 & tB7 & tB8 & tB9 & tB10 & tB11 & tB12 & tB13 & tB14 & tB15 & tB16 & tB17 & tB18 & tB19 & tB20 & tB21 & tB22 & tB23 & tB24 & tB25 & tB26 & tB27 & tB28 & tB29 & tBM1 & tBM2 & tBM3 & tBM4 & tBM5 & tBM6 & tBM7 & tBM8 & tBM9 & tBM10 & tBM11 & tBM12 & tBM13 & tBM14 & tBM15 & tBM16 & tBM17 & tBM18 & tBM19 & tBM20 & tBM21 & tBM22 & tBM23 & tBM24 & tBM25 & tBM26 & tBM27 & tBM28 & tBM29)
                    {
                        continue_btn.Enabled = true;
                        back_btn.Enabled = true;
                    }
                    else
                    {
                        continue_btn.Enabled = false;
                        back_btn.Enabled = false;
                    }
                }
            
        }

        private void BM14_TextChanged(object sender, EventArgs e)
        {
                BM14.ForeColor = Color.Black;
                bool okay = true;
                //neuer Text darf nur Zeichen aus der Liste digits (in der Klasse deklariert)
                foreach (char c in BM14.Text.ToCharArray())
                {
                    //sobald es ein unpassendes Zeichen gibt, aufhoeren und Fehlermeldung ausgeben
                    if (!digits.Contains<char>(c))
                    {
                        BM14.ForeColor = Color.Red;
                        okay = false;
                        tBM14 = false;
                        continue_btn.Enabled = false;
                        back_btn.Enabled = false;
                        break;
                    }
                }
                if (okay == true)
                {
                    BM14.ForeColor = Color.Black;
                    tBM14 = true;
                    if (tB1 & tB2 & tB3 & tB4 & tB5 & tB6 & tB7 & tB8 & tB9 & tB10 & tB11 & tB12 & tB13 & tB14 & tB15 & tB16 & tB17 & tB18 & tB19 & tB20 & tB21 & tB22 & tB23 & tB24 & tB25 & tB26 & tB27 & tB28 & tB29 & tBM1 & tBM2 & tBM3 & tBM4 & tBM5 & tBM6 & tBM7 & tBM8 & tBM9 & tBM10 & tBM11 & tBM12 & tBM13 & tBM14 & tBM15 & tBM16 & tBM17 & tBM18 & tBM19 & tBM20 & tBM21 & tBM22 & tBM23 & tBM24 & tBM25 & tBM26 & tBM27 & tBM28 & tBM29)
                    {
                        continue_btn.Enabled = true;
                        back_btn.Enabled = true;
                    }
                    else
                    {
                        continue_btn.Enabled = false;
                        back_btn.Enabled = false;
                    }
                }
            
        }

        private void BM15_TextChanged(object sender, EventArgs e)
        {
                BM15.ForeColor = Color.Black;
                bool okay = true;
                //neuer Text darf nur Zeichen aus der Liste digits (in der Klasse deklariert)
                foreach (char c in BM15.Text.ToCharArray())
                {
                    //sobald es ein unpassendes Zeichen gibt, aufhoeren und Fehlermeldung ausgeben
                    if (!digits.Contains<char>(c))
                    {
                        BM15.ForeColor = Color.Red;
                        okay = false;
                        tBM15 = false;
                        continue_btn.Enabled = false;
                        back_btn.Enabled = false;
                        break;
                    }
                }
                if (okay == true)
                {
                    BM15.ForeColor = Color.Black;
                    tBM15 = true;
                    if (tB1 & tB2 & tB3 & tB4 & tB5 & tB6 & tB7 & tB8 & tB9 & tB10 & tB11 & tB12 & tB13 & tB14 & tB15 & tB16 & tB17 & tB18 & tB19 & tB20 & tB21 & tB22 & tB23 & tB24 & tB25 & tB26 & tB27 & tB28 & tB29 & tBM1 & tBM2 & tBM3 & tBM4 & tBM5 & tBM6 & tBM7 & tBM8 & tBM9 & tBM10 & tBM11 & tBM12 & tBM13 & tBM14 & tBM15 & tBM16 & tBM17 & tBM18 & tBM19 & tBM20 & tBM21 & tBM22 & tBM23 & tBM24 & tBM25 & tBM26 & tBM27 & tBM28 & tBM29)
                    {
                        continue_btn.Enabled = true;
                        back_btn.Enabled = true;
                    }
                    else
                    {
                        continue_btn.Enabled = false;
                        back_btn.Enabled = false;
                    }
                }
            
        }

        private void BM16_TextChanged(object sender, EventArgs e)
        {
                BM16.ForeColor = Color.Black;
                bool okay = true;
                //neuer Text darf nur Zeichen aus der Liste digits (in der Klasse deklariert)
                foreach (char c in BM16.Text.ToCharArray())
                {
                    //sobald es ein unpassendes Zeichen gibt, aufhoeren und Fehlermeldung ausgeben
                    if (!digits.Contains<char>(c))
                    {
                        BM16.ForeColor = Color.Red;
                        okay = false;
                        tBM16 = false;
                        continue_btn.Enabled = false;
                        back_btn.Enabled = false;
                        break;
                    }
                }
                if (okay == true)
                {
                    BM16.ForeColor = Color.Black;
                    tBM16 = true;
                    if (tB1 & tB2 & tB3 & tB4 & tB5 & tB6 & tB7 & tB8 & tB9 & tB10 & tB11 & tB12 & tB13 & tB14 & tB15 & tB16 & tB17 & tB18 & tB19 & tB20 & tB21 & tB22 & tB23 & tB24 & tB25 & tB26 & tB27 & tB28 & tB29 & tBM1 & tBM2 & tBM3 & tBM4 & tBM5 & tBM6 & tBM7 & tBM8 & tBM9 & tBM10 & tBM11 & tBM12 & tBM13 & tBM14 & tBM15 & tBM16 & tBM17 & tBM18 & tBM19 & tBM20 & tBM21 & tBM22 & tBM23 & tBM24 & tBM25 & tBM26 & tBM27 & tBM28 & tBM29)
                    {
                        continue_btn.Enabled = true;
                        back_btn.Enabled = true;
                    }
                    else
                    {
                        continue_btn.Enabled = false;
                        back_btn.Enabled = false;
                    }
                }
            
        }

        private void BM17_TextChanged(object sender, EventArgs e)
        {
                BM17.ForeColor = Color.Black;
                bool okay = true;
                //neuer Text darf nur Zeichen aus der Liste digits (in der Klasse deklariert)
                foreach (char c in BM17.Text.ToCharArray())
                {
                    //sobald es ein unpassendes Zeichen gibt, aufhoeren und Fehlermeldung ausgeben
                    if (!digits.Contains<char>(c))
                    {
                        BM17.ForeColor = Color.Red;
                        okay = false;
                        tBM17 = false;
                        continue_btn.Enabled = false;
                        back_btn.Enabled = false;
                        break;
                    }
                }
                if (okay == true)
                {
                    BM17.ForeColor = Color.Black;
                    tBM17 = true;
                    if (tB1 & tB2 & tB3 & tB4 & tB5 & tB6 & tB7 & tB8 & tB9 & tB10 & tB11 & tB12 & tB13 & tB14 & tB15 & tB16 & tB17 & tB18 & tB19 & tB20 & tB21 & tB22 & tB23 & tB24 & tB25 & tB26 & tB27 & tB28 & tB29 & tBM1 & tBM2 & tBM3 & tBM4 & tBM5 & tBM6 & tBM7 & tBM8 & tBM9 & tBM10 & tBM11 & tBM12 & tBM13 & tBM14 & tBM15 & tBM16 & tBM17 & tBM18 & tBM19 & tBM20 & tBM21 & tBM22 & tBM23 & tBM24 & tBM25 & tBM26 & tBM27 & tBM28 & tBM29)
                    {
                        continue_btn.Enabled = true;
                        back_btn.Enabled = true;
                    }
                    else
                    {
                        continue_btn.Enabled = false;
                        back_btn.Enabled = false;
                    }
                }
            
        }

        private void BM18_TextChanged(object sender, EventArgs e)
        {
                BM18.ForeColor = Color.Black;
                bool okay = true;
                //neuer Text darf nur Zeichen aus der Liste digits (in der Klasse deklariert)
                foreach (char c in BM18.Text.ToCharArray())
                {
                    //sobald es ein unpassendes Zeichen gibt, aufhoeren und Fehlermeldung ausgeben
                    if (!digits.Contains<char>(c))
                    {
                        BM18.ForeColor = Color.Red;
                        okay = false;
                        tBM18 = false;
                        continue_btn.Enabled = false;
                        back_btn.Enabled = false;
                        break;
                    }
                }
                if (okay == true)
                {
                    BM18.ForeColor = Color.Black;
                    tBM18 = true;
                    if (tB1 & tB2 & tB3 & tB4 & tB5 & tB6 & tB7 & tB8 & tB9 & tB10 & tB11 & tB12 & tB13 & tB14 & tB15 & tB16 & tB17 & tB18 & tB19 & tB20 & tB21 & tB22 & tB23 & tB24 & tB25 & tB26 & tB27 & tB28 & tB29 & tBM1 & tBM2 & tBM3 & tBM4 & tBM5 & tBM6 & tBM7 & tBM8 & tBM9 & tBM10 & tBM11 & tBM12 & tBM13 & tBM14 & tBM15 & tBM16 & tBM17 & tBM18 & tBM19 & tBM20 & tBM21 & tBM22 & tBM23 & tBM24 & tBM25 & tBM26 & tBM27 & tBM28 & tBM29)
                    {
                        continue_btn.Enabled = true;
                        back_btn.Enabled = true;
                    }
                    else
                    {
                        continue_btn.Enabled = false;
                        back_btn.Enabled = false;
                    }
                }
            
        }

        private void BM19_TextChanged(object sender, EventArgs e)
        {
                BM19.ForeColor = Color.Black;
                bool okay = true;
                //neuer Text darf nur Zeichen aus der Liste digits (in der Klasse deklariert)
                foreach (char c in BM19.Text.ToCharArray())
                {
                    //sobald es ein unpassendes Zeichen gibt, aufhoeren und Fehlermeldung ausgeben
                    if (!digits.Contains<char>(c))
                    {
                        BM19.ForeColor = Color.Red;
                        okay = false;
                        tBM19 = false;
                        continue_btn.Enabled = false;
                        back_btn.Enabled = false;
                        break;
                    }
                }
                if (okay == true)
                {
                    BM19.ForeColor = Color.Black;
                    tBM19 = true;
                    if (tB1 & tB2 & tB3 & tB4 & tB5 & tB6 & tB7 & tB8 & tB9 & tB10 & tB11 & tB12 & tB13 & tB14 & tB15 & tB16 & tB17 & tB18 & tB19 & tB20 & tB21 & tB22 & tB23 & tB24 & tB25 & tB26 & tB27 & tB28 & tB29 & tBM1 & tBM2 & tBM3 & tBM4 & tBM5 & tBM6 & tBM7 & tBM8 & tBM9 & tBM10 & tBM11 & tBM12 & tBM13 & tBM14 & tBM15 & tBM16 & tBM17 & tBM18 & tBM19 & tBM20 & tBM21 & tBM22 & tBM23 & tBM24 & tBM25 & tBM26 & tBM27 & tBM28 & tBM29)
                    {
                        continue_btn.Enabled = true;
                        back_btn.Enabled = true;
                    }
                    else
                    {
                        continue_btn.Enabled = false;
                        back_btn.Enabled = false;
                    }
                }
            
        }

        private void BM20_TextChanged(object sender, EventArgs e)
        {
                BM20.ForeColor = Color.Black;
                bool okay = true;
                //neuer Text darf nur Zeichen aus der Liste digits (in der Klasse deklariert)
                foreach (char c in BM20.Text.ToCharArray())
                {
                    //sobald es ein unpassendes Zeichen gibt, aufhoeren und Fehlermeldung ausgeben
                    if (!digits.Contains<char>(c))
                    {
                        BM20.ForeColor = Color.Red;
                        okay = false;
                        tBM20 = false;
                        continue_btn.Enabled = false;
                        back_btn.Enabled = false;
                        break;
                    }
                }
                if (okay == true)
                {
                    BM20.ForeColor = Color.Black;
                    tBM20 = true;
                    if (tB1 & tB2 & tB3 & tB4 & tB5 & tB6 & tB7 & tB8 & tB9 & tB10 & tB11 & tB12 & tB13 & tB14 & tB15 & tB16 & tB17 & tB18 & tB19 & tB20 & tB21 & tB22 & tB23 & tB24 & tB25 & tB26 & tB27 & tB28 & tB29 & tBM1 & tBM2 & tBM3 & tBM4 & tBM5 & tBM6 & tBM7 & tBM8 & tBM9 & tBM10 & tBM11 & tBM12 & tBM13 & tBM14 & tBM15 & tBM16 & tBM17 & tBM18 & tBM19 & tBM20 & tBM21 & tBM22 & tBM23 & tBM24 & tBM25 & tBM26 & tBM27 & tBM28 & tBM29)
                    {
                        continue_btn.Enabled = true;
                        back_btn.Enabled = true;
                    }
                    else
                    {
                        continue_btn.Enabled = false;
                        back_btn.Enabled = false;
                    }
                }
            
        }

        private void BM21_TextChanged(object sender, EventArgs e)
        {
                BM21.ForeColor = Color.Black;
                bool okay = true;
                //neuer Text darf nur Zeichen aus der Liste digits (in der Klasse deklariert)
                foreach (char c in BM21.Text.ToCharArray())
                {
                    //sobald es ein unpassendes Zeichen gibt, aufhoeren und Fehlermeldung ausgeben
                    if (!digits.Contains<char>(c))
                    {
                        BM21.ForeColor = Color.Red;
                        okay = false;
                        tBM21 = false;
                        continue_btn.Enabled = false;
                        back_btn.Enabled = false;
                        break;
                    }
                }
                if (okay == true)
                {
                    BM21.ForeColor = Color.Black;
                    tBM21 = true;
                    if (tB1 & tB2 & tB3 & tB4 & tB5 & tB6 & tB7 & tB8 & tB9 & tB10 & tB11 & tB12 & tB13 & tB14 & tB15 & tB16 & tB17 & tB18 & tB19 & tB20 & tB21 & tB22 & tB23 & tB24 & tB25 & tB26 & tB27 & tB28 & tB29 & tBM1 & tBM2 & tBM3 & tBM4 & tBM5 & tBM6 & tBM7 & tBM8 & tBM9 & tBM10 & tBM11 & tBM12 & tBM13 & tBM14 & tBM15 & tBM16 & tBM17 & tBM18 & tBM19 & tBM20 & tBM21 & tBM22 & tBM23 & tBM24 & tBM25 & tBM26 & tBM27 & tBM28 & tBM29)
                    {
                        continue_btn.Enabled = true;
                        back_btn.Enabled = true;
                    }
                    else
                    {
                        continue_btn.Enabled = false;
                        back_btn.Enabled = false;
                    }
                }
            
        }

        private void BM22_TextChanged(object sender, EventArgs e)
        {
                BM22.ForeColor = Color.Black;
                bool okay = true;
                //neuer Text darf nur Zeichen aus der Liste digits (in der Klasse deklariert)
                foreach (char c in BM22.Text.ToCharArray())
                {
                    //sobald es ein unpassendes Zeichen gibt, aufhoeren und Fehlermeldung ausgeben
                    if (!digits.Contains<char>(c))
                    {
                        BM22.ForeColor = Color.Red;
                        okay = false;
                        tBM22 = false;
                        continue_btn.Enabled = false;
                        back_btn.Enabled = false;
                        break;
                    }
                }
                if (okay == true)
                {
                    BM22.ForeColor = Color.Black;
                    tBM22 = true;
                    if (tB1 & tB2 & tB3 & tB4 & tB5 & tB6 & tB7 & tB8 & tB9 & tB10 & tB11 & tB12 & tB13 & tB14 & tB15 & tB16 & tB17 & tB18 & tB19 & tB20 & tB21 & tB22 & tB23 & tB24 & tB25 & tB26 & tB27 & tB28 & tB29 & tBM1 & tBM2 & tBM3 & tBM4 & tBM5 & tBM6 & tBM7 & tBM8 & tBM9 & tBM10 & tBM11 & tBM12 & tBM13 & tBM14 & tBM15 & tBM16 & tBM17 & tBM18 & tBM19 & tBM20 & tBM21 & tBM22 & tBM23 & tBM24 & tBM25 & tBM26 & tBM27 & tBM28 & tBM29)
                    {
                        continue_btn.Enabled = true;
                        back_btn.Enabled = true;
                    }
                    else
                    {
                        continue_btn.Enabled = false;
                        back_btn.Enabled = false;
                    }
                }
            
        }

        private void BM23_TextChanged(object sender, EventArgs e)
        {
                BM23.ForeColor = Color.Black;
                bool okay = true;
                //neuer Text darf nur Zeichen aus der Liste digits (in der Klasse deklariert)
                foreach (char c in BM23.Text.ToCharArray())
                {
                    //sobald es ein unpassendes Zeichen gibt, aufhoeren und Fehlermeldung ausgeben
                    if (!digits.Contains<char>(c))
                    {
                        BM23.ForeColor = Color.Red;
                        okay = false;
                        tBM23 = false;
                        continue_btn.Enabled = false;
                        back_btn.Enabled = false;
                        break;
                    }
                }
                if (okay == true)
                {
                    BM23.ForeColor = Color.Black;
                    tBM23 = true;
                    if (tB1 & tB2 & tB3 & tB4 & tB5 & tB6 & tB7 & tB8 & tB9 & tB10 & tB11 & tB12 & tB13 & tB14 & tB15 & tB16 & tB17 & tB18 & tB19 & tB20 & tB21 & tB22 & tB23 & tB24 & tB25 & tB26 & tB27 & tB28 & tB29 & tBM1 & tBM2 & tBM3 & tBM4 & tBM5 & tBM6 & tBM7 & tBM8 & tBM9 & tBM10 & tBM11 & tBM12 & tBM13 & tBM14 & tBM15 & tBM16 & tBM17 & tBM18 & tBM19 & tBM20 & tBM21 & tBM22 & tBM23 & tBM24 & tBM25 & tBM26 & tBM27 & tBM28 & tBM29)
                    {
                        continue_btn.Enabled = true;
                        back_btn.Enabled = true;
                    }
                    else
                    {
                        continue_btn.Enabled = false;
                        back_btn.Enabled = false;
                    }
                }
            
        }

        private void BM24_TextChanged(object sender, EventArgs e)
        {
                BM24.ForeColor = Color.Black;
                bool okay = true;
                //neuer Text darf nur Zeichen aus der Liste digits (in der Klasse deklariert)
                foreach (char c in BM24.Text.ToCharArray())
                {
                    //sobald es ein unpassendes Zeichen gibt, aufhoeren und Fehlermeldung ausgeben
                    if (!digits.Contains<char>(c))
                    {
                        BM24.ForeColor = Color.Red;
                        okay = false;
                        tBM24 = false;
                        continue_btn.Enabled = false;
                        back_btn.Enabled = false;
                        break;
                    }
                }
                if (okay == true)
                {
                    BM24.ForeColor = Color.Black;
                    tBM24 = true;
                    if (tB1 & tB2 & tB3 & tB4 & tB5 & tB6 & tB7 & tB8 & tB9 & tB10 & tB11 & tB12 & tB13 & tB14 & tB15 & tB16 & tB17 & tB18 & tB19 & tB20 & tB21 & tB22 & tB23 & tB24 & tB25 & tB26 & tB27 & tB28 & tB29 & tBM1 & tBM2 & tBM3 & tBM4 & tBM5 & tBM6 & tBM7 & tBM8 & tBM9 & tBM10 & tBM11 & tBM12 & tBM13 & tBM14 & tBM15 & tBM16 & tBM17 & tBM18 & tBM19 & tBM20 & tBM21 & tBM22 & tBM23 & tBM24 & tBM25 & tBM26 & tBM27 & tBM28 & tBM29)
                    {
                        continue_btn.Enabled = true;
                        back_btn.Enabled = true;
                    }
                    else
                    {
                        continue_btn.Enabled = false;
                        back_btn.Enabled = false;
                    }
                
            }
        }

        private void BM25_TextChanged(object sender, EventArgs e)
        {
                BM25.ForeColor = Color.Black;
                bool okay = true;
                //neuer Text darf nur Zeichen aus der Liste digits (in der Klasse deklariert)
                foreach (char c in BM25.Text.ToCharArray())
                {
                    //sobald es ein unpassendes Zeichen gibt, aufhoeren und Fehlermeldung ausgeben
                    if (!digits.Contains<char>(c))
                    {
                        BM25.ForeColor = Color.Red;
                        okay = false;
                        tBM25 = false;
                        continue_btn.Enabled = false;
                        back_btn.Enabled = false;
                        break;
                    }
                }
                if (okay == true)
                {
                    BM25.ForeColor = Color.Black;
                    tBM25 = true;
                    if (tB1 & tB2 & tB3 & tB4 & tB5 & tB6 & tB7 & tB8 & tB9 & tB10 & tB11 & tB12 & tB13 & tB14 & tB15 & tB16 & tB17 & tB18 & tB19 & tB20 & tB21 & tB22 & tB23 & tB24 & tB25 & tB26 & tB27 & tB28 & tB29 & tBM1 & tBM2 & tBM3 & tBM4 & tBM5 & tBM6 & tBM7 & tBM8 & tBM9 & tBM10 & tBM11 & tBM12 & tBM13 & tBM14 & tBM15 & tBM16 & tBM17 & tBM18 & tBM19 & tBM20 & tBM21 & tBM22 & tBM23 & tBM24 & tBM25 & tBM26 & tBM27 & tBM28 & tBM29)
                    {
                        continue_btn.Enabled = true;
                        back_btn.Enabled = true;
                    }
                    else
                    {
                        continue_btn.Enabled = false;
                        back_btn.Enabled = false;
                    }
                }
            
        }

        private void BM26_TextChanged(object sender, EventArgs e)
        {
                BM26.ForeColor = Color.Black;
                bool okay = true;
                //neuer Text darf nur Zeichen aus der Liste digits (in der Klasse deklariert)
                foreach (char c in BM26.Text.ToCharArray())
                {
                    //sobald es ein unpassendes Zeichen gibt, aufhoeren und Fehlermeldung ausgeben
                    if (!digits.Contains<char>(c))
                    {
                        BM26.ForeColor = Color.Red;
                        okay = false;
                        tBM26 = false;
                        continue_btn.Enabled = false;
                        back_btn.Enabled = false;
                        break;
                    }
                }
                if (okay == true)
                {
                    BM26.ForeColor = Color.Black;
                    tBM26 = true;
                    if (tB1 & tB2 & tB3 & tB4 & tB5 & tB6 & tB7 & tB8 & tB9 & tB10 & tB11 & tB12 & tB13 & tB14 & tB15 & tB16 & tB17 & tB18 & tB19 & tB20 & tB21 & tB22 & tB23 & tB24 & tB25 & tB26 & tB27 & tB28 & tB29 & tBM1 & tBM2 & tBM3 & tBM4 & tBM5 & tBM6 & tBM7 & tBM8 & tBM9 & tBM10 & tBM11 & tBM12 & tBM13 & tBM14 & tBM15 & tBM16 & tBM17 & tBM18 & tBM19 & tBM20 & tBM21 & tBM22 & tBM23 & tBM24 & tBM25 & tBM26 & tBM27 & tBM28 & tBM29)
                    {
                        continue_btn.Enabled = true;
                        back_btn.Enabled = true;
                    }
                    else
                    {
                        continue_btn.Enabled = false;
                        back_btn.Enabled = false;
                    }
                }
            
        }

        private void BM27_TextChanged(object sender, EventArgs e)
        {
                BM27.ForeColor = Color.Black;
                bool okay = true;
                //neuer Text darf nur Zeichen aus der Liste digits (in der Klasse deklariert)
                foreach (char c in BM27.Text.ToCharArray())
                {
                    //sobald es ein unpassendes Zeichen gibt, aufhoeren und Fehlermeldung ausgeben
                    if (!digits.Contains<char>(c))
                    {
                        BM27.ForeColor = Color.Red;
                        okay = false;
                        tBM27 = false;
                        continue_btn.Enabled = false;
                        back_btn.Enabled = false;
                        break;
                    }
                }
                if (okay == true)
                {
                    BM27.ForeColor = Color.Black;
                    tBM27 = true;
                    if (tB1 & tB2 & tB3 & tB4 & tB5 & tB6 & tB7 & tB8 & tB9 & tB10 & tB11 & tB12 & tB13 & tB14 & tB15 & tB16 & tB17 & tB18 & tB19 & tB20 & tB21 & tB22 & tB23 & tB24 & tB25 & tB26 & tB27 & tB28 & tB29 & tBM1 & tBM2 & tBM3 & tBM4 & tBM5 & tBM6 & tBM7 & tBM8 & tBM9 & tBM10 & tBM11 & tBM12 & tBM13 & tBM14 & tBM15 & tBM16 & tBM17 & tBM18 & tBM19 & tBM20 & tBM21 & tBM22 & tBM23 & tBM24 & tBM25 & tBM26 & tBM27 & tBM28 & tBM29)
                    {
                        continue_btn.Enabled = true;
                        back_btn.Enabled = true;
                    }
                    else
                    {
                        continue_btn.Enabled = false;
                        back_btn.Enabled = false;
                    }
                }
            
        }

        private void BM28_TextChanged(object sender, EventArgs e)
        {
                BM28.ForeColor = Color.Black;
                bool okay = true;
                //neuer Text darf nur Zeichen aus der Liste digits (in der Klasse deklariert)
                foreach (char c in BM28.Text.ToCharArray())
                {
                    //sobald es ein unpassendes Zeichen gibt, aufhoeren und Fehlermeldung ausgeben
                    if (!digits.Contains<char>(c))
                    {
                        BM28.ForeColor = Color.Red;
                        okay = false;
                        tBM28 = false;
                        continue_btn.Enabled = false;
                        back_btn.Enabled = false;
                        break;
                    }
                }
                if (okay == true)
                {
                    BM28.ForeColor = Color.Black;
                    tBM28 = true;
                    if (tB1 & tB2 & tB3 & tB4 & tB5 & tB6 & tB7 & tB8 & tB9 & tB10 & tB11 & tB12 & tB13 & tB14 & tB15 & tB16 & tB17 & tB18 & tB19 & tB20 & tB21 & tB22 & tB23 & tB24 & tB25 & tB26 & tB27 & tB28 & tB29 & tBM1 & tBM2 & tBM3 & tBM4 & tBM5 & tBM6 & tBM7 & tBM8 & tBM9 & tBM10 & tBM11 & tBM12 & tBM13 & tBM14 & tBM15 & tBM16 & tBM17 & tBM18 & tBM19 & tBM20 & tBM21 & tBM22 & tBM23 & tBM24 & tBM25 & tBM26 & tBM27 & tBM28 & tBM29)
                    {
                        continue_btn.Enabled = true;
                        back_btn.Enabled = true;
                    }
                    else
                    {
                        continue_btn.Enabled = false;
                        back_btn.Enabled = false;
                    }
                
            }
        }

        private void BM29_TextChanged(object sender, EventArgs e)
        {
                BM29.ForeColor = Color.Black;
                bool okay = true;
                //neuer Text darf nur Zeichen aus der Liste digits (in der Klasse deklariert)
                foreach (char c in BM29.Text.ToCharArray())
                {
                    //sobald es ein unpassendes Zeichen gibt, aufhoeren und Fehlermeldung ausgeben
                    if (!digits.Contains<char>(c))
                    {
                        BM29.ForeColor = Color.Red;
                        okay = false;
                        tBM29 = false;
                        continue_btn.Enabled = false;
                        back_btn.Enabled = false;
                        break;
                    }
                }
                if (okay == true)
                {
                    BM29.ForeColor = Color.Black;
                    tBM29 = true;
                    if (tB1 & tB2 & tB3 & tB4 & tB5 & tB6 & tB7 & tB8 & tB9 & tB10 & tB11 & tB12 & tB13 & tB14 & tB15 & tB16 & tB17 & tB18 & tB19 & tB20 & tB21 & tB22 & tB23 & tB24 & tB25 & tB26 & tB27 & tB28 & tB29 & tBM1 & tBM2 & tBM3 & tBM4 & tBM5 & tBM6 & tBM7 & tBM8 & tBM9 & tBM10 & tBM11 & tBM12 & tBM13 & tBM14 & tBM15 & tBM16 & tBM17 & tBM18 & tBM19 & tBM20 & tBM21 & tBM22 & tBM23 & tBM24 & tBM25 & tBM26 & tBM27 & tBM28 & tBM29)
                    {
                        continue_btn.Enabled = true;
                        back_btn.Enabled = true;
                    }
                    else
                    {
                        continue_btn.Enabled = false;
                        back_btn.Enabled = false;
                    }
                }            
        }

        private void B1_TextChanged(object sender, EventArgs e)
        {
            B1.ForeColor = Color.Black;
            bool okay = true;
            //neuer Text darf nur Zeichen aus der Liste digits (in der Klasse deklariert)
            foreach (char c in B1.Text.ToCharArray())
            {
                //sobald es ein unpassendes Zeichen gibt, aufhoeren und Fehlermeldung ausgeben
                if (!letters.Contains<char>(c))
                {
                    B1.ForeColor = Color.Red;
                    okay = false;
                    tB1 = false;
                    continue_btn.Enabled = false;
                    back_btn.Enabled = false;
                    break;
                }
            }
            if (okay == true)
            {
                B1.ForeColor = Color.Black;
                tB1 = true;
                if (tB1 & tB2 & tB3 & tB4 & tB5 & tB6 & tB7 & tB8 & tB9 & tB10 & tB11 & tB12 & tB13 & tB14 & tB15 & tB16 & tB17 & tB18 & tB19 & tB20 & tB21 & tB22 & tB23 & tB24 & tB25 & tB26 & tB27 & tB28 & tB29 & tBM1 & tBM2 & tBM3 & tBM4 & tBM5 & tBM6 & tBM7 & tBM8 & tBM9 & tBM10 & tBM11 & tBM12 & tBM13 & tBM14 & tBM15 & tBM16 & tBM17 & tBM18 & tBM19 & tBM20 & tBM21 & tBM22 & tBM23 & tBM24 & tBM25 & tBM26 & tBM27 & tBM28 & tBM29)
                {
                    continue_btn.Enabled = true;
                    back_btn.Enabled = true;
                }
                else
                {
                    continue_btn.Enabled = false;
                    back_btn.Enabled = false;
                }
            }  
        }

        private void B2_TextChanged(object sender, EventArgs e)
        {
            B2.ForeColor = Color.Black;
            bool okay = true;
            //neuer Text darf nur Zeichen aus der Liste digits (in der Klasse deklariert)
            foreach (char c in B2.Text.ToCharArray())
            {
                //sobald es ein unpassendes Zeichen gibt, aufhoeren und Fehlermeldung ausgeben
                if (!letters.Contains<char>(c))
                {
                    B2.ForeColor = Color.Red;
                    okay = false;
                    tB2 = false;
                    continue_btn.Enabled = false;
                    back_btn.Enabled = false;
                    break;
                }
            }
            if (okay == true)
            {
                B2.ForeColor = Color.Black;
                tB2 = true;
                if (tB1 & tB2 & tB3 & tB4 & tB5 & tB6 & tB7 & tB8 & tB9 & tB10 & tB11 & tB12 & tB13 & tB14 & tB15 & tB16 & tB17 & tB18 & tB19 & tB20 & tB21 & tB22 & tB23 & tB24 & tB25 & tB26 & tB27 & tB28 & tB29 & tBM1 & tBM2 & tBM3 & tBM4 & tBM5 & tBM6 & tBM7 & tBM8 & tBM9 & tBM10 & tBM11 & tBM12 & tBM13 & tBM14 & tBM15 & tBM16 & tBM17 & tBM18 & tBM19 & tBM20 & tBM21 & tBM22 & tBM23 & tBM24 & tBM25 & tBM26 & tBM27 & tBM28 & tBM29)
                {
                    continue_btn.Enabled = true;
                    back_btn.Enabled = true;
                }
                else
                {
                    continue_btn.Enabled = false;
                    back_btn.Enabled = false;
                }
            }  
        }

        private void B3_TextChanged(object sender, EventArgs e)
        {
            B3.ForeColor = Color.Black;
            bool okay = true;
            //neuer Text darf nur Zeichen aus der Liste digits (in der Klasse deklariert)
            foreach (char c in B3.Text.ToCharArray())
            {
                //sobald es ein unpassendes Zeichen gibt, aufhoeren und Fehlermeldung ausgeben
                if (!letters.Contains<char>(c))
                {
                    B3.ForeColor = Color.Red;
                    okay = false;
                    tB3 = false;
                    continue_btn.Enabled = false;
                    back_btn.Enabled = false;
                    break;
                }
            }
            if (okay == true)
            {
                B3.ForeColor = Color.Black;
                tB3 = true;
                if (tB1 & tB2 & tB3 & tB4 & tB5 & tB6 & tB7 & tB8 & tB9 & tB10 & tB11 & tB12 & tB13 & tB14 & tB15 & tB16 & tB17 & tB18 & tB19 & tB20 & tB21 & tB22 & tB23 & tB24 & tB25 & tB26 & tB27 & tB28 & tB29 & tBM1 & tBM2 & tBM3 & tBM4 & tBM5 & tBM6 & tBM7 & tBM8 & tBM9 & tBM10 & tBM11 & tBM12 & tBM13 & tBM14 & tBM15 & tBM16 & tBM17 & tBM18 & tBM19 & tBM20 & tBM21 & tBM22 & tBM23 & tBM24 & tBM25 & tBM26 & tBM27 & tBM28 & tBM29)
                {
                    continue_btn.Enabled = true;
                    back_btn.Enabled = true;
                }
                else
                {
                    continue_btn.Enabled = false;
                    back_btn.Enabled = false;
                }
            }  
        }

        private void B4_TextChanged(object sender, EventArgs e)
        {
            B4.ForeColor = Color.Black;
            bool okay = true;
            //neuer Text darf nur Zeichen aus der Liste digits (in der Klasse deklariert)
            foreach (char c in B4.Text.ToCharArray())
            {
                //sobald es ein unpassendes Zeichen gibt, aufhoeren und Fehlermeldung ausgeben
                if (!letters.Contains<char>(c))
                {
                    B4.ForeColor = Color.Red;
                    okay = false;
                    tB4 = false;
                    continue_btn.Enabled = false;
                    back_btn.Enabled = false;
                    break;
                }
            }
            if (okay == true)
            {
                B4.ForeColor = Color.Black;
                tB4 = true;
                if (tB1 & tB2 & tB3 & tB4 & tB5 & tB6 & tB7 & tB8 & tB9 & tB10 & tB11 & tB12 & tB13 & tB14 & tB15 & tB16 & tB17 & tB18 & tB19 & tB20 & tB21 & tB22 & tB23 & tB24 & tB25 & tB26 & tB27 & tB28 & tB29 & tBM1 & tBM2 & tBM3 & tBM4 & tBM5 & tBM6 & tBM7 & tBM8 & tBM9 & tBM10 & tBM11 & tBM12 & tBM13 & tBM14 & tBM15 & tBM16 & tBM17 & tBM18 & tBM19 & tBM20 & tBM21 & tBM22 & tBM23 & tBM24 & tBM25 & tBM26 & tBM27 & tBM28 & tBM29)
                {
                    continue_btn.Enabled = true;
                    back_btn.Enabled = true;
                }
                else
                {
                    continue_btn.Enabled = false;
                    back_btn.Enabled = false;
                }
            }  
        }

        private void B5_TextChanged(object sender, EventArgs e)
        {
            B5.ForeColor = Color.Black;
            bool okay = true;
            //neuer Text darf nur Zeichen aus der Liste digits (in der Klasse deklariert)
            foreach (char c in B5.Text.ToCharArray())
            {
                //sobald es ein unpassendes Zeichen gibt, aufhoeren und Fehlermeldung ausgeben
                if (!letters.Contains<char>(c))
                {
                    B5.ForeColor = Color.Red;
                    okay = false;
                    tB5 = false;
                    continue_btn.Enabled = false;
                    back_btn.Enabled = false;
                    break;
                }
            }
            if (okay == true)
            {
                B5.ForeColor = Color.Black;
                tB5 = true;
                if (tB1 & tB2 & tB3 & tB4 & tB5 & tB6 & tB7 & tB8 & tB9 & tB10 & tB11 & tB12 & tB13 & tB14 & tB15 & tB16 & tB17 & tB18 & tB19 & tB20 & tB21 & tB22 & tB23 & tB24 & tB25 & tB26 & tB27 & tB28 & tB29 & tBM1 & tBM2 & tBM3 & tBM4 & tBM5 & tBM6 & tBM7 & tBM8 & tBM9 & tBM10 & tBM11 & tBM12 & tBM13 & tBM14 & tBM15 & tBM16 & tBM17 & tBM18 & tBM19 & tBM20 & tBM21 & tBM22 & tBM23 & tBM24 & tBM25 & tBM26 & tBM27 & tBM28 & tBM29)
                {
                    continue_btn.Enabled = true;
                    back_btn.Enabled = true;
                }
                else
                {
                    continue_btn.Enabled = false;
                    back_btn.Enabled = false;
                }
            }  
        }

        private void B6_TextChanged(object sender, EventArgs e)
        {
            B6.ForeColor = Color.Black;
            bool okay = true;
            //neuer Text darf nur Zeichen aus der Liste digits (in der Klasse deklariert)
            foreach (char c in B6.Text.ToCharArray())
            {
                //sobald es ein unpassendes Zeichen gibt, aufhoeren und Fehlermeldung ausgeben
                if (!letters.Contains<char>(c))
                {
                    B6.ForeColor = Color.Red;
                    okay = false;
                    tB6 = false;
                    continue_btn.Enabled = false;
                    back_btn.Enabled = false;
                    break;
                }
            }
            if (okay == true)
            {
                B6.ForeColor = Color.Black;
                tB6 = true;
                if (tB1 & tB2 & tB3 & tB4 & tB5 & tB6 & tB7 & tB8 & tB9 & tB10 & tB11 & tB12 & tB13 & tB14 & tB15 & tB16 & tB17 & tB18 & tB19 & tB20 & tB21 & tB22 & tB23 & tB24 & tB25 & tB26 & tB27 & tB28 & tB29 & tBM1 & tBM2 & tBM3 & tBM4 & tBM5 & tBM6 & tBM7 & tBM8 & tBM9 & tBM10 & tBM11 & tBM12 & tBM13 & tBM14 & tBM15 & tBM16 & tBM17 & tBM18 & tBM19 & tBM20 & tBM21 & tBM22 & tBM23 & tBM24 & tBM25 & tBM26 & tBM27 & tBM28 & tBM29)
                {
                    continue_btn.Enabled = true;
                    back_btn.Enabled = true;
                }
                else
                {
                    continue_btn.Enabled = false;
                    back_btn.Enabled = false;
                }
            }  
        }

        private void B7_TextChanged(object sender, EventArgs e)
        {
            B7.ForeColor = Color.Black;
            bool okay = true;
            //neuer Text darf nur Zeichen aus der Liste digits (in der Klasse deklariert)
            foreach (char c in B7.Text.ToCharArray())
            {
                //sobald es ein unpassendes Zeichen gibt, aufhoeren und Fehlermeldung ausgeben
                if (!letters.Contains<char>(c))
                {
                    B7.ForeColor = Color.Red;
                    okay = false;
                    tB7 = false;
                    continue_btn.Enabled = false;
                    back_btn.Enabled = false;
                    break;
                }
            }
            if (okay == true)
            {
                B7.ForeColor = Color.Black;
                tB7 = true;
                if (tB1 & tB2 & tB3 & tB4 & tB5 & tB6 & tB7 & tB8 & tB9 & tB10 & tB11 & tB12 & tB13 & tB14 & tB15 & tB16 & tB17 & tB18 & tB19 & tB20 & tB21 & tB22 & tB23 & tB24 & tB25 & tB26 & tB27 & tB28 & tB29 & tBM1 & tBM2 & tBM3 & tBM4 & tBM5 & tBM6 & tBM7 & tBM8 & tBM9 & tBM10 & tBM11 & tBM12 & tBM13 & tBM14 & tBM15 & tBM16 & tBM17 & tBM18 & tBM19 & tBM20 & tBM21 & tBM22 & tBM23 & tBM24 & tBM25 & tBM26 & tBM27 & tBM28 & tBM29)
                {
                    continue_btn.Enabled = true;
                    back_btn.Enabled = true;
                }
                else
                {
                    continue_btn.Enabled = false;
                    back_btn.Enabled = false;
                }
            }  
        }

        private void B8_TextChanged(object sender, EventArgs e)
        {
            B8.ForeColor = Color.Black;
            bool okay = true;
            //neuer Text darf nur Zeichen aus der Liste digits (in der Klasse deklariert)
            foreach (char c in B8.Text.ToCharArray())
            {
                //sobald es ein unpassendes Zeichen gibt, aufhoeren und Fehlermeldung ausgeben
                if (!letters.Contains<char>(c))
                {
                    B8.ForeColor = Color.Red;
                    okay = false;
                    tB8 = false;
                    continue_btn.Enabled = false;
                    back_btn.Enabled = false;
                    break;
                }
            }
            if (okay == true)
            {
                B8.ForeColor = Color.Black;
                tB8 = true;
                if (tB1 & tB2 & tB3 & tB4 & tB5 & tB6 & tB7 & tB8 & tB9 & tB10 & tB11 & tB12 & tB13 & tB14 & tB15 & tB16 & tB17 & tB18 & tB19 & tB20 & tB21 & tB22 & tB23 & tB24 & tB25 & tB26 & tB27 & tB28 & tB29 & tBM1 & tBM2 & tBM3 & tBM4 & tBM5 & tBM6 & tBM7 & tBM8 & tBM9 & tBM10 & tBM11 & tBM12 & tBM13 & tBM14 & tBM15 & tBM16 & tBM17 & tBM18 & tBM19 & tBM20 & tBM21 & tBM22 & tBM23 & tBM24 & tBM25 & tBM26 & tBM27 & tBM28 & tBM29)
                {
                    continue_btn.Enabled = true;
                    back_btn.Enabled = true;
                }
                else
                {
                    continue_btn.Enabled = false;
                    back_btn.Enabled = false;
                }
            }  
        }

        private void B9_TextChanged(object sender, EventArgs e)
        {
            B9.ForeColor = Color.Black;
            bool okay = true;
            //neuer Text darf nur Zeichen aus der Liste digits (in der Klasse deklariert)
            foreach (char c in B9.Text.ToCharArray())
            {
                //sobald es ein unpassendes Zeichen gibt, aufhoeren und Fehlermeldung ausgeben
                if (!letters.Contains<char>(c))
                {
                    B9.ForeColor = Color.Red;
                    okay = false;
                    tB9 = false;
                    continue_btn.Enabled = false;
                    back_btn.Enabled = false;
                    break;
                }
            }
            if (okay == true)
            {
                B9.ForeColor = Color.Black;
                tB9 = true;
                if (tB1 & tB2 & tB3 & tB4 & tB5 & tB6 & tB7 & tB8 & tB9 & tB10 & tB11 & tB12 & tB13 & tB14 & tB15 & tB16 & tB17 & tB18 & tB19 & tB20 & tB21 & tB22 & tB23 & tB24 & tB25 & tB26 & tB27 & tB28 & tB29 & tBM1 & tBM2 & tBM3 & tBM4 & tBM5 & tBM6 & tBM7 & tBM8 & tBM9 & tBM10 & tBM11 & tBM12 & tBM13 & tBM14 & tBM15 & tBM16 & tBM17 & tBM18 & tBM19 & tBM20 & tBM21 & tBM22 & tBM23 & tBM24 & tBM25 & tBM26 & tBM27 & tBM28 & tBM29)
                {
                    continue_btn.Enabled = true;
                    back_btn.Enabled = true;
                }
                else
                {
                    continue_btn.Enabled = false;
                    back_btn.Enabled = false;
                }
            }  
        }

        private void B10_TextChanged(object sender, EventArgs e)
        {
            B10.ForeColor = Color.Black;
            bool okay = true;
            //neuer Text darf nur Zeichen aus der Liste digits (in der Klasse deklariert)
            foreach (char c in B10.Text.ToCharArray())
            {
                //sobald es ein unpassendes Zeichen gibt, aufhoeren und Fehlermeldung ausgeben
                if (!letters.Contains<char>(c))
                {
                    B10.ForeColor = Color.Red;
                    okay = false;
                    tB10 = false;
                    continue_btn.Enabled = false;
                    back_btn.Enabled = false;
                    break;
                }
            }
            if (okay == true)
            {
                B10.ForeColor = Color.Black;
                tB10 = true;
                if (tB1 & tB2 & tB3 & tB4 & tB5 & tB6 & tB7 & tB8 & tB9 & tB10 & tB11 & tB12 & tB13 & tB14 & tB15 & tB16 & tB17 & tB18 & tB19 & tB20 & tB21 & tB22 & tB23 & tB24 & tB25 & tB26 & tB27 & tB28 & tB29 & tBM1 & tBM2 & tBM3 & tBM4 & tBM5 & tBM6 & tBM7 & tBM8 & tBM9 & tBM10 & tBM11 & tBM12 & tBM13 & tBM14 & tBM15 & tBM16 & tBM17 & tBM18 & tBM19 & tBM20 & tBM21 & tBM22 & tBM23 & tBM24 & tBM25 & tBM26 & tBM27 & tBM28 & tBM29)
                {
                    continue_btn.Enabled = true;
                    back_btn.Enabled = true;
                }
                else
                {
                    continue_btn.Enabled = false;
                    back_btn.Enabled = false;
                }
            }  
        }

        private void B11_TextChanged(object sender, EventArgs e)
        {
            B11.ForeColor = Color.Black;
            bool okay = true;
            //neuer Text darf nur Zeichen aus der Liste digits (in der Klasse deklariert)
            foreach (char c in B11.Text.ToCharArray())
            {
                //sobald es ein unpassendes Zeichen gibt, aufhoeren und Fehlermeldung ausgeben
                if (!letters.Contains<char>(c))
                {
                    B11.ForeColor = Color.Red;
                    okay = false;
                    tB11 = false;
                    continue_btn.Enabled = false;
                    back_btn.Enabled = false;
                    break;
                }
            }
            if (okay == true)
            {
                B11.ForeColor = Color.Black;
                tB11 = true;
                if (tB1 & tB2 & tB3 & tB4 & tB5 & tB6 & tB7 & tB8 & tB9 & tB10 & tB11 & tB12 & tB13 & tB14 & tB15 & tB16 & tB17 & tB18 & tB19 & tB20 & tB21 & tB22 & tB23 & tB24 & tB25 & tB26 & tB27 & tB28 & tB29 & tBM1 & tBM2 & tBM3 & tBM4 & tBM5 & tBM6 & tBM7 & tBM8 & tBM9 & tBM10 & tBM11 & tBM12 & tBM13 & tBM14 & tBM15 & tBM16 & tBM17 & tBM18 & tBM19 & tBM20 & tBM21 & tBM22 & tBM23 & tBM24 & tBM25 & tBM26 & tBM27 & tBM28 & tBM29)
                {
                    continue_btn.Enabled = true;
                    back_btn.Enabled = true;
                }
                else
                {
                    continue_btn.Enabled = false;
                    back_btn.Enabled = false;
                }
            }  
        }

        private void B12_TextChanged(object sender, EventArgs e)
        {
            B12.ForeColor = Color.Black;
            bool okay = true;
            //neuer Text darf nur Zeichen aus der Liste digits (in der Klasse deklariert)
            foreach (char c in B12.Text.ToCharArray())
            {
                //sobald es ein unpassendes Zeichen gibt, aufhoeren und Fehlermeldung ausgeben
                if (!letters.Contains<char>(c))
                {
                    B12.ForeColor = Color.Red;
                    okay = false;
                    tB12 = false;
                    continue_btn.Enabled = false;
                    back_btn.Enabled = false;
                    break;
                }
            }
            if (okay == true)
            {
                B12.ForeColor = Color.Black;
                tB12 = true;
                if (tB1 & tB2 & tB3 & tB4 & tB5 & tB6 & tB7 & tB8 & tB9 & tB10 & tB11 & tB12 & tB13 & tB14 & tB15 & tB16 & tB17 & tB18 & tB19 & tB20 & tB21 & tB22 & tB23 & tB24 & tB25 & tB26 & tB27 & tB28 & tB29 & tBM1 & tBM2 & tBM3 & tBM4 & tBM5 & tBM6 & tBM7 & tBM8 & tBM9 & tBM10 & tBM11 & tBM12 & tBM13 & tBM14 & tBM15 & tBM16 & tBM17 & tBM18 & tBM19 & tBM20 & tBM21 & tBM22 & tBM23 & tBM24 & tBM25 & tBM26 & tBM27 & tBM28 & tBM29)
                {
                    continue_btn.Enabled = true;
                    back_btn.Enabled = true;
                }
                else
                {
                    continue_btn.Enabled = false;
                    back_btn.Enabled = false;
                }
            }  
        }

        private void B13_TextChanged(object sender, EventArgs e)
        {
            B13.ForeColor = Color.Black;
            bool okay = true;
            //neuer Text darf nur Zeichen aus der Liste digits (in der Klasse deklariert)
            foreach (char c in B13.Text.ToCharArray())
            {
                //sobald es ein unpassendes Zeichen gibt, aufhoeren und Fehlermeldung ausgeben
                if (!letters.Contains<char>(c))
                {
                    B13.ForeColor = Color.Red;
                    okay = false;
                    tB13 = false;
                    continue_btn.Enabled = false;
                    back_btn.Enabled = false;
                    break;
                }
            }
            if (okay == true)
            {
                B13.ForeColor = Color.Black;
                tB13 = true;
                if (tB1 & tB2 & tB3 & tB4 & tB5 & tB6 & tB7 & tB8 & tB9 & tB10 & tB11 & tB12 & tB13 & tB14 & tB15 & tB16 & tB17 & tB18 & tB19 & tB20 & tB21 & tB22 & tB23 & tB24 & tB25 & tB26 & tB27 & tB28 & tB29 & tBM1 & tBM2 & tBM3 & tBM4 & tBM5 & tBM6 & tBM7 & tBM8 & tBM9 & tBM10 & tBM11 & tBM12 & tBM13 & tBM14 & tBM15 & tBM16 & tBM17 & tBM18 & tBM19 & tBM20 & tBM21 & tBM22 & tBM23 & tBM24 & tBM25 & tBM26 & tBM27 & tBM28 & tBM29)
                {
                    continue_btn.Enabled = true;
                    back_btn.Enabled = true;
                }
                else
                {
                    continue_btn.Enabled = false;
                    back_btn.Enabled = false;
                }
            }  
        }

        private void B14_TextChanged(object sender, EventArgs e)
        {
            B14.ForeColor = Color.Black;
            bool okay = true;
            //neuer Text darf nur Zeichen aus der Liste digits (in der Klasse deklariert)
            foreach (char c in B14.Text.ToCharArray())
            {
                //sobald es ein unpassendes Zeichen gibt, aufhoeren und Fehlermeldung ausgeben
                if (!letters.Contains<char>(c))
                {
                    B14.ForeColor = Color.Red;
                    okay = false;
                    tB14 = false;
                    continue_btn.Enabled = false;
                    back_btn.Enabled = false;
                    break;
                }
            }
            if (okay == true)
            {
                B14.ForeColor = Color.Black;
                tB14 = true;
                if (tB1 & tB2 & tB3 & tB4 & tB5 & tB6 & tB7 & tB8 & tB9 & tB10 & tB11 & tB12 & tB13 & tB14 & tB15 & tB16 & tB17 & tB18 & tB19 & tB20 & tB21 & tB22 & tB23 & tB24 & tB25 & tB26 & tB27 & tB28 & tB29 & tBM1 & tBM2 & tBM3 & tBM4 & tBM5 & tBM6 & tBM7 & tBM8 & tBM9 & tBM10 & tBM11 & tBM12 & tBM13 & tBM14 & tBM15 & tBM16 & tBM17 & tBM18 & tBM19 & tBM20 & tBM21 & tBM22 & tBM23 & tBM24 & tBM25 & tBM26 & tBM27 & tBM28 & tBM29)
                {
                    continue_btn.Enabled = true;
                    back_btn.Enabled = true;
                }
                else
                {
                    continue_btn.Enabled = false;
                    back_btn.Enabled = false;
                }
            }  
        }

        private void B15_TextChanged(object sender, EventArgs e)
        {
            B15.ForeColor = Color.Black;
            bool okay = true;
            //neuer Text darf nur Zeichen aus der Liste digits (in der Klasse deklariert)
            foreach (char c in B15.Text.ToCharArray())
            {
                //sobald es ein unpassendes Zeichen gibt, aufhoeren und Fehlermeldung ausgeben
                if (!letters.Contains<char>(c))
                {
                    B15.ForeColor = Color.Red;
                    okay = false;
                    tB15 = false;
                    continue_btn.Enabled = false;
                    back_btn.Enabled = false;
                    break;
                }
            }
            if (okay == true)
            {
                B15.ForeColor = Color.Black;
                tB15 = true;
                if (tB1 & tB2 & tB3 & tB4 & tB5 & tB6 & tB7 & tB8 & tB9 & tB10 & tB11 & tB12 & tB13 & tB14 & tB15 & tB16 & tB17 & tB18 & tB19 & tB20 & tB21 & tB22 & tB23 & tB24 & tB25 & tB26 & tB27 & tB28 & tB29 & tBM1 & tBM2 & tBM3 & tBM4 & tBM5 & tBM6 & tBM7 & tBM8 & tBM9 & tBM10 & tBM11 & tBM12 & tBM13 & tBM14 & tBM15 & tBM16 & tBM17 & tBM18 & tBM19 & tBM20 & tBM21 & tBM22 & tBM23 & tBM24 & tBM25 & tBM26 & tBM27 & tBM28 & tBM29)
                {
                    continue_btn.Enabled = true;
                    back_btn.Enabled = true;
                }
                else
                {
                    continue_btn.Enabled = false;
                    back_btn.Enabled = false;
                }
            }  
        }

        private void B16_TextChanged(object sender, EventArgs e)
        {
            B16.ForeColor = Color.Black;
            bool okay = true;
            //neuer Text darf nur Zeichen aus der Liste digits (in der Klasse deklariert)
            foreach (char c in B16.Text.ToCharArray())
            {
                //sobald es ein unpassendes Zeichen gibt, aufhoeren und Fehlermeldung ausgeben
                if (!letters.Contains<char>(c))
                {
                    B16.ForeColor = Color.Red;
                    okay = false;
                    tB16 = false;
                    continue_btn.Enabled = false;
                    back_btn.Enabled = false;
                    break;
                }
            }
            if (okay == true)
            {
                B16.ForeColor = Color.Black;
                tB16 = true;
                if (tB1 & tB2 & tB3 & tB4 & tB5 & tB6 & tB7 & tB8 & tB9 & tB10 & tB11 & tB12 & tB13 & tB14 & tB15 & tB16 & tB17 & tB18 & tB19 & tB20 & tB21 & tB22 & tB23 & tB24 & tB25 & tB26 & tB27 & tB28 & tB29 & tBM1 & tBM2 & tBM3 & tBM4 & tBM5 & tBM6 & tBM7 & tBM8 & tBM9 & tBM10 & tBM11 & tBM12 & tBM13 & tBM14 & tBM15 & tBM16 & tBM17 & tBM18 & tBM19 & tBM20 & tBM21 & tBM22 & tBM23 & tBM24 & tBM25 & tBM26 & tBM27 & tBM28 & tBM29)
                {
                    continue_btn.Enabled = true;
                    back_btn.Enabled = true;
                }
                else
                {
                    continue_btn.Enabled = false;
                    back_btn.Enabled = false;
                }
            }  
        }

        private void B17_TextChanged(object sender, EventArgs e)
        {
            B17.ForeColor = Color.Black;
            bool okay = true;
            //neuer Text darf nur Zeichen aus der Liste digits (in der Klasse deklariert)
            foreach (char c in B17.Text.ToCharArray())
            {
                //sobald es ein unpassendes Zeichen gibt, aufhoeren und Fehlermeldung ausgeben
                if (!letters.Contains<char>(c))
                {
                    B17.ForeColor = Color.Red;
                    okay = false;
                    tB17 = false;
                    continue_btn.Enabled = false;
                    back_btn.Enabled = false;
                    break;
                }
            }
            if (okay == true)
            {
                B17.ForeColor = Color.Black;
                tB17 = true;
                if (tB1 & tB2 & tB3 & tB4 & tB5 & tB6 & tB7 & tB8 & tB9 & tB10 & tB11 & tB12 & tB13 & tB14 & tB15 & tB16 & tB17 & tB18 & tB19 & tB20 & tB21 & tB22 & tB23 & tB24 & tB25 & tB26 & tB27 & tB28 & tB29 & tBM1 & tBM2 & tBM3 & tBM4 & tBM5 & tBM6 & tBM7 & tBM8 & tBM9 & tBM10 & tBM11 & tBM12 & tBM13 & tBM14 & tBM15 & tBM16 & tBM17 & tBM18 & tBM19 & tBM20 & tBM21 & tBM22 & tBM23 & tBM24 & tBM25 & tBM26 & tBM27 & tBM28 & tBM29)
                {
                    continue_btn.Enabled = true;
                    back_btn.Enabled = true;
                }
                else
                {
                    continue_btn.Enabled = false;
                    back_btn.Enabled = false;
                }
            }  
        }

        private void B18_TextChanged(object sender, EventArgs e)
        {
            B18.ForeColor = Color.Black;
            bool okay = true;
            //neuer Text darf nur Zeichen aus der Liste digits (in der Klasse deklariert)
            foreach (char c in B18.Text.ToCharArray())
            {
                //sobald es ein unpassendes Zeichen gibt, aufhoeren und Fehlermeldung ausgeben
                if (!letters.Contains<char>(c))
                {
                    B18.ForeColor = Color.Red;
                    okay = false;
                    tB18 = false;
                    continue_btn.Enabled = false;
                    back_btn.Enabled = false;
                    break;
                }
            }
            if (okay == true)
            {
                B18.ForeColor = Color.Black;
                tB18 = true;
                if (tB1 & tB2 & tB3 & tB4 & tB5 & tB6 & tB7 & tB8 & tB9 & tB10 & tB11 & tB12 & tB13 & tB14 & tB15 & tB16 & tB17 & tB18 & tB19 & tB20 & tB21 & tB22 & tB23 & tB24 & tB25 & tB26 & tB27 & tB28 & tB29 & tBM1 & tBM2 & tBM3 & tBM4 & tBM5 & tBM6 & tBM7 & tBM8 & tBM9 & tBM10 & tBM11 & tBM12 & tBM13 & tBM14 & tBM15 & tBM16 & tBM17 & tBM18 & tBM19 & tBM20 & tBM21 & tBM22 & tBM23 & tBM24 & tBM25 & tBM26 & tBM27 & tBM28 & tBM29)
                {
                    continue_btn.Enabled = true;
                    back_btn.Enabled = true;
                }
                else
                {
                    continue_btn.Enabled = false;
                    back_btn.Enabled = false;
                }
            }  
        }

        private void B19_TextChanged(object sender, EventArgs e)
        {
            B19.ForeColor = Color.Black;
            bool okay = true;
            //neuer Text darf nur Zeichen aus der Liste digits (in der Klasse deklariert)
            foreach (char c in B19.Text.ToCharArray())
            {
                //sobald es ein unpassendes Zeichen gibt, aufhoeren und Fehlermeldung ausgeben
                if (!letters.Contains<char>(c))
                {
                    B19.ForeColor = Color.Red;
                    okay = false;
                    tB19 = false;
                    continue_btn.Enabled = false;
                    back_btn.Enabled = false;
                    break;
                }
            }
            if (okay == true)
            {
                B19.ForeColor = Color.Black;
                tB19 = true;
                if (tB1 & tB2 & tB3 & tB4 & tB5 & tB6 & tB7 & tB8 & tB9 & tB10 & tB11 & tB12 & tB13 & tB14 & tB15 & tB16 & tB17 & tB18 & tB19 & tB20 & tB21 & tB22 & tB23 & tB24 & tB25 & tB26 & tB27 & tB28 & tB29 & tBM1 & tBM2 & tBM3 & tBM4 & tBM5 & tBM6 & tBM7 & tBM8 & tBM9 & tBM10 & tBM11 & tBM12 & tBM13 & tBM14 & tBM15 & tBM16 & tBM17 & tBM18 & tBM19 & tBM20 & tBM21 & tBM22 & tBM23 & tBM24 & tBM25 & tBM26 & tBM27 & tBM28 & tBM29)
                {
                    continue_btn.Enabled = true;
                    back_btn.Enabled = true;
                }
                else
                {
                    continue_btn.Enabled = false;
                    back_btn.Enabled = false;
                }
            }  
        }

        private void B20_TextChanged(object sender, EventArgs e)
        {
            B20.ForeColor = Color.Black;
            bool okay = true;
            //neuer Text darf nur Zeichen aus der Liste digits (in der Klasse deklariert)
            foreach (char c in B20.Text.ToCharArray())
            {
                //sobald es ein unpassendes Zeichen gibt, aufhoeren und Fehlermeldung ausgeben
                if (!letters.Contains<char>(c))
                {
                    B20.ForeColor = Color.Red;
                    okay = false;
                    tB20 = false;
                    continue_btn.Enabled = false;
                    back_btn.Enabled = false;
                    break;
                }
            }
            if (okay == true)
            {
                B20.ForeColor = Color.Black;
                tB20 = true;
                if (tB1 & tB2 & tB3 & tB4 & tB5 & tB6 & tB7 & tB8 & tB9 & tB10 & tB11 & tB12 & tB13 & tB14 & tB15 & tB16 & tB17 & tB18 & tB19 & tB20 & tB21 & tB22 & tB23 & tB24 & tB25 & tB26 & tB27 & tB28 & tB29 & tBM1 & tBM2 & tBM3 & tBM4 & tBM5 & tBM6 & tBM7 & tBM8 & tBM9 & tBM10 & tBM11 & tBM12 & tBM13 & tBM14 & tBM15 & tBM16 & tBM17 & tBM18 & tBM19 & tBM20 & tBM21 & tBM22 & tBM23 & tBM24 & tBM25 & tBM26 & tBM27 & tBM28 & tBM29)
                {
                    continue_btn.Enabled = true;
                    back_btn.Enabled = true;
                }
                else
                {
                    continue_btn.Enabled = false;
                    back_btn.Enabled = false;
                }
            }  
        }

        private void B21_TextChanged(object sender, EventArgs e)
        {
            B21.ForeColor = Color.Black;
            bool okay = true;
            //neuer Text darf nur Zeichen aus der Liste digits (in der Klasse deklariert)
            foreach (char c in B21.Text.ToCharArray())
            {
                //sobald es ein unpassendes Zeichen gibt, aufhoeren und Fehlermeldung ausgeben
                if (!letters.Contains<char>(c))
                {
                    B21.ForeColor = Color.Red;
                    okay = false;
                    tB21 = false;
                    continue_btn.Enabled = false;
                    back_btn.Enabled = false;
                    break;
                }
            }
            if (okay == true)
            {
                B21.ForeColor = Color.Black;
                tB21 = true;
                if (tB1 & tB2 & tB3 & tB4 & tB5 & tB6 & tB7 & tB8 & tB9 & tB10 & tB11 & tB12 & tB13 & tB14 & tB15 & tB16 & tB17 & tB18 & tB19 & tB20 & tB21 & tB22 & tB23 & tB24 & tB25 & tB26 & tB27 & tB28 & tB29 & tBM1 & tBM2 & tBM3 & tBM4 & tBM5 & tBM6 & tBM7 & tBM8 & tBM9 & tBM10 & tBM11 & tBM12 & tBM13 & tBM14 & tBM15 & tBM16 & tBM17 & tBM18 & tBM19 & tBM20 & tBM21 & tBM22 & tBM23 & tBM24 & tBM25 & tBM26 & tBM27 & tBM28 & tBM29)
                {
                    continue_btn.Enabled = true;
                    back_btn.Enabled = true;
                }
                else
                {
                    continue_btn.Enabled = false;
                    back_btn.Enabled = false;
                }
            }  
        }

        private void B22_TextChanged(object sender, EventArgs e)
        {
            B22.ForeColor = Color.Black;
            bool okay = true;
            //neuer Text darf nur Zeichen aus der Liste digits (in der Klasse deklariert)
            foreach (char c in B22.Text.ToCharArray())
            {
                //sobald es ein unpassendes Zeichen gibt, aufhoeren und Fehlermeldung ausgeben
                if (!letters.Contains<char>(c))
                {
                    B22.ForeColor = Color.Red;
                    okay = false;
                    tB22 = false;
                    continue_btn.Enabled = false;
                    back_btn.Enabled = false;
                    break;
                }
            }
            if (okay == true)
            {
                B22.ForeColor = Color.Black;
                tB22 = true;
                if (tB1 & tB2 & tB3 & tB4 & tB5 & tB6 & tB7 & tB8 & tB9 & tB10 & tB11 & tB12 & tB13 & tB14 & tB15 & tB16 & tB17 & tB18 & tB19 & tB20 & tB21 & tB22 & tB23 & tB24 & tB25 & tB26 & tB27 & tB28 & tB29 & tBM1 & tBM2 & tBM3 & tBM4 & tBM5 & tBM6 & tBM7 & tBM8 & tBM9 & tBM10 & tBM11 & tBM12 & tBM13 & tBM14 & tBM15 & tBM16 & tBM17 & tBM18 & tBM19 & tBM20 & tBM21 & tBM22 & tBM23 & tBM24 & tBM25 & tBM26 & tBM27 & tBM28 & tBM29)
                {
                    continue_btn.Enabled = true;
                    back_btn.Enabled = true;
                }
                else
                {
                    continue_btn.Enabled = false;
                    back_btn.Enabled = false;
                }
            }  
        }

        private void B23_TextChanged(object sender, EventArgs e)
        {
            B23.ForeColor = Color.Black;
            bool okay = true;
            //neuer Text darf nur Zeichen aus der Liste digits (in der Klasse deklariert)
            foreach (char c in B23.Text.ToCharArray())
            {
                //sobald es ein unpassendes Zeichen gibt, aufhoeren und Fehlermeldung ausgeben
                if (!letters.Contains<char>(c))
                {
                    B23.ForeColor = Color.Red;
                    okay = false;
                    tB23 = false;
                    continue_btn.Enabled = false;
                    back_btn.Enabled = false;
                    break;
                }
            }
            if (okay == true)
            {
                B23.ForeColor = Color.Black;
                tB23 = true;
                if (tB1 & tB2 & tB3 & tB4 & tB5 & tB6 & tB7 & tB8 & tB9 & tB10 & tB11 & tB12 & tB13 & tB14 & tB15 & tB16 & tB17 & tB18 & tB19 & tB20 & tB21 & tB22 & tB23 & tB24 & tB25 & tB26 & tB27 & tB28 & tB29 & tBM1 & tBM2 & tBM3 & tBM4 & tBM5 & tBM6 & tBM7 & tBM8 & tBM9 & tBM10 & tBM11 & tBM12 & tBM13 & tBM14 & tBM15 & tBM16 & tBM17 & tBM18 & tBM19 & tBM20 & tBM21 & tBM22 & tBM23 & tBM24 & tBM25 & tBM26 & tBM27 & tBM28 & tBM29)
                {
                    continue_btn.Enabled = true;
                    back_btn.Enabled = true;
                }
                else
                {
                    continue_btn.Enabled = false;
                    back_btn.Enabled = false;
                }
            }  
        }

        private void B24_TextChanged(object sender, EventArgs e)
        {
            B24.ForeColor = Color.Black;
            bool okay = true;
            //neuer Text darf nur Zeichen aus der Liste digits (in der Klasse deklariert)
            foreach (char c in B24.Text.ToCharArray())
            {
                //sobald es ein unpassendes Zeichen gibt, aufhoeren und Fehlermeldung ausgeben
                if (!letters.Contains<char>(c))
                {
                    B24.ForeColor = Color.Red;
                    okay = false;
                    tB24 = false;
                    continue_btn.Enabled = false;
                    back_btn.Enabled = false;
                    break;
                }
            }
            if (okay == true)
            {
                B24.ForeColor = Color.Black;
                tB24 = true;
                if (tB1 & tB2 & tB3 & tB4 & tB5 & tB6 & tB7 & tB8 & tB9 & tB10 & tB11 & tB12 & tB13 & tB14 & tB15 & tB16 & tB17 & tB18 & tB19 & tB20 & tB21 & tB22 & tB23 & tB24 & tB25 & tB26 & tB27 & tB28 & tB29 & tBM1 & tBM2 & tBM3 & tBM4 & tBM5 & tBM6 & tBM7 & tBM8 & tBM9 & tBM10 & tBM11 & tBM12 & tBM13 & tBM14 & tBM15 & tBM16 & tBM17 & tBM18 & tBM19 & tBM20 & tBM21 & tBM22 & tBM23 & tBM24 & tBM25 & tBM26 & tBM27 & tBM28 & tBM29)
                {
                    continue_btn.Enabled = true;
                    back_btn.Enabled = true;
                }
                else
                {
                    continue_btn.Enabled = false;
                    back_btn.Enabled = false;
                }
            }  
        }

        private void B25_TextChanged(object sender, EventArgs e)
        {
            B25.ForeColor = Color.Black;
            bool okay = true;
            //neuer Text darf nur Zeichen aus der Liste digits (in der Klasse deklariert)
            foreach (char c in B25.Text.ToCharArray())
            {
                //sobald es ein unpassendes Zeichen gibt, aufhoeren und Fehlermeldung ausgeben
                if (!letters.Contains<char>(c))
                {
                    B25.ForeColor = Color.Red;
                    okay = false;
                    tB25 = false;
                    continue_btn.Enabled = false;
                    back_btn.Enabled = false;
                    break;
                }
            }
            if (okay == true)
            {
                B25.ForeColor = Color.Black;
                tB25 = true;
                if (tB1 & tB2 & tB3 & tB4 & tB5 & tB6 & tB7 & tB8 & tB9 & tB10 & tB11 & tB12 & tB13 & tB14 & tB15 & tB16 & tB17 & tB18 & tB19 & tB20 & tB21 & tB22 & tB23 & tB24 & tB25 & tB26 & tB27 & tB28 & tB29 & tBM1 & tBM2 & tBM3 & tBM4 & tBM5 & tBM6 & tBM7 & tBM8 & tBM9 & tBM10 & tBM11 & tBM12 & tBM13 & tBM14 & tBM15 & tBM16 & tBM17 & tBM18 & tBM19 & tBM20 & tBM21 & tBM22 & tBM23 & tBM24 & tBM25 & tBM26 & tBM27 & tBM28 & tBM29)
                {
                    continue_btn.Enabled = true;
                    back_btn.Enabled = true;
                }
                else
                {
                    continue_btn.Enabled = false;
                    back_btn.Enabled = false;
                }
            }  
        }

        private void B26_TextChanged(object sender, EventArgs e)
        {
            B26.ForeColor = Color.Black;
            bool okay = true;
            //neuer Text darf nur Zeichen aus der Liste digits (in der Klasse deklariert)
            foreach (char c in B26.Text.ToCharArray())
            {
                //sobald es ein unpassendes Zeichen gibt, aufhoeren und Fehlermeldung ausgeben
                if (!letters.Contains<char>(c))
                {
                    B26.ForeColor = Color.Red;
                    okay = false;
                    tB26 = false;
                    continue_btn.Enabled = false;
                    back_btn.Enabled = false;
                    break;
                }
            }
            if (okay == true)
            {
                B26.ForeColor = Color.Black;
                tB26 = true;
                if (tB1 & tB2 & tB3 & tB4 & tB5 & tB6 & tB7 & tB8 & tB9 & tB10 & tB11 & tB12 & tB13 & tB14 & tB15 & tB16 & tB17 & tB18 & tB19 & tB20 & tB21 & tB22 & tB23 & tB24 & tB25 & tB26 & tB27 & tB28 & tB29 & tBM1 & tBM2 & tBM3 & tBM4 & tBM5 & tBM6 & tBM7 & tBM8 & tBM9 & tBM10 & tBM11 & tBM12 & tBM13 & tBM14 & tBM15 & tBM16 & tBM17 & tBM18 & tBM19 & tBM20 & tBM21 & tBM22 & tBM23 & tBM24 & tBM25 & tBM26 & tBM27 & tBM28 & tBM29)
                {
                    continue_btn.Enabled = true;
                    back_btn.Enabled = true;
                }
                else
                {
                    continue_btn.Enabled = false;
                    back_btn.Enabled = false;
                }
            }  
        }

        private void B27_TextChanged(object sender, EventArgs e)
        {
            B27.ForeColor = Color.Black;
            bool okay = true;
            //neuer Text darf nur Zeichen aus der Liste digits (in der Klasse deklariert)
            foreach (char c in B27.Text.ToCharArray())
            {
                //sobald es ein unpassendes Zeichen gibt, aufhoeren und Fehlermeldung ausgeben
                if (!letters.Contains<char>(c))
                {
                    B27.ForeColor = Color.Red;
                    okay = false;
                    tB27 = false;
                    continue_btn.Enabled = false;
                    back_btn.Enabled = false;
                    break;
                }
            }
            if (okay == true)
            {
                B27.ForeColor = Color.Black;
                tB27 = true;
                if (tB1 & tB2 & tB3 & tB4 & tB5 & tB6 & tB7 & tB8 & tB9 & tB10 & tB11 & tB12 & tB13 & tB14 & tB15 & tB16 & tB17 & tB18 & tB19 & tB20 & tB21 & tB22 & tB23 & tB24 & tB25 & tB26 & tB27 & tB28 & tB29 & tBM1 & tBM2 & tBM3 & tBM4 & tBM5 & tBM6 & tBM7 & tBM8 & tBM9 & tBM10 & tBM11 & tBM12 & tBM13 & tBM14 & tBM15 & tBM16 & tBM17 & tBM18 & tBM19 & tBM20 & tBM21 & tBM22 & tBM23 & tBM24 & tBM25 & tBM26 & tBM27 & tBM28 & tBM29)
                {
                    continue_btn.Enabled = true;
                    back_btn.Enabled = true;
                }
                else
                {
                    continue_btn.Enabled = false;
                    back_btn.Enabled = false;
                }
            }  
        }

        private void B28_TextChanged(object sender, EventArgs e)
        {
            B28.ForeColor = Color.Black;
            bool okay = true;
            //neuer Text darf nur Zeichen aus der Liste digits (in der Klasse deklariert)
            foreach (char c in B28.Text.ToCharArray())
            {
                //sobald es ein unpassendes Zeichen gibt, aufhoeren und Fehlermeldung ausgeben
                if (!letters.Contains<char>(c))
                {
                    B28.ForeColor = Color.Red;
                    okay = false;
                    tB28 = false;
                    continue_btn.Enabled = false;
                    back_btn.Enabled = false;
                    break;
                }
            }
            if (okay == true)
            {
                B28.ForeColor = Color.Black;
                tB28 = true;
                if (tB1 & tB2 & tB3 & tB4 & tB5 & tB6 & tB7 & tB8 & tB9 & tB10 & tB11 & tB12 & tB13 & tB14 & tB15 & tB16 & tB17 & tB18 & tB19 & tB20 & tB21 & tB22 & tB23 & tB24 & tB25 & tB26 & tB27 & tB28 & tB29 & tBM1 & tBM2 & tBM3 & tBM4 & tBM5 & tBM6 & tBM7 & tBM8 & tBM9 & tBM10 & tBM11 & tBM12 & tBM13 & tBM14 & tBM15 & tBM16 & tBM17 & tBM18 & tBM19 & tBM20 & tBM21 & tBM22 & tBM23 & tBM24 & tBM25 & tBM26 & tBM27 & tBM28 & tBM29)
                {
                    continue_btn.Enabled = true;
                    back_btn.Enabled = true;
                }
                else
                {
                    continue_btn.Enabled = false;
                    back_btn.Enabled = false;
                }
            }  
        }

        private void B29_TextChanged(object sender, EventArgs e)
        {
            B29.ForeColor = Color.Black;
            bool okay = true;
            //neuer Text darf nur Zeichen aus der Liste digits (in der Klasse deklariert)
            foreach (char c in B29.Text.ToCharArray())
            {
                //sobald es ein unpassendes Zeichen gibt, aufhoeren und Fehlermeldung ausgeben
                if (!letters.Contains<char>(c))
                {
                    B29.ForeColor = Color.Red;
                    okay = false;
                    tB29 = false;
                    continue_btn.Enabled = false;
                    back_btn.Enabled = false;
                    break;
                }
            }
            if (okay == true)
            {
                B29.ForeColor = Color.Black;
                tB29 = true;
                if (tB1 & tB2 & tB3 & tB4 & tB5 & tB6 & tB7 & tB8 & tB9 & tB10 & tB11 & tB12 & tB13 & tB14 & tB15 & tB16 & tB17 & tB18 & tB19 & tB20 & tB21 & tB22 & tB23 & tB24 & tB25 & tB26 & tB27 & tB28 & tB29 & tBM1 & tBM2 & tBM3 & tBM4 & tBM5 & tBM6 & tBM7 & tBM8 & tBM9 & tBM10 & tBM11 & tBM12 & tBM13 & tBM14 & tBM15 & tBM16 & tBM17 & tBM18 & tBM19 & tBM20 & tBM21 & tBM22 & tBM23 & tBM24 & tBM25 & tBM26 & tBM27 & tBM28 & tBM29)
                {
                    continue_btn.Enabled = true;
                    back_btn.Enabled = true;
                }
                else
                {
                    continue_btn.Enabled = false;
                    back_btn.Enabled = false;
                }
            }  
        }

        private void default_btn_Click(object sender, EventArgs e)
        {
            setValues();
        }

    }
}
