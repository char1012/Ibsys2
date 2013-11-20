using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Data.OleDb;
using Visiblox.Charts;
using System.Data.SqlClient;

namespace IBSYS2
{
    public partial class ImportPrognose : Form
    {
        private OleDbConnection myconn;
        public ImportPrognose()
        {
            InitializeComponent();
            continue_btn.Enabled = false;
            string databasename = @"Provider=Microsoft.ACE.OLEDB.12.0; Data Source=IBSYS_DB.accdb";
            myconn = new OleDbConnection(databasename);
            System.Windows.Forms.ToolTip ToolTip1 = new System.Windows.Forms.ToolTip();
            ToolTip1.SetToolTip(this.pictureBox7, "Wählen Sie als erstes die aktuelle Periode aus und betätigen Sie anschließend die bereitgestellte Schaltfläche zum Import der XML-Datei. \nIm Anschluss geben Sie bitte ihre Prognosen für die kommenden Perioden ein. \nAnschließend können Sie mit der Bearbeitung fortfahren.");
            ToolTip1.SetToolTip(this.label11, "Wählen Sie als erstes die aktuelle Periode aus und betätigen Sie anschließend die bereitgestellte Schaltfläche zum Import der XML-Datei.");
            ToolTip1.SetToolTip(this.label12, "Geben Sie nun Ihre Prognose für die nächsten Perioden an.");
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem == null)
            {
                System.Windows.Forms.MessageBox.Show("Wählen Sie zuerst die Periode aus.", "Keine Periode ausgewählt", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);
            }
            else
            { 
                openFileDialog1.Title = "Wählen Sie Ihre XML-Datei aus";
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    String File = openFileDialog1.FileName;
                    if (File.Contains(".xml"))
                    {
                        // Initialisierung DB-Verbindung
                        OleDbCommand cmd = new OleDbCommand();
                        cmd.CommandType = CommandType.Text;
                        //MessageBox.Show(System.Environment.CurrentDirectory + "");
                        cmd.Connection = myconn;
                        myconn.Open();
                        int pos = File.IndexOf("result.xml");
                        int per = Convert.ToInt32(File.Substring(pos - 1, 1));
                        int ausgewähltePeriode = comboBox1.SelectedIndex + 1;
                        cmd.CommandText = @"select Periode from Lager";

                        if (ausgewähltePeriode != per)
                        {
                            System.Windows.Forms.MessageBox.Show("Die ausgewählte Datei stimmt nicht mit ihrer ausgewählten Periode überein, überprüfen Sie das bitte.", "Falsche Periode/Datei ausgewählt", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);
                        }
                        else
                        {
                            int PeriodeDB = '0';
                            OleDbDataReader perReader = cmd.ExecuteReader();
                            while (perReader.Read())
                            {
                                PeriodeDB = Convert.ToInt32(perReader["Periode"]);
                            }
                            myconn.Close();
                            if (per == PeriodeDB)
                            {
                                System.Windows.Forms.MessageBox.Show("Die XML-Datei wurde bereits in die Datenbank eingespeist, herzlichen Dank ;-)");
                                continue_btn.Enabled = true;
                            }
                            else
                            {
                                myconn.Open();
                                //Aufruf der Klasse XMLReaderClass mit Verarbeitung des XML-Dokuments
                                XMLReaderClass xmlclass = new XMLReaderClass();
                                xmlclass.XMLReader(cmd, File);
                                continue_btn.Enabled = true;
                                myconn.Close();
                            }

                        }
                    }
                    else
                    {
                        MessageBox.Show("Wählen Sie eine *.XML-Datei für den Import der Daten aus. \nDiese können Sie unter scsim herunterladen.", "Falsches Format");
                    }
                }

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
           
        }

        private void ImportPrognose_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void toolTip1_Popup(object sender, PopupEventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            continue_btn.Enabled = false;
        }

