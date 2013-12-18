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
            this.NR = new System.Windows.Forms.Label();
            this.Splitting1 = new System.Windows.Forms.TextBox();
            this.Splitting2 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.continue_btn = new System.Windows.Forms.Button();
            this.abr_btn = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.Menge = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // NR
            // 
            this.NR.AutoSize = true;
            this.NR.Location = new System.Drawing.Point(30, 96);
            this.NR.Name = "NR";
            this.NR.Size = new System.Drawing.Size(23, 13);
            this.NR.TabIndex = 0;
            this.NR.Text = "NR";
            // 
            // Splitting1
            // 
            this.Splitting1.Location = new System.Drawing.Point(140, 71);
            this.Splitting1.Name = "Splitting1";
            this.Splitting1.Size = new System.Drawing.Size(54, 20);
            this.Splitting1.TabIndex = 1;
            this.Splitting1.TextChanged += new System.EventHandler(this.Splitting1_TextChanged);
            // 
            // Splitting2
            // 
            this.Splitting2.Location = new System.Drawing.Point(140, 115);
            this.Splitting2.Name = "Splitting2";
            this.Splitting2.ReadOnly = true;
            this.Splitting2.Size = new System.Drawing.Size(54, 20);
            this.Splitting2.TabIndex = 2;
            this.Splitting2.TextChanged += new System.EventHandler(this.Splitting2_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(35, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(198, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Bitte geben Sie die Aufteilung an wie Sie";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(35, 41);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(199, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "die Produktion für das Teil splitten wollen";
            // 
            // continue_btn
            // 
            this.continue_btn.Location = new System.Drawing.Point(158, 157);
            this.continue_btn.Name = "continue_btn";
            this.continue_btn.Size = new System.Drawing.Size(75, 23);
            this.continue_btn.TabIndex = 5;
            this.continue_btn.Text = "Bestätigen";
            this.continue_btn.UseVisualStyleBackColor = true;
            this.continue_btn.Click += new System.EventHandler(this.continue_btn_Click);
            // 
            // abr_btn
            // 
            this.abr_btn.Location = new System.Drawing.Point(35, 157);
            this.abr_btn.Name = "abr_btn";
            this.abr_btn.Size = new System.Drawing.Size(65, 23);
            this.abr_btn.TabIndex = 6;
            this.abr_btn.Text = "Abbruch";
            this.abr_btn.UseVisualStyleBackColor = true;
            this.abr_btn.Click += new System.EventHandler(this.abr_btn_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 96);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(24, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Teil";
            // 
            // Menge
            // 
            this.Menge.Location = new System.Drawing.Point(60, 93);
            this.Menge.Name = "Menge";
            this.Menge.ReadOnly = true;
            this.Menge.Size = new System.Drawing.Size(40, 20);
            this.Menge.TabIndex = 8;
            // 
            // Splitting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(252, 192);
            this.Controls.Add(this.Menge);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.abr_btn);
            this.Controls.Add(this.continue_btn);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.Splitting2);
            this.Controls.Add(this.Splitting1);
            this.Controls.Add(this.NR);
            this.Name = "Splitting";
            this.Text = "Splitting";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label NR;
        private System.Windows.Forms.TextBox Splitting1;
        private System.Windows.Forms.TextBox Splitting2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button continue_btn;
        private System.Windows.Forms.Button abr_btn;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox Menge;
    }
}