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
    public partial class Kaufteildisposition : Form
    {
        private OleDbConnection myconn;
        public Kaufteildisposition()
        {
            
            InitializeComponent();

            string databasename = @"Provider=Microsoft.ACE.OLEDB.12.0; Data Source=IBSYS_DB.accdb";
            myconn = new OleDbConnection(databasename);
            OleDbCommand cmd = new OleDbCommand();
            cmd.CommandType = CommandType.Text;
            cmd.Connection = myconn;
            OleDbCommand cmd2 = new OleDbCommand();
            cmd2.CommandType = CommandType.Text;
            cmd2.Connection = myconn;
            OleDbCommand cmd3 = new OleDbCommand();
            cmd3.CommandType = CommandType.Text;
            cmd3.Connection = myconn;
            try
            {
                myconn.Open();
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("DB-Verbindung wurde nicht ordnungsgemäß geschlossen bei der letzten Verwendung, Verbindung wird neu gestartet, bitte haben Sie einen Moment Geduld.");
                myconn.Close();
                myconn.Open();
            }

            int[,] Prognosen = { {90,190,160},{160,160,160},{160,160,160},{150,150,200}};
            int[,] Verwendung = { {1,0,0}, {0,1,0}, {0,0,1}, {7,7,7 }, { 4,4,4 }, { 2,2,2 }, {4,5,6}, {3,3,3}, {0,0,2}, {0,0,72}, {4,4,4}, {1,1,1}, {1,1,1}, {1,1,1}, {2,2,2}, {1,1,1}, {1,1,1}, {2,2,2}, {1,1,1}, {3,3,3}, {1,1,1}, {1,1,1}, {1,1,1}, {2,2,2}, {2,0,0}, {72,0,0}, {0,2,0}, {0,72,0}, {2,2,2} };
            double[,] mengeProdukte = new double[29,29];

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
            M1.Text = "test";
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
    }
}
