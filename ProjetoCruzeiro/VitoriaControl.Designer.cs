namespace ProjetoCruzeiro
{
    partial class VitoriaControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VitoriaControl));
            this.pnl_vitoria = new System.Windows.Forms.Panel();
            this.pbx_voltar = new System.Windows.Forms.PictureBox();
            this.pbx_continuar = new System.Windows.Forms.PictureBox();
            this.pbx_pontuacaotexto = new System.Windows.Forms.PictureBox();
            this.lbl_score = new System.Windows.Forms.Label();
            this.pbx_estrelas = new System.Windows.Forms.PictureBox();
            this.pnl_vitoria.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbx_voltar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbx_continuar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbx_pontuacaotexto)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbx_estrelas)).BeginInit();
            this.SuspendLayout();
            // 
            // pnl_vitoria
            // 
            this.pnl_vitoria.BackColor = System.Drawing.Color.Transparent;
            this.pnl_vitoria.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pnl_vitoria.BackgroundImage")));
            this.pnl_vitoria.Controls.Add(this.pbx_voltar);
            this.pnl_vitoria.Controls.Add(this.pbx_continuar);
            this.pnl_vitoria.Controls.Add(this.pbx_pontuacaotexto);
            this.pnl_vitoria.Controls.Add(this.lbl_score);
            this.pnl_vitoria.Controls.Add(this.pbx_estrelas);
            this.pnl_vitoria.Location = new System.Drawing.Point(123, 3);
            this.pnl_vitoria.Name = "pnl_vitoria";
            this.pnl_vitoria.Size = new System.Drawing.Size(699, 717);
            this.pnl_vitoria.TabIndex = 0;
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
            this.pbx_voltar.Click += new System.EventHandler(this.pbx_voltar_Click);
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
            this.pbx_continuar.Click += new System.EventHandler(this.btn_continuar_Click);
            // 
            // pbx_pontuacaotexto
            // 
            this.pbx_pontuacaotexto.BackColor = System.Drawing.Color.Transparent;
            this.pbx_pontuacaotexto.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pbx_pontuacaotexto.BackgroundImage")));
            this.pbx_pontuacaotexto.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pbx_pontuacaotexto.Location = new System.Drawing.Point(240, 317);
            this.pbx_pontuacaotexto.Name = "pbx_pontuacaotexto";
            this.pbx_pontuacaotexto.Size = new System.Drawing.Size(214, 54);
            this.pbx_pontuacaotexto.TabIndex = 4;
            this.pbx_pontuacaotexto.TabStop = false;
            // 
            // lbl_score
            // 
            this.lbl_score.Font = new System.Drawing.Font("Chocolate Cookies", 80.25F);
            this.lbl_score.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.lbl_score.Location = new System.Drawing.Point(164, 374);
            this.lbl_score.Name = "lbl_score";
            this.lbl_score.Size = new System.Drawing.Size(372, 128);
            this.lbl_score.TabIndex = 1;
            this.lbl_score.Text = "12345";
            this.lbl_score.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
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
            // VitoriaControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.Controls.Add(this.pnl_vitoria);
            this.Name = "VitoriaControl";
            this.Size = new System.Drawing.Size(960, 720);
            this.pnl_vitoria.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbx_voltar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbx_continuar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbx_pontuacaotexto)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbx_estrelas)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnl_vitoria;
        private System.Windows.Forms.Label lbl_score;
        private System.Windows.Forms.PictureBox pbx_estrelas;
        private System.Windows.Forms.PictureBox pbx_continuar;
        private System.Windows.Forms.PictureBox pbx_pontuacaotexto;
        private System.Windows.Forms.PictureBox pbx_voltar;
    }
}
