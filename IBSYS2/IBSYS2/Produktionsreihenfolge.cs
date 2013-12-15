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
    public partial class Produktionsreihenfolge : UserControl
    {
        public Produktionsreihenfolge()
        {
            InitializeComponent();

            //Produktionsmengen die übergeben werden müssen
            int[,] produktionsmengen = { {1, 150 }, {2, 150 }, { 3, 150}, {4, 50 }, {5, 60 }, {7, 90 }, {8, 100 }, {9, 200 }, {10, 60 }, {11, 130 }, {12, 33333}, {13, 343}, {14, 120}, {15, 90}, {16,70}, {17,34}, {18,60}, {19,300}, {20,70}, {26,100},{29,120},{30,70},{31,190},{49,51},{50,45},{51,37},{55,130}, {54,100}, {56,200} };

            //Produktionsmengen Textboxen befüllen
            for (int i = 1; i < 30; i++)
            {
                int teil = produktionsmengen[i, 0];
                int menge = produktionsmengen[i, 1];
                Control[] found = this.Controls.Find("P" + teil.ToString(), true);
                ((TextBox)found[0]).Text = menge.ToString();
            }
        
        
        }



        private void pic_en_Click(object sender, EventArgs e)
        {
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
            }
            else
            {
                //DE Brotkrumenleiste
                lbl_Startseite.Text = (Sprachen.DE_LBL_STARTSEITE);
                lbl_Sicherheitsbestand.Text = (Sprachen.DE_LBL_SICHERHEITSBESTAND);
                lbl_Produktion.Text = (Sprachen.DE_LBL_PRODUKTION);
                lbl_Produktionsreihenfolge.Text = (Sprachen.DE_LBL_PRODUKTIONSREIHENFOLGE);
                lbl_Kapazitaetsplan.Text = (Sprachen.DE_LBL_KAPATITAETSPLAN);
                lbl_Kaufteiledisposition.Text = (Sprachen.DE_LBL_KAUFTEILEDISPOSITION);
                lbl_Ergebnis.Text = (Sprachen.DE_LBL_ERGEBNIS);
            }
        }

        private void btn_back_Click(object sender, EventArgs e)
        {
            this.Controls.Clear();
            UserControl prod = new Produktion();
            this.Controls.Add(prod);
        }

        private void continue_btn_Click(object sender, EventArgs e)
        {
            this.Controls.Clear();
            UserControl kapazitaet = new Kapazitaetsplan();
            this.Controls.Add(kapazitaet);
        }

        private void lbl_Startseite_Click(object sender, EventArgs e)
        {
            this.Controls.Clear();
            UserControl import = new ImportPrognose();
            this.Controls.Add(import);
        }

        private void lbl_Sicherheitsbestand_Click(object sender, EventArgs e)
        {
            this.Controls.Clear();
            UserControl sicherheit = new Sicherheitsbestand();
            this.Controls.Add(sicherheit);
        }

        private void lbl_Kapazitaetsplan_Click(object sender, EventArgs e)
        {
            this.Controls.Clear();
            UserControl kapazitaet = new Kapazitaetsplan();
            this.Controls.Add(kapazitaet);
        }

        private void lbl_Produktion_Click(object sender, EventArgs e)
        {
            this.Controls.Clear();
            UserControl prod = new Produktion();
            this.Controls.Add(prod);
        }
    }
}
