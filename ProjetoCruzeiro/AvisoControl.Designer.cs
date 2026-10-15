namespace ProjetoCruzeiro
{
    partial class AvisoControl
    {
        /// <summary> 
        /// Variável de designer necessária.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Limpar os recursos que estão sendo usados.
        /// </summary>
        /// <param name="disposing">true se for necessário descartar os recursos gerenciados; caso contrário, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código gerado pelo Designer de Componentes

        /// <summary> 
        /// Método necessário para suporte ao Designer - não modifique 
        /// o conteúdo deste método com o editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.lbl_aviso = new System.Windows.Forms.Label();
            this.lbl_aviso2 = new System.Windows.Forms.Label();
            this.timerAviso = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "label1";
            // 
            // lbl_aviso
            // 
            this.lbl_aviso.AutoSize = true;
            this.lbl_aviso.Font = new System.Drawing.Font("League Spartan SemiBold", 30F);
            this.lbl_aviso.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.lbl_aviso.Location = new System.Drawing.Point(550, 161);
            this.lbl_aviso.Name = "lbl_aviso";
            this.lbl_aviso.Size = new System.Drawing.Size(147, 60);
            this.lbl_aviso.TabIndex = 1;
            this.lbl_aviso.Text = "AVISO!";
            // 
            // lbl_aviso2
            // 
            this.lbl_aviso2.Font = new System.Drawing.Font("League Spartan SemiBold", 20F);
            this.lbl_aviso2.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.lbl_aviso2.Location = new System.Drawing.Point(135, 326);
            this.lbl_aviso2.Name = "lbl_aviso2";
            this.lbl_aviso2.Size = new System.Drawing.Size(967, 149);
            this.lbl_aviso2.TabIndex = 2;
            this.lbl_aviso2.Text = "Esse jogo contêm tópicos sensíveis e pode não ser recomendado para todas as idade" +
    "s.";
            this.lbl_aviso2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lbl_aviso2.Click += new System.EventHandler(this.lbl_aviso2_Click);
            // 
            // timerAviso
            // 
            this.timerAviso.Interval = 4000;
            this.timerAviso.Tick += new System.EventHandler(this.timerAviso_Tick_1);
            // 
            // AvisoControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.Controls.Add(this.lbl_aviso2);
            this.Controls.Add(this.lbl_aviso);
            this.Controls.Add(this.label1);
            this.Name = "AvisoControl";
            this.Size = new System.Drawing.Size(1280, 720);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lbl_aviso;
        private System.Windows.Forms.Label lbl_aviso2;
        private System.Windows.Forms.Timer timerAviso;
    }
}
