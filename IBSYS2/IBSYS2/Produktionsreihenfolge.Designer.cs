namespace IBSYS2
{
    partial class Produktionsreihenfolge
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Produktionsreihenfolge));
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.pic_de = new System.Windows.Forms.PictureBox();
            this.pic_en = new System.Windows.Forms.PictureBox();
            this.lbl_Startseite = new System.Windows.Forms.Label();
            this.lbl_Sicherheitsbestand = new System.Windows.Forms.Label();
            this.lbl_Ergebnis = new System.Windows.Forms.Label();
            this.lbl_Kaufteiledisposition = new System.Windows.Forms.Label();
            this.lbl_Produktion = new System.Windows.Forms.Label();
            this.lbl_Produktionsreihenfolge = new System.Windows.Forms.Label();
            this.lbl_Kapazitaetsplan = new System.Windows.Forms.Label();
            this.btn_back = new System.Windows.Forms.Button();
            this.continue_btn = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pic_de)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pic_en)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox4
            // 
            this.pictureBox4.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox4.Image")));
            this.pictureBox4.Location = new System.Drawing.Point(13, 42);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(959, 63);
            this.pictureBox4.TabIndex = 71;
            this.pictureBox4.TabStop = false;
            // 
            // pic_de
            // 
            this.pic_de.Image = ((System.Drawing.Image)(resources.GetObject("pic_de.Image")));
            this.pic_de.Location = new System.Drawing.Point(942, 10);
            this.pic_de.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.pic_de.Name = "pic_de";
            this.pic_de.Size = new System.Drawing.Size(30, 19);
            this.pic_de.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pic_de.TabIndex = 139;
            this.pic_de.TabStop = false;
            this.pic_de.Click += new System.EventHandler(this.pic_de_Click);
            // 
            // pic_en
            // 
            this.pic_en.Image = ((System.Drawing.Image)(resources.GetObject("pic_en.Image")));
            this.pic_en.Location = new System.Drawing.Point(908, 10);
            this.pic_en.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.pic_en.Name = "pic_en";
            this.pic_en.Size = new System.Drawing.Size(30, 19);
            this.pic_en.TabIndex = 140;
            this.pic_en.TabStop = false;
            this.pic_en.Click += new System.EventHandler(this.pic_en_Click);
            // 
            // lbl_Startseite
            // 
            this.lbl_Startseite.AutoSize = true;
            this.lbl_Startseite.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(143)))), ((int)(((byte)(170)))), ((int)(((byte)(220)))));
            this.lbl_Startseite.Font = new System.Drawing.Font("Corbel", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Startseite.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lbl_Startseite.Location = new System.Drawing.Point(25, 63);
            this.lbl_Startseite.Name = "lbl_Startseite";
            this.lbl_Startseite.Size = new System.Drawing.Size(75, 19);
            this.lbl_Startseite.TabIndex = 141;
            this.lbl_Startseite.Text = "Startseite";
            this.lbl_Startseite.Click += new System.EventHandler(this.lbl_Startseite_Click);
            // 
            // lbl_Sicherheitsbestand
            // 
            this.lbl_Sicherheitsbestand.AutoSize = true;
            this.lbl_Sicherheitsbestand.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(143)))), ((int)(((byte)(170)))), ((int)(((byte)(220)))));
            this.lbl_Sicherheitsbestand.Font = new System.Drawing.Font("Corbel", 12F);
            this.lbl_Sicherheitsbestand.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lbl_Sicherheitsbestand.Location = new System.Drawing.Point(122, 63);
            this.lbl_Sicherheitsbestand.Name = "lbl_Sicherheitsbestand";
            this.lbl_Sicherheitsbestand.Size = new System.Drawing.Size(138, 19);
            this.lbl_Sicherheitsbestand.TabIndex = 142;
            this.lbl_Sicherheitsbestand.Text = "Sicherheitsbestand";
            this.lbl_Sicherheitsbestand.Click += new System.EventHandler(this.lbl_Sicherheitsbestand_Click);
            // 
            // lbl_Ergebnis
            // 
            this.lbl_Ergebnis.AutoSize = true;
            this.lbl_Ergebnis.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(206)))), ((int)(((byte)(206)))));
            this.lbl_Ergebnis.Font = new System.Drawing.Font("Corbel", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Ergebnis.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lbl_Ergebnis.Location = new System.Drawing.Point(905, 63);
            this.lbl_Ergebnis.Name = "lbl_Ergebnis";
            this.lbl_Ergebnis.Size = new System.Drawing.Size(67, 19);
            this.lbl_Ergebnis.TabIndex = 143;
            this.lbl_Ergebnis.Text = "Ergebnis";
            // 
            // lbl_Kaufteiledisposition
            // 
            this.lbl_Kaufteiledisposition.AutoSize = true;
            this.lbl_Kaufteiledisposition.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(206)))), ((int)(((byte)(206)))));
            this.lbl_Kaufteiledisposition.Font = new System.Drawing.Font("Corbel", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Kaufteiledisposition.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lbl_Kaufteiledisposition.Location = new System.Drawing.Point(716, 63);
            this.lbl_Kaufteiledisposition.Name = "lbl_Kaufteiledisposition";
            this.lbl_Kaufteiledisposition.Size = new System.Drawing.Size(136, 19);
            this.lbl_Kaufteiledisposition.TabIndex = 144;
            this.lbl_Kaufteiledisposition.Text = "Kaufteildisposition";
            // 
            // lbl_Produktion
            // 
            this.lbl_Produktion.AutoSize = true;
            this.lbl_Produktion.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(143)))), ((int)(((byte)(170)))), ((int)(((byte)(220)))));
            this.lbl_Produktion.Font = new System.Drawing.Font("Corbel", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Produktion.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lbl_Produktion.Location = new System.Drawing.Point(279, 63);
            this.lbl_Produktion.Name = "lbl_Produktion";
            this.lbl_Produktion.Size = new System.Drawing.Size(84, 19);
            this.lbl_Produktion.TabIndex = 145;
            this.lbl_Produktion.Text = "Produktion";
            this.lbl_Produktion.Click += new System.EventHandler(this.lbl_Produktion_Click);
            // 
            // lbl_Produktionsreihenfolge
            // 
            this.lbl_Produktionsreihenfolge.AutoSize = true;
            this.lbl_Produktionsreihenfolge.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(143)))), ((int)(((byte)(170)))), ((int)(((byte)(220)))));
            this.lbl_Produktionsreihenfolge.Font = new System.Drawing.Font("Corbel", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Produktionsreihenfolge.ForeColor = System.Drawing.Color.Black;
            this.lbl_Produktionsreihenfolge.Location = new System.Drawing.Point(392, 63);
            this.lbl_Produktionsreihenfolge.Name = "lbl_Produktionsreihenfolge";
            this.lbl_Produktionsreihenfolge.Size = new System.Drawing.Size(167, 19);
            this.lbl_Produktionsreihenfolge.TabIndex = 146;
            this.lbl_Produktionsreihenfolge.Text = "Produktionsreihenfolge";
            // 
            // lbl_Kapazitaetsplan
            // 
            this.lbl_Kapazitaetsplan.AutoSize = true;
            this.lbl_Kapazitaetsplan.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(218)))), ((int)(((byte)(227)))), ((int)(((byte)(243)))));
            this.lbl_Kapazitaetsplan.Font = new System.Drawing.Font("Corbel", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Kapazitaetsplan.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lbl_Kapazitaetsplan.Location = new System.Drawing.Point(573, 63);
            this.lbl_Kapazitaetsplan.Name = "lbl_Kapazitaetsplan";
            this.lbl_Kapazitaetsplan.Size = new System.Drawing.Size(110, 19);
            this.lbl_Kapazitaetsplan.TabIndex = 147;
            this.lbl_Kapazitaetsplan.Text = "Kapazitätsplan";
            this.lbl_Kapazitaetsplan.Click += new System.EventHandler(this.lbl_Kapazitaetsplan_Click);
            // 
            // btn_back
            // 
            this.btn_back.BackColor = System.Drawing.Color.Lavender;
            this.btn_back.Location = new System.Drawing.Point(59, 437);
            this.btn_back.Name = "btn_back";
            this.btn_back.Size = new System.Drawing.Size(116, 23);
            this.btn_back.TabIndex = 148;
            this.btn_back.Text = "Zurück";
            this.btn_back.UseVisualStyleBackColor = false;
            this.btn_back.Click += new System.EventHandler(this.btn_back_Click);
            // 
            // continue_btn
            // 
            this.continue_btn.BackColor = System.Drawing.Color.Lavender;
            this.continue_btn.Location = new System.Drawing.Point(822, 437);
            this.continue_btn.Name = "continue_btn";
            this.continue_btn.Size = new System.Drawing.Size(116, 23);
            this.continue_btn.TabIndex = 149;
            this.continue_btn.Text = "Weiter";
            this.continue_btn.UseVisualStyleBackColor = false;
            this.continue_btn.Click += new System.EventHandler(this.continue_btn_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Location = new System.Drawing.Point(29, 124);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(943, 307);
            this.groupBox1.TabIndex = 150;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Produktionsreihenfolge und Splittung";
            // 
            // Produktionsreihenfolge
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.continue_btn);
            this.Controls.Add(this.btn_back);
            this.Controls.Add(this.lbl_Kapazitaetsplan);
            this.Controls.Add(this.lbl_Produktionsreihenfolge);
            this.Controls.Add(this.lbl_Produktion);
            this.Controls.Add(this.lbl_Kaufteiledisposition);
            this.Controls.Add(this.lbl_Ergebnis);
            this.Controls.Add(this.lbl_Sicherheitsbestand);
            this.Controls.Add(this.lbl_Startseite);
            this.Controls.Add(this.pic_en);
            this.Controls.Add(this.pic_de);
            this.Controls.Add(this.pictureBox4);
            this.Name = "Produktionsreihenfolge";
            this.Size = new System.Drawing.Size(1000, 500);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pic_de)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pic_en)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox4;
        private System.Windows.Forms.PictureBox pic_de;
        private System.Windows.Forms.PictureBox pic_en;
        private System.Windows.Forms.Label lbl_Startseite;
        private System.Windows.Forms.Label lbl_Sicherheitsbestand;
        private System.Windows.Forms.Label lbl_Ergebnis;
        private System.Windows.Forms.Label lbl_Kaufteiledisposition;
        private System.Windows.Forms.Label lbl_Produktion;
        private System.Windows.Forms.Label lbl_Produktionsreihenfolge;
        private System.Windows.Forms.Label lbl_Kapazitaetsplan;
        private System.Windows.Forms.Button btn_back;
        private System.Windows.Forms.Button continue_btn;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}