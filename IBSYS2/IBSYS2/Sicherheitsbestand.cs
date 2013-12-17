using System;
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
using System.Resources;


namespace IBSYS2
{
    public partial class Sicherheitsbestand : UserControl
    {
        private OleDbConnection myconn;
        private char[] digits = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
        bool tP1 = false, tP2 = false, tP3 = false, tE1 = false, tE2 = false, tE3 = false, tE4 = false, tE5 = false, tE6 = false, tE7 = false, tE8 = false, tE9 = false, tE10 = false, tE11 = false, tE12 = false, tE13 = false, tE14 = false, tE15 = false, tE16 = false, tE17 = false, tE18 = false, tE19 = false, tE20 = false, tE21 = false, tE22 = false, tE23 = false, tE24 = false, tE25 = false, tE26 = false, tE27 = false, tE28 = false, tE29 = false, tE30 = false, tE31 = false, tE32 = false, tE33 = false;
        private String sprache = "de";

        // Datenweitergabe:
        int aktPeriode;
        int[] auftraege = new int[12];
        double[,] direktverkaeufe = new double[3, 4];
        int[,] sicherheitsbest = new int[30, 5];
        int[,] produktion = new int[30, 2];
        int[,] produktionProg = new int[3, 5];
        int[,] prodReihenfolge = new int[30, 2];
        int[,] kapazitaet = new int[15, 5];
        int[,] kaufauftraege = new int[29, 6];

        public Sicherheitsbestand()
        {
            InitializeComponent();
            continue_btn.Enabled = true;
            string databasename = @"Provider=Microsoft.ACE.OLEDB.12.0; Data Source=IBSYS_DB.accdb";
            myconn = new OleDbConnection(databasename);
            if (pic_en.SizeMode != PictureBoxSizeMode.StretchImage)
            {
                System.Windows.Forms.ToolTip ToolTipDE = new System.Windows.Forms.ToolTip();
                ToolTipDE.SetToolTip(this.infoP, Sprachen.DE_INFOP);
                ToolTipDE.SetToolTip(this.infoE, Sprachen.DE_INFOE);
            }
            else
            {
                System.Windows.Forms.ToolTip ToolTipEN = new System.Windows.Forms.ToolTip();
                ToolTipEN.SetToolTip(this.infoP, Sprachen.EN_INFOP);
                ToolTipEN.SetToolTip(this.infoE, Sprachen.EN_INFOE);
            }
            textfeldSperren();
            
            Ausgabe_P1.Enabled = false;
            Ausgabe_P2.Enabled = false;
            Ausgabe_P3.Enabled = false;
            continue_btn.Enabled = false;
            eteileberechnen_btn.Enabled = false;
        }

        public Sicherheitsbestand(int aktPeriode, int[] auftraege, double[,] direktverkaeufe, int[,] sicherheitsbest,
            int[,] produktion, int[,] produktionProg, int[,] prodReihenfolge, int[,] kapazitaet, int[,] kaufauftraege,
            String sprache)
        {
            this.sprache = sprache;
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
            sprachen();
            continue_btn.Enabled = true;
            string databasename = @"Provider=Microsoft.ACE.OLEDB.12.0; Data Source=IBSYS_DB.accdb";
            myconn = new OleDbConnection(databasename);
            if (pic_en.SizeMode != PictureBoxSizeMode.StretchImage)
            {
                System.Windows.Forms.ToolTip ToolTipDE = new System.Windows.Forms.ToolTip();
                ToolTipDE.SetToolTip(this.infoP, Sprachen.DE_INFOP);
                ToolTipDE.SetToolTip(this.infoE, Sprachen.DE_INFOE);
            }
            else
            {
                System.Windows.Forms.ToolTip ToolTipEN = new System.Windows.Forms.ToolTip();
                ToolTipEN.SetToolTip(this.infoP, Sprachen.EN_INFOP);
                ToolTipEN.SetToolTip(this.infoE, Sprachen.EN_INFOE);
            }
            textfeldSperren();

            Ausgabe_P1.Enabled = false;
            Ausgabe_P2.Enabled = false;
            Ausgabe_P3.Enabled = false;
            continue_btn.Enabled = false;
            eteileberechnen_btn.Enabled = false;

            Boolean bereitsBerechnet = false;
            for (int i = 0; i < sicherheitsbest.GetLength(0); i++)
            {
                if (sicherheitsbest[i, 1] > 0)
                {
                    bereitsBerechnet = true;
                    break;
                }
            }
            // wenn bereits Werte vorhanden sind, Felder fuellen
            // Kapbedarf trotzdem nochmal berechnen
            if (bereitsBerechnet == true)
            {
                // TextBoxen fuellen
                Eingabe_P1.Text = sicherheitsbest[0, 1].ToString();
                Eingabe_P2.Text = sicherheitsbest[1, 1].ToString();
                Eingabe_P3.Text = sicherheitsbest[2, 1].ToString();
                E041.Text = sicherheitsbest[3, 1].ToString();
                E052.Text = sicherheitsbest[4, 1].ToString();
                E063.Text = sicherheitsbest[5, 1].ToString();
                E071.Text = sicherheitsbest[6, 1].ToString();
                E082.Text = sicherheitsbest[7, 1].ToString();
                E093.Text = sicherheitsbest[8, 1].ToString();
                E101.Text = sicherheitsbest[9, 1].ToString();
                E112.Text = sicherheitsbest[10, 1].ToString();
                E123.Text = sicherheitsbest[11, 1].ToString();
                E131.Text = sicherheitsbest[12, 1].ToString();
                E142.Text = sicherheitsbest[13, 1].ToString();
                E153.Text = sicherheitsbest[14, 1].ToString();
                // E16
                E161.Text = sicherheitsbest[15, 2].ToString();
                E162.Text = sicherheitsbest[15, 3].ToString();
                E163.Text = sicherheitsbest[15, 4].ToString();
                // E17
                E171.Text = sicherheitsbest[16, 2].ToString();
                E172.Text = sicherheitsbest[16, 3].ToString();
                E173.Text = sicherheitsbest[16, 4].ToString();
                //
                E181.Text = sicherheitsbest[17, 1].ToString();
                E192.Text = sicherheitsbest[18, 1].ToString();
                E203.Text = sicherheitsbest[19, 1].ToString();
                // E26
                E261.Text = sicherheitsbest[20, 2].ToString();
                E262.Text = sicherheitsbest[20, 3].ToString();
                E263.Text = sicherheitsbest[20, 4].ToString();
                //
                E293.Text = sicherheitsbest[21, 1].ToString();
                E303.Text = sicherheitsbest[22, 1].ToString();
                E313.Text = sicherheitsbest[23, 1].ToString();
                E491.Text = sicherheitsbest[24, 1].ToString();
                E501.Text = sicherheitsbest[25, 1].ToString();
                E511.Text = sicherheitsbest[26, 1].ToString();
                E542.Text = sicherheitsbest[27, 1].ToString();
                E552.Text = sicherheitsbest[28, 1].ToString();
                E562.Text = sicherheitsbest[29, 1].ToString();

                //alle E-Teile durchlaufen um herauszufinden, ob hier schon einmal etwas 
                // kalkuliert wurde (wenn ja, buttons auf true setzen)
                // es koennten ja auch nur Werte in P1, P2 und P3 enthalten sein
                for (int i = 1; i <= 562; i++) // nicht schoen, geht aber
                {
                    Control[] controls = this.Controls.Find("E" + i.ToString(), true);
                    if (controls.Length > 0)
                    {
                        String wert = controls[0].Text;
                        if (wert != "" & wert != "0")
                        {
                            // Werte auf der rechten Seite kalkulieren
                            double gLagerbestandP1 = Convert.ToDouble(Eingabe_P1.Text);
                            double gLagerbestandP2 = Convert.ToDouble(Eingabe_P2.Text);
                            double gLagerbestandP3 = Convert.ToDouble(Eingabe_P3.Text);
                            int mengeP1 = auftraege[0] + Convert.ToInt32(direktverkaeufe[0, 1]); // Direktverkauefe auf normale auftraege schlagen
                            int mengeP2 = auftraege[1] + Convert.ToInt32(direktverkaeufe[1, 1]);
                            int mengeP3 = auftraege[2] + Convert.ToInt32(direktverkaeufe[2, 1]);
                            double sicherheitsbestandP1 = sicherheitsbestandBerechnen(mengeP1, gLagerbestandP1, "1");
                            Ausgabe_P1.Text = Convert.ToString(sicherheitsbestandP1);
                            double sicherheitsbestandP2 = sicherheitsbestandBerechnen(mengeP2, gLagerbestandP2, "2");
                            Ausgabe_P2.Text = Convert.ToString(sicherheitsbestandP2);
                            double sicherheitsbestandP3 = sicherheitsbestandBerechnen(mengeP3, gLagerbestandP3, "3");
                            Ausgabe_P3.Text = Convert.ToString(sicherheitsbestandP3);

                            setButtons(true);
                            textfeldFreigeben();
                            break;
                        }
                    }
                }


            }
        }

        public void setButtons(Boolean b)
        {
            btn_back.Enabled = b;
            continue_btn.Enabled = b;
            lbl_Startseite.Enabled = b;
            lbl_Auftraege.Enabled = b;
        }

        private void eteileberechnen_btn_Click(object sender, EventArgs e)
        {
            if (Eingabe_P1.Text == "0" | Eingabe_P2.Text == "0" | Eingabe_P3.Text == "0")
            {
                valueZero();
                DialogResult dialogResult;
                if (pic_de.SizeMode == PictureBoxSizeMode.StretchImage)
                {
                    dialogResult = MessageBox.Show("In Ihren Eingaben sind noch einige Felder mit der Eingabe 0. Ist dies gewollt?", "Wollen Sie fortfahren?", MessageBoxButtons.YesNo);
                }
                else
                {
                    dialogResult = MessageBox.Show("In your entries are still some fields with the input 0. Is this correct?", "Do you want to continue?", MessageBoxButtons.YesNo);
                }
                if (dialogResult == DialogResult.Yes)
                {
                    berechnen();
                }
            } 
            else 
            {
                berechnen();
            }
        }

