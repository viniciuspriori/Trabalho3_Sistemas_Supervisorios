namespace Trabalho3_Sistemas_Supervisorios
{
    partial class Form1
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
            this.richTextBoxElements = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // richTextBoxElements
            // 
            this.richTextBoxElements.AccessibleRole = System.Windows.Forms.AccessibleRole.Caret;
            this.richTextBoxElements.Location = new System.Drawing.Point(36, 31);
            this.richTextBoxElements.Name = "richTextBoxElements";
            this.richTextBoxElements.ReadOnly = true;
            this.richTextBoxElements.Size = new System.Drawing.Size(536, 337);
            this.richTextBoxElements.TabIndex = 0;
            this.richTextBoxElements.Text = "";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.richTextBoxElements);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBoxElements;
    }
}

