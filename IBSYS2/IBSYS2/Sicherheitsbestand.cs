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
        bool tP1 = false, tP2 = false, tP3 = false;


        public Sicherheitsbestand()
        {
            InitializeComponent();
            continue_btn.Enabled = true;
            default_btn.Enabled = false;
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
            double sicherheitsbestandP1 = sicherheitsbestandBerechnen(gLagerbestandP1, "1");
            Ausgabe_P1.Text = Convert.ToString(sicherheitsbestandP1);
            double sicherheitsbestandP2 = sicherheitsbestandBerechnen(gLagerbestandP2, "2");
            Ausgabe_P2.Text = Convert.ToString(sicherheitsbestandP2);
            double sicherheitsbestandP3 = sicherheitsbestandBerechnen(gLagerbestandP3, "3");
            Ausgabe_P3.Text = Convert.ToString(sicherheitsbestandP3);


            double gLE26P1 = geplanterLagerbestand(sicherheitsbestandP1, 100);
            E261.Text = Convert.ToString(gLE26P1);
            double gLE51P1 = geplanterLagerbestand(sicherheitsbestandP1, 100);
            E511.Text = Convert.ToString(gLE51P1);
            double gLE16P1 = geplanterLagerbestand(sicherheitsbestandP1, 100);
            E161.Text = Convert.ToString(gLE16P1);
            double gLE17P1 = geplanterLagerbestand(sicherheitsbestandP1, 100);
            E171.Text = Convert.ToString(gLE17P1);
            double gLE50P1 = geplanterLagerbestand(sicherheitsbestandP1, 100);
            E501.Text = Convert.ToString(gLE50P1);
            double gLE4P1 = geplanterLagerbestand(sicherheitsbestandP1, 100);
            E041.Text = Convert.ToString(gLE4P1);
            double gLE10P1 = geplanterLagerbestand(sicherheitsbestandP1, 100);
            E101.Text = Convert.ToString(gLE10P1);
            double gLE49P1 = geplanterLagerbestand(sicherheitsbestandP1, 100);
            E491.Text = Convert.ToString(gLE49P1);
            double gLE7P1 = geplanterLagerbestand(sicherheitsbestandP1, 100);
            E071.Text = Convert.ToString(gLE7P1);
            double gLE13P1 = geplanterLagerbestand(sicherheitsbestandP1, 100);
            E131.Text = Convert.ToString(gLE13P1);
            double gLE18P1 = geplanterLagerbestand(sicherheitsbestandP1, 100);
            E181.Text = Convert.ToString(gLE18P1);

            double gLE26P2 = geplanterLagerbestand(sicherheitsbestandP2, 100);
            E262.Text = Convert.ToString(gLE26P2);
            double gLE56P2 = geplanterLagerbestand(sicherheitsbestandP2, 100);
            E562.Text = Convert.ToString(gLE56P2);
            double gLE16P2 = geplanterLagerbestand(sicherheitsbestandP2, 100);
            E162.Text = Convert.ToString(gLE16P2);
            double gLE17P2 = geplanterLagerbestand(sicherheitsbestandP2, 100);
            E172.Text = Convert.ToString(gLE17P2);
            double gLE55P2 = geplanterLagerbestand(sicherheitsbestandP2, 100);
            E552.Text = Convert.ToString(gLE55P2);
            double gLE5P2 = geplanterLagerbestand(sicherheitsbestandP2, 100);
            E052.Text = Convert.ToString(gLE5P2);
            double gLE11P2 = geplanterLagerbestand(sicherheitsbestandP2, 100);
            E112.Text = Convert.ToString(gLE11P2);
            double gLE54P2 = geplanterLagerbestand(sicherheitsbestandP2, 100);
            E542.Text = Convert.ToString(gLE54P2);
            double gLE8P2 = geplanterLagerbestand(sicherheitsbestandP2, 100);
            E082.Text = Convert.ToString(gLE8P2);
            double gLE14P2 = geplanterLagerbestand(sicherheitsbestandP2, 100);
            E142.Text = Convert.ToString(gLE14P2);
            double gLE19P2 = geplanterLagerbestand(sicherheitsbestandP2, 100);
            E192.Text = Convert.ToString(gLE19P2);

            double gLE26P3 = geplanterLagerbestand(sicherheitsbestandP3, 100);
            E263.Text = Convert.ToString(gLE26P3);
            double gLE31P3 = geplanterLagerbestand(sicherheitsbestandP3, 100);
            E313.Text = Convert.ToString(gLE31P3);
            double gLE16P3 = geplanterLagerbestand(sicherheitsbestandP3, 100);
            E163.Text = Convert.ToString(gLE16P3);
            double gLE17P3 = geplanterLagerbestand(sicherheitsbestandP3, 100);
            E173.Text = Convert.ToString(gLE17P3);
            double gLE30P3 = geplanterLagerbestand(sicherheitsbestandP3, 100);
            E303.Text = Convert.ToString(gLE30P3);
            double gLE6P3 = geplanterLagerbestand(sicherheitsbestandP3, 100);
            E063.Text = Convert.ToString(gLE6P3);
            double gLE12P3 = geplanterLagerbestand(sicherheitsbestandP3, 100);
            E123.Text = Convert.ToString(gLE12P3);
            double gLE29P3 = geplanterLagerbestand(sicherheitsbestandP3, 100);
            E293.Text = Convert.ToString(gLE29P3);
            double gLE9P3 = geplanterLagerbestand(sicherheitsbestandP3, 100);
            E093.Text = Convert.ToString(gLE9P3);
            double gLE15P3 = geplanterLagerbestand(sicherheitsbestandP3, 100);
            E153.Text = Convert.ToString(gLE15P3);
            double gLE20P3 = geplanterLagerbestand(sicherheitsbestandP3, 100);
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

        public double sicherheitsbestandBerechnen(double gLagerbestand, string teilenummer_FK)
        {
            //TODO Wird zukünftig aus ersterm Schritt übergeben (Prognose)
            int prognose = 100;
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
            double number = 0;
            if (String.IsNullOrEmpty(E261.Text))
            {
                E261.ForeColor = Color.Red;
                if (pic_de.SizeMode == PictureBoxSizeMode.StretchImage)
                {
                    E261.Text = "Ausstehend";
                }
                else
                {
                    E261.Text = "Outstanding";
                }
            }
            else
            {
                try
                {
                    number = Convert.ToDouble(E261.Text);
                    E261.ForeColor = Color.Black;

                }
                catch
                {
                    E261.ForeColor = Color.Red;
                    if (pic_de.SizeMode == PictureBoxSizeMode.StretchImage)
                    {
                        E261.Text = "Gültige Zahl";
                    }
                    else
                    {
                        E261.Text = "Valid number";
                    }
                    return;
                }
            }
        }

        private void E511_TextChanged(object sender, EventArgs e)
        {
            double number = 0;
            if (String.IsNullOrEmpty(E511.Text))
            {
                E511.ForeColor = Color.Red;
                if (pic_de.SizeMode == PictureBoxSizeMode.StretchImage)
                {
                    E511.Text = "Ausstehend";
                }
                else
                {
                    E511.Text = "Outstanding";
                }
            }
            else
            {
                try
                {
                    number = Convert.ToDouble(E511.Text);
                    E511.ForeColor = Color.Black;

                }
                catch
                {
                    E511.ForeColor = Color.Red;
                    if (pic_de.SizeMode == PictureBoxSizeMode.StretchImage)
                    {
                        E511.Text = "Gültige Zahl";
                    }
                    else
                    {
                        E511.Text = "Valid number";
                    } 
                    return;
                }
            }
        }

        private void E161_TextChanged(object sender, EventArgs e)
        {
            double number = 0;
            if (String.IsNullOrEmpty(E161.Text))
            {
                E161.ForeColor = Color.Red;
                if (pic_de.SizeMode == PictureBoxSizeMode.StretchImage)
                {
                    E161.Text = "Ausstehend";
                }
                else
                {
                    E161.Text = "Outstanding";
                }
            }
            else
            {
                try
                {
                    number = Convert.ToDouble(E161.Text);
                    E161.ForeColor = Color.Black;

                }
                catch
                {
                    E161.ForeColor = Color.Red;
                    if (pic_de.SizeMode == PictureBoxSizeMode.StretchImage)
                    {
                        E161.Text = "Gültige Zahl";
                    }
                    else
                    {
                        E161.Text = "Valid number";
                    } 
                    return;
                }
            }
        }

        private void E171_TextChanged(object sender, EventArgs e)
        {
            double number = 0;
            if (String.IsNullOrEmpty(E171.Text))
            {
                E171.ForeColor = Color.Red;
                if (pic_de.SizeMode == PictureBoxSizeMode.StretchImage)
                {
                    E171.Text = "Ausstehend";
                }
                else
                {
                    E171.Text = "Outstanding";
                }
            }
            else
            {
                try
                {
                    number = Convert.ToDouble(E171.Text);
                    E171.ForeColor = Color.Black;

                }
                catch
                {
                    E171.ForeColor = Color.Red;
                    if (pic_de.SizeMode == PictureBoxSizeMode.StretchImage)
                    {
                        E171.Text = "Gültige Zahl";
                    }
                    else
                    {
                        E171.Text = "Valid number";
                    } 
                    return;
                }
            }
        }

        private void E501_TextChanged(object sender, EventArgs e)
        {
            double number = 0;
            if (String.IsNullOrEmpty(E501.Text))
            {
                E501.ForeColor = Color.Red;
                if (pic_de.SizeMode == PictureBoxSizeMode.StretchImage)
                {
                    E501.Text = "Ausstehend";
                }
                else
                {
                    E501.Text = "Outstanding";
                }
            }
            else
            {
                try
                {
                    number = Convert.ToDouble(E501.Text);
                    E501.ForeColor = Color.Black;

                }
                catch
                {
                    E501.ForeColor = Color.Red;
                    if (pic_de.SizeMode == PictureBoxSizeMode.StretchImage)
                    {
                        E501.Text = "Gültige Zahl";
                    }
                    else
                    {
                        E501.Text = "Valid number";
                    } 
                    return;
                }
            }
        }

        private void E041_TextChanged(object sender, EventArgs e)
        {
            double number = 0;
            if (String.IsNullOrEmpty(E041.Text))
            {
                E041.ForeColor = Color.Red;
                if (pic_de.SizeMode == PictureBoxSizeMode.StretchImage)
                {
                    E041.Text = "Ausstehend";
                }
                else
                {
                    E041.Text = "Outstanding";
                }
            }
            else
            {
                try
                {
                    number = Convert.ToDouble(E041.Text);
                    E041.ForeColor = Color.Black;

                }
                catch
                {
                    E041.ForeColor = Color.Red;
                    if (pic_de.SizeMode == PictureBoxSizeMode.StretchImage)
                    {
                        E041.Text = "Gültige Zahl";
                    }
                    else
                    {
                        E041.Text = "Valid number";
                    } 
                    return;
                }
            }
        }

        private void E101_TextChanged(object sender, EventArgs e)
        {
            double number = 0;
            if (String.IsNullOrEmpty(E101.Text))
            {
                E101.ForeColor = Color.Red;
                if (pic_de.SizeMode == PictureBoxSizeMode.StretchImage)
                {
                    E101.Text = "Ausstehend";
                }
                else
                {
                    E101.Text = "Outstanding";
                }
            }
            else
            {
                try
                {
                    number = Convert.ToDouble(E101.Text);
                    E101.ForeColor = Color.Black;

                }
                catch
                {
                    E101.ForeColor = Color.Red;
                    if (pic_de.SizeMode == PictureBoxSizeMode.StretchImage)
                    {
                        E101.Text = "Gültige Zahl";
                    }
                    else
                    {
                        E101.Text = "Valid number";
                    } 
                    return;
                }
            }
        }

        private void E491_TextChanged(object sender, EventArgs e)
        {
            double number = 0;
            if (String.IsNullOrEmpty(E491.Text))
            {
                E491.ForeColor = Color.Red;
                if (pic_de.SizeMode == PictureBoxSizeMode.StretchImage)
                {
                    E491.Text = "Ausstehend";
                }
                else
                {
                    E491.Text = "Outstanding";
                }
            }
            else
            {
                try
                {
                    number = Convert.ToDouble(E491.Text);
                    E491.ForeColor = Color.Black;

                }
                catch
                {
                    E491.ForeColor = Color.Red;
                    if (pic_de.SizeMode == PictureBoxSizeMode.StretchImage)
                    {
                        E491.Text = "Gültige Zahl";
                    }
                    else
                    {
                        E491.Text = "Valid number";
                    } 
                    return;
                }
            }
        }

        private void E071_TextChanged(object sender, EventArgs e)
        {
            double number = 0;
            if (String.IsNullOrEmpty(E071.Text))
            {
                E071.ForeColor = Color.Red;
                if (pic_de.SizeMode == PictureBoxSizeMode.StretchImage)
                {
                    E071.Text = "Ausstehend";
                }
                else
                {
                    E071.Text = "Outstanding";
                }
            }
            else
            {
                try
                {
                    number = Convert.ToDouble(E071.Text);
                    E071.ForeColor = Color.Black;

                }
                catch
                {
                    E071.ForeColor = Color.Red;
                    if (pic_de.SizeMode == PictureBoxSizeMode.StretchImage)
                    {
                        E071.Text = "Gültige Zahl";
                    }
                    else
                    {
                        E071.Text = "Valid number";
                    } 
                    return;
                }
            }
        }

        private void E131_TextChanged(object sender, EventArgs e)
        {
            double number = 0;
            if (String.IsNullOrEmpty(E131.Text))
            {
                E131.ForeColor = Color.Red;
                if (pic_de.SizeMode == PictureBoxSizeMode.StretchImage)
                {
                    E131.Text = "Ausstehend";
                }
                else
                {
                    E131.Text = "Outstanding";
                }
            }
            else
            {
                try
                {
                    number = Convert.ToDouble(E131.Text);
                    E131.ForeColor = Color.Black;

                }
                catch
                {
                    E131.ForeColor = Color.Red;
                    if (pic_de.SizeMode == PictureBoxSizeMode.StretchImage)
                    {
                        E131.Text = "Gültige Zahl";
                    }
                    else
                    {
                        E131.Text = "Valid number";
                    } 
                    return;
                }
            }
        }

        private void E181_TextChanged(object sender, EventArgs e)
        {
            double number = 0;
            if (String.IsNullOrEmpty(E181.Text))
            {
                E181.ForeColor = Color.Red;
                if (pic_de.SizeMode == PictureBoxSizeMode.StretchImage)
                {
                    E181.Text = "Ausstehend";
                }
                else
                {
                    E181.Text = "Outstanding";
                }
            }
            else
            {
                try
                {
                    number = Convert.ToDouble(E181.Text);
                    E181.ForeColor = Color.Black;

                }
                catch
                {
                    E181.ForeColor = Color.Red;
                    if (pic_de.SizeMode == PictureBoxSizeMode.StretchImage)
                    {
                        E181.Text = "Gültige Zahl";
                    }
                    else
                    {
                        E181.Text = "Valid number";
                    } 
                    return;
                }
            }
        }

        private void E262_TextChanged(object sender, EventArgs e)
        {
            double number = 0;
            if (String.IsNullOrEmpty(E262.Text))
            {
                E262.ForeColor = Color.Red;
                if (pic_de.SizeMode == PictureBoxSizeMode.StretchImage)
                {
                    E262.Text = "Ausstehend";
                }
                else
                {
                    E262.Text = "Outstanding";
                }
            }
            else
            {
                try
                {
                    number = Convert.ToDouble(E262.Text);
                    E262.ForeColor = Color.Black;

                }
                catch
                {
                    E262.ForeColor = Color.Red;
                    if (pic_de.SizeMode == PictureBoxSizeMode.StretchImage)
                    {
                        E262.Text = "Gültige Zahl";
                    }
                    else
                    {
                        E262.Text = "Valid number";
                    } 
                    return;
                }
            }
        }

        private void E562_TextChanged(object sender, EventArgs e)
        {
            double number = 0;
            if (String.IsNullOrEmpty(E562.Text))
            {
                E562.ForeColor = Color.Red;
                if (pic_de.SizeMode == PictureBoxSizeMode.StretchImage)
                {
                    E562.Text = "Ausstehend";
                }
                else
                {
                    E562.Text = "Outstanding";
                }
            }
            else
            {
                try
                {
                    number = Convert.ToDouble(E562.Text);
                    E562.ForeColor = Color.Black;

                }
                catch
                {
                    E562.ForeColor = Color.Red;
                    if (pic_de.SizeMode == PictureBoxSizeMode.StretchImage)
                    {
                        E562.Text = "Gültige Zahl";
                    }
                    else
                    {
                        E562.Text = "Valid number";
                    } 
                    return;
                }
            }
        }

        private void E162_TextChanged(object sender, EventArgs e)
        {
            double number = 0;
            if (String.IsNullOrEmpty(E162.Text))
            {
                E162.ForeColor = Color.Red;
                if (pic_de.SizeMode == PictureBoxSizeMode.StretchImage)
                {
                    E162.Text = "Ausstehend";
                }
                else
                {
                    E162.Text = "Outstanding";
                }
            }
            else
            {
                try
                {
                    number = Convert.ToDouble(E162.Text);
                    E162.ForeColor = Color.Black;

                }
                catch
                {
                    E162.ForeColor = Color.Red;
                    if (pic_de.SizeMode == PictureBoxSizeMode.StretchImage)
                    {
                        E162.Text = "Gültige Zahl";
                    }
                    else
                    {
                        E162.Text = "Valid number";
                    } 
                    return;
                }
            }
        }

        private void E172_TextChanged(object sender, EventArgs e)
        {
            double number = 0;
            if (String.IsNullOrEmpty(E172.Text))
            {
                E172.ForeColor = Color.Red;
                if (pic_de.SizeMode == PictureBoxSizeMode.StretchImage)
                {
                    E172.Text = "Ausstehend";
                }
                else
                {
                    E172.Text = "Outstanding";
                }
            }
            else
            {
                try
                {
                    number = Convert.ToDouble(E172.Text);
                    E172.ForeColor = Color.Black;

                }
                catch
                {
                    E172.ForeColor = Color.Red;
                    if (pic_de.SizeMode == PictureBoxSizeMode.StretchImage)
                    {
                        E172.Text = "Gültige Zahl";
                    }
                    else
                    {
                        E172.Text = "Valid number";
                    } 
                    return;
                }
            }
        }

        private void E552_TextChanged(object sender, EventArgs e)
        {
            double number = 0;
            if (String.IsNullOrEmpty(E552.Text))
            {
                E552.ForeColor = Color.Red;
                if (pic_de.SizeMode == PictureBoxSizeMode.StretchImage)
                {
                    E552.Text = "Ausstehend";
                }
                else
                {
                    E552.Text = "Outstanding";
                }
            }
            else
            {
                try
                {
                    number = Convert.ToDouble(E552.Text);
                    E552.ForeColor = Color.Black;

                }
                catch
                {
                    E552.ForeColor = Color.Red;
                    if (pic_de.SizeMode == PictureBoxSizeMode.StretchImage)
                    {
                        E552.Text = "Gültige Zahl";
                    }
                    else
                    {
                        E552.Text = "Valid number";
                    } 
                    return;
                }
            }
        }

        private void E052_TextChanged(object sender, EventArgs e)
        {
            double number = 0;
            if (String.IsNullOrEmpty(E052.Text))
            {
                E052.ForeColor = Color.Red;
                if (pic_de.SizeMode == PictureBoxSizeMode.StretchImage)
                {
                    E052.Text = "Ausstehend";
                }
                else
                {
                    E052.Text = "Outstanding";
                }
            }
            else
            {
                try
                {
                    number = Convert.ToDouble(E052.Text);
                    E052.ForeColor = Color.Black;

                }
                catch
                {
                    E052.ForeColor = Color.Red;
                    if (pic_de.SizeMode == PictureBoxSizeMode.StretchImage)
                    {
                        E052.Text = "Gültige Zahl";
                    }
                    else
                    {
                        E052.Text = "Valid number";
                    } 
                    return;
                }
            }
        }

        private void E112_TextChanged(object sender, EventArgs e)
        {
            double number = 0;
            if (String.IsNullOrEmpty(E112.Text))
            {
                E112.ForeColor = Color.Red;
                if (pic_de.SizeMode == PictureBoxSizeMode.StretchImage)
                {
                    E112.Text = "Ausstehend";
                }
                else
                {
                    E112.Text = "Outstanding";
                }
            }
            else
            {
                try
                {
                    number = Convert.ToDouble(E112.Text);
                    E112.ForeColor = Color.Black;

                }
                catch
                {
                    E112.ForeColor = Color.Red;
                    if (pic_de.SizeMode == PictureBoxSizeMode.StretchImage)
                    {
                        E112.Text = "Gültige Zahl";
                    }
                    else
                    {
                        E112.Text = "Valid number";
                    } 
                    return;
                }
            }
        }

        private void E542_TextChanged(object sender, EventArgs e)
        {
            double number = 0;
            if (String.IsNullOrEmpty(E542.Text))
            {
                E542.ForeColor = Color.Red;
                if (pic_de.SizeMode == PictureBoxSizeMode.StretchImage)
                {
                    E542.Text = "Ausstehend";
                }
                else
                {
                    E542.Text = "Outstanding";
                }
            }
            else
            {
                try
                {
                    number = Convert.ToDouble(E542.Text);
                    E542.ForeColor = Color.Black;

                }
                catch
                {
                    E542.ForeColor = Color.Red;
                    if (pic_de.SizeMode == PictureBoxSizeMode.StretchImage)
                    {
                        E542.Text = "Gültige Zahl";
                    }
                    else
                    {
                        E542.Text = "Valid number";
                    } 
                    return;
                }
            }
        }

        private void E082_TextChanged(object sender, EventArgs e)
        {
            double number = 0;
            if (String.IsNullOrEmpty(E082.Text))
            {
                E082.ForeColor = Color.Red;
                if (pic_de.SizeMode == PictureBoxSizeMode.StretchImage)
                {
                    E082.Text = "Ausstehend";
                }
                else
                {
                    E082.Text = "Outstanding";
                }
            }
            else
            {
                try
                {
                    number = Convert.ToDouble(E082.Text);
                    E082.ForeColor = Color.Black;

                }
                catch
                {
                    E082.ForeColor = Color.Red;
                    if (pic_de.SizeMode == PictureBoxSizeMode.StretchImage)
                    {
                        E082.Text = "Gültige Zahl";
                    }
                    else
                    {
                        E082.Text = "Valid number";
                    } 
                    return;
                }
            }
        }

        private void E142_TextChanged(object sender, EventArgs e)
        {
            double number = 0;
            if (String.IsNullOrEmpty(E142.Text))
            {
                E142.ForeColor = Color.Red;
                if (pic_de.SizeMode == PictureBoxSizeMode.StretchImage)
                {
                    E142.Text = "Ausstehend";
                }
                else
                {
                    E142.Text = "Outstanding";
                }
            }
            else
            {
                try
                {
                    number = Convert.ToDouble(E142.Text);
                    E142.ForeColor = Color.Black;

                }
                catch
                {
                    E142.ForeColor = Color.Red;
                    if (pic_de.SizeMode == PictureBoxSizeMode.StretchImage)
                    {
                        E142.Text = "Gültige Zahl";
                    }
                    else
                    {
                        E142.Text = "Valid number";
                    } 
                    return;
                }
            }
        }

        private void E192_TextChanged(object sender, EventArgs e)
        {
            double number = 0;
            if (String.IsNullOrEmpty(E192.Text))
            {
                E192.ForeColor = Color.Red;
                if (pic_de.SizeMode == PictureBoxSizeMode.StretchImage)
                {
                    E192.Text = "Ausstehend";
                }
                else
                {
                    E192.Text = "Outstanding";
                }
            }
            else
            {
                try
                {
                    number = Convert.ToDouble(E192.Text);
                    E192.ForeColor = Color.Black;

                }
                catch
                {
                    E192.ForeColor = Color.Red;
                    if (pic_de.SizeMode == PictureBoxSizeMode.StretchImage)
                    {
                        E192.Text = "Gültige Zahl";
                    }
                    else
                    {
                        E192.Text = "Valid number";
                    } 
                    return;
                }
            }
        }

        private void E263_TextChanged(object sender, EventArgs e)
        {
            double number = 0;
            if (String.IsNullOrEmpty(E263.Text))
            {
                E263.ForeColor = Color.Red;
                if (pic_de.SizeMode == PictureBoxSizeMode.StretchImage)
                {
                    E263.Text = "Ausstehend";
                }
                else
                {
                    E263.Text = "Outstanding";
                }
            }
            else
            {
                try
                {
                    number = Convert.ToDouble(E263.Text);
                    E263.ForeColor = Color.Black;

                }
                catch
                {
                    E263.ForeColor = Color.Red;
                    if (pic_de.SizeMode == PictureBoxSizeMode.StretchImage)
                    {
                        E263.Text = "Gültige Zahl";
                    }
                    else
                    {
                        E263.Text = "Valid number";
                    } 
                    return;
                }
            }
        }

        private void E313_TextChanged(object sender, EventArgs e)
        {
            double number = 0;
            if (String.IsNullOrEmpty(E313.Text))
            {
                E313.ForeColor = Color.Red;
                if (pic_de.SizeMode == PictureBoxSizeMode.StretchImage)
                {
                    E313.Text = "Ausstehend";
                }
                else
                {
                    E313.Text = "Outstanding";
                }
            }
            else
            {
                try
                {
                    number = Convert.ToDouble(E313.Text);
                    E313.ForeColor = Color.Black;

                }
                catch
                {
                    E313.ForeColor = Color.Red;
                    if (pic_de.SizeMode == PictureBoxSizeMode.StretchImage)
                    {
                        E313.Text = "Gültige Zahl";
                    }
                    else
                    {
                        E313.Text = "Valid number";
                    } 
                    return;
                }
            }
        }

        private void E163_TextChanged(object sender, EventArgs e)
        {
            double number = 0;
            if (String.IsNullOrEmpty(E163.Text))
            {
                E163.ForeColor = Color.Red;
                if (pic_de.SizeMode == PictureBoxSizeMode.StretchImage)
                {
                    E163.Text = "Ausstehend";
                }
                else
                {
                    E163.Text = "Outstanding";
                }
            }
            else
            {
                try
                {
                    number = Convert.ToDouble(E163.Text);
                    E163.ForeColor = Color.Black;

                }
                catch
                {
                    E163.ForeColor = Color.Red;
                    if (pic_de.SizeMode == PictureBoxSizeMode.StretchImage)
                    {
                        E163.Text = "Gültige Zahl";
                    }
                    else
                    {
                        E163.Text = "Valid number";
                    } 
                    return;
                }
            }
        }

        private void E173_TextChanged(object sender, EventArgs e)
        {
            double number = 0;
            if (String.IsNullOrEmpty(E173.Text))
            {
                E173.ForeColor = Color.Red;
                if (pic_de.SizeMode == PictureBoxSizeMode.StretchImage)
                {
                    E173.Text = "Ausstehend";
                }
                else
                {
                    E173.Text = "Outstanding";
                }
            }
            else
            {
                try
                {
                    number = Convert.ToDouble(E173.Text);
                    E173.ForeColor = Color.Black;

                }
                catch
                {
                    E173.ForeColor = Color.Red;
                    if (pic_de.SizeMode == PictureBoxSizeMode.StretchImage)
                    {
                        E173.Text = "Gültige Zahl";
                    }
                    else
                    {
                        E173.Text = "Valid number";
                    } 
                    return;
                }
            }
        }

        private void E303_TextChanged(object sender, EventArgs e)
        {
            double number = 0;
            if (String.IsNullOrEmpty(E303.Text))
            {
                E303.ForeColor = Color.Red;
                if (pic_de.SizeMode == PictureBoxSizeMode.StretchImage)
                {
                    E303.Text = "Ausstehend";
                }
                else
                {
                    E303.Text = "Outstanding";
                }
            }
            else
            {
                try
                {
                    number = Convert.ToDouble(E303.Text);
                    E303.ForeColor = Color.Black;

                }
                catch
                {
                    E303.ForeColor = Color.Red;
                    if (pic_de.SizeMode == PictureBoxSizeMode.StretchImage)
                    {
                        E303.Text = "Gültige Zahl";
                    }
                    else
                    {
                        E303.Text = "Valid number";
                    } 
                    return;
                }
            }
        }

        private void E063_TextChanged(object sender, EventArgs e)
        {
            double number = 0;
            if (String.IsNullOrEmpty(E063.Text))
            {
                E063.ForeColor = Color.Red;
                if (pic_de.SizeMode == PictureBoxSizeMode.StretchImage)
                {
                    E063.Text = "Ausstehend";
                }
                else
                {
                    E063.Text = "Outstanding";
                }
            }
            else
            {
                try
                {
                    number = Convert.ToDouble(E063.Text);
                    E063.ForeColor = Color.Black;

                }
                catch
                {
                    E063.ForeColor = Color.Red;
                    if (pic_de.SizeMode == PictureBoxSizeMode.StretchImage)
                    {
                        E063.Text = "Gültige Zahl";
                    }
                    else
                    {
                        E063.Text = "Valid number";
                    } 
                    return;
                }
            }
        }

        private void E123_TextChanged(object sender, EventArgs e)
        {
            double number = 0;
            if (String.IsNullOrEmpty(E123.Text))
            {
                E123.ForeColor = Color.Red;
                if (pic_de.SizeMode == PictureBoxSizeMode.StretchImage)
                {
                    E123.Text = "Ausstehend";
                }
                else
                {
                    E123.Text = "Outstanding";
                }
            }
            else
            {
                try
                {
                    number = Convert.ToDouble(E123.Text);
                    E123.ForeColor = Color.Black;

                }
                catch
                {
                    E123.ForeColor = Color.Red;
                    if (pic_de.SizeMode == PictureBoxSizeMode.StretchImage)
                    {
                        E123.Text = "Gültige Zahl";
                    }
                    else
                    {
                        E123.Text = "Valid number";
                    } 
                    return;
                }
            }
        }

        private void E293_TextChanged(object sender, EventArgs e)
        {
            double number = 0;
            if (String.IsNullOrEmpty(E293.Text))
            {
                E293.ForeColor = Color.Red;
                if (pic_de.SizeMode == PictureBoxSizeMode.StretchImage)
                {
                    E293.Text = "Ausstehend";
                }
                else
                {
                    E293.Text = "Outstanding";
                }
            }
            else
            {
                try
                {
                    number = Convert.ToDouble(E293.Text);
                    E293.ForeColor = Color.Black;

                }
                catch
                {
                    E293.ForeColor = Color.Red;
                    if (pic_de.SizeMode == PictureBoxSizeMode.StretchImage)
                    {
                        E293.Text = "Gültige Zahl";
                    }
                    else
                    {
                        E293.Text = "Valid number";
                    } 
                    return;
                }
            }
        }

        private void E093_TextChanged(object sender, EventArgs e)
        {
            double number = 0;
            if (String.IsNullOrEmpty(E093.Text))
            {
                E093.ForeColor = Color.Red;
                if (pic_de.SizeMode == PictureBoxSizeMode.StretchImage)
                {
                    E093.Text = "Ausstehend";
                }
                else
                {
                    E093.Text = "Outstanding";
                }
            }
            else
            {
                try
                {
                    number = Convert.ToDouble(E093.Text);
                    E093.ForeColor = Color.Black;

                }
                catch
                {
                    E093.ForeColor = Color.Red;
                    if (pic_de.SizeMode == PictureBoxSizeMode.StretchImage)
                    {
                        E093.Text = "Gültige Zahl";
                    }
                    else
                    {
                        E093.Text = "Valid number";
                    } 
                    return;
                }
            }
        }

        private void E153_TextChanged(object sender, EventArgs e)
        {
            double number = 0;
            if (String.IsNullOrEmpty(E153.Text))
            {
                E153.ForeColor = Color.Red;
                if (pic_de.SizeMode == PictureBoxSizeMode.StretchImage)
                {
                    E153.Text = "Ausstehend";
                }
                else
                {
                    E153.Text = "Outstanding";
                }
            }
            else
            {
                try
                {
                    number = Convert.ToDouble(E153.Text);
                    E153.ForeColor = Color.Black;

                }
                catch
                {
                    E153.ForeColor = Color.Red;
                    if (pic_de.SizeMode == PictureBoxSizeMode.StretchImage)
                    {
                        E153.Text = "Gültige Zahl";
                    }
                    else
                    {
                        E153.Text = "Valid number";
                    } 
                    return;
                }
            }
        }

        private void E203_TextChanged(object sender, EventArgs e)
        {
            double number = 0;
            if (String.IsNullOrEmpty(E203.Text))
            {
                E203.ForeColor = Color.Red;
                if (pic_de.SizeMode == PictureBoxSizeMode.StretchImage)
                {
                    E203.Text = "Ausstehend";
                }
                else
                {
                    E203.Text = "Outstanding";
                }
            }
            else
            {
                try
                {
                    number = Convert.ToDouble(E203.Text);
                    E203.ForeColor = Color.Black;

                }
                catch
                {
                    E203.ForeColor = Color.Red;
                    if (pic_de.SizeMode == PictureBoxSizeMode.StretchImage)
                    {
                        E203.Text = "Gültige Zahl";
                    }
                    else
                    {
                        E203.Text = "Valid number";
                    } 
                    return;
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
                default_btn.Text = (Sprachen.EN_BTN_DEFAULT);

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
                default_btn.Text = (Sprachen.DE_BTN_DEFAULT);

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
            this.Controls.Clear();
            UserControl import = new ImportPrognose();
            this.Controls.Add(import);
        }

        private void continue_btn_Click(object sender, EventArgs e)
        {
            this.Controls.Clear();
            UserControl prod = new Produktion();
            this.Controls.Add(prod);
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
                            this.Controls.Clear();
                            UserControl import = new ImportPrognose();
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
                            this.Controls.Clear();
                            UserControl prod = new Produktion();
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
        }

        private void button1_Click(object sender, EventArgs e)
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
    }
}
