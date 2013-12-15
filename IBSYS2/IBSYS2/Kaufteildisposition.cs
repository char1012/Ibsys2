﻿using System;
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



        public Kaufteildisposition()
        {
            
            InitializeComponent();

            string databasename = @"Provider=Microsoft.ACE.OLEDB.12.0; Data Source=IBSYS_DB.accdb";
            myconn = new OleDbConnection(databasename);



            OleDbCommand cmd1 = new OleDbCommand();
            OleDbCommand cmd2 = new OleDbCommand();
            OleDbCommand cmd3 = new OleDbCommand();
            OleDbCommand cmd4 = new OleDbCommand();
            OleDbCommand cmd5 = new OleDbCommand();
            OleDbCommand cmd6 = new OleDbCommand();
            OleDbCommand cmd7 = new OleDbCommand();

            cmd1.CommandType = CommandType.Text;
            cmd2.CommandType = CommandType.Text;
            cmd3.CommandType = CommandType.Text;
            cmd4.CommandType = CommandType.Text;
            cmd5.CommandType = CommandType.Text;
            cmd6.CommandType = CommandType.Text;
            cmd7.CommandType = CommandType.Text;

            cmd1.Connection = myconn;
            cmd2.Connection = myconn;
            cmd3.Connection = myconn;
            cmd4.Connection = myconn;
            cmd5.Connection = myconn;
            cmd6.Connection = myconn;
            cmd7.Connection = myconn;

            OleDbDataReader dbReader;
            OleDbDataReader dbReader1;

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

            int komplett = 0;
            int[,] Warteliste_ohne_Mat = new int[99,2];
            cmd1.CommandText = @"SELECT Arbeitszeit_Erzeugnis_FK, Anzahl, Kaufteil_Teilenummer_FK FROM Kaufteil_Arbeitszeit_Erzeugnis;";
            dbReader = cmd1.ExecuteReader();
            while (dbReader.Read())
            {

                int K_TNR_FK = Convert.ToInt32(dbReader["Arbeitszeit_Erzeugnis_FK"]);
                int anz_t = Convert.ToInt32(dbReader["Anzahl"]);
                int kaufteil = Convert.ToInt32(dbReader["Kaufteil_Teilenummer_FK"]);
                cmd2.CommandText = @"SELECT Erzeugnis_Teilenummer_FK, Arbeitsplatz_FK, Reihenfolge FROM Arbeitsplatz_Erzeugnis WHERE ID = " + K_TNR_FK + ";";

                    dbReader1 = cmd2.ExecuteReader();
                    while (dbReader1.Read())
                    {
                        int reihenfolge = Convert.ToInt32(dbReader1["Reihenfolge"]);

                        if (reihenfolge != 1)
                        {
                            int erzeugnis = Convert.ToInt32(dbReader1["Erzeugnis_Teilenummer_FK"]);
                            //Alle Plätze ermitteln, an denen das Erzeugnis-Teil durchkommt
                            int menge = 0;
                            cmd3.CommandText = @"SELECT Erzeugnis_Teilenummer_FK, Arbeitsplatz_FK FROM Arbeitsplatz_Erzeugnis where Erzeugnis_Teilenummer_FK = " + erzeugnis + " AND Reihenfolge <>" + 1 + ";";

                            OleDbDataReader dbReader2 = cmd3.ExecuteReader();
                            while (dbReader2.Read())
                            {
                                int erzeugnis_teil = Convert.ToInt32(dbReader2["Erzeugnis_Teilenummer_FK"]);
                                int arbeitsplatz_fk = Convert.ToInt32(dbReader2["Arbeitsplatz_FK"]);
                                //Suche in Warteliste des jeweiligen Arbeitsplatzes, ob Erzeugnis vorhanden
                                cmd4.CommandText = @"SELECT Arbeitsplatz_FK, Menge FROM Warteliste_Arbeitsplatz where Teilenummer_FK = " + erzeugnis_teil + " AND Arbeitsplatz_FK =" + arbeitsplatz_fk +";";
                                OleDbDataReader dbReader3 = cmd4.ExecuteReader();
                                while (dbReader3.Read())
                                {
                                    //Sichern der Ergebnisse
                                    try
                                    {
                                        menge += Convert.ToInt32(dbReader3["Menge"]);
                                    }
                                    catch(Exception)
                                    {

                                    }
                                }
                                dbReader3.Close();
                                //Suche in Bearbeitung nach den Teilen + eventuelle Addition zur Menge aus Warteliste_Arbeitsplatz
                                cmd5.CommandText = @"SELECT Arbeitsplatz_FK, Menge FROM Bearbeitung where Teilenummer_FK = " + erzeugnis_teil + "and Arbeitsplatz_FK =" + arbeitsplatz_fk + ";";
                                OleDbDataReader dbReader4 = cmd5.ExecuteReader();
                                while (dbReader4.Read())
                                {
                                    try
                                    {
                                        menge += Convert.ToInt32(dbReader4["Menge"]);

                                    }
                                    catch (Exception)
                                    {

                                    }
                                }
                                dbReader4.Close();
                            }
                            dbReader2.Close();
                            komplett = menge * anz_t;
                            menge = 0;
                        }

                    }
                    int a = 0;
                    if (komplett > 0)
                    {
                        Warteliste_ohne_Mat[a, 0] = kaufteil;
                        Warteliste_ohne_Mat[a, 1] = komplett;
                        MessageBox.Show("Kautteil: " + Warteliste_ohne_Mat[a, 0] + " Menge: " + Warteliste_ohne_Mat[a, 1]);
                    }
                    a++;
                    komplett = 0;
                    dbReader1.Close();
            }
            dbReader.Close();

            /*
            cmd6.CommandText = @"SELECT Erzeugnis_Teilenummer_FK, Menge FROM Warteliste_Material WHERE Teilenummer_FK = " + erzeugnis_teil + ";";
            OleDbDataReader dbReader5 = cmd4.ExecuteReader();
            while (dbReader5.Read())
            {
                //Sichern der Ergebnisse
                try
                {
                    int materialWarteliste = Convert.ToInt32(dbReader["Erzeugnis_Teilenummer_FK"]);
                    int mengeWarteliste = Convert.ToInt32(dbReader["Menge"]);
                }
                catch (Exception)
                {

                }

            }
            dbReader5.Close();
            */

            // Muss übergeben werden
            int[,] Prognosen = { {90,190,160},{160,160,160},{160,160,160},{150,150,200}};
            //Die Verwendung der Teile, fix
            int[,] Verwendung = { {1,0,0}, {0,1,0}, {0,0,1}, {7,7,7 }, { 4,4,4 }, { 2,2,2 }, {4,5,6}, {3,3,3}, {0,0,2}, {0,0,72}, {4,4,4}, {1,1,1}, {1,1,1}, {1,1,1}, {2,2,2}, {1,1,1}, {1,1,1}, {2,2,2}, {1,1,1}, {3,3,3}, {1,1,1}, {1,1,1}, {1,1,1}, {2,2,2}, {2,0,0}, {72,0,0}, {0,2,0}, {0,72,0}, {2,2,2} };
            double[,] mengeProdukte = new double[29,29];

            //Lieferfrist und Lieferabweichung, fix
            double[] lieferfrist = new double[] {1.8, 1.7, 1.2, 3.2, 0.9, 0.9, 1.7, 2.1, 1.9, 1.6, 2.2, 1.2, 1.5, 1.7, 1.5, 1.7, 0.9, 1.2, 2.0, 1.0, 1.7, 0.9, 1.1, 1.0, 1.6, 1.6, 1.7, 1.6, 0.7};
            double[] abweichung = new double[] {0.4, 0.4, 0.2, 0.3, 0.2, 0.2, 0.4, 0.5, 0.5, 0.3, 0.4, 0.1, 0.3, 0.4, 0.3, 0.2, 0.2, 0.3, 0.5, 0.2, 0.3, 0.3, 0.1, 0.2, 0.4, 0.2, 0.3, 0.5, 0.2};
            
            //Aus DB + hier muss noch Zugang + Entnahme dazu
            double[] Lager = new double[] { 570, 60, 250, 18490, 4300, 250, 2305, 5500, 735, 21960, 5380, 400, 720, 690, 0, 985, 1440, 1080, 850, 3640, 1650, 1350, 580, 2410, 1210, 34480, 990, 36840, 1100 };


            //Rechnung
            double test = 0;
            int i = 0;

            for (int zaehler = 0; zaehler < 29; zaehler++)
            {
                for (i = 0; i < 4; i++) //Iteration der Produkte P21, P22, P23, ...
                {
                    for (int x = 0; x < 3; x++) //Iteration durch Verwendung 
                    {
                        test = test + (Prognosen[i, x] * Verwendung[zaehler, x]);
                    }
                    mengeProdukte[zaehler,i] = test;
                    test = 0;
                }
            }

            double[] minMenge = new double[30];
            double[] bruttosumme = new double[30];
            double testvalue = 0;
            for (int ramba = 0; ramba < 29; ramba++)
            {
                
                for (int zamba = 0; zamba < 4; zamba++)
                {
                    testvalue = testvalue + mengeProdukte[ramba,zamba];
                }
                bruttosumme[ramba] = testvalue;
                testvalue = 0;
                minMenge[ramba] = bruttosumme[ramba] / 4 * (lieferfrist[ramba] + abweichung[ramba]);
                //MessageBox.Show(" minmenge" + ramba + " " + minMenge[ramba]);    
            }

            double[] Reichweite = new double[29];
            for (int blib = 0; blib < 29; blib++)
            {
                if (Lager[blib] <= mengeProdukte[blib, 1])
                {
                    Reichweite[blib] = Lager[blib] / mengeProdukte[blib, 1];
                }
                else if (Lager[blib] <= mengeProdukte[blib,1] + mengeProdukte[blib, 2])
                {
                    Reichweite[blib] = 1 + (( Lager[blib] - mengeProdukte[blib, 1] ) /  mengeProdukte[blib, 2]) ;
                }
                else if (Lager[blib] <= mengeProdukte[blib,1] + mengeProdukte[blib, 2] + mengeProdukte[blib, 3])
                {
                    Reichweite[blib] = 2 + (( Lager[blib] - mengeProdukte[blib, 1] - mengeProdukte[blib, 2] ) / mengeProdukte[blib, 3]);
                }
                else if (Lager[blib] <= mengeProdukte[blib,1] + mengeProdukte[blib, 2] + mengeProdukte[blib, 3] + mengeProdukte[blib, 4])
                {
                    Reichweite[blib] = 3 + (( Lager[blib] - mengeProdukte[blib, 1] - mengeProdukte[blib, 2] - mengeProdukte[blib, 3] ) / mengeProdukte[blib, 4]);
                }
                else
                    Reichweite[blib] = 4;
                    //MessageBox.Show("Reiweite:" + "zaehler:" + blib + "Menge:" + Reichweite[blib]);
            }

            //Bestellart von 21
            if (Reichweite[0] - (lieferfrist[0] + abweichung[0]) <= 0)
            {
                B1.Text = "E";
            }
            else if (Reichweite[0] - (lieferfrist[0] + abweichung[0]) <= 1)
            {
                B1.Text = "N";
            }
            else
                B1.Text = "";

            //Bestellart von 22
            if (Reichweite[1] - (lieferfrist[1] + abweichung[1]) <= 0)
            {
                B2.Text = "E";
            }
            else if (Reichweite[1] - (lieferfrist[1] + abweichung[1]) <= 1)
            {
                B2.Text = "N";
            }
            else
                B2.Text = "";

            //Bestellart von 23
            if (Reichweite[2] - (lieferfrist[2] + abweichung[2]) <= 0)
            {
                B3.Text = "E";
            }
            else if (Reichweite[2] - (lieferfrist[2] + abweichung[2]) <= 1)
            {
                B3.Text = "N";
            }
            else
                B3.Text = "";

            //Bestellart von 24
            if (Reichweite[3] - (lieferfrist[3] + abweichung[3]) <= 0)
            {
                B4.Text = "E";
            }
            else if (Reichweite[3] - (lieferfrist[3] + abweichung[3]) <= 1)
            {
                B4.Text = "N";
            }
            else
                B4.Text = "";

            //Bestellart von 25
            if (Reichweite[4] - (lieferfrist[4] + abweichung[4]) <= 0)
            {
                B5.Text = "E";
            }
            else if (Reichweite[4] - (lieferfrist[4] + abweichung[4]) <= 1)
            {
                B5.Text = "N";
            }
            else
                B5.Text = "";

            //Bestellart von 27
            if (Reichweite[5] - (lieferfrist[5] + abweichung[5]) <= 0)
            {
                B7.Text = "E";
            }
            else if (Reichweite[5] - (lieferfrist[5] + abweichung[5]) <= 1)
            {
                B7.Text = "N";
            }
            else
                B7.Text = "";

            //Bestellart von 28
            if (Reichweite[6] - (lieferfrist[6] + abweichung[6]) <= 0)
            {
                B8.Text = "E";
            }
            else if (Reichweite[6] - (lieferfrist[6] + abweichung[6]) <= 1)
            {
                B8.Text = "N";
            }
            else
                B8.Text = "";

            //Bestellart von 32
            if (Reichweite[7] - (lieferfrist[7] + abweichung[7]) <= 0)
            {
                B12.Text = "E";
            }
            else if (Reichweite[7] - (lieferfrist[7] + abweichung[7]) <= 1)
            {
                B12.Text = "N";
            }
            else
                B12.Text = "";

            //Bestellart von 33
            if (Reichweite[8] - (lieferfrist[8] + abweichung[8]) <= 0)
            {
                B13.Text = "E";
            }
            else if (Reichweite[8] - (lieferfrist[8] + abweichung[8]) <= 1)
            {
                B13.Text = "N";
            }
            else
                B13.Text = "";

            //Bestellart von 34
            if (Reichweite[9] - (lieferfrist[9] + abweichung[9]) <= 0)
            {
                B14.Text = "E";
            }
            else if (Reichweite[9] - (lieferfrist[9] + abweichung[9]) <= 1)
            {
                B14.Text = "N";
            }
            else
                B14.Text = "";

            //Bestellart von 35
            if (Reichweite[10] - (lieferfrist[10] + abweichung[10]) <= 0)
            {
                B15.Text = "E";
            }
            else if (Reichweite[10] - (lieferfrist[10] + abweichung[10]) <= 1)
            {
                B15.Text = "N";
            }
            else
                B15.Text = "";

            //Bestellart von 36
            if (Reichweite[11] - (lieferfrist[11] + abweichung[11]) <= 0)
            {
                B16.Text = "E";
            }
            else if (Reichweite[11] - (lieferfrist[11] + abweichung[11]) <= 1)
            {
                B16.Text = "N";
            }
            else
                B16.Text = "";

            //Bestellart von 37
            if (Reichweite[12] - (lieferfrist[12] + abweichung[12]) <= 0)
            {
                B17.Text = "E";
            }
            else if (Reichweite[12] - (lieferfrist[12] + abweichung[12]) <= 1)
            {
                B17.Text = "N";
            }
            else
                B17.Text = "";

            //Bestellart von 38
            if (Reichweite[13] - (lieferfrist[13] + abweichung[13]) <= 0)
            {
                B18.Text = "E";
            }
            else if (Reichweite[13] - (lieferfrist[13] + abweichung[13]) <= 1)
            {
                B18.Text = "N";
            }
            else
                B18.Text = "";

            //Bestellart von 39
            if (Reichweite[14] - (lieferfrist[14] + abweichung[14]) <= 0)
            {
                B19.Text = "E";
            }
            else if (Reichweite[14] - (lieferfrist[14] + abweichung[14]) <= 1)
            {
                B19.Text = "N";
            }
            else
                B19.Text = "";

            //Bestellart von 40
            if (Reichweite[15] - (lieferfrist[15] + abweichung[15]) <= 0)
            {
                B20.Text = "E";
            }
            else if (Reichweite[15] - (lieferfrist[15] + abweichung[15]) <= 1)
            {
                B20.Text = "N";
            }
            else
                B20.Text = "";

            //Bestellart von 41
            if (Reichweite[16] - (lieferfrist[16] + abweichung[16]) <= 0)
            {
                B21.Text = "E";
            }
            else if (Reichweite[16] - (lieferfrist[16] + abweichung[16]) <= 1)
            {
                B21.Text = "N";
            }
            else
                B21.Text = "";

            //Bestellart von 42
            if (Reichweite[17] - (lieferfrist[17] + abweichung[17]) <= 0)
            {
                B22.Text = "E";
            }
            else if (Reichweite[17] - (lieferfrist[17] + abweichung[17]) <= 1)
            {
                B22.Text = "N";
            }
            else
                B22.Text = "";

            //Bestellart von 43
            if (Reichweite[18] - (lieferfrist[18] + abweichung[18]) <= 0)
            {
                B23.Text = "E";
            }
            else if (Reichweite[18] - (lieferfrist[18] + abweichung[18]) <= 1)
            {
                B23.Text = "N";
            }
            else
                B23.Text = "";

            //Bestellart von 44
            if (Reichweite[19] - (lieferfrist[19] + abweichung[19]) <= 0)
            {
                B24.Text = "E";
            }
            else if (Reichweite[19] - (lieferfrist[19] + abweichung[19]) <= 1)
            {
                B24.Text = "N";
            }
            else
                B24.Text = "";

            //Bestellart von 45
            if (Reichweite[20] - (lieferfrist[20] + abweichung[20]) <= 0)
            {
                B25.Text = "E";
            }
            else if (Reichweite[20] - (lieferfrist[20] + abweichung[20]) <= 1)
            {
                B25.Text = "N";
            }
            else
                B25.Text = "";

            //Bestellart von 46
            if (Reichweite[21] - (lieferfrist[21] + abweichung[21]) <= 0)
            {
                B26.Text = "E";
            }
            else if (Reichweite[21] - (lieferfrist[21] + abweichung[21]) <= 1)
            {
                B26.Text = "N";
            }
            else
                B26.Text = "";

            //Bestellart von 47
            if (Reichweite[22] - (lieferfrist[22] + abweichung[22]) <= 0)
            {
                B27.Text = "E";
            }
            else if (Reichweite[22] - (lieferfrist[22] + abweichung[22]) <= 1)
            {
                B27.Text = "N";
            }
            else
                B27.Text = "";

            //Bestellart von 48
            if (Reichweite[23] - (lieferfrist[23] + abweichung[23]) <= 0)
            {
                B28.Text = "E";
            }
            else if (Reichweite[23] - (lieferfrist[23] + abweichung[23]) <= 1)
            {
                B28.Text = "N";
            }
            else
                B28.Text = "";

            //Bestellart von 52
            if (Reichweite[24] - (lieferfrist[24] + abweichung[24]) <= 0)
            {
                B32.Text = "E";
            }
            else if (Reichweite[24] - (lieferfrist[24] + abweichung[24]) <= 1)
            {
                B32.Text = "N";
            }
            else
                B32.Text = "";

            //Bestellart von 53
            if (Reichweite[25] - (lieferfrist[25] + abweichung[25]) <= 0)
            {
                B33.Text = "E";
            }
            else if (Reichweite[25] - (lieferfrist[25] + abweichung[25]) <= 1)
            {
                B33.Text = "N";
            }
            else
                B33.Text = "";

            //Bestellart von 57
            if (Reichweite[26] - (lieferfrist[26] + abweichung[26]) <= 0)
            { 
                B37.Text = "E";
            }
            else if (Reichweite[26] - (lieferfrist[26] + abweichung[26]) <= 1)
            {
                B37.Text = "N";
            }
            else
                B37.Text = "";

            //Bestellart von 58
            if (Reichweite[27] - (lieferfrist[27] + abweichung[27]) <= 0)
            {
                B38.Text = "E";
            }
            else if (Reichweite[27] - (lieferfrist[27] + abweichung[27]) <= 1)
            {
                B38.Text = "N";
            }
            else
                B38.Text = "";

            //Bestellart von 59
            if (Reichweite[28] - (lieferfrist[28] + abweichung[28]) <= 0)
            {
                B39.Text = "E";
            }
            else if (Reichweite[28] - (lieferfrist[28] + abweichung[28]) <= 1)
            {
                B39.Text = "N";
            }
            else
                B38.Text = "";




            if (B1.Text != "") M1.Text = "" + minMenge[0];
            if (B2.Text != "") M2.Text = "" + minMenge[1];
            if (B3.Text != "") M3.Text = "" + minMenge[2];
            if (B4.Text != "") M4.Text = "" + minMenge[3];
            if (B5.Text != "") M5.Text = "" + minMenge[4];
            if (B7.Text != "") M7.Text = "" + minMenge[5];
            if (B8.Text != "") M8.Text = "" + minMenge[6];
            if (B12.Text != "") M12.Text = "" + minMenge[7];
            if (B13.Text != "") M13.Text = "" + minMenge[8];
            if (B14.Text != "") M14.Text = "" + minMenge[9];
            if (B15.Text != "") M15.Text = "" + minMenge[10];
            if (B16.Text != "") M16.Text = "" + minMenge[11];
            if (B17.Text != "") M17.Text = "" + minMenge[12];
            if (B18.Text != "") M18.Text = "" + minMenge[13];
            if (B19.Text != "") M19.Text = "" + minMenge[14];
            if (B20.Text != "") M20.Text = "" + minMenge[15];
            if (B21.Text != "") M21.Text = "" + minMenge[16];
            if (B22.Text != "") M22.Text = "" + minMenge[17];
            if (B23.Text != "") M23.Text = "" + minMenge[18];
            if (B24.Text != "") M24.Text = "" + minMenge[19];
            if (B25.Text != "") M25.Text = "" + minMenge[20];
            if (B26.Text != "") M26.Text = "" + minMenge[21];
            if (B27.Text != "") M27.Text = "" + minMenge[22];
            if (B28.Text != "") M28.Text = "" + minMenge[23];
            if (B32.Text != "") M32.Text = "" + minMenge[24];
            if (B33.Text != "") M33.Text = "" + minMenge[25];
            if (B37.Text != "") M37.Text = "" + minMenge[26];
            if (B38.Text != "") M38.Text = "" + minMenge[27];
            if (B39.Text != "") M39.Text = "" + minMenge[28];

      
        // Dicountmenge
        int iks = 0;
        int[] discountmenge = new int[29];
        cmd7.CommandText = @"SELECT Teilenummer, Diskontmenge FROM Teil where Diskontmenge > 0 ORDER BY Teilenummer ASC;";
        OleDbDataReader dbReader6 = cmd7.ExecuteReader();
        while (dbReader6.Read())
        {
            discountmenge[iks] = Convert.ToInt32(dbReader6["Diskontmenge"]);
            iks++;
        }
        D1.Text = "" + discountmenge[0];
        D2.Text = "" + discountmenge[1];
        D3.Text = "" + discountmenge[2];
        D4.Text = "" + discountmenge[3];
        D5.Text = "" + discountmenge[4];
        D6.Text = "" + discountmenge[5];
        D7.Text = "" + discountmenge[6];
        D8.Text = "" + discountmenge[7];
        D9.Text = "" + discountmenge[8];
        D10.Text = "" + discountmenge[9];
        D11.Text = "" + discountmenge[10];
        D12.Text = "" + discountmenge[11];
        D13.Text = "" + discountmenge[12];
        D14.Text = "" + discountmenge[13];
        D15.Text = "" + discountmenge[14];
        D16.Text = "" + discountmenge[15];
        D17.Text = "" + discountmenge[16];
        D18.Text = "" + discountmenge[17];
        D19.Text = "" + discountmenge[18];
        D20.Text = "" + discountmenge[19];
        D21.Text = "" + discountmenge[20];
        D22.Text = "" + discountmenge[21];
        D23.Text = "" + discountmenge[22];
        D24.Text = "" + discountmenge[23];
        D25.Text = "" + discountmenge[24];
        D26.Text = "" + discountmenge[25];
        D27.Text = "" + discountmenge[26];
        D28.Text = "" + discountmenge[27];
        D29.Text = "" + discountmenge[28];
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Kaufteildisposition_Load(object sender, EventArgs e)
        {
            

        }

        private void textBox19_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox40_TextChanged(object sender, EventArgs e)
        {

        }

        private void continue_btn_Click(object sender, EventArgs e)
        {
            this.Controls.Clear();
            UserControl ergebnis = new Ergebnis();
            this.Controls.Add(ergebnis);
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void M20_TextChanged(object sender, EventArgs e)
        {

        }

        private void B25_TextChanged(object sender, EventArgs e)
        {

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
            this.Controls.Clear();
            UserControl kapazitaet = new Kapazitaetsplan();
            this.Controls.Add(kapazitaet);
        }

        private void lbl_Startseite_Click(object sender, EventArgs e)
        {
            this.Controls.Clear();
            UserControl import = new ImportPrognose();
            this.Controls.Add(import);
        }

        private void lbl_Sicherheitsbestand_Click(object sender, EventArgs e)
        {
            this.Controls.Clear();
            UserControl sicherheit = new Sicherheitsbestand();
            this.Controls.Add(sicherheit);
        }

        private void lbl_Produktion_Click(object sender, EventArgs e)
        {
            this.Controls.Clear();
            UserControl prod = new Produktion();
            this.Controls.Add(prod);
        }

        private void lbl_Produktionsreihenfolge_Click(object sender, EventArgs e)
        {
            this.Controls.Clear();
            UserControl prodreihenfolge = new Produktionsreihenfolge();
            this.Controls.Add(prodreihenfolge);
        }

        private void lbl_Kapazitaetsplan_Click(object sender, EventArgs e)
        {
            this.Controls.Clear();
            UserControl kapazitaet = new Kapazitaetsplan();
            this.Controls.Add(kapazitaet);
        }

        private void lbl_Ergebnis_Click(object sender, EventArgs e)
        {
            this.Controls.Clear();
            UserControl ergebnis = new Ergebnis();
            this.Controls.Add(ergebnis);
        }

        private void D11_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
