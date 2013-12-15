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
    public partial class Produktion_ETeile : Form
    {
        private OleDbConnection myconn;
        private char[] digits = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
        int periode;
        int produktionp1;
        int produktionp2;
        int produktionp3;
        List<int> sicherheitsbe = new List<int>();

        public Produktion_ETeile(int per, string p1, string p2, string p3, List<int> sicherheitsbestand)
        {
            InitializeComponent();
            string databasename = @"Provider=Microsoft.ACE.OLEDB.12.0; Data Source=IBSYS_DB.accdb";
            myconn = new OleDbConnection(databasename);

            this.periode = per;
            this.produktionp1 = Convert.ToInt32(p1);
            this.produktionp2 = Convert.ToInt32(p2);
            this.produktionp3 = Convert.ToInt32(p3);
            this.sicherheitsbe = sicherheitsbestand;

            berechneProduktion();
        }

        private void berechneProduktion()
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
		    p26 = produktionp1 + sicherheitsbe[0] + sicherheitsbe[11] + sicherheitsbe[22];
            p51 = produktionp1 + sicherheitsbe[1];

            p16 = p51 + sicherheitsbe[2] + sicherheitsbe[13] + sicherheitsbe[24];
            p17 = p51 + sicherheitsbe[3] + sicherheitsbe[14] + sicherheitsbe[25];
            p50 = p51 + sicherheitsbe[4];

            p4 = p50 + sicherheitsbe[5];
            p10 = p50 + sicherheitsbe[6];
            p49 = p50 + sicherheitsbe[7];

            p7 = p49 + sicherheitsbe[8];
            p13 = p49 + sicherheitsbe[9];
            p18 = p49 + sicherheitsbe[10];

            p56 = produktionp2 + sicherheitsbe[12];

            p55 = p56 + sicherheitsbe[15];

            p5 = p55 + sicherheitsbe[16];
            p11 = p55 + sicherheitsbe[17];
            p54 = p55 + sicherheitsbe[18];

            p8 = p54 + sicherheitsbe[19];
            p14 = p54 + sicherheitsbe[20];
            p19 = p54 + sicherheitsbe[21];

            p31 = produktionp3 + sicherheitsbe[23];

            p30 = p31 + sicherheitsbe[26];

            p6 = p30 + sicherheitsbe[27];
            p12 = p30 + sicherheitsbe[28];
            p29 = p30 + sicherheitsbe[29];

            p9 = p29 + sicherheitsbe[30];
            p15 = p29 + sicherheitsbe[31];
            p20 = p29 + sicherheitsbe[32]; 
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
                     if (bearbeitung[i][0] == teilenummer[2])
                     {
                         p16 -= bearbeitung[i][1];
                     }
                     if (bearbeitung[i][0] == teilenummer[3])
                     {
                         p17 -= bearbeitung[i][1];
                     }
                     if (bearbeitung[i][0] == teilenummer[4])
                     {
                         p50 -= bearbeitung[i][1];
                     }
                     if (bearbeitung[i][0] == teilenummer[5])
                     {
                         p4 -= bearbeitung[i][1];
                     }
                     if (bearbeitung[i][0] == teilenummer[6])
                     {
                         p10 -= bearbeitung[i][1];
                     }
                     if (bearbeitung[i][0] == teilenummer[7])
                     {
                         p49 -= bearbeitung[i][1];
                     }
                     if (bearbeitung[i][0] == teilenummer[8])
                     {
                         p7 -= bearbeitung[i][1];
                     }
                     if (bearbeitung[i][0] == teilenummer[9])
                     {
                         p13 -= bearbeitung[i][1];
                     }
                     if (bearbeitung[i][0] == teilenummer[10])
                     {
                         p18 -= bearbeitung[i][1];
                     }
                     if (bearbeitung[i][0] == teilenummer[11])
                     {
                         p56 -= bearbeitung[i][1];
                     }
                     if (bearbeitung[i][0] == teilenummer[12])
                     {
                         p55 -= bearbeitung[i][1];
                     }
                     if (bearbeitung[i][0] == teilenummer[13])
                     {
                         p5 -= bearbeitung[i][1];
                     }
                     if (bearbeitung[i][0] == teilenummer[14])
                     {
                         p11 -= bearbeitung[i][1];
                     }
                     if (bearbeitung[i][0] == teilenummer[15])
                     {
                         p54 -= bearbeitung[i][1];
                     }
                     if (bearbeitung[i][0] == teilenummer[16])
                     {
                         p8 -= bearbeitung[i][1];
                     }
                     if (bearbeitung[i][0] == teilenummer[17])
                     {
                         p14 -= bearbeitung[i][1];
                     }
                     if (bearbeitung[i][0] == teilenummer[18])
                     {
                         p19 -= bearbeitung[i][1];
                     }
                     if (bearbeitung[i][0] == teilenummer[19])
                     {
                         p31 -= bearbeitung[i][1];
                     }
                     if (bearbeitung[i][0] == teilenummer[20])
                     {
                         p30 -= bearbeitung[i][1];
                     }
                     if (bearbeitung[i][0] == teilenummer[21])
                     {
                         p6 -= bearbeitung[i][1];
                     }
                     if (bearbeitung[i][0] == teilenummer[22])
                     {
                         p12 -= bearbeitung[i][1];
                     }
                     if (bearbeitung[i][0] == teilenummer[23])
                     {
                         p29 -= bearbeitung[i][1];
                     }
                     if (bearbeitung[i][0] == teilenummer[24])
                     {
                         p9 -= bearbeitung[i][1];
                     }
                     if (bearbeitung[i][0] == teilenummer[25])
                     {
                         p15 -= bearbeitung[i][1];
                     }
                     if (bearbeitung[i][0] == teilenummer[26])
                     {
                         p20 -= bearbeitung[i][1];
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

                 #region In textBox
                 textBox1.Text = p4.ToString();
                 textBox2.Text = p5.ToString();
                 textBox3.Text = p6.ToString();
                 textBox4.Text = p7.ToString();
                 textBox5.Text = p8.ToString();
                 textBox6.Text = p9.ToString();
                 textBox7.Text = p10.ToString();
                 textBox8.Text = p11.ToString();
                 textBox9.Text = p12.ToString();
                 textBox10.Text = p13.ToString();
                 textBox11.Text = p14.ToString();
                 textBox12.Text = p15.ToString();
                 textBox13.Text = p16.ToString();
                 textBox14.Text = p17.ToString();
                 textBox15.Text = p18.ToString();
                 textBox16.Text = p19.ToString();
                 textBox17.Text = p20.ToString();
                 textBox18.Text = p26.ToString();
                 textBox19.Text = p29.ToString();
                 textBox20.Text = p30.ToString();
                 textBox21.Text = p31.ToString();
                 textBox22.Text = p49.ToString();
                 textBox23.Text = p50.ToString();
                 textBox24.Text = p51.ToString();
                 textBox25.Text = p54.ToString();
                 textBox26.Text = p54.ToString();
                 textBox27.Text = p56.ToString();
             }
            #endregion
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #region Textboxen
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
                    textBox1.ForeColor = Color.Black; ;
                }
            }

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

        }

        private void textBox13_TextChanged(object sender, EventArgs e)
        {
            if (textBox13.Text == "")
            {
                textBox13.ForeColor = Color.Red;
            }
            else
            {
                textBox13.ForeColor = Color.Black;
                bool okay = true;

                foreach (char c in textBox13.Text.ToCharArray())
                {

                    if (!digits.Contains<char>(c))
                    {
                        textBox13.ForeColor = Color.Red;
                        okay = false;
                        break;
                    }
                }
                if (okay == true)
                {
                    textBox13.ForeColor = Color.Black;
                }
            }
        }

        private void textBox14_TextChanged(object sender, EventArgs e)
        {
            if (textBox14.Text == "")
            {
                textBox14.ForeColor = Color.Red;
            }
            else
            {
                textBox14.ForeColor = Color.Black;
                bool okay = true;

                foreach (char c in textBox14.Text.ToCharArray())
                {

                    if (!digits.Contains<char>(c))
                    {
                        textBox14.ForeColor = Color.Red;
                        okay = false;
                        break;
                    }
                }
                if (okay == true)
                {
                    textBox14.ForeColor = Color.Black;
                }
            }
        }

        private void textBox15_TextChanged(object sender, EventArgs e)
        {
            if (textBox15.Text == "")
            {
                textBox15.ForeColor = Color.Red;
            }
            else
            {
                textBox15.ForeColor = Color.Black;
                bool okay = true;

                foreach (char c in textBox15.Text.ToCharArray())
                {

                    if (!digits.Contains<char>(c))
                    {
                        textBox15.ForeColor = Color.Red;
                        okay = false;
                        break;
                    }
                }
                if (okay == true)
                {
                    textBox15.ForeColor = Color.Black;
                }
            }
        }

        private void textBox16_TextChanged(object sender, EventArgs e)
        {
            if (textBox16.Text == "")
            {
                textBox16.ForeColor = Color.Red;
            }
            else
            {
                textBox16.ForeColor = Color.Black;
                bool okay = true;

                foreach (char c in textBox16.Text.ToCharArray())
                {

                    if (!digits.Contains<char>(c))
                    {
                        textBox16.ForeColor = Color.Red;
                        okay = false;
                        break;
                    }
                }
                if (okay == true)
                {
                    textBox16.ForeColor = Color.Black;
                }
            }
        }

        private void textBox17_TextChanged(object sender, EventArgs e)
        {
            if (textBox17.Text == "")
            {
                textBox17.ForeColor = Color.Red;
            }
            else
            {
                textBox17.ForeColor = Color.Black;
                bool okay = true;

                foreach (char c in textBox17.Text.ToCharArray())
                {

                    if (!digits.Contains<char>(c))
                    {
                        textBox17.ForeColor = Color.Red;
                        okay = false;
                        break;
                    }
                }
                if (okay == true)
                {
                    textBox17.ForeColor = Color.Black;
                }
            }
        }

        private void textBox18_TextChanged(object sender, EventArgs e)
        {
            if (textBox18.Text == "")
            {
                textBox18.ForeColor = Color.Red;
            }
            else
            {
                textBox18.ForeColor = Color.Black;
                bool okay = true;

                foreach (char c in textBox18.Text.ToCharArray())
                {

                    if (!digits.Contains<char>(c))
                    {
                        textBox18.ForeColor = Color.Red;
                        okay = false;
                        break;
                    }
                }
                if (okay == true)
                {
                    textBox18.ForeColor = Color.Black;
                }
            }
        }

        private void textBox19_TextChanged(object sender, EventArgs e)
        {
            if (textBox19.Text == "")
            {
                textBox19.ForeColor = Color.Red;
            }
            else
            {
                textBox19.ForeColor = Color.Black;
                bool okay = true;

                foreach (char c in textBox19.Text.ToCharArray())
                {

                    if (!digits.Contains<char>(c))
                    {
                        textBox19.ForeColor = Color.Red;
                        okay = false;
                        break;
                    }
                }
                if (okay == true)
                {
                    textBox19.ForeColor = Color.Black;
                }
            }
        }

        private void textBox20_TextChanged(object sender, EventArgs e)
        {
            if (textBox20.Text == "")
            {
                textBox20.ForeColor = Color.Red;
            }
            else
            {
                textBox20.ForeColor = Color.Black;
                bool okay = true;

                foreach (char c in textBox20.Text.ToCharArray())
                {

                    if (!digits.Contains<char>(c))
                    {
                        textBox20.ForeColor = Color.Red;
                        okay = false;
                        break;
                    }
                }
                if (okay == true)
                {
                    textBox20.ForeColor = Color.Black;
                }
            }
        }

        private void textBox21_TextChanged(object sender, EventArgs e)
        {
            if (textBox21.Text == "")
            {
                textBox21.ForeColor = Color.Red;
            }
            else
            {
                textBox21.ForeColor = Color.Black;
                bool okay = true;

                foreach (char c in textBox21.Text.ToCharArray())
                {

                    if (!digits.Contains<char>(c))
                    {
                        textBox21.ForeColor = Color.Red;
                        okay = false;
                        break;
                    }
                }
                if (okay == true)
                {
                    textBox21.ForeColor = Color.Black;
                }
            }
        }

        private void textBox22_TextChanged(object sender, EventArgs e)
        {
            if (textBox22.Text == "")
            {
                textBox22.ForeColor = Color.Red;
            }
            else
            {
                textBox22.ForeColor = Color.Black;
                bool okay = true;

                foreach (char c in textBox22.Text.ToCharArray())
                {

                    if (!digits.Contains<char>(c))
                    {
                        textBox22.ForeColor = Color.Red;
                        okay = false;
                        break;
                    }
                }
                if (okay == true)
                {
                    textBox22.ForeColor = Color.Black;
                }
            }
        }

        private void textBox23_TextChanged(object sender, EventArgs e)
        {
            if (textBox23.Text == "")
            {
                textBox23.ForeColor = Color.Red;
            }
            else
            {
                textBox23.ForeColor = Color.Black;
                bool okay = true;

                foreach (char c in textBox23.Text.ToCharArray())
                {

                    if (!digits.Contains<char>(c))
                    {
                        textBox23.ForeColor = Color.Red;
                        okay = false;
                        break;
                    }
                }
                if (okay == true)
                {
                    textBox23.ForeColor = Color.Black;
                }
            }
        }

        private void textBox24_TextChanged(object sender, EventArgs e)
        {
            if (textBox24.Text == "")
            {
                textBox24.ForeColor = Color.Red;
            }
            else
            {
                textBox24.ForeColor = Color.Black;
                bool okay = true;

                foreach (char c in textBox24.Text.ToCharArray())
                {

                    if (!digits.Contains<char>(c))
                    {
                        textBox24.ForeColor = Color.Red;
                        okay = false;
                        break;
                    }
                }
                if (okay == true)
                {
                    textBox24.ForeColor = Color.Black;
                }
            }
        }

        private void textBox25_TextChanged(object sender, EventArgs e)
        {
            if (textBox25.Text == "")
            {
                textBox25.ForeColor = Color.Red;
            }
            else
            {
                textBox25.ForeColor = Color.Black;
                bool okay = true;

                foreach (char c in textBox25.Text.ToCharArray())
                {

                    if (!digits.Contains<char>(c))
                    {
                        textBox25.ForeColor = Color.Red;
                        okay = false;
                        break;
                    }
                }
                if (okay == true)
                {
                    textBox25.ForeColor = Color.Black;
                }
            }
        }

        private void textBox26_TextChanged(object sender, EventArgs e)
        {
            if (textBox26.Text == "")
            {
                textBox26.ForeColor = Color.Red;
            }
            else
            {
                textBox26.ForeColor = Color.Black;
                bool okay = true;

                foreach (char c in textBox26.Text.ToCharArray())
                {

                    if (!digits.Contains<char>(c))
                    {
                        textBox26.ForeColor = Color.Red;
                        okay = false;
                        break;
                    }
                }
                if (okay == true)
                {
                    textBox26.ForeColor = Color.Black;
                }
            }
        }

        private void textBox27_TextChanged(object sender, EventArgs e)
        {
            if (textBox27.Text == "")
            {
                textBox27.ForeColor = Color.Red;
            }
            else
            {
                textBox27.ForeColor = Color.Black;
                bool okay = true;

                foreach (char c in textBox27.Text.ToCharArray())
                {

                    if (!digits.Contains<char>(c))
                    {
                        textBox27.ForeColor = Color.Red;
                        okay = false;
                        break;
                    }
                }
                if (okay == true)
                {
                    textBox27.ForeColor = Color.Black;
                }
            }
        }
        #endregion

        private void button2_Click(object sender, EventArgs e)
        {
            berechneProduktion();
        }

        public void sprachen()
        {
            if (pic_en.SizeMode == PictureBoxSizeMode.StretchImage)
            {
                button2.Text = Sprachen.EN_BTN_DEFAULT;
                groupBox1.Text = Sprachen.EN_PRE_GB_ETEILE;
            }
            else
            {
                button2.Text = Sprachen.DE_BTN_DEFAULT;
                groupBox1.Text = Sprachen.DE_PRE_GB_ETEILE;

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

    }
}
