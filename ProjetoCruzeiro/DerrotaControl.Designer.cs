namespace ProjetoCruzeiro
{
    partial class DerrotaControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DerrotaControl));
            this.pnl_derrota = new System.Windows.Forms.Panel();
            this.pbx_voltar = new System.Windows.Forms.PictureBox();
            this.pbx_continuar = new System.Windows.Forms.PictureBox();
            this.pbx_estrelas = new System.Windows.Forms.PictureBox();
            this.lbl_texto = new System.Windows.Forms.Label();
            this.pnl_derrota.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbx_voltar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbx_continuar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbx_estrelas)).BeginInit();
            this.SuspendLayout();
            // 
            // pnl_derrota
            // 
            this.pnl_derrota.BackColor = System.Drawing.Color.Transparent;
            this.pnl_derrota.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pnl_derrota.BackgroundImage")));
            this.pnl_derrota.Controls.Add(this.pbx_voltar);
            this.pnl_derrota.Controls.Add(this.pbx_continuar);
            this.pnl_derrota.Controls.Add(this.lbl_texto);
            this.pnl_derrota.Controls.Add(this.pbx_estrelas);
            this.pnl_derrota.Location = new System.Drawing.Point(131, 2);
            this.pnl_derrota.Name = "pnl_derrota";
            this.pnl_derrota.Size = new System.Drawing.Size(699, 717);
            this.pnl_derrota.TabIndex = 1;
            // 
            // pbx_voltar
            // 
            this.pbx_voltar.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pbx_voltar.BackgroundImage")));
            this.pbx_voltar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pbx_voltar.Location = new System.Drawing.Point(180, 543);
            this.pbx_voltar.Name = "pbx_voltar";
            this.pbx_voltar.Size = new System.Drawing.Size(125, 125);
            this.pbx_voltar.TabIndex = 6;
            this.pbx_voltar.TabStop = false;
            // 
            // pbx_continuar
            // 
            this.pbx_continuar.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pbx_continuar.BackgroundImage")));
            this.pbx_continuar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pbx_continuar.Location = new System.Drawing.Point(401, 543);
            this.pbx_continuar.Name = "pbx_continuar";
            this.pbx_continuar.Size = new System.Drawing.Size(125, 125);
            this.pbx_continuar.TabIndex = 5;
            this.pbx_continuar.TabStop = false;
            // 
            // pbx_estrelas
            // 
            this.pbx_estrelas.BackColor = System.Drawing.Color.Transparent;
            this.pbx_estrelas.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pbx_estrelas.BackgroundImage")));
            this.pbx_estrelas.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pbx_estrelas.Location = new System.Drawing.Point(164, 151);
            this.pbx_estrelas.Name = "pbx_estrelas";
            this.pbx_estrelas.Size = new System.Drawing.Size(372, 160);
            this.pbx_estrelas.TabIndex = 0;
            this.pbx_estrelas.TabStop = false;
            // 
            // lbl_texto
            // 
            this.lbl_texto.Font = new System.Drawing.Font("Times New Roman", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_texto.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.lbl_texto.Location = new System.Drawing.Point(138, 365);
            this.lbl_texto.Name = "lbl_texto";
            this.lbl_texto.Size = new System.Drawing.Size(427, 135);
            this.lbl_texto.TabIndex = 1;
            this.lbl_texto.Text = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
            this.lbl_texto.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // DerrotaControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.pnl_derrota);
            this.Name = "DerrotaControl";
            this.Size = new System.Drawing.Size(960, 720);
            this.pnl_derrota.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbx_voltar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbx_continuar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbx_estrelas)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnl_derrota;
        private System.Windows.Forms.PictureBox pbx_voltar;
        private System.Windows.Forms.PictureBox pbx_continuar;
        private System.Windows.Forms.PictureBox pbx_estrelas;
        private System.Windows.Forms.Label lbl_texto;
    }
}
