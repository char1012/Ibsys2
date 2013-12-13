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
        int periode;
        int[] auftraege;
        int[] direktverkaeufe;
        int[,] sicherheitsbest;
        int[,] produktion;
        int[] produktionProg;
        int[,] kapazitaet;
        int[,] kaufauftraege;
        int[] storevalues;

        public Ergebnis()
        {
            InitializeComponent();
            result();
        }

        public void result()
        {
            // simulieren (sollen spaeter Parameter sein)
            // Zahlen entsprechen Werten auf Excel-Sheet / Periode 7
            // Periode (nicht fuer XML-Export benoetigt)
            periode = 6;
            // Auftraege (aktuelle Periode + prognostizierte Periode) {aktP1, aktP2, aktP3, n+1P1, n+1P2, ...}
            auftraege = new int[12]{100,200,100,150,150,150,150,150,150,150,150,200};
            // Direktverkaeufe
            direktverkaeufe = new int[3]{0,0,0};
            // Sicherheitsbestaende (nicht fuer XML-Export benoetigt)
            sicherheitsbest = new int[30, 2];
            sicherheitsbest[0, 0] = 1;
            sicherheitsbest[0, 1] = 70; // Teil p1 mit 70 Stueck Sicherheitsbestand
            sicherheitsbest[1, 0] = 2;
            sicherheitsbest[1, 1] = 80;
            sicherheitsbest[2, 0] = 3;
            sicherheitsbest[2, 1] = 230;
            sicherheitsbest[3, 0] = 4;
            sicherheitsbest[3, 1] = 70;
            sicherheitsbest[4, 0] = 5;
            sicherheitsbest[4, 1] = 80;
            sicherheitsbest[5, 0] = 6;
            sicherheitsbest[5, 1] = 80;
            sicherheitsbest[6, 0] = 7;
            sicherheitsbest[6, 1] = 70;
            sicherheitsbest[7, 0] = 8;
            sicherheitsbest[7, 1] = 80;
            sicherheitsbest[8, 0] = 9;
            sicherheitsbest[8, 1] = 80;
            sicherheitsbest[9, 0] = 10;
            sicherheitsbest[9, 1] = 70;
            sicherheitsbest[10, 0] = 11;
            sicherheitsbest[10, 1] = 80;
            sicherheitsbest[11, 0] = 12;
            sicherheitsbest[11, 1] = 80;
            sicherheitsbest[12, 0] = 13;
            sicherheitsbest[12, 1] = 70;
            sicherheitsbest[13, 0] = 14;
            sicherheitsbest[13, 1] = 80;
            sicherheitsbest[14, 0] = 15;
            sicherheitsbest[14, 1] = 80;
            sicherheitsbest[15, 0] = 16;
            sicherheitsbest[15, 1] = 70;
            sicherheitsbest[16, 0] = 17;
            sicherheitsbest[16, 1] = 70;
            sicherheitsbest[17, 0] = 18;
            sicherheitsbest[17, 1] = 70;
            sicherheitsbest[18, 0] = 19;
            sicherheitsbest[18, 1] = 80;
            sicherheitsbest[19, 0] = 20;
            sicherheitsbest[19, 1] = 80;
            sicherheitsbest[20, 0] = 26;
            sicherheitsbest[20, 1] = 70;
            sicherheitsbest[21, 0] = 29;
            sicherheitsbest[21, 1] = 80;
            sicherheitsbest[22, 0] = 30;
            sicherheitsbest[22, 1] = 80;
            sicherheitsbest[23, 0] = 31;
            sicherheitsbest[23, 1] = 80;
            sicherheitsbest[24, 0] = 49;
            sicherheitsbest[24, 1] = 70;
            sicherheitsbest[25, 0] = 50;
            sicherheitsbest[25, 1] = 70;
            sicherheitsbest[26, 0] = 51;
            sicherheitsbest[26, 1] = 70;
            sicherheitsbest[27, 0] = 54;
            sicherheitsbest[27, 1] = 80;
            sicherheitsbest[28, 0] = 55;
            sicherheitsbest[28, 1] = 80;
            sicherheitsbest[29, 0] = 56;
            sicherheitsbest[29, 1] = 80;
            // Produktion aktuelle Periode (P- und E-Teile) - in korrekter Produktionsreihenfolge
            produktion = new int[30, 2];
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
            produktion[5, 1] = -110;
            produktion[6, 0] = 7;
            produktion[6, 1] = 50;
            produktion[7, 0] = 8;
            produktion[7, 1] = 150;
            produktion[8, 0] = 9;
            produktion[8, 1] = -200;
            produktion[9, 0] = 10;
            produktion[9, 1] = 60;
            produktion[10, 0] = 11;
            produktion[10, 1] = 160;
            produktion[11, 0] = 12;
            produktion[11, 1] = -110;
            produktion[12, 0] = 13;
            produktion[12, 1] = 50;
            produktion[13, 0] = 14;
            produktion[13, 1] = 150;
            produktion[14, 0] = 15;
            produktion[14, 1] = -200;
            produktion[15, 0] = 16;
            produktion[15, 1] = 20 + 130 + 90;
            produktion[16, 0] = 17;
            produktion[16, 1] = 20 + 130 + 90;
            produktion[17, 0] = 18;
            produktion[17, 1] = 50;
            produktion[18, 0] = 19;
            produktion[18, 1] = 150;
            produktion[19, 0] = 20;
            produktion[19, 1] = -200;
            produktion[20, 0] = 26;
            produktion[20, 1] = 50 + 160 + 130;
            produktion[21, 0] = 29;
            produktion[21, 1] = -110;
            produktion[22, 0] = 30;
            produktion[22, 1] = -20;
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
            // Produktion prognostizierte Perioden (nur P-Teile, nicht fuer XML-Export benoetigt)
            // {n+1P1, n+1P2, n+1P3, n+2P1, n+2P2, ...}
            produktionProg = new int[9]{160,160,160,160,160,160,150,150,200};
            // Schichten und Ueberstunden
            kapazitaet = new int[14, 3];
            kapazitaet[0, 0] = 1;
            kapazitaet[0, 1] = 1;
            kapazitaet[0, 2] = 110;
            kapazitaet[1, 0] = 2;
            kapazitaet[1, 1] = 1;
            kapazitaet[1, 2] = 19;
            kapazitaet[2, 0] = 3;
            kapazitaet[2, 1] = 1;
            kapazitaet[2, 2] = 12;
            kapazitaet[3, 0] = 4;
            kapazitaet[3, 1] = 1;
            kapazitaet[3, 2] = 161;
            kapazitaet[4, 0] = 6;
            kapazitaet[4, 1] = 1;
            kapazitaet[4, 2] = 0;
            kapazitaet[5, 0] = 7;
            kapazitaet[5, 1] = 1;
            kapazitaet[5, 2] = 190;
            kapazitaet[6, 0] = 8;
            kapazitaet[6, 1] = 1;
            kapazitaet[6, 2] = 20;
            kapazitaet[7, 0] = 9;
            kapazitaet[7, 1] = 1;
            kapazitaet[7, 2] = 240;
            kapazitaet[8, 0] = 10;
            kapazitaet[8, 1] = 2;
            kapazitaet[8, 2] = 5;
            kapazitaet[9, 0] = 11;
            kapazitaet[9, 1] = 2;
            kapazitaet[9, 2] = 0;
            kapazitaet[10, 0] = 12;
            kapazitaet[10, 1] = 1;
            kapazitaet[10, 2] = 130;
            kapazitaet[11, 0] = 13;
            kapazitaet[11, 1] = 1;
            kapazitaet[11, 2] = 0;
            kapazitaet[12, 0] = 14;
            kapazitaet[12, 1] = 1;
            kapazitaet[12, 2] = 0;
            kapazitaet[13, 0] = 15;
            kapazitaet[13, 1] = 1;
            kapazitaet[13, 2] = 14;
            // Kaufauftraege
            // 5 = normal, 4 = express, 0 = keine Bestellung
            kaufauftraege = new int[29, 3];
            kaufauftraege[0, 0] = 21;
            kaufauftraege[0, 1] = 0;
            kaufauftraege[0, 2] = 0;
            kaufauftraege[1, 0] = 22;
            kaufauftraege[1, 1] = 0;
            kaufauftraege[1, 2] = 0;
            kaufauftraege[2, 0] = 23;
            kaufauftraege[2, 1] = 240;
            kaufauftraege[2, 2] = 5;
            kaufauftraege[3, 0] = 24;
            kaufauftraege[3, 1] = 0;
            kaufauftraege[3, 2] = 0;
            kaufauftraege[4, 0] = 25;
            kaufauftraege[4, 1] = 3600;
            kaufauftraege[4, 2] = 5;
            kaufauftraege[5, 0] = 27;
            kaufauftraege[5, 1] = 0;
            kaufauftraege[5, 2] = 0;
            kaufauftraege[6, 0] = 28;
            kaufauftraege[6, 1] = 0;
            kaufauftraege[6, 2] = 0;
            kaufauftraege[7, 0] = 32;
            kaufauftraege[7, 1] = 3730;
            kaufauftraege[7, 2] = 5;
            kaufauftraege[8, 0] = 33;
            kaufauftraege[8, 1] = 820;
            kaufauftraege[8, 2] = 4;
            kaufauftraege[9, 0] = 34;
            kaufauftraege[9, 1] = 23300;
            kaufauftraege[9, 2] = 4;
            kaufauftraege[10, 0] = 35;
            kaufauftraege[10, 1] = 0;
            kaufauftraege[10, 2] = 0;
            kaufauftraege[11, 0] = 36;
            kaufauftraege[11, 1] = 625;
            kaufauftraege[11, 2] = 5;
            kaufauftraege[12, 0] = 37;
            kaufauftraege[12, 1] = 0;
            kaufauftraege[12, 2] = 0;
            kaufauftraege[13, 0] = 38;
            kaufauftraege[13, 1] = 0;
            kaufauftraege[13, 2] = 0;
            kaufauftraege[14, 0] = 39;
            kaufauftraege[14, 1] = 1800;
            kaufauftraege[14, 2] = 4;
            kaufauftraege[15, 0] = 40;
            kaufauftraege[15, 1] = 0;
            kaufauftraege[15, 2] = 0;
            kaufauftraege[16, 0] = 41;
            kaufauftraege[16, 1] = 0;
            kaufauftraege[16, 2] = 0;
            kaufauftraege[17, 0] = 42;
            kaufauftraege[17, 1] = 1800;
            kaufauftraege[17, 2] = 5;
            kaufauftraege[18, 0] = 43;
            kaufauftraege[18, 1] = 0;
            kaufauftraege[18, 2] = 0;
            kaufauftraege[19, 0] = 44;
            kaufauftraege[19, 1] = 0;
            kaufauftraege[19, 2] = 0;
            kaufauftraege[20, 0] = 45;
            kaufauftraege[20, 1] = 0;
            kaufauftraege[20, 2] = 0;
            kaufauftraege[21, 0] = 46;
            kaufauftraege[21, 1] = 0;
            kaufauftraege[21, 2] = 0;
            kaufauftraege[22, 0] = 47;
            kaufauftraege[22, 1] = 0;
            kaufauftraege[22, 2] = 0;
            kaufauftraege[23, 0] = 48;
            kaufauftraege[23, 1] = 0;
            kaufauftraege[23, 2] = 0;
            kaufauftraege[24, 0] = 52;
            kaufauftraege[24, 1] = 0;
            kaufauftraege[24, 2] = 0;
            kaufauftraege[25, 0] = 53;
            kaufauftraege[25, 1] = 0;
            kaufauftraege[25, 2] = 0;
            kaufauftraege[26, 0] = 57;
            kaufauftraege[26, 1] = 660;
            kaufauftraege[26, 2] = 5;
            kaufauftraege[27, 0] = 58;
            kaufauftraege[27, 1] = 25000;
            kaufauftraege[27, 2] = 5;
            kaufauftraege[28, 0] = 59;
            kaufauftraege[28, 1] = 900;
            kaufauftraege[28, 2] = 5;

            storevalues = calculateStorevalue(periode, auftraege, direktverkaeufe, produktion);

            // Textfelder fuellen
            

        }

        private int[] calculateStorevalue(int periode, int[] auftraege, int[] direktverkaeufe, int[,] produktion)
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
                System.Windows.Forms.MessageBox.Show("DB-Verbindung wurde nicht ordnungsgemäß geschlossen bei der letzten Verwendung, Verbindung wird neu gestartet, bitte haben Sie einen Moment Geduld.");
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
            cmd.CommandText = @"SELECT Teilenummer_FK, Teilewert FROM Lager WHERE Periode = " + periode + ";";
            dbReader = cmd.ExecuteReader();
            int n = 0;
            while (dbReader.Read()) // hier sollte nur eine Zeile herauskommen
            {
                teilewerte[n, 0] = Convert.ToDouble(dbReader["Teilenummer_FK"]);
                teilewerte[n, 1] = Convert.ToDouble(dbReader["Teilewert"]);
                n++;
            }
            dbReader.Close();

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
                        wertVerkaeufe += (direktverkaeufe[i] * teilewerte[no, 1]);
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

            MessageBox.Show(storevalue[0] + " " + storevalue[1] + " " + storevalue[2]);    

            return storevalue;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        public void sprachen(String sprache)
        {
            if (sprache != "de")
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

                //EN Groupboxen
                groupBox2.Text = (Sprachen.EN_ER_GROUPBOX2);
                groupBox3.Text = (Sprachen.EN_ER_GROUPBOX3);
                groupBox4.Text = (Sprachen.EN_ER_GROUPBOX4);
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

                //DE Groupboxen
                groupBox2.Text = (Sprachen.DE_ER_GROUPBOX2);
                groupBox3.Text = (Sprachen.DE_ER_GROUPBOX3);
                groupBox4.Text = (Sprachen.DE_ER_GROUPBOX4);
            }
        }

        private void pic_en_Click(object sender, EventArgs e)
        {
            string sprache = "en";
            sprachen(sprache);
        }

        private void pic_de_Click(object sender, EventArgs e)
        {
            string sprache = "de";
            sprachen(sprache);
        }
    }
}