        private void continue_btn_Click(object sender, EventArgs e)
        {
            double number = 0;
            if (String.IsNullOrEmpty(textBox1.Text))
            {
                textBox1.ForeColor = Color.Red;
                textBox1.Text = "Ausstehend";
            }
            else
            {
                try
                {
                    number = Convert.ToDouble(textBox1.Text);
                    textBox1.ForeColor = Color.Black;

                }
                catch
                {
                    textBox1.ForeColor = Color.Red;
                    textBox1.Text = "Gültige Zahl"; 
                    return;
                }
            }
            if (String.IsNullOrEmpty(textBox2.Text))
            {
                textBox2.ForeColor = Color.Red;
                textBox2.Text = "Ausstehend";
            }
            else
            {
                try
                {
                    number = Convert.ToDouble(textBox2.Text);
                    textBox2.ForeColor = Color.Black;
                }
                catch
                {
                    textBox2.ForeColor = Color.Red;
                    textBox2.Text = "Gültige Zahl";
                    return;
                }
            }

            if (String.IsNullOrEmpty(textBox3.Text))
            {
                textBox3.ForeColor = Color.Red;
                textBox3.Text = "Ausstehend";
            }
            else
            {
                try
                {
                    number = Convert.ToDouble(textBox3.Text);
                    textBox3.ForeColor = Color.Black;
                }
                catch
                {
                    textBox3.ForeColor = Color.Red;
                    textBox3.Text = "Gültige Zahl";
                    return;
                }
            }
            if (String.IsNullOrEmpty(textBox4.Text))
            {
                textBox4.ForeColor = Color.Red;
                textBox4.Text = "Ausstehend";
            }
            else
            {
                try
                {
                    number = Convert.ToDouble(textBox4.Text);
                    textBox4.ForeColor = Color.Black;

                }
                catch
                {
                    textBox4.ForeColor = Color.Red;
                    textBox4.Text = "Gültige Zahl";
                    return;
                }
            }
            if (String.IsNullOrEmpty(textBox5.Text))
            {
                textBox5.ForeColor = Color.Red;
                textBox5.Text = "Ausstehend";
            }
            else
            {
                try
                {
                    number = Convert.ToDouble(textBox5.Text);
                    textBox5.ForeColor = Color.Black;

                }
                catch
                {
                    textBox5.ForeColor = Color.Red;
                    textBox5.Text = "Gültige Zahl";
                    return;
                }
            }
            if (String.IsNullOrEmpty(textBox6.Text))
            {
                textBox6.ForeColor = Color.Red;
                textBox6.Text = "Ausstehend";
            }
            else
            {
                try
                {
                    number = Convert.ToDouble(textBox6.Text);
                    textBox6.ForeColor = Color.Black;

                }
                catch
                {
                    textBox6.ForeColor = Color.Red;
                    textBox6.Text = "Gültige Zahl";
                    return;
                }
            }
            if (String.IsNullOrEmpty(textBox7.Text))
            {
                textBox7.ForeColor = Color.Red;
                textBox7.Text = "Ausstehend";
            }
            else
            {
                try
                {
                    number = Convert.ToDouble(textBox7.Text);
                    textBox7.ForeColor = Color.Black;

                }
                catch
                {
                    textBox7.ForeColor = Color.Red;
                    textBox7.Text = "Gültige Zahl";
                    return;
                }
            }
            if (String.IsNullOrEmpty(textBox8.Text))
            {
                textBox8.ForeColor = Color.Red;
                textBox8.Text = "Ausstehend";
            }
            else
            {
                try
                {
                    number = Convert.ToDouble(textBox8.Text);
                    textBox8.ForeColor = Color.Black;

                }
                catch
                {
                    textBox8.ForeColor = Color.Red;
                    textBox8.Text = "Gültige Zahl";
                    return;
                }
            }
            if (String.IsNullOrEmpty(textBox9.Text))
            {
                textBox9.ForeColor = Color.Red;
                textBox9.Text = "Ausstehend";
            }
            else
            {
                try
                {
                    number = Convert.ToDouble(textBox9.Text);
                    textBox9.ForeColor = Color.Black;

                }
                catch
                {
                    textBox9.ForeColor = Color.Red;
                    textBox9.Text = "Gültige Zahl";
                    return;
                }
            }
            if (String.IsNullOrEmpty(textBox10.Text))
            {
                textBox10.ForeColor = Color.Red;
                textBox10.Text = "Ausstehend";
            }
            else
            {
                try
                {
                    number = Convert.ToDouble(textBox10.Text);
                    textBox10.ForeColor = Color.Black;

                }
                catch
                {
                    textBox10.ForeColor = Color.Red;
                    textBox10.Text = "Gültige Zahl";
                    return;
                }
            }
            if (String.IsNullOrEmpty(textBox11.Text))
            {
                textBox11.ForeColor = Color.Red;
                textBox11.Text = "Ausstehend";
            }
            else
            {
                try
                {
                    number = Convert.ToDouble(textBox11.Text);
                    textBox11.ForeColor = Color.Black;

                }
                catch
                {
                    textBox11.ForeColor = Color.Red;
                    textBox11.Text = "Gültige Zahl";
                    return;
                }
            }
            if (String.IsNullOrEmpty(textBox12.Text))
            {
                textBox12.ForeColor = Color.Red;
                textBox12.Text = "Ausstehend";
            }
            else
            {
                try
                {
                    number = Convert.ToDouble(textBox12.Text);
                    textBox12.ForeColor = Color.Black;

                }
                catch
                {
                    textBox12.ForeColor = Color.Red;
                    textBox12.Text = "Gültige Zahl";
                    return;
                }
            }

        }



    }
}