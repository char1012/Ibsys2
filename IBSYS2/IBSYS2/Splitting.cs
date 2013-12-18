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
        private char[] digits = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
        List<List<int>> teile_liste1 = new List<List<int>>();
        int position;

        public void splitting(List<List<int>> teile_liste1, int y)
        {
            int VLE_teil = teile_liste1[teile_liste1.Count - 1][0];
            int VLE_menge = teile_liste1[teile_liste1.Count - 1][1];
            teile_liste1.Add(new List<int>()); //Liste ein Element hinzufügen
            teile_liste1[teile_liste1.Count - 1].Add(VLE_teil);
            teile_liste1[teile_liste1.Count - 1].Add(VLE_menge);
            for (int x = teile_liste1.Count - 1; x > y; x--)
            {
                teile_liste1[x][0] = teile_liste1[x - 1][0];
                teile_liste1[x][1] = teile_liste1[x - 1][1];
            }
            teile_liste1[y - 1][1] = Convert.ToInt32(Splitting1.Text);
            teile_liste1[y][1] = Convert.ToInt32(Splitting2.Text);

        }

        public Splitting(List<List<int>> teile_liste, int y)
        {
            int position = y - 1;
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
        }
    }
}
