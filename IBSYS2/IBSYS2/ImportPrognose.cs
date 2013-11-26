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
using System.Data.SqlClient;

namespace IBSYS2
{
    public partial class ImportPrognose : Form
    {
        private OleDbConnection myconn;
        private char[] digits = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

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
            timer1.Interval = 1000;
            timer1.Enabled = true; 

        }

        //http://stackoverflow.com/questions/11445125/disabling-particular-items-in-a-combobox
        //Font myFont = new Font("Aerial", 10, FontStyle.Regular);

        //private void comboBox1_DrawItem(object sender, DrawItemEventArgs e)
        //{
        //    if (e.Index == 0)//We are disabling item based on Index, you can have your logic here
        //    {
        //        e.Graphics.DrawString(comboBox1.Items[e.Index].ToString(), myFont, Brushes.LightGray, e.Bounds);
        //    }
        //    else
        //    {
        //        e.DrawBackground();
        //        e.Graphics.DrawString(comboBox1.Items[e.Index].ToString(), myFont, Brushes.Black, e.Bounds);
        //        e.DrawFocusRectangle();
        //    }
        //} 

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
                        int ausgewähltePeriode = comboBox1.SelectedIndex + 1;
                        cmd.CommandText = @"select Periode from Lager";

                        //Periode aus Datei auslesen sowie Kontrolle, ob es die richtige ist
                        String filename = openFileDialog1.FileName;
                        int period = 0;
                        XmlReader reader = XmlReader.Create(filename);
                        XmlDocument doc = new XmlDocument();
                        doc.Load(filename);
                        XmlNode data = doc.DocumentElement;
                        foreach (XmlNode node in data.SelectNodes("/results"))
                        {
                            period = Convert.ToInt32(node.Attributes["period"].InnerText);
                        }