        public void berechnen()
        {
            double gLagerbestandP1 = Convert.ToDouble(Eingabe_P1.Text);
            double gLagerbestandP2 = Convert.ToDouble(Eingabe_P2.Text);
            double gLagerbestandP3 = Convert.ToDouble(Eingabe_P3.Text);
            int mengeP1 = auftraege[0] + Convert.ToInt32(direktverkaeufe[0, 1]); // Direktverkauefe auf normale auftraege schlagen
            int mengeP2 = auftraege[1] + Convert.ToInt32(direktverkaeufe[1, 1]);
            int mengeP3 = auftraege[2] + Convert.ToInt32(direktverkaeufe[2, 1]);
            double sicherheitsbestandP1 = sicherheitsbestandBerechnen(mengeP1, gLagerbestandP1, "1");
            Ausgabe_P1.Text = Convert.ToString(sicherheitsbestandP1);
            double sicherheitsbestandP2 = sicherheitsbestandBerechnen(mengeP2, gLagerbestandP2, "2");
            Ausgabe_P2.Text = Convert.ToString(sicherheitsbestandP2);
            double sicherheitsbestandP3 = sicherheitsbestandBerechnen(mengeP3, gLagerbestandP3, "3");
            Ausgabe_P3.Text = Convert.ToString(sicherheitsbestandP3);


            double gLE26P1 = geplanterLagerbestand(gLagerbestandP1, 100);
            E261.Text = Convert.ToString(gLE26P1);
            double gLE51P1 = geplanterLagerbestand(gLagerbestandP1, 100);
            E511.Text = Convert.ToString(gLE51P1);
            double gLE16P1 = geplanterLagerbestand(gLagerbestandP1, 100);
            E161.Text = Convert.ToString(gLE16P1);
            double gLE17P1 = geplanterLagerbestand(gLagerbestandP1, 100);
            E171.Text = Convert.ToString(gLE17P1);
            double gLE50P1 = geplanterLagerbestand(gLagerbestandP1, 100);
            E501.Text = Convert.ToString(gLE50P1);
            double gLE4P1 = geplanterLagerbestand(gLagerbestandP1, 100);
            E041.Text = Convert.ToString(gLE4P1);
            double gLE10P1 = geplanterLagerbestand(gLagerbestandP1, 100);
            E101.Text = Convert.ToString(gLE10P1);
            double gLE49P1 = geplanterLagerbestand(gLagerbestandP1, 100);
            E491.Text = Convert.ToString(gLE49P1);
            double gLE7P1 = geplanterLagerbestand(gLagerbestandP1, 100);
            E071.Text = Convert.ToString(gLE7P1);
            double gLE13P1 = geplanterLagerbestand(gLagerbestandP1, 100);
            E131.Text = Convert.ToString(gLE13P1);
            double gLE18P1 = geplanterLagerbestand(gLagerbestandP1, 100);
            E181.Text = Convert.ToString(gLE18P1);

            double gLE26P2 = geplanterLagerbestand(gLagerbestandP2, 100);
            E262.Text = Convert.ToString(gLE26P2);
            double gLE56P2 = geplanterLagerbestand(gLagerbestandP2, 100);
            E562.Text = Convert.ToString(gLE56P2);
            double gLE16P2 = geplanterLagerbestand(gLagerbestandP2, 100);
            E162.Text = Convert.ToString(gLE16P2);
            double gLE17P2 = geplanterLagerbestand(gLagerbestandP2, 100);
            E172.Text = Convert.ToString(gLE17P2);
            double gLE55P2 = geplanterLagerbestand(gLagerbestandP2, 100);
            E552.Text = Convert.ToString(gLE55P2);
            double gLE5P2 = geplanterLagerbestand(gLagerbestandP2, 100);
            E052.Text = Convert.ToString(gLE5P2);
            double gLE11P2 = geplanterLagerbestand(gLagerbestandP2, 100);
            E112.Text = Convert.ToString(gLE11P2);
            double gLE54P2 = geplanterLagerbestand(gLagerbestandP2, 100);
            E542.Text = Convert.ToString(gLE54P2);
            double gLE8P2 = geplanterLagerbestand(gLagerbestandP2, 100);
            E082.Text = Convert.ToString(gLE8P2);
            double gLE14P2 = geplanterLagerbestand(gLagerbestandP2, 100);
            E142.Text = Convert.ToString(gLE14P2);
            double gLE19P2 = geplanterLagerbestand(gLagerbestandP2, 100);
            E192.Text = Convert.ToString(gLE19P2);

            double gLE26P3 = geplanterLagerbestand(gLagerbestandP3, 100);
            E263.Text = Convert.ToString(gLE26P3);
            double gLE31P3 = geplanterLagerbestand(gLagerbestandP3, 100);
            E313.Text = Convert.ToString(gLE31P3);
            double gLE16P3 = geplanterLagerbestand(gLagerbestandP3, 100);
            E163.Text = Convert.ToString(gLE16P3);
            double gLE17P3 = geplanterLagerbestand(gLagerbestandP3, 100);
            E173.Text = Convert.ToString(gLE17P3);
            double gLE30P3 = geplanterLagerbestand(gLagerbestandP3, 100);
            E303.Text = Convert.ToString(gLE30P3);
            double gLE6P3 = geplanterLagerbestand(gLagerbestandP3, 100);
            E063.Text = Convert.ToString(gLE6P3);
            double gLE12P3 = geplanterLagerbestand(gLagerbestandP3, 100);
            E123.Text = Convert.ToString(gLE12P3);
            double gLE29P3 = geplanterLagerbestand(gLagerbestandP3, 100);
            E293.Text = Convert.ToString(gLE29P3);
            double gLE9P3 = geplanterLagerbestand(gLagerbestandP3, 100);
            E093.Text = Convert.ToString(gLE9P3);
            double gLE15P3 = geplanterLagerbestand(gLagerbestandP3, 100);
            E153.Text = Convert.ToString(gLE15P3);
            double gLE20P3 = geplanterLagerbestand(gLagerbestandP3, 100);
            E203.Text = Convert.ToString(gLE20P3);

            textfeldFreigeben();

            continue_btn.Enabled = true;
        }
        public double geplanterLagerbestand(double sicherheitsbestand, int ver)
        {
            double geplanterLagerbestand = 0.0;
            geplanterLagerbestand = (sicherheitsbestand / 100) * ver;
            return geplanterLagerbestand;
        }

