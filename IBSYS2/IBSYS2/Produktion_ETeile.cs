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
        List<int> lagerbestand = new List<int>();
        List<int> bearbeitung = new List<int>();
        List<int> wartelisteAr = new List<int>();
        List<int> wartelisteMa = new List<int>();

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

        private int Daten(string teilenummer_FK, string spalte, string spalte1, string tabelle, int periode)
        {
            OleDbCommand cmd = new OleDbCommand();
            cmd.CommandType = CommandType.Text;
            cmd.Connection = myconn;
            try
            {
                myconn.Open();
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("DB-Verbindung wurde nicht ordnugnsgemäß geschlossen bei der letzten Verwendung, Verbindung wird neu gestartet, bitte haben Sie einen Moment Geduld.");
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

            lagerbestand.Add(Daten("26", "Bestand", "Teilenummer_FK", "Lager", periode));
            lagerbestand.Add(Daten("51", "Bestand", "Teilenummer_FK", "Lager", periode));
            lagerbestand.Add(Daten("16", "Bestand", "Teilenummer_FK", "Lager", periode));
            lagerbestand.Add(Daten("17", "Bestand", "Teilenummer_FK", "Lager", periode));
            lagerbestand.Add(Daten("50", "Bestand", "Teilenummer_FK", "Lager", periode));
            lagerbestand.Add(Daten("4", "Bestand", "Teilenummer_FK", "Lager", periode));
            lagerbestand.Add(Daten("10", "Bestand", "Teilenummer_FK", "Lager", periode));
            lagerbestand.Add(Daten("49", "Bestand", "Teilenummer_FK", "Lager", periode));
            lagerbestand.Add(Daten("7", "Bestand", "Teilenummer_FK", "Lager", periode));
            lagerbestand.Add(Daten("13", "Bestand", "Teilenummer_FK", "Lager", periode));
            lagerbestand.Add(Daten("18", "Bestand", "Teilenummer_FK", "Lager", periode));
            lagerbestand.Add(Daten("56", "Bestand", "Teilenummer_FK", "Lager", periode));
            lagerbestand.Add(Daten("55", "Bestand", "Teilenummer_FK", "Lager", periode));
            lagerbestand.Add(Daten("5", "Bestand", "Teilenummer_FK", "Lager", periode));
            lagerbestand.Add(Daten("11", "Bestand", "Teilenummer_FK", "Lager", periode));
            lagerbestand.Add(Daten("54", "Bestand", "Teilenummer_FK", "Lager", periode));
            lagerbestand.Add(Daten("8", "Bestand", "Teilenummer_FK", "Lager", periode));
            lagerbestand.Add(Daten("14", "Bestand", "Teilenummer_FK", "Lager", periode));
            lagerbestand.Add(Daten("19", "Bestand", "Teilenummer_FK", "Lager", periode));
            lagerbestand.Add(Daten("31", "Bestand", "Teilenummer_FK", "Lager", periode));
            lagerbestand.Add(Daten("30", "Bestand", "Teilenummer_FK", "Lager", periode));
            lagerbestand.Add(Daten("6", "Bestand", "Teilenummer_FK", "Lager", periode));
            lagerbestand.Add(Daten("12", "Bestand", "Teilenummer_FK", "Lager", periode));
            lagerbestand.Add(Daten("29", "Bestand", "Teilenummer_FK", "Lager", periode));
            lagerbestand.Add(Daten("9", "Bestand", "Teilenummer_FK", "Lager", periode));
            lagerbestand.Add(Daten("15", "Bestand", "Teilenummer_FK", "Lager", periode));
            lagerbestand.Add(Daten("20", "Bestand", "Teilenummer_FK", "Lager", periode));

            bearbeitung.Add(Daten("26", "Menge", "Teilenummer_FK", "Bearbeitung", periode));
            bearbeitung.Add(Daten("51", "Menge", "Teilenummer_FK", "Bearbeitung", periode));
            bearbeitung.Add(Daten("16", "Menge", "Teilenummer_FK", "Bearbeitung", periode));
            bearbeitung.Add(Daten("17", "Menge", "Teilenummer_FK", "Bearbeitung", periode));
            bearbeitung.Add(Daten("50", "Menge", "Teilenummer_FK", "Bearbeitung", periode));
            bearbeitung.Add(Daten("4", "Menge", "Teilenummer_FK", "Bearbeitung", periode));
            bearbeitung.Add(Daten("10", "Menge", "Teilenummer_FK", "Bearbeitung", periode));
            bearbeitung.Add(Daten("49", "Menge", "Teilenummer_FK", "Bearbeitung", periode));
            bearbeitung.Add(Daten("7", "Menge", "Teilenummer_FK", "Bearbeitung", periode));
            bearbeitung.Add(Daten("13", "Menge", "Teilenummer_FK", "Bearbeitung", periode));
            bearbeitung.Add(Daten("18", "Menge", "Teilenummer_FK", "Bearbeitung", periode));
            bearbeitung.Add(Daten("56", "Menge", "Teilenummer_FK", "Bearbeitung", periode));
            bearbeitung.Add(Daten("55", "Menge", "Teilenummer_FK", "Bearbeitung", periode));
            bearbeitung.Add(Daten("5", "Menge", "Teilenummer_FK", "Bearbeitung", periode));
            bearbeitung.Add(Daten("11", "Menge", "Teilenummer_FK", "Bearbeitung", periode));
            bearbeitung.Add(Daten("54", "Menge", "Teilenummer_FK", "Bearbeitung", periode));
            bearbeitung.Add(Daten("8", "Menge", "Teilenummer_FK", "Bearbeitung", periode));
            bearbeitung.Add(Daten("14", "Menge", "Teilenummer_FK", "Bearbeitung", periode));
            bearbeitung.Add(Daten("19", "Menge", "Teilenummer_FK", "Bearbeitung", periode));
            bearbeitung.Add(Daten("31", "Menge", "Teilenummer_FK", "Bearbeitung", periode));
            bearbeitung.Add(Daten("30", "Menge", "Teilenummer_FK", "Bearbeitung", periode));
            bearbeitung.Add(Daten("6", "Menge", "Teilenummer_FK", "Bearbeitung", periode));
            bearbeitung.Add(Daten("12", "Menge", "Teilenummer_FK", "Bearbeitung", periode));
            bearbeitung.Add(Daten("29", "Menge", "Teilenummer_FK", "Bearbeitung", periode));
            bearbeitung.Add(Daten("9", "Menge", "Teilenummer_FK", "Bearbeitung", periode));
            bearbeitung.Add(Daten("15", "Menge", "Teilenummer_FK", "Bearbeitung", periode));
            bearbeitung.Add(Daten("20", "Menge", "Teilenummer_FK", "Bearbeitung", periode));

            wartelisteAr.Add(Daten("26", "Menge", "Teilenummer_FK", "Warteliste_Arbeitsplatz", periode));
            wartelisteAr.Add(Daten("51", "Menge", "Teilenummer_FK", "Warteliste_Arbeitsplatz", periode));
            wartelisteAr.Add(Daten("16", "Menge", "Teilenummer_FK", "Warteliste_Arbeitsplatz", periode));
            wartelisteAr.Add(Daten("17", "Menge", "Teilenummer_FK", "Warteliste_Arbeitsplatz", periode));
            wartelisteAr.Add(Daten("50", "Menge", "Teilenummer_FK", "Warteliste_Arbeitsplatz", periode));
            wartelisteAr.Add(Daten("4", "Menge", "Teilenummer_FK", "Warteliste_Arbeitsplatz", periode));
            wartelisteAr.Add(Daten("10", "Menge", "Teilenummer_FK", "Warteliste_Arbeitsplatz", periode));
            wartelisteAr.Add(Daten("49", "Menge", "Teilenummer_FK", "Warteliste_Arbeitsplatz", periode));
            wartelisteAr.Add(Daten("7", "Menge", "Teilenummer_FK", "Warteliste_Arbeitsplatz", periode));
            wartelisteAr.Add(Daten("13", "Menge", "Teilenummer_FK", "Warteliste_Arbeitsplatz", periode));
            wartelisteAr.Add(Daten("18", "Menge", "Teilenummer_FK", "Warteliste_Arbeitsplatz", periode));
            wartelisteAr.Add(Daten("56", "Menge", "Teilenummer_FK", "Warteliste_Arbeitsplatz", periode));
            wartelisteAr.Add(Daten("55", "Menge", "Teilenummer_FK", "Warteliste_Arbeitsplatz", periode));
            wartelisteAr.Add(Daten("5", "Menge", "Teilenummer_FK", "Warteliste_Arbeitsplatz", periode));
            wartelisteAr.Add(Daten("11", "Menge", "Teilenummer_FK", "Warteliste_Arbeitsplatz", periode));
            wartelisteAr.Add(Daten("54", "Menge", "Teilenummer_FK", "Warteliste_Arbeitsplatz", periode));
            wartelisteAr.Add(Daten("8", "Menge", "Teilenummer_FK", "Warteliste_Arbeitsplatz", periode));
            wartelisteAr.Add(Daten("14", "Menge", "Teilenummer_FK", "Warteliste_Arbeitsplatz", periode));
            wartelisteAr.Add(Daten("19", "Menge", "Teilenummer_FK", "Warteliste_Arbeitsplatz", periode));
            wartelisteAr.Add(Daten("31", "Menge", "Teilenummer_FK", "Warteliste_Arbeitsplatz", periode));
            wartelisteAr.Add(Daten("30", "Menge", "Teilenummer_FK", "Warteliste_Arbeitsplatz", periode));
            wartelisteAr.Add(Daten("6", "Menge", "Teilenummer_FK", "Warteliste_Arbeitsplatz", periode));
            wartelisteAr.Add(Daten("12", "Menge", "Teilenummer_FK", "Warteliste_Arbeitsplatz", periode));
            wartelisteAr.Add(Daten("29", "Menge", "Teilenummer_FK", "Warteliste_Arbeitsplatz", periode));
            wartelisteAr.Add(Daten("9", "Menge", "Teilenummer_FK", "Warteliste_Arbeitsplatz", periode));
            wartelisteAr.Add(Daten("15", "Menge", "Teilenummer_FK", "Warteliste_Arbeitsplatz", periode));
            wartelisteAr.Add(Daten("20", "Menge", "Teilenummer_FK", "Warteliste_Arbeitsplatz", periode));

            wartelisteMa.Add(Daten("26", "Menge", "Fehlteil_Teilenummer_FK", "Warteliste_Material", periode));
            wartelisteMa.Add(Daten("51", "Menge", "Fehlteil_Teilenummer_FK", "Warteliste_Material", periode));
            wartelisteMa.Add(Daten("16", "Menge", "Fehlteil_Teilenummer_FK", "Warteliste_Material", periode));
            wartelisteMa.Add(Daten("17", "Menge", "Fehlteil_Teilenummer_FK", "Warteliste_Material", periode));
            wartelisteMa.Add(Daten("50", "Menge", "Fehlteil_Teilenummer_FK", "Warteliste_Material", periode));
            wartelisteMa.Add(Daten("4", "Menge", "Fehlteil_Teilenummer_FK", "Warteliste_Material", periode));
            wartelisteMa.Add(Daten("10", "Menge", "Fehlteil_Teilenummer_FK", "Warteliste_Material", periode));
            wartelisteMa.Add(Daten("49", "Menge", "Fehlteil_Teilenummer_FK", "Warteliste_Material", periode));
            wartelisteMa.Add(Daten("7", "Menge", "Fehlteil_Teilenummer_FK", "Warteliste_Material", periode));
            wartelisteMa.Add(Daten("13", "Menge", "Fehlteil_Teilenummer_FK", "Warteliste_Material", periode));
            wartelisteMa.Add(Daten("18", "Menge", "Fehlteil_Teilenummer_FK", "Warteliste_Material", periode));
            wartelisteMa.Add(Daten("56", "Menge", "Fehlteil_Teilenummer_FK", "Warteliste_Material", periode));
            wartelisteMa.Add(Daten("55", "Menge", "Fehlteil_Teilenummer_FK", "Warteliste_Material", periode));
            wartelisteMa.Add(Daten("5", "Menge", "Fehlteil_Teilenummer_FK", "Warteliste_Material", periode));
            wartelisteMa.Add(Daten("11", "Menge", "Fehlteil_Teilenummer_FK", "Warteliste_Material", periode));
            wartelisteMa.Add(Daten("54", "Menge", "Fehlteil_Teilenummer_FK", "Warteliste_Material", periode));
            wartelisteMa.Add(Daten("8", "Menge", "Fehlteil_Teilenummer_FK", "Warteliste_Material", periode));
            wartelisteMa.Add(Daten("14", "Menge", "Fehlteil_Teilenummer_FK", "Warteliste_Material", periode));
            wartelisteMa.Add(Daten("19", "Menge", "Fehlteil_Teilenummer_FK", "Warteliste_Material", periode));
            wartelisteMa.Add(Daten("31", "Menge", "Fehlteil_Teilenummer_FK", "Warteliste_Material", periode));
            wartelisteMa.Add(Daten("30", "Menge", "Fehlteil_Teilenummer_FK", "Warteliste_Material", periode));
            wartelisteMa.Add(Daten("6", "Menge", "Fehlteil_Teilenummer_FK", "Warteliste_Material", periode));
            wartelisteMa.Add(Daten("12", "Menge", "Fehlteil_Teilenummer_FK", "Warteliste_Material", periode));
            wartelisteMa.Add(Daten("29", "Menge", "Fehlteil_Teilenummer_FK", "Warteliste_Material", periode));
            wartelisteMa.Add(Daten("9", "Menge", "Fehlteil_Teilenummer_FK", "Warteliste_Material", periode));
            wartelisteMa.Add(Daten("15", "Menge", "Fehlteil_Teilenummer_FK", "Warteliste_Material", periode));
            wartelisteMa.Add(Daten("20", "Menge", "Fehlteil_Teilenummer_FK", "Warteliste_Material", periode));

            //Berechnung P1
            p26 = produktionp1 + sicherheitsbe[0] + sicherheitsbe[11] + sicherheitsbe[22] - lagerbestand[0] - wartelisteAr[0] - wartelisteMa[0] - bearbeitung[0];
            p51 = produktionp1 + sicherheitsbe[1] - lagerbestand[1] - wartelisteAr[1] - wartelisteMa[1] - bearbeitung[1];

            p16 = p51 + sicherheitsbe[2] + sicherheitsbe[13] + sicherheitsbe[24] - lagerbestand[2] - wartelisteAr[2] - wartelisteMa[2] - bearbeitung[2];
            p17 = p51 + sicherheitsbe[3] + sicherheitsbe[14] + sicherheitsbe[25] - lagerbestand[3] - wartelisteAr[3] - wartelisteMa[3] - bearbeitung[3];
            p50 = p51 + sicherheitsbe[4] - lagerbestand[4] - wartelisteAr[4] - wartelisteMa[4] - bearbeitung[4];

            p4 = p50 + sicherheitsbe[5] - lagerbestand[5] - wartelisteAr[5] - wartelisteMa[5] - bearbeitung[5];
            p10 = p50 + sicherheitsbe[6] - lagerbestand[6] - wartelisteAr[6] - wartelisteMa[6] - bearbeitung[6];
            p49 = p50 + sicherheitsbe[7] - lagerbestand[7] - wartelisteAr[7] - wartelisteMa[7] - bearbeitung[7];

            p7 = p49 + sicherheitsbe[8] - lagerbestand[8] - wartelisteAr[8] - wartelisteMa[8] - bearbeitung[8];
            p13 = p49 + sicherheitsbe[9] - lagerbestand[9] - wartelisteAr[9] - wartelisteMa[9] - bearbeitung[9];
            p18 = p49 + sicherheitsbe[10] - lagerbestand[10] - wartelisteAr[10] - wartelisteMa[10] - bearbeitung[10];


            //Berechnung P2
            p56 = produktionp2 + sicherheitsbe[12] - lagerbestand[11] - wartelisteAr[11] - wartelisteMa[11] - bearbeitung[11];

            p55 = p56 + sicherheitsbe[15] - lagerbestand[12] - wartelisteAr[12] - wartelisteMa[12] - bearbeitung[12];

            p5 = p55 + sicherheitsbe[16] - lagerbestand[13] - wartelisteAr[13] - wartelisteMa[13] - bearbeitung[13];
            p11 = p55 + sicherheitsbe[17] - lagerbestand[14] - wartelisteAr[14] - wartelisteMa[14] - bearbeitung[14];
            p54 = p55 + sicherheitsbe[18] - lagerbestand[15] - wartelisteAr[15] - wartelisteMa[15] - bearbeitung[15];

            p8 = p54 + sicherheitsbe[19] - lagerbestand[16] - wartelisteAr[16] - wartelisteMa[16] - bearbeitung[16];
            p14 = p54 + sicherheitsbe[20] - lagerbestand[17] - wartelisteAr[17] - wartelisteMa[17] - bearbeitung[17];
            p19 = p54 + sicherheitsbe[21] - lagerbestand[18] - wartelisteAr[18] - wartelisteMa[18] - bearbeitung[18];


            //Berechnung P3
            p31 = produktionp3 + sicherheitsbe[23] - lagerbestand[19] - wartelisteAr[19] - wartelisteMa[19] - bearbeitung[19];

            p30 = p31 + sicherheitsbe[26] - lagerbestand[20] - wartelisteAr[20] - wartelisteMa[20] - bearbeitung[20];

            p6 = p30 + sicherheitsbe[27] - lagerbestand[21] - wartelisteAr[21] - wartelisteMa[21] - bearbeitung[21];
            p12 = p30 + sicherheitsbe[28] - lagerbestand[22] - wartelisteAr[22] - wartelisteMa[22] - bearbeitung[22];
            p29 = p30 + sicherheitsbe[29] - lagerbestand[23] - wartelisteAr[23] - wartelisteMa[23] - bearbeitung[23];

            p9 = p29 + sicherheitsbe[30] - lagerbestand[24] - wartelisteAr[24] - wartelisteMa[24] - bearbeitung[24];
            p15 = p29 + sicherheitsbe[31] - lagerbestand[25] - wartelisteAr[25] - wartelisteMa[25] - bearbeitung[25];
            p20 = p29 + sicherheitsbe[32] - lagerbestand[26] - wartelisteAr[26] - wartelisteMa[26] - bearbeitung[26];

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

            #endregion
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
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
    }
}
