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
using System.Resources;

namespace IBSYS2
{
    public partial class ImportPrognose : Form
    {
        private OleDbConnection myconn;
        private char[] digits = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

        public ImportPrognose()
        {
            InitializeComponent();
            button2.Enabled = false;
            continue_btn.Enabled = false;
            string databasename = @"Provider=Microsoft.ACE.OLEDB.12.0; Data Source=IBSYS_DB.accdb";
            myconn = new OleDbConnection(databasename);
           
            String sprache = "de";
            if (sprache == "de") { 
                System.Windows.Forms.ToolTip ToolTipP = new System.Windows.Forms.ToolTip();
                ToolTipP.SetToolTip(this.pictureBox7, Sprachen.DE_IP_INFO);
                System.Windows.Forms.ToolTip ToolTipP1 = new System.Windows.Forms.ToolTip();
                ToolTipP.SetToolTip(this.lbl_schritt1, Sprachen.DE_IP_INFO_SCHRITT1);
                System.Windows.Forms.ToolTip ToolTipP2 = new System.Windows.Forms.ToolTip();
                ToolTipP.SetToolTip(this.lbl_schritt2, Sprachen.DE_IP_INFO_SCHRITT2);
            }
            else
            {
                System.Windows.Forms.ToolTip ToolTipP = new System.Windows.Forms.ToolTip();
                ToolTipP.SetToolTip(this.pictureBox7, Sprachen.EN_IP_INFO);
                System.Windows.Forms.ToolTip ToolTipP1 = new System.Windows.Forms.ToolTip();
                ToolTipP.SetToolTip(this.lbl_schritt1, Sprachen.EN_IP_INFO_SCHRITT1);
                System.Windows.Forms.ToolTip ToolTipP2 = new System.Windows.Forms.ToolTip();
                ToolTipP.SetToolTip(this.lbl_schritt2, Sprachen.EN_IP_INFO_SCHRITT2);
            }
       //     System.Windows.Forms.ToolTip ToolTip1 = new System.Windows.Forms.ToolTip();

            // Arne: kannd as weg? Ungenutzter Code macht Klasse furchtbar unuebersichtlich...
            //timer1.Interval = 1000;
            //timer1.Enabled = true; 
        }

        // Arne: kann das weg?
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

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            String text = comboBox1.Text;
            if (text == "Periode 1")
                button2.Enabled = false;
            else
                button2.Enabled = true;
            continue_btn.Enabled = false;
        }

        // Sprung-Button, kann spaeter entfernt werden
        private void button1_Click(object sender, EventArgs e)
        {
            this.Controls.Clear();
            UserControl sicherheit = new Sicherheitsbestand();
            this.Controls.Add(sicherheit);
        }

        private void button2_Click(object sender, EventArgs e)
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

                        if (ausgewähltePeriode != period)
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

