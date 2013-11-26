﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Xml;


namespace IBSYS2
{
    public partial class Sicherheitsbestand : Form
    {
        private OleDbConnection myconn;

        public Sicherheitsbestand()
        {
            InitializeComponent();
            continue_btn.Enabled = true;
            string databasename = @"Provider=Microsoft.ACE.OLEDB.12.0; Data Source=IBSYS_DB.accdb";
            myconn = new OleDbConnection(databasename);

            System.Windows.Forms.ToolTip ToolTipP = new System.Windows.Forms.ToolTip();
            ToolTipP.SetToolTip(this.infoP, "Bitte den Sicherheitsbestand eingeben, welcher für die P-Teile gehalten werden soll.");
            System.Windows.Forms.ToolTip ToolTipE = new System.Windows.Forms.ToolTip();
            ToolTipE.SetToolTip(this.infoE, "- Diese Felder der Sicherheitsbestände für die E-Teile ist vor Berechnung der P-Teile nicht pflegbar. \n" + "- Das Ergbenis der Sicherheitsbestände der E-Teile wird vom System berechnet, können aber nach Bedarf händisch nachgefplegt werden. \n" + "- Um fortzufahren auf 'Fortfahren' klicken.");

            E261.Enabled = false;
            E511.Enabled = false;
            E161.Enabled = false;
            E171.Enabled = false;
            E501.Enabled = false;
            E041.Enabled = false;
            E101.Enabled = false;
            E491.Enabled = false;
            E071.Enabled = false;
            E131.Enabled = false;
            E181.Enabled = false;
            E262.Enabled = false;
            E562.Enabled = false;
            E162.Enabled = false;
            E172.Enabled = false;
            E552.Enabled = false;
            E052.Enabled = false;
            E112.Enabled = false;
            E542.Enabled = false;
            E082.Enabled = false;
            E142.Enabled = false;
            E192.Enabled = false;
            E263.Enabled = false;
            E313.Enabled = false;
            E163.Enabled = false;
            E173.Enabled = false;
            E303.Enabled = false;
            E063.Enabled = false;
            E123.Enabled = false;
            E293.Enabled = false;
            E093.Enabled = false;
            E153.Enabled = false;
            E203.Enabled = false;

            Ausgabe_P1.Enabled = false;
            Ausgabe_P2.Enabled = false;
            Ausgabe_P3.Enabled = false;
        }

