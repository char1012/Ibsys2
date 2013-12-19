namespace IBSYS2
{
    partial class Splitting
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Splitting));
            this.Splitting1 = new System.Windows.Forms.TextBox();
            this.Splitting2 = new System.Windows.Forms.TextBox();
            this.continue_btn = new System.Windows.Forms.Button();
            this.abr_btn = new System.Windows.Forms.Button();
            this.lbl_teil = new System.Windows.Forms.Label();
            this.Menge = new System.Windows.Forms.TextBox();
            this.gp_sp = new System.Windows.Forms.GroupBox();
            this.NR = new System.Windows.Forms.Label();
            this.infoP = new System.Windows.Forms.PictureBox();
            this.pic_en = new System.Windows.Forms.PictureBox();
            this.pic_de = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.gp_sp.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.infoP)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pic_en)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pic_de)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // Splitting1
            // 
            this.Splitting1.Location = new System.Drawing.Point(177, 34);
            this.Splitting1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Splitting1.Name = "Splitting1";
            this.Splitting1.Size = new System.Drawing.Size(71, 22);
            this.Splitting1.TabIndex = 1;
            this.Splitting1.TextChanged += new System.EventHandler(this.Splitting1_TextChanged);
            // 
            // Splitting2
            // 
            this.Splitting2.Location = new System.Drawing.Point(177, 75);
            this.Splitting2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Splitting2.Name = "Splitting2";
            this.Splitting2.ReadOnly = true;
            this.Splitting2.Size = new System.Drawing.Size(71, 22);
            this.Splitting2.TabIndex = 2;
            this.Splitting2.TextChanged += new System.EventHandler(this.Splitting2_TextChanged);
            // 
            // continue_btn
            // 
            this.continue_btn.BackColor = System.Drawing.Color.Lavender;
            this.continue_btn.Location = new System.Drawing.Point(236, 235);
            this.continue_btn.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.continue_btn.Name = "continue_btn";
            this.continue_btn.Size = new System.Drawing.Size(100, 28);
            this.continue_btn.TabIndex = 5;
            this.continue_btn.Text = "OK";
            this.continue_btn.UseVisualStyleBackColor = false;
            this.continue_btn.Click += new System.EventHandler(this.continue_btn_Click);
            // 
            // abr_btn
            // 
            this.abr_btn.BackColor = System.Drawing.Color.Lavender;
            this.abr_btn.Location = new System.Drawing.Point(141, 235);
            this.abr_btn.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.abr_btn.Name = "abr_btn";
            this.abr_btn.Size = new System.Drawing.Size(87, 28);
            this.abr_btn.TabIndex = 6;
            this.abr_btn.Text = "Abbruch";
            this.abr_btn.UseVisualStyleBackColor = false;
            this.abr_btn.Click += new System.EventHandler(this.abr_btn_Click);
            // 
            // lbl_teil
            // 
            this.lbl_teil.AutoSize = true;
            this.lbl_teil.Location = new System.Drawing.Point(4, 61);
            this.lbl_teil.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbl_teil.Name = "lbl_teil";
            this.lbl_teil.Size = new System.Drawing.Size(31, 17);
            this.lbl_teil.TabIndex = 7;
            this.lbl_teil.Text = "Teil";
            // 
            // Menge
            // 
            this.Menge.Location = new System.Drawing.Point(62, 58);
            this.Menge.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Menge.Name = "Menge";
            this.Menge.ReadOnly = true;
            this.Menge.Size = new System.Drawing.Size(52, 22);
            this.Menge.TabIndex = 8;
            // 
            // gp_sp
            // 
            this.gp_sp.Controls.Add(this.pictureBox1);
            this.gp_sp.Controls.Add(this.infoP);
            this.gp_sp.Controls.Add(this.Splitting2);
            this.gp_sp.Controls.Add(this.Splitting1);
            this.gp_sp.Controls.Add(this.lbl_teil);
            this.gp_sp.Controls.Add(this.Menge);
            this.gp_sp.Controls.Add(this.NR);
            this.gp_sp.Location = new System.Drawing.Point(27, 51);
            this.gp_sp.Name = "gp_sp";
            this.gp_sp.Size = new System.Drawing.Size(309, 142);
            this.gp_sp.TabIndex = 9;
            this.gp_sp.TabStop = false;
            this.gp_sp.Text = "Splittung";
            // 
            // NR
            // 
            this.NR.AutoSize = true;
            this.NR.Location = new System.Drawing.Point(31, 61);
            this.NR.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.NR.Name = "NR";
            this.NR.Size = new System.Drawing.Size(28, 17);
            this.NR.TabIndex = 0;
            this.NR.Text = "NR";
            // 
            // infoP
            // 
            this.infoP.Image = ((System.Drawing.Image)(resources.GetObject("infoP.Image")));
            this.infoP.Location = new System.Drawing.Point(256, 47);
            this.infoP.Margin = new System.Windows.Forms.Padding(4);
            this.infoP.Name = "infoP";
            this.infoP.Size = new System.Drawing.Size(45, 43);
            this.infoP.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.infoP.TabIndex = 85;
            this.infoP.TabStop = false;
            // 
            // pic_en
            // 
            this.pic_en.Image = ((System.Drawing.Image)(resources.GetObject("pic_en.Image")));
            this.pic_en.Location = new System.Drawing.Point(258, 11);
            this.pic_en.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pic_en.Name = "pic_en";
            this.pic_en.Size = new System.Drawing.Size(40, 23);
            this.pic_en.TabIndex = 141;
            this.pic_en.TabStop = false;
            this.pic_en.Click += new System.EventHandler(this.pic_en_Click);
            // 
            // pic_de
            // 
            this.pic_de.Image = ((System.Drawing.Image)(resources.GetObject("pic_de.Image")));
            this.pic_de.Location = new System.Drawing.Point(303, 11);
            this.pic_de.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pic_de.Name = "pic_de";
            this.pic_de.Size = new System.Drawing.Size(40, 23);
            this.pic_de.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pic_de.TabIndex = 140;
            this.pic_de.TabStop = false;
            this.pic_de.Click += new System.EventHandler(this.pic_de_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(116, 28);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(58, 79);
            this.pictureBox1.TabIndex = 142;
            this.pictureBox1.TabStop = false;
            // 
            // Splitting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(361, 283);
            this.Controls.Add(this.pic_en);
            this.Controls.Add(this.pic_de);
            this.Controls.Add(this.continue_btn);
            this.Controls.Add(this.gp_sp);
            this.Controls.Add(this.abr_btn);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "Splitting";
            this.Text = "SCMPlus";
            this.gp_sp.ResumeLayout(false);
            this.gp_sp.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.infoP)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pic_en)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pic_de)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox Splitting1;
        private System.Windows.Forms.TextBox Splitting2;
        private System.Windows.Forms.Button continue_btn;
        private System.Windows.Forms.Button abr_btn;
        private System.Windows.Forms.Label lbl_teil;
        private System.Windows.Forms.TextBox Menge;
        private System.Windows.Forms.GroupBox gp_sp;
        private System.Windows.Forms.Label NR;
        private System.Windows.Forms.PictureBox infoP;
        private System.Windows.Forms.PictureBox pic_en;
        private System.Windows.Forms.PictureBox pic_de;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}