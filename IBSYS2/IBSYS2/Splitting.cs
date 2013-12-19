using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IBSYS2
{
    public partial class Splitting : Form
    {

        // Datenweitergabe:
        private String sprache = "de";
        int aktPeriode;
        int[] auftraege = new int[12];
        double[,] direktverkaeufe = new double[3, 4];
        int[,] sicherheitsbest = new int[30, 5];
        int[,] produktion = new int[30, 2];
        int[,] produktionProg = new int[3, 5];
        List<List<int>> prodReihenfolge = new List<List<int>>();
        int[,] kapazitaet = new int[15, 5];
        int[,] kaufauftraege = new int[29, 6];


        private char[] digits = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
        List<List<int>> teile_liste1 = new List<List<int>>();
        int position;
        UserControl prodReihenfolgeForm;

        public void splitting(List<List<int>> teile_liste1, int position)
        {
            int VLE_teil = teile_liste1[teile_liste1.Count - 1][0];
            int VLE_menge = teile_liste1[teile_liste1.Count - 1][1];
            teile_liste1.Add(new List<int>()); //Liste ein Element hinzufügen
            teile_liste1[teile_liste1.Count - 1].Add(VLE_teil);
            teile_liste1[teile_liste1.Count - 1].Add(VLE_menge);
            for (int x = teile_liste1.Count - 1; x > position; x--)
            {
                teile_liste1[x][0] = teile_liste1[x - 1][0];
                teile_liste1[x][1] = teile_liste1[x - 1][1];
            }
            teile_liste1[position][1] = Convert.ToInt32(Splitting1.Text);
            teile_liste1[position+1][1] = Convert.ToInt32(Splitting2.Text);

        }

        public Splitting(List<List<int>> teile_liste, int y, String sprache, UserControl prodReihenfolge)
        {
            this.sprache = sprache;
            this.position = y - 1;
            this.prodReihenfolgeForm = prodReihenfolge;
            //position = y - 1;
            InitializeComponent();
            teile_liste1 = teile_liste;
            Menge.Text = teile_liste1[position][1].ToString();
            NR.Text = teile_liste1[position][0].ToString();
            Splitting1.Text = (teile_liste1[position][1] / 2).ToString();
            Splitting2.Text = (teile_liste1[position][1] / 2).ToString();
        }

        public Splitting(List<List<int>> teile_liste, int y, String sprache, int aktPeriode, int[] auftraege, double[,] direktverkaeufe, int[,] sicherheitsbest,
            int[,] produktion, int[,] produktionProg, List<List<int>> prodReihenfolge, int[,] kapazitaet, int[,] kaufauftraege)
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
            this.position = y - 1;
            //position = y - 1;
            InitializeComponent();
            teile_liste1 = teile_liste;
            Menge.Text = teile_liste1[position][1].ToString();
            NR.Text = teile_liste1[position][0].ToString();
            Splitting1.Text = (teile_liste1[position][1] / 2).ToString();
            Splitting2.Text = (teile_liste1[position][1] / 2).ToString();
        }

        private void Splitting2_TextChanged(object sender, EventArgs e)
        {

        }

        private void Splitting1_TextChanged(object sender, EventArgs e)
        {
            //Abfrage3 ob Werrt null oder Zeichen:
            if (Splitting1.Text == "")
            {
                Splitting1.ForeColor = Color.Red;
                //textBox3.Text = "Geben Sie einen Wert ein";
            }
            else
            {
                Splitting1.ForeColor = Color.Black;
                bool okay = true;
                // neuer Text darf nur Zeichen aus der Liste digits (in der Klasse deklariert)
                foreach (char c in Splitting1.Text.ToCharArray())
                {
                    // sobald es ein unpassendes Zeichen gibt, aufhoeren und Fehlermeldung ausgeben
                    if (!digits.Contains<char>(c))
                    {
                        Splitting1.ForeColor = Color.Red;
                        okay = false;
                        continue_btn.Enabled = false;
                        break;
                    }
                }
                if (okay == true)
                {
                    Splitting1.ForeColor = Color.Black;
                    //Tue Kram hier.
                    if (Convert.ToInt32(Splitting1.Text) < 1 || Convert.ToInt32(Splitting1.Text) > Convert.ToInt32(Menge.Text))
                    {
                        Splitting1.ForeColor = Color.Red;
                        continue_btn.Enabled = false;
                    }
                    else
                    {
                        Splitting2.Text = (Convert.ToInt32(Menge.Text) - Convert.ToInt32(Splitting1.Text)).ToString();
                        continue_btn.Enabled = true;
                    }
                }
            }            
        }

        private void abr_btn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void continue_btn_Click(object sender, EventArgs e)
        {
            splitting(teile_liste1, position);
            Produktionsreihenfolge rf = (Produktionsreihenfolge) prodReihenfolgeForm;
            rf.vonSplitnachReihenfolge(teile_liste1);
            this.Close();
        }
    }
}
