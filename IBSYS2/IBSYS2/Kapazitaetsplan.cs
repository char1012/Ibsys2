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
            System.Windows.Forms.ToolTip ToolTip1 = new System.Windows.Forms.ToolTip();
            ToolTip1.SetToolTip(this.pictureBox7, "Der berechnete Kapazitätsbedarf ist nicht änderbar." +
                "\nSie können jedoch für jeden Arbeitsplatz die Überstunden pro Periode, " + 
                "die Überstunden pro Tag und die Anzahl der Schichten anpassen." + 
                "\nEine Änderung bei Überstunden/Periode bewirkt eine Neuberechnung von Überstunden/Tag " +
                "und umgekehrt.\nWenn in der Zeile Schichten eine rote 3 angezeigt wird, " + 
                "bedeutet dies, dass mehr als drei Schichten benötigt werden. In diesem Fall sollten Sie " + 
                "ihre Produktionsmenge anpassen.\nDen Arbeitsplatz 5 gibt es nicht.");

            // Dieser Konstruktor soll in Zukunft von Produktion.cs mit den Parametern
            // int periode und eines zweidimensionales int-Array (Teilenummer, Produktionsmenge) aufgerufen.
            // Diese Werte werden momentan simuliert.
            int periode = 7;
            int[,] teile = new int [30,2];
            teile[0,0] = 1;
            teile[0,1] = 90; // Teil p1 mit 90 Stueck Produktion
            teile[1,0] = 2;
            teile[1,1] = 190;
            teile[2,0] = 3;
            teile[2,1] = 160;
            teile[3,0] = 4;
            teile[3,1] = 60;
            teile[4,0] = 5;
            teile[4,1] = 160;
            teile[5,0] = 6;
            teile[5,1] = -110;
            teile[6,0] = 7;
            teile[6,1] = 50;
            teile[7,0] = 8;
            teile[7,1] = 150;
            teile[8,0] = 9;
            teile[8,1] = -200;
            teile[9,0] = 10;
            teile[9,1] = 60;
            teile[10,0] = 11;
            teile[10,1] = 160;
            teile[11,0] = 12;
            teile[11,1] = -110;
            teile[12,0] = 13;
            teile[12,1] = 50;
            teile[13,0] = 14;
            teile[13,1] = 150;
            teile[14,0] = 15;
            teile[14,1] = -200;
            teile[15,0] = 16;
            teile[15,1] = 20+130+90;
            teile[16,0] = 17;
            teile[16,1] = 20+130+90;
            teile[17,0] = 18;
            teile[17,1] = 50;
            teile[18,0] = 19;
            teile[18,1] = 150;
            teile[19,0] = 20;
            teile[19,1] = -200;
            teile[20,0] = 26;
            teile[20,1] = 50+160+130;
            teile[21,0] = 29;
            teile[21,1] = -110;
            teile[22,0] = 30;
            teile[22,1] = -20;
            teile[23,0] = 31;
            teile[23,1] = 70;
            teile[24,0] = 49;
            teile[24,1] = 60;
            teile[25,0] = 50;
            teile[25,1] = 70;
            teile[26,0] = 51;
            teile[26,1] = 80;
            teile[27,0] = 54;
            teile[27,1] = 160;
            teile[28,0] = 55;
            teile[28,1] = 170;
            teile[29,0] = 56;
            teile[29,1] = 180;

            string databasename = @"Provider=Microsoft.ACE.OLEDB.12.0; Data Source=IBSYS_DB.accdb";
            myconn = new OleDbConnection(databasename);
            OleDbCommand cmd = new OleDbCommand();
            cmd.CommandType = CommandType.Text;
            cmd.Connection = myconn;
            try
            {
                myconn.Open();
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("DB-Verbindung wurde nicht ordnungsgemäß geschlossen bei der letzten Verwendung, Verbindung wird neu gestartet, bitte haben Sie einen Moment Geduld.");
                myconn.Close();
                myconn.Open();
            }

            // Berechnung des Kapazitaetsbedarfs
            // 1. Bearbeitungszeit + Ruestzeit
            int[] plaetze = new int[15];
            int bearbeitungszeit = 0;
            int ruestzeit = 0;
            for (int i = 0; i < plaetze.Length; ++i)
            {
                // Fuer jeden Arbeitsplatz die Zeilen raussuchen, die ihn betreffen
                int platznr = i + 1;
                cmd.CommandText = @"SELECT Erzeugnis_Teilenummer_FK, Bearbeitungszeit, Rüstzeit FROM Arbeitsplatz_Erzeugnis WHERE Arbeitsplatz_FK = " + platznr + ";";
                OleDbDataReader dbReader = cmd.ExecuteReader();
                while (dbReader.Read())
                {
                    // Fuer jede dieser Zeilen, die Liste mit den Produktionsmengen durchlaufen ...
                    for (int no = 0; no < teile.GetLength(0); ++no)
                    {
                        // ... und pruefen, ob es sich um das Teil aus der DB-Zeile handelt
                        if (teile[no, 0] == Convert.ToInt32(dbReader["Erzeugnis_Teilenummer_FK"]))
                        {
                            // Wenn ja, die Bearbeitungszeit fuer dieses Teil auf diesem Platz berechnen ...
                            int zeit = Convert.ToInt32(dbReader["Bearbeitungszeit"]) * teile[no, 1];
                            // ... und wenn die Zeit nicht 0 ist, die bearbeitungszeit und Ruestzeit um diesen Wert erhoehen
                            if (zeit > 0)
                            {
                                bearbeitungszeit = bearbeitungszeit + zeit;
                                ruestzeit = ruestzeit + Convert.ToInt32(dbReader["Rüstzeit"]);
                            }
                        }
                    }
                }
                plaetze[i] = bearbeitungszeit + ruestzeit;
                bearbeitungszeit = 0;
                ruestzeit = 0;
                dbReader.Close();
            }
            // 2. Rueckstand Bearbeitungszeit + Ruestzeit

            myconn.Close();

            // Zeile Kapazitaetsbedarf fuellen
            for (int i = 0; i < plaetze.Length; ++i)
            {
                int k = i + 1;
                this.Controls.Find("K" + k.ToString(), true)[0].Text = plaetze[i].ToString();
            }

            // Zeile Ueberstunden/Periode fuellen -> Kalkulation der Ueberstd auf Grundlage des Kap.bedarfs
            for (int i = 0; i < plaetze.Length; ++i)
            {
                int up = i + 1;
                TextBox kText = (TextBox) this.Controls.Find("K" + up.ToString(), true)[0];
                int ueberstd = 0;
                if (Convert.ToInt32(kText.Text) > 2400 && Convert.ToInt32(kText.Text) <= 3600)
                {
                    int zuviel = Convert.ToInt32(kText.Text) - 2400; // Stunden, die mehr als 2400 sind
                    ueberstd = zuviel + zuviel/5; // plus 1/5 mehr zur Sicherheit
                }
                else if (Convert.ToInt32(kText.Text) > 2300 && Convert.ToInt32(kText.Text) <= 2400)
                {
                    ueberstd = Convert.ToInt32(kText.Text) - 2300;
                }
                else if (Convert.ToInt32(kText.Text) > 4800 && Convert.ToInt32(kText.Text) <= 6000)
                {
                    int zuviel = Convert.ToInt32(kText.Text) - 4800; // Stunden, die mehr als 4800 sind
                    ueberstd = zuviel + zuviel/5; // plus 1/5 mehr zur Sicherheit
                }
                else if (Convert.ToInt32(kText.Text) > 4700 && Convert.ToInt32(kText.Text) <= 4800)
                {
                    ueberstd = Convert.ToInt32(kText.Text) - 4700;
                }
                if (ueberstd > 1200)
                {
                    ueberstd = 1200;
                }
                this.Controls.Find("UP" + up.ToString(), true)[0].Text = ueberstd.ToString();
            }

            // Zeile Ueberstunden/Tag fuellen -> 1/5 von Ueberstunden/Periode
            for (int i = 0; i < plaetze.Length; ++i)
            {
                int ut = i + 1;
                TextBox upText = (TextBox)this.Controls.Find("UP" + ut.ToString(), true)[0];
                int ueberstd = (int) Math.Round(Convert.ToDouble(upText.Text) / 5);
                this.Controls.Find("UT" + ut.ToString(), true)[0].Text = ueberstd.ToString();
            }

            // Zeile Schichten fuellen
            for (int i = 0; i < plaetze.Length; ++i)
            {
                int s = i + 1;
                TextBox kText = (TextBox)this.Controls.Find("K" + s.ToString(), true)[0];
                int schicht = 1;
                if (Convert.ToInt32(kText.Text) <= 3600)
                {
                    schicht = 1;
                }
                else if (Convert.ToInt32(kText.Text) > 3600 && Convert.ToInt32(kText.Text) <= 6000)
                {
                    schicht = 2;
                }
                else if (Convert.ToInt32(kText.Text) > 6000 && Convert.ToInt32(kText.Text) <= 7200)
                {
                    schicht = 3;
                }
                else if (Convert.ToInt32(kText.Text) > 7200) // Wenn mehr als 3 Schichten benoetigt werden
                {
                    schicht = 3;
                    this.Controls.Find("S" + s.ToString(), true)[0].ForeColor = Color.Red;
                }
                this.Controls.Find("S" + s.ToString(), true)[0].Text = schicht.ToString();
            }
        }

        private void A1_Click(object sender, EventArgs e)
        {

        }
    }
}
