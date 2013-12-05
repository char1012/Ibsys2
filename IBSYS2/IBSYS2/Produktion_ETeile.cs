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
    public partial class Produktion_ETeile : UserControl
    {
        private OleDbConnection myconn;
        private char[] digits = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
        
        //TO DO periode aus Import
        int periode = 6;
        public Produktion_ETeile()
        {
            InitializeComponent();
            string databasename = @"Provider=Microsoft.ACE.OLEDB.12.0; Data Source=IBSYS_DB.accdb";
            myconn = new OleDbConnection(databasename);
            berechneProduktion();
        }

        private void berechneProduktion()
        {
            #region P1 berechnen
		
            #region Daten

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

            //Produktion
            //TO DO int produktionp1 aus Produktion
            int produktionp1 = 150;

            //Lagerbestand 
            int bestande26 = Daten("26", "Bestand", "Teilenummer_FK", "Lager", periode);
            int bestande51 = Daten("51", "Bestand", "Teilenummer_FK", "Lager", periode);
            int bestande16 = Daten("16", "Bestand", "Teilenummer_FK", "Lager", periode);
            int bestande17 = Daten("17", "Bestand", "Teilenummer_FK", "Lager", periode);
            int bestande50 = Daten("50", "Bestand", "Teilenummer_FK", "Lager", periode);
            int bestande4 = Daten("4", "Bestand", "Teilenummer_FK", "Lager", periode);
            int bestande10 = Daten("10", "Bestand", "Teilenummer_FK", "Lager", periode);
            int bestande49 = Daten("49", "Bestand", "Teilenummer_FK", "Lager", periode);
            int bestande7 = Daten("7", "Bestand", "Teilenummer_FK", "Lager", periode);
            int bestande13 = Daten("13", "Bestand", "Teilenummer_FK", "Lager", periode);
            int bestande18 = Daten("18", "Bestand", "Teilenummer_FK", "Lager", periode);

            //Sicherheitsbestand
            // TO DO aus Sicherheitsbestand
            int sb26 = 20;
            int sb51 = 10;
            int sb16 = 30;
            int sb17 = 15;
            int sb50 = 15;
            int sb4 = 10;
            int sb10 = 25;
            int sb49 = 19;
            int sb7 = 25;
            int sb13 = 16;
            int sb18 = 20;

            //Warteliste
            int wa26 = Daten("26", "Menge", "Teilenummer_FK", "Warteliste_Arbeitsplatz", periode);
            int wa51 = Daten("51", "Menge", "Teilenummer_FK", "Warteliste_Arbeitsplatz", periode);
            int wa16 = Daten("16", "Menge", "Teilenummer_FK", "Warteliste_Arbeitsplatz", periode);
            int wa17 = Daten("17", "Menge", "Teilenummer_FK", "Warteliste_Arbeitsplatz", periode);
            int wa50 = Daten("50", "Menge", "Teilenummer_FK", "Warteliste_Arbeitsplatz", periode);
            int wa4 = Daten("4", "Menge", "Teilenummer_FK", "Warteliste_Arbeitsplatz", periode);
            int wa10 = Daten("10", "Menge", "Teilenummer_FK", "Warteliste_Arbeitsplatz", periode);
            int wa49 = Daten("49", "Menge", "Teilenummer_FK", "Warteliste_Arbeitsplatz", periode);
            int wa7 = Daten("7", "Menge", "Teilenummer_FK", "Warteliste_Arbeitsplatz", periode);
            int wa13 = Daten("13", "Menge", "Teilenummer_FK", "Warteliste_Arbeitsplatz", periode);
            int wa18 = Daten("18", "Menge", "Teilenummer_FK", "Warteliste_Arbeitsplatz", periode);

            int wm26 = Daten("26", "Menge", "Fehlteil_Teilenummer_FK", "Warteliste_Material", periode);
            int wm51 = Daten("51", "Menge", "Fehlteil_Teilenummer_FK", "Warteliste_Material", periode);
            int wm16 = Daten("16", "Menge", "Fehlteil_Teilenummer_FK", "Warteliste_Material", periode);
            int wm17 = Daten("17", "Menge", "Fehlteil_Teilenummer_FK", "Warteliste_Material", periode);
            int wm50 = Daten("50", "Menge", "Fehlteil_Teilenummer_FK", "Warteliste_Material", periode);
            int wm4 = Daten("4", "Menge", "Fehlteil_Teilenummer_FK", "Warteliste_Material", periode);
            int wm10 = Daten("10", "Menge", "Fehlteil_Teilenummer_FK", "Warteliste_Material", periode);
            int wm49 = Daten("49", "Menge", "Fehlteil_Teilenummer_FK", "Warteliste_Material", periode);
            int wm7 = Daten("7", "Menge", "Fehlteil_Teilenummer_FK", "Warteliste_Material", periode);
            int wm13 = Daten("13", "Menge", "Fehlteil_Teilenummer_FK", "Warteliste_Material", periode);
            int wm18 = Daten("18", "Menge", "Fehlteil_Teilenummer_FK", "Warteliste_Material", periode);

            //Bearbeitung
            int b26 = Daten("26", "Menge", "Teilenummer_FK", "Bearbeitung", periode);
            int b51 = Daten("51", "Menge", "Teilenummer_FK", "Bearbeitung", periode);
            int b16 = Daten("16", "Menge", "Teilenummer_FK", "Bearbeitung", periode);
            int b17 = Daten("17", "Menge", "Teilenummer_FK", "Bearbeitung", periode);
            int b50 = Daten("50", "Menge", "Teilenummer_FK", "Bearbeitung", periode);
            int b4 = Daten("4", "Menge", "Teilenummer_FK", "Bearbeitung", periode);
            int b10 = Daten("10", "Menge", "Teilenummer_FK", "Bearbeitung", periode);
            int b49 = Daten("49", "Menge", "Teilenummer_FK", "Bearbeitung", periode);
            int b7 = Daten("7", "Menge", "Teilenummer_FK", "Bearbeitung", periode);
            int b13 = Daten("13", "Menge", "Teilenummer_FK", "Bearbeitung", periode);
            int b18 = Daten("18", "Menge", "Teilenummer_FK", "Bearbeitung", periode);

            #endregion
            
            //Eigentliche Berechnung
            p26 = produktionp1 + sb26 - bestande26 - wa26 - wm26 - b26;
            p51 = produktionp1 + sb51 - bestande51 - wa51 - wm51 - b51;

            p16 = p51 + sb16 - bestande16 - wa16 - wm16 - b16;
            p17 = p51 + sb17 - bestande17 - wa17 - wm17 - b17;
            p50 = p51 + sb50 - bestande50 - wa50 - wm50 - b50;

            p4 = p50 + sb4 - bestande4 - wa4 - wm4 - b4;
            p10 = p50 + sb10 - bestande10 - wa10 - wm10 - b10;
            p49 = p50 + sb49 - bestande49 - wa49 - wm49 - b49;

            p7 = p49 + sb7 - bestande7 - wa7 - wm7 - b7;
            p13 = p49 + sb13 - bestande13 - wa13 - wm13 - b13;
            p18 = p49 + sb18 - bestande18 - wa18 - wm18 - b18; 
	#endregion

            #region P2 berechnen
            #region Daten

            int p26_2;
            int p56;
            int p16_2;
            int p17_2;
            int p55;
            int p5;
            int p11;
            int p54;
            int p8;
            int p14;
            int p19;

            //Produktion
            //TO DO int produktionp1 aus Produktion
            int produktionp2 = 100;

            //Lagerbestand 
            int bestande56 = Daten("56", "Bestand", "Teilenummer_FK", "Lager", periode);
            int bestande55 = Daten("55", "Bestand", "Teilenummer_FK", "Lager", periode);
            int bestande5 = Daten("5", "Bestand", "Teilenummer_FK", "Lager", periode);
            int bestande11 = Daten("11", "Bestand", "Teilenummer_FK", "Lager", periode);
            int bestande54 = Daten("54", "Bestand", "Teilenummer_FK", "Lager", periode);
            int bestande8 = Daten("8", "Bestand", "Teilenummer_FK", "Lager", periode);
            int bestande14 = Daten("14", "Bestand", "Teilenummer_FK", "Lager", periode);
            int bestande19 = Daten("19", "Bestand", "Teilenummer_FK", "Lager", periode);

            //Sicherheitsbestand
            // TO DO aus Sicherheitsbestand
            int sb26_2 = 20;
            int sb56 = 10;
            int sb16_2 = 30;
            int sb17_2 = 15;
            int sb55 = 15;
            int sb5 = 10;
            int sb11 = 25;
            int sb54 = 19;
            int sb8 = 25;
            int sb14 = 16;
            int sb19 = 20;

            //Warteliste
            int wa56 = Daten("56", "Menge", "Teilenummer_FK", "Warteliste_Arbeitsplatz", periode);
            int wa55 = Daten("55", "Menge", "Teilenummer_FK", "Warteliste_Arbeitsplatz", periode);
            int wa5 = Daten("5", "Menge", "Teilenummer_FK", "Warteliste_Arbeitsplatz", periode);
            int wa11 = Daten("11", "Menge", "Teilenummer_FK", "Warteliste_Arbeitsplatz", periode);
            int wa54 = Daten("54", "Menge", "Teilenummer_FK", "Warteliste_Arbeitsplatz", periode);
            int wa8 = Daten("8", "Menge", "Teilenummer_FK", "Warteliste_Arbeitsplatz", periode);
            int wa14 = Daten("14", "Menge", "Teilenummer_FK", "Warteliste_Arbeitsplatz", periode);
            int wa19 = Daten("19", "Menge", "Teilenummer_FK", "Warteliste_Arbeitsplatz", periode);

            int wm56 = Daten("56", "Menge", "Fehlteil_Teilenummer_FK", "Warteliste_Material", periode);
            int wm55 = Daten("55", "Menge", "Fehlteil_Teilenummer_FK", "Warteliste_Material", periode);
            int wm5 = Daten("5", "Menge", "Fehlteil_Teilenummer_FK", "Warteliste_Material", periode);
            int wm11 = Daten("11", "Menge", "Fehlteil_Teilenummer_FK", "Warteliste_Material", periode);
            int wm54 = Daten("54", "Menge", "Fehlteil_Teilenummer_FK", "Warteliste_Material", periode);
            int wm8 = Daten("8", "Menge", "Fehlteil_Teilenummer_FK", "Warteliste_Material", periode);
            int wm14 = Daten("14", "Menge", "Fehlteil_Teilenummer_FK", "Warteliste_Material", periode);
            int wm19 = Daten("19", "Menge", "Fehlteil_Teilenummer_FK", "Warteliste_Material", periode);

            //Bearbeitung
            int b56 = Daten("56", "Menge", "Teilenummer_FK", "Bearbeitung", periode);
            int b55 = Daten("55", "Menge", "Teilenummer_FK", "Bearbeitung", periode);
            int b5 = Daten("5", "Menge", "Teilenummer_FK", "Bearbeitung", periode);
            int b11 = Daten("11", "Menge", "Teilenummer_FK", "Bearbeitung", periode);
            int b54 = Daten("54", "Menge", "Teilenummer_FK", "Bearbeitung", periode);
            int b8 = Daten("8", "Menge", "Teilenummer_FK", "Bearbeitung", periode);
            int b14 = Daten("14", "Menge", "Teilenummer_FK", "Bearbeitung", periode);
            int b19 = Daten("19", "Menge", "Teilenummer_FK", "Bearbeitung", periode);

            #endregion

            //Eigentliche Berechnung
            p26_2 = produktionp2 + sb26_2 - bestande26 - wa26 - wm26 - b26;
            p56 = produktionp2 + sb56 - bestande56 - wa56 - wm56 - b56;

            p16_2 = p56 + sb16_2 - bestande16 - wa16 - wm16 - b16;
            p17_2 = p56 + sb17_2 - bestande17 - wa17 - wm17 - b17;
            p55 = p56 + sb55 - bestande55 - wa55 - wm55 - b55;

            p5 = p55 + sb5 - bestande5 - wa5 - wm5 - b5;
            p11 = p55 + sb11 - bestande11 - wa11 - wm11 - b11;
            p54 = p55 + sb54 - bestande54 - wa54 - wm54 - b54;

            p8 = p54 + sb8 - bestande8 - wa8 - wm8 - b8;
            p14 = p54 + sb14 - bestande14 - wa14 - wm14 - b14;
            p19 = p54 + sb19 - bestande19 - wa19 - wm19 - b19; 
            #endregion

            #region P3 berechnen
            #region Daten

            int p26_3;
            int p31;
            int p16_3;
            int p17_3;
            int p30;
            int p6;
            int p12;
            int p29;
            int p9;
            int p15;
            int p20;

            //Produktion
            //TO DO int produktionp1 aus Produktion
            int produktionp3 = 200;

            //Lagerbestand 
            int bestande31 = Daten("31", "Bestand", "Teilenummer_FK", "Lager", periode);
            int bestande30 = Daten("30", "Bestand", "Teilenummer_FK", "Lager", periode);
            int bestande6 = Daten("6", "Bestand", "Teilenummer_FK", "Lager", periode);
            int bestande12 = Daten("12", "Bestand", "Teilenummer_FK", "Lager", periode);
            int bestande29 = Daten("29", "Bestand", "Teilenummer_FK", "Lager", periode);
            int bestande9 = Daten("9", "Bestand", "Teilenummer_FK", "Lager", periode);
            int bestande15 = Daten("15", "Bestand", "Teilenummer_FK", "Lager", periode);
            int bestande20 = Daten("20", "Bestand", "Teilenummer_FK", "Lager", periode);

            //Sicherheitsbestand
            // TO DO aus Sicherheitsbestand
            int sb26_3 = 20;
            int sb31 = 10;
            int sb16_3 = 30;
            int sb17_3 = 15;
            int sb30 = 15;
            int sb6 = 10;
            int sb12 = 25;
            int sb29 = 19;
            int sb9 = 25;
            int sb15 = 16;
            int sb20 = 20;

            //Warteliste
            int wa31 = Daten("31", "Menge", "Teilenummer_FK", "Warteliste_Arbeitsplatz", periode);
            int wa30 = Daten("30", "Menge", "Teilenummer_FK", "Warteliste_Arbeitsplatz", periode);
            int wa6 = Daten("6", "Menge", "Teilenummer_FK", "Warteliste_Arbeitsplatz", periode);
            int wa12 = Daten("12", "Menge", "Teilenummer_FK", "Warteliste_Arbeitsplatz", periode);
            int wa29 = Daten("29", "Menge", "Teilenummer_FK", "Warteliste_Arbeitsplatz", periode);
            int wa9 = Daten("9", "Menge", "Teilenummer_FK", "Warteliste_Arbeitsplatz", periode);
            int wa15 = Daten("15", "Menge", "Teilenummer_FK", "Warteliste_Arbeitsplatz", periode);
            int wa20 = Daten("20", "Menge", "Teilenummer_FK", "Warteliste_Arbeitsplatz", periode);

            int wm31 = Daten("31", "Menge", "Fehlteil_Teilenummer_FK", "Warteliste_Material", periode);
            int wm30 = Daten("30", "Menge", "Fehlteil_Teilenummer_FK", "Warteliste_Material", periode);
            int wm6 = Daten("6", "Menge", "Fehlteil_Teilenummer_FK", "Warteliste_Material", periode);
            int wm12 = Daten("12", "Menge", "Fehlteil_Teilenummer_FK", "Warteliste_Material", periode);
            int wm29 = Daten("29", "Menge", "Fehlteil_Teilenummer_FK", "Warteliste_Material", periode);
            int wm9 = Daten("9", "Menge", "Fehlteil_Teilenummer_FK", "Warteliste_Material", periode);
            int wm15 = Daten("15", "Menge", "Fehlteil_Teilenummer_FK", "Warteliste_Material", periode);
            int wm20 = Daten("20", "Menge", "Fehlteil_Teilenummer_FK", "Warteliste_Material", periode);

            //Bearbeitung
            int b31 = Daten("31", "Menge", "Teilenummer_FK", "Bearbeitung", periode);
            int b30 = Daten("30", "Menge", "Teilenummer_FK", "Bearbeitung", periode);
            int b6 = Daten("6", "Menge", "Teilenummer_FK", "Bearbeitung", periode);
            int b12 = Daten("12", "Menge", "Teilenummer_FK", "Bearbeitung", periode);
            int b29 = Daten("29", "Menge", "Teilenummer_FK", "Bearbeitung", periode);
            int b9 = Daten("9", "Menge", "Teilenummer_FK", "Bearbeitung", periode);
            int b15 = Daten("15", "Menge", "Teilenummer_FK", "Bearbeitung", periode);
            int b20 = Daten("20", "Menge", "Teilenummer_FK", "Bearbeitung", periode);

            #endregion

            //Eigentliche Berechnung
            p26_3 = produktionp3 + sb26_3 - bestande26 - wa26 - wm26 - b26;
            p31 = produktionp3 + sb31 - bestande31 - wa31 - wm31 - b31;

            p16_3 = p31 + sb16_3 - bestande16 - wa16 - wm16 - b16;
            p17_3 = p31 + sb17_3 - bestande17 - wa17 - wm17 - b17;
            p30 = p31 + sb30 - bestande30 - wa30 - wm30 - b30;

            p6 = p30 + sb6 - bestande6 - wa6 - wm6 - b6;
            p12 = p30 + sb12 - bestande12 - wa12 - wm12 - b12;
            p29 = p30 + sb29 - bestande29 - wa29 - wm29 - b29;

            p9 = p29 + sb9 - bestande9 - wa9 - wm9 - b9;
            p15 = p29 + sb15 - bestande15 - wa15 - wm15 - b15;
            p20 = p29 + sb20 - bestande20 - wa20 - wm20 - b20; 
            #endregion

            #region Zusammenrechnen und in textBox
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
