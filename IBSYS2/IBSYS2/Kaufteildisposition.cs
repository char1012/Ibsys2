using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data.OleDb;



namespace IBSYS2
{
    public partial class Kaufteildisposition : Form
    {
        private OleDbConnection myconn;
        public Kaufteildisposition()
        {
            
            InitializeComponent();
            //Annahme Produktion P1/P2/P3 wie folgt, Daten müssen später aus entsprechendem Feld ausgelesen werden, sobald programmiert von Zuständigen

            int prodP1 = 90;
            int prodP2 = 190;
            int prodP3 = 160;

            /*
            * Errechnung des Produktionsbedarfs nach Produkt
            * Formel Excel - =$E$4*'Eingabe Aufträge'!Z$8 - Rechnung
            * Zugriff auf DB-Tabelle "Verwendung"
             * 
            * */
            string databasename = @"Provider=Microsoft.ACE.OLEDB.12.0; Data Source=IBSYS_DB.accdb";
            myconn = new OleDbConnection(databasename);
            MessageBox.Show("Neue Form");

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
            cmd.CommandText = @"select Periode from Lager";
            OleDbDataReader dbReader = cmd.ExecuteReader();
            while (dbReader.Read())
            {
                //PeriodeDB = Convert.ToInt32(dbReader["Periode"]);
                //MessageBox.Show("dbReader " + dbReader["K_Teil"]);
            }
            myconn.Close();

           
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Kaufteildisposition_Load(object sender, EventArgs e)
        {
            

        }
    }
}
