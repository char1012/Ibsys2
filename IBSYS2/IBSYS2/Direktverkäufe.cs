using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IBSYS2
{
    public partial class Direktverkäufe : Form
    {
        private char[] digits = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
        private char[] fordouble = new char[]{'0', '1', '2', '3', '4', '5', '6', '7', '8', '9', ','};
        double[,] direktverkauf = new double[3, 4];
        private String sprache = "de";

        public Direktverkäufe(double[,] direkt, String sprache)
        {
            InitializeComponent();
            this.sprache = sprache;
            sprachen();
            this.direktverkauf = direkt;
            fuelleFelder(direktverkauf);
        }

        private void check()
        {
            bool weiter = true;
            for (int i = 1; i <= 9; ++i)
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
        private void fuelleFelder(double[,] direktverkauf)
        {
            Console.WriteLine(direktverkauf[0, 1]);
            textBox1.Text = direktverkauf[0, 1].ToString();
            textBox4.Text = direktverkauf[0, 2].ToString();
            textBox7.Text = direktverkauf[0, 3].ToString();
            textBox2.Text = direktverkauf[1, 1].ToString();
            textBox5.Text = direktverkauf[1, 2].ToString();
            textBox8.Text = direktverkauf[1, 3].ToString();
            textBox3.Text = direktverkauf[2, 1].ToString();
            textBox6.Text = direktverkauf[2, 2].ToString();
            textBox9.Text = direktverkauf[2, 3].ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            direktverkauf[0, 0] = 1;
            if (textBox1.Text != "")
            {
                direktverkauf[0, 1] = Convert.ToDouble(textBox1.Text);
            }
            else { direktverkauf[0, 1] = 0; }
            if (textBox4.Text != "")
            {
                direktverkauf[0, 2] = Convert.ToDouble(textBox4.Text);
            }
            else { direktverkauf[0, 2] = 0; }
            if (textBox7.Text != "")
            {
                direktverkauf[0, 3] = Convert.ToDouble(textBox7.Text);
            }
            else { direktverkauf[0, 3] = 0; }

            direktverkauf[1, 0] = 2;
            if (textBox2.Text != "")
            {
                direktverkauf[1, 1] = Convert.ToDouble(textBox2.Text);
            }
            else { direktverkauf[1, 1] = 0; }
            if (textBox5.Text != "")
            {
                direktverkauf[1, 2] = Convert.ToDouble(textBox5.Text);
            }
            else { direktverkauf[1, 2] = 0; }
            if (textBox4.Text != "")
            {
                direktverkauf[1, 3] = Convert.ToDouble(textBox8.Text);
            }
            else { direktverkauf[1, 3] = 0; }

            direktverkauf[1, 0] = 3;
            if (textBox2.Text != "")
            {
                direktverkauf[2, 1] = Convert.ToDouble(textBox3.Text);
            }
            else { direktverkauf[2, 1] = 0; }
            if (textBox5.Text != "")
            {
                direktverkauf[2, 2] = Convert.ToDouble(textBox6.Text);
            }
            else { direktverkauf[2, 2] = 0; }
            if (textBox8.Text != "")
            {
                direktverkauf[2, 3] = Convert.ToDouble(textBox9.Text);
            }
            else { direktverkauf[2, 3] = 0; }
            ImportPrognose import = new ImportPrognose(sprache);
            import.Direktverkaeufe(direktverkauf);
            this.Close();
        }

        #region Textboxen TextChanged
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
                    textBox2.ForeColor = Color.Black; ;
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
                    textBox3.ForeColor = Color.Black; ;
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

                    if (!fordouble.Contains<char>(c))
                    {
                        textBox4.ForeColor = Color.Red;
                        okay = false;
                        break;
                    }
                }
                if (okay == true)
                {
                    textBox4.ForeColor = Color.Black; ;
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

                    if (!fordouble.Contains<char>(c))
                    {
                        textBox5.ForeColor = Color.Red;
                        okay = false;
                        break;
                    }
                }
                if (okay == true)
                {
                    textBox5.ForeColor = Color.Black; ;
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

                    if (!fordouble.Contains<char>(c))
                    {
                        textBox6.ForeColor = Color.Red;
                        okay = false;
                        break;
                    }
                }
                if (okay == true)
                {
                    textBox6.ForeColor = Color.Black; ;
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

                    if (!fordouble.Contains<char>(c))
                    {
                        textBox7.ForeColor = Color.Red;
                        okay = false;
                        break;
                    }
                }
                if (okay == true)
                {
                    textBox7.ForeColor = Color.Black; ;
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

                    if (!fordouble.Contains<char>(c))
                    {
                        textBox8.ForeColor = Color.Red;
                        okay = false;
                        break;
                    }
                }
                if (okay == true)
                {
                    textBox8.ForeColor = Color.Black; ;
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

                    if (!fordouble.Contains<char>(c))
                    {
                        textBox9.ForeColor = Color.Red;
                        okay = false;
                        break;
                    }
                }
                if (okay == true)
                {
                    textBox9.ForeColor = Color.Black; ;
                }
            }
            check();
        }

        #endregion


        public void sprachen()
        {
            if (pic_en.SizeMode == PictureBoxSizeMode.StretchImage | sprache != "de")
            {
                groupBox1.Text = Sprachen.EN_DV_GROUPBOX1;
                label4.Text = Sprachen.EN_DV_LABEL4;
                label5.Text = Sprachen.EN_DV_LABEL5;
                label6.Text = Sprachen.EN_DV_LABEL6;
            }
            else
            {
                groupBox1.Text = Sprachen.DE_PRE_GB_ETEILE;
                label4.Text = Sprachen.DE_DV_LABEL4;
                label5.Text = Sprachen.DE_DV_LABEL5;
                label6.Text = Sprachen.DE_DV_LABEL6;

            }
        }

        private void pic_de_Click_1(object sender, EventArgs e)
        {
            pic_de.SizeMode = PictureBoxSizeMode.StretchImage;
            pic_en.SizeMode = PictureBoxSizeMode.Normal;
            sprachen();
            sprache = "de";
        }

        private void pic_en_Click_1(object sender, EventArgs e)
        {
            pic_en.SizeMode = PictureBoxSizeMode.StretchImage;
            pic_de.SizeMode = PictureBoxSizeMode.Normal;
            sprachen();
            sprache = "en";
        }

    }
}
