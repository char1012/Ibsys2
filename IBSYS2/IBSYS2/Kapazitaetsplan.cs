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

namespace IBSYS2
{
    public partial class Kapazitaetsplan : Form
    {
        private OleDbConnection myconn;

        public Kapazitaetsplan()
        {
            InitializeComponent();
            continue_btn.Enabled = true; // false, wenn Zellen geleert werden
            string databasename = @"Provider=Microsoft.ACE.OLEDB.12.0; Data Source=IBSYS_DB.accdb";
            myconn = new OleDbConnection(databasename);
            System.Windows.Forms.ToolTip ToolTip1 = new System.Windows.Forms.ToolTip();
            ToolTip1.SetToolTip(this.pictureBox7, "Der berechnete Kapazitätsbedarf ist nicht änderbar." +
                "\nSie können jedoch für jeden Arbeitsplatz die Überstunden pro Periode, " + 
                "die Überstunden pro Tag und die Anzahl der Schichten anpassen." + 
                "\nEine Änderung bei Überstunden/Periode bewirkt eine Neuberechnung von Überstunden/Tag " +
                "und umgekehrt. \nDen Arbeitsplatz 5 gibt es nicht.");

            // Dieser Konstruktor soll in Zukunft von Produktion.cs mit den Parametern
            // p1_per1, p1_per2, p1_per3, p1_per4, p2_per1, p2_per2, p2_per3, p2_per4,
            // p3_per1, p3_per2, p3_per3, p3_per4 (alles Integer) aufgerufen werden.
            // Diese Werte werden momentan simuliert.
        }

        private void A1_Click(object sender, EventArgs e)
        {

        }
    }
}