        private void continue_btn_Click(object sender, EventArgs e)
        {
            // Achtung: in button2_click wird der continue_btn bereits enabled,
            // obwohl noch keine Eingabe in den Textfeldern vorgenommen wurde

            // Uebergabe:
            // aktuelle Periode (Integer)
            // 12 Integer fuer die Auftraege
            // 3 Integer fuer Zusatzauftraege
            int zLAP1 = Convert.ToInt32(txt_zLAP1.Text);
            int zLAP2 = Convert.ToInt32(txt_zLAP2.Text);
            int zLAP3 = Convert.ToInt32(txt_zLAP3.Text);
            int aPP1 = Convert.ToInt32(tb_aktP1.Text);
            int aPP2 = Convert.ToInt32(textBox2.Text);
            int aPP3 = Convert.ToInt32(textBox3.Text);
            int mengeP1 = zLAP1 + aPP1;
            int mengeP2 = zLAP2 + aPP2;
            int mengeP3 = zLAP3 + aPP3;
            //   System.Windows.Forms.MessageBox.Show("Menge P1: " + mengeP1 + " Menge P2: " + mengeP2 + " Menge P3: " + mengeP3);

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

        private void pic_en_Click(object sender, EventArgs e)
        {
            string sprache = "en";
            sprachen(sprache);
        }

        private void pic_de_Click(object sender, EventArgs e)
        {
            string sprache = "de";
            sprachen(sprache);
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

        // Arne: kann das weg?
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

        private void txt_zLAP1_TextChanged(object sender, EventArgs e)
        {
            if (txt_zLAP1.Text == "")
            {
                txt_zLAP1.ForeColor = Color.Red;
                continue_btn.Enabled = false;
            }
            else
            {
                txt_zLAP1.ForeColor = Color.Black;
                bool okay = true;
                // neuer Text darf nur Zeichen aus der Liste digits (in der Klasse deklariert)
                foreach (char c in txt_zLAP1.Text.ToCharArray())
                {
                    // sobald es ein unpassendes Zeichen gibt, aufhoeren und Fehlermeldung ausgeben
                    if (!digits.Contains<char>(c))
                    {
                        txt_zLAP1.ForeColor = Color.Red;
                        okay = false;
                        break;
                    }
                }
                if (okay == true)
                {
                    txt_zLAP1.ForeColor = Color.Black;
                    continue_btn.Enabled = true;
                }
            }
        }

        private void txt_zLAP2_TextChanged(object sender, EventArgs e)
        {
            if (txt_zLAP2.Text == "")
            {
                txt_zLAP2.ForeColor = Color.Red;
                continue_btn.Enabled = false;
            }
            else
            {
                txt_zLAP2.ForeColor = Color.Black;
                bool okay = true;
                // neuer Text darf nur Zeichen aus der Liste digits (in der Klasse deklariert)
                foreach (char c in txt_zLAP2.Text.ToCharArray())
                {
                    // sobald es ein unpassendes Zeichen gibt, aufhoeren und Fehlermeldung ausgeben
                    if (!digits.Contains<char>(c))
                    {
                        txt_zLAP2.ForeColor = Color.Red;
                        okay = false;
                        break;
                    }
                }
                if (okay == true)
                {
                    txt_zLAP2.ForeColor = Color.Black;
                    continue_btn.Enabled = true;
                }
            }
        }

        private void txt_zLAP3_TextChanged(object sender, EventArgs e)
        {
            if (txt_zLAP3.Text == "")
            {
                txt_zLAP3.ForeColor = Color.Red;
                continue_btn.Enabled = false;
            }
            else
            {
                txt_zLAP3.ForeColor = Color.Black;
                bool okay = true;
                // neuer Text darf nur Zeichen aus der Liste digits (in der Klasse deklariert)
                foreach (char c in txt_zLAP3.Text.ToCharArray())
                {
                    // sobald es ein unpassendes Zeichen gibt, aufhoeren und Fehlermeldung ausgeben
                    if (!digits.Contains<char>(c))
                    {
                        txt_zLAP3.ForeColor = Color.Red;
                        okay = false;
                        break;
                    }
                }
                if (okay == true)
                {
                    txt_zLAP3.ForeColor = Color.Black;
                    continue_btn.Enabled = true;
                }
            }
        }

        public void sprachen(String sprache)
        {
            if (sprache != "de")
            {
                //EN Brotkrumenleiste
                lbl_Startseite.Text = (Sprachen.EN_LBL_STARTSEITE);
                lbl_Sicherheitsbestand.Text = (Sprachen.EN_LBL_SICHERHEITSBESTAND);
                lbl_Produktion.Text = (Sprachen.EN_LBL_PRODUKTION);
                lbl_Kapazitaetsplan.Text = (Sprachen.EN_LBL_KAPATITAETSPLAN);
                lbl_Kaufteiledisposition.Text = (Sprachen.EN_LBL_KAUFTEILEDISPOSITION);
                lbl_Ergebnis.Text = (Sprachen.EN_LBL_ERGEBNIS);

                //EN Buttons
                continue_btn.Text = (Sprachen.EN_BTN_IP_BERECHNUNG_STARTEN);
                button1.Text = (Sprachen.EN_BTN_IP_SPRUNG);
                button2.Text = (Sprachen.EN_BTN_IP_DATEI_AUSWAEHLEN);

                //EN Groupboxen
                groupBox1.Text = (Sprachen.EN_IP_GROUPBOX1);

                //EN Labels
                lbl_schritt1.Text = (Sprachen.EN_LBL_IP_SCHRITT1);
                lbl_schritt2.Text = (Sprachen.EN_LBL_IP_SCHRITT2);
                lbl_schritt3.Text = (Sprachen.EN_LBL_IP_SCHRITT3);
                lbl_aktuellePeriode.Text = (Sprachen.EN_LBL_IP_AKTUELLE_PERIODE);
                lbl_periodeX.Text = (Sprachen.EN_LBL_IP_PERIODEX);
                lbl_periodeX1.Text = (Sprachen.EN_LBL_IP_PERIODEX1);
                lbl_periodeX2.Text = (Sprachen.EN_LBL_IP_PERIODEX2);

                //EN Tooltip
                System.Windows.Forms.ToolTip ToolTipP = new System.Windows.Forms.ToolTip();
                ToolTipP.SetToolTip(this.pictureBox7, Sprachen.EN_IP_INFO);
                System.Windows.Forms.ToolTip ToolTipP1 = new System.Windows.Forms.ToolTip();
                ToolTipP.SetToolTip(this.lbl_schritt1, Sprachen.EN_IP_INFO_SCHRITT1);
                System.Windows.Forms.ToolTip ToolTipP2 = new System.Windows.Forms.ToolTip();
                ToolTipP.SetToolTip(this.lbl_schritt2, Sprachen.EN_IP_INFO_SCHRITT2);

                //EN ComboBox
                comboBox1.Text = (Sprachen.EN_CB_IP_PERIODE_AUSWAEHLEN);
            }
            else
            {
                //DE Brotkrumenleiste
                lbl_Sicherheitsbestand.Text = (Sprachen.DE_LBL_SICHERHEITSBESTAND);
                lbl_Startseite.Text = (Sprachen.DE_LBL_STARTSEITE);
                lbl_Produktion.Text = (Sprachen.DE_LBL_PRODUKTION);
                lbl_Kapazitaetsplan.Text = (Sprachen.DE_LBL_KAPATITAETSPLAN);
                lbl_Kaufteiledisposition.Text = (Sprachen.DE_LBL_KAUFTEILEDISPOSITION);
                lbl_Ergebnis.Text = (Sprachen.DE_LBL_ERGEBNIS);

                //DE Buttons
                continue_btn.Text = (Sprachen.DE_BTN_IP_BERECHNUNG_STARTEN);
                button1.Text = (Sprachen.DE_BTN_IP_SPRUNG);
                button2.Text = (Sprachen.DE_BTN_IP_DATEI_AUSWAEHLEN);


                //DE Groupboxen
                groupBox1.Text = (Sprachen.DE_IP_GROUPBOX1);

                //DE Labels
                lbl_schritt1.Text = (Sprachen.DE_LBL_IP_SCHRITT1);
                lbl_schritt2.Text = (Sprachen.DE_LBL_IP_SCHRITT2);
                lbl_schritt3.Text = (Sprachen.DE_LBL_IP_SCHRITT3);
                lbl_optional.Text = (Sprachen.DE_LBL_IP_OPTIONAL);
                lbl_aktuellePeriode.Text = (Sprachen.DE_LBL_IP_AKTUELLE_PERIODE);
                lbl_periodeX.Text = (Sprachen.DE_LBL_IP_PERIODEX);
                lbl_periodeX1.Text = (Sprachen.DE_LBL_IP_PERIODEX1);
                lbl_periodeX2.Text = (Sprachen.DE_LBL_IP_PERIODEX2);

                //DE Tooltip
                System.Windows.Forms.ToolTip ToolTipP = new System.Windows.Forms.ToolTip();
                ToolTipP.SetToolTip(this.pictureBox7, Sprachen.DE_IP_INFO);
                System.Windows.Forms.ToolTip ToolTipP1 = new System.Windows.Forms.ToolTip();
                ToolTipP.SetToolTip(this.lbl_schritt1, Sprachen.DE_IP_INFO_SCHRITT1);
                System.Windows.Forms.ToolTip ToolTipP2 = new System.Windows.Forms.ToolTip();
                ToolTipP.SetToolTip(this.lbl_schritt2, Sprachen.DE_IP_INFO_SCHRITT2);

                //DE ComboBox
                comboBox1.Text = (Sprachen.DE_CB_IP_PERIODE_AUSWAEHLEN);
            }
        }

    }
}