        private void continue_btn_Click_1(object sender, EventArgs e)
        {
            //Auslesen der TextFelder
            //TODO Validierung
            int gLagerbestandP1 = Convert.ToInt32(Eingabe_P1.Text);
            int gLagerbestandP2 = Convert.ToInt32(Eingabe_P2.Text);
            int gLagerbestandP3 = Convert.ToInt32(Eingabe_P3.Text);

            int sicherheitsbestandP1 = sicherheitsbestandBerechnen(gLagerbestandP1, "1");
            Ausgabe_P1.Text = Convert.ToString(sicherheitsbestandP1);
            int sicherheitsbestandP2 = sicherheitsbestandBerechnen(gLagerbestandP2, "2");
            Ausgabe_P2.Text = Convert.ToString(sicherheitsbestandP2);
            int sicherheitsbestandP3 = sicherheitsbestandBerechnen(gLagerbestandP3, "3");
            Ausgabe_P3.Text = Convert.ToString(sicherheitsbestandP3);

            int gLE26P1 = geplanterLagerbestand(sicherheitsbestandP1, 70);
            E261.Text = Convert.ToString(gLE26P1);
            int gLE51P1 = geplanterLagerbestand(sicherheitsbestandP1, 70);
            E511.Text = Convert.ToString(gLE51P1);
            int gLE16P1 = geplanterLagerbestand(sicherheitsbestandP1, 70);
            E161.Text = Convert.ToString(gLE16P1);
            int gLE17P1 = geplanterLagerbestand(sicherheitsbestandP1, 70);
            E171.Text = Convert.ToString(gLE17P1);
            int gLE50P1 = geplanterLagerbestand(sicherheitsbestandP1, 70);
            E501.Text = Convert.ToString(gLE50P1);
            int gLE4P1 = geplanterLagerbestand(sicherheitsbestandP1, 70);
            E041.Text = Convert.ToString(gLE4P1);
            int gLE10P1 = geplanterLagerbestand(sicherheitsbestandP1, 70);
            E101.Text = Convert.ToString(gLE10P1);
            int gLE49P1 = geplanterLagerbestand(sicherheitsbestandP1, 70);
            E491.Text = Convert.ToString(gLE49P1);
            int gLE7P1 = geplanterLagerbestand(sicherheitsbestandP1, 70);
            E071.Text = Convert.ToString(gLE7P1);
            int gLE13P1 = geplanterLagerbestand(sicherheitsbestandP1, 70);
            E131.Text = Convert.ToString(gLE13P1);
            int gLE18P1 = geplanterLagerbestand(sicherheitsbestandP1, 70);
            E181.Text = Convert.ToString(gLE18P1);

            int gLE26P2 = geplanterLagerbestand(sicherheitsbestandP2, 70);
            E262.Text = Convert.ToString(gLE26P2);
            int gLE56P2 = geplanterLagerbestand(sicherheitsbestandP2, 70);
            E562.Text = Convert.ToString(gLE56P2);
            int gLE16P2 = geplanterLagerbestand(sicherheitsbestandP2, 70);
            E162.Text = Convert.ToString(gLE16P2);
            int gLE17P2 = geplanterLagerbestand(sicherheitsbestandP2, 70);
            E172.Text = Convert.ToString(gLE17P2);
            int gLE55P2 = geplanterLagerbestand(sicherheitsbestandP2, 70);
            E552.Text = Convert.ToString(gLE55P2);
            int gLE5P2 = geplanterLagerbestand(sicherheitsbestandP2, 70);
            E052.Text = Convert.ToString(gLE5P2);
            int gLE11P2 = geplanterLagerbestand(sicherheitsbestandP2, 70);
            E112.Text = Convert.ToString(gLE11P2);
            int gLE54P2 = geplanterLagerbestand(sicherheitsbestandP2, 70);
            E542.Text = Convert.ToString(gLE54P2);
            int gLE8P2 = geplanterLagerbestand(sicherheitsbestandP2, 70);
            E082.Text = Convert.ToString(gLE8P2);
            int gLE14P2 = geplanterLagerbestand(sicherheitsbestandP2, 70);
            E142.Text = Convert.ToString(gLE14P2);
            int gLE19P2 = geplanterLagerbestand(sicherheitsbestandP2, 70);
            E192.Text = Convert.ToString(gLE19P2);

            int gLE26P3 = geplanterLagerbestand(sicherheitsbestandP3, 70);
            E263.Text = Convert.ToString(gLE26P3);
            int gLE31P3 = geplanterLagerbestand(sicherheitsbestandP3, 70);
            E313.Text = Convert.ToString(gLE31P3);
            int gLE16P3 = geplanterLagerbestand(sicherheitsbestandP3, 70);
            E163.Text = Convert.ToString(gLE16P3);
            int gLE17P3 = geplanterLagerbestand(sicherheitsbestandP3, 70);
            E173.Text = Convert.ToString(gLE17P3);
            int gLE30P3 = geplanterLagerbestand(sicherheitsbestandP3, 70);
            E303.Text = Convert.ToString(gLE30P3);
            int gLE6P3 = geplanterLagerbestand(sicherheitsbestandP3, 70);
            E063.Text = Convert.ToString(gLE6P3);
            int gLE12P3 = geplanterLagerbestand(sicherheitsbestandP3, 70);
            E123.Text = Convert.ToString(gLE12P3);
            int gLE29P3 = geplanterLagerbestand(sicherheitsbestandP3, 70);
            E293.Text = Convert.ToString(gLE29P3);
            int gLE9P3 = geplanterLagerbestand(sicherheitsbestandP3, 70);
            E093.Text = Convert.ToString(gLE9P3);
            int gLE15P3 = geplanterLagerbestand(sicherheitsbestandP3, 70);
            E153.Text = Convert.ToString(gLE15P3);
            int gLE20P3 = geplanterLagerbestand(sicherheitsbestandP3, 70);
            E203.Text = Convert.ToString(gLE20P3);
            textfeldFreigeben();
        }

