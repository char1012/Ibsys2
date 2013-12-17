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

        List<List<int>> teile_liste1 = new List<List<int>>();
        public Splitting(List<List<int>> teile_liste, int y)

        {
            InitializeComponent();
            teile_liste1 = teile_liste;
            Menge.Text = teile_liste1[y][1].ToString();
            NR.Text = teile_liste1[y][0].ToString();
            Splitting1.Text = (teile_liste1[y][0] / 2).ToString();
            Splitting2.Text = (teile_liste1[y][0] / 2).ToString();

            splitting(teile_liste1, y);
        }

        public void splitting(List<List<int>> teile_liste1, int y)
        {
            teile_liste1.Add(new List<int>()); //Liste ein Element hinzufügen
            for (int x = 1; x < teile_liste1.Count ; x++)
            {
                teile_liste1[y + x + 1][0] = teile_liste1[y + x][0]; 
                teile_liste1[y + x + 1][0] = teile_liste1[y + x][1];
            }
            teile_liste1[y][0] = Convert.ToInt32(Splitting1.Text);
            teile_liste1[y + 1][0] = Convert.ToInt32(Splitting1.Text);
            teile_liste1[y + 1][1] = teile_liste1[y][1];
        }
    }
}
