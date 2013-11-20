﻿using System;
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

    }
}