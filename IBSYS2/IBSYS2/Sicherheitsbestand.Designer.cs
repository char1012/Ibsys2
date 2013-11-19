namespace IBSYS2
{
    partial class Sicherheitsbestand
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
            this.Ueberschrift = new System.Windows.Forms.Label();
            this.Eingabe_P1 = new System.Windows.Forms.TextBox();
            this.Eingabe_P2 = new System.Windows.Forms.TextBox();
            this.Eingabe_P3 = new System.Windows.Forms.TextBox();
            this.P1 = new System.Windows.Forms.Label();
            this.P2 = new System.Windows.Forms.Label();
            this.P3 = new System.Windows.Forms.Label();
            this.continue_btn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Ueberschrift
            // 
            this.Ueberschrift.AutoSize = true;
            this.Ueberschrift.Location = new System.Drawing.Point(56, 50);
            this.Ueberschrift.Name = "Ueberschrift";
            this.Ueberschrift.Size = new System.Drawing.Size(97, 13);
            this.Ueberschrift.TabIndex = 0;
            this.Ueberschrift.Text = "Sicherheitsbestand";
            this.Ueberschrift.Click += new System.EventHandler(this.label1_Click);
            // 
            // Eingabe_P1
            // 
            this.Eingabe_P1.Location = new System.Drawing.Point(103, 97);
            this.Eingabe_P1.Name = "Eingabe_P1";
            this.Eingabe_P1.Size = new System.Drawing.Size(100, 20);
            this.Eingabe_P1.TabIndex = 1;
            // 
            // Eingabe_P2
            // 
            this.Eingabe_P2.Location = new System.Drawing.Point(103, 133);
            this.Eingabe_P2.Name = "Eingabe_P2";
            this.Eingabe_P2.Size = new System.Drawing.Size(100, 20);
            this.Eingabe_P2.TabIndex = 2;
            // 
            // Eingabe_P3
            // 
            this.Eingabe_P3.Location = new System.Drawing.Point(103, 170);
            this.Eingabe_P3.Name = "Eingabe_P3";
            this.Eingabe_P3.Size = new System.Drawing.Size(100, 20);
            this.Eingabe_P3.TabIndex = 3;
            // 
            // P1
            // 
            this.P1.AutoSize = true;
            this.P1.Location = new System.Drawing.Point(56, 97);
            this.P1.Name = "P1";
            this.P1.Size = new System.Drawing.Size(20, 13);
            this.P1.TabIndex = 4;
            this.P1.Text = "P1";
            // 
            // P2
            // 
            this.P2.AutoSize = true;
            this.P2.Location = new System.Drawing.Point(56, 136);
            this.P2.Name = "P2";
            this.P2.Size = new System.Drawing.Size(20, 13);
            this.P2.TabIndex = 5;
            this.P2.Text = "P2";
            // 
            // P3
            // 
            this.P3.AutoSize = true;
            this.P3.Location = new System.Drawing.Point(56, 173);
            this.P3.Name = "P3";
            this.P3.Size = new System.Drawing.Size(20, 13);
            this.P3.TabIndex = 6;
            this.P3.Text = "P3";
            // 
            // continue_btn
            // 
            this.continue_btn.Location = new System.Drawing.Point(296, 212);
            this.continue_btn.Name = "continue_btn";
            this.continue_btn.Size = new System.Drawing.Size(75, 23);
            this.continue_btn.TabIndex = 7;
            this.continue_btn.Text = "Fortfahren";
            this.continue_btn.UseVisualStyleBackColor = true;
            // 
            // Sicherheitsbestand
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(426, 261);
            this.Controls.Add(this.continue_btn);
            this.Controls.Add(this.P3);
            this.Controls.Add(this.P2);
            this.Controls.Add(this.P1);
            this.Controls.Add(this.Eingabe_P3);
            this.Controls.Add(this.Eingabe_P2);
            this.Controls.Add(this.Eingabe_P1);
            this.Controls.Add(this.Ueberschrift);
            this.Name = "Sicherheitsbestand";
            this.Text = "Sicherheitsbestand";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label Ueberschrift;
        private System.Windows.Forms.TextBox Eingabe_P1;
        private System.Windows.Forms.TextBox Eingabe_P2;
        private System.Windows.Forms.TextBox Eingabe_P3;
        private System.Windows.Forms.Label P1;
        private System.Windows.Forms.Label P2;
        private System.Windows.Forms.Label P3;
        private System.Windows.Forms.Button continue_btn;
    }
}