                        if (ausgewähltePeriode != period )
                        {
                            System.Windows.Forms.MessageBox.Show("Die ausgewählte Datei stimmt nicht mit ihrer ausgewählten Periode überein. Für die Berechnung der neuen Periode wird das XML-File der vergangenen Periode benötigt.", "Falsche Periode/Datei ausgewählt", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);
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
                            if (period == PeriodeDB)
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

            if (tb_aktP1.Text == "")
            {
                tb_aktP1.ForeColor = Color.Red;
                continue_btn.Enabled = false;
            }
            else
            {
                tb_aktP1.ForeColor = Color.Black;
                bool okay = true;
                // neuer Text darf nur Zeichen aus der Liste digits (in der Klasse deklariert)
                foreach (char c in tb_aktP1.Text.ToCharArray())
                {
                    // sobald es ein unpassendes Zeichen gibt, aufhoeren und Fehlermeldung ausgeben
                    if (!digits.Contains<char>(c))
                    {
                        tb_aktP1.ForeColor = Color.Red;
                        okay = false;
                        break;
                    }
                }
                if (okay == true)
                {
                    tb_aktP1.ForeColor = Color.Black;
                    continue_btn.Enabled = true;
                }
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (textBox2.Text == "")
            {
                textBox2.ForeColor = Color.Red;
                continue_btn.Enabled = false;
            }
            else
            {
                textBox2.ForeColor = Color.Black;
                bool okay = true;
                 //neuer Text darf nur Zeichen aus der Liste digits (in der Klasse deklariert)
                foreach (char c in textBox2.Text.ToCharArray())
                {
                     //sobald es ein unpassendes Zeichen gibt, aufhoeren und Fehlermeldung ausgeben
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
                    continue_btn.Enabled = true;
                }
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if (textBox3.Text == "")
            {
                textBox3.ForeColor = Color.Red;
                continue_btn.Enabled = false;
            }
            else
            {
                textBox3.ForeColor = Color.Black;
                bool okay = true;
                // neuer Text darf nur Zeichen aus der Liste digits (in der Klasse deklariert)
                foreach (char c in textBox3.Text.ToCharArray())
                {
                    // sobald es ein unpassendes Zeichen gibt, aufhoeren und Fehlermeldung ausgeben
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
                    continue_btn.Enabled = true;
                }
            }
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            if (textBox6.Text == "")
            {
                textBox6.ForeColor = Color.Red;
                continue_btn.Enabled = false;
            }
            else
            {
                textBox6.ForeColor = Color.Black;
                bool okay = true;
                // neuer Text darf nur Zeichen aus der Liste digits (in der Klasse deklariert)
                foreach (char c in textBox6.Text.ToCharArray())
                {
                    // sobald es ein unpassendes Zeichen gibt, aufhoeren und Fehlermeldung ausgeben
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
                    continue_btn.Enabled = true;
                }
            }
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            if (textBox5.Text == "")
            {
                textBox5.ForeColor = Color.Red;
                continue_btn.Enabled = false;
            }
            else
            {
                textBox5.ForeColor = Color.Black;
                bool okay = true;
                // neuer Text darf nur Zeichen aus der Liste digits (in der Klasse deklariert)
                foreach (char c in textBox5.Text.ToCharArray())
                {
                    // sobald es ein unpassendes Zeichen gibt, aufhoeren und Fehlermeldung ausgeben
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
                    continue_btn.Enabled = true;
                }
            }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            if (textBox4.Text == "")
            {
                textBox4.ForeColor = Color.Red;
                continue_btn.Enabled = false;
            }
            else
            {
                textBox4.ForeColor = Color.Black;
                bool okay = true;
                // neuer Text darf nur Zeichen aus der Liste digits (in der Klasse deklariert)
                foreach (char c in textBox4.Text.ToCharArray())
                {
                    // sobald es ein unpassendes Zeichen gibt, aufhoeren und Fehlermeldung ausgeben
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
                    continue_btn.Enabled = true;
                }
            }
        }

        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            continue_btn.Enabled = false;
            //if (comboBox1.SelectedIndex == 1)
            //    comboBox1.SelectedIndex = -1;
        }

        private void continue_btn_Click(object sender, EventArgs e)
        {

            //if (String.IsNullOrEmpty(textBox12.Text))
            //{
            //    textBox12.ForeColor = Color.Red;
            //    textBox12.Text = "Ausstehend";
            //}
            //else
            //{
            //    try
            //    {
            //        number = Convert.ToDouble(textBox12.Text);
            //        textBox12.ForeColor = Color.Black;

            //    }
            //    catch
            //    {
            //        textBox12.ForeColor = Color.Red;
            //        textBox12.Text = "Gültige Zahl";
            //        return;
            //    }
            //}

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            new ImportPrognose().Hide();
            new Kapazitaetsplan().Show();
            new Kaufteildisposition().Show();
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            if (textBox7.Text == "")
            {
                textBox7.ForeColor = Color.Red;
                continue_btn.Enabled = false;
            }
            else
            {
                textBox7.ForeColor = Color.Black;
                bool okay = true;
                // neuer Text darf nur Zeichen aus der Liste digits (in der Klasse deklariert)
                foreach (char c in textBox7.Text.ToCharArray())
                {
                    // sobald es ein unpassendes Zeichen gibt, aufhoeren und Fehlermeldung ausgeben
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
                    continue_btn.Enabled = true;
                }
            }
        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            if (textBox8.Text == "")
            {
                textBox8.ForeColor = Color.Red;
                continue_btn.Enabled = false;
            }
            else
            {
                textBox8.ForeColor = Color.Black;
                bool okay = true;
                // neuer Text darf nur Zeichen aus der Liste digits (in der Klasse deklariert)
                foreach (char c in textBox8.Text.ToCharArray())
                {
                    // sobald es ein unpassendes Zeichen gibt, aufhoeren und Fehlermeldung ausgeben
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
                    continue_btn.Enabled = true;
                }
            }
        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {
            if (textBox9.Text == "")
            {
                textBox9.ForeColor = Color.Red;
                continue_btn.Enabled = false;
            }
            else
            {
                textBox9.ForeColor = Color.Black;
                bool okay = true;
                // neuer Text darf nur Zeichen aus der Liste digits (in der Klasse deklariert)
                foreach (char c in textBox9.Text.ToCharArray())
                {
                    // sobald es ein unpassendes Zeichen gibt, aufhoeren und Fehlermeldung ausgeben
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
                    continue_btn.Enabled = true;
                }
            }
        }

        private void textBox10_TextChanged(object sender, EventArgs e)
        {
            if (textBox10.Text == "")
            {
                textBox10.ForeColor = Color.Red;
                continue_btn.Enabled = false;
            }
            else
            {
                textBox10.ForeColor = Color.Black;
                bool okay = true;
                // neuer Text darf nur Zeichen aus der Liste digits (in der Klasse deklariert)
                foreach (char c in textBox10.Text.ToCharArray())
                {
                    // sobald es ein unpassendes Zeichen gibt, aufhoeren und Fehlermeldung ausgeben
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
                    continue_btn.Enabled = true;
                }
            }
        }

        private void textBox11_TextChanged(object sender, EventArgs e)
        {
            if (textBox11.Text == "")
            {
                textBox11.ForeColor = Color.Red;
                continue_btn.Enabled = false;
            }
            else
            {
                textBox11.ForeColor = Color.Black;
                bool okay = true;
                // neuer Text darf nur Zeichen aus der Liste digits (in der Klasse deklariert)
                foreach (char c in textBox11.Text.ToCharArray())
                {
                    // sobald es ein unpassendes Zeichen gibt, aufhoeren und Fehlermeldung ausgeben
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
                    continue_btn.Enabled = true;
                }
            }
        }

        private void textBox12_TextChanged(object sender, EventArgs e)
        {
            if (textBox12.Text == "")
            {
                textBox12.ForeColor = Color.Red;
                continue_btn.Enabled = false;
            }
            else
            {
                textBox12.ForeColor = Color.Black;
                bool okay = true;
                // neuer Text darf nur Zeichen aus der Liste digits (in der Klasse deklariert)
                foreach (char c in textBox12.Text.ToCharArray())
                {
                    // sobald es ein unpassendes Zeichen gibt, aufhoeren und Fehlermeldung ausgeben
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
                    continue_btn.Enabled = true;
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

        //    if (tb_aktP1.Text == "")
        //    {
        //        tb_aktP1.ForeColor = Color.Red;
        //        continue_btn.Enabled = false;
        //    }
        //    else
        //    {
        //        tb_aktP1.ForeColor = Color.Black;
        //        bool okay = true;
        //        // neuer Text darf nur Zeichen aus der Liste digits (in der Klasse deklariert)
        //        foreach (char c in tb_aktP1.Text.ToCharArray())
        //        {
        //            // sobald es ein unpassendes Zeichen gibt, aufhoeren und Fehlermeldung ausgeben
        //            if (!digits.Contains<char>(c))
        //            {
        //                tb_aktP1.ForeColor = Color.Red;
        //                okay = false;
        //                break;
        //            }
        //        }
        //        if (okay == true)
        //        {
        //            tb_aktP1.ForeColor = Color.Black;
        //            continue_btn.Enabled = true;
        //        }
        //    }

        //    if (textBox2.Text == "")
        //    {
        //        textBox2.ForeColor = Color.Red;
        //        continue_btn.Enabled = false;
        //    }
        //    else
        //    {
        //        textBox2.ForeColor = Color.Black;
        //        bool okay = true;
        //        // neuer Text darf nur Zeichen aus der Liste digits (in der Klasse deklariert)
        //        foreach (char c in textBox2.Text.ToCharArray())
        //        {
        //            // sobald es ein unpassendes Zeichen gibt, aufhoeren und Fehlermeldung ausgeben
        //            if (!digits.Contains<char>(c))
        //            {
        //                textBox2.ForeColor = Color.Red;
        //                okay = false;
        //                break;
        //            }
        //        }
        //        if (okay == true)
        //        {
        //            textBox2.ForeColor = Color.Black;
        //            continue_btn.Enabled = true;
        //        }
        //    }

        //    if (textBox3.Text == "")
        //    {
        //        textBox3.ForeColor = Color.Red;
        //        continue_btn.Enabled = false;
        //    }
        //    else
        //    {
        //        textBox3.ForeColor = Color.Black;
        //        bool okay = true;
        //        // neuer Text darf nur Zeichen aus der Liste digits (in der Klasse deklariert)
        //        foreach (char c in textBox3.Text.ToCharArray())
        //        {
        //            // sobald es ein unpassendes Zeichen gibt, aufhoeren und Fehlermeldung ausgeben
        //            if (!digits.Contains<char>(c))
        //            {
        //                textBox3.ForeColor = Color.Red;
        //                okay = false;
        //                break;
        //            }
        //        }
        //        if (okay == true)
        //        {
        //            textBox3.ForeColor = Color.Black;
        //            continue_btn.Enabled = true;
        //        }
        //    }

        //    if (textBox4.Text == "")
        //    {
        //        textBox4.ForeColor = Color.Red;
        //        continue_btn.Enabled = false;
        //    }
        //    else
        //    {
        //        textBox4.ForeColor = Color.Black;
        //        bool okay = true;
        //        // neuer Text darf nur Zeichen aus der Liste digits (in der Klasse deklariert)
        //        foreach (char c in textBox4.Text.ToCharArray())
        //        {
        //            // sobald es ein unpassendes Zeichen gibt, aufhoeren und Fehlermeldung ausgeben
        //            if (!digits.Contains<char>(c))
        //            {
        //                textBox4.ForeColor = Color.Red;
        //                okay = false;
        //                break;
        //            }
        //        }
        //        if (okay == true)
        //        {
        //            textBox4.ForeColor = Color.Black;
        //            continue_btn.Enabled = true;
        //        }
        //    }

        //    if (textBox5.Text == "")
        //    {
        //        textBox5.ForeColor = Color.Red;
        //        continue_btn.Enabled = false;
        //    }
        //    else
        //    {
        //        textBox5.ForeColor = Color.Black;
        //        bool okay = true;
        //        // neuer Text darf nur Zeichen aus der Liste digits (in der Klasse deklariert)
        //        foreach (char c in textBox5.Text.ToCharArray())
        //        {
        //            // sobald es ein unpassendes Zeichen gibt, aufhoeren und Fehlermeldung ausgeben
        //            if (!digits.Contains<char>(c))
        //            {
        //                textBox5.ForeColor = Color.Red;
        //                okay = false;
        //                break;
        //            }
        //        }
        //        if (okay == true)
        //        {
        //            textBox5.ForeColor = Color.Black;
        //            continue_btn.Enabled = true;
        //        }
        //    }

        //    if (textBox6.Text == "")
        //    {
        //        textBox6.ForeColor = Color.Red;
        //        continue_btn.Enabled = false;
        //    }
        //    else
        //    {
        //        textBox6.ForeColor = Color.Black;
        //        bool okay = true;
        //        // neuer Text darf nur Zeichen aus der Liste digits (in der Klasse deklariert)
        //        foreach (char c in textBox6.Text.ToCharArray())
        //        {
        //            // sobald es ein unpassendes Zeichen gibt, aufhoeren und Fehlermeldung ausgeben
        //            if (!digits.Contains<char>(c))
        //            {
        //                textBox6.ForeColor = Color.Red;
        //                okay = false;
        //                break;
        //            }
        //        }
        //        if (okay == true)
        //        {
        //            textBox6.ForeColor = Color.Black;
        //            continue_btn.Enabled = true;
        //        }
        //    }

        //    if (textBox7.Text == "")
        //    {
        //        textBox7.ForeColor = Color.Red;
        //        continue_btn.Enabled = false;
        //    }
        //    else
        //    {
        //        textBox7.ForeColor = Color.Black;
        //        bool okay = true;
        //        // neuer Text darf nur Zeichen aus der Liste digits (in der Klasse deklariert)
        //        foreach (char c in textBox7.Text.ToCharArray())
        //        {
        //            // sobald es ein unpassendes Zeichen gibt, aufhoeren und Fehlermeldung ausgeben
        //            if (!digits.Contains<char>(c))
        //            {
        //                textBox7.ForeColor = Color.Red;
        //                okay = false;
        //                break;
        //            }
        //        }
        //        if (okay == true)
        //        {
        //            textBox7.ForeColor = Color.Black;
        //            continue_btn.Enabled = true;
        //        }
        //    }


        //    if (textBox8.Text == "")
        //    {
        //        textBox8.ForeColor = Color.Red;
        //        continue_btn.Enabled = false;
        //    }
        //    else
        //    {
        //        textBox8.ForeColor = Color.Black;
        //        bool okay = true;
        //        // neuer Text darf nur Zeichen aus der Liste digits (in der Klasse deklariert)
        //        foreach (char c in textBox8.Text.ToCharArray())
        //        {
        //            // sobald es ein unpassendes Zeichen gibt, aufhoeren und Fehlermeldung ausgeben
        //            if (!digits.Contains<char>(c))
        //            {
        //                textBox8.ForeColor = Color.Red;
        //                okay = false;
        //                break;
        //            }
        //        }
        //        if (okay == true)
        //        {
        //            textBox8.ForeColor = Color.Black;
        //            continue_btn.Enabled = true;
        //        }
        //    }

        //    if (textBox9.Text == "")
        //    {
        //        textBox9.ForeColor = Color.Red;
        //        continue_btn.Enabled = false;
        //    }
        //    else
        //    {
        //        textBox9.ForeColor = Color.Black;
        //        bool okay = true;
        //        // neuer Text darf nur Zeichen aus der Liste digits (in der Klasse deklariert)
        //        foreach (char c in textBox9.Text.ToCharArray())
        //        {
        //            // sobald es ein unpassendes Zeichen gibt, aufhoeren und Fehlermeldung ausgeben
        //            if (!digits.Contains<char>(c))
        //            {
        //                textBox9.ForeColor = Color.Red;
        //                okay = false;
        //                break;
        //            }
        //        }
        //        if (okay == true)
        //        {
        //            textBox9.ForeColor = Color.Black;
        //            continue_btn.Enabled = true;
        //        }
        //    }

        //    if (textBox10.Text == "")
        //    {
        //        textBox10.ForeColor = Color.Red;
        //        continue_btn.Enabled = false;
        //    }
        //    else
        //    {
        //        textBox10.ForeColor = Color.Black;
        //        bool okay = true;
        //        // neuer Text darf nur Zeichen aus der Liste digits (in der Klasse deklariert)
        //        foreach (char c in textBox10.Text.ToCharArray())
        //        {
        //            // sobald es ein unpassendes Zeichen gibt, aufhoeren und Fehlermeldung ausgeben
        //            if (!digits.Contains<char>(c))
        //            {
        //                textBox10.ForeColor = Color.Red;
        //                okay = false;
        //                break;
        //            }
        //        }
        //        if (okay == true)
        //        {
        //            textBox10.ForeColor = Color.Black;
        //            continue_btn.Enabled = true;
        //        }
        //    }

        //    if (textBox11.Text == "")
        //    {
        //        textBox11.ForeColor = Color.Red;
        //        continue_btn.Enabled = false;
        //    }
        //    else
        //    {
        //        textBox11.ForeColor = Color.Black;
        //        bool okay = true;
        //        // neuer Text darf nur Zeichen aus der Liste digits (in der Klasse deklariert)
        //        foreach (char c in textBox11.Text.ToCharArray())
        //        {
        //            // sobald es ein unpassendes Zeichen gibt, aufhoeren und Fehlermeldung ausgeben
        //            if (!digits.Contains<char>(c))
        //            {
        //                textBox11.ForeColor = Color.Red;
        //                okay = false;
        //                break;
        //            }
        //        }
        //        if (okay == true)
        //        {
        //            textBox11.ForeColor = Color.Black;
        //            continue_btn.Enabled = true;
        //        }
        //    }

        //    if (textBox12.Text == "")
        //    {
        //        textBox12.ForeColor = Color.Red;
        //        continue_btn.Enabled = false;
        //    }
        //    else
        //    {
        //        textBox12.ForeColor = Color.Black;
        //        bool okay = true;
        //        // neuer Text darf nur Zeichen aus der Liste digits (in der Klasse deklariert)
        //        foreach (char c in textBox12.Text.ToCharArray())
        //        {
        //            // sobald es ein unpassendes Zeichen gibt, aufhoeren und Fehlermeldung ausgeben
        //            if (!digits.Contains<char>(c))
        //            {
        //                textBox12.ForeColor = Color.Red;
        //                okay = false;
        //                break;
        //            }
        //        }
        //        if (okay == true)
        //        {
        //            textBox12.ForeColor = Color.Black;
        //            continue_btn.Enabled = true;
        //        }
        //    }
        }


      


    }
}