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
    public partial class ImportPrognose : UserControl
    {
        private OleDbConnection myconn;
        private char[] digits = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
        bool tB1 = true, tB2 = true, tB3 = true, tB4 = true, tB5 = true, tB6 = true, tB7 = true, tB8 = true, tB9 = true, tB10 = true, tB11 = true, tB12 = true, tB13 = true, tB14 = true, tB15 = true, fileselected = true;

        public ImportPrognose()
        {
            InitializeComponent();
            button2.Enabled = false;
            continue_btn.Enabled = false;
            string databasename = @"Provider=Microsoft.ACE.OLEDB.12.0; Data Source=IBSYS_DB.accdb";
            myconn = new OleDbConnection(databasename);
            System.Windows.Forms.ToolTip ToolTipDE = new System.Windows.Forms.ToolTip();
            System.Windows.Forms.ToolTip ToolTipEN = new System.Windows.Forms.ToolTip();
            if (pic_de.SizeMode != PictureBoxSizeMode.Normal) { 
                ToolTipDE.SetToolTip(this.pictureBox7, Sprachen.DE_IP_INFO);
                ToolTipDE.SetToolTip(this.lbl_schritt1, Sprachen.DE_IP_INFO_SCHRITT1);
                ToolTipDE.SetToolTip(this.lbl_schritt2, Sprachen.DE_IP_INFO_SCHRITT2);
            }
            else
            {
                ToolTipEN.SetToolTip(this.pictureBox7, Sprachen.EN_IP_INFO);
                ToolTipEN.SetToolTip(this.lbl_schritt1, Sprachen.EN_IP_INFO_SCHRITT1);
                ToolTipEN.SetToolTip(this.lbl_schritt2, Sprachen.EN_IP_INFO_SCHRITT2);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            String text = comboBox1.Text;
            if (text == "Periode 1")
                button2.Enabled = false;
            else
                button2.Enabled = true;
            continue_btn.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //ExportXMLClass exportXML = new ExportXMLClass();
            //exportXML.XMLExport();
           // Kaufteildisposition ktdispo = new Kaufteildisposition();
            UserControl p = new Produktionsreihenfolge();
            //p.Show();
            //ktdispo.ShowDialog();
            this.Controls.Clear();
            //UserControl sicherheit = new Sicherheitsbestand();
            //UserControl ergebnis = new Ergebnis();
            this.Controls.Add(p);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem == null)
            {
                if (pic_de.SizeMode == PictureBoxSizeMode.StretchImage)
                {
                    System.Windows.Forms.MessageBox.Show("Wählen Sie zuerst die Periode aus.", "Keine Periode ausgewählt", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("First, select the period.", "No period selected", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);

                }
            }

            else
            {
                if (pic_de.SizeMode == PictureBoxSizeMode.StretchImage)
                {
                    openFileDialog1.Title = "Wählen Sie Ihre XML-Datei aus";
                }
                else
                {
                    openFileDialog1.Title = "Select your XML file";
                }
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    fileselected = false;
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

                        if ((ausgewähltePeriode - 1) != period)
                        {
                            if (pic_de.SizeMode == PictureBoxSizeMode.StretchImage)
                            {
                                System.Windows.Forms.MessageBox.Show("Die ausgewählte Datei stimmt nicht mit ihrer ausgewählten Periode überein. Für die Berechnung der neuen Periode wird das XML-File der vergangenen Periode benötigt.", "Falsche Periode/Datei ausgewählt", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);
                            }
                            else
                            {
                                System.Windows.Forms.MessageBox.Show("The selected file does not match to its selected period.For the calculation of the new period the XML file from the previous period is required.", "Wrong periode/file selected", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);
                            }
                        }
                        else
                        {
                            fileselected = true;
                            int PeriodeDB = '0';
                            OleDbDataReader perReader = cmd.ExecuteReader();
                            while (perReader.Read())
                            {
                                PeriodeDB = Convert.ToInt32(perReader["Periode"]);
                            }
                            myconn.Close();
                            if (period == PeriodeDB)
                            {
                                // Beim Benutzer nachfragen, ob er das wirklich moechte
                                DialogResult result;
                                if (pic_de.SizeMode == PictureBoxSizeMode.StretchImage)
                                {
                                    result = MessageBox.Show("In der Datenbank sind bereits Daten für diese Periode gespeichert.\n"
                                       + "Wollen Sie die gespeicherten Daten überschreiben?\n"
                                       + "Wenn Sie nein wählen, werden die vorhandenen Daten verwendet.", "Periode bereits vorhanden", MessageBoxButtons.YesNo,
                                       MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                                }
                                else
                                {
                                    result = MessageBox.Show("Data for this period are already stored in the database d.\n"
                                        + "Do you want to overwrite the stored data?\n"
                                        + "If you select No, the existing data will be used.", "Period already exists", MessageBoxButtons.YesNo,
                                        MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                                }

                                if (result == DialogResult.Yes)
                                {
                                    myconn.Open();

                                    // vorhandene Daten der Periode loeschen
                                    cmd.CommandText = @"DELETE * FROM Lager WHERE Periode = " + period + ";";
                                    OleDbDataReader dbReader = cmd.ExecuteReader();
                                    dbReader.Close();
                                    cmd.CommandText = @"DELETE * FROM Bestellung WHERE Periode = " + period + ";";
                                    dbReader = cmd.ExecuteReader();
                                    dbReader.Close();
                                    cmd.CommandText = @"DELETE * FROM Warteliste_Arbeitsplatz WHERE Periode = " + period + ";";
                                    dbReader = cmd.ExecuteReader();
                                    dbReader.Close();
                                    cmd.CommandText = @"DELETE * FROM Warteliste_Material WHERE Periode = " + period + ";";
                                    dbReader = cmd.ExecuteReader();
                                    dbReader.Close();
                                    cmd.CommandText = @"DELETE * FROM Bearbeitung WHERE Periode = " + period + ";";
                                    dbReader = cmd.ExecuteReader();
                                    dbReader.Close();
                                    cmd.CommandText = @"DELETE * FROM Leerzeitenkosten WHERE Periode = " + period + ";";
                                    dbReader = cmd.ExecuteReader();
                                    dbReader.Close();
                                    cmd.CommandText = @"DELETE * FROM Informationen WHERE Periode = " + period + ";";
                                    dbReader = cmd.ExecuteReader();
                                    dbReader.Close();

                                    // XMLReaderClass aufrufen
                                    //Aufruf der Klasse XMLReaderClass mit Verarbeitung des XML-Dokuments
                                    XMLReaderClass xmlclass = new XMLReaderClass();
                                    xmlclass.XMLReader(cmd, File);
                                    if (pic_de.SizeMode == PictureBoxSizeMode.StretchImage)
                                    {
                                        System.Windows.Forms.MessageBox.Show("Die Dateien wurden erfolgreich importiert, vielen Dank für ihre Geduld.", "XML-Datensatz eingelesen");
                                    }
                                    else
                                    {
                                        System.Windows.Forms.MessageBox.Show("The files were imported successfully, thank you for your patience.", "XML-dataset imported");
                                    }
                                    myconn.Close();
                                }

                                //Aufruf Funktion Validierung Werte in Feldern enthalten?
                                if (tB1 & tB2 & tB3 & tB4 & tB5 & tB6 & tB7 & tB8 & tB9 & tB10 & tB11 & tB12 & tB13 & tB14 & tB15 & fileselected) // 
                                {
                                    continue_btn.Enabled = true;
                                }
                            }
                            else
                            {
                                myconn.Open();
                                //Aufruf der Klasse XMLReaderClass mit Verarbeitung des XML-Dokuments
                                XMLReaderClass xmlclass = new XMLReaderClass();
                                xmlclass.XMLReader(cmd, File);
                                if (pic_de.SizeMode == PictureBoxSizeMode.StretchImage)
                                {
                                    System.Windows.Forms.MessageBox.Show("Die Dateien wurden erfolgreich importiert, vielen Dank für ihre Geduld.", "XML-Datensatz eingelesen");
                                }
                                else
                                {
                                    System.Windows.Forms.MessageBox.Show("The files were imported successfully, thank you for your patience.", "XML-dataset imported");
                                }
                                if (tB1 & tB2 & tB3 & tB4 & tB5 & tB6 & tB7 & tB8 & tB9 & tB10 & tB11 & tB12 & tB13 & tB14 & tB15 & fileselected) // 
                                {
                                    continue_btn.Enabled = true;
                                }
                                myconn.Close();
                            }
                        }
                    }
                    else
                    {
                        if (pic_de.SizeMode == PictureBoxSizeMode.StretchImage)
                        {
                            MessageBox.Show("Wählen Sie eine *.XML-Datei für den Import der Daten aus. \nDiese können Sie unter scsim herunterladen.", "Falsches Format");
                        }
                        else
                        {
                            MessageBox.Show("Select a *. XML file to import the data. \nThis can be downloaded from scsim.", "Wrong format");
                        }
                    }
                }

            }
        }

        private void continue_btn_Click(object sender, EventArgs e)
        {
            // Achtung: in button2_click wird der continue_btn bereits enabled,
            // obwohl noch keine Eingabe in den Textfeldern vorgenommen wurde
            if (tb_aktP1.Text == "0" | textBox2.Text == "0" | textBox3.Text == "0" | textBox4.Text == "0" | textBox5.Text == "0" | textBox6.Text == "0" | textBox7.Text == "0" | textBox8.Text == "0" | textBox9.Text == "0" | textBox10.Text == "0" | textBox11.Text == "0" | textBox12.Text == "0")
            {
                valueZero();
                DialogResult dialogResult;
                if (pic_de.SizeMode == PictureBoxSizeMode.StretchImage)
                {
                    dialogResult = MessageBox.Show("In Ihren Eingaben sind noch einige Felder mit der Eingabe 0. Ist dies gewollt?", "Wollen Sie fortfahren?", MessageBoxButtons.YesNo);
                }
                else
                {
                    dialogResult = MessageBox.Show("In your entries are still some fields with the input 0. Is this correct?", "Do you want to continue?", MessageBoxButtons.YesNo);
                }
                if (dialogResult == DialogResult.Yes)
                {
                    // Uebergabe:
                    // aktuelle Periode (Integer)
                    // 12 Integer fuer die Auftraege
                    // 3 Integer fuer Zusatzauftraege
                    // fuer Produktion etc. normale und Zusatzauftraege zusammen rechnen
                    int zLAP1 = Convert.ToInt32(txt_zLAP1.Text);
                    int zLAP2 = Convert.ToInt32(txt_zLAP2.Text);
                    int zLAP3 = Convert.ToInt32(txt_zLAP3.Text);
                    int aPP1 = Convert.ToInt32(tb_aktP1.Text);
                    int aPP2 = Convert.ToInt32(textBox2.Text);
                    int aPP3 = Convert.ToInt32(textBox3.Text);
                    int p2P1 = Convert.ToInt32(textBox4.Text);
                    int p2P2 = Convert.ToInt32(textBox5.Text);
                    int p2P3 = Convert.ToInt32(textBox6.Text);
                    int p3P1 = Convert.ToInt32(textBox7.Text);
                    int p3P2 = Convert.ToInt32(textBox8.Text);
                    int p3P3 = Convert.ToInt32(textBox9.Text);
                    int p4P1 = Convert.ToInt32(textBox10.Text);
                    int p4P2 = Convert.ToInt32(textBox11.Text);
                    int p4P3 = Convert.ToInt32(textBox12.Text);
                    int mengeP1 = zLAP1 + aPP1;
                    int mengeP2 = zLAP2 + aPP2;
                    int mengeP3 = zLAP3 + aPP3;

                    this.Controls.Clear();
                    UserControl sicherheit = new Sicherheitsbestand();
                    this.Controls.Add(sicherheit);
                }
            }
            else
            {
                // Uebergabe:
                // aktuelle Periode (Integer)
                // 12 Integer fuer die Auftraege
                // 3 Integer fuer Zusatzauftraege
                // fuer Produktion etc. normale und Zusatzauftraege zusammen rechnen
                int zLAP1 = Convert.ToInt32(txt_zLAP1.Text);
                int zLAP2 = Convert.ToInt32(txt_zLAP2.Text);
                int zLAP3 = Convert.ToInt32(txt_zLAP3.Text);
                int aPP1 = Convert.ToInt32(tb_aktP1.Text);
                int aPP2 = Convert.ToInt32(textBox2.Text);
                int aPP3 = Convert.ToInt32(textBox3.Text);
                int p2P1 = Convert.ToInt32(textBox4.Text);
                int p2P2 = Convert.ToInt32(textBox5.Text);
                int p2P3 = Convert.ToInt32(textBox6.Text);
                int p3P1 = Convert.ToInt32(textBox7.Text);
                int p3P2 = Convert.ToInt32(textBox8.Text);
                int p3P3 = Convert.ToInt32(textBox9.Text);
                int p4P1 = Convert.ToInt32(textBox10.Text);
                int p4P2 = Convert.ToInt32(textBox11.Text);
                int p4P3 = Convert.ToInt32(textBox12.Text);
                int mengeP1 = zLAP1 + aPP1;
                int mengeP2 = zLAP2 + aPP2;
                int mengeP3 = zLAP3 + aPP3;

                this.Controls.Clear();
                UserControl sicherheit = new Sicherheitsbestand();
                this.Controls.Add(sicherheit);
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

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

            if (tb_aktP1.Text == "")
            {
                tb_aktP1.ForeColor = Color.Red;
                tb_aktP1.Text = "Geben Sie einen Wert ein";
                tB1 = false;
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
                        tB1 = false;
                        continue_btn.Enabled = false;
                        break;
                    }
                }
                if (okay == true)
                {
                    tb_aktP1.ForeColor = Color.Black;
                    tB1 = true;
                    if (tB1 & tB2 & tB3 & tB4 & tB5 & tB6 & tB7 & tB8 & tB9 & tB10 & tB11 & tB12 & tB13 & tB14 & tB15 & fileselected)  
                    {
                        continue_btn.Enabled = true;
                    }
                    else
                    {
                        continue_btn.Enabled = false;
                    }
                }
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (textBox2.Text == "")
            {
                textBox2.ForeColor = Color.Red;
                textBox2.Text = "Geben Sie einen Wert ein";
                tB2 = false;
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
                        tB2 = false;
                        continue_btn.Enabled = false;
                        break;
                    }
                }
                if (okay == true)
                {
                    textBox2.ForeColor = Color.Black;
                    tB2 = true;
                    if (tB1 & tB2 & tB3 & tB4 & tB5 & tB6 & tB7 & tB8 & tB9 & tB10 & tB11 & tB12 & tB13 & tB14 & tB15 & fileselected)
                    {
                        continue_btn.Enabled = true;
                    }
                    else
                    {
                        continue_btn.Enabled = false;
                    }
                }
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if (textBox3.Text == "")
            {
                textBox3.ForeColor = Color.Red;
                textBox3.Text = "Geben Sie einen Wert ein";
                tB3 = false;
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
                        tB3 = false;
                        continue_btn.Enabled = false;
                        break;
                    }
                }
                if (okay == true)
                {
                    textBox3.ForeColor = Color.Black;
                    tB3 = true;
                    if (tB1 & tB2 & tB3 & tB4 & tB5 & tB6 & tB7 & tB8 & tB9 & tB10 & tB11 & tB12 & tB13 & tB14 & tB15 & fileselected)
                    {
                        continue_btn.Enabled = true;
                    }
                    else
                    {
                        continue_btn.Enabled = false;
                    }
                }
            }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            if (textBox4.Text == "")
            {
                textBox4.ForeColor = Color.Red;
                textBox4.Text = "Geben Sie einen Wert ein";
                tB4 = false;
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
                        tB4 = false;
                        continue_btn.Enabled = false;
                        break;
                    }
                }
                if (okay == true)
                {
                    textBox4.ForeColor = Color.Black;
                    tB4 = true;
                    if (tB1 & tB2 & tB3 & tB4 & tB5 & tB6 & tB7 & tB8 & tB9 & tB10 & tB11 & tB12 & tB13 & tB14 & tB15 & fileselected)
                    {
                        continue_btn.Enabled = true;
                    }
                    else
                    {
                        continue_btn.Enabled = false;
                    }
                }
            }
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            if (textBox5.Text == "")
            {
                textBox5.ForeColor = Color.Red;
                textBox5.Text = "Geben Sie einen Wert ein";
                tB5 = false;
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
                        tB5 = false;
                        continue_btn.Enabled = false;
                        break;
                    }
                }
                if (okay == true)
                {
                    textBox5.ForeColor = Color.Black;
                    tB5 = true;
                    if (tB1 & tB2 & tB3 & tB4 & tB5 & tB6 & tB7 & tB8 & tB9 & tB10 & tB11 & tB12 & tB13 & tB14 & tB15 & fileselected)
                    {
                        continue_btn.Enabled = true;
                    }
                    else
                    {
                        continue_btn.Enabled = false;
                    }
                }
            }
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            if (textBox6.Text == "")
            {
                textBox6.ForeColor = Color.Red;
                textBox6.Text = "Geben Sie einen Wert ein";
                tB6 = false;
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
                        tB6 = false;
                        continue_btn.Enabled = false;
                        break;
                    }
                }
                if (okay == true)
                {
                    textBox6.ForeColor = Color.Black;
                    tB6 = true;
                    if (tB1 & tB2 & tB3 & tB4 & tB5 & tB6 & tB7 & tB8 & tB9 & tB10 & tB11 & tB12 & tB13 & tB14 & tB15 & fileselected)
                    {
                        continue_btn.Enabled = true;
                    }
                    else
                    {
                        continue_btn.Enabled = false;
                    }
                }
            }
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            if (textBox7.Text == "")
            {
                textBox7.ForeColor = Color.Red;
                textBox7.Text = "Geben Sie einen Wert ein";
                tB7 = false;
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
                        tB7 = false;
                        continue_btn.Enabled = false;
                        break;
                    }
                }
                if (okay == true)
                {
                    textBox7.ForeColor = Color.Black;
                    tB7 = true;
                    if (tB1 & tB2 & tB3 & tB4 & tB5 & tB6 & tB7 & tB8 & tB9 & tB10 & tB11 & tB12 & tB13 & tB14 & tB15 & fileselected)
                    {
                        continue_btn.Enabled = true;
                    }
                    else
                    {
                        continue_btn.Enabled = false;
                    }
                }
            }
        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            if (textBox8.Text == "")
            {
                textBox8.ForeColor = Color.Red;
                textBox8.Text = "Geben Sie einen Wert ein";
                tB8 = false;
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
                        tB8 = false;
                        continue_btn.Enabled = false;
                        break;
                    }
                }
                if (okay == true)
                {
                    textBox8.ForeColor = Color.Black;
                    tB8 = true;
                    if (tB1 & tB2 & tB3 & tB4 & tB5 & tB6 & tB7 & tB8 & tB9 & tB10 & tB11 & tB12 & tB13 & tB14 & tB15 & fileselected)
                    {
                        continue_btn.Enabled = true;
                    }
                    else
                    {
                        continue_btn.Enabled = false;
                    }
                }
            }
        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {
            if (textBox9.Text == "")
            {
                textBox9.ForeColor = Color.Red;
                textBox9.Text = "Geben Sie einen Wert ein";
                tB9 = false;
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
                        tB9 = false;
                        continue_btn.Enabled = false;
                        break;
                    }
                }
                if (okay == true)
                {
                    textBox9.ForeColor = Color.Black;
                    tB9 = true;
                    if (tB1 & tB2 & tB3 & tB4 & tB5 & tB6 & tB7 & tB8 & tB9 & tB10 & tB11 & tB12 & tB13 & tB14 & tB15 & fileselected)
                    {
                        continue_btn.Enabled = true;
                    }
                    else
                    {
                        continue_btn.Enabled = false;
                    }
                }
            }
        }

        private void textBox10_TextChanged(object sender, EventArgs e)
        {
            if (textBox10.Text == "")
            {
                textBox10.ForeColor = Color.Red;
                textBox10.Text = "Geben Sie einen Wert ein";
                tB10 = false;
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
                        tB10 = false;
                        continue_btn.Enabled = false;
                        break;
                    }
                }
                if (okay == true)
                {
                    textBox10.ForeColor = Color.Black;
                    tB10 = true;
                    if (tB1 & tB2 & tB3 & tB4 & tB5 & tB6 & tB7 & tB8 & tB9 & tB10 & tB11 & tB12 & tB13 & tB14 & tB15 & fileselected)
                    {
                        continue_btn.Enabled = true;
                    }
                    else
                    {
                        continue_btn.Enabled = false;
                    }
                }
            }
        }

        private void textBox11_TextChanged(object sender, EventArgs e)
        {
            if (textBox11.Text == "")
            {
                textBox11.ForeColor = Color.Red;
                textBox11.Text = "Geben Sie einen Wert ein";
                tB11 = false;
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
                        tB11 = false;
                        continue_btn.Enabled = false;
                        break;
                    }
                }
                if (okay == true)
                {
                    textBox11.ForeColor = Color.Black;
                    tB11 = true;
                    if (tB1 & tB2 & tB3 & tB4 & tB5 & tB6 & tB7 & tB8 & tB9 & tB10 & tB11 & tB12 & tB13 & tB14 & tB15 & fileselected)
                    {
                        continue_btn.Enabled = true;
                    }
                    else
                    {
                        continue_btn.Enabled = false;
                    }
                }
            }
        }

        private void textBox12_TextChanged(object sender, EventArgs e)
        {
            if (textBox12.Text == "")
            {
                textBox12.ForeColor = Color.Red;
                textBox12.Text = "Geben Sie einen Wert ein";
                tB12 = false;
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
                        tB12 = false;
                        continue_btn.Enabled = false;
                        break;
                    }
                }
                if (okay == true)
                {
                    textBox12.ForeColor = Color.Black;
                    tB12 = true;
                    if (tB1 & tB2 & tB3 & tB4 & tB5 & tB6 & tB7 & tB8 & tB9 & tB10 & tB11 & tB12 & tB13 & tB14 & tB15 & fileselected)
                    {
                        continue_btn.Enabled = true;
                    }
                    else
                    {
                        continue_btn.Enabled = false;
                    }
                }
            }
        }



        private void txt_zLAP1_TextChanged(object sender, EventArgs e)
        {
            if (txt_zLAP1.Text == "")
            {
                txt_zLAP1.ForeColor = Color.Red;
                txt_zLAP1.Text = "Geben Sie einen Wert ein";
                tB13 = false;
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
                        tB13 = false;
                        continue_btn.Enabled = false;
                        break;
                    }
                }
                if (okay == true)
                {
                    txt_zLAP1.ForeColor = Color.Black;
                    tB13 = true;
                    if (tB1 & tB2 & tB3 & tB4 & tB5 & tB6 & tB7 & tB8 & tB9 & tB10 & tB11 & tB12 & tB13 & tB14 & tB15 & fileselected)
                    {
                        continue_btn.Enabled = true;
                    }
                    else
                    {
                        continue_btn.Enabled = false;
                    }
                }
            }
        }

        private void txt_zLAP2_TextChanged(object sender, EventArgs e)
        {
            if (txt_zLAP2.Text == "")
            {
                txt_zLAP2.ForeColor = Color.Red;
                txt_zLAP2.Text = "Geben Sie einen Wert ein";
                tB14 = false;
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
                        tB14 = false;
                        continue_btn.Enabled = false;
                        break;
                    }
                }
                if (okay == true)
                {
                    txt_zLAP2.ForeColor = Color.Black;
                    tB14 = true;
                    if (tB1 & tB2 & tB3 & tB4 & tB5 & tB6 & tB7 & tB8 & tB9 & tB10 & tB11 & tB12 & tB13 & tB14 & tB15 & fileselected)
                    {
                        continue_btn.Enabled = true;
                    }
                    else
                    {
                        continue_btn.Enabled = false;
                    }
                }
            }
        }

        private void txt_zLAP3_TextChanged(object sender, EventArgs e)
        {
            if (txt_zLAP3.Text == "")
            {
                txt_zLAP3.ForeColor = Color.Red;
                txt_zLAP3.Text = "Geben Sie einen Wert ein";
                tB15 = false;
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
                        tB15 = false;
                        continue_btn.Enabled = false;
                        break;
                    }
                }
                if (okay == true)
                {
                    txt_zLAP3.ForeColor = Color.Black;
                    tB15 = true;
                    if (tB1 & tB2 & tB3 & tB4 & tB5 & tB6 & tB7 & tB8 & tB9 & tB10 & tB11 & tB12 & tB13 & tB14 & tB15 & fileselected)
                    {
                        continue_btn.Enabled = true;
                    }
                    else
                    {
                        continue_btn.Enabled = false;
                    }
                }
            }
        }

        private void valueZero()
        {
            if (tb_aktP1.Text == "0")
            {
                tb_aktP1.ForeColor = Color.Red;
            }
            if (textBox2.Text == "0")
            {
                textBox2.ForeColor = Color.Red;
            }
            if (textBox3.Text == "0")
            {
                textBox3.ForeColor = Color.Red;
            }
            if (textBox4.Text == "0")
            {
                textBox4.ForeColor = Color.Red;
            }
            if (textBox5.Text == "0")
            {
                textBox5.ForeColor = Color.Red;
            }
            if (textBox6.Text == "0")
            {
                textBox6.ForeColor = Color.Red;
            }
            if (textBox7.Text == "0")
            {
                textBox7.ForeColor = Color.Red;
            }
            if (textBox8.Text == "0")
            {
                textBox8.ForeColor = Color.Red;
            }
            if (textBox9.Text == "0")
            {
                textBox9.ForeColor = Color.Red;
            }
            if (textBox10.Text == "0")
            {
                textBox10.ForeColor = Color.Red;
            }
            if (textBox11.Text == "0")
            {
                textBox11.ForeColor = Color.Red;
            }
            if (textBox12.Text == "0")
            {
                textBox12.ForeColor = Color.Red;
            }

        }

       
        public void sprachen()
        {
            if (pic_en.SizeMode == PictureBoxSizeMode.StretchImage)
            {
                //EN Brotkrumenleiste
                lbl_Startseite.Text = (Sprachen.EN_LBL_STARTSEITE);
                lbl_Sicherheitsbestand.Text = (Sprachen.EN_LBL_SICHERHEITSBESTAND);
                lbl_Produktionsreihenfolge.Text = (Sprachen.EN_LBL_PRODUKTIONSREIHENFOLGE);
                lbl_Produktion.Text = (Sprachen.EN_LBL_PRODUKTION);
                lbl_Kapazitaetsplan.Text = (Sprachen.EN_LBL_KAPATITAETSPLAN);
                lbl_Kaufteiledisposition.Text = (Sprachen.EN_LBL_KAUFTEILEDISPOSITION);
                lbl_Ergebnis.Text = (Sprachen.EN_LBL_ERGEBNIS);

                //EN Buttons
                clear_btn.Text = (Sprachen.EN_BTN_CLEAR);
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
                System.Windows.Forms.ToolTip ToolTipEN = new System.Windows.Forms.ToolTip();
                ToolTipEN.SetToolTip(this.pictureBox7, Sprachen.EN_IP_INFO);
                ToolTipEN.SetToolTip(this.lbl_schritt1, Sprachen.EN_IP_INFO_SCHRITT1);
                ToolTipEN.SetToolTip(this.lbl_schritt2, Sprachen.EN_IP_INFO_SCHRITT2);

                //EN ComboBox
                comboBox1.Text = (Sprachen.EN_CB_IP_PERIODE_AUSWAEHLEN);
            }
            else
            {
                //DE Brotkrumenleiste
                lbl_Sicherheitsbestand.Text = (Sprachen.DE_LBL_SICHERHEITSBESTAND);
                lbl_Startseite.Text = (Sprachen.DE_LBL_STARTSEITE);
                lbl_Produktionsreihenfolge.Text = (Sprachen.DE_LBL_PRODUKTIONSREIHENFOLGE);
                lbl_Produktion.Text = (Sprachen.DE_LBL_PRODUKTION);
                lbl_Kapazitaetsplan.Text = (Sprachen.DE_LBL_KAPATITAETSPLAN);
                lbl_Kaufteiledisposition.Text = (Sprachen.DE_LBL_KAUFTEILEDISPOSITION);
                lbl_Ergebnis.Text = (Sprachen.DE_LBL_ERGEBNIS);

                //DE Buttons
                clear_btn.Text = (Sprachen.DE_BTN_CLEAR);
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
                System.Windows.Forms.ToolTip ToolTipDE = new System.Windows.Forms.ToolTip();
                ToolTipDE.SetToolTip(this.pictureBox7, Sprachen.DE_IP_INFO);
                ToolTipDE.SetToolTip(this.lbl_schritt1, Sprachen.DE_IP_INFO_SCHRITT1);
                ToolTipDE.SetToolTip(this.lbl_schritt2, Sprachen.DE_IP_INFO_SCHRITT2);

                //DE ComboBox
                comboBox1.Text = (Sprachen.DE_CB_IP_PERIODE_AUSWAEHLEN);
            }
        }

        private void lbl_Sicherheitsbestand_Click(object sender, EventArgs e)
        {
            this.Controls.Clear();
            UserControl sicherheit = new Sicherheitsbestand();
            this.Controls.Add(sicherheit);
        }

        private void clear_btn_Click(object sender, EventArgs e)
        {
            // Beim Benutzer nachfragen, ob er das wirklich moechte
            DialogResult result;
            if (pic_de.SizeMode == PictureBoxSizeMode.StretchImage)
            {
                result = MessageBox.Show("Sind Sie sich sicher, dass Sie die Datenbank leeren möchten?\n"
                    + "Dadurch werden alle importierten Daten unwiderruflich gelöscht.", "Datenbank leeren", MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            }
            else
            {
                result = MessageBox.Show("Are you sure, that you want to clear the database??\n"
                + "All the imported data will be deleted.", "Clear database", MessageBoxButtons.YesNo,
                MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            }

            // wenn ja, die entsprechenden Tabellen der DB leeren
            if (result == DialogResult.Yes)
            {
                // DB-Verbindung
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

                // Mitteilung einblenden
                ProcessMessage message = new ProcessMessage();
                message.Show(this);
                message.Update();
                this.Enabled = false;

                // alle Import-Tabellen leeren und Ids zuruecksetzen (7 Tabellen betroffen)
                cmd.CommandText = @"DELETE * FROM Lager";
                OleDbDataReader dbReader = cmd.ExecuteReader();
                dbReader.Close();
                cmd.CommandText = @"DELETE * FROM Bestellung";
                dbReader = cmd.ExecuteReader();
                dbReader.Close();
                cmd.CommandText = @"DELETE * FROM Warteliste_Arbeitsplatz";
                dbReader = cmd.ExecuteReader();
                dbReader.Close();
                cmd.CommandText = @"DELETE * FROM Warteliste_Material";
                dbReader = cmd.ExecuteReader();
                dbReader.Close();
                cmd.CommandText = @"DELETE * FROM Bearbeitung";
                dbReader = cmd.ExecuteReader();
                dbReader.Close();
                cmd.CommandText = @"DELETE * FROM Leerzeitenkosten";
                dbReader = cmd.ExecuteReader();
                dbReader.Close();
                cmd.CommandText = @"DELETE * FROM Informationen";
                dbReader = cmd.ExecuteReader();
                dbReader.Close();

                message.Close();
                this.Enabled = true;
                if (pic_de.SizeMode == PictureBoxSizeMode.StretchImage)
                {
                    MessageBox.Show("Alle importierten Daten wurden gelöscht.");
                }
                else
                {
                    MessageBox.Show("All imported data has been cleared.");

                }
            }
        }

    }
}