﻿using System;
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
    public partial class Produktion : UserControl
    {
        private OleDbConnection myconn;
        private char[] digits = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
        //TO DO: Periode wird später aus Import geholt
        int periode = 6;

        public Produktion()
        {
           // var UserControl kapa= new Kapazitaetsplan();
            InitializeComponent();
            continue_btn.Enabled = false;

            string databasename = @"Provider=Microsoft.ACE.OLEDB.12.0; Data Source=IBSYS_DB.accdb";
            myconn = new OleDbConnection(databasename);
            berechneProduktion();
        }

        private void check()
        {
            bool weiter = true;
            for (int i = 1; i <= 12; ++i)
            {
                if (this.Controls.Find("textBox" + i.ToString(), true)[0].Text == "" )
                {
                    weiter = false;
                    
                }
                else
                {
                    continue;
                }
            }
            if(weiter == true)
            {
                continue_btn.Enabled = true;
            }
            else
            {
                continue_btn.Enabled = false;
            }
        }

        private void berechneProduktion()
        {
            //für aktuelle Periode
            // TO DOEingabe Aufträge zukünftig aus ImportPrognose
            double p1 = 100;
            double p2 = 100;
            double p3 = 100;

            //+ eingabe Sicherheitsbestand 
            // TO DOZukünftig aus Sicherheitsbestand
            double sp1 = 50;
            double sp2 = 50;
            double sp3 = 50;

            //- Lagerbestand Vorperiode 
            int lagerbestandp1 = Daten("1", "Bestand", "Teilenummer_FK", "Lager", periode);
            int lagerbestandp2 = Daten("2", "Bestand", "Teilenummer_FK", "Lager", periode);
            int lagerbestandp3 = Daten("3", "Bestand", "Teilenummer_FK", "Lager", periode);
            
            //- Aufträge in Warteschlange 
            int WartelisteMap1 = Daten("1", "Menge", "Fehlteil_Teilenummer_FK", "Warteliste_Material", periode);
            int WartelisteMap2 = Daten("2", "Menge", "Fehlteil_Teilenummer_FK", "Warteliste_Material", periode);
            int WartelisteMap3 = Daten("3", "Menge", "Fehlteil_Teilenummer_FK", "Warteliste_Material", periode);
            int WartelisteAr1 = Daten("1", "Menge", "Teilenummer_FK", "Warteliste_Arbeitsplatz", periode);
            int WartelisteAr2 = Daten("2", "Menge", "Teilenummer_FK", "Warteliste_Arbeitsplatz", periode);
            int WartelisteAr3 = Daten("3", "Menge", "Teilenummer_FK", "Warteliste_Arbeitsplatz", periode);
            
            //- Aufträge in Bearbeitung
            int Bearbeitungp1 = Daten("1", "Menge", "Teilenummer_FK", "Bearbeitung", periode);
            int Bearbeitungp2 = Daten("2", "Menge", "Teilenummer_FK", "Bearbeitung", periode);
            int Bearbeitungp3 = Daten("3", "Menge", "Teilenummer_FK", "Bearbeitung", periode);

           // Eingabe Aufträge + eingabe Sicherheitsbestand - Lagerbestand Vorperiode - Aufträge in Warteschlange - Aufträge in Bearbeitung
            textBox1.Text = Convert.ToInt32(p1 + sp1 - lagerbestandp1 - WartelisteAr1 - WartelisteMap1 - Bearbeitungp1).ToString();
            textBox2.Text = Convert.ToInt32(p2 + sp2 - lagerbestandp2 - WartelisteAr2 - WartelisteMap2 - Bearbeitungp2).ToString();
            textBox3.Text = Convert.ToInt32(p3 + sp3 - lagerbestandp3 - WartelisteAr3 - WartelisteMap3 - Bearbeitungp3).ToString();
            
            #region Produktion der Prognosen
            // TO DO Daten aus Import verwenden, dies sind nur Testdaten
            double prognose1p1 = 200;
            double prognose1p2 = 250;
            double prognose1p3 = 100;
            double prognose2p1 = 150;
            double prognose2p2 = 100;
            double prognose2p3 = 300;
            double prognose3p1 = 250;
            double prognose3p2 = 150;
            double prognose3p3 = 300;

            string prognosep1 = Convert.ToInt32((prognose1p1 + prognose2p1 + prognose3p1) / 3 * 1.1).ToString();
            textBox6.Text = prognosep1;
            textBox7.Text = prognosep1;
            textBox10.Text = prognosep1;

            string prognosep2 = Convert.ToInt32((prognose1p2 + prognose2p2 + prognose3p2) / 3 * 1.1).ToString();
            textBox4.Text = prognosep2;
            textBox8.Text = prognosep2;
            textBox11.Text = prognosep2;

            string prognosep3 = Convert.ToInt32((prognose1p3 + prognose2p3 + prognose3p3) / 3 * 1.1).ToString();
            textBox5.Text = prognosep3;
            textBox9.Text = prognosep3;
            textBox12.Text = prognosep3; 
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

        #region textBoxen
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
                    textBox1.ForeColor = Color.Black;;
                }
            }
            check();
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
            check();
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
            check();
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
            check();
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
            check();
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
            check();
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
            check();
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
            check();
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
            check();
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
            check();
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
            check();
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
            check();
        } 

        #endregion

        #region Navigation

        private void continue_btn_Click(object sender, EventArgs e)
        {
            //TO DO zu Kapazitätsplan
            for (int i = 1; i <= 12; ++i)
            {
                if (this.Controls.Find("textBox" + i.ToString(), true)[0].Text == "0")
                {
                    string message = "Sie haben mindestens an einer Stelle angegeben, dass Sie nichts produzieren wollen. Sind Sie sich sicher?";
                    string caption = "Sind Sie sich sicher?";
                    MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                    DialogResult result;

                    result = MessageBox.Show(message, caption, buttons);

                    if (result == DialogResult.Yes)
                    {
                        System.Windows.Forms.MessageBox.Show("Weiter gehts", "Riiiiiiesen", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);
                        break;
                    }
                    break;
                }
                else
                {
                    if (i == 12)
                    {
                        System.Windows.Forms.MessageBox.Show("Weiter gehts", "Riiiiiiesen", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);
                    }
                    else { continue; }
                }
            }

        }

        private void back_Click(object sender, EventArgs e)
        {
            //TO Do zu Sicherheitsbestand
        }
        private void pictureBox3_Click(object sender, EventArgs e)
        {
            //TO DO zu Kapazitätsplan
        }

        private void label6_Click(object sender, EventArgs e)
        {
            //TO DO zu Sicherheitsbestand
        }

        private void label5_Click(object sender, EventArgs e)
        {
            //TO DO zu Startseite
        } 
        #endregion

        private void ETeile_Click(object sender, EventArgs e)
        {
            new Produktion_ETeile().Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            berechneProduktion();
        }
    }

}