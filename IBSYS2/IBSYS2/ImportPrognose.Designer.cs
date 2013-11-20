namespace IBSYS2
{
    partial class ImportPrognose
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.button2 = new System.Windows.Forms.Button();
            this.continue_btn = new System.Windows.Forms.Button();
            this.Periode1 = new System.Windows.Forms.Label();
            this.Periode2 = new System.Windows.Forms.Label();
            this.Periode3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.textBox7 = new System.Windows.Forms.TextBox();
            this.textBox8 = new System.Windows.Forms.TextBox();
            this.textBox9 = new System.Windows.Forms.TextBox();
            this.textBox10 = new System.Windows.Forms.TextBox();
            this.textBox11 = new System.Windows.Forms.TextBox();
            this.textBox12 = new System.Windows.Forms.TextBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(24, 226);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(138, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "Datei auswählen";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // continue_btn
            // 
            this.continue_btn.Location = new System.Drawing.Point(345, 226);
            this.continue_btn.Name = "continue_btn";
            this.continue_btn.Size = new System.Drawing.Size(138, 23);
            this.continue_btn.TabIndex = 2;
            this.continue_btn.Text = "Berechnung starten";
            this.continue_btn.UseVisualStyleBackColor = true;
            this.continue_btn.Click += new System.EventHandler(this.button1_Click);
            // 
            // Periode1
            // 
            this.Periode1.AutoSize = true;
            this.Periode1.Location = new System.Drawing.Point(164, 78);
            this.Periode1.Name = "Periode1";
            this.Periode1.Size = new System.Drawing.Size(53, 13);
            this.Periode1.TabIndex = 12;
            this.Periode1.Text = "Periode X";
            this.Periode1.Click += new System.EventHandler(this.label1_Click);
            // 
            // Periode2
            // 
            this.Periode2.AutoSize = true;
            this.Periode2.Location = new System.Drawing.Point(263, 78);
            this.Periode2.Name = "Periode2";
            this.Periode2.Size = new System.Drawing.Size(65, 13);
            this.Periode2.TabIndex = 13;
            this.Periode2.Text = "Periode X+1";
            // 
            // Periode3
            // 
            this.Periode3.AutoSize = true;
            this.Periode3.Location = new System.Drawing.Point(362, 78);
            this.Periode3.Name = "Periode3";
            this.Periode3.Size = new System.Drawing.Size(65, 13);
            this.Periode3.TabIndex = 14;
            this.Periode3.Text = "Periode X+2";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(68, 78);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 13);
            this.label1.TabIndex = 18;
            this.label1.Text = "Aktuelle Periode";
            this.label1.Click += new System.EventHandler(this.label1_Click_1);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(68, 123);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(84, 20);
            this.textBox1.TabIndex = 17;
            this.textBox1.Text = "P2";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(68, 149);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(84, 20);
            this.textBox2.TabIndex = 16;
            this.textBox2.Text = "P3";
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(68, 97);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(84, 20);
            this.textBox3.TabIndex = 15;
            this.textBox3.Text = "P1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(34, 100);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(20, 13);
            this.label2.TabIndex = 19;
            this.label2.Text = "P1";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(34, 126);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(20, 13);
            this.label3.TabIndex = 20;
            this.label3.Text = "P2";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(34, 152);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(20, 13);
            this.label4.TabIndex = 21;
            this.label4.Text = "P3";
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(167, 123);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(84, 20);
            this.textBox4.TabIndex = 24;
            this.textBox4.Text = "P2";
            // 
            // textBox5
            // 
            this.textBox5.Location = new System.Drawing.Point(167, 149);
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(84, 20);
            this.textBox5.TabIndex = 23;
            this.textBox5.Text = "P3";
            // 
            // textBox6
            // 
            this.textBox6.Location = new System.Drawing.Point(167, 97);
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new System.Drawing.Size(84, 20);
            this.textBox6.TabIndex = 22;
            this.textBox6.Text = "P1";
            // 
            // textBox7
            // 
            this.textBox7.Location = new System.Drawing.Point(266, 123);
            this.textBox7.Name = "textBox7";
            this.textBox7.Size = new System.Drawing.Size(84, 20);
            this.textBox7.TabIndex = 27;
            this.textBox7.Text = "P2";
            // 
            // textBox8
            // 
            this.textBox8.Location = new System.Drawing.Point(266, 149);
            this.textBox8.Name = "textBox8";
            this.textBox8.Size = new System.Drawing.Size(84, 20);
            this.textBox8.TabIndex = 26;
            this.textBox8.Text = "P3";
            // 
            // textBox9
            // 
            this.textBox9.Location = new System.Drawing.Point(266, 97);
            this.textBox9.Name = "textBox9";
            this.textBox9.Size = new System.Drawing.Size(84, 20);
            this.textBox9.TabIndex = 25;
            this.textBox9.Text = "P1";
            // 
            // textBox10
            // 
            this.textBox10.Location = new System.Drawing.Point(365, 123);
            this.textBox10.Name = "textBox10";
            this.textBox10.Size = new System.Drawing.Size(84, 20);
            this.textBox10.TabIndex = 30;
            this.textBox10.Text = "P2";
            // 
            // textBox11
            // 
            this.textBox11.Location = new System.Drawing.Point(365, 149);
            this.textBox11.Name = "textBox11";
            this.textBox11.Size = new System.Drawing.Size(84, 20);
            this.textBox11.TabIndex = 29;
            this.textBox11.Text = "P3";
            // 
            // textBox12
            // 
            this.textBox12.Location = new System.Drawing.Point(365, 97);
            this.textBox12.Name = "textBox12";
            this.textBox12.Size = new System.Drawing.Size(84, 20);
            this.textBox12.TabIndex = 28;
            this.textBox12.Text = "P1";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "Periode 1",
            "Periode 2",
            "Periode 3",
            "Periode 4",
            "Periode 5",
            "Periode 6",
            "Periode 7",
            "Periode 8",
            "Periode 9"});
            this.comboBox1.Location = new System.Drawing.Point(68, 36);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 21);
            this.comboBox1.TabIndex = 31;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // ImportPrognose
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(526, 261);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.textBox10);
            this.Controls.Add(this.textBox11);
            this.Controls.Add(this.textBox12);
            this.Controls.Add(this.textBox7);
            this.Controls.Add(this.textBox8);
            this.Controls.Add(this.textBox9);
            this.Controls.Add(this.textBox4);
            this.Controls.Add(this.textBox5);
            this.Controls.Add(this.textBox6);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.Periode3);
            this.Controls.Add(this.Periode2);
            this.Controls.Add(this.Periode1);
            this.Controls.Add(this.continue_btn);
            this.Controls.Add(this.button2);
            this.Name = "ImportPrognose";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.ImportPrognose_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button continue_btn;
        private System.Windows.Forms.Label Periode1;
        private System.Windows.Forms.Label Periode2;
        private System.Windows.Forms.Label Periode3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.TextBox textBox6;
        private System.Windows.Forms.TextBox textBox7;
        private System.Windows.Forms.TextBox textBox8;
        private System.Windows.Forms.TextBox textBox9;
        private System.Windows.Forms.TextBox textBox10;
        private System.Windows.Forms.TextBox textBox11;
        private System.Windows.Forms.TextBox textBox12;
        private System.Windows.Forms.ComboBox comboBox1;
    }
}

