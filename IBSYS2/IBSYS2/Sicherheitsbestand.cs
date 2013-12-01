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
    public partial class Sicherheitsbestand : Form
    {
        private OleDbConnection myconn;

        public Sicherheitsbestand()
        {


            InitializeComponent();
            continue_btn.Enabled = true;
            string databasename = @"Provider=Microsoft.ACE.OLEDB.12.0; Data Source=IBSYS_DB.accdb";
            myconn = new OleDbConnection(databasename);
      
            textfeldSperren();
            
            Ausgabe_P1.Enabled = false;
            Ausgabe_P2.Enabled = false;
            Ausgabe_P3.Enabled = false;
            continue_btn.Enabled = false;
           eteileberechnen_btn.Enabled = false;

        }
        public void ConvertToString()
        {

        }
        private void eteileberechnen_btn_Click_1(object sender, EventArgs e)
        {
                int gLagerbestandP1 = Convert.ToInt32(Eingabe_P1.Text);
                int gLagerbestandP2 = Convert.ToInt32(Eingabe_P2.Text);
                int gLagerbestandP3 = Convert.ToInt32(Eingabe_P3.Text);

                int sicherheitsbestandP1 = sicherheitsbestandBerechnen(gLagerbestandP1, "1");
                Ausgabe_P1.Text = Convert.ToString(sicherheitsbestandP1);
                int sicherheitsbestandP2 = sicherheitsbestandBerechnen(gLagerbestandP2, "2");
                Ausgabe_P2.Text = Convert.ToString(sicherheitsbestandP2);
                int sicherheitsbestandP3 = sicherheitsbestandBerechnen(gLagerbestandP3, "3");
                Ausgabe_P3.Text = Convert.ToString(sicherheitsbestandP3);
                p1_label.Text = Convert.ToString(sicherheitsbestandP1) + " St.";
                p2_label.Text = Convert.ToString(sicherheitsbestandP2) + " St.";
                p3_label.Text = Convert.ToString(sicherheitsbestandP3) + " St.";
            
                int gLE26P1 = geplanterLagerbestand(sicherheitsbestandP1, 100);
                E261.Text = Convert.ToString(gLE26P1);
                int gLE51P1 = geplanterLagerbestand(sicherheitsbestandP1, 100);
                E511.Text = Convert.ToString(gLE51P1);
                int gLE16P1 = geplanterLagerbestand(sicherheitsbestandP1, 100);
                E161.Text = Convert.ToString(gLE16P1);
                int gLE17P1 = geplanterLagerbestand(sicherheitsbestandP1, 100);
                E171.Text = Convert.ToString(gLE17P1);
                int gLE50P1 = geplanterLagerbestand(sicherheitsbestandP1, 100);
                E501.Text = Convert.ToString(gLE50P1);
                int gLE4P1 = geplanterLagerbestand(sicherheitsbestandP1, 100);
                E041.Text = Convert.ToString(gLE4P1);
                int gLE10P1 = geplanterLagerbestand(sicherheitsbestandP1, 100);
                E101.Text = Convert.ToString(gLE10P1);
                int gLE49P1 = geplanterLagerbestand(sicherheitsbestandP1, 100);
                E491.Text = Convert.ToString(gLE49P1);
                int gLE7P1 = geplanterLagerbestand(sicherheitsbestandP1, 100);
                E071.Text = Convert.ToString(gLE7P1);
                int gLE13P1 = geplanterLagerbestand(sicherheitsbestandP1, 100);
                E131.Text = Convert.ToString(gLE13P1);
                int gLE18P1 = geplanterLagerbestand(sicherheitsbestandP1, 100);
                E181.Text = Convert.ToString(gLE18P1);

                int gLE26P2 = geplanterLagerbestand(sicherheitsbestandP2, 100);
                E262.Text = Convert.ToString(gLE26P2);
                int gLE56P2 = geplanterLagerbestand(sicherheitsbestandP2, 100);
                E562.Text = Convert.ToString(gLE56P2);
                int gLE16P2 = geplanterLagerbestand(sicherheitsbestandP2, 100);
                E162.Text = Convert.ToString(gLE16P2);
                int gLE17P2 = geplanterLagerbestand(sicherheitsbestandP2, 100);
                E172.Text = Convert.ToString(gLE17P2);
                int gLE55P2 = geplanterLagerbestand(sicherheitsbestandP2, 100);
                E552.Text = Convert.ToString(gLE55P2);
                int gLE5P2 = geplanterLagerbestand(sicherheitsbestandP2, 100);
                E052.Text = Convert.ToString(gLE5P2);
                int gLE11P2 = geplanterLagerbestand(sicherheitsbestandP2, 100);
                E112.Text = Convert.ToString(gLE11P2);
                int gLE54P2 = geplanterLagerbestand(sicherheitsbestandP2, 100);
                E542.Text = Convert.ToString(gLE54P2);
                int gLE8P2 = geplanterLagerbestand(sicherheitsbestandP2, 100);
                E082.Text = Convert.ToString(gLE8P2);
                int gLE14P2 = geplanterLagerbestand(sicherheitsbestandP2, 100);
                E142.Text = Convert.ToString(gLE14P2);
                int gLE19P2 = geplanterLagerbestand(sicherheitsbestandP2, 100);
                E192.Text = Convert.ToString(gLE19P2);

                int gLE26P3 = geplanterLagerbestand(sicherheitsbestandP3, 100);
                E263.Text = Convert.ToString(gLE26P3);
                int gLE31P3 = geplanterLagerbestand(sicherheitsbestandP3, 100);
                E313.Text = Convert.ToString(gLE31P3);
                int gLE16P3 = geplanterLagerbestand(sicherheitsbestandP3, 100);
                E163.Text = Convert.ToString(gLE16P3);
                int gLE17P3 = geplanterLagerbestand(sicherheitsbestandP3, 100);
                E173.Text = Convert.ToString(gLE17P3);
                int gLE30P3 = geplanterLagerbestand(sicherheitsbestandP3, 100);
                E303.Text = Convert.ToString(gLE30P3);
                int gLE6P3 = geplanterLagerbestand(sicherheitsbestandP3, 100);
                E063.Text = Convert.ToString(gLE6P3);
                int gLE12P3 = geplanterLagerbestand(sicherheitsbestandP3, 100);
                E123.Text = Convert.ToString(gLE12P3);
                int gLE29P3 = geplanterLagerbestand(sicherheitsbestandP3, 100);
                E293.Text = Convert.ToString(gLE29P3);
                int gLE9P3 = geplanterLagerbestand(sicherheitsbestandP3, 100);
                E093.Text = Convert.ToString(gLE9P3);
                int gLE15P3 = geplanterLagerbestand(sicherheitsbestandP3, 100);
                E153.Text = Convert.ToString(gLE15P3);
                int gLE20P3 = geplanterLagerbestand(sicherheitsbestandP3, 100);
                E203.Text = Convert.ToString(gLE20P3);

                    textfeldFreigeben();


                    continue_btn.Enabled = true;
                
            
        }

                 

        public int geplanterLagerbestand(int sicherheitsbestand, int ver)
        {
            int geplanterLagerbestand = 0;
            geplanterLagerbestand = (sicherheitsbestand / 100) * ver;
            return geplanterLagerbestand;
        }

        public int sicherheitsbestandBerechnen(int gLagerbestand, string teilenummer_FK)
        {
            //Wird zukünftig aus ersterm Schritt übergeben
            int prognose = 100;
            int sicherheitsbestand = 0;
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

        private void continue_btn_Click(object sender, EventArgs e)
        {
        }

        private void Eingabe_P2_TextChanged_1(object sender, EventArgs e)
        {



                double number = 0;
                if (String.IsNullOrEmpty(Eingabe_P2.Text))
                {
                    Eingabe_P2.ForeColor = Color.Red;
                    Eingabe_P2.Text = "Ausstehend";
                }
                else
                {
                    try
                    {
                        number = Convert.ToDouble(Eingabe_P2.Text);
                        Eingabe_P2.ForeColor = Color.Black;

                    }
                    catch
                    {
                        Eingabe_P2.ForeColor = Color.Red;
                        Eingabe_P2.Text = "Gültige Zahl";
                        return;
                    }
            }
        }
        private void label1_Click(object sender, EventArgs e)
        {
        }
        private void Eingabe_P1_TextChanged(object sender, EventArgs e)
        {
            double number = 0;
            if (String.IsNullOrEmpty(Eingabe_P1.Text))
            {
                Eingabe_P1.ForeColor = Color.Red;
                Eingabe_P1.Text = "Ausstehend";
            }
            else
            {
                try
                {
                    number = Convert.ToDouble(Eingabe_P1.Text);
                    Eingabe_P1.ForeColor = Color.Black;

                }
                catch
                {
                    Eingabe_P1.ForeColor = Color.Red;
                    Eingabe_P1.Text = "Gültige Zahl";
                    return;
                }
            }
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
            double number = 0;
            if (String.IsNullOrEmpty(E261.Text))
            {
                E261.ForeColor = Color.Red;
                E261.Text = "Ausstehend";
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
                    E261.Text = "Gültige Zahl";
                    return;
                }
            }
        }

        private void Eingabe_P3_TextChanged_1(object sender, EventArgs e)
        {
            double number = 0;
            if (String.IsNullOrEmpty(Eingabe_P3.Text))
            {
                Eingabe_P3.ForeColor = Color.Red;
                Eingabe_P3.Text = "Ausstehend";
            }
            else
            {
                try
                {
                    number = Convert.ToDouble(Eingabe_P3.Text);
                    Eingabe_P3.ForeColor = Color.Black;

                }
                catch
                {
                    Eingabe_P3.ForeColor = Color.Red;
                    Eingabe_P3.Text = "Gültige Zahl";
                    return;
                }
            }
            eteileberechnen_btn.Enabled = true;
        
        }

        private void E511_TextChanged(object sender, EventArgs e)
        {
            double number = 0;
            if (String.IsNullOrEmpty(E511.Text))
            {
                E511.ForeColor = Color.Red;
                E511.Text = "Ausstehend";
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
                    E511.Text = "Gültige Zahl";
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
                E161.Text = "Ausstehend";
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
                    E161.Text = "Gültige Zahl";
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
                E171.Text = "Ausstehend";
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
                    E171.Text = "Gültige Zahl";
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
                E501.Text = "Ausstehend";
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
                    E501.Text = "Gültige Zahl";
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
                E041.Text = "Ausstehend";
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
                    E041.Text = "Gültige Zahl";
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
                E101.Text = "Ausstehend";
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
                    E101.Text = "Gültige Zahl";
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
                E491.Text = "Ausstehend";
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
                    E491.Text = "Gültige Zahl";
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
                E071.Text = "Ausstehend";
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
                    E071.Text = "Gültige Zahl";
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
                E131.Text = "Ausstehend";
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
                    E131.Text = "Gültige Zahl";
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
                E181.Text = "Ausstehend";
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
                    E181.Text = "Gültige Zahl";
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
                E262.Text = "Ausstehend";
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
                    E262.Text = "Gültige Zahl";
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
                E562.Text = "Ausstehend";
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
                    E562.Text = "Gültige Zahl";
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
                E162.Text = "Ausstehend";
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
                    E162.Text = "Gültige Zahl";
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
                E172.Text = "Ausstehend";
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
                    E172.Text = "Gültige Zahl";
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
                E552.Text = "Ausstehend";
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
                    E552.Text = "Gültige Zahl";
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
                E052.Text = "Ausstehend";
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
                    E052.Text = "Gültige Zahl";
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
                E112.Text = "Ausstehend";
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
                    E112.Text = "Gültige Zahl";
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
                E542.Text = "Ausstehend";
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
                    E542.Text = "Gültige Zahl";
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
                E082.Text = "Ausstehend";
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
                    E082.Text = "Gültige Zahl";
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
                E142.Text = "Ausstehend";
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
                    E142.Text = "Gültige Zahl";
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
                E192.Text = "Ausstehend";
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
                    E192.Text = "Gültige Zahl";
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
                E263.Text = "Ausstehend";
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
                    E263.Text = "Gültige Zahl";
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
                E313.Text = "Ausstehend";
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
                    E313.Text = "Gültige Zahl";
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
                E163.Text = "Ausstehend";
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
                    E163.Text = "Gültige Zahl";
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
                E173.Text = "Ausstehend";
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
                    E173.Text = "Gültige Zahl";
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
                E303.Text = "Ausstehend";
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
                    E303.Text = "Gültige Zahl";
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
                E063.Text = "Ausstehend";
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
                    E063.Text = "Gültige Zahl";
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
                E123.Text = "Ausstehend";
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
                    E123.Text = "Gültige Zahl";
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
                E293.Text = "Ausstehend";
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
                    E293.Text = "Gültige Zahl";
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
                E093.Text = "Ausstehend";
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
                    E093.Text = "Gültige Zahl";
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
                E153.Text = "Ausstehend";
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
                    E153.Text = "Gültige Zahl";
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
                E203.Text = "Ausstehend";
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
                    E203.Text = "Gültige Zahl";
                    return;
                }
            }
        }

        private void groupBox2_Enter_1(object sender, EventArgs e)
        {

        }


        public void sprachen(String sprache)
        {
            if (sprache != "de")
            {  
                //EN Brotkrumenleiste
                lbl_Startseite.Text = (Sprachen.EN_LBL_STARTSEITE);
                lbl_Sicherheitsbestand.Text = (Sprachen.EN_LBL_SICHERHEITSBESTAND);
                lbl_Auftraege.Text = (Sprachen.EN_LBL_AUFTRAEGE);
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
                label4.Text = (Sprachen.EN_LABEL4);
                label9.Text = (Sprachen.EN_LABLE9);

                //EN Tooltip
                System.Windows.Forms.ToolTip ToolTipP = new System.Windows.Forms.ToolTip();
                ToolTipP.SetToolTip(this.infoP, Sprachen.EN_INFOP);
                System.Windows.Forms.ToolTip ToolTipE = new System.Windows.Forms.ToolTip();
                ToolTipE.SetToolTip(this.infoE, Sprachen.EN_INFOE);
            }
            else
            {
                //DE Brotkrumenleiste
                lbl_Sicherheitsbestand.Text = (Sprachen.DE_LBL_SICHERHEITSBESTAND);
                lbl_Startseite.Text = (Sprachen.DE_LBL_STARTSEITE);
                lbl_Auftraege.Text = (Sprachen.DE_LBL_AUFTRAEGE);
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
                label4.Text = (Sprachen.DE_LABEL4);
                label9.Text = (Sprachen.DE_LABLE9);

                //DE Tooltip
                System.Windows.Forms.ToolTip ToolTipP = new System.Windows.Forms.ToolTip();
                ToolTipP.SetToolTip(this.infoP, Sprachen.DE_INFOP);
                System.Windows.Forms.ToolTip ToolTipE = new System.Windows.Forms.ToolTip();
                ToolTipE.SetToolTip(this.infoE, Sprachen.DE_INFOE);
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            radioButton2.Checked = false;
            string sprache = "de";
            sprachen(sprache);
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            radioButton1.Checked = false;
            string sprache = "en";
            sprachen(sprache);
        }

    }
}
