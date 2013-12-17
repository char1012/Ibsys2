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
        private String sprache = "de";
        private char[] digits = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
        int[,] sicherheitsbe = new int[30, 2];
        int[,] berProduktion = new int[30, 2];

        int[,] backupProduktion = new int[30, 2];

        public Produktion_ETeile(int[,] beProduktion, int[,] sicherheitsbest, String sprache)
        {
            this.sprache = sprache;
            InitializeComponent();
            sprachen();
            string databasename = @"Provider=Microsoft.ACE.OLEDB.12.0; Data Source=IBSYS_DB.accdb";
            myconn = new OleDbConnection(databasename);

            this.sicherheitsbe = sicherheitsbest;
            this.berProduktion = beProduktion;
            this.backupProduktion = beProduktion;

            button1.Enabled = false;

            berechneProduktion(berProduktion);
        }

        private void berechneProduktion(int[,] beProduktion)
        {

               #region In textBox
                 if (beProduktion[3, 1].ToString().StartsWith("-"))
                 {
                     textBox1.Text = "0";
                 }
                 else
                 {
                     textBox1.Text = beProduktion[3, 1].ToString();
                 }

                 if (beProduktion[4, 1].ToString().StartsWith("-"))
                 {
                     textBox2.Text = "0";
                 }
                 else
                 {
                     textBox2.Text = beProduktion[4, 1].ToString();
                 }

                 if (beProduktion[5, 1].ToString().StartsWith("-"))
                 {
                     textBox3.Text = "0";
                 }
                 else
                 {
                     textBox3.Text = beProduktion[5, 1].ToString();
                 }

                 if (beProduktion[6, 1].ToString().StartsWith("-"))
                 {
                     textBox4.Text = "0";
                 }
                 else
                 {
                     textBox4.Text = beProduktion[6, 1].ToString();
                 }

                 if (beProduktion[7, 1].ToString().StartsWith("-"))
                 {
                     textBox5.Text = "0";
                 }
                 else
                 {
                     textBox5.Text = beProduktion[7, 1].ToString();
                 }

                 if (beProduktion[8, 1].ToString().StartsWith("-"))
                 {
                     textBox6.Text = "0";
                 }
                 else
                 {
                     textBox6.Text = beProduktion[8, 1].ToString();
                 }

                 if (beProduktion[9, 1].ToString().StartsWith("-"))
                 {
                     textBox7.Text = "0";
                 }
                 else
                 {
                     textBox7.Text = beProduktion[9, 1].ToString();
                 }

                 if (beProduktion[10, 1].ToString().StartsWith("-"))
                 {
                     textBox8.Text = "0";
                 }
                 else
                 {
                     textBox8.Text = beProduktion[10, 1].ToString();
                 }

                 if (beProduktion[11, 1].ToString().StartsWith("-"))
                 {
                     textBox9.Text = "0";
                 }
                 else
                 {
                     textBox9.Text = beProduktion[11, 1].ToString();
                 }

                 if (beProduktion[12, 1].ToString().StartsWith("-"))
                 {
                     textBox10.Text = "0";
                 }
                 else
                 {
                     textBox10.Text = beProduktion[12, 1].ToString();
                 }

                 if (beProduktion[13, 1].ToString().StartsWith("-"))
                 {
                     textBox11.Text = "0";
                 }
                 else
                 {
                     textBox11.Text = beProduktion[13, 1].ToString();
                 }

                 if (beProduktion[14, 1].ToString().StartsWith("-"))
                 {
                     textBox12.Text = "0";
                 }
                 else
                 {
                     textBox12.Text = beProduktion[14, 1].ToString();
                 }

                 if (beProduktion[15, 1].ToString().StartsWith("-"))
                 {
                     textBox13.Text = "0";
                 }
                 else
                 {
                     textBox13.Text = beProduktion[15, 1].ToString();
                 }

                 if (beProduktion[16, 1].ToString().StartsWith("-"))
                 {
                     textBox14.Text = "0";
                 }
                 else
                 {
                     textBox14.Text = beProduktion[16, 1].ToString();
                 }

                 if (beProduktion[17, 1].ToString().StartsWith("-"))
                 {
                     textBox15.Text = "0";
                 }
                 else
                 {
                     textBox15.Text = beProduktion[17, 1].ToString();
                 }

                 if (beProduktion[18, 1].ToString().StartsWith("-"))
                 {
                     textBox16.Text = "0";
                 }
                 else
                 {
                     textBox16.Text = beProduktion[18, 1].ToString();
                 }

                 if (beProduktion[19, 1].ToString().StartsWith("-"))
                 {
                     textBox17.Text = "0";
                 }
                 else
                 {
                     textBox17.Text = beProduktion[19, 1].ToString();
                 }

                 if (beProduktion[20, 1].ToString().StartsWith("-"))
                 {
                     textBox18.Text = "0";
                 }
                 else
                 {
                     textBox18.Text = beProduktion[20, 1].ToString();
                 }

                 if (beProduktion[21, 1].ToString().StartsWith("-"))
                 {
                     textBox19.Text = "0";
                 }
                 else
                 {
                     textBox19.Text = beProduktion[21, 1].ToString();
                 }

                 if (beProduktion[22, 1].ToString().StartsWith("-"))
                 {
                     textBox20.Text = "0";
                 }
                 else
                 {
                     textBox20.Text = beProduktion[22, 1].ToString();
                 }

                 if (beProduktion[23, 1].ToString().StartsWith("-"))
                 {
                     textBox21.Text = "0";
                 }
                 else
                 {
                     textBox21.Text = beProduktion[23, 1].ToString();
                 }

                 if (beProduktion[24, 1].ToString().StartsWith("-"))
                 {
                     textBox22.Text = "0";
                 }
                 else
                 {
                     textBox22.Text = beProduktion[24, 1].ToString();
                 }

                 if (beProduktion[25, 1].ToString().StartsWith("-"))
                 {
                     textBox23.Text = "0";
                 }
                 else
                 {
                     textBox23.Text = beProduktion[25, 1].ToString();
                 }

                 if (beProduktion[26, 1].ToString().StartsWith("-"))
                 {
                     textBox24.Text = "0";
                 }
                 else
                 {
                     textBox24.Text = beProduktion[26, 1].ToString();
                 }

                 if (beProduktion[27, 1].ToString().StartsWith("-"))
                 {
                     textBox25.Text = "0";
                 }
                 else
                 {
                     textBox25.Text = beProduktion[27, 1].ToString();
                 }

                 if (beProduktion[28, 1].ToString().StartsWith("-"))
                 {
                     textBox26.Text = "0";
                 }
                 else
                 {
                     textBox26.Text = beProduktion[28, 1].ToString();
                 }

                 if (beProduktion[29, 1].ToString().StartsWith("-"))
                 {
                     textBox27.Text = "0";
                 }
                 else
                 {
                     textBox27.Text = beProduktion[29, 1].ToString();
                 }

                 #endregion
             
        }

        private void check()
        {
            bool weiter = true;
            for (int i = 1; i <= 27; ++i)
            {
                if (this.Controls.Find("textBox" + i.ToString(), true)[0].Text == "" || this.Controls.Find("textBox" + i.ToString(), true)[0].ForeColor == Color.Red)
                {
                    weiter = false;

                }
                else
                {
                    continue;
                }
            }
            if (weiter == true)
            {
                button1.Enabled = true;
            }
            else
            {
                button1.Enabled = false;
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            berProduktion[3, 1] = Convert.ToInt32(textBox1.Text);
            berProduktion[4, 1] = Convert.ToInt32(textBox2.Text);
            berProduktion[5, 1] = Convert.ToInt32(textBox3.Text);
            berProduktion[6, 1] = Convert.ToInt32(textBox4.Text);
            berProduktion[7, 1] = Convert.ToInt32(textBox5.Text);
            berProduktion[8, 1] = Convert.ToInt32(textBox6.Text);
            berProduktion[9, 1] = Convert.ToInt32(textBox7.Text);
            berProduktion[10, 1] = Convert.ToInt32(textBox8.Text);
            berProduktion[11, 1] = Convert.ToInt32(textBox9.Text); 
            berProduktion[12, 1] = Convert.ToInt32(textBox10.Text); 
            berProduktion[13, 1] = Convert.ToInt32(textBox11.Text);
            berProduktion[14, 1] = Convert.ToInt32(textBox12.Text);
            berProduktion[15, 1] = Convert.ToInt32(textBox13.Text);
            berProduktion[16, 1] = Convert.ToInt32(textBox14.Text);
            berProduktion[17, 1] = Convert.ToInt32(textBox15.Text);
            berProduktion[18, 1] = Convert.ToInt32(textBox16.Text);
            berProduktion[19, 1] = Convert.ToInt32(textBox17.Text);
            berProduktion[20, 1] = Convert.ToInt32(textBox18.Text);
            berProduktion[21, 1] = Convert.ToInt32(textBox19.Text);
            berProduktion[22, 1] = Convert.ToInt32(textBox20.Text);
            berProduktion[23, 1] = Convert.ToInt32(textBox21.Text);
            berProduktion[24, 1] = Convert.ToInt32(textBox22.Text);
            berProduktion[25, 1] = Convert.ToInt32(textBox23.Text);
            berProduktion[26, 1] = Convert.ToInt32(textBox24.Text);
            berProduktion[27, 1] = Convert.ToInt32(textBox25.Text);
            berProduktion[28, 1] = Convert.ToInt32(textBox26.Text);
            berProduktion[29, 1] = Convert.ToInt32(textBox27.Text);
            Produktion prod = new Produktion();
            prod.vonProduktionEteile(berProduktion);
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
            check();
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
            check();
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
            check();
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
            check();
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
            check();
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
            check();
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
            check();
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
            check();
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
            check();
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
            check();
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
            check();
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
            check();
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
            check();
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
            check();
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
            check();
        }
        #endregion

        private void button2_Click(object sender, EventArgs e)
        {
            Produktion prod = new Produktion(sicherheitsbe);
            backupProduktion = prod.ProduktionETeile();       
            berechneProduktion(backupProduktion);
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
