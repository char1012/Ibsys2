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

        // hier lokal die Prodreihenfolge speichern - fuer dich Lukas Anmerkung: Später initialisieren sobald Länge von Liste bekannt
        int[,] berProduktionsreihenfolge = new int[30, 2];

        List<List<int>> teile_liste = new List<List<int>>();

        public void vonSplitnachReihenfolge(List<List<int>> teile_liste1)
        {
            this.teile_liste = teile_liste1;
            tabelle_erstellen(teile_liste);
        }

        public void tabelle_erstellen(List<List<int>> teile_liste)
        {
            List<Button> button_liste = new List<Button>();
            tableLayoutPanel.Controls.Clear();
            tableLayoutPanel.ColumnStyles.Clear();
            tableLayoutPanel.RowStyles.Clear();
            tableLayoutPanel.ColumnCount = 5;
            tableLayoutPanel.RowCount = teile_liste.Count + 1;
            tableLayoutPanel.AutoScroll = true;
            for (int x = 0; x < 5; x++)
            {
                //First add a column
                tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

                for (int y = 0; y < teile_liste.Count + 1; y++)
                {
                    Label label = new Label();
                    Button buttonUp = new Button();
                    buttonUp.Text = "hoch";
                    Button buttonDown = new Button();
                    buttonDown.Text = "runter";

                    //Next, add a row.  Only do this when once, when creating the first column
                    if (x == 0)
                    {
                        tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                    }
                    if (y == 0)
                    {
                        // keine Ueberschrift fuer Spalte 5
                        tableLayoutPanel.Controls.Add(label, x, y);
                    }
                    else
                    {
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
                            label.Tag = y;
                            label.Click += new EventHandler(label_click);
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
                            buttonDown.Tag = y;
                            button_liste.Add(buttonDown);
                            buttonDown.Click += new EventHandler(buttonDown_click);
                        }
                    }
                }
            }
        }

        public Produktionsreihenfolge()
        {
            InitializeComponent();
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

            // Mitteilung einblenden
            ProcessMessage message = new ProcessMessage(sprache);
            message.Show(this);
            message.Location = new Point(500, 300);
            message.Update();
            this.Enabled = false;

            Boolean bereitsBerechnet = false;
            for (int i = 0; i < prodReihenfolge.GetLength(0); i++)
            {
                if (prodReihenfolge[i, 1] > 0)
                {
                    bereitsBerechnet = true;
                    break;
                }
            }
            // wenn bereits Werte vorhanden sind, diese uebernehmen
            if (bereitsBerechnet == true)
            {
                // TODO teile_liste fuellen mit Werten aus prodReihenfolge
                berProduktionsreihenfolge = prodReihenfolge;
            }
            else
            {
                int[,] teile = produktion;
                //Array in Liste
                int[] reihenfolge = { 7, 13, 18, 8, 14, 19, 9, 15, 20, 49, 4, 10, 54, 5, 11, 29, 6, 12, 16, 17, 50, 55, 30, 26, 51, 56, 31, 1, 2, 3 };

                //Produktionsreihenfolge in List sortieren 
                for (int joern = 0; joern <= 29; joern++)
                {
                    int teil = reihenfolge[joern];
                    for (int fred = 0; fred <= 29; fred++)
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
                tabelle_erstellen(teile_liste);

                // teile_liste => prodReihenfolge
            }

            message.Close();
            this.Enabled = true;
        }

        void label_click(object sender, EventArgs e)
        {
            Label button = (Label)sender;
            int listitem = (int)button.Tag;
            Splitting split = new Splitting(teile_liste, listitem, sprache, this);
            split.Show();
        }

        void buttonUp_click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            if (button.Tag.ToString() == "1")
            {
                MessageBox.Show("Bereits auf Position 1");
            }
            else
            {
                int listitem = (int)button.Tag - 1;
                int teil1 = teile_liste[listitem][0];
                int menge1 = teile_liste[listitem][1];
                int teil2 = teile_liste[listitem - 1][0];
                int menge2 = teile_liste[listitem - 1][1];

                teile_liste[listitem][0] = teil2;
                teile_liste[listitem][1] = menge2;
                teile_liste[listitem - 1][0] = teil1;
                teile_liste[listitem - 1][1] = menge1;
                tabelle_erstellen(teile_liste);
            }
        }

        void buttonDown_click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            if (Convert.ToInt32(button.Tag) == teile_liste.Count)
            {
                MessageBox.Show("Bereits auf der letzten Position");
            }
            else
            {
                int listitem = (int)button.Tag;
                int teil1 = teile_liste[listitem][0];
                int menge1 = teile_liste[listitem][1];
                int teil2 = teile_liste[listitem - 1][0];
                int menge2 = teile_liste[listitem - 1][1];

                teile_liste[listitem][0] = teil2;
                teile_liste[listitem][1] = menge2;
                teile_liste[listitem - 1][0] = teil1;
                teile_liste[listitem - 1][1] = menge1;
                tabelle_erstellen(teile_liste);
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

        private void tableLayoutPanel_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
