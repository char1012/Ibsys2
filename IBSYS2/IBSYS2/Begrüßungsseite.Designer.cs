﻿namespace IBSYS2
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
            this.pic_en = new System.Windows.Forms.PictureBox();
            this.pic_de = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.Logo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Starten)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pic_en)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pic_de)).BeginInit();
            this.SuspendLayout();
            // 
            // Logo
            // 
            this.Logo.Image = ((System.Drawing.Image)(resources.GetObject("Logo.Image")));
            this.Logo.Location = new System.Drawing.Point(1167, 567);
            this.Logo.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Logo.Name = "Logo";
            this.Logo.Size = new System.Drawing.Size(167, 63);
            this.Logo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Logo.TabIndex = 85;
            this.Logo.TabStop = false;
            // 
            // Starten
            // 
            this.Starten.Image = ((System.Drawing.Image)(resources.GetObject("Starten.Image")));
            this.Starten.Location = new System.Drawing.Point(492, 247);
            this.Starten.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Starten.Name = "Starten";
            this.Starten.Size = new System.Drawing.Size(316, 224);
            this.Starten.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Starten.TabIndex = 86;
            this.Starten.TabStop = false;
            this.Starten.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // pic_en
            // 
            this.pic_en.Image = ((System.Drawing.Image)(resources.GetObject("pic_en.Image")));
            this.pic_en.Location = new System.Drawing.Point(1232, 12);
            this.pic_en.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pic_en.Name = "pic_en";
            this.pic_en.Size = new System.Drawing.Size(40, 23);
            this.pic_en.TabIndex = 151;
            this.pic_en.TabStop = false;
            this.pic_en.Click += new System.EventHandler(this.pic_en_Click);
            // 
            // pic_de
            // 
            this.pic_de.Image = ((System.Drawing.Image)(resources.GetObject("pic_de.Image")));
            this.pic_de.Location = new System.Drawing.Point(1277, 12);
            this.pic_de.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pic_de.Name = "pic_de";
            this.pic_de.Size = new System.Drawing.Size(40, 23);
            this.pic_de.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pic_de.TabIndex = 150;
            this.pic_de.TabStop = false;
            this.pic_de.Click += new System.EventHandler(this.pic_de_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(261, 89);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(766, 81);
            this.label1.TabIndex = 152;
            this.label1.Text = "Willkommen beim SCMPlus";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // Begrüßungsseite
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1332, 630);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pic_en);
            this.Controls.Add(this.pic_de);
            this.Controls.Add(this.Starten);
            this.Controls.Add(this.Logo);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "Begrüßungsseite";
            this.Text = "SCMPlus";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Begrüßungsseite_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.Logo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Starten)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pic_en)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pic_de)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void label1_Click(object sender, System.EventArgs e)
        {
            throw new System.NotImplementedException();
        }

        #endregion

        private System.Windows.Forms.PictureBox Logo;
        private System.Windows.Forms.PictureBox Starten;
        private System.Windows.Forms.PictureBox pic_en;
        private System.Windows.Forms.PictureBox pic_de;
        private System.Windows.Forms.Label label1;
    }
}