namespace ProjetoCruzeiro
{
    partial class JogoForm
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
            this.panelJogo1 = new System.Windows.Forms.Panel();
            this.panelJogo2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.panelJogo2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelJogo1
            // 
            this.panelJogo1.Location = new System.Drawing.Point(0, 0);
            this.panelJogo1.Name = "panelJogo1";
            this.panelJogo1.Size = new System.Drawing.Size(960, 720);
            this.panelJogo1.TabIndex = 2;
            this.panelJogo1.Paint += new System.Windows.Forms.PaintEventHandler(this.panelJogo1_Paint);
            // 
            // panelJogo2
            // 
            this.panelJogo2.Controls.Add(this.panel3);
            this.panelJogo2.Location = new System.Drawing.Point(959, 0);
            this.panelJogo2.Name = "panelJogo2";
            this.panelJogo2.Size = new System.Drawing.Size(324, 720);
            this.panelJogo2.TabIndex = 4;
            // 
            // panel3
            // 
            this.panel3.Location = new System.Drawing.Point(927, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(354, 720);
            this.panel3.TabIndex = 3;
            // 
            // JogoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1279, 718);
            this.Controls.Add(this.panelJogo2);
            this.Controls.Add(this.panelJogo1);
            this.Name = "JogoForm";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.JogoForm_Load);
            this.panelJogo2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelJogo1;
        private System.Windows.Forms.Panel panelJogo2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
    }
}