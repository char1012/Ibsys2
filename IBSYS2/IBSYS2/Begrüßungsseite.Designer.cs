namespace IBSYS2
{
    partial class Begrüßungsseite
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Begrüßungsseite));
            this.Logo = new System.Windows.Forms.PictureBox();
            this.Starten = new System.Windows.Forms.PictureBox();
            this.clear_btn = new System.Windows.Forms.Button();
            this.pic_en = new System.Windows.Forms.PictureBox();
            this.pic_de = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.Logo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Starten)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pic_en)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pic_de)).BeginInit();
            this.SuspendLayout();
            // 
            // Logo
            // 
            this.Logo.Image = ((System.Drawing.Image)(resources.GetObject("Logo.Image")));
            this.Logo.Location = new System.Drawing.Point(352, 71);
            this.Logo.Name = "Logo";
            this.Logo.Size = new System.Drawing.Size(217, 80);
            this.Logo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Logo.TabIndex = 85;
            this.Logo.TabStop = false;
            // 
            // Starten
            // 
            this.Starten.Image = ((System.Drawing.Image)(resources.GetObject("Starten.Image")));
            this.Starten.Location = new System.Drawing.Point(384, 191);
            this.Starten.Name = "Starten";
            this.Starten.Size = new System.Drawing.Size(134, 117);
            this.Starten.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Starten.TabIndex = 86;
            this.Starten.TabStop = false;
            this.Starten.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // clear_btn
            // 
            this.clear_btn.Location = new System.Drawing.Point(384, 332);
            this.clear_btn.Name = "clear_btn";
            this.clear_btn.Size = new System.Drawing.Size(134, 23);
            this.clear_btn.TabIndex = 87;
            this.clear_btn.Text = "Datenbank leeren";
            this.clear_btn.UseVisualStyleBackColor = true;
            this.clear_btn.Click += new System.EventHandler(this.Clear_btn_Click);
            // 
            // pic_en
            // 
            this.pic_en.Image = ((System.Drawing.Image)(resources.GetObject("pic_en.Image")));
            this.pic_en.Location = new System.Drawing.Point(908, 10);
            this.pic_en.Margin = new System.Windows.Forms.Padding(2);
            this.pic_en.Name = "pic_en";
            this.pic_en.Size = new System.Drawing.Size(30, 19);
            this.pic_en.TabIndex = 151;
            this.pic_en.TabStop = false;
            this.pic_en.Click += new System.EventHandler(this.pic_en_Click);
            // 
            // pic_de
            // 
            this.pic_de.Image = ((System.Drawing.Image)(resources.GetObject("pic_de.Image")));
            this.pic_de.Location = new System.Drawing.Point(942, 10);
            this.pic_de.Margin = new System.Windows.Forms.Padding(2);
            this.pic_de.Name = "pic_de";
            this.pic_de.Size = new System.Drawing.Size(30, 19);
            this.pic_de.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pic_de.TabIndex = 150;
            this.pic_de.TabStop = false;
            this.pic_de.Click += new System.EventHandler(this.pic_de_Click);
            // 
            // Begrüßungsseite
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(999, 512);
            this.Controls.Add(this.pic_en);
            this.Controls.Add(this.pic_de);
            this.Controls.Add(this.clear_btn);
            this.Controls.Add(this.Starten);
            this.Controls.Add(this.Logo);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Begrüßungsseite";
            this.Text = "SCMPlus";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Begrüßungsseite_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.Logo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Starten)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pic_en)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pic_de)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox Logo;
        private System.Windows.Forms.PictureBox Starten;
        private System.Windows.Forms.Button clear_btn;
        private System.Windows.Forms.PictureBox pic_en;
        private System.Windows.Forms.PictureBox pic_de;
    }
}