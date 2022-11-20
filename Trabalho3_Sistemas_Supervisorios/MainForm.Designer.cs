namespace Trabalho3_Sistemas_Supervisorios
{
    partial class MainForm
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.labelEstadoEsteira = new System.Windows.Forms.Label();
            this.buttonReset = new System.Windows.Forms.Button();
            this.buttonStart = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxCountGeralTransp = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxCountGeralOpacas = new System.Windows.Forms.TextBox();
            this.labelTransp = new System.Windows.Forms.Label();
            this.textBoxCountTransp = new System.Windows.Forms.TextBox();
            this.labelOpacas = new System.Windows.Forms.Label();
            this.textBoxCountOpacas = new System.Windows.Forms.TextBox();
            this.labelCountGeral = new System.Windows.Forms.Label();
            this.labelContadorDePecas = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.labelEstadoEsteira);
            this.panel1.Controls.Add(this.buttonReset);
            this.panel1.Controls.Add(this.buttonStart);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.textBoxCountGeralTransp);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.textBoxCountGeralOpacas);
            this.panel1.Controls.Add(this.labelTransp);
            this.panel1.Controls.Add(this.textBoxCountTransp);
            this.panel1.Controls.Add(this.labelOpacas);
            this.panel1.Controls.Add(this.textBoxCountOpacas);
            this.panel1.Controls.Add(this.labelCountGeral);
            this.panel1.Controls.Add(this.labelContadorDePecas);
            this.panel1.Location = new System.Drawing.Point(49, 26);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(364, 324);
            this.panel1.TabIndex = 0;
            // 
            // labelEstadoEsteira
            // 
            this.labelEstadoEsteira.AutoSize = true;
            this.labelEstadoEsteira.Location = new System.Drawing.Point(145, 235);
            this.labelEstadoEsteira.Name = "labelEstadoEsteira";
            this.labelEstadoEsteira.Size = new System.Drawing.Size(79, 13);
            this.labelEstadoEsteira.TabIndex = 12;
            this.labelEstadoEsteira.Text = "Esteira Parada.";
            // 
            // buttonReset
            // 
            this.buttonReset.Location = new System.Drawing.Point(215, 265);
            this.buttonReset.Name = "buttonReset";
            this.buttonReset.Size = new System.Drawing.Size(107, 37);
            this.buttonReset.TabIndex = 11;
            this.buttonReset.Text = "Reset";
            this.buttonReset.UseVisualStyleBackColor = true;
            this.buttonReset.Click += new System.EventHandler(this.buttonReset_Click);
            // 
            // buttonStart
            // 
            this.buttonStart.Location = new System.Drawing.Point(43, 265);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(107, 37);
            this.buttonStart.TabIndex = 10;
            this.buttonStart.Text = "Iniciar";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(175, 177);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(78, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Transparentes:";
            // 
            // textBoxCountGeralTransp
            // 
            this.textBoxCountGeralTransp.Location = new System.Drawing.Point(259, 174);
            this.textBoxCountGeralTransp.Name = "textBoxCountGeralTransp";
            this.textBoxCountGeralTransp.ReadOnly = true;
            this.textBoxCountGeralTransp.Size = new System.Drawing.Size(72, 20);
            this.textBoxCountGeralTransp.TabIndex = 8;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(25, 177);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Opacas:";
            // 
            // textBoxCountGeralOpacas
            // 
            this.textBoxCountGeralOpacas.Location = new System.Drawing.Point(78, 174);
            this.textBoxCountGeralOpacas.Name = "textBoxCountGeralOpacas";
            this.textBoxCountGeralOpacas.ReadOnly = true;
            this.textBoxCountGeralOpacas.Size = new System.Drawing.Size(72, 20);
            this.textBoxCountGeralOpacas.TabIndex = 6;
            // 
            // labelTransp
            // 
            this.labelTransp.AutoSize = true;
            this.labelTransp.Location = new System.Drawing.Point(175, 63);
            this.labelTransp.Name = "labelTransp";
            this.labelTransp.Size = new System.Drawing.Size(78, 13);
            this.labelTransp.TabIndex = 5;
            this.labelTransp.Text = "Transparentes:";
            // 
            // textBoxCountTransp
            // 
            this.textBoxCountTransp.Location = new System.Drawing.Point(259, 60);
            this.textBoxCountTransp.Name = "textBoxCountTransp";
            this.textBoxCountTransp.ReadOnly = true;
            this.textBoxCountTransp.Size = new System.Drawing.Size(72, 20);
            this.textBoxCountTransp.TabIndex = 4;
            // 
            // labelOpacas
            // 
            this.labelOpacas.AutoSize = true;
            this.labelOpacas.Location = new System.Drawing.Point(25, 63);
            this.labelOpacas.Name = "labelOpacas";
            this.labelOpacas.Size = new System.Drawing.Size(47, 13);
            this.labelOpacas.TabIndex = 3;
            this.labelOpacas.Text = "Opacas:";
            // 
            // textBoxCountOpacas
            // 
            this.textBoxCountOpacas.Location = new System.Drawing.Point(78, 60);
            this.textBoxCountOpacas.Name = "textBoxCountOpacas";
            this.textBoxCountOpacas.ReadOnly = true;
            this.textBoxCountOpacas.Size = new System.Drawing.Size(72, 20);
            this.textBoxCountOpacas.TabIndex = 2;
            // 
            // labelCountGeral
            // 
            this.labelCountGeral.AutoSize = true;
            this.labelCountGeral.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F);
            this.labelCountGeral.Location = new System.Drawing.Point(38, 120);
            this.labelCountGeral.Name = "labelCountGeral";
            this.labelCountGeral.Size = new System.Drawing.Size(284, 29);
            this.labelCountGeral.TabIndex = 1;
            this.labelCountGeral.Text = "Contador Geral de Peças";
            // 
            // labelContadorDePecas
            // 
            this.labelContadorDePecas.AutoSize = true;
            this.labelContadorDePecas.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F);
            this.labelContadorDePecas.Location = new System.Drawing.Point(59, 10);
            this.labelContadorDePecas.Name = "labelContadorDePecas";
            this.labelContadorDePecas.Size = new System.Drawing.Size(219, 29);
            this.labelContadorDePecas.TabIndex = 0;
            this.labelContadorDePecas.Text = "Contador de Peças";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(472, 406);
            this.Controls.Add(this.panel1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label labelTransp;
        private System.Windows.Forms.TextBox textBoxCountTransp;
        private System.Windows.Forms.Label labelOpacas;
        private System.Windows.Forms.TextBox textBoxCountOpacas;
        private System.Windows.Forms.Label labelCountGeral;
        private System.Windows.Forms.Label labelContadorDePecas;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxCountGeralTransp;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxCountGeralOpacas;
        private System.Windows.Forms.Label labelEstadoEsteira;
        private System.Windows.Forms.Button buttonReset;
        private System.Windows.Forms.Button buttonStart;
    }
}