        public int geplanterLagerbestand(int sicherheitsbestand, int ver)
        {
            int geplanterLagerbestand = 0;
            geplanterLagerbestand = (sicherheitsbestand / 100) * ver;
            return geplanterLagerbestand;
        }

        public int sicherheitsbestandBerechnen(int gLagerbestand, string teilenummer_FK)
        {
            int prognose = 100;
            int sicherheitsbestand = 0;
            int lBestand = datenHolen(teilenummer_FK, "Bestand", "Teilenummer_FK", "Lager");
            int wMatMenge = datenHolen(teilenummer_FK, "Menge", "Fehlteil_Teilenummer_FK", "Warteliste_Material");
            int wArbMenge = datenHolen(teilenummer_FK, "Menge", "Teilenummer_FK", "Warteliste_Arbeitsplatz");
            return sicherheitsbestand = prognose + gLagerbestand - lBestand - wMatMenge - wArbMenge;
        }

        public int datenHolen(string teilenummer_FK, string spalte, string spalte1, string tabelle)
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
            string tmp = cmd.CommandText = @"SELECT * FROM " + tabelle + " WHERE " + spalte1 + " = " + teilenummer_FK;
            if (tmp == null)
            {
                int datuminttemp = 0;
                return datuminttemp;
            }
            cmd.CommandText = @"SELECT * FROM " + tabelle + " WHERE " + spalte1 + " = " + teilenummer_FK;
            OleDbDataReader dr = cmd.ExecuteReader();
            string datum = string.Empty;
            while (dr.Read())
            {
                datum = dr[spalte].ToString();
            }
            int datumint = 0;
            if (datum != null)
            {
                if (datum != "")
                {
                    datumint = Convert.ToInt32(datum.ToString());
                }
            }
            dr.Close();
            myconn.Close();
            return datumint;
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }
        private void Eingabe_P1_TextChanged(object sender, EventArgs e)
        {
        }
        private void Eingabe_P2_TextChanged(object sender, EventArgs e)
        {
        }
        private void Eingabe_P3_TextChanged(object sender, EventArgs e)
        {
        }

        private void groupBox1_Enter(object sender, System.EventArgs e)
        {
        }

        private void Sicherheitsbestand_Load(object sender, System.EventArgs e)
        {
        }

        private void label3_Click(object sender, System.EventArgs e)
        {
        }

        private void groupBox2_Enter(object sender, System.EventArgs e)
        {
        }

        private void textBox1_TextChanged(object sender, System.EventArgs e)
        {
        }

        private void textBox5_TextChanged(object sender, System.EventArgs e)
        {
        }

        private void label11_Click(object sender, System.EventArgs e)
        {
        }

        private void textBox3_TextChanged(object sender, System.EventArgs e)
        {
        }
                
        public void textfeldFreigeben(){
            E261.Enabled = true;
            E511.Enabled = true;
            E161.Enabled = true;
            E171.Enabled = true;
            E501.Enabled = true;
            E041.Enabled = true;
            E101.Enabled = true;
            E491.Enabled = true;
            E071.Enabled = true;
            E131.Enabled = true;
            E181.Enabled = true;
            E262.Enabled = true;
            E562.Enabled = true;
            E162.Enabled = true;
            E172.Enabled = true;
            E552.Enabled = true;
            E052.Enabled = true;
            E112.Enabled = true;
            E542.Enabled = true;
            E082.Enabled = true;
            E142.Enabled = true;
            E192.Enabled = true;
            E263.Enabled = true;
            E313.Enabled = true;
            E163.Enabled = true;
            E173.Enabled = true;
            E303.Enabled = true;
            E063.Enabled = true;
            E123.Enabled = true;
            E293.Enabled = true;
            E093.Enabled = true;
            E153.Enabled = true;
            E203.Enabled = true;
        }
    }
}