        public double sicherheitsbestandBerechnen(int prognose, double gLagerbestand, string teilenummer_FK)
        {
            double sicherheitsbestand = 0;
            int lBestand = datenHolen(teilenummer_FK, "Bestand", "Teilenummer_FK", "Lager");
            int wMatMenge = datenHolen(teilenummer_FK, "Menge", "Fehlteil_Teilenummer_FK", "Warteliste_Material");
            int wArbMenge = datenHolen(teilenummer_FK, "Menge", "Teilenummer_FK", "Warteliste_Arbeitsplatz");
            //Sicherheitsbestand = Prognose + geplanterLagerbestand - Lagerbestand - MengeWarteliste_Material - Menge Warteliste_Arbeitsplatz
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

        public void textfeldSperren()
        {
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

        private void Eingabe_P2_TextChanged(object sender, EventArgs e)
        {
            if (Eingabe_P2.Text == "")
            {
                eteileberechnen_btn.Enabled = false;
                tP2 = false;
            }
            else
            {
                Eingabe_P2.ForeColor = Color.Black;
                bool okay = true;
                //neuer Text darf nur Zeichen aus der Liste digits (in der Klasse deklariert)
                foreach (char c in Eingabe_P2.Text.ToCharArray())
                {
                    //sobald es ein unpassendes Zeichen gibt, aufhoeren und Fehlermeldung ausgeben
                    if (!digits.Contains<char>(c))
                    {
                        Eingabe_P2.ForeColor = Color.Red;
                        okay = false;
                        tP2 = false;
                        eteileberechnen_btn.Enabled = false;
                        break;
                    }
                }
                if (okay == true)
                {
                    Eingabe_P2.ForeColor = Color.Black;
                    tP2 = true;
                    if (tP1 & tP2 & tP3)
                    {
                        eteileberechnen_btn.Enabled = true;
                    }
                    else
                    {
                        eteileberechnen_btn.Enabled = false;
                    }
                }
            }
        }

        private void Eingabe_P1_TextChanged(object sender, EventArgs e)
        {
            if (Eingabe_P1.Text == "")
            {
                eteileberechnen_btn.Enabled = false;
                tP1 = false;
            }
            else
            {
                Eingabe_P1.ForeColor = Color.Black;
                bool okay = true;
                //neuer Text darf nur Zeichen aus der Liste digits (in der Klasse deklariert)
                foreach (char c in Eingabe_P1.Text.ToCharArray())
                {
                    //sobald es ein unpassendes Zeichen gibt, aufhoeren und Fehlermeldung ausgeben
                    if (!digits.Contains<char>(c))
                    {
                        Eingabe_P1.ForeColor = Color.Red;
                        okay = false;
                        tP1 = false;
                        eteileberechnen_btn.Enabled = false;
                        break;
                    }
                }
                if (okay == true)
                {
                    Eingabe_P1.ForeColor = Color.Black;
                    tP1 = true;
                    if (tP1 & tP2 & tP3)
                    {
                        eteileberechnen_btn.Enabled = true;
                    }
                    else
                    {
                        eteileberechnen_btn.Enabled = false;
                    }
                }
            }
        }

        private void Eingabe_P3_TextChanged_1(object sender, EventArgs e)
        {
            if (Eingabe_P3.Text == "")
            {
                eteileberechnen_btn.Enabled = false;
                tP3 = false;
            }
            else
            {
                Eingabe_P3.ForeColor = Color.Black;
                bool okay = true;
                //neuer Text darf nur Zeichen aus der Liste digits (in der Klasse deklariert)
                foreach (char c in Eingabe_P3.Text.ToCharArray())
                {
                    //sobald es ein unpassendes Zeichen gibt, aufhoeren und Fehlermeldung ausgeben
                    if (!digits.Contains<char>(c))
                    {
                        Eingabe_P3.ForeColor = Color.Red;
                        okay = false;
                        tP3 = false;
                        eteileberechnen_btn.Enabled = false;
                        break;
                    }
                }
                if (okay == true)
                {
                    Eingabe_P3.ForeColor = Color.Black;
                    tP3 = true;
                    if (tP1 & tP2 & tP3)
                    {
                        eteileberechnen_btn.Enabled = true;
                    }
                    else
                    {
                        eteileberechnen_btn.Enabled = false;
                    }
                }
            }
        }

        private void textBox3_TextChanged(object sender, System.EventArgs e)
        {
            if (E261.Text == "")
            {
                continue_btn.Enabled = false;
                tE33 = false;
            }
            else
            {
                E261.ForeColor = Color.Black;
                bool okay = true;
                //neuer Text darf nur Zeichen aus der Liste digits (in der Klasse deklariert)
                foreach (char c in E261.Text.ToCharArray())
                {
                    //sobald es ein unpassendes Zeichen gibt, aufhoeren und Fehlermeldung ausgeben
                    if (!digits.Contains<char>(c))
                    {
                        E261.ForeColor = Color.Red;
                        okay = false;
                        tE33 = false;
                        continue_btn.Enabled = false;
                        break;
                    }
                }
                if (okay == true)
                {
                    E261.ForeColor = Color.Black;
                    tE33 = true;
                    if (tE1 & tE2 & tE3 & tE4 & tE5 & tE6 & tE7 & tE8 & tE9 & tE10 & tE11 & tE12 & tE13 & tE14 & tE15 & tE16 & tE17 & tE18 & tE19 & tE20 & tE21 & tE22 & tE23 & tE24 & tE25 & tE26 & tE27 & tE28 & tE29 & tE30 & tE31 & tE32 & tE33)
                    {
                        continue_btn.Enabled = true;
                    }
                    else
                    {
                        continue_btn.Enabled = false;
                    }
                }
            }
        }

        private void E511_TextChanged(object sender, EventArgs e)
        {
            if (E511.Text == "")
            {
                continue_btn.Enabled = false;
                tE1 = false;
            }
            else
            {
                E511.ForeColor = Color.Black;
                bool okay = true;
                //neuer Text darf nur Zeichen aus der Liste digits (in der Klasse deklariert)
                foreach (char c in E511.Text.ToCharArray())
                {
                    //sobald es ein unpassendes Zeichen gibt, aufhoeren und Fehlermeldung ausgeben
                    if (!digits.Contains<char>(c))
                    {
                        E511.ForeColor = Color.Red;
                        okay = false;
                        tE1 = false;
                        continue_btn.Enabled = false;
                        break;
                    }
                }
                if (okay == true)
                {
                    E511.ForeColor = Color.Black;
                    tE1 = true;
                    if (tE1 & tE2 & tE3 & tE4 & tE5 & tE6 & tE7 & tE8 & tE9 & tE10 & tE11 & tE12 & tE13 & tE14 & tE15 & tE16 & tE17 & tE18 & tE19 & tE20 & tE21 & tE22 & tE23 & tE24 & tE25 & tE26 & tE27 & tE28 & tE29 & tE30 & tE31 & tE32 & tE33)
                    {
                        continue_btn.Enabled = true;
                    }
                    else
                    {
                        continue_btn.Enabled = false;
                    }
                }
            }
        }

        private void E161_TextChanged(object sender, EventArgs e)
        {
            if (E161.Text == "")
            {
                continue_btn.Enabled = false;
                tE2 = false;
            }
            else
            {
                E161.ForeColor = Color.Black;
                bool okay = true;
                //neuer Text darf nur Zeichen aus der Liste digits (in der Klasse deklariert)
                foreach (char c in E161.Text.ToCharArray())
                {
                    //sobald es ein unpassendes Zeichen gibt, aufhoeren und Fehlermeldung ausgeben
                    if (!digits.Contains<char>(c))
                    {
                        E161.ForeColor = Color.Red;
                        okay = false;
                        tE2 = false;
                        continue_btn.Enabled = false;
                        break;
                    }
                }
                if (okay == true)
                {
                    E161.ForeColor = Color.Black;
                    tE2 = true;
                    if (tE1 & tE2 & tE3 & tE4 & tE5 & tE6 & tE7 & tE8 & tE9 & tE10 & tE11 & tE12 & tE13 & tE14 & tE15 & tE16 & tE17 & tE18 & tE19 & tE20 & tE21 & tE22 & tE23 & tE24 & tE25 & tE26 & tE27 & tE28 & tE29 & tE30 & tE31 & tE32 & tE33)
                    {
                        continue_btn.Enabled = true;
                    }
                    else
                    {
                        continue_btn.Enabled = false;
                    }
                }
            }
        }

        private void E171_TextChanged(object sender, EventArgs e)
        {
            if (E171.Text == "")
            {
                continue_btn.Enabled = false;
                tE3 = false;
            }
            else
            {
                E171.ForeColor = Color.Black;
                bool okay = true;
                //neuer Text darf nur Zeichen aus der Liste digits (in der Klasse deklariert)
                foreach (char c in E171.Text.ToCharArray())
                {
                    //sobald es ein unpassendes Zeichen gibt, aufhoeren und Fehlermeldung ausgeben
                    if (!digits.Contains<char>(c))
                    {
                        E171.ForeColor = Color.Red;
                        okay = false;
                        tE3 = false;
                        continue_btn.Enabled = false;
                        break;
                    }
                }
                if (okay == true)
                {
                    E171.ForeColor = Color.Black;
                    tE3 = true;
                    if (tE1 & tE2 & tE3 & tE4 & tE5 & tE6 & tE7 & tE8 & tE9 & tE10 & tE11 & tE12 & tE13 & tE14 & tE15 & tE16 & tE17 & tE18 & tE19 & tE20 & tE21 & tE22 & tE23 & tE24 & tE25 & tE26 & tE27 & tE28 & tE29 & tE30 & tE31 & tE32 & tE33)
                    {
                        continue_btn.Enabled = true;
                    }
                    else
                    {
                        continue_btn.Enabled = false;
                    }
                }
            }
        }

        private void E501_TextChanged(object sender, EventArgs e)
        {
            if (E501.Text == "")
            {
                continue_btn.Enabled = false;
                tE4 = false;
            }
            else
            {
                E501.ForeColor = Color.Black;
                bool okay = true;
                //neuer Text darf nur Zeichen aus der Liste digits (in der Klasse deklariert)
                foreach (char c in E501.Text.ToCharArray())
                {
                    //sobald es ein unpassendes Zeichen gibt, aufhoeren und Fehlermeldung ausgeben
                    if (!digits.Contains<char>(c))
                    {
                        E501.ForeColor = Color.Red;
                        okay = false;
                        tE4 = false;
                        continue_btn.Enabled = false;
                        break;
                    }
                }
                if (okay == true)
                {
                    E501.ForeColor = Color.Black;
                    tE4 = true;
                    if (tE1 & tE2 & tE3 & tE4 & tE5 & tE6 & tE7 & tE8 & tE9 & tE10 & tE11 & tE12 & tE13 & tE14 & tE15 & tE16 & tE17 & tE18 & tE19 & tE20 & tE21 & tE22 & tE23 & tE24 & tE25 & tE26 & tE27 & tE28 & tE29 & tE30 & tE31 & tE32 & tE33)
                    {
                        continue_btn.Enabled = true;
                    }
                    else
                    {
                        continue_btn.Enabled = false;
                    }
                }
            }
        }

        private void E041_TextChanged(object sender, EventArgs e)
        {
            if (E041.Text == "")
            {
                continue_btn.Enabled = false;
                tE5 = false;
            }
            else
            {
                E041.ForeColor = Color.Black;
                bool okay = true;
                //neuer Text darf nur Zeichen aus der Liste digits (in der Klasse deklariert)
                foreach (char c in E041.Text.ToCharArray())
                {
                    //sobald es ein unpassendes Zeichen gibt, aufhoeren und Fehlermeldung ausgeben
                    if (!digits.Contains<char>(c))
                    {
                        E041.ForeColor = Color.Red;
                        okay = false;
                        tE5 = false;
                        continue_btn.Enabled = false;
                        break;
                    }
                }
                if (okay == true)
                {
                    E041.ForeColor = Color.Black;
                    tE5 = true;
                    if (tE1 & tE2 & tE3 & tE4 & tE5 & tE6 & tE7 & tE8 & tE9 & tE10 & tE11 & tE12 & tE13 & tE14 & tE15 & tE16 & tE17 & tE18 & tE19 & tE20 & tE21 & tE22 & tE23 & tE24 & tE25 & tE26 & tE27 & tE28 & tE29 & tE30 & tE31 & tE32 & tE33)
                    {
                        continue_btn.Enabled = true;
                    }
                    else
                    {
                        continue_btn.Enabled = false;
                    }
                }
            }
        }

        private void E101_TextChanged(object sender, EventArgs e)
        {
            if (E101.Text == "")
            {
                continue_btn.Enabled = false;
                tE6 = false;
            }
            else
            {
                E101.ForeColor = Color.Black;
                bool okay = true;
                //neuer Text darf nur Zeichen aus der Liste digits (in der Klasse deklariert)
                foreach (char c in E101.Text.ToCharArray())
                {
                    //sobald es ein unpassendes Zeichen gibt, aufhoeren und Fehlermeldung ausgeben
                    if (!digits.Contains<char>(c))
                    {
                        E101.ForeColor = Color.Red;
                        okay = false;
                        tE6 = false;
                        continue_btn.Enabled = false;
                        break;
                    }
                }
                if (okay == true)
                {
                    E101.ForeColor = Color.Black;
                    tE6 = true;
                    if (tE1 & tE2 & tE3 & tE4 & tE5 & tE6 & tE7 & tE8 & tE9 & tE10 & tE11 & tE12 & tE13 & tE14 & tE15 & tE16 & tE17 & tE18 & tE19 & tE20 & tE21 & tE22 & tE23 & tE24 & tE25 & tE26 & tE27 & tE28 & tE29 & tE30 & tE31 & tE32 & tE33)
                    {
                        continue_btn.Enabled = true;
                    }
                    else
                    {
                        continue_btn.Enabled = false;
                    }
                }
            }
        }

        private void E491_TextChanged(object sender, EventArgs e)
        {
            if (E491.Text == "")
            {
                continue_btn.Enabled = false;
                tE7 = false;
            }
            else
            {
                E491.ForeColor = Color.Black;
                bool okay = true;
                //neuer Text darf nur Zeichen aus der Liste digits (in der Klasse deklariert)
                foreach (char c in E491.Text.ToCharArray())
                {
                    //sobald es ein unpassendes Zeichen gibt, aufhoeren und Fehlermeldung ausgeben
                    if (!digits.Contains<char>(c))
                    {
                        E491.ForeColor = Color.Red;
                        okay = false;
                        tE7 = false;
                        continue_btn.Enabled = false;
                        break;
                    }
                }
                if (okay == true)
                {
                    E491.ForeColor = Color.Black;
                    tE7 = true;
                    if (tE1 & tE2 & tE3 & tE4 & tE5 & tE6 & tE7 & tE8 & tE9 & tE10 & tE11 & tE12 & tE13 & tE14 & tE15 & tE16 & tE17 & tE18 & tE19 & tE20 & tE21 & tE22 & tE23 & tE24 & tE25 & tE26 & tE27 & tE28 & tE29 & tE30 & tE31 & tE32 & tE33)
                    {
                        continue_btn.Enabled = true;
                    }
                    else
                    {
                        continue_btn.Enabled = false;
                    }
                }
            }
        }

        private void E071_TextChanged(object sender, EventArgs e)
        {
            if (E071.Text == "")
            {
                continue_btn.Enabled = false;
                tE8 = false;
            }
            else
            {
                E071.ForeColor = Color.Black;
                bool okay = true;
                //neuer Text darf nur Zeichen aus der Liste digits (in der Klasse deklariert)
                foreach (char c in E071.Text.ToCharArray())
                {
                    //sobald es ein unpassendes Zeichen gibt, aufhoeren und Fehlermeldung ausgeben
                    if (!digits.Contains<char>(c))
                    {
                        E071.ForeColor = Color.Red;
                        okay = false;
                        tE8 = false;
                        continue_btn.Enabled = false;
                        break;
                    }
                }
                if (okay == true)
                {
                    E071.ForeColor = Color.Black;
                    tE8 = true;
                    if (tE1 & tE2 & tE3 & tE4 & tE5 & tE6 & tE7 & tE8 & tE9 & tE10 & tE11 & tE12 & tE13 & tE14 & tE15 & tE16 & tE17 & tE18 & tE19 & tE20 & tE21 & tE22 & tE23 & tE24 & tE25 & tE26 & tE27 & tE28 & tE29 & tE30 & tE31 & tE32 & tE33)
                    {
                        continue_btn.Enabled = true;
                    }
                    else
                    {
                        continue_btn.Enabled = false;
                    }
                }
            }
        }

        private void E131_TextChanged(object sender, EventArgs e)
        {
            if (E131.Text == "")
            {
                continue_btn.Enabled = false;
                tE9 = false;
            }
            else
            {
                E131.ForeColor = Color.Black;
                bool okay = true;
                //neuer Text darf nur Zeichen aus der Liste digits (in der Klasse deklariert)
                foreach (char c in E131.Text.ToCharArray())
                {
                    //sobald es ein unpassendes Zeichen gibt, aufhoeren und Fehlermeldung ausgeben
                    if (!digits.Contains<char>(c))
                    {
                        E131.ForeColor = Color.Red;
                        okay = false;
                        tE9 = false;
                        continue_btn.Enabled = false;
                        break;
                    }
                }
                if (okay == true)
                {
                    E131.ForeColor = Color.Black;
                    tE9 = true;
                    if (tE1 & tE2 & tE3 & tE4 & tE5 & tE6 & tE7 & tE8 & tE9 & tE10 & tE11 & tE12 & tE13 & tE14 & tE15 & tE16 & tE17 & tE18 & tE19 & tE20 & tE21 & tE22 & tE23 & tE24 & tE25 & tE26 & tE27 & tE28 & tE29 & tE30 & tE31 & tE32 & tE33)
                    {
                        continue_btn.Enabled = true;
                    }
                    else
                    {
                        continue_btn.Enabled = false;
                    }
                }
            }
        }

        private void E181_TextChanged(object sender, EventArgs e)
        {
            if (E181.Text == "")
            {
                continue_btn.Enabled = false;
                tE10 = false;
            }
            else
            {
                E181.ForeColor = Color.Black;
                bool okay = true;
                //neuer Text darf nur Zeichen aus der Liste digits (in der Klasse deklariert)
                foreach (char c in E181.Text.ToCharArray())
                {
                    //sobald es ein unpassendes Zeichen gibt, aufhoeren und Fehlermeldung ausgeben
                    if (!digits.Contains<char>(c))
                    {
                        E181.ForeColor = Color.Red;
                        okay = false;
                        tE10 = false;
                        continue_btn.Enabled = false;
                        break;
                    }
                }
                if (okay == true)
                {
                    E181.ForeColor = Color.Black;
                    tE10 = true;
                    if (tE1 & tE2 & tE3 & tE4 & tE5 & tE6 & tE7 & tE8 & tE9 & tE10 & tE11 & tE12 & tE13 & tE14 & tE15 & tE16 & tE17 & tE18 & tE19 & tE20 & tE21 & tE22 & tE23 & tE24 & tE25 & tE26 & tE27 & tE28 & tE29 & tE30 & tE31 & tE32 & tE33)
                    {
                        continue_btn.Enabled = true;
                    }
                    else
                    {
                        continue_btn.Enabled = false;
                    }
                }
            }
        }

        private void E262_TextChanged(object sender, EventArgs e)
        {
            if (E262.Text == "")
            {
                continue_btn.Enabled = false;
                tE11 = false;
            }
            else
            {
                E262.ForeColor = Color.Black;
                bool okay = true;
                //neuer Text darf nur Zeichen aus der Liste digits (in der Klasse deklariert)
                foreach (char c in E262.Text.ToCharArray())
                {
                    //sobald es ein unpassendes Zeichen gibt, aufhoeren und Fehlermeldung ausgeben
                    if (!digits.Contains<char>(c))
                    {
                        E262.ForeColor = Color.Red;
                        okay = false;
                        tE11 = false;
                        continue_btn.Enabled = false;
                        break;
                    }
                }
                if (okay == true)
                {
                    E262.ForeColor = Color.Black;
                    tE11 = true;
                    if (tE1 & tE2 & tE3 & tE4 & tE5 & tE6 & tE7 & tE8 & tE9 & tE10 & tE11 & tE12 & tE13 & tE14 & tE15 & tE16 & tE17 & tE18 & tE19 & tE20 & tE21 & tE22 & tE23 & tE24 & tE25 & tE26 & tE27 & tE28 & tE29 & tE30 & tE31 & tE32 & tE33)
                    {
                        continue_btn.Enabled = true;
                    }
                    else
                    {
                        continue_btn.Enabled = false;
                    }
                }
            }
        }

        private void E562_TextChanged(object sender, EventArgs e)
        {
            if (E562.Text == "")
            {
                continue_btn.Enabled = false;
                tE12 = false;
            }
            else
            {
                E562.ForeColor = Color.Black;
                bool okay = true;
                //neuer Text darf nur Zeichen aus der Liste digits (in der Klasse deklariert)
                foreach (char c in E562.Text.ToCharArray())
                {
                    //sobald es ein unpassendes Zeichen gibt, aufhoeren und Fehlermeldung ausgeben
                    if (!digits.Contains<char>(c))
                    {
                        E562.ForeColor = Color.Red;
                        okay = false;
                        tE12 = false;
                        continue_btn.Enabled = false;
                        break;
                    }
                }
                if (okay == true)
                {
                    E562.ForeColor = Color.Black;
                    tE12 = true;
                    if (tE1 & tE2 & tE3 & tE4 & tE5 & tE6 & tE7 & tE8 & tE9 & tE10 & tE11 & tE12 & tE13 & tE14 & tE15 & tE16 & tE17 & tE18 & tE19 & tE20 & tE21 & tE22 & tE23 & tE24 & tE25 & tE26 & tE27 & tE28 & tE29 & tE30 & tE31 & tE32 & tE33)
                    {
                        continue_btn.Enabled = true;
                    }
                    else
                    {
                        continue_btn.Enabled = false;
                    }
                }
            }
        }

        private void E162_TextChanged(object sender, EventArgs e)
        {
            if (E162.Text == "")
            {
                continue_btn.Enabled = false;
                tE13 = false;
            }
            else
            {
                E162.ForeColor = Color.Black;
                bool okay = true;
                //neuer Text darf nur Zeichen aus der Liste digits (in der Klasse deklariert)
                foreach (char c in E162.Text.ToCharArray())
                {
                    //sobald es ein unpassendes Zeichen gibt, aufhoeren und Fehlermeldung ausgeben
                    if (!digits.Contains<char>(c))
                    {
                        E162.ForeColor = Color.Red;
                        okay = false;
                        tE13 = false;
                        continue_btn.Enabled = false;
                        break;
                    }
                }
                if (okay == true)
                {
                    E162.ForeColor = Color.Black;
                    tE13 = true;
                    if (tE1 & tE2 & tE3 & tE4 & tE5 & tE6 & tE7 & tE8 & tE9 & tE10 & tE11 & tE12 & tE13 & tE14 & tE15 & tE16 & tE17 & tE18 & tE19 & tE20 & tE21 & tE22 & tE23 & tE24 & tE25 & tE26 & tE27 & tE28 & tE29 & tE30 & tE31 & tE32 & tE33)
                    {
                        continue_btn.Enabled = true;
                    }
                    else
                    {
                        continue_btn.Enabled = false;
                    }
                }
            }
        }

        private void E172_TextChanged(object sender, EventArgs e)
        {
            if (E172.Text == "")
            {
                continue_btn.Enabled = false;
                tE14 = false;
            }
            else
            {
                E172.ForeColor = Color.Black;
                bool okay = true;
                //neuer Text darf nur Zeichen aus der Liste digits (in der Klasse deklariert)
                foreach (char c in E172.Text.ToCharArray())
                {
                    //sobald es ein unpassendes Zeichen gibt, aufhoeren und Fehlermeldung ausgeben
                    if (!digits.Contains<char>(c))
                    {
                        E172.ForeColor = Color.Red;
                        okay = false;
                        tE14 = false;
                        continue_btn.Enabled = false;
                        break;
                    }
                }
                if (okay == true)
                {
                    E172.ForeColor = Color.Black;
                    tE14 = true;
                    if (tE1 & tE2 & tE3 & tE4 & tE5 & tE6 & tE7 & tE8 & tE9 & tE10 & tE11 & tE12 & tE13 & tE14 & tE15 & tE16 & tE17 & tE18 & tE19 & tE20 & tE21 & tE22 & tE23 & tE24 & tE25 & tE26 & tE27 & tE28 & tE29 & tE30 & tE31 & tE32 & tE33)
                    {
                        continue_btn.Enabled = true;
                    }
                    else
                    {
                        continue_btn.Enabled = false;
                    }
                }
            }
        }

        private void E552_TextChanged(object sender, EventArgs e)
        {
            if (E552.Text == "")
            {
                continue_btn.Enabled = false;
                tE15 = false;
            }
            else
            {
                E552.ForeColor = Color.Black;
                bool okay = true;
                //neuer Text darf nur Zeichen aus der Liste digits (in der Klasse deklariert)
                foreach (char c in E552.Text.ToCharArray())
                {
                    //sobald es ein unpassendes Zeichen gibt, aufhoeren und Fehlermeldung ausgeben
                    if (!digits.Contains<char>(c))
                    {
                        E552.ForeColor = Color.Red;
                        okay = false;
                        tE15 = false;
                        continue_btn.Enabled = false;
                        break;
                    }
                }
                if (okay == true)
                {
                    E552.ForeColor = Color.Black;
                    tE15 = true;
                    if (tE1 & tE2 & tE3 & tE4 & tE5 & tE6 & tE7 & tE8 & tE9 & tE10 & tE11 & tE12 & tE13 & tE14 & tE15 & tE16 & tE17 & tE18 & tE19 & tE20 & tE21 & tE22 & tE23 & tE24 & tE25 & tE26 & tE27 & tE28 & tE29 & tE30 & tE31 & tE32 & tE33)
                    {
                        continue_btn.Enabled = true;
                    }
                    else
                    {
                        continue_btn.Enabled = false;
                    }
                }
            }
        }

        private void E052_TextChanged(object sender, EventArgs e)
        {
            if (E052.Text == "")
            {
                continue_btn.Enabled = false;
                tE16 = false;
            }
            else
            {
                E052.ForeColor = Color.Black;
                bool okay = true;
                //neuer Text darf nur Zeichen aus der Liste digits (in der Klasse deklariert)
                foreach (char c in E052.Text.ToCharArray())
                {
                    //sobald es ein unpassendes Zeichen gibt, aufhoeren und Fehlermeldung ausgeben
                    if (!digits.Contains<char>(c))
                    {
                        E052.ForeColor = Color.Red;
                        okay = false;
                        tE16 = false;
                        continue_btn.Enabled = false;
                        break;
                    }
                }
                if (okay == true)
                {
                    E052.ForeColor = Color.Black;
                    tE16 = true;
                    if (tE1 & tE2 & tE3 & tE4 & tE5 & tE6 & tE7 & tE8 & tE9 & tE10 & tE11 & tE12 & tE13 & tE14 & tE15 & tE16 & tE17 & tE18 & tE19 & tE20 & tE21 & tE22 & tE23 & tE24 & tE25 & tE26 & tE27 & tE28 & tE29 & tE30 & tE31 & tE32 & tE33)
                    {
                        continue_btn.Enabled = true;
                    }
                    else
                    {
                        continue_btn.Enabled = false;
                    }
                }
            }
        }

        private void E112_TextChanged(object sender, EventArgs e)
        {
            if (E112.Text == "")
            {
                continue_btn.Enabled = false;
                tE17 = false;
            }
            else
            {
                E112.ForeColor = Color.Black;
                bool okay = true;
                //neuer Text darf nur Zeichen aus der Liste digits (in der Klasse deklariert)
                foreach (char c in E112.Text.ToCharArray())
                {
                    //sobald es ein unpassendes Zeichen gibt, aufhoeren und Fehlermeldung ausgeben
                    if (!digits.Contains<char>(c))
                    {
                        E112.ForeColor = Color.Red;
                        okay = false;
                        tE17 = false;
                        continue_btn.Enabled = false;
                        break;
                    }
                }
                if (okay == true)
                {
                    E112.ForeColor = Color.Black;
                    tE17 = true;
                    if (tE1 & tE2 & tE3 & tE4 & tE5 & tE6 & tE7 & tE8 & tE9 & tE10 & tE11 & tE12 & tE13 & tE14 & tE15 & tE16 & tE17 & tE18 & tE19 & tE20 & tE21 & tE22 & tE23 & tE24 & tE25 & tE26 & tE27 & tE28 & tE29 & tE30 & tE31 & tE32 & tE33)
                    {
                        continue_btn.Enabled = true;
                    }
                    else
                    {
                        continue_btn.Enabled = false;
                    }
                }
            }
        }

        private void E542_TextChanged(object sender, EventArgs e)
        {
            if (E542.Text == "")
            {
                continue_btn.Enabled = false;
                tE18 = false;
            }
            else
            {
                E542.ForeColor = Color.Black;
                bool okay = true;
                //neuer Text darf nur Zeichen aus der Liste digits (in der Klasse deklariert)
                foreach (char c in E542.Text.ToCharArray())
                {
                    //sobald es ein unpassendes Zeichen gibt, aufhoeren und Fehlermeldung ausgeben
                    if (!digits.Contains<char>(c))
                    {
                        E542.ForeColor = Color.Red;
                        okay = false;
                        tE18 = false;
                        continue_btn.Enabled = false;
                        break;
                    }
                }
                if (okay == true)
                {
                    E542.ForeColor = Color.Black;
                    tE18 = true;
                    if (tE1 & tE2 & tE3 & tE4 & tE5 & tE6 & tE7 & tE8 & tE9 & tE10 & tE11 & tE12 & tE13 & tE14 & tE15 & tE16 & tE17 & tE18 & tE19 & tE20 & tE21 & tE22 & tE23 & tE24 & tE25 & tE26 & tE27 & tE28 & tE29 & tE30 & tE31 & tE32 & tE33)
                    {
                        continue_btn.Enabled = true;
                    }
                    else
                    {
                        continue_btn.Enabled = false;
                    }
                }
            }
        }

        private void E082_TextChanged(object sender, EventArgs e)
        {
            if (E082.Text == "")
            {
                continue_btn.Enabled = false;
                tE19 = false;
            }
            else
            {
                E082.ForeColor = Color.Black;
                bool okay = true;
                //neuer Text darf nur Zeichen aus der Liste digits (in der Klasse deklariert)
                foreach (char c in E082.Text.ToCharArray())
                {
                    //sobald es ein unpassendes Zeichen gibt, aufhoeren und Fehlermeldung ausgeben
                    if (!digits.Contains<char>(c))
                    {
                        E082.ForeColor = Color.Red;
                        okay = false;
                        tE19 = false;
                        continue_btn.Enabled = false;
                        break;
                    }
                }
                if (okay == true)
                {
                    E082.ForeColor = Color.Black;
                    tE19 = true;
                    if (tE1 & tE2 & tE3 & tE4 & tE5 & tE6 & tE7 & tE8 & tE9 & tE10 & tE11 & tE12 & tE13 & tE14 & tE15 & tE16 & tE17 & tE18 & tE19 & tE20 & tE21 & tE22 & tE23 & tE24 & tE25 & tE26 & tE27 & tE28 & tE29 & tE30 & tE31 & tE32 & tE33)
                    {
                        continue_btn.Enabled = true;
                    }
                    else
                    {
                        continue_btn.Enabled = false;
                    }
                }
            }
        }

        private void E142_TextChanged(object sender, EventArgs e)
        {
            if (E142.Text == "")
            {
                continue_btn.Enabled = false;
                tE20 = false;
            }
            else
            {
                E142.ForeColor = Color.Black;
                bool okay = true;
                //neuer Text darf nur Zeichen aus der Liste digits (in der Klasse deklariert)
                foreach (char c in E142.Text.ToCharArray())
                {
                    //sobald es ein unpassendes Zeichen gibt, aufhoeren und Fehlermeldung ausgeben
                    if (!digits.Contains<char>(c))
                    {
                        E142.ForeColor = Color.Red;
                        okay = false;
                        tE20 = false;
                        continue_btn.Enabled = false;
                        break;
                    }
                }
                if (okay == true)
                {
                    E142.ForeColor = Color.Black;
                    tE20 = true;
                    if (tE1 & tE2 & tE3 & tE4 & tE5 & tE6 & tE7 & tE8 & tE9 & tE10 & tE11 & tE12 & tE13 & tE14 & tE15 & tE16 & tE17 & tE18 & tE19 & tE20 & tE21 & tE22 & tE23 & tE24 & tE25 & tE26 & tE27 & tE28 & tE29 & tE30 & tE31 & tE32 & tE33)
                    {
                        continue_btn.Enabled = true;
                    }
                    else
                    {
                        continue_btn.Enabled = false;
                    }
                }
            }
        }

        private void E192_TextChanged(object sender, EventArgs e)
        {
            if (E192.Text == "")
            {
                continue_btn.Enabled = false;
                tE21 = false;
            }
            else
            {
                E192.ForeColor = Color.Black;
                bool okay = true;
                //neuer Text darf nur Zeichen aus der Liste digits (in der Klasse deklariert)
                foreach (char c in E192.Text.ToCharArray())
                {
                    //sobald es ein unpassendes Zeichen gibt, aufhoeren und Fehlermeldung ausgeben
                    if (!digits.Contains<char>(c))
                    {
                        E192.ForeColor = Color.Red;
                        okay = false;
                        tE21 = false;
                        continue_btn.Enabled = false;
                        break;
                    }
                }
                if (okay == true)
                {
                    E192.ForeColor = Color.Black;
                    tE21 = true;
                    if (tE1 & tE2 & tE3 & tE4 & tE5 & tE6 & tE7 & tE8 & tE9 & tE10 & tE11 & tE12 & tE13 & tE14 & tE15 & tE16 & tE17 & tE18 & tE19 & tE20 & tE21 & tE22 & tE23 & tE24 & tE25 & tE26 & tE27 & tE28 & tE29 & tE30 & tE31 & tE32 & tE33)
                    {
                        continue_btn.Enabled = true;
                    }
                    else
                    {
                        continue_btn.Enabled = false;
                    }
                }
            }
        }

        private void E263_TextChanged(object sender, EventArgs e)
        {
            if (E263.Text == "")
            {
                continue_btn.Enabled = false;
                tE22 = false;
            }
            else
            {
                E263.ForeColor = Color.Black;
                bool okay = true;
                //neuer Text darf nur Zeichen aus der Liste digits (in der Klasse deklariert)
                foreach (char c in E263.Text.ToCharArray())
                {
                    //sobald es ein unpassendes Zeichen gibt, aufhoeren und Fehlermeldung ausgeben
                    if (!digits.Contains<char>(c))
                    {
                        E263.ForeColor = Color.Red;
                        okay = false;
                        tE22 = false;
                        continue_btn.Enabled = false;
                        break;
                    }
                }
                if (okay == true)
                {
                    E263.ForeColor = Color.Black;
                    tE22 = true;
                    if (tE1 & tE2 & tE3 & tE4 & tE5 & tE6 & tE7 & tE8 & tE9 & tE10 & tE11 & tE12 & tE13 & tE14 & tE15 & tE16 & tE17 & tE18 & tE19 & tE20 & tE21 & tE22 & tE23 & tE24 & tE25 & tE26 & tE27 & tE28 & tE29 & tE30 & tE31 & tE32 & tE33)
                    {
                        continue_btn.Enabled = true;
                    }
                    else
                    {
                        continue_btn.Enabled = false;
                    }
                }
            }
        }

        private void E313_TextChanged(object sender, EventArgs e)
        {
            if (E313.Text == "")
            {
                continue_btn.Enabled = false;
                tE23 = false;
            }
            else
            {
                E313.ForeColor = Color.Black;
                bool okay = true;
                //neuer Text darf nur Zeichen aus der Liste digits (in der Klasse deklariert)
                foreach (char c in E313.Text.ToCharArray())
                {
                    //sobald es ein unpassendes Zeichen gibt, aufhoeren und Fehlermeldung ausgeben
                    if (!digits.Contains<char>(c))
                    {
                        E313.ForeColor = Color.Red;
                        okay = false;
                        tE23 = false;
                        continue_btn.Enabled = false;
                        break;
                    }
                }
                if (okay == true)
                {
                    E313.ForeColor = Color.Black;
                    tE23 = true;
                    if (tE1 & tE2 & tE3 & tE4 & tE5 & tE6 & tE7 & tE8 & tE9 & tE10 & tE11 & tE12 & tE13 & tE14 & tE15 & tE16 & tE17 & tE18 & tE19 & tE20 & tE21 & tE22 & tE23 & tE24 & tE25 & tE26 & tE27 & tE28 & tE29 & tE30 & tE31 & tE32 & tE33)
                    {
                        continue_btn.Enabled = true;
                    }
                    else
                    {
                        continue_btn.Enabled = false;
                    }
                }
            }
        }

        private void E163_TextChanged(object sender, EventArgs e)
        {
            if (E163.Text == "")
            {
                continue_btn.Enabled = false;
                tE24 = false;
            }
            else
            {
                E163.ForeColor = Color.Black;
                bool okay = true;
                //neuer Text darf nur Zeichen aus der Liste digits (in der Klasse deklariert)
                foreach (char c in E163.Text.ToCharArray())
                {
                    //sobald es ein unpassendes Zeichen gibt, aufhoeren und Fehlermeldung ausgeben
                    if (!digits.Contains<char>(c))
                    {
                        E163.ForeColor = Color.Red;
                        okay = false;
                        tE24 = false;
                        continue_btn.Enabled = false;
                        break;
                    }
                }
                if (okay == true)
                {
                    E163.ForeColor = Color.Black;
                    tE24 = true;
                    if (tE1 & tE2 & tE3 & tE4 & tE5 & tE6 & tE7 & tE8 & tE9 & tE10 & tE11 & tE12 & tE13 & tE14 & tE15 & tE16 & tE17 & tE18 & tE19 & tE20 & tE21 & tE22 & tE23 & tE24 & tE25 & tE26 & tE27 & tE28 & tE29 & tE30 & tE31 & tE32 & tE33)
                    {
                        continue_btn.Enabled = true;
                    }
                    else
                    {
                        continue_btn.Enabled = false;
                    }
                }
            }
        }

        private void E173_TextChanged(object sender, EventArgs e)
        {
            if (E173.Text == "")
            {
                continue_btn.Enabled = false;
                tE25 = false;
            }
            else
            {
                E173.ForeColor = Color.Black;
                bool okay = true;
                //neuer Text darf nur Zeichen aus der Liste digits (in der Klasse deklariert)
                foreach (char c in E173.Text.ToCharArray())
                {
                    //sobald es ein unpassendes Zeichen gibt, aufhoeren und Fehlermeldung ausgeben
                    if (!digits.Contains<char>(c))
                    {
                        E173.ForeColor = Color.Red;
                        okay = false;
                        tE25 = false;
                        continue_btn.Enabled = false;
                        break;
                    }
                }
                if (okay == true)
                {
                    E173.ForeColor = Color.Black;
                    tE25 = true;
                    if (tE1 & tE2 & tE3 & tE4 & tE5 & tE6 & tE7 & tE8 & tE9 & tE10 & tE11 & tE12 & tE13 & tE14 & tE15 & tE16 & tE17 & tE18 & tE19 & tE20 & tE21 & tE22 & tE23 & tE24 & tE25 & tE26 & tE27 & tE28 & tE29 & tE30 & tE31 & tE32 & tE33)
                    {
                        continue_btn.Enabled = true;
                    }
                    else
                    {
                        continue_btn.Enabled = false;
                    }
                }
            }
        }

        private void E303_TextChanged(object sender, EventArgs e)
        {
            if (E303.Text == "")
            {
                continue_btn.Enabled = false;
                tE26 = false;
            }
            else
            {
                E303.ForeColor = Color.Black;
                bool okay = true;
                //neuer Text darf nur Zeichen aus der Liste digits (in der Klasse deklariert)
                foreach (char c in E303.Text.ToCharArray())
                {
                    //sobald es ein unpassendes Zeichen gibt, aufhoeren und Fehlermeldung ausgeben
                    if (!digits.Contains<char>(c))
                    {
                        E303.ForeColor = Color.Red;
                        okay = false;
                        tE26 = false;
                        continue_btn.Enabled = false;
                        break;
                    }
                }
                if (okay == true)
                {
                    E303.ForeColor = Color.Black;
                    tE26 = true;
                    if (tE1 & tE2 & tE3 & tE4 & tE5 & tE6 & tE7 & tE8 & tE9 & tE10 & tE11 & tE12 & tE13 & tE14 & tE15 & tE16 & tE17 & tE18 & tE19 & tE20 & tE21 & tE22 & tE23 & tE24 & tE25 & tE26 & tE27 & tE28 & tE29 & tE30 & tE31 & tE32 & tE33)
                    {
                        continue_btn.Enabled = true;
                    }
                    else
                    {
                        continue_btn.Enabled = false;
                    }
                }
            }
        }

        private void E063_TextChanged(object sender, EventArgs e)
        {
            if (E063.Text == "")
            {
                continue_btn.Enabled = false;
                tE27 = false;
            }
            else
            {
                E063.ForeColor = Color.Black;
                bool okay = true;
                //neuer Text darf nur Zeichen aus der Liste digits (in der Klasse deklariert)
                foreach (char c in E063.Text.ToCharArray())
                {
                    //sobald es ein unpassendes Zeichen gibt, aufhoeren und Fehlermeldung ausgeben
                    if (!digits.Contains<char>(c))
                    {
                        E063.ForeColor = Color.Red;
                        okay = false;
                        tE27 = false;
                        continue_btn.Enabled = false;
                        break;
                    }
                }
                if (okay == true)
                {
                    E063.ForeColor = Color.Black;
                    tE27 = true;
                    if (tE1 & tE2 & tE3 & tE4 & tE5 & tE6 & tE7 & tE8 & tE9 & tE10 & tE11 & tE12 & tE13 & tE14 & tE15 & tE16 & tE17 & tE18 & tE19 & tE20 & tE21 & tE22 & tE23 & tE24 & tE25 & tE26 & tE27 & tE28 & tE29 & tE30 & tE31 & tE32 & tE33)
                    {
                        continue_btn.Enabled = true;
                    }
                    else
                    {
                        continue_btn.Enabled = false;
                    }
                }
            }
        }

        private void E123_TextChanged(object sender, EventArgs e)
        {
            if (E123.Text == "")
            {
                continue_btn.Enabled = false;
                tE28 = false;
            }
            else
            {
                E123.ForeColor = Color.Black;
                bool okay = true;
                //neuer Text darf nur Zeichen aus der Liste digits (in der Klasse deklariert)
                foreach (char c in E123.Text.ToCharArray())
                {
                    //sobald es ein unpassendes Zeichen gibt, aufhoeren und Fehlermeldung ausgeben
                    if (!digits.Contains<char>(c))
                    {
                        E123.ForeColor = Color.Red;
                        okay = false;
                        tE28 = false;
                        continue_btn.Enabled = false;
                        break;
                    }
                }
                if (okay == true)
                {
                    E123.ForeColor = Color.Black;
                    tE28 = true;
                    if (tE1 & tE2 & tE3 & tE4 & tE5 & tE6 & tE7 & tE8 & tE9 & tE10 & tE11 & tE12 & tE13 & tE14 & tE15 & tE16 & tE17 & tE18 & tE19 & tE20 & tE21 & tE22 & tE23 & tE24 & tE25 & tE26 & tE27 & tE28 & tE29 & tE30 & tE31 & tE32 & tE33)
                    {
                        continue_btn.Enabled = true;
                    }
                    else
                    {
                        continue_btn.Enabled = false;
                    }
                }
            }
        }

        private void E293_TextChanged(object sender, EventArgs e)
        {
            if (E293.Text == "")
            {
                continue_btn.Enabled = false;
                tE29 = false;
            }
            else
            {
                E293.ForeColor = Color.Black;
                bool okay = true;
                //neuer Text darf nur Zeichen aus der Liste digits (in der Klasse deklariert)
                foreach (char c in E293.Text.ToCharArray())
                {
                    //sobald es ein unpassendes Zeichen gibt, aufhoeren und Fehlermeldung ausgeben
                    if (!digits.Contains<char>(c))
                    {
                        E293.ForeColor = Color.Red;
                        okay = false;
                        tE29 = false;
                        continue_btn.Enabled = false;
                        break;
                    }
                }
                if (okay == true)
                {
                    E293.ForeColor = Color.Black;
                    tE29 = true;
                    if (tE1 & tE2 & tE3 & tE4 & tE5 & tE6 & tE7 & tE8 & tE9 & tE10 & tE11 & tE12 & tE13 & tE14 & tE15 & tE16 & tE17 & tE18 & tE19 & tE20 & tE21 & tE22 & tE23 & tE24 & tE25 & tE26 & tE27 & tE28 & tE29 & tE30 & tE31 & tE32 & tE33)
                    {
                        continue_btn.Enabled = true;
                    }
                    else
                    {
                        continue_btn.Enabled = false;
                    }
                }
            }
        }

        private void E093_TextChanged(object sender, EventArgs e)
        {
            if (E093.Text == "")
            {
                continue_btn.Enabled = false;
                tE30 = false;
            }
            else
            {
                E093.ForeColor = Color.Black;
                bool okay = true;
                //neuer Text darf nur Zeichen aus der Liste digits (in der Klasse deklariert)
                foreach (char c in E093.Text.ToCharArray())
                {
                    //sobald es ein unpassendes Zeichen gibt, aufhoeren und Fehlermeldung ausgeben
                    if (!digits.Contains<char>(c))
                    {
                        E093.ForeColor = Color.Red;
                        okay = false;
                        tE30 = false;
                        continue_btn.Enabled = false;
                        break;
                    }
                }
                if (okay == true)
                {
                    E093.ForeColor = Color.Black;
                    tE30 = true;
                    if (tE1 & tE2 & tE3 & tE4 & tE5 & tE6 & tE7 & tE8 & tE9 & tE10 & tE11 & tE12 & tE13 & tE14 & tE15 & tE16 & tE17 & tE18 & tE19 & tE20 & tE21 & tE22 & tE23 & tE24 & tE25 & tE26 & tE27 & tE28 & tE29 & tE30 & tE31 & tE32 & tE33)
                    {
                        continue_btn.Enabled = true;
                    }
                    else
                    {
                        continue_btn.Enabled = false;
                    }
                }
            }
        }

        private void E153_TextChanged(object sender, EventArgs e)
        {
            if (E153.Text == "")
            {
                continue_btn.Enabled = false;
                tE31 = false;
            }
            else
            {
                E153.ForeColor = Color.Black;
                bool okay = true;
                //neuer Text darf nur Zeichen aus der Liste digits (in der Klasse deklariert)
                foreach (char c in E153.Text.ToCharArray())
                {
                    //sobald es ein unpassendes Zeichen gibt, aufhoeren und Fehlermeldung ausgeben
                    if (!digits.Contains<char>(c))
                    {
                        E153.ForeColor = Color.Red;
                        okay = false;
                        tE31 = false;
                        continue_btn.Enabled = false;
                        break;
                    }
                }
                if (okay == true)
                {
                    E153.ForeColor = Color.Black;
                    tE31 = true;
                    if (tE1 & tE2 & tE3 & tE4 & tE5 & tE6 & tE7 & tE8 & tE9 & tE10 & tE11 & tE12 & tE13 & tE14 & tE15 & tE16 & tE17 & tE18 & tE19 & tE20 & tE21 & tE22 & tE23 & tE24 & tE25 & tE26 & tE27 & tE28 & tE29 & tE30 & tE31 & tE32 & tE33)
                    {
                        continue_btn.Enabled = true;
                    }
                    else
                    {
                        continue_btn.Enabled = false;
                    }
                }
            }
        }

        private void E203_TextChanged(object sender, EventArgs e)
        {
            if (E203.Text == "")
            {
                continue_btn.Enabled = false;
                tE32 = false;
            }
            else
            {
                E203.ForeColor = Color.Black;
                bool okay = true;
                //neuer Text darf nur Zeichen aus der Liste digits (in der Klasse deklariert)
                foreach (char c in E203.Text.ToCharArray())
                {
                    //sobald es ein unpassendes Zeichen gibt, aufhoeren und Fehlermeldung ausgeben
                    if (!digits.Contains<char>(c))
                    {
                        E203.ForeColor = Color.Red;
                        okay = false;
                        tE32 = false;
                        continue_btn.Enabled = false;
                        break;
                    }
                }
                if (okay == true)
                {
                    E203.ForeColor = Color.Black;
                    tE32 = true;
                    if (tE1 & tE2 & tE3 & tE4 & tE5 & tE6 & tE7 & tE8 & tE9 & tE10 & tE11 & tE12 & tE13 & tE14 & tE15 & tE16 & tE17 & tE18 & tE19 & tE20 & tE21 & tE22 & tE23 & tE24 & tE25 & tE26 & tE27 & tE28 & tE29 & tE30 & tE31 & tE32 & tE33)
                    {
                        continue_btn.Enabled = true;
                    }
                    else
                    {
                        continue_btn.Enabled = false;
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
                lbl_Auftraege.Text = (Sprachen.EN_LBL_PRODUKTION);
                lbl_Produktionsreihenfolge.Text = (Sprachen.EN_LBL_PRODUKTIONSREIHENFOLGE);
                lbl_Kapazitaetsplan.Text = (Sprachen.EN_LBL_KAPATITAETSPLAN);
                lbl_Kaufteiledisposition.Text = (Sprachen.EN_LBL_KAUFTEILEDISPOSITION);
                lbl_Ergebnis.Text = (Sprachen.EN_LBL_ERGEBNIS);

                //EN Buttons
                eteileberechnen_btn.Text = (Sprachen.EN_BTN_ETEILEBERECHNEN);
                continue_btn.Text = (Sprachen.EN_BTN_CONTINUE);
                btn_back.Text = (Sprachen.EN_BTN_BACK);

                //EN Groupboxen
                groupBox1.Text = (Sprachen.EN_GROUPBOX1);
                groupBox3.Text = (Sprachen.EN_GROUPBOX3);
                groupBox2.Text = (Sprachen.EN_GROUPBOX2);

                //EN Labels
                //label4.Text = (Sprachen.EN_LABEL4);
                label9.Text = (Sprachen.EN_LABLE9);

                //EN Tooltip
                System.Windows.Forms.ToolTip ToolTipEN = new System.Windows.Forms.ToolTip();
                ToolTipEN.SetToolTip(this.infoP, Sprachen.EN_INFOP);
                ToolTipEN.SetToolTip(this.infoE, Sprachen.EN_INFOE);
            }
            else
            {
                //DE Brotkrumenleiste
                lbl_Sicherheitsbestand.Text = (Sprachen.DE_LBL_SICHERHEITSBESTAND);
                lbl_Startseite.Text = (Sprachen.DE_LBL_STARTSEITE);
                lbl_Auftraege.Text = (Sprachen.DE_LBL_PRODUKTION);
                lbl_Produktionsreihenfolge.Text = (Sprachen.DE_LBL_PRODUKTIONSREIHENFOLGE);
                lbl_Kapazitaetsplan.Text = (Sprachen.DE_LBL_KAPATITAETSPLAN);
                lbl_Kaufteiledisposition.Text = (Sprachen.DE_LBL_KAUFTEILEDISPOSITION);
                lbl_Ergebnis.Text = (Sprachen.DE_LBL_ERGEBNIS);

                //DE Buttons
                eteileberechnen_btn.Text = (Sprachen.DE_BTN_ETEILEBERECHNEN);
                continue_btn.Text = (Sprachen.DE_BTN_CONTINUE);
                btn_back.Text = (Sprachen.DE_BTN_BACK);

                //DE Groupboxen
                groupBox1.Text = (Sprachen.DE_GROUPBOX1);
                groupBox3.Text = (Sprachen.DE_GROUPBOX3);
                groupBox2.Text = (Sprachen.DE_GROUPBOX2);

                //DE Labels
                //label4.Text = (Sprachen.DE_LABEL4);
                label9.Text = (Sprachen.DE_LABLE9);

                //DE Tooltip
                System.Windows.Forms.ToolTip ToolTipDE = new System.Windows.Forms.ToolTip();
                ToolTipDE.SetToolTip(this.infoP, Sprachen.DE_INFOP);
                ToolTipDE.SetToolTip(this.infoE, Sprachen.DE_INFOE);
            }
        }

        private void pic_de_Click(object sender, EventArgs e)
        {
            pic_de.SizeMode = PictureBoxSizeMode.StretchImage;
            pic_en.SizeMode = PictureBoxSizeMode.Normal;
            sprachen(); 
        }

        private void pic_en_Click(object sender, EventArgs e)
        {
            pic_en.SizeMode = PictureBoxSizeMode.StretchImage;
            pic_de.SizeMode = PictureBoxSizeMode.Normal;
            sprachen(); 
        }

        private void btn_back_Click(object sender, EventArgs e)
        {
            // Datenweitergabe

            // sicherheitsbest fuellen
            sicherheitsbest[0, 0] = 1;
            sicherheitsbest[0, 1] = Convert.ToInt32(Eingabe_P1.Text);
            sicherheitsbest[1, 0] = 2;
            sicherheitsbest[1, 1] = Convert.ToInt32(Eingabe_P2.Text);
            sicherheitsbest[2, 0] = 3;
            sicherheitsbest[2, 1] = Convert.ToInt32(Eingabe_P3.Text);
            sicherheitsbest[3, 0] = 4;
            sicherheitsbest[3, 1] = Convert.ToInt32(E041.Text);
            sicherheitsbest[4, 0] = 5;
            sicherheitsbest[4, 1] = Convert.ToInt32(E052.Text);
            sicherheitsbest[5, 0] = 6;
            sicherheitsbest[5, 1] = Convert.ToInt32(E063.Text);
            sicherheitsbest[6, 0] = 7;
            sicherheitsbest[6, 1] = Convert.ToInt32(E071.Text);
            sicherheitsbest[7, 0] = 8;
            sicherheitsbest[7, 1] = Convert.ToInt32(E082.Text);
            sicherheitsbest[8, 0] = 9;
            sicherheitsbest[8, 1] = Convert.ToInt32(E093.Text);
            sicherheitsbest[9, 0] = 10;
            sicherheitsbest[9, 1] = Convert.ToInt32(E101.Text);
            sicherheitsbest[10, 0] = 11;
            sicherheitsbest[10, 1] = Convert.ToInt32(E112.Text);
            sicherheitsbest[11, 0] = 12;
            sicherheitsbest[11, 1] = Convert.ToInt32(E123.Text);
            sicherheitsbest[12, 0] = 13;
            sicherheitsbest[12, 1] = Convert.ToInt32(E131.Text);
            sicherheitsbest[13, 0] = 14;
            sicherheitsbest[13, 1] = Convert.ToInt32(E142.Text);
            sicherheitsbest[14, 0] = 15;
            sicherheitsbest[14, 1] = Convert.ToInt32(E153.Text);
            int wert16 = Convert.ToInt32(E161.Text) + Convert.ToInt32(E162.Text) + Convert.ToInt32(E163.Text);
            sicherheitsbest[15, 0] = 16;
            sicherheitsbest[15, 1] = wert16;
            sicherheitsbest[15, 2] = Convert.ToInt32(E161.Text);
            sicherheitsbest[15, 3] = Convert.ToInt32(E162.Text);
            sicherheitsbest[15, 4] = Convert.ToInt32(E163.Text);
            int wert17 = Convert.ToInt32(E171.Text) + Convert.ToInt32(E172.Text) + Convert.ToInt32(E173.Text);
            sicherheitsbest[16, 0] = 17;
            sicherheitsbest[16, 1] = wert17;
            sicherheitsbest[16, 2] = Convert.ToInt32(E171.Text);
            sicherheitsbest[16, 3] = Convert.ToInt32(E172.Text);
            sicherheitsbest[16, 4] = Convert.ToInt32(E173.Text);
            sicherheitsbest[17, 0] = 18;
            sicherheitsbest[17, 1] = Convert.ToInt32(E181.Text);
            sicherheitsbest[18, 0] = 19;
            sicherheitsbest[18, 1] = Convert.ToInt32(E192.Text);
            sicherheitsbest[19, 0] = 20;
            sicherheitsbest[19, 1] = Convert.ToInt32(E203.Text);
            int wert26 = Convert.ToInt32(E261.Text) + Convert.ToInt32(E262.Text) + Convert.ToInt32(E263.Text);
            sicherheitsbest[20, 0] = 26;
            sicherheitsbest[20, 1] = wert26;
            sicherheitsbest[20, 2] = Convert.ToInt32(E261.Text);
            sicherheitsbest[20, 3] = Convert.ToInt32(E262.Text);
            sicherheitsbest[20, 4] = Convert.ToInt32(E263.Text);
            sicherheitsbest[21, 0] = 29;
            sicherheitsbest[21, 1] = Convert.ToInt32(E293.Text);
            sicherheitsbest[22, 0] = 30;
            sicherheitsbest[22, 1] = Convert.ToInt32(E303.Text);
            sicherheitsbest[23, 0] = 31;
            sicherheitsbest[23, 1] = Convert.ToInt32(E313.Text);
            sicherheitsbest[24, 0] = 49;
            sicherheitsbest[24, 1] = Convert.ToInt32(E491.Text);
            sicherheitsbest[25, 0] = 50;
            sicherheitsbest[25, 1] = Convert.ToInt32(E501.Text);
            sicherheitsbest[26, 0] = 51;
            sicherheitsbest[26, 1] = Convert.ToInt32(E511.Text);
            sicherheitsbest[27, 0] = 54;
            sicherheitsbest[27, 1] = Convert.ToInt32(E542.Text);
            sicherheitsbest[28, 0] = 55;
            sicherheitsbest[28, 1] = Convert.ToInt32(E552.Text);
            sicherheitsbest[29, 0] = 56;
            sicherheitsbest[29, 1] = Convert.ToInt32(E562.Text);

            this.Controls.Clear();
            UserControl import = new ImportPrognose(aktPeriode, auftraege, direktverkaeufe,
                sicherheitsbest, produktion, produktionProg, prodReihenfolge, kapazitaet, kaufauftraege, sprache);
            this.Controls.Add(import);
        }

        private void continue_btn_Click(object sender, EventArgs e)
        {

            if (E041.Text == "0" | E052.Text == "0" | E063.Text == "0" | E071.Text == "0" | E082.Text == "0" | E093.Text == "0" | E101.Text == "0" | E112.Text == "0" | E123.Text == "0" | E131.Text == "0" | E142.Text == "0" | E153.Text == "0" | E161.Text == "0" | E162.Text == "0" | E163.Text == "0" | E171.Text == "0" | E172.Text == "0" | E173.Text == "0" | E181.Text == "0" | E192.Text == "0" | E203.Text == "0" | E261.Text == "0" | E262.Text == "0" | E263.Text == "0" | E293.Text == "0" | E303.Text == "0" | E313.Text == "0" | E491.Text == "0" | E501.Text == "0" | E511.Text == "0" | E542.Text == "0" | E552.Text == "0" | E562.Text == "0")
            {
                valueZero();
                DialogResult dialogResult;
                if (pic_de.SizeMode == PictureBoxSizeMode.StretchImage)
                {
                    dialogResult = MessageBox.Show("In Ihren Eingaben sind noch einige Felder mit der Eingabe 0. Ist dies gewollt?", "Wollen Sie fortfahren?", MessageBoxButtons.YesNo);
                }
                else
                {
                    dialogResult = MessageBox.Show("In your entries are still some fields with the input 0. Is this correct?", "Do you want to continue?", MessageBoxButtons.YesNo);
                }
                if (dialogResult == DialogResult.Yes)
                {
                    // Datenweitergabe

                    // sicherheitsbest fuellen
                    sicherheitsbest[0, 0] = 1;
                    sicherheitsbest[0, 1] = Convert.ToInt32(Eingabe_P1.Text);
                    sicherheitsbest[1, 0] = 2;
                    sicherheitsbest[1, 1] = Convert.ToInt32(Eingabe_P2.Text);
                    sicherheitsbest[2, 0] = 3;
                    sicherheitsbest[2, 1] = Convert.ToInt32(Eingabe_P3.Text);
                    sicherheitsbest[3, 0] = 4;
                    sicherheitsbest[3, 1] = Convert.ToInt32(E041.Text);
                    sicherheitsbest[4, 0] = 5;
                    sicherheitsbest[4, 1] = Convert.ToInt32(E052.Text);
                    sicherheitsbest[5, 0] = 6;
                    sicherheitsbest[5, 1] = Convert.ToInt32(E063.Text);
                    sicherheitsbest[6, 0] = 7;
                    sicherheitsbest[6, 1] = Convert.ToInt32(E071.Text);
                    sicherheitsbest[7, 0] = 8;
                    sicherheitsbest[7, 1] = Convert.ToInt32(E082.Text);
                    sicherheitsbest[8, 0] = 9;
                    sicherheitsbest[8, 1] = Convert.ToInt32(E093.Text);
                    sicherheitsbest[9, 0] = 10;
                    sicherheitsbest[9, 1] = Convert.ToInt32(E101.Text);
                    sicherheitsbest[10, 0] = 11;
                    sicherheitsbest[10, 1] = Convert.ToInt32(E112.Text);
                    sicherheitsbest[11, 0] = 12;
                    sicherheitsbest[11, 1] = Convert.ToInt32(E123.Text);
                    sicherheitsbest[12, 0] = 13;
                    sicherheitsbest[12, 1] = Convert.ToInt32(E131.Text);
                    sicherheitsbest[13, 0] = 14;
                    sicherheitsbest[13, 1] = Convert.ToInt32(E142.Text);
                    sicherheitsbest[14, 0] = 15;
                    sicherheitsbest[14, 1] = Convert.ToInt32(E153.Text);
                    int wert16 = Convert.ToInt32(E161.Text) + Convert.ToInt32(E162.Text) + Convert.ToInt32(E163.Text);
                    sicherheitsbest[15, 0] = 16;
                    sicherheitsbest[15, 1] = wert16;
                    sicherheitsbest[15, 2] = Convert.ToInt32(E161.Text);
                    sicherheitsbest[15, 3] = Convert.ToInt32(E162.Text);
                    sicherheitsbest[15, 4] = Convert.ToInt32(E163.Text);
                    int wert17 = Convert.ToInt32(E171.Text) + Convert.ToInt32(E172.Text) + Convert.ToInt32(E173.Text);
                    sicherheitsbest[16, 0] = 17;
                    sicherheitsbest[16, 1] = wert17;
                    sicherheitsbest[16, 2] = Convert.ToInt32(E171.Text);
                    sicherheitsbest[16, 3] = Convert.ToInt32(E172.Text);
                    sicherheitsbest[16, 4] = Convert.ToInt32(E173.Text);
                    sicherheitsbest[17, 0] = 18;
                    sicherheitsbest[17, 1] = Convert.ToInt32(E181.Text);
                    sicherheitsbest[18, 0] = 19;
                    sicherheitsbest[18, 1] = Convert.ToInt32(E192.Text);
                    sicherheitsbest[19, 0] = 20;
                    sicherheitsbest[19, 1] = Convert.ToInt32(E203.Text);
                    int wert26 = Convert.ToInt32(E261.Text) + Convert.ToInt32(E262.Text) + Convert.ToInt32(E263.Text);
                    sicherheitsbest[20, 0] = 26;
                    sicherheitsbest[20, 1] = wert26;
                    sicherheitsbest[20, 2] = Convert.ToInt32(E261.Text);
                    sicherheitsbest[20, 3] = Convert.ToInt32(E262.Text);
                    sicherheitsbest[20, 4] = Convert.ToInt32(E263.Text);
                    sicherheitsbest[21, 0] = 29;
                    sicherheitsbest[21, 1] = Convert.ToInt32(E293.Text);
                    sicherheitsbest[22, 0] = 30;
                    sicherheitsbest[22, 1] = Convert.ToInt32(E303.Text);
                    sicherheitsbest[23, 0] = 31;
                    sicherheitsbest[23, 1] = Convert.ToInt32(E313.Text);
                    sicherheitsbest[24, 0] = 49;
                    sicherheitsbest[24, 1] = Convert.ToInt32(E491.Text);
                    sicherheitsbest[25, 0] = 50;
                    sicherheitsbest[25, 1] = Convert.ToInt32(E501.Text);
                    sicherheitsbest[26, 0] = 51;
                    sicherheitsbest[26, 1] = Convert.ToInt32(E511.Text);
                    sicherheitsbest[27, 0] = 54;
                    sicherheitsbest[27, 1] = Convert.ToInt32(E542.Text);
                    sicherheitsbest[28, 0] = 55;
                    sicherheitsbest[28, 1] = Convert.ToInt32(E552.Text);
                    sicherheitsbest[29, 0] = 56;
                    sicherheitsbest[29, 1] = Convert.ToInt32(E562.Text);

                    this.Controls.Clear();
                    UserControl prod = new Produktion(aktPeriode, auftraege, direktverkaeufe,
                        sicherheitsbest, produktion, produktionProg, prodReihenfolge, kapazitaet, kaufauftraege, sprache);
                    this.Controls.Add(prod);
                }
            }
            else
            {
                // Datenweitergabe

                // sicherheitsbest fuellen
                sicherheitsbest[0, 0] = 1;
                sicherheitsbest[0, 1] = Convert.ToInt32(Eingabe_P1.Text);
                sicherheitsbest[1, 0] = 2;
                sicherheitsbest[1, 1] = Convert.ToInt32(Eingabe_P2.Text);
                sicherheitsbest[2, 0] = 3;
                sicherheitsbest[2, 1] = Convert.ToInt32(Eingabe_P3.Text);
                sicherheitsbest[3, 0] = 4;
                sicherheitsbest[3, 1] = Convert.ToInt32(E041.Text);
                sicherheitsbest[4, 0] = 5;
                sicherheitsbest[4, 1] = Convert.ToInt32(E052.Text);
                sicherheitsbest[5, 0] = 6;
                sicherheitsbest[5, 1] = Convert.ToInt32(E063.Text);
                sicherheitsbest[6, 0] = 7;
                sicherheitsbest[6, 1] = Convert.ToInt32(E071.Text);
                sicherheitsbest[7, 0] = 8;
                sicherheitsbest[7, 1] = Convert.ToInt32(E082.Text);
                sicherheitsbest[8, 0] = 9;
                sicherheitsbest[8, 1] = Convert.ToInt32(E093.Text);
                sicherheitsbest[9, 0] = 10;
                sicherheitsbest[9, 1] = Convert.ToInt32(E101.Text);
                sicherheitsbest[10, 0] = 11;
                sicherheitsbest[10, 1] = Convert.ToInt32(E112.Text);
                sicherheitsbest[11, 0] = 12;
                sicherheitsbest[11, 1] = Convert.ToInt32(E123.Text);
                sicherheitsbest[12, 0] = 13;
                sicherheitsbest[12, 1] = Convert.ToInt32(E131.Text);
                sicherheitsbest[13, 0] = 14;
                sicherheitsbest[13, 1] = Convert.ToInt32(E142.Text);
                sicherheitsbest[14, 0] = 15;
                sicherheitsbest[14, 1] = Convert.ToInt32(E153.Text);
                int wert16 = Convert.ToInt32(E161.Text) + Convert.ToInt32(E162.Text) + Convert.ToInt32(E163.Text);
                sicherheitsbest[15, 0] = 16;
                sicherheitsbest[15, 1] = wert16;
                sicherheitsbest[15, 2] = Convert.ToInt32(E161.Text);
                sicherheitsbest[15, 3] = Convert.ToInt32(E162.Text);
                sicherheitsbest[15, 4] = Convert.ToInt32(E163.Text);
                int wert17 = Convert.ToInt32(E171.Text) + Convert.ToInt32(E172.Text) + Convert.ToInt32(E173.Text);
                sicherheitsbest[16, 0] = 17;
                sicherheitsbest[16, 1] = wert17;
                sicherheitsbest[16, 2] = Convert.ToInt32(E171.Text);
                sicherheitsbest[16, 3] = Convert.ToInt32(E172.Text);
                sicherheitsbest[16, 4] = Convert.ToInt32(E173.Text);
                sicherheitsbest[17, 0] = 18;
                sicherheitsbest[17, 1] = Convert.ToInt32(E181.Text);
                sicherheitsbest[18, 0] = 19;
                sicherheitsbest[18, 1] = Convert.ToInt32(E192.Text);
                sicherheitsbest[19, 0] = 20;
                sicherheitsbest[19, 1] = Convert.ToInt32(E203.Text);
                int wert26 = Convert.ToInt32(E261.Text) + Convert.ToInt32(E262.Text) + Convert.ToInt32(E263.Text);
                sicherheitsbest[20, 0] = 26;
                sicherheitsbest[20, 1] = wert26;
                sicherheitsbest[20, 2] = Convert.ToInt32(E261.Text);
                sicherheitsbest[20, 3] = Convert.ToInt32(E262.Text);
                sicherheitsbest[20, 4] = Convert.ToInt32(E263.Text);
                sicherheitsbest[21, 0] = 29;
                sicherheitsbest[21, 1] = Convert.ToInt32(E293.Text);
                sicherheitsbest[22, 0] = 30;
                sicherheitsbest[22, 1] = Convert.ToInt32(E303.Text);
                sicherheitsbest[23, 0] = 31;
                sicherheitsbest[23, 1] = Convert.ToInt32(E313.Text);
                sicherheitsbest[24, 0] = 49;
                sicherheitsbest[24, 1] = Convert.ToInt32(E491.Text);
                sicherheitsbest[25, 0] = 50;
                sicherheitsbest[25, 1] = Convert.ToInt32(E501.Text);
                sicherheitsbest[26, 0] = 51;
                sicherheitsbest[26, 1] = Convert.ToInt32(E511.Text);
                sicherheitsbest[27, 0] = 54;
                sicherheitsbest[27, 1] = Convert.ToInt32(E542.Text);
                sicherheitsbest[28, 0] = 55;
                sicherheitsbest[28, 1] = Convert.ToInt32(E552.Text);
                sicherheitsbest[29, 0] = 56;
                sicherheitsbest[29, 1] = Convert.ToInt32(E562.Text);

                this.Controls.Clear();
                UserControl prod = new Produktion(aktPeriode, auftraege, direktverkaeufe,
                    sicherheitsbest, produktion, produktionProg, prodReihenfolge, kapazitaet, kaufauftraege, sprache);
                this.Controls.Add(prod);
            }
        }

        private void lbl_Startseite_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(Eingabe_P1.Text) == false)
            {
                if (String.IsNullOrEmpty(Eingabe_P2.Text) == false)
                {
                    if (String.IsNullOrEmpty(Eingabe_P3.Text) == false)
                    {
                        if (continue_btn.Enabled == true)
                        {
                            // Datenweitergabe

                            // sicherheitsbest fuellen
                            sicherheitsbest[0, 0] = 1;
                            sicherheitsbest[0, 1] = Convert.ToInt32(Eingabe_P1.Text);
                            sicherheitsbest[1, 0] = 2;
                            sicherheitsbest[1, 1] = Convert.ToInt32(Eingabe_P2.Text);
                            sicherheitsbest[2, 0] = 3;
                            sicherheitsbest[2, 1] = Convert.ToInt32(Eingabe_P3.Text);
                            sicherheitsbest[3, 0] = 4;
                            sicherheitsbest[3, 1] = Convert.ToInt32(E041.Text);
                            sicherheitsbest[4, 0] = 5;
                            sicherheitsbest[4, 1] = Convert.ToInt32(E052.Text);
                            sicherheitsbest[5, 0] = 6;
                            sicherheitsbest[5, 1] = Convert.ToInt32(E063.Text);
                            sicherheitsbest[6, 0] = 7;
                            sicherheitsbest[6, 1] = Convert.ToInt32(E071.Text);
                            sicherheitsbest[7, 0] = 8;
                            sicherheitsbest[7, 1] = Convert.ToInt32(E082.Text);
                            sicherheitsbest[8, 0] = 9;
                            sicherheitsbest[8, 1] = Convert.ToInt32(E093.Text);
                            sicherheitsbest[9, 0] = 10;
                            sicherheitsbest[9, 1] = Convert.ToInt32(E101.Text);
                            sicherheitsbest[10, 0] = 11;
                            sicherheitsbest[10, 1] = Convert.ToInt32(E112.Text);
                            sicherheitsbest[11, 0] = 12;
                            sicherheitsbest[11, 1] = Convert.ToInt32(E123.Text);
                            sicherheitsbest[12, 0] = 13;
                            sicherheitsbest[12, 1] = Convert.ToInt32(E131.Text);
                            sicherheitsbest[13, 0] = 14;
                            sicherheitsbest[13, 1] = Convert.ToInt32(E142.Text);
                            sicherheitsbest[14, 0] = 15;
                            sicherheitsbest[14, 1] = Convert.ToInt32(E153.Text);
                            int wert16 = Convert.ToInt32(E161.Text) + Convert.ToInt32(E162.Text) + Convert.ToInt32(E163.Text);
                            sicherheitsbest[15, 0] = 16;
                            sicherheitsbest[15, 1] = wert16;
                            sicherheitsbest[15, 2] = Convert.ToInt32(E161.Text);
                            sicherheitsbest[15, 3] = Convert.ToInt32(E162.Text);
                            sicherheitsbest[15, 4] = Convert.ToInt32(E163.Text);
                            int wert17 = Convert.ToInt32(E171.Text) + Convert.ToInt32(E172.Text) + Convert.ToInt32(E173.Text);
                            sicherheitsbest[16, 0] = 17;
                            sicherheitsbest[16, 1] = wert17;
                            sicherheitsbest[16, 2] = Convert.ToInt32(E171.Text);
                            sicherheitsbest[16, 3] = Convert.ToInt32(E172.Text);
                            sicherheitsbest[16, 4] = Convert.ToInt32(E173.Text);
                            sicherheitsbest[17, 0] = 18;
                            sicherheitsbest[17, 1] = Convert.ToInt32(E181.Text);
                            sicherheitsbest[18, 0] = 19;
                            sicherheitsbest[18, 1] = Convert.ToInt32(E192.Text);
                            sicherheitsbest[19, 0] = 20;
                            sicherheitsbest[19, 1] = Convert.ToInt32(E203.Text);
                            int wert26 = Convert.ToInt32(E261.Text) + Convert.ToInt32(E262.Text) + Convert.ToInt32(E263.Text);
                            sicherheitsbest[20, 0] = 26;
                            sicherheitsbest[20, 1] = wert26;
                            sicherheitsbest[20, 2] = Convert.ToInt32(E261.Text);
                            sicherheitsbest[20, 3] = Convert.ToInt32(E262.Text);
                            sicherheitsbest[20, 4] = Convert.ToInt32(E263.Text);
                            sicherheitsbest[21, 0] = 29;
                            sicherheitsbest[21, 1] = Convert.ToInt32(E293.Text);
                            sicherheitsbest[22, 0] = 30;
                            sicherheitsbest[22, 1] = Convert.ToInt32(E303.Text);
                            sicherheitsbest[23, 0] = 31;
                            sicherheitsbest[23, 1] = Convert.ToInt32(E313.Text);
                            sicherheitsbest[24, 0] = 49;
                            sicherheitsbest[24, 1] = Convert.ToInt32(E491.Text);
                            sicherheitsbest[25, 0] = 50;
                            sicherheitsbest[25, 1] = Convert.ToInt32(E501.Text);
                            sicherheitsbest[26, 0] = 51;
                            sicherheitsbest[26, 1] = Convert.ToInt32(E511.Text);
                            sicherheitsbest[27, 0] = 54;
                            sicherheitsbest[27, 1] = Convert.ToInt32(E542.Text);
                            sicherheitsbest[28, 0] = 55;
                            sicherheitsbest[28, 1] = Convert.ToInt32(E552.Text);
                            sicherheitsbest[29, 0] = 56;
                            sicherheitsbest[29, 1] = Convert.ToInt32(E562.Text);

                            this.Controls.Clear();
                            UserControl import = new ImportPrognose(aktPeriode, auftraege, direktverkaeufe,
                                sicherheitsbest, produktion, produktionProg, prodReihenfolge, kapazitaet, kaufauftraege, sprache);
                            this.Controls.Add(import);
                        }
                    }

                }
            }
        }

        private void lbl_Auftraege_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(Eingabe_P1.Text) == false)
            {
                if (String.IsNullOrEmpty(Eingabe_P2.Text) == false)
                {
                    if (String.IsNullOrEmpty(Eingabe_P3.Text) == false)
                    {
                        if (continue_btn.Enabled == true)
                        {
                            // Datenweitergabe

                            // sicherheitsbest fuellen
                            sicherheitsbest[0, 0] = 1;
                            sicherheitsbest[0, 1] = Convert.ToInt32(Eingabe_P1.Text);
                            sicherheitsbest[1, 0] = 2;
                            sicherheitsbest[1, 1] = Convert.ToInt32(Eingabe_P2.Text);
                            sicherheitsbest[2, 0] = 3;
                            sicherheitsbest[2, 1] = Convert.ToInt32(Eingabe_P3.Text);
                            sicherheitsbest[3, 0] = 4;
                            sicherheitsbest[3, 1] = Convert.ToInt32(E041.Text);
                            sicherheitsbest[4, 0] = 5;
                            sicherheitsbest[4, 1] = Convert.ToInt32(E052.Text);
                            sicherheitsbest[5, 0] = 6;
                            sicherheitsbest[5, 1] = Convert.ToInt32(E063.Text);
                            sicherheitsbest[6, 0] = 7;
                            sicherheitsbest[6, 1] = Convert.ToInt32(E071.Text);
                            sicherheitsbest[7, 0] = 8;
                            sicherheitsbest[7, 1] = Convert.ToInt32(E082.Text);
                            sicherheitsbest[8, 0] = 9;
                            sicherheitsbest[8, 1] = Convert.ToInt32(E093.Text);
                            sicherheitsbest[9, 0] = 10;
                            sicherheitsbest[9, 1] = Convert.ToInt32(E101.Text);
                            sicherheitsbest[10, 0] = 11;
                            sicherheitsbest[10, 1] = Convert.ToInt32(E112.Text);
                            sicherheitsbest[11, 0] = 12;
                            sicherheitsbest[11, 1] = Convert.ToInt32(E123.Text);
                            sicherheitsbest[12, 0] = 13;
                            sicherheitsbest[12, 1] = Convert.ToInt32(E131.Text);
                            sicherheitsbest[13, 0] = 14;
                            sicherheitsbest[13, 1] = Convert.ToInt32(E142.Text);
                            sicherheitsbest[14, 0] = 15;
                            sicherheitsbest[14, 1] = Convert.ToInt32(E153.Text);
                            int wert16 = Convert.ToInt32(E161.Text) + Convert.ToInt32(E162.Text) + Convert.ToInt32(E163.Text);
                            sicherheitsbest[15, 0] = 16;
                            sicherheitsbest[15, 1] = wert16;
                            sicherheitsbest[15, 2] = Convert.ToInt32(E161.Text);
                            sicherheitsbest[15, 3] = Convert.ToInt32(E162.Text);
                            sicherheitsbest[15, 4] = Convert.ToInt32(E163.Text);
                            int wert17 = Convert.ToInt32(E171.Text) + Convert.ToInt32(E172.Text) + Convert.ToInt32(E173.Text);
                            sicherheitsbest[16, 0] = 17;
                            sicherheitsbest[16, 1] = wert17;
                            sicherheitsbest[16, 2] = Convert.ToInt32(E171.Text);
                            sicherheitsbest[16, 3] = Convert.ToInt32(E172.Text);
                            sicherheitsbest[16, 4] = Convert.ToInt32(E173.Text);
                            sicherheitsbest[17, 0] = 18;
                            sicherheitsbest[17, 1] = Convert.ToInt32(E181.Text);
                            sicherheitsbest[18, 0] = 19;
                            sicherheitsbest[18, 1] = Convert.ToInt32(E192.Text);
                            sicherheitsbest[19, 0] = 20;
                            sicherheitsbest[19, 1] = Convert.ToInt32(E203.Text);
                            int wert26 = Convert.ToInt32(E261.Text) + Convert.ToInt32(E262.Text) + Convert.ToInt32(E263.Text);
                            sicherheitsbest[20, 0] = 26;
                            sicherheitsbest[20, 1] = wert26;
                            sicherheitsbest[20, 2] = Convert.ToInt32(E261.Text);
                            sicherheitsbest[20, 3] = Convert.ToInt32(E262.Text);
                            sicherheitsbest[20, 4] = Convert.ToInt32(E263.Text);
                            sicherheitsbest[21, 0] = 29;
                            sicherheitsbest[21, 1] = Convert.ToInt32(E293.Text);
                            sicherheitsbest[22, 0] = 30;
                            sicherheitsbest[22, 1] = Convert.ToInt32(E303.Text);
                            sicherheitsbest[23, 0] = 31;
                            sicherheitsbest[23, 1] = Convert.ToInt32(E313.Text);
                            sicherheitsbest[24, 0] = 49;
                            sicherheitsbest[24, 1] = Convert.ToInt32(E491.Text);
                            sicherheitsbest[25, 0] = 50;
                            sicherheitsbest[25, 1] = Convert.ToInt32(E501.Text);
                            sicherheitsbest[26, 0] = 51;
                            sicherheitsbest[26, 1] = Convert.ToInt32(E511.Text);
                            sicherheitsbest[27, 0] = 54;
                            sicherheitsbest[27, 1] = Convert.ToInt32(E542.Text);
                            sicherheitsbest[28, 0] = 55;
                            sicherheitsbest[28, 1] = Convert.ToInt32(E552.Text);
                            sicherheitsbest[29, 0] = 56;
                            sicherheitsbest[29, 1] = Convert.ToInt32(E562.Text);

                            this.Controls.Clear();
                            UserControl prod = new Produktion(aktPeriode, auftraege, direktverkaeufe,
                                sicherheitsbest, produktion, produktionProg, prodReihenfolge, kapazitaet, kaufauftraege, sprache);
                            this.Controls.Add(prod);
                        }
                    }
                }
            }
        }
        private void valueZero()
        {
            if (Eingabe_P1.Text == "0")
            {
                Eingabe_P1.ForeColor = Color.Red;
            }
            if (Eingabe_P2.Text == "0")
            {
                Eingabe_P2.ForeColor = Color.Red;
            }
            if (Eingabe_P3.Text == "0")
            {
                Eingabe_P3.ForeColor = Color.Red;
            }
            if (E041.Text == "0")
            {
                E041.ForeColor = Color.Red;
            }
            if (E052.Text == "0")
            {
                E052.ForeColor = Color.Red;
            }
            if (E063.Text == "0")
            {
                E063.ForeColor = Color.Red;
            }
            if (E071.Text == "0")
            {
                E071.ForeColor = Color.Red;
            }
            if (E082.Text == "0")
            {
                E082.ForeColor = Color.Red;
            }
            if (E093.Text == "0")
            {
                E093.ForeColor = Color.Red;
            }
            if (E101.Text == "0")
            {
                E101.ForeColor = Color.Red;
            }
            if (E112.Text == "0")
            {
                E112.ForeColor = Color.Red;
            }
            if (E123.Text == "0")
            {
                E123.ForeColor = Color.Red;
            }
            if (E131.Text == "0")
            {
                E131.ForeColor = Color.Red;
            }
            if (E142.Text == "0")
            {
                E142.ForeColor = Color.Red;
            }
            if (E153.Text == "0")
            {
                E153.ForeColor = Color.Red;
            }
            if (E161.Text == "0")
            {
                E161.ForeColor = Color.Red;
            }
            if (E162.Text == "0")
            {
                E162.ForeColor = Color.Red;
            }
            if (E163.Text == "0")
            {
                E163.ForeColor = Color.Red;
            }
            if (E171.Text == "0")
            {
                E171.ForeColor = Color.Red;
            }
            if (E172.Text == "0")
            {
                E172.ForeColor = Color.Red;
            }
            if (E173.Text == "0")
            {
                E173.ForeColor = Color.Red;
            }
            if (E181.Text == "0")
            {
                E181.ForeColor = Color.Red;
            }
            if (E192.Text == "0")
            {
                E192.ForeColor = Color.Red;
            }
            if (E203.Text == "0")
            {
                E203.ForeColor = Color.Red;
            }
            if (E261.Text == "0")
            {
                E261.ForeColor = Color.Red;
            }
            if (E262.Text == "0")
            {
                E262.ForeColor = Color.Red;
            }
            if (E263.Text == "0")
            {
                E263.ForeColor = Color.Red;
            }
            if (E293.Text == "0")
            {
                E293.ForeColor = Color.Red;
            }
            if (E303.Text == "0")
            {
                E303.ForeColor = Color.Red;
            }
            if (E313.Text == "0")
            {
                E313.ForeColor = Color.Red;
            }
            if (E491.Text == "0")
            {
                E491.ForeColor = Color.Red;
            }
            if (E501.Text == "0")
            {
                E501.ForeColor = Color.Red;
            }
            if (E511.Text == "0")
            {
                E511.ForeColor = Color.Red;
            }
            if (E542.Text == "0")
            {
                E542.ForeColor = Color.Red;
            }
            if (E552.Text == "0")
            {
                E552.ForeColor = Color.Red;
            }
            if (E562.Text == "0")
            {
                E562.ForeColor = Color.Red;
            }
        }
    }
}
