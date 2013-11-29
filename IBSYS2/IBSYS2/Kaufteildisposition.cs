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
            
            //Aus DB
            double[] Lager = new double[] { 570, 60, 250, 18490, 4300, 250, 2305, 5500, 735, 21960, 5380, 400, 720, 690, 0, 985, 1440, 1080, 850, 3640, 1650, 1350, 580, 2410, 1210, 34480, 990, 36840, 1100 };


            //Rechnung
            double test = 0;
            int i = 0;
            int t = 0;
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


            M1.Text = "" + minMenge[0];
            M2.Text = "" + minMenge[1];
            M3.Text = "" + minMenge[2];
            M4.Text = "" + minMenge[3];
            M5.Text = "" + minMenge[4];
            M7.Text = "" + minMenge[5];
            M8.Text = "" + minMenge[6];
            M12.Text = "" + minMenge[7];
            M13.Text = "" + minMenge[8];
            M14.Text = "" + minMenge[9];
            M15.Text = "" + minMenge[10];
            M16.Text = "" + minMenge[11];
            M17.Text = "" + minMenge[12];
            M18.Text = "" + minMenge[13];
            M19.Text = "" + minMenge[14];
            M20.Text = "" + minMenge[15];
            M21.Text = "" + minMenge[16];
            M22.Text = "" + minMenge[17];
            M23.Text = "" + minMenge[18];
            M24.Text = "" + minMenge[19];
            M25.Text = "" + minMenge[20];
            M26.Text = "" + minMenge[21];
            M27.Text = "" + minMenge[22];
            M28.Text = "" + minMenge[23];
            M32.Text = "" + minMenge[24];
            M33.Text = "" + minMenge[25];
            M37.Text = "" + minMenge[26];
            M38.Text = "" + minMenge[27];
            M39.Text = "" + minMenge[28];

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
    }
}
