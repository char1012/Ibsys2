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
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
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
            // 
            // Splitting2
            // 
            this.Splitting2.Location = new System.Drawing.Point(140, 115);
            this.Splitting2.Name = "Splitting2";
            this.Splitting2.Size = new System.Drawing.Size(54, 20);
            this.Splitting2.TabIndex = 2;
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
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(158, 157);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 5;
            this.button1.Text = "Bestätigen";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(35, 157);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(65, 23);
            this.button2.TabIndex = 6;
            this.button2.Text = "Abbruch";
            this.button2.UseVisualStyleBackColor = true;
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
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
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
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox Menge;
    }
}