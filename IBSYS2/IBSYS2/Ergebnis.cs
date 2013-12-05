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
    public partial class Ergebnis : Form
    {
        public Ergebnis()
        {
            InitializeComponent();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        public void sprachen(String sprache)
        {
            if (sprache != "de")
            {
                //EN Brotkrumenleiste
                lbl_Startseite.Text = (Sprachen.EN_LBL_STARTSEITE);
                lbl_Sicherheitsbestand.Text = (Sprachen.EN_LBL_SICHERHEITSBESTAND);
                lbl_Produktion.Text = (Sprachen.EN_LBL_PRODUKTION);
                lbl_Kapazitaetsplan.Text = (Sprachen.EN_LBL_KAPATITAETSPLAN);
                lbl_Kaufteiledisposition.Text = (Sprachen.EN_LBL_KAUFTEILEDISPOSITION);
                lbl_Ergebnis.Text = (Sprachen.EN_LBL_ERGEBNIS);

                //EN Buttons
                End_btn.Text = (Sprachen.EN_BTN_XML_EXPORT);

                //EN Groupboxen
                groupBox2.Text = (Sprachen.EN_ER_GROUPBOX2);
                groupBox3.Text = (Sprachen.EN_ER_GROUPBOX3);
                groupBox4.Text = (Sprachen.EN_ER_GROUPBOX4);
                groupBox1.Text = (Sprachen.EN_ER_GROUPBOX1);
            }
            else
            {
                //DE Brotkrumenleiste
                lbl_Sicherheitsbestand.Text = (Sprachen.DE_LBL_SICHERHEITSBESTAND);
                lbl_Startseite.Text = (Sprachen.DE_LBL_STARTSEITE);
                lbl_Produktion.Text = (Sprachen.DE_LBL_PRODUKTION);
                lbl_Kapazitaetsplan.Text = (Sprachen.DE_LBL_KAPATITAETSPLAN);
                lbl_Kaufteiledisposition.Text = (Sprachen.DE_LBL_KAUFTEILEDISPOSITION);
                lbl_Ergebnis.Text = (Sprachen.DE_LBL_ERGEBNIS);

                //DE Buttons
                End_btn.Text = (Sprachen.DE_BTN_XML_EXPORT);

                //DE Groupboxen
                groupBox2.Text = (Sprachen.DE_ER_GROUPBOX2);
                groupBox3.Text = (Sprachen.DE_ER_GROUPBOX3);
                groupBox4.Text = (Sprachen.DE_ER_GROUPBOX4);
                groupBox1.Text = (Sprachen.DE_ER_GROUPBOX1);
            }
        }

        private void pic_en_Click(object sender, EventArgs e)
        {
            string sprache = "en";
            sprachen(sprache);
        }

        private void pic_de_Click(object sender, EventArgs e)
        {
            string sprache = "de";
            sprachen(sprache);
        }
    }
}
