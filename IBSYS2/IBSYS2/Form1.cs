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
    public partial class Form1 : Form
    {
        private OleDbConnection myconn;
        public Form1()
        {
            InitializeComponent();
            string databasename = @"Provider=Microsoft.ACE.OLEDB.12.0; Data Source=IBSYS_DB.accdb";
            myconn = new OleDbConnection(databasename);
        }

       
        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Mein Git funktioniert jetzt auch :) AC", "SPAGHETTIMONSTER");
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

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

    }
}