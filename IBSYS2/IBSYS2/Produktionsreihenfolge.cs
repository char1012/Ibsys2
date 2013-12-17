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
        Button buttonUp = new Button();
        Button buttonDown = new Button();
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

        // hier lokal die Prodreihenfolge speichern - fuer dich Lukas
        int[,] berProduktionsreihenfolge = new int[30, 2];

        public Produktionsreihenfolge()
        {
            InitializeComponent();

            // simulieren
            int[,] teile = new int[30, 2];
            teile[0, 0] = 1;
            teile[0, 1] = 90; // Teil p1 mit 90 Stueck Produktion
            teile[1, 0] = 2;
            teile[1, 1] = 190;
            teile[2, 0] = 3;
            teile[2, 1] = 160;
            teile[3, 0] = 4;
            teile[3, 1] = 60;
            teile[4, 0] = 5;
            teile[4, 1] = 160;
            teile[5, 0] = 6;
            teile[5, 1] = -110;
            teile[6, 0] = 7;
            teile[6, 1] = 50;
            teile[7, 0] = 8;
            teile[7, 1] = 150;
            teile[8, 0] = 9;
            teile[8, 1] = -200;
            teile[9, 0] = 10;
            teile[9, 1] = 60;
            teile[10, 0] = 11;
            teile[10, 1] = 160;
            teile[11, 0] = 12;
            teile[11, 1] = -110;
            teile[12, 0] = 13;
            teile[12, 1] = 50;
            teile[13, 0] = 14;
            teile[13, 1] = 150;
            teile[14, 0] = 15;
            teile[14, 1] = -200;
            teile[15, 0] = 16;
            teile[15, 1] = 20 + 130 + 90;
            teile[16, 0] = 17;
            teile[16, 1] = 20 + 130 + 90;
            teile[17, 0] = 18;
            teile[17, 1] = 50;
            teile[18, 0] = 19;
            teile[18, 1] = 150;
            teile[19, 0] = 20;
            teile[19, 1] = -200;
            teile[20, 0] = 26;
            teile[20, 1] = 50 + 160 + 130;
            teile[21, 0] = 29;
            teile[21, 1] = -110;
            teile[22, 0] = 30;
            teile[22, 1] = -20;
            teile[23, 0] = 31;
            teile[23, 1] = 70;
            teile[24, 0] = 49;
            teile[24, 1] = 60;
            teile[25, 0] = 50;
            teile[25, 1] = 70;
            teile[26, 0] = 51;
            teile[26, 1] = 80;
            teile[27, 0] = 54;
            teile[27, 1] = 160;
            teile[28, 0] = 55;
            teile[28, 1] = 170;
            teile[29, 0] = 56;
            teile[29, 1] = 180;

            // TODO: array in eine Produktionsreihenfolge sortieren
  
            //Array in zweidimensionale Liste überführt
            List<List<int>> teile_liste_unsortiert = new List<List<int>>();
            List<List<int>> teile_liste = new List<List<int>>();
            int[] reihenfolge = {7,13,18,8,14,19,9,15,20,49,4,10,54,5,11,29,6,12,16,17,50,55,30,26,51,56,31,1,2,3};
            for (int x = 0; x < 29; x++)
            {
                teile_liste_unsortiert.Add(new List<int>());
                teile_liste_unsortiert[x].Add(teile[x, 0]);
                teile_liste_unsortiert[x].Add(teile[x, 1]);
            }

            //Produktionsreihenfolge in List sortieren 
            for (int joern = 0; joern <= 29; joern++)
            {
                int teil = reihenfolge[joern];
                for(int fred = 0; fred <= 29; fred++)
                    {
                        if (teile[fred, 0] == teil)
                        {
                            int menge = teile[fred, 1];
                            teile_liste.Add(new List<int>());
                            teile_liste[joern].Add(teil);
                            teile_liste[joern].Add(menge);
                        }
                    }
            }

            List<Button> button_liste = new List<Button>();
            tableLayoutPanel.Controls.Clear();
            tableLayoutPanel.ColumnStyles.Clear();
            tableLayoutPanel.RowStyles.Clear();
            tableLayoutPanel.ColumnCount = 5;
            tableLayoutPanel.RowCount = teile.GetLength(0)+1;
            tableLayoutPanel.AutoScroll = true;
            for (int x = 0; x < 5; x++)
            {
                //First add a column
                tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

                for (int y = 0; y < teile.GetLength(0); y++)
                {
                    Label label = new Label();
                    buttonUp.Text = "hoch";
                    buttonDown.Text = "runter";

                    //Next, add a row.  Only do this when once, when creating the first column
                    if (x == 0)
                    {
                        tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                    }
                    if (y == 0)
                    {
                        if (x == 0)
                        {
                            label.Text = "Position";
                        }
                        else if (x == 1)
                        {
                            label.Text = "Teil";
                        }
                        else if (x == 2)
                        {
                            label.Text = "Menge";
                        }
                        else if (x == 3)
                        {
                            label.Text = "Sortieren";
                        }
                        // keine Ueberschrift fuer Spalte 5
                        tableLayoutPanel.Controls.Add(label, x, y);
                    }
                    else {
                        int i = y - 1;
                        if (x == 0)
                        {
                            label.Text = y.ToString();
                            tableLayoutPanel.Controls.Add(label, x, y);
                        }
                        else if (x == 1)
                        {
                            label.Text = teile_liste[i][0].ToString();
                            tableLayoutPanel.Controls.Add(label, x, y);
                        }
                        else if (x == 2)
                        {
                            label.Text = teile_liste[i][1].ToString();
                            tableLayoutPanel.Controls.Add(label, x, y);
                        }
                        else if (x == 3)
                        {
                            tableLayoutPanel.Controls.Add(buttonUp, x, y);
                            buttonUp.Tag = y;
                            button_liste.Add(buttonUp);
                            buttonUp.Click += new EventHandler(buttonUp_click);
 
                        }
                        else if (x == 4)
                        {
                            tableLayoutPanel.Controls.Add(buttonDown, x, y);
                        }
                    }
              }
           }
        }

        public Produktionsreihenfolge(int aktPeriode, int[] auftraege, double[,] direktverkaeufe, int[,] sicherheitsbest,
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

            Boolean bereitsBerechnet = false;
            for (int i = 0; i < kapazitaet.GetLength(0); i++)
            {
                if (kapazitaet[i, 1] > 0)
                {
                    bereitsBerechnet = true;
                    break;
                }
            }
            // wenn bereits Werte vorhanden sind, diese uebernehmen
            if (bereitsBerechnet == true)
            {
                berProduktionsreihenfolge = prodReihenfolge;
            }
            else
            {
                // TODO Sabrina: hier den Code aus dem anderen Konstruktor einfuegen, wenn fertig
                // zum testen:
                berProduktionsreihenfolge = produktion;
            }
        }

        void buttonUp_click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            if (button.Tag.ToString() == "1")
            {
                MessageBox.Show("joern");
            }
            else
            {
                string listitem = button.Tag.ToString();
                
            }
        }

        private void pic_en_Click(object sender, EventArgs e)
        {
            pic_en.SizeMode = PictureBoxSizeMode.StretchImage;
            pic_de.SizeMode = PictureBoxSizeMode.Normal;
            sprachen();
            sprache = "en";
        }

        private void pic_de_Click(object sender, EventArgs e)
        {
            pic_de.SizeMode = PictureBoxSizeMode.StretchImage;
            pic_en.SizeMode = PictureBoxSizeMode.Normal;
            sprachen();
            sprache = "de";
        }

        public void sprachen()
        {
            if (pic_en.SizeMode == PictureBoxSizeMode.StretchImage | sprache == "en")
            {
                //EN Brotkrumenleiste
                lbl_Startseite.Text = (Sprachen.EN_LBL_STARTSEITE);
                lbl_Sicherheitsbestand.Text = (Sprachen.EN_LBL_SICHERHEITSBESTAND);
                lbl_Produktion.Text = (Sprachen.EN_LBL_PRODUKTION);
                lbl_Produktionsreihenfolge.Text = (Sprachen.EN_LBL_PRODUKTIONSREIHENFOLGE);
                lbl_Kapazitaetsplan.Text = (Sprachen.EN_LBL_KAPATITAETSPLAN);
                lbl_Kaufteiledisposition.Text = (Sprachen.EN_LBL_KAUFTEILEDISPOSITION);
                lbl_Ergebnis.Text = (Sprachen.EN_LBL_ERGEBNIS);

                //EN Button
                btn_back.Text = (Sprachen.EN_BTN_BACK);
                continue_btn.Text = (Sprachen.EN_BTN_CONTINUE);

                //GroupBox
                groupBox1.Text = (Sprachen.EN_GB_PR_PROD_SPLITT);

                buttonDown.Text = "down";
                buttonUp.Text = "up";
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

                //EN Button
                btn_back.Text = (Sprachen.DE_BTN_BACK);
                continue_btn.Text = (Sprachen.DE_BTN_CONTINUE);

                //GroupBox
                groupBox1.Text = (Sprachen.DE_GB_PR_PROD_SPLITT);

                buttonDown.Text = "Runter";
                buttonUp.Text = "Hoch";

            }
        }

        private void btn_back_Click(object sender, EventArgs e)
        {
            // Datenweitergabe

            prodReihenfolge = berProduktionsreihenfolge;

            this.Controls.Clear();
            UserControl prod = new Produktion(aktPeriode, auftraege, direktverkaeufe,
                sicherheitsbest, produktion, produktionProg, prodReihenfolge, kapazitaet, kaufauftraege, sprache);
            this.Controls.Add(prod);
        }

        private void continue_btn_Click(object sender, EventArgs e)
        {
            // Datenweitergabe

            prodReihenfolge = berProduktionsreihenfolge;

            this.Controls.Clear();
            UserControl kapaplan = new Kapazitaetsplan(aktPeriode, auftraege, direktverkaeufe,
                sicherheitsbest, produktion, produktionProg, prodReihenfolge, kapazitaet, kaufauftraege, sprache);
            this.Controls.Add(kapaplan);
        }

        private void lbl_Startseite_Click(object sender, EventArgs e)
        {
            // Datenweitergabe

            prodReihenfolge = berProduktionsreihenfolge;

            this.Controls.Clear();
            UserControl import = new ImportPrognose(aktPeriode, auftraege, direktverkaeufe,
                sicherheitsbest, produktion, produktionProg, prodReihenfolge, kapazitaet, kaufauftraege, sprache);
            this.Controls.Add(import);
        }

        private void lbl_Sicherheitsbestand_Click(object sender, EventArgs e)
        {
            // Datenweitergabe

            prodReihenfolge = berProduktionsreihenfolge;

            this.Controls.Clear();
            UserControl sicherheit = new Sicherheitsbestand(aktPeriode, auftraege, direktverkaeufe,
                sicherheitsbest, produktion, produktionProg, prodReihenfolge, kapazitaet, kaufauftraege, sprache);
            this.Controls.Add(sicherheit);
        }

        private void lbl_Kapazitaetsplan_Click(object sender, EventArgs e)
        {
            // Datenweitergabe

            prodReihenfolge = berProduktionsreihenfolge;

            this.Controls.Clear();
            UserControl kapaplan = new Kapazitaetsplan(aktPeriode, auftraege, direktverkaeufe,
                sicherheitsbest, produktion, produktionProg, prodReihenfolge, kapazitaet, kaufauftraege, sprache);
            this.Controls.Add(kapaplan);
        }

        private void lbl_Produktion_Click(object sender, EventArgs e)
        {
            // Datenweitergabe

            prodReihenfolge = berProduktionsreihenfolge;

            this.Controls.Clear();
            UserControl prod = new Produktion(aktPeriode, auftraege, direktverkaeufe,
                sicherheitsbest, produktion, produktionProg, prodReihenfolge, kapazitaet, kaufauftraege, sprache);
            this.Controls.Add(prod);
        }
        /*
        private void Plus7_Click(object sender, EventArgs e)
        {
            int[] Feld_nummern = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 26, 29, 30, 31, 49, 50, 51, 54, 55, 56 };
            int zahl = Convert.ToInt32(P7.Text);
            if (Convert.ToInt32(P7.Text) < 30)
            {
                for (int i = 0; i < 30; i++)
                {
                    Control[] found = this.Controls.Find("P" + Feld_nummern[i].ToString(), true);
                    int InhaltTextbox = Convert.ToInt32(((TextBox)found[0]).Text);
                    if (InhaltTextbox == zahl + 1)
                    {
                        ((TextBox)found[0]).Text = zahl.ToString();
                    }
                }
                P7.Text = (zahl + 1).ToString();
            }
            else { int joern1 = 30; } //message
        }

        private void Minus7_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(P7.Text) > 1)
            {
                int[] Feld_nummern = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 26, 29, 30, 31, 49, 50, 51, 54, 55, 56 };
                int zahl = Convert.ToInt32(P7.Text);

                for (int i = 0; i < 30; i++)
                {
                    Control[] found = this.Controls.Find("P" + Feld_nummern[i].ToString(), true);
                    int InhaltTextbox = Convert.ToInt32(((TextBox)found[0]).Text);
                    if (InhaltTextbox == zahl - 1)
                    {
                        ((TextBox)found[0]).Text = zahl.ToString();
                    }
                }
                P7.Text = (zahl - 1).ToString();
            }
            else { int joern = 1; }//message
        }*/
    }
}
