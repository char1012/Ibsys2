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
            DialogResult result = MessageBox.Show("Sind Sie sich sicher, dass Sie die Datenbank leeren möchten?\n"
                + "Dadurch werden alle importierten Daten unwiderruflich gelöscht.", "Datenbank leeren", MessageBoxButtons.YesNo,
                MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

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
                    System.Windows.Forms.MessageBox.Show("DB-Verbindung wurde nicht ordnungsgemäß geschlossen bei der letzten Verwendung, Verbindung wird neu gestartet, bitte haben Sie einen Moment Geduld.");
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
                MessageBox.Show("Alle importierten Daten wurden gelöscht.");
            }
        }

        private void Begrüßungsseite_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result2 = MessageBox.Show("Sind Sie sicher, dass Sie die Anwendung schließen möchten?\n"
                + "Dadurch werden alle Änderungen verworfen.", "Anwendung schließen", MessageBoxButtons.YesNo,
                MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (result2 == DialogResult.No)
            {
                e.Cancel = true;
            }
        }
    }
}
