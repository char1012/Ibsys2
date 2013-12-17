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
    public partial class Kapazitaetsplan : UserControl
    {
        private OleDbConnection myconn;
        // Liste der zulaessigen Zeichen bei Benutzereingaben
        private char[] digits = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
        // Listen zum Speichern der Startwerte (Aenderungen nachvollziehen + Feststellen, ob initiale Belegung)
        private int[] schichten;
        // Liste, um zu kontrollieren, ob alle Zellen korrekt sind
        private Boolean[] correct = new Boolean[30] { true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true };

        // Datenweitergabe:
        int aktPeriode;
        int[] auftraege = new int[12];
        int[] direktverkaeufe = new int[3];
        int[,] sicherheitsbest = new int[30, 5];
        int[,] produktion = new int[30, 2];
        int[,] produktionProg = new int[3, 5];
        int[,] prodReihenfolge = new int[30, 2];
        int[,] kapazitaet = new int[15, 5];
        int[,] kaufauftraege = new int[29, 6];

        public Kapazitaetsplan()
        {
            InitializeComponent();
            setButtons(true); // false, wenn Zellen geleert werden
            setValues();
            if (pic_de.SizeMode == PictureBoxSizeMode.StretchImage)
            {
                System.Windows.Forms.ToolTip ToolTipDE = new System.Windows.Forms.ToolTip();
                ToolTipDE.SetToolTip(this.pictureBox7, Sprachen.DE_KP_INFO);
            }
            else
            {
                System.Windows.Forms.ToolTip ToolTipEN = new System.Windows.Forms.ToolTip();
                ToolTipEN.SetToolTip(this.pictureBox7, Sprachen.EN_KP_INFO);
            }

        }

        public Kapazitaetsplan(int aktPeriode, int[] auftraege, int[] direktverkaeufe, int[,] sicherheitsbest,
            int[,] produktion, int[,] produktionProg, int[,] prodReihenfolge, int[,] kapazitaet, int[,] kaufauftraege)
        {
            this.aktPeriode = aktPeriode;
            if (auftraege != null)
            {
                this.auftraege = auftraege;
            }
            if (direktverkaeufe != null)
            {
                this.direktverkaeufe = direktverkaeufe;
            }
            if (sicherheitsbest != null)
            {
                this.sicherheitsbest = sicherheitsbest;
            }
            if (produktion != null)
            {
                this.produktion = produktion;
            }
            if (produktionProg != null)
            {
                this.produktionProg = produktionProg;
            }
            if (prodReihenfolge != null)
            {
                this.prodReihenfolge = prodReihenfolge;
            }
            if (kapazitaet != null)
            {
                this.kapazitaet = kapazitaet;
            }
            if (kaufauftraege != null)
            {
                this.kaufauftraege = kaufauftraege;
            }

            InitializeComponent();
            setButtons(true);
            if (pic_de.SizeMode == PictureBoxSizeMode.StretchImage)
            {
                System.Windows.Forms.ToolTip ToolTipDE = new System.Windows.Forms.ToolTip();
                ToolTipDE.SetToolTip(this.pictureBox7, Sprachen.DE_KP_INFO);
            }
            else
            {
                System.Windows.Forms.ToolTip ToolTipEN = new System.Windows.Forms.ToolTip();
                ToolTipEN.SetToolTip(this.pictureBox7, Sprachen.EN_KP_INFO);
            }

            Boolean bereitsBerechnet = false;
            for (int i = 0; i < kapazitaet.GetLength(0); i++)
            {
                if (kapazitaet[i, 1] > 0)
                {
                    bereitsBerechnet = true;
                    break;
                }
            }
            // wenn bereits Werte vorhanden sind, Felder fuellen
            // Kapbedarf trotzdem nochmal berechnen
            if (bereitsBerechnet == true)
            {
                // Mitteilung einblenden
                ProcessMessage message = new ProcessMessage();
                message.Show(this);
                message.Location = new Point(500, 300);
                message.Update();
                this.Enabled = false;

                int periode = aktPeriode - 1; // Periode des xmls
                int[,] teile = produktion; // Produktion

                // Methode zur Berechnung der Werte aufrufen
                int[] plaetze = calculate(periode, teile);

                // Zeilen fuellen
                for (int i = 0; i < plaetze.Length; ++i)
                {
                    int k = i + 1;
                    this.Controls.Find("K" + k.ToString(), true)[0].Text = plaetze[i].ToString();
                    this.Controls.Find("UP" + k.ToString(), true)[0].Text = kapazitaet[i, 2].ToString();
                    this.Controls.Find("UT" + k.ToString(), true)[0].Text = kapazitaet[i, 3].ToString();
                    this.Controls.Find("S" + k.ToString(), true)[0].Text = kapazitaet[i, 4].ToString();
                }

                message.Close();
                this.Enabled = true;
            }
            // sonst Werte berechnen
            else
            {
                setValues();
            }
        }

        public void setButtons(Boolean b)
        {
            back_btn.Enabled = b;
            continue_btn.Enabled = b;
            lbl_Startseite.Enabled = b;
            lbl_Sicherheitsbestand.Enabled = b;
            lbl_Produktion.Enabled = b;
            lbl_Produktionsreihenfolge.Enabled = b;
            lbl_Kapazitaetsplan.Enabled = b;
            lbl_Kaufteiledisposition.Enabled = b;
            lbl_Ergebnis.Enabled = b;
        }

        public void setValues()
        {
            // Mitteilung einblenden
            ProcessMessage message = new ProcessMessage();
            message.Show(this);
            message.Location = new Point(500, 300);
            message.Update();
            this.Enabled = false;

            int periode = aktPeriode - 1; // Periode des xmls
            int[,] teile = produktion; // Produktion

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
                    this.Controls.Find("S" + s.ToString(), true)[0].BackColor = Color.Red;
                }
                this.Controls.Find("S" + s.ToString(), true)[0].Text = schicht.ToString();
                schichten[i] = schicht; // Startwert der Zeile Schichten speichern
            }

            message.Close();
            this.Enabled = true;
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
                // b) Infos aus Warteliste_Arbeitsplatz zur aktuellen Periode
                a = 0;
                List<List<int>> warteliste_arbeitsplatz = new List<List<int>>();
                cmd.CommandText = @"SELECT Arbeitsplatz_FK, Teilenummer_FK, Menge, Zeitbedarf FROM Warteliste_Arbeitsplatz WHERE Periode = " + periode + ";";
                dbReader = cmd.ExecuteReader();
                while (dbReader.Read())
                {
                    warteliste_arbeitsplatz.Add(new List<int>());
                    warteliste_arbeitsplatz[a].Add(Convert.ToInt32(dbReader["Arbeitsplatz_FK"]));
                    warteliste_arbeitsplatz[a].Add(Convert.ToInt32(dbReader["Teilenummer_FK"]));
                    warteliste_arbeitsplatz[a].Add(Convert.ToInt32(dbReader["Menge"]));
                    warteliste_arbeitsplatz[a].Add(Convert.ToInt32(dbReader["Zeitbedarf"]));
                    ++a;
                }
                dbReader.Close();
                // c) Infos aus Bearbeitung zur aktuellen Periode
                a = 0;
                List<List<int>> bearbeitung = new List<List<int>>();
                cmd.CommandText = @"SELECT Arbeitsplatz_FK, Teilenummer_FK, Menge, Zeitbedarf FROM Bearbeitung WHERE Periode = " + periode + ";";
                dbReader = cmd.ExecuteReader();
                while (dbReader.Read())
                {
                    bearbeitung.Add(new List<int>());
                    bearbeitung[a].Add(Convert.ToInt32(dbReader["Arbeitsplatz_FK"]));
                    bearbeitung[a].Add(Convert.ToInt32(dbReader["Teilenummer_FK"]));
                    bearbeitung[a].Add(Convert.ToInt32(dbReader["Menge"]));
                    bearbeitung[a].Add(Convert.ToInt32(dbReader["Zeitbedarf"]));
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
                for (int n = 0; n < warteliste_arbeitsplatz.Count; ++n)
                {
                    if (warteliste_arbeitsplatz[n][0] == platznr)
                    {
                        int teilenummer = warteliste_arbeitsplatz[n][1];
                        // Bearbeitungszeit
                        rueckstandBearbeitungszeit += warteliste_arbeitsplatz[n][3];
                        // Ruestzeit
                        for (int no = 0; no < arbeitsplatz_erzeugnis.Count; ++no)
                        {
                            if (arbeitsplatz_erzeugnis[no][0] == teilenummer)
                            {
                                rueckstandRuestzeit += arbeitsplatz_erzeugnis[no][2];
                            }
                        }
                    }
                }

                // 2.2. In Bearbeitung ueberpruefen -> nur Bearbeitungszeit, keine Rüstzeit
                for (int n = 0; n < bearbeitung.Count; ++n)
                {
                    if (bearbeitung[n][0] == platznr)
                    {
                        rueckstandBearbeitungszeit += bearbeitung[n][3];
                    }
                }

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
                                for (int no = 0; no < warteliste_arbeitsplatz.Count; ++no)
                                {
                                    if (warteliste_arbeitsplatz[no][0] == andererPlatz
                                            && warteliste_arbeitsplatz[no][1] == teilenummer)
                                    {
                                        rueckstandBearbeitungszeit += bzeit * warteliste_arbeitsplatz[no][2];
                                        rueckstandRuestzeit += rzeit;
                                    }
                                }
                                // kontrollieren, ob diese in Bearbeitung liegen
                                for (int no = 0; no < bearbeitung.Count; ++no)
                                {
                                    if (bearbeitung[no][0] == andererPlatz
                                            && bearbeitung[no][1] == teilenummer)
                                    {
                                        rueckstandBearbeitungszeit += bzeit * bearbeitung[no][2];
                                        rueckstandRuestzeit += rzeit;
                                    }
                                }
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
            // Datenweitergabe

            // Werte aus TextBoxen in kapazitaet auslesen
            for (int i = 0; i < kapazitaet.GetLength(0); ++i)
            {
                int k = i + 1;
                kapazitaet[i, 0] = k;
                kapazitaet[i, 1] = Convert.ToInt32(this.Controls.Find("K" + k.ToString(), true)[0].Text);
                kapazitaet[i, 2] = Convert.ToInt32(this.Controls.Find("UP" + k.ToString(), true)[0].Text);
                kapazitaet[i, 3] = Convert.ToInt32(this.Controls.Find("UT" + k.ToString(), true)[0].Text);
                kapazitaet[i, 4] = Convert.ToInt32(this.Controls.Find("S" + k.ToString(), true)[0].Text);
            }

            this.Controls.Clear();
            UserControl kaufteile = new Kaufteildisposition(aktPeriode, auftraege, direktverkaeufe,
                sicherheitsbest, produktion, produktionProg, prodReihenfolge, kapazitaet, kaufauftraege);
            this.Controls.Add(kaufteile);
        }

        private void UP1_TextChanged(object sender, EventArgs e)
        {
            if (UP1.Text == "")
            {
                UT1.Text = "";
                setButtons(false);
                correct[0] = false;
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
                    UP1.ForeColor = Color.Black;
                    int zeit = (int)Math.Round(Convert.ToDouble(UP1.Text) / 5);
                    UT1.Text = zeit.ToString();
                    correct[0] = true;
                    if (!correct.Contains(false))
                    {
                        setButtons(true);
                    }
                }
                else
                {
                    UP1.ForeColor = Color.Red;
                    UT1.Text = "";
                    setButtons(false);
                    correct[0] = false;
                }
            }
        }

        private void UP2_TextChanged(object sender, EventArgs e)
        {
            if (UP2.Text == "")
            {
                UT2.Text = "";
                setButtons(false);
                correct[1] = false;
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
                    UP2.ForeColor = Color.Black;
                    int zeit = (int)Math.Round(Convert.ToDouble(UP2.Text) / 5);
                    UT2.Text = zeit.ToString();
                    correct[1] = true;
                    if (!correct.Contains(false))
                    {
                        setButtons(true);
                    }
                }
                else
                {
                    UP2.ForeColor = Color.Red;
                    UT2.Text = ""; 
                    setButtons(false);
                    correct[1] = false;
                }
            }
        }

        private void UP3_TextChanged(object sender, EventArgs e)
        {
            if (UP3.Text == "")
            {
                UT3.Text = "";
                setButtons(false);
                correct[2] = false;
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
                    UP3.ForeColor = Color.Black;
                    int zeit = (int)Math.Round(Convert.ToDouble(UP3.Text) / 5);
                    UT3.Text = zeit.ToString();
                    correct[2] = true;
                    if (!correct.Contains(false))
                    {
                        setButtons(true);
                    }
                }
                else
                {
                    UP3.ForeColor = Color.Red;
                    UT3.Text = "";
                    setButtons(false);
                    correct[2] = false;
                }
            }
        }

        private void UP4_TextChanged(object sender, EventArgs e)
        {
            if (UP4.Text == "")
            {
                UT4.Text = "";
                setButtons(false);
                correct[3] = false;
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
                    UP4.ForeColor = Color.Black;
                    int zeit = (int)Math.Round(Convert.ToDouble(UP4.Text) / 5);
                    UT4.Text = zeit.ToString();
                    correct[3] = true;
                    if (!correct.Contains(false))
                    {
                        setButtons(true);
                    }
                }
                else
                {
                    UP4.ForeColor = Color.Red;
                    UT4.Text = ""; 
                    setButtons(false);
                    correct[3] = false;
                }
            }
        }

        private void UP5_TextChanged(object sender, EventArgs e)
        {
            if (UP5.Text == "")
            {
                UT5.Text = "";
                setButtons(false);
                correct[4] = false;
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
                    UP5.ForeColor = Color.Black;
                    int zeit = (int)Math.Round(Convert.ToDouble(UP5.Text) / 5);
                    UT5.Text = zeit.ToString();
                    correct[4] = true;
                    if (!correct.Contains(false))
                    {
                        setButtons(true);
                    }
                }
                else
                {
                    UP5.ForeColor = Color.Red;
                    UT5.Text = ""; 
                    setButtons(false);
                    correct[4] = false;
                }
            }
        }

        private void UP6_TextChanged(object sender, EventArgs e)
        {
            if (UP6.Text == "")
            {
                UT6.Text = "";
                setButtons(false);
                correct[5] = false;
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
                    UP6.ForeColor = Color.Black;
                    int zeit = (int)Math.Round(Convert.ToDouble(UP6.Text) / 5);
                    UT6.Text = zeit.ToString();
                    correct[5] = true;
                    if (!correct.Contains(false))
                    {
                        setButtons(true);
                    }
                }
                else
                {
                    UP6.ForeColor = Color.Red;
                    UT6.Text = "";
                    setButtons(false);
                    correct[5] = false;
                }
            }
        }

        private void UP7_TextChanged(object sender, EventArgs e)
        {
            if (UP7.Text == "")
            {
                UT7.Text = "";
                setButtons(false);
                correct[6] = false;
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
                    UP7.ForeColor = Color.Black;
                    int zeit = (int)Math.Round(Convert.ToDouble(UP7.Text) / 5);
                    UT7.Text = zeit.ToString();
                    correct[6] = true;
                    if (!correct.Contains(false))
                    {
                        setButtons(true);
                    }
                }
                else
                {
                    UP7.ForeColor = Color.Red;
                    UT7.Text = "";
                    setButtons(false);
                    correct[6] = false;
                }
            }
        }

        private void UP8_TextChanged(object sender, EventArgs e)
        {
            if (UP8.Text == "")
            {
                UT8.Text = "";
                setButtons(false);
                correct[7] = false;
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
                    UP8.ForeColor = Color.Black;
                    int zeit = (int)Math.Round(Convert.ToDouble(UP8.Text) / 5);
                    UT8.Text = zeit.ToString();
                    correct[7] = true;
                    if (!correct.Contains(false))
                    {
                        setButtons(true);
                    }
                }
                else
                {
                    UP8.ForeColor = Color.Red;
                    UT8.Text = "";
                    setButtons(false);
                    correct[7] = false;
                }
            }
        }

        private void UP9_TextChanged(object sender, EventArgs e)
        {
            if (UP9.Text == "")
            {
                UT9.Text = "";
                setButtons(false);
                correct[8] = false;
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
                    UP9.ForeColor = Color.Black;
                    int zeit = (int)Math.Round(Convert.ToDouble(UP9.Text) / 5);
                    UT9.Text = zeit.ToString();
                    correct[8] = true;
                    if (!correct.Contains(false))
                    {
                        setButtons(true);
                    }
                }
                else
                {
                    UP9.ForeColor = Color.Red;
                    UT9.Text = ""; 
                    setButtons(false);
                    correct[8] = false;
                }
            }
        }

        private void UP10_TextChanged(object sender, EventArgs e)
        {
            if (UP10.Text == "")
            {
                UT10.Text = "";
                setButtons(false);
                correct[9] = false;
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
                    UP10.ForeColor = Color.Black;
                    int zeit = (int)Math.Round(Convert.ToDouble(UP10.Text) / 5);
                    UT10.Text = zeit.ToString();
                    correct[9] = true;
                    if (!correct.Contains(false))
                    {
                        setButtons(true);
                    }
                }
                else
                {
                    UP10.ForeColor = Color.Red;
                    UT10.Text = "";
                    setButtons(false);
                    correct[9] = false;
                }
            }
        }

        private void UP11_TextChanged(object sender, EventArgs e)
        {
            if (UP11.Text == "")
            {
                UT11.Text = "";
                setButtons(false);
                correct[10] = false;
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
                    UP11.ForeColor = Color.Black;
                    int zeit = (int)Math.Round(Convert.ToDouble(UP11.Text) / 5);
                    UT11.Text = zeit.ToString();
                    correct[10] = true;
                    if (!correct.Contains(false))
                    {
                        setButtons(true);
                    }
                }
                else
                {
                    UP11.ForeColor = Color.Red;
                    UT11.Text = ""; 
                    setButtons(false);
                    correct[10] = false;
                }
            }
        }

        private void UP12_TextChanged(object sender, EventArgs e)
        {
            if (UP12.Text == "")
            {
                UT12.Text = "";
                setButtons(false);
                correct[11] = false;
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
                    UP12.ForeColor = Color.Black;
                    int zeit = (int)Math.Round(Convert.ToDouble(UP12.Text) / 5);
                    UT12.Text = zeit.ToString();
                    correct[11] = true;
                    if (!correct.Contains(false))
                    {
                        setButtons(true);
                    }
                }
                else
                {
                    UP12.ForeColor = Color.Red;
                    UT12.Text = ""; 
                    setButtons(false);
                    correct[11] = false;
                }
            }
        }

        private void UP13_TextChanged(object sender, EventArgs e)
        {
            if (UP13.Text == "")
            {
                UT13.Text = "";
                setButtons(false);
                correct[12] = false;
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
                    UP13.ForeColor = Color.Black;
                    int zeit = (int)Math.Round(Convert.ToDouble(UP13.Text) / 5);
                    UT13.Text = zeit.ToString();
                    correct[12] = true;
                    if (!correct.Contains(false))
                    {
                        setButtons(true);
                    }
                }
                else
                {
                    UP13.ForeColor = Color.Red;
                    UT13.Text = "";
                    setButtons(false);
                    correct[12] = false;
                }
            }
        }

        private void UP14_TextChanged(object sender, EventArgs e)
        {
            if (UP14.Text == "")
            {
                UT14.Text = "";
                setButtons(false);
                correct[13] = false;
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
                    UP14.ForeColor = Color.Black;
                    int zeit = (int)Math.Round(Convert.ToDouble(UP14.Text) / 5);
                    UT14.Text = zeit.ToString();
                    correct[13] = true;
                    if (!correct.Contains(false))
                    {
                        setButtons(true);
                    }
                }
                else
                {
                    UP14.ForeColor = Color.Red;
                    UT14.Text = ""; 
                    setButtons(false);
                    correct[13] = false;
                }
            }
        }

        private void UP15_TextChanged(object sender, EventArgs e)
        {
            if (UP15.Text == "")
            {
                UT15.Text = "";
                setButtons(false);
                correct[14] = false;
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
                    UP15.ForeColor = Color.Black;
                    int zeit = (int)Math.Round(Convert.ToDouble(UP15.Text) / 5);
                    UT15.Text = zeit.ToString();
                    correct[14] = true;
                    if (!correct.Contains(false))
                    {
                        setButtons(true);
                    }
                }
                else
                {
                    UP15.ForeColor = Color.Red;
                    UT15.Text = ""; 
                    setButtons(false);
                    correct[14] = false;
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
                    correct[15] = true;
                    if (!correct.Contains(false))
                    {
                        setButtons(true);
                    }
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
                    setButtons(false);
                    correct[15] = false;
                }
                else
                {
                    if (pic_de.SizeMode == PictureBoxSizeMode.StretchImage)
                    {
                        MessageBox.Show("Es sind nur Werte von 1 bis 3 zulässig.");
                    }
                    else
                    {
                        MessageBox.Show("Only values ​​1 to 3 permitted.");
                    }
                    S1.Text = alt.ToString();
                    correct[15] = true;
                    if (!correct.Contains(false))
                    {
                        setButtons(true);
                    }
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
                    correct[16] = true;
                    if (!correct.Contains(false))
                    {
                        setButtons(true);
                    }
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
                    setButtons(false);
                    correct[16] = false;
                }
                else
                {
                    if (pic_de.SizeMode == PictureBoxSizeMode.StretchImage)
                    {
                        MessageBox.Show("Es sind nur Werte von 1 bis 3 zulässig.");
                    }
                    else
                    {
                        MessageBox.Show("Only values ​​1 to 3 permitted.");
                    }
                    S2.Text = alt.ToString();
                    correct[16] = true;
                    if (!correct.Contains(false))
                    {
                        setButtons(true);
                    }
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
                    correct[17] = true;
                    if (!correct.Contains(false))
                    {
                        setButtons(true);
                    }
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
                    setButtons(false);
                    correct[17] = false;
                }
                else
                {
                    if (pic_de.SizeMode == PictureBoxSizeMode.StretchImage)
                    {
                        MessageBox.Show("Es sind nur Werte von 1 bis 3 zulässig.");
                    }
                    else
                    {
                        MessageBox.Show("Only values ​​1 to 3 permitted.");
                    }
                    S3.Text = alt.ToString();
                    correct[17] = true;
                    if (!correct.Contains(false))
                    {
                        setButtons(true);
                    }
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
                    correct[18] = true;
                    if (!correct.Contains(false))
                    {
                        setButtons(true);
                    }
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
                    setButtons(false);
                    correct[18] = false;
                }
                else
                {
                    if (pic_de.SizeMode == PictureBoxSizeMode.StretchImage)
                    {
                        MessageBox.Show("Es sind nur Werte von 1 bis 3 zulässig.");
                    }
                    else
                    {
                        MessageBox.Show("Only values ​​1 to 3 permitted.");
                    }
                    S4.Text = alt.ToString();
                    correct[18] = true;
                    if (!correct.Contains(false))
                    {
                        setButtons(true);
                    }
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
                    correct[19] = true;
                    if (!correct.Contains(false))
                    {
                        setButtons(true);
                    }
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
                    setButtons(false);
                    correct[19] = false;
                }
                else
                {
                    if (pic_de.SizeMode == PictureBoxSizeMode.StretchImage)
                    {
                        MessageBox.Show("Es sind nur Werte von 1 bis 3 zulässig.");
                    }
                    else
                    {
                        MessageBox.Show("Only values ​​1 to 3 permitted.");
                    }
                    S5.Text = alt.ToString();
                    correct[19] = true;
                    if (!correct.Contains(false))
                    {
                        setButtons(true);
                    }
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
                    correct[20] = true;
                    if (!correct.Contains(false))
                    {
                        setButtons(true);
                    }
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
                    setButtons(false);
                    correct[20] = false;
                }
                else
                {
                    if (pic_de.SizeMode == PictureBoxSizeMode.StretchImage)
                    {
                        MessageBox.Show("Es sind nur Werte von 1 bis 3 zulässig.");
                    }
                    else
                    {
                        MessageBox.Show("Only values ​​1 to 3 permitted.");
                    }
                    S6.Text = alt.ToString();
                    correct[20] = true;
                    if (!correct.Contains(false))
                    {
                        setButtons(true);
                    }
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
                    correct[21] = true;
                    if (!correct.Contains(false))
                    {
                        setButtons(true);
                    }
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
                    setButtons(false);
                    correct[21] = false;
                }
                else
                {
                    if (pic_de.SizeMode == PictureBoxSizeMode.StretchImage)
                    {
                        MessageBox.Show("Es sind nur Werte von 1 bis 3 zulässig.");
                    }
                    else
                    {
                        MessageBox.Show("Only values ​​1 to 3 permitted.");
                    }
                    S7.Text = alt.ToString();
                    correct[21] = true;
                    if (!correct.Contains(false))
                    {
                        setButtons(true);
                    }
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
                    correct[22] = true;
                    if (!correct.Contains(false))
                    {
                        setButtons(true);
                    }
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
                    setButtons(false);
                    correct[22] = false;
                }
                else
                {
                    if (pic_de.SizeMode == PictureBoxSizeMode.StretchImage)
                    {
                        MessageBox.Show("Es sind nur Werte von 1 bis 3 zulässig.");
                    }
                    else
                    {
                        MessageBox.Show("Only values ​​1 to 3 permitted.");
                    }
                    S8.Text = alt.ToString();
                    correct[22] = true;
                    if (!correct.Contains(false))
                    {
                        setButtons(true);
                    }
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
                    correct[23] = true;
                    if (!correct.Contains(false))
                    {
                        setButtons(true);
                    }
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
                    setButtons(false);
                    correct[23] = false;
                }
                else
                {
                    if (pic_de.SizeMode == PictureBoxSizeMode.StretchImage)
                    {
                        MessageBox.Show("Es sind nur Werte von 1 bis 3 zulässig.");
                    }
                    else
                    {
                        MessageBox.Show("Only values ​​1 to 3 permitted.");
                    }
                    S9.Text = alt.ToString();
                    correct[23] = true;
                    if (!correct.Contains(false))
                    {
                        setButtons(true);
                    }
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
                    correct[24] = true;
                    if (!correct.Contains(false))
                    {
                        setButtons(true);
                    }
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
                    setButtons(false);
                    correct[24] = false;
                }
                else
                {
                    if (pic_de.SizeMode == PictureBoxSizeMode.StretchImage)
                    {
                        MessageBox.Show("Es sind nur Werte von 1 bis 3 zulässig.");
                    }
                    else
                    {
                        MessageBox.Show("Only values ​​1 to 3 permitted.");
                    }
                    S10.Text = alt.ToString();
                    correct[24] = true;
                    if (!correct.Contains(false))
                    {
                        setButtons(true);
                    }
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
                    correct[25] = true;
                    if (!correct.Contains(false))
                    {
                        setButtons(true);
                    }
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
                    setButtons(false);
                    correct[25] = false;
                }
                else
                {
                    if (pic_de.SizeMode == PictureBoxSizeMode.StretchImage)
                    {
                        MessageBox.Show("Es sind nur Werte von 1 bis 3 zulässig.");
                    }
                    else
                    {
                        MessageBox.Show("Only values ​​1 to 3 permitted.");
                    }
                    S11.Text = alt.ToString();
                    correct[25] = true;
                    if (!correct.Contains(false))
                    {
                        setButtons(true);
                    }
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
                    correct[26] = true;
                    if (!correct.Contains(false))
                    {
                        setButtons(true);
                    }
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
                    setButtons(false);
                    correct[26] = false;
                }
                else
                {
                    if (pic_de.SizeMode == PictureBoxSizeMode.StretchImage)
                    {
                        MessageBox.Show("Es sind nur Werte von 1 bis 3 zulässig.");
                    }
                    else
                    {
                        MessageBox.Show("Only values ​​1 to 3 permitted.");
                    }
                    S12.Text = alt.ToString();
                    correct[26] = true;
                    if (!correct.Contains(false))
                    {
                        setButtons(true);
                    }
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
                    correct[27] = true;
                    if (!correct.Contains(false))
                    {
                        setButtons(true);
                    }
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
                    setButtons(false);
                    correct[27] = false;
                }
                else
                {
                    if (pic_de.SizeMode == PictureBoxSizeMode.StretchImage)
                    {
                        MessageBox.Show("Es sind nur Werte von 1 bis 3 zulässig.");
                    }
                    else
                    {
                        MessageBox.Show("Only values ​​1 to 3 permitted.");
                    }
                    S13.Text = alt.ToString();
                    correct[27] = true;
                    if (!correct.Contains(false))
                    {
                        setButtons(true);
                    }
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
                    correct[28] = true;
                    if (!correct.Contains(false))
                    {
                        setButtons(true);
                    }
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
                    setButtons(false);
                    correct[28] = false;
                }
                else
                {
                    if (pic_de.SizeMode == PictureBoxSizeMode.StretchImage)
                    {
                        MessageBox.Show("Es sind nur Werte von 1 bis 3 zulässig.");
                    }
                    else
                    {
                        MessageBox.Show("Only values ​​1 to 3 permitted.");
                    }
                    S14.Text = alt.ToString();
                    correct[28] = true;
                    if (!correct.Contains(false))
                    {
                        setButtons(true);
                    }
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
                    correct[29] = true;
                    if (!correct.Contains(false))
                    {
                        setButtons(true);
                    }
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
                    setButtons(false);
                    correct[29] = false;
                }
                else
                {
                    if (pic_de.SizeMode == PictureBoxSizeMode.StretchImage)
                    {
                        MessageBox.Show("Es sind nur Werte von 1 bis 3 zulässig.");
                    }
                    else
                    {
                        MessageBox.Show("Only values ​​1 to 3 permitted.");
                    }
                    S15.Text = alt.ToString();
                    correct[29] = true;
                    if (!correct.Contains(false))
                    {
                        setButtons(true);
                    }
                }
            }
        }
        public void sprachen()
        {
            if (pic_en.SizeMode == PictureBoxSizeMode.StretchImage)
            {
                //EN Brotkrumenleiste
                lbl_Startseite.Text = (Sprachen.EN_LBL_STARTSEITE);
                lbl_Sicherheitsbestand.Text = (Sprachen.EN_LBL_SICHERHEITSBESTAND);
                lbl_Produktion.Text = (Sprachen.EN_LBL_PRODUKTION);
                lbl_Produktionsreihenfolge.Text = (Sprachen.EN_LBL_PRODUKTIONSREIHENFOLGE);
                lbl_Kapazitaetsplan.Text = (Sprachen.EN_LBL_KAPATITAETSPLAN);
                lbl_Kaufteiledisposition.Text = (Sprachen.EN_LBL_KAUFTEILEDISPOSITION);
                lbl_Ergebnis.Text = (Sprachen.EN_LBL_ERGEBNIS);

                //EN Buttons
                continue_btn.Text = (Sprachen.EN_BTN_CONTINUE);
                back_btn.Text = (Sprachen.EN_BTN_BACK);
                default_btn.Text = (Sprachen.EN_BTN_DEFAULT);

                //EN Groupboxen
                groupBox1.Text = (Sprachen.EN_KP_GROUPBOX1);

                //EN Labels
                label1.Text = (Sprachen.EN_LBL_KD_INFO);
                KapBedarf.Text = (Sprachen.EN_LBL_KD_KBEDARF);
                UeberstundenPeriode.Text = (Sprachen.EN_LBL_KD_UEBERSTUNDENP);
                UeberstundenTag.Text = (Sprachen.EN_LBL_KD_UEBERSTUNDENT);
                Schichten.Text = (Sprachen.EN_LBL_KD_SCHICHTEN);

                //EN Tooltip
                System.Windows.Forms.ToolTip ToolTipP = new System.Windows.Forms.ToolTip();
                ToolTipP.SetToolTip(this.pictureBox7, Sprachen.EN_KP_INFO);

            }
            else
            {
                //DE Brotkrumenleiste
                lbl_Startseite.Text = (Sprachen.DE_LBL_STARTSEITE);
                lbl_Sicherheitsbestand.Text = (Sprachen.DE_LBL_SICHERHEITSBESTAND);
                lbl_Produktionsreihenfolge.Text = (Sprachen.DE_LBL_PRODUKTIONSREIHENFOLGE);
                lbl_Produktion.Text = (Sprachen.DE_LBL_PRODUKTION);
                lbl_Kapazitaetsplan.Text = (Sprachen.DE_LBL_KAPATITAETSPLAN);
                lbl_Kaufteiledisposition.Text = (Sprachen.DE_LBL_KAUFTEILEDISPOSITION);
                lbl_Ergebnis.Text = (Sprachen.DE_LBL_ERGEBNIS);

                //DE Buttons
                continue_btn.Text = (Sprachen.DE_BTN_CONTINUE);
                back_btn.Text = (Sprachen.DE_BTN_BACK);
                default_btn.Text = (Sprachen.DE_BTN_DEFAULT);

                //DE Groupboxen
                groupBox1.Text = (Sprachen.DE_KP_GROUPBOX1);

                //DE Labels
                label1.Text = (Sprachen.DE_LBL_KD_INFO);
                KapBedarf.Text = (Sprachen.DE_LBL_KD_KBEDARF);
                UeberstundenPeriode.Text = (Sprachen.DE_LBL_KD_UEBERSTUNDENP);
                UeberstundenTag.Text = (Sprachen.DE_LBL_KD_UEBERSTUNDENT);
                Schichten.Text = (Sprachen.DE_LBL_KD_SCHICHTEN);

                //EN Tooltip
                System.Windows.Forms.ToolTip ToolTipP = new System.Windows.Forms.ToolTip();
                ToolTipP.SetToolTip(this.pictureBox7, Sprachen.DE_KP_INFO);
            }
        }
        private void pic_en_Click(object sender, EventArgs e){
           pic_en.SizeMode = PictureBoxSizeMode.StretchImage;
           pic_de.SizeMode = PictureBoxSizeMode.Normal;
           sprachen();           
        }

        private void pic_de_Click(object sender, EventArgs e)
        {
            pic_de.SizeMode = PictureBoxSizeMode.StretchImage;
            pic_en.SizeMode = PictureBoxSizeMode.Normal;
            sprachen();  
        }

        private void back_btn_Click(object sender, EventArgs e)
        {
            // Datenweitergabe

            // Werte aus TextBoxen in kapazitaet auslesen
            for (int i = 0; i < kapazitaet.GetLength(0); ++i)
            {
                int k = i + 1;
                kapazitaet[i, 0] = k;
                kapazitaet[i, 1] = Convert.ToInt32(this.Controls.Find("K" + k.ToString(), true)[0].Text);
                kapazitaet[i, 2] = Convert.ToInt32(this.Controls.Find("UP" + k.ToString(), true)[0].Text);
                kapazitaet[i, 3] = Convert.ToInt32(this.Controls.Find("UT" + k.ToString(), true)[0].Text);
                kapazitaet[i, 4] = Convert.ToInt32(this.Controls.Find("S" + k.ToString(), true)[0].Text);
            }

            this.Controls.Clear();
            UserControl prodreihenfolge = new Produktionsreihenfolge(aktPeriode, auftraege, direktverkaeufe,
                sicherheitsbest, produktion, produktionProg, prodReihenfolge, kapazitaet, kaufauftraege);
            this.Controls.Add(prodreihenfolge);
        }

        private void lbl_Startseite_Click(object sender, EventArgs e)
        {
            // Datenweitergabe

            // Werte aus TextBoxen in kapazitaet auslesen
            for (int i = 0; i < kapazitaet.GetLength(0); ++i)
            {
                int k = i + 1;
                kapazitaet[i, 0] = k;
                kapazitaet[i, 1] = Convert.ToInt32(this.Controls.Find("K" + k.ToString(), true)[0].Text);
                kapazitaet[i, 2] = Convert.ToInt32(this.Controls.Find("UP" + k.ToString(), true)[0].Text);
                kapazitaet[i, 3] = Convert.ToInt32(this.Controls.Find("UT" + k.ToString(), true)[0].Text);
                kapazitaet[i, 4] = Convert.ToInt32(this.Controls.Find("S" + k.ToString(), true)[0].Text);
            }

            this.Controls.Clear();
            UserControl import = new ImportPrognose(aktPeriode, auftraege, direktverkaeufe,
                sicherheitsbest, produktion, produktionProg, prodReihenfolge, kapazitaet, kaufauftraege);
            this.Controls.Add(import);
        }

        private void lbl_Sicherheitsbestand_Click(object sender, EventArgs e)
        {
            // Datenweitergabe

            // Werte aus TextBoxen in kapazitaet auslesen
            for (int i = 0; i < kapazitaet.GetLength(0); ++i)
            {
                int k = i + 1;
                kapazitaet[i, 0] = k;
                kapazitaet[i, 1] = Convert.ToInt32(this.Controls.Find("K" + k.ToString(), true)[0].Text);
                kapazitaet[i, 2] = Convert.ToInt32(this.Controls.Find("UP" + k.ToString(), true)[0].Text);
                kapazitaet[i, 3] = Convert.ToInt32(this.Controls.Find("UT" + k.ToString(), true)[0].Text);
                kapazitaet[i, 4] = Convert.ToInt32(this.Controls.Find("S" + k.ToString(), true)[0].Text);
            }

            this.Controls.Clear();
            UserControl sicherheit = new Sicherheitsbestand(aktPeriode, auftraege, direktverkaeufe,
                sicherheitsbest, produktion, produktionProg, prodReihenfolge, kapazitaet, kaufauftraege);
            this.Controls.Add(sicherheit);
        }

        private void lbl_Produktion_Click(object sender, EventArgs e)
        {
            // Datenweitergabe

            // Werte aus TextBoxen in kapazitaet auslesen
            for (int i = 0; i < kapazitaet.GetLength(0); ++i)
            {
                int k = i + 1;
                kapazitaet[i, 0] = k;
                kapazitaet[i, 1] = Convert.ToInt32(this.Controls.Find("K" + k.ToString(), true)[0].Text);
                kapazitaet[i, 2] = Convert.ToInt32(this.Controls.Find("UP" + k.ToString(), true)[0].Text);
                kapazitaet[i, 3] = Convert.ToInt32(this.Controls.Find("UT" + k.ToString(), true)[0].Text);
                kapazitaet[i, 4] = Convert.ToInt32(this.Controls.Find("S" + k.ToString(), true)[0].Text);
            }

            this.Controls.Clear();
            UserControl prod = new Produktion(aktPeriode, auftraege, direktverkaeufe,
                sicherheitsbest, produktion, produktionProg, prodReihenfolge, kapazitaet, kaufauftraege);
            this.Controls.Add(prod);
        }

        private void lbl_Produktionsreihenfolge_Click(object sender, EventArgs e)
        {
            // Datenweitergabe

            // Werte aus TextBoxen in kapazitaet auslesen
            for (int i = 0; i < kapazitaet.GetLength(0); ++i)
            {
                int k = i + 1;
                kapazitaet[i, 0] = k;
                kapazitaet[i, 1] = Convert.ToInt32(this.Controls.Find("K" + k.ToString(), true)[0].Text);
                kapazitaet[i, 2] = Convert.ToInt32(this.Controls.Find("UP" + k.ToString(), true)[0].Text);
                kapazitaet[i, 3] = Convert.ToInt32(this.Controls.Find("UT" + k.ToString(), true)[0].Text);
                kapazitaet[i, 4] = Convert.ToInt32(this.Controls.Find("S" + k.ToString(), true)[0].Text);
            }

            this.Controls.Clear();
            UserControl prodreihenfolge = new Produktionsreihenfolge(aktPeriode, auftraege, direktverkaeufe,
                sicherheitsbest, produktion, produktionProg, prodReihenfolge, kapazitaet, kaufauftraege);
            this.Controls.Add(prodreihenfolge);
        }

        private void lbl_Kaufteiledisposition_Click(object sender, EventArgs e)
        {
            // Datenweitergabe

            // Werte aus TextBoxen in kapazitaet auslesen
            for (int i = 0; i < kapazitaet.GetLength(0); ++i)
            {
                int k = i + 1;
                kapazitaet[i, 0] = k;
                kapazitaet[i, 1] = Convert.ToInt32(this.Controls.Find("K" + k.ToString(), true)[0].Text);
                kapazitaet[i, 2] = Convert.ToInt32(this.Controls.Find("UP" + k.ToString(), true)[0].Text);
                kapazitaet[i, 3] = Convert.ToInt32(this.Controls.Find("UT" + k.ToString(), true)[0].Text);
                kapazitaet[i, 4] = Convert.ToInt32(this.Controls.Find("S" + k.ToString(), true)[0].Text);
            }

            this.Controls.Clear();
            UserControl kaufteile = new Kaufteildisposition(aktPeriode, auftraege, direktverkaeufe,
                sicherheitsbest, produktion, produktionProg, prodReihenfolge, kapazitaet, kaufauftraege);
            this.Controls.Add(kaufteile);
        }

    }
}
