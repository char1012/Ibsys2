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
        // Liste der zulaessigen Zeichen bei Benutzereingaben
        private char[] digits = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
        // Listen zum Speichern der Startwerte (Aenderungen nachvollziehen + Feststellen, ob initiale Belegung)
        private int[] schichten;

        public Kapazitaetsplan()
        {
            InitializeComponent();
            continue_btn.Enabled = true; // false, wenn Zellen geleert werden
            System.Windows.Forms.ToolTip ToolTip1 = new System.Windows.Forms.ToolTip();
            ToolTip1.SetToolTip(this.pictureBox7, "- Der berechnete Kapazitätsbedarf ist nicht änderbar. " +
                "Sie können jedoch für jeden Arbeitsplatz die Überstunden pro Periode " + 
                "\n   und die Anzahl der Schichten anpassen. " + 
                "Eine Änderung bei Überstunden/Periode bewirkt eine Neuberechnung von Überstunden/Tag. " +
                "\n- Wenn in der Zeile Schichten eine rote 3 angezeigt wird, " + 
                "bedeutet dies, dass mehr als drei Schichten benötigt werden.\n   In diesem Fall sollten Sie " + 
                "ihre Produktionsmengen anpassen.\n- Den Arbeitsplatz 5 gibt es nicht.");
        }

        public void setValues()
        {
            // Diese Methode wird in Zukunft von Produktion.cs mit den Parametern
            // int periode und eines zweidimensionales int-Array (Teilenummer, Produktionsmenge) aufgerufen.
            // Diese Werte werden momentan simuliert.
            int periode = 6; // Periode des xmls
            int[,] teile = new int[30, 2];
            teile[0, 0] = 1;
            teile[0, 1] = 90; // Teil p1 mit 90 Stueck Produktion
            teile[1, 0] = 2;
            teile[1, 1] = 190;
            teile[2, 0] = 3;
            teile[2, 1] = 160;
            teile[3, 0] = 4;
            teile[3, 1] = 60;
            teile[4, 0] = 5;
            teile[4, 1] = 160;
            teile[5, 0] = 6;
            teile[5, 1] = -110;
            teile[6, 0] = 7;
            teile[6, 1] = 50;
            teile[7, 0] = 8;
            teile[7, 1] = 150;
            teile[8, 0] = 9;
            teile[8, 1] = -200;
            teile[9, 0] = 10;
            teile[9, 1] = 60;
            teile[10, 0] = 11;
            teile[10, 1] = 160;
            teile[11, 0] = 12;
            teile[11, 1] = -110;
            teile[12, 0] = 13;
            teile[12, 1] = 50;
            teile[13, 0] = 14;
            teile[13, 1] = 150;
            teile[14, 0] = 15;
            teile[14, 1] = -200;
            teile[15, 0] = 16;
            teile[15, 1] = 20 + 130 + 90;
            teile[16, 0] = 17;
            teile[16, 1] = 20 + 130 + 90;
            teile[17, 0] = 18;
            teile[17, 1] = 50;
            teile[18, 0] = 19;
            teile[18, 1] = 150;
            teile[19, 0] = 20;
            teile[19, 1] = -200;
            teile[20, 0] = 26;
            teile[20, 1] = 50 + 160 + 130;
            teile[21, 0] = 29;
            teile[21, 1] = -110;
            teile[22, 0] = 30;
            teile[22, 1] = -20;
            teile[23, 0] = 31;
            teile[23, 1] = 70;
            teile[24, 0] = 49;
            teile[24, 1] = 60;
            teile[25, 0] = 50;
            teile[25, 1] = 70;
            teile[26, 0] = 51;
            teile[26, 1] = 80;
            teile[27, 0] = 54;
            teile[27, 1] = 160;
            teile[28, 0] = 55;
            teile[28, 1] = 170;
            teile[29, 0] = 56;
            teile[29, 1] = 180;

            // Methode zur Berechnung der Werte aufrufen
            int[] plaetze = calculate(periode, teile);

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
                TextBox kText = (TextBox)this.Controls.Find("K" + up.ToString(), true)[0];
                int ueberstd = 0;
                if (Convert.ToInt32(kText.Text) > 2400 && Convert.ToInt32(kText.Text) <= 3600)
                {
                    int zuviel = Convert.ToInt32(kText.Text) - 2400; // Stunden, die mehr als 2400 sind
                    ueberstd = zuviel + zuviel / 5; // plus 1/5 mehr zur Sicherheit
                }
                else if (Convert.ToInt32(kText.Text) > 2300 && Convert.ToInt32(kText.Text) <= 2400)
                {
                    ueberstd = Convert.ToInt32(kText.Text) - 2300;
                }
                else if (Convert.ToInt32(kText.Text) > 4800 && Convert.ToInt32(kText.Text) <= 6000)
                {
                    int zuviel = Convert.ToInt32(kText.Text) - 4800; // Stunden, die mehr als 4800 sind
                    ueberstd = zuviel + zuviel / 5; // plus 1/5 mehr zur Sicherheit
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
                int ueberstd = (int)Math.Round(Convert.ToDouble(upText.Text) / 5);
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
                schichten[i] = schicht; // Startwert der Zeile Schichten speichern
            }

        }

        public int[] calculate(int periode, int[,] teile)
        {
            // Die Werte in schichten sollen wieder auf 0 gesetzt werden
            schichten = new int[15];

            string databasename = @"Provider=Microsoft.ACE.OLEDB.12.0; Data Source=IBSYS_DB.accdb";
            myconn = new OleDbConnection(databasename);
            OleDbCommand cmd = new OleDbCommand();
            cmd.CommandType = CommandType.Text;
            cmd.Connection = myconn;
            OleDbCommand cmd2 = new OleDbCommand();
            cmd2.CommandType = CommandType.Text;
            cmd2.Connection = myconn;
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

            int[] plaetze = new int[15];
            for (int i = 0; i < plaetze.Length; ++i)
            {
                int platznr = i + 1;
                int bearbeitungszeit = 0;
                int ruestzeit = 0;
                int rueckstandBearbeitungszeit = 0;
                int rueckstandRuestzeit = 0;

                // 0. um weniger DB-Abfragen zu machen (vorher 11), werden hier häufig ben. Infos gespeichert
                // a) Infos aus Arbeitsplatz_Erzeugnis zum aktuellen Arbeitsplatz
                //    = alle Zeilen, in denen dieser Arbeitsplatz vorkommt
                int a = 0;
                List<List<int>> arbeitsplatz_erzeugnis = new List<List<int>>();
                // in diesem Fall eine Liste, weil nicht sicher ist, wie viele Zeilen es gibt
                cmd.CommandText = @"SELECT Erzeugnis_Teilenummer_FK, Bearbeitungszeit, Rüstzeit, Reihenfolge FROM Arbeitsplatz_Erzeugnis WHERE Arbeitsplatz_FK = " + platznr + ";";
                OleDbDataReader dbReader = cmd.ExecuteReader();
                while (dbReader.Read())
                {
                    arbeitsplatz_erzeugnis.Add(new List<int>());
                    arbeitsplatz_erzeugnis[a].Add(Convert.ToInt32(dbReader["Erzeugnis_Teilenummer_FK"]));
                    arbeitsplatz_erzeugnis[a].Add(Convert.ToInt32(dbReader["Bearbeitungszeit"]));
                    arbeitsplatz_erzeugnis[a].Add(Convert.ToInt32(dbReader["Rüstzeit"]));
                    arbeitsplatz_erzeugnis[a].Add(Convert.ToInt32(dbReader["Reihenfolge"]));
                    ++a;
                }
                dbReader.Close();

                // 1. Bearbeitungszeit + Ruestzeit

                for (int n = 0; n < arbeitsplatz_erzeugnis.Count; ++n)
                {
                    // Fuer jede dieser Zeilen, die Liste mit den Produktionsmengen durchlaufen ...
                    for (int no = 0; no < teile.GetLength(0); ++no)
                    {
                        // ... und pruefen, ob es sich um das Teil aus der DB-Zeile handelt und
                        // die entsprechende Produktionsmenge nicht 0 ist
                        if (teile[no, 0] == arbeitsplatz_erzeugnis[n][0] && teile[no, 1] > 0)
                        {
                            bearbeitungszeit += arbeitsplatz_erzeugnis[n][1] * teile[no, 1];
                            ruestzeit += arbeitsplatz_erzeugnis[n][2];
                        }
                    }
                }

                // 2. Rueckstand Bearbeitungszeit + Ruestzeit

                // ueberpruefen, ob es vorgelagerte Arbeitsplaetze gibt (erstmal unabhaengig vom Teil)
                bool vorgelagert = false;
                for (int n = 0; n < arbeitsplatz_erzeugnis.Count; ++n)
                {
                    if (arbeitsplatz_erzeugnis[n][3] != 1)
                    {
                        vorgelagert = true;
                        break;
                    }
                }

                // In jedem Fall die Tabellen Warteliste und Bearbeitung auf Eintraege fuer diesen Platz ueberpruefen

                // 2.1. Warteliste Arbeitsplatz ueberpruefen -> Bearbeitungszeit und Ruestzeit
                cmd.CommandText = @"SELECT Teilenummer_FK, Zeitbedarf FROM Warteliste_Arbeitsplatz WHERE Arbeitsplatz_FK = " + platznr
                    + " AND Periode = " + periode + ";";
                dbReader = cmd.ExecuteReader();
                while (dbReader.Read())
                {
                    int teilenummer = Convert.ToInt32(dbReader["Teilenummer_FK"]);
                    // Bearbeitungszeit
                    rueckstandBearbeitungszeit += Convert.ToInt32(dbReader["Zeitbedarf"]);
                    // Ruestzeit
                    for (int n = 0; n < arbeitsplatz_erzeugnis.Count; ++n)
                    {
                        if (arbeitsplatz_erzeugnis[n][0] == teilenummer)
                        {
                            rueckstandRuestzeit += arbeitsplatz_erzeugnis[n][2];
                        }
                    }
                }
                dbReader.Close();

                // 2.2. In Bearbeitung ueberpruefen -> nur Bearbeitungszeit, keine Rüstzeit
                cmd.CommandText = @"SELECT Zeitbedarf FROM Bearbeitung WHERE Arbeitsplatz_FK = " + platznr
                    + " AND Periode = " + periode + ";";
                dbReader = cmd.ExecuteReader();
                while (dbReader.Read())
                {
                    rueckstandBearbeitungszeit += Convert.ToInt32(dbReader["Zeitbedarf"]);
                }
                dbReader.Close();

                // 2.3. Warteliste Material ueberpruefen
                cmd.CommandText = @"SELECT Erzeugnis_Teilenummer_FK, Menge FROM Warteliste_Material WHERE Periode = " + periode + ";";
                dbReader = cmd.ExecuteReader();
                while (dbReader.Read())
                {
                    int materialWarteliste = Convert.ToInt32(dbReader["Erzeugnis_Teilenummer_FK"]);
                    int menge = Convert.ToInt32(dbReader["Menge"]);
                    // Ich gehe an dieser Stelle davon aus, dass die Produktion von materialWarteliste
                    // noch gar nicht angestossen wurde und es noch durch alle Plaetze durch muss.
                    // -> alle Plaetze heraussuchen und kontrollieren, ob der aktuelle dazu gehoert
                    cmd2.CommandText = @"SELECT Arbeitsplatz_FK, Bearbeitungszeit, Rüstzeit FROM Arbeitsplatz_Erzeugnis WHERE Erzeugnis_Teilenummer_FK = " + materialWarteliste + ";";
                    OleDbDataReader dbReader2 = cmd2.ExecuteReader();
                    while (dbReader2.Read())
                    {
                        if (platznr == Convert.ToInt32(dbReader2["Arbeitsplatz_FK"]))
                        {
                            rueckstandBearbeitungszeit += Convert.ToInt32(dbReader2["Bearbeitungszeit"]) * menge;
                            rueckstandRuestzeit += Convert.ToInt32(dbReader2["Rüstzeit"]);
                        }
                    }
                    dbReader2.Close();
                }
                dbReader.Close();

                // 2.4. wenn vorgelagert == true, muess kontrolliert werden, ob fuer die vorgelagerten Plaetze
                // Eintraege in den Tabellen Warteliste und Bearbeitung stehen
                if (vorgelagert == true)
                {
                    // herausfinden, bei welchen Teilen es vorgelagerte Plaetze gibt
                    for (int n = 0; n < arbeitsplatz_erzeugnis.Count; ++n)
                    {
                        if (arbeitsplatz_erzeugnis[n][3] != 1)
                        {
                            int teilenummer = arbeitsplatz_erzeugnis[n][0];
                            int bzeit = arbeitsplatz_erzeugnis[n][1];
                            int rzeit = arbeitsplatz_erzeugnis[n][2];
                            int reihenfolge = arbeitsplatz_erzeugnis[n][3];
                            // herausfinden, welche Plaetze dies sind (exkl. dem aktuellen Platz)
                            cmd.CommandText = @"SELECT Arbeitsplatz_FK FROM Arbeitsplatz_Erzeugnis WHERE Erzeugnis_Teilenummer_FK = " + teilenummer
                                + " AND Arbeitsplatz_FK <> " + platznr + " AND Reihenfolge < " + reihenfolge + ";";
                            dbReader = cmd.ExecuteReader();
                            while (dbReader.Read())
                            {
                                int andererPlatz = Convert.ToInt32(dbReader["Arbeitsplatz_FK"]);
                                // kontrollieren, ob diese in der Warteliste Arbeitsplatz liegen
                                // Menge wird benoetigt, um dies dann mit den eigenen Werten fuer B.zeit und R.zeit zu verrechnen
                                cmd2.CommandText = @"SELECT Menge FROM Warteliste_Arbeitsplatz WHERE Teilenummer_FK = " + teilenummer
                                    + " AND Arbeitsplatz_FK = " + andererPlatz + " AND periode = " + periode + ";";
                                OleDbDataReader dbReader2 = cmd2.ExecuteReader();
                                while (dbReader2.Read()) // hier sollte nur eine Zeile herauskommen
                                {
                                    rueckstandBearbeitungszeit += bzeit * Convert.ToInt32(dbReader2["Menge"]);
                                    rueckstandRuestzeit += rzeit;
                                }
                                dbReader2.Close();
                                // kontrollieren, ob diese in Bearbeitung liegen
                                cmd2.CommandText = @"SELECT Menge FROM Bearbeitung WHERE Teilenummer_FK = " + teilenummer
                                    + " AND Arbeitsplatz_FK = " + andererPlatz + " AND periode = " + periode + ";";
                                dbReader2 = cmd2.ExecuteReader();
                                while (dbReader2.Read())
                                {
                                    rueckstandBearbeitungszeit += bzeit * Convert.ToInt32(dbReader2["Menge"]);
                                    rueckstandRuestzeit += rzeit;
                                }
                                dbReader2.Close();
                            }
                            dbReader.Close();
                        }
                    }
                }
                plaetze[i] = bearbeitungszeit + ruestzeit + rueckstandBearbeitungszeit + rueckstandRuestzeit;
            }
            myconn.Close();
            return plaetze;
        }

        private void default_btn_Click(object sender, EventArgs e)
        {
            setValues();
        }

        private void continue_btn_Click(object sender, EventArgs e)
        {
            // Pruefung gestrichen, weil es dem Benutzer nicht moeglich ist,
            // mit falschen Werten diese Methode aufzurufen
            MessageBox.Show("Hier ginge es eigentlich weiter zur Kaufteildisposition.");
        }

        private void UP1_TextChanged(object sender, EventArgs e)
        {
            if (UP1.Text == "")
            {
                UT1.Text = "";
                continue_btn.Enabled = false;
            }
            else
            {
                bool okay = true;
                // neuer Text darf nur Zeichen aus der Liste digits (in der Klasse deklariert)
                foreach (char c in UP1.Text.ToCharArray())
                {
                    // sobald es ein unpassendes Zeichen gibt, aufhoeren und Fehlermeldung ausgeben
                    if (!digits.Contains<char>(c))
                    {
                        okay = false;
                        break;
                    }
                }
                if (okay == true && Convert.ToInt32(UP1.Text) <= 1200)
                {
                    int zeit = (int)Math.Round(Convert.ToDouble(UP1.Text) / 5);
                    UT1.Text = zeit.ToString();
                    continue_btn.Enabled = true;
                }
                else
                {
                    MessageBox.Show("Es sind nur Werte von 0 bis 1200 zulässig.");
                    continue_btn.Enabled = false;
                }
            }
        }

        private void UP2_TextChanged(object sender, EventArgs e)
        {
            if (UP2.Text == "")
            {
                UT2.Text = "";
                continue_btn.Enabled = false;
            }
            else
            {
                bool okay = true;
                // neuer Text darf nur Zeichen aus der Liste digits (in der Klasse deklariert)
                foreach (char c in UP2.Text.ToCharArray())
                {
                    // sobald es ein unpassendes Zeichen gibt, aufhoeren und Fehlermeldung ausgeben
                    if (!digits.Contains<char>(c))
                    {
                        okay = false;
                        break;
                    }
                }
                if (okay == true && Convert.ToInt32(UP2.Text) <= 1200)
                {
                    int zeit = (int)Math.Round(Convert.ToDouble(UP2.Text) / 5);
                    UT2.Text = zeit.ToString();
                    continue_btn.Enabled = true;
                }
                else
                {
                    MessageBox.Show("Es sind nur Werte von 0 bis 1200 zulässig.");
                    continue_btn.Enabled = false;
                }
            }
        }

        private void UP3_TextChanged(object sender, EventArgs e)
        {
            if (UP3.Text == "")
            {
                UT3.Text = "";
                continue_btn.Enabled = false;
            }
            else
            {
                bool okay = true;
                // neuer Text darf nur Zeichen aus der Liste digits (in der Klasse deklariert)
                foreach (char c in UP3.Text.ToCharArray())
                {
                    // sobald es ein unpassendes Zeichen gibt, aufhoeren und Fehlermeldung ausgeben
                    if (!digits.Contains<char>(c))
                    {
                        okay = false;
                        break;
                    }
                }
                if (okay == true && Convert.ToInt32(UP3.Text) <= 1200)
                {
                    int zeit = (int)Math.Round(Convert.ToDouble(UP3.Text) / 5);
                    UT3.Text = zeit.ToString();
                    continue_btn.Enabled = true;
                }
                else
                {
                    MessageBox.Show("Es sind nur Werte von 0 bis 1200 zulässig.");
                    continue_btn.Enabled = false;
                }
            }
        }

        private void UP4_TextChanged(object sender, EventArgs e)
        {
            if (UP4.Text == "")
            {
                UT4.Text = "";
                continue_btn.Enabled = false;
            }
            else
            {
                bool okay = true;
                // neuer Text darf nur Zeichen aus der Liste digits (in der Klasse deklariert)
                foreach (char c in UP4.Text.ToCharArray())
                {
                    // sobald es ein unpassendes Zeichen gibt, aufhoeren und Fehlermeldung ausgeben
                    if (!digits.Contains<char>(c))
                    {
                        okay = false;
                        break;
                    }
                }
                if (okay == true && Convert.ToInt32(UP4.Text) <= 1200)
                {
                    int zeit = (int)Math.Round(Convert.ToDouble(UP4.Text) / 5);
                    UT4.Text = zeit.ToString();
                    continue_btn.Enabled = true;
                }
                else
                {
                    MessageBox.Show("Es sind nur Werte von 0 bis 1200 zulässig.");
                    continue_btn.Enabled = false;
                }
            }
        }

        private void UP5_TextChanged(object sender, EventArgs e)
        {
            if (UP5.Text == "")
            {
                UT5.Text = "";
                continue_btn.Enabled = false;
            }
            else
            {
                bool okay = true;
                // neuer Text darf nur Zeichen aus der Liste digits (in der Klasse deklariert)
                foreach (char c in UP5.Text.ToCharArray())
                {
                    // sobald es ein unpassendes Zeichen gibt, aufhoeren und Fehlermeldung ausgeben
                    if (!digits.Contains<char>(c))
                    {
                        okay = false;
                        break;
                    }
                }
                if (okay == true && Convert.ToInt32(UP5.Text) <= 1200)
                {
                    int zeit = (int)Math.Round(Convert.ToDouble(UP5.Text) / 5);
                    UT5.Text = zeit.ToString();
                    continue_btn.Enabled = true;
                }
                else
                {
                    MessageBox.Show("Es sind nur Werte von 0 bis 1200 zulässig.");
                    continue_btn.Enabled = false;
                }
            }
        }

        private void UP6_TextChanged(object sender, EventArgs e)
        {
            if (UP6.Text == "")
            {
                UT6.Text = "";
                continue_btn.Enabled = false;
            }
            else
            {
                bool okay = true;
                // neuer Text darf nur Zeichen aus der Liste digits (in der Klasse deklariert)
                foreach (char c in UP6.Text.ToCharArray())
                {
                    // sobald es ein unpassendes Zeichen gibt, aufhoeren und Fehlermeldung ausgeben
                    if (!digits.Contains<char>(c))
                    {
                        okay = false;
                        break;
                    }
                }
                if (okay == true && Convert.ToInt32(UP6.Text) <= 1200)
                {
                    int zeit = (int)Math.Round(Convert.ToDouble(UP6.Text) / 5);
                    UT6.Text = zeit.ToString();
                    continue_btn.Enabled = true;
                }
                else
                {
                    MessageBox.Show("Es sind nur Werte von 0 bis 1200 zulässig.");
                    continue_btn.Enabled = false;
                }
            }
        }

        private void UP7_TextChanged(object sender, EventArgs e)
        {
            if (UP7.Text == "")
            {
                UT7.Text = "";
                continue_btn.Enabled = false;
            }
            else
            {
                bool okay = true;
                // neuer Text darf nur Zeichen aus der Liste digits (in der Klasse deklariert)
                foreach (char c in UP7.Text.ToCharArray())
                {
                    // sobald es ein unpassendes Zeichen gibt, aufhoeren und Fehlermeldung ausgeben
                    if (!digits.Contains<char>(c))
                    {
                        okay = false;
                        break;
                    }
                }
                if (okay == true && Convert.ToInt32(UP7.Text) <= 1200)
                {
                    int zeit = (int)Math.Round(Convert.ToDouble(UP7.Text) / 5);
                    UT7.Text = zeit.ToString();
                    continue_btn.Enabled = true;
                }
                else
                {
                    MessageBox.Show("Es sind nur Werte von 0 bis 1200 zulässig.");
                    continue_btn.Enabled = false;
                }
            }
        }

        private void UP8_TextChanged(object sender, EventArgs e)
        {
            if (UP8.Text == "")
            {
                UT8.Text = "";
                continue_btn.Enabled = false;
            }
            else
            {
                bool okay = true;
                // neuer Text darf nur Zeichen aus der Liste digits (in der Klasse deklariert)
                foreach (char c in UP8.Text.ToCharArray())
                {
                    // sobald es ein unpassendes Zeichen gibt, aufhoeren und Fehlermeldung ausgeben
                    if (!digits.Contains<char>(c))
                    {
                        okay = false;
                        break;
                    }
                }
                if (okay == true && Convert.ToInt32(UP8.Text) <= 1200)
                {
                    int zeit = (int)Math.Round(Convert.ToDouble(UP8.Text) / 5);
                    UT8.Text = zeit.ToString();
                    continue_btn.Enabled = true;
                }
                else
                {
                    MessageBox.Show("Es sind nur Werte von 0 bis 1200 zulässig.");
                    continue_btn.Enabled = false;
                }
            }
        }

        private void UP9_TextChanged(object sender, EventArgs e)
        {
            if (UP9.Text == "")
            {
                UT9.Text = "";
                continue_btn.Enabled = false;
            }
            else
            {
                bool okay = true;
                // neuer Text darf nur Zeichen aus der Liste digits (in der Klasse deklariert)
                foreach (char c in UP9.Text.ToCharArray())
                {
                    // sobald es ein unpassendes Zeichen gibt, aufhoeren und Fehlermeldung ausgeben
                    if (!digits.Contains<char>(c))
                    {
                        okay = false;
                        break;
                    }
                }
                if (okay == true && Convert.ToInt32(UP9.Text) <= 1200)
                {
                    int zeit = (int)Math.Round(Convert.ToDouble(UP9.Text) / 5);
                    UT9.Text = zeit.ToString();
                    continue_btn.Enabled = true;
                }
                else
                {
                    MessageBox.Show("Es sind nur Werte von 0 bis 1200 zulässig.");
                    continue_btn.Enabled = false;
                }
            }
        }

        private void UP10_TextChanged(object sender, EventArgs e)
        {
            if (UP10.Text == "")
            {
                UT10.Text = "";
                continue_btn.Enabled = false;
            }
            else
            {
                bool okay = true;
                // neuer Text darf nur Zeichen aus der Liste digits (in der Klasse deklariert)
                foreach (char c in UP10.Text.ToCharArray())
                {
                    // sobald es ein unpassendes Zeichen gibt, aufhoeren und Fehlermeldung ausgeben
                    if (!digits.Contains<char>(c))
                    {
                        okay = false;
                        break;
                    }
                }
                if (okay == true && Convert.ToInt32(UP10.Text) <= 1200)
                {
                    int zeit = (int)Math.Round(Convert.ToDouble(UP10.Text) / 5);
                    UT10.Text = zeit.ToString();
                    continue_btn.Enabled = true;
                }
                else
                {
                    MessageBox.Show("Es sind nur Werte von 0 bis 1200 zulässig.");
                    continue_btn.Enabled = false;
                }
            }
        }

        private void UP11_TextChanged(object sender, EventArgs e)
        {
            if (UP11.Text == "")
            {
                UT11.Text = "";
                continue_btn.Enabled = false;
            }
            else
            {
                bool okay = true;
                // neuer Text darf nur Zeichen aus der Liste digits (in der Klasse deklariert)
                foreach (char c in UP11.Text.ToCharArray())
                {
                    // sobald es ein unpassendes Zeichen gibt, aufhoeren und Fehlermeldung ausgeben
                    if (!digits.Contains<char>(c))
                    {
                        okay = false;
                        break;
                    }
                }
                if (okay == true && Convert.ToInt32(UP11.Text) <= 1200)
                {
                    int zeit = (int)Math.Round(Convert.ToDouble(UP11.Text) / 5);
                    UT11.Text = zeit.ToString();
                    continue_btn.Enabled = true;
                }
                else
                {
                    MessageBox.Show("Es sind nur Werte von 0 bis 1200 zulässig.");
                    continue_btn.Enabled = false;
                }
            }
        }

        private void UP12_TextChanged(object sender, EventArgs e)
        {
            if (UP12.Text == "")
            {
                UT12.Text = "";
                continue_btn.Enabled = false;
            }
            else
            {
                bool okay = true;
                // neuer Text darf nur Zeichen aus der Liste digits (in der Klasse deklariert)
                foreach (char c in UP12.Text.ToCharArray())
                {
                    // sobald es ein unpassendes Zeichen gibt, aufhoeren und Fehlermeldung ausgeben
                    if (!digits.Contains<char>(c))
                    {
                        okay = false;
                        break;
                    }
                }
                if (okay == true && Convert.ToInt32(UP12.Text) <= 1200)
                {
                    int zeit = (int)Math.Round(Convert.ToDouble(UP12.Text) / 5);
                    UT12.Text = zeit.ToString();
                    continue_btn.Enabled = true;
                }
                else
                {
                    MessageBox.Show("Es sind nur Werte von 0 bis 1200 zulässig.");
                    continue_btn.Enabled = false;
                }
            }
        }

        private void UP13_TextChanged(object sender, EventArgs e)
        {
            if (UP13.Text == "")
            {
                UT13.Text = "";
                continue_btn.Enabled = false;
            }
            else
            {
                bool okay = true;
                // neuer Text darf nur Zeichen aus der Liste digits (in der Klasse deklariert)
                foreach (char c in UP13.Text.ToCharArray())
                {
                    // sobald es ein unpassendes Zeichen gibt, aufhoeren und Fehlermeldung ausgeben
                    if (!digits.Contains<char>(c))
                    {
                        okay = false;
                        break;
                    }
                }
                if (okay == true && Convert.ToInt32(UP13.Text) <= 1200)
                {
                    int zeit = (int)Math.Round(Convert.ToDouble(UP13.Text) / 5);
                    UT13.Text = zeit.ToString();
                    continue_btn.Enabled = true;
                }
                else
                {
                    MessageBox.Show("Es sind nur Werte von 0 bis 1200 zulässig.");
                    continue_btn.Enabled = false;
                }
            }
        }

        private void UP14_TextChanged(object sender, EventArgs e)
        {
            if (UP14.Text == "")
            {
                UT14.Text = "";
                continue_btn.Enabled = false;
            }
            else
            {
                bool okay = true;
                // neuer Text darf nur Zeichen aus der Liste digits (in der Klasse deklariert)
                foreach (char c in UP14.Text.ToCharArray())
                {
                    // sobald es ein unpassendes Zeichen gibt, aufhoeren und Fehlermeldung ausgeben
                    if (!digits.Contains<char>(c))
                    {
                        okay = false;
                        break;
                    }
                }
                if (okay == true && Convert.ToInt32(UP14.Text) <= 1200)
                {
                    int zeit = (int)Math.Round(Convert.ToDouble(UP14.Text) / 5);
                    UT14.Text = zeit.ToString();
                    continue_btn.Enabled = true;
                }
                else
                {
                    MessageBox.Show("Es sind nur Werte von 0 bis 1200 zulässig.");
                    continue_btn.Enabled = false;
                }
            }
        }

        private void UP15_TextChanged(object sender, EventArgs e)
        {
            if (UP15.Text == "")
            {
                UT15.Text = "";
                continue_btn.Enabled = false;
            }
            else
            {
                bool okay = true;
                // neuer Text darf nur Zeichen aus der Liste digits (in der Klasse deklariert)
                foreach (char c in UP15.Text.ToCharArray())
                {
                    // sobald es ein unpassendes Zeichen gibt, aufhoeren und Fehlermeldung ausgeben
                    if (!digits.Contains<char>(c))
                    {
                        okay = false;
                        break;
                    }
                }
                if (okay == true && Convert.ToInt32(UP15.Text) <= 1200)
                {
                    int zeit = (int)Math.Round(Convert.ToDouble(UP15.Text) / 5);
                    UT15.Text = zeit.ToString();
                    continue_btn.Enabled = true;
                }
                else
                {
                    MessageBox.Show("Es sind nur Werte von 0 bis 1200 zulässig.");
                    continue_btn.Enabled = false;
                }
            }
        }

        private void S1_TextChanged(object sender, EventArgs e)
        {
            // Achtung: erstmaliges Belegen der TextBoxen mit Werten loest ein TextChanged aus,
            // in diesem Fall ist der alte Werte 0 und es muss nichts geprueft werden
            int alt = schichten[0];
            if (alt != 0)
            {
                if (S1.Text == "1" || S1.Text == "2" || S1.Text == "3")
                {
                    continue_btn.Enabled = true;
                    // Wert der Zeile Ueberstd/Periode anpassen (loest autom. Aenderung der Zeile Ueberstd/Tag aus)
                    int neu = Convert.ToInt32(S1.Text);
                    // Wenn die Periode nach oben gesetzt wird, soll Ueberstd/Periode auf 0 gesetzt werden
                    if (neu > alt)
                    {
                        UP1.Text = "0";
                    }
                    // Wenn die Periode nach unten gesetzt wird, soll Ueberstd/Periode auf 1200 gesetzt werden
                    else if (neu < alt)
                    {
                        UP1.Text = "1200";
                    }
                    schichten[0] = neu;
                }
                else if (S1.Text == "")
                {
                    continue_btn.Enabled = false;
                }
                else
                {
                    MessageBox.Show("Es sind nur Werte von 1 bis 3 zulässig.");
                    S1.Text = alt.ToString();
                    continue_btn.Enabled = false;
                }
            }
        }

        private void S2_TextChanged(object sender, EventArgs e)
        {
            // Achtung: erstmaliges Belegen der TextBoxen mit Werten loest ein TextChanged aus,
            // in diesem Fall ist der alte Werte 0 und es muss nichts geprueft werden
            int alt = schichten[1];
            if (alt != 0)
            {
                if (S2.Text == "1" || S2.Text == "2" || S2.Text == "3")
                {
                    continue_btn.Enabled = true;
                    // Wert der Zeile Ueberstd/Periode anpassen (loest autom. Aenderung der Zeile Ueberstd/Tag aus)
                    int neu = Convert.ToInt32(S2.Text);
                    // Wenn die Periode nach oben gesetzt wird, soll Ueberstd/Periode auf 0 gesetzt werden
                    if (neu > alt)
                    {
                        UP2.Text = "0";
                    }
                    // Wenn die Periode nach unten gesetzt wird, soll Ueberstd/Periode auf 1200 gesetzt werden
                    else if (neu < alt)
                    {
                        UP2.Text = "1200";
                    }
                    schichten[1] = neu;
                }
                else if (S2.Text == "")
                {
                    continue_btn.Enabled = false;
                }
                else
                {
                    MessageBox.Show("Es sind nur Werte von 1 bis 3 zulässig.");
                    S2.Text = alt.ToString();
                    continue_btn.Enabled = false;
                }
            }
        }

        private void S3_TextChanged(object sender, EventArgs e)
        {
            // Achtung: erstmaliges Belegen der TextBoxen mit Werten loest ein TextChanged aus,
            // in diesem Fall ist der alte Werte 0 und es muss nichts geprueft werden
            int alt = schichten[2];
            if (alt != 0)
            {
                if (S3.Text == "1" || S3.Text == "2" || S3.Text == "3")
                {
                    continue_btn.Enabled = true;
                    // Wert der Zeile Ueberstd/Periode anpassen (loest autom. Aenderung der Zeile Ueberstd/Tag aus)
                    int neu = Convert.ToInt32(S3.Text);
                    // Wenn die Periode nach oben gesetzt wird, soll Ueberstd/Periode auf 0 gesetzt werden
                    if (neu > alt)
                    {
                        UP3.Text = "0";
                    }
                    // Wenn die Periode nach unten gesetzt wird, soll Ueberstd/Periode auf 1200 gesetzt werden
                    else if (neu < alt)
                    {
                        UP3.Text = "1200";
                    }
                    schichten[2] = neu;
                }
                else if (S3.Text == "")
                {
                    continue_btn.Enabled = false;
                }
                else
                {
                    MessageBox.Show("Es sind nur Werte von 1 bis 3 zulässig.");
                    S3.Text = alt.ToString();
                    continue_btn.Enabled = false;
                }
            }
        }

        private void S4_TextChanged(object sender, EventArgs e)
        {
            // Achtung: erstmaliges Belegen der TextBoxen mit Werten loest ein TextChanged aus,
            // in diesem Fall ist der alte Werte 0 und es muss nichts geprueft werden
            int alt = schichten[3];
            if (alt != 0)
            {
                if (S4.Text == "1" || S4.Text == "2" || S4.Text == "3")
                {
                    continue_btn.Enabled = true;
                    // Wert der Zeile Ueberstd/Periode anpassen (loest autom. Aenderung der Zeile Ueberstd/Tag aus)
                    int neu = Convert.ToInt32(S4.Text);
                    // Wenn die Periode nach oben gesetzt wird, soll Ueberstd/Periode auf 0 gesetzt werden
                    if (neu > alt)
                    {
                        UP4.Text = "0";
                    }
                    // Wenn die Periode nach unten gesetzt wird, soll Ueberstd/Periode auf 1200 gesetzt werden
                    else if (neu < alt)
                    {
                        UP4.Text = "1200";
                    }
                    schichten[3] = neu;
                }
                else if (S4.Text == "")
                {
                    continue_btn.Enabled = false;
                }
                else
                {
                    MessageBox.Show("Es sind nur Werte von 1 bis 3 zulässig.");
                    S4.Text = alt.ToString();
                    continue_btn.Enabled = false;
                }
            }
        }

        private void S5_TextChanged(object sender, EventArgs e)
        {
            // Achtung: erstmaliges Belegen der TextBoxen mit Werten loest ein TextChanged aus,
            // in diesem Fall ist der alte Werte 0 und es muss nichts geprueft werden
            int alt = schichten[4];
            if (alt != 0)
            {
                if (S5.Text == "1" || S5.Text == "2" || S5.Text == "3")
                {
                    continue_btn.Enabled = true;
                    // Wert der Zeile Ueberstd/Periode anpassen (loest autom. Aenderung der Zeile Ueberstd/Tag aus)
                    int neu = Convert.ToInt32(S5.Text);
                    // Wenn die Periode nach oben gesetzt wird, soll Ueberstd/Periode auf 0 gesetzt werden
                    if (neu > alt)
                    {
                        UP5.Text = "0";
                    }
                    // Wenn die Periode nach unten gesetzt wird, soll Ueberstd/Periode auf 1200 gesetzt werden
                    else if (neu < alt)
                    {
                        UP5.Text = "1200";
                    }
                    schichten[4] = neu;
                }
                else if (S5.Text == "")
                {
                    continue_btn.Enabled = false;
                }
                else
                {
                    MessageBox.Show("Es sind nur Werte von 1 bis 3 zulässig.");
                    S5.Text = alt.ToString();
                    continue_btn.Enabled = false;
                }
            }
        }

        private void S6_TextChanged(object sender, EventArgs e)
        {
            // Achtung: erstmaliges Belegen der TextBoxen mit Werten loest ein TextChanged aus,
            // in diesem Fall ist der alte Werte 0 und es muss nichts geprueft werden
            int alt = schichten[5];
            if (alt != 0)
            {
                if (S6.Text == "1" || S6.Text == "2" || S6.Text == "3")
                {
                    continue_btn.Enabled = true;
                    // Wert der Zeile Ueberstd/Periode anpassen (loest autom. Aenderung der Zeile Ueberstd/Tag aus)
                    int neu = Convert.ToInt32(S6.Text);
                    // Wenn die Periode nach oben gesetzt wird, soll Ueberstd/Periode auf 0 gesetzt werden
                    if (neu > alt)
                    {
                        UP6.Text = "0";
                    }
                    // Wenn die Periode nach unten gesetzt wird, soll Ueberstd/Periode auf 1200 gesetzt werden
                    else if (neu < alt)
                    {
                        UP6.Text = "1200";
                    }
                    schichten[5] = neu;
                }
                else if (S6.Text == "")
                {
                    continue_btn.Enabled = false;
                }
                else
                {
                    MessageBox.Show("Es sind nur Werte von 1 bis 3 zulässig.");
                    S6.Text = alt.ToString();
                    continue_btn.Enabled = false;
                }
            }
        }

        private void S7_TextChanged(object sender, EventArgs e)
        {
            // Achtung: erstmaliges Belegen der TextBoxen mit Werten loest ein TextChanged aus,
            // in diesem Fall ist der alte Werte 0 und es muss nichts geprueft werden
            int alt = schichten[6];
            if (alt != 0)
            {
                if (S7.Text == "1" || S7.Text == "2" || S7.Text == "3")
                {
                    continue_btn.Enabled = true;
                    // Wert der Zeile Ueberstd/Periode anpassen (loest autom. Aenderung der Zeile Ueberstd/Tag aus)
                    int neu = Convert.ToInt32(S7.Text);
                    // Wenn die Periode nach oben gesetzt wird, soll Ueberstd/Periode auf 0 gesetzt werden
                    if (neu > alt)
                    {
                        UP7.Text = "0";
                    }
                    // Wenn die Periode nach unten gesetzt wird, soll Ueberstd/Periode auf 1200 gesetzt werden
                    else if (neu < alt)
                    {
                        UP7.Text = "1200";
                    }
                    schichten[6] = neu;
                }
                else if (S7.Text == "")
                {
                    continue_btn.Enabled = false;
                }
                else
                {
                    MessageBox.Show("Es sind nur Werte von 1 bis 3 zulässig.");
                    S7.Text = alt.ToString();
                    continue_btn.Enabled = false;
                }
            }
        }

        private void S8_TextChanged(object sender, EventArgs e)
        {
            // Achtung: erstmaliges Belegen der TextBoxen mit Werten loest ein TextChanged aus,
            // in diesem Fall ist der alte Werte 0 und es muss nichts geprueft werden
            int alt = schichten[7];
            if (alt != 0)
            {
                if (S8.Text == "1" || S8.Text == "2" || S8.Text == "3")
                {
                    continue_btn.Enabled = true;
                    // Wert der Zeile Ueberstd/Periode anpassen (loest autom. Aenderung der Zeile Ueberstd/Tag aus)
                    int neu = Convert.ToInt32(S8.Text);
                    // Wenn die Periode nach oben gesetzt wird, soll Ueberstd/Periode auf 0 gesetzt werden
                    if (neu > alt)
                    {
                        UP8.Text = "0";
                    }
                    // Wenn die Periode nach unten gesetzt wird, soll Ueberstd/Periode auf 1200 gesetzt werden
                    else if (neu < alt)
                    {
                        UP8.Text = "1200";
                    }
                    schichten[7] = neu;
                }
                else if (S8.Text == "")
                {
                    continue_btn.Enabled = false;
                }
                else
                {
                    MessageBox.Show("Es sind nur Werte von 1 bis 3 zulässig.");
                    S8.Text = alt.ToString();
                    continue_btn.Enabled = false;
                }
            }
        }

        private void S9_TextChanged(object sender, EventArgs e)
        {
            // Achtung: erstmaliges Belegen der TextBoxen mit Werten loest ein TextChanged aus,
            // in diesem Fall ist der alte Werte 0 und es muss nichts geprueft werden
            int alt = schichten[8];
            if (alt != 0)
            {
                if (S9.Text == "1" || S9.Text == "2" || S9.Text == "3")
                {
                    continue_btn.Enabled = true;
                    // Wert der Zeile Ueberstd/Periode anpassen (loest autom. Aenderung der Zeile Ueberstd/Tag aus)
                    int neu = Convert.ToInt32(S9.Text);
                    // Wenn die Periode nach oben gesetzt wird, soll Ueberstd/Periode auf 0 gesetzt werden
                    if (neu > alt)
                    {
                        UP9.Text = "0";
                    }
                    // Wenn die Periode nach unten gesetzt wird, soll Ueberstd/Periode auf 1200 gesetzt werden
                    else if (neu < alt)
                    {
                        UP9.Text = "1200";
                    }
                    schichten[8] = neu;
                }
                else if (S9.Text == "")
                {
                    continue_btn.Enabled = false;
                }
                else
                {
                    MessageBox.Show("Es sind nur Werte von 1 bis 3 zulässig.");
                    S9.Text = alt.ToString();
                    continue_btn.Enabled = false;
                }
            }
        }

        private void S10_TextChanged(object sender, EventArgs e)
        {
            // Achtung: erstmaliges Belegen der TextBoxen mit Werten loest ein TextChanged aus,
            // in diesem Fall ist der alte Werte 0 und es muss nichts geprueft werden
            int alt = schichten[9];
            if (alt != 0)
            {
                if (S10.Text == "1" || S10.Text == "2" || S10.Text == "3")
                {
                    continue_btn.Enabled = true;
                    // Wert der Zeile Ueberstd/Periode anpassen (loest autom. Aenderung der Zeile Ueberstd/Tag aus)
                    int neu = Convert.ToInt32(S10.Text);
                    // Wenn die Periode nach oben gesetzt wird, soll Ueberstd/Periode auf 0 gesetzt werden
                    if (neu > alt)
                    {
                        UP10.Text = "0";
                    }
                    // Wenn die Periode nach unten gesetzt wird, soll Ueberstd/Periode auf 1200 gesetzt werden
                    else if (neu < alt)
                    {
                        UP10.Text = "1200";
                    }
                    schichten[9] = neu;
                }
                else if (S10.Text == "")
                {
                    continue_btn.Enabled = false;
                }
                else
                {
                    MessageBox.Show("Es sind nur Werte von 1 bis 3 zulässig.");
                    S10.Text = alt.ToString();
                    continue_btn.Enabled = false;
                }
            }
        }

        private void S11_TextChanged(object sender, EventArgs e)
        {
            // Achtung: erstmaliges Belegen der TextBoxen mit Werten loest ein TextChanged aus,
            // in diesem Fall ist der alte Werte 0 und es muss nichts geprueft werden
            int alt = schichten[10];
            if (alt != 0)
            {
                if (S11.Text == "1" || S11.Text == "2" || S11.Text == "3")
                {
                    continue_btn.Enabled = true;
                    // Wert der Zeile Ueberstd/Periode anpassen (loest autom. Aenderung der Zeile Ueberstd/Tag aus)
                    int neu = Convert.ToInt32(S11.Text);
                    // Wenn die Periode nach oben gesetzt wird, soll Ueberstd/Periode auf 0 gesetzt werden
                    if (neu > alt)
                    {
                        UP11.Text = "0";
                    }
                    // Wenn die Periode nach unten gesetzt wird, soll Ueberstd/Periode auf 1200 gesetzt werden
                    else if (neu < alt)
                    {
                        UP11.Text = "1200";
                    }
                    schichten[10] = neu;
                }
                else if (S11.Text == "")
                {
                    continue_btn.Enabled = false;
                }
                else
                {
                    MessageBox.Show("Es sind nur Werte von 1 bis 3 zulässig.");
                    S11.Text = alt.ToString();
                    continue_btn.Enabled = false;
                }
            }
        }

        private void S12_TextChanged(object sender, EventArgs e)
        {
            // Achtung: erstmaliges Belegen der TextBoxen mit Werten loest ein TextChanged aus,
            // in diesem Fall ist der alte Werte 0 und es muss nichts geprueft werden
            int alt = schichten[11];
            if (alt != 0)
            {
                if (S12.Text == "1" || S12.Text == "2" || S12.Text == "3")
                {
                    continue_btn.Enabled = true;
                    // Wert der Zeile Ueberstd/Periode anpassen (loest autom. Aenderung der Zeile Ueberstd/Tag aus)
                    int neu = Convert.ToInt32(S12.Text);
                    // Wenn die Periode nach oben gesetzt wird, soll Ueberstd/Periode auf 0 gesetzt werden
                    if (neu > alt)
                    {
                        UP12.Text = "0";
                    }
                    // Wenn die Periode nach unten gesetzt wird, soll Ueberstd/Periode auf 1200 gesetzt werden
                    else if (neu < alt)
                    {
                        UP12.Text = "1200";
                    }
                    schichten[11] = neu;
                }
                else if (S12.Text == "")
                {
                    continue_btn.Enabled = false;
                }
                else
                {
                    MessageBox.Show("Es sind nur Werte von 1 bis 3 zulässig.");
                    S12.Text = alt.ToString();
                    continue_btn.Enabled = false;
                }
            }
        }

        private void S13_TextChanged(object sender, EventArgs e)
        {
            // Achtung: erstmaliges Belegen der TextBoxen mit Werten loest ein TextChanged aus,
            // in diesem Fall ist der alte Werte 0 und es muss nichts geprueft werden
            int alt = schichten[12];
            if (alt != 0)
            {
                if (S13.Text == "1" || S13.Text == "2" || S13.Text == "3")
                {
                    continue_btn.Enabled = true;
                    // Wert der Zeile Ueberstd/Periode anpassen (loest autom. Aenderung der Zeile Ueberstd/Tag aus)
                    int neu = Convert.ToInt32(S13.Text);
                    // Wenn die Periode nach oben gesetzt wird, soll Ueberstd/Periode auf 0 gesetzt werden
                    if (neu > alt)
                    {
                        UP13.Text = "0";
                    }
                    // Wenn die Periode nach unten gesetzt wird, soll Ueberstd/Periode auf 1200 gesetzt werden
                    else if (neu < alt)
                    {
                        UP13.Text = "1200";
                    }
                    schichten[12] = neu;
                }
                else if (S13.Text == "")
                {
                    continue_btn.Enabled = false;
                }
                else
                {
                    MessageBox.Show("Es sind nur Werte von 1 bis 3 zulässig.");
                    S13.Text = alt.ToString();
                    continue_btn.Enabled = false;
                }
            }
        }

        private void S14_TextChanged(object sender, EventArgs e)
        {
            // Achtung: erstmaliges Belegen der TextBoxen mit Werten loest ein TextChanged aus,
            // in diesem Fall ist der alte Werte 0 und es muss nichts geprueft werden
            int alt = schichten[13];
            if (alt != 0)
            {
                if (S14.Text == "1" || S14.Text == "2" || S14.Text == "3")
                {
                    continue_btn.Enabled = true;
                    // Wert der Zeile Ueberstd/Periode anpassen (loest autom. Aenderung der Zeile Ueberstd/Tag aus)
                    int neu = Convert.ToInt32(S14.Text);
                    // Wenn die Periode nach oben gesetzt wird, soll Ueberstd/Periode auf 0 gesetzt werden
                    if (neu > alt)
                    {
                        UP14.Text = "0";
                    }
                    // Wenn die Periode nach unten gesetzt wird, soll Ueberstd/Periode auf 1200 gesetzt werden
                    else if (neu < alt)
                    {
                        UP14.Text = "1200";
                    }
                    schichten[13] = neu;
                }
                else if (S14.Text == "")
                {
                    continue_btn.Enabled = false;
                }
                else
                {
                    MessageBox.Show("Es sind nur Werte von 1 bis 3 zulässig.");
                    S14.Text = alt.ToString();
                    continue_btn.Enabled = false;
                }
            }
        }

        private void S15_TextChanged(object sender, EventArgs e)
        {
            // Achtung: erstmaliges Belegen der TextBoxen mit Werten loest ein TextChanged aus,
            // in diesem Fall ist der alte Werte 0 und es muss nichts geprueft werden
            int alt = schichten[14];
            if (alt != 0)
            {
                if (S15.Text == "1" || S15.Text == "2" || S15.Text == "3")
                {
                    continue_btn.Enabled = true;
                    // Wert der Zeile Ueberstd/Periode anpassen (loest autom. Aenderung der Zeile Ueberstd/Tag aus)
                    int neu = Convert.ToInt32(S15.Text);
                    // Wenn die Periode nach oben gesetzt wird, soll Ueberstd/Periode auf 0 gesetzt werden
                    if (neu > alt)
                    {
                        UP15.Text = "0";
                    }
                    // Wenn die Periode nach unten gesetzt wird, soll Ueberstd/Periode auf 1200 gesetzt werden
                    else if (neu < alt)
                    {
                        UP15.Text = "1200";
                    }
                    schichten[14] = neu;
                }
                else if (S15.Text == "")
                {
                    continue_btn.Enabled = false;
                }
                else
                {
                    MessageBox.Show("Es sind nur Werte von 1 bis 3 zulässig.");
                    S15.Text = alt.ToString();
                    continue_btn.Enabled = false;
                }
            }
        }
    }
}
