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

namespace IBSYS2
{
    public partial class ImportPrognose : Form
    {
        private OleDbConnection myconn;
        public ImportPrognose()
        {
            InitializeComponent();
            string databasename = @"Provider=Microsoft.ACE.OLEDB.12.0; Data Source=IBSYS_DB.accdb";
            myconn = new OleDbConnection(databasename);
            System.Windows.Forms.ToolTip ToolTip1 = new System.Windows.Forms.ToolTip();
            ToolTip1.SetToolTip(this.pictureBox7, "Wählen Sie als erstes die aktuelle Periode aus und betätigen Sie anschließend die bereitgestellte Schaltfläche zum Import der XML-Datei. \nIm Anschluss geben Sie bitte ihre Prognosen für die kommenden Perioden ein. \nAnschließend können Sie mit der Bearbeitung fortfahren.");
            ToolTip1.SetToolTip(this.label11, "Wählen Sie als erstes die aktuelle Periode aus und betätigen Sie anschließend die bereitgestellte Schaltfläche zum Import der XML-Datei.");
            ToolTip1.SetToolTip(this.label12, "Geben Sie nun Ihre Prognose für die nächsten Perioden an.");
        }


        private void button2_Click(object sender, EventArgs e)
        {
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
                    //Aufruf der Klasse XMLReaderClass mit Verarbeitung des XML-Dokuments
                    XMLReaderClass xmlclass = new XMLReaderClass();
                    xmlclass.XMLReader(cmd, File);
                }
                else
                {
                    MessageBox.Show("Wählen Sie eine *.XML-Datei für den Import der Daten aus. \nDiese können Sie unter scsim herunterladen.", "Falsches Format");
                }

            }
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

    }
}