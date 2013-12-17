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
using System.Threading;

namespace IBSYS2
{
    public partial class Begrüßungsseite : Form
    {
        private OleDbConnection myconn;
        private String sprache = "de";

        public Begrüßungsseite()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Controls.Clear();
            UserControl import = new ImportPrognose(sprache);
            this.Controls.Add(import);
        }

        private void pic_en_Click(object sender, EventArgs e)
        {
            pic_en.SizeMode = PictureBoxSizeMode.StretchImage;
            pic_de.SizeMode = PictureBoxSizeMode.Normal;
            label1.Text = "Welcom to the SCMPlus";
            //Startbutton ist ein Bild, deshalb keine Überstzung möglich, 
            //Vorschlag: Button auf START umbenennen.
            sprache = "en";
        }

        private void pic_de_Click(object sender, EventArgs e)
        {
            pic_de.SizeMode = PictureBoxSizeMode.StretchImage;
            pic_en.SizeMode = PictureBoxSizeMode.Normal;
            label1.Text = "Willkommen beim SCMPlus";
            sprache = "de";
        }


        private void Begrüßungsseite_FormClosing(object sender, FormClosingEventArgs e)
        {

            if (pic_de.SizeMode == PictureBoxSizeMode.StretchImage & sprache == "de")
            {
                DialogResult result2 = MessageBox.Show(Sprachen.DE_MSG_INFO1, Sprachen.DE_MSG_INFO2, MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if (result2 == DialogResult.No)
                {
                    e.Cancel = true;
                }
            }
            else
            {
                DialogResult result2 = MessageBox.Show(Sprachen.EN_MSG_INFO1, Sprachen.EN_MSG_INFO2, MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if (result2 == DialogResult.No)
                {
                    e.Cancel = true;
                }
            }
        }

    }
}
