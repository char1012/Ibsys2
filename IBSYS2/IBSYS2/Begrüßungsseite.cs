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
using System.Threading;

namespace IBSYS2
{
    public partial class Begrüßungsseite : Form
    {
        private OleDbConnection myconn;

        public Begrüßungsseite()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Controls.Clear();
            UserControl import = new ImportPrognose();
            this.Controls.Add(import);
        }

        private void Clear_btn_Click(object sender, EventArgs e)
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

        private void pic_en_Click(object sender, EventArgs e)
        {
            clear_btn.Text = (Sprachen.EN_BTN_CLEAR);
            pic_en.SizeMode = PictureBoxSizeMode.StretchImage;
            pic_de.SizeMode = PictureBoxSizeMode.Normal;
        }

        private void pic_de_Click(object sender, EventArgs e)
        {
            clear_btn.Text = (Sprachen.DE_BTN_CLEAR);
            pic_de.SizeMode = PictureBoxSizeMode.StretchImage;
            pic_en.SizeMode = PictureBoxSizeMode.Normal;
        }


        private void Begrüßungsseite_FormClosing(object sender, FormClosingEventArgs e)
        {

            if (pic_de.SizeMode == PictureBoxSizeMode.StretchImage)
            {
                DialogResult result2 = MessageBox.Show(Sprachen.DE_MSG_INFO1, Sprachen.DE_MSG_INFO2, MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if (result2 == DialogResult.No)
                {
                    e.Cancel = true;
                }
            }
            else
            {
                DialogResult result2 = MessageBox.Show(Sprachen.EN_MSG_INFO1, Sprachen.EN_MSG_INFO2, MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if (result2 == DialogResult.No)
                {
                    e.Cancel = true;
                }
            }
        }
    }
}
