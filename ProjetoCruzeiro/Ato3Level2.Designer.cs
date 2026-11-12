namespace ProjetoCruzeiro
{
    partial class Ato3Level2
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Ato3Level2));
            this.flpFalas = new System.Windows.Forms.FlowLayoutPanel();
            this.flpResposta = new System.Windows.Forms.FlowLayoutPanel();
            this.lblNomePeca = new System.Windows.Forms.Label();
            this.pbx_reiniciar = new System.Windows.Forms.PictureBox();
            this.pbx_confirmar = new System.Windows.Forms.PictureBox();
            this.pbx_fala = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbx_reiniciar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbx_confirmar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbx_fala)).BeginInit();
            this.SuspendLayout();
            // 
            // flpFalas
            // 
            this.flpFalas.AllowDrop = true;
            this.flpFalas.AutoScroll = true;
            this.flpFalas.BackColor = System.Drawing.Color.Transparent;
            this.flpFalas.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.flpFalas.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flpFalas.Font = new System.Drawing.Font("Times New Roman", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.flpFalas.Location = new System.Drawing.Point(27, 98);
            this.flpFalas.Name = "flpFalas";
            this.flpFalas.Size = new System.Drawing.Size(445, 600);
            this.flpFalas.TabIndex = 0;
            // 
            // flpResposta
            // 
            this.flpResposta.AllowDrop = true;
            this.flpResposta.AutoScroll = true;
            this.flpResposta.BackColor = System.Drawing.Color.Transparent;
            this.flpResposta.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flpResposta.Location = new System.Drawing.Point(495, 98);
            this.flpResposta.Name = "flpResposta";
            this.flpResposta.Size = new System.Drawing.Size(445, 600);
            this.flpResposta.TabIndex = 1;
            // 
            // lblNomePeca
            // 
            this.lblNomePeca.Font = new System.Drawing.Font("Times New Roman", 16.25F, System.Drawing.FontStyle.Bold);
            this.lblNomePeca.ForeColor = System.Drawing.Color.PapayaWhip;
            this.lblNomePeca.Location = new System.Drawing.Point(490, 19);
            this.lblNomePeca.Name = "lblNomePeca";
            this.lblNomePeca.Size = new System.Drawing.Size(295, 62);
            this.lblNomePeca.TabIndex = 0;
            this.lblNomePeca.Text = "*titulo*";
            this.lblNomePeca.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pbx_reiniciar
            // 
            this.pbx_reiniciar.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pbx_reiniciar.BackgroundImage")));
            this.pbx_reiniciar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pbx_reiniciar.Location = new System.Drawing.Point(343, 19);
            this.pbx_reiniciar.Name = "pbx_reiniciar";
            this.pbx_reiniciar.Size = new System.Drawing.Size(129, 61);
            this.pbx_reiniciar.TabIndex = 5;
            this.pbx_reiniciar.TabStop = false;
            // 
            // pbx_confirmar
            // 
            this.pbx_confirmar.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pbx_confirmar.BackgroundImage")));
            this.pbx_confirmar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pbx_confirmar.Location = new System.Drawing.Point(791, 17);
            this.pbx_confirmar.Name = "pbx_confirmar";
            this.pbx_confirmar.Size = new System.Drawing.Size(154, 61);
            this.pbx_confirmar.TabIndex = 5;
            this.pbx_confirmar.TabStop = false;
            // 
            // pbx_fala
            // 
            this.pbx_fala.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pbx_fala.BackgroundImage")));
            this.pbx_fala.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pbx_fala.Location = new System.Drawing.Point(0, 0);
            this.pbx_fala.Name = "pbx_fala";
            this.pbx_fala.Size = new System.Drawing.Size(315, 81);
            this.pbx_fala.TabIndex = 6;
            this.pbx_fala.TabStop = false;
            // 
            // Ato3Level2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.Controls.Add(this.pbx_fala);
            this.Controls.Add(this.pbx_confirmar);
            this.Controls.Add(this.pbx_reiniciar);
            this.Controls.Add(this.lblNomePeca);
            this.Controls.Add(this.flpResposta);
            this.Controls.Add(this.flpFalas);
            this.Name = "Ato3Level2";
            this.Size = new System.Drawing.Size(960, 720);
            ((System.ComponentModel.ISupportInitialize)(this.pbx_reiniciar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbx_confirmar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbx_fala)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flpFalas;
        private System.Windows.Forms.FlowLayoutPanel flpResposta;
        private System.Windows.Forms.Label lblNomePeca;
        private System.Windows.Forms.PictureBox pbx_reiniciar;
        private System.Windows.Forms.PictureBox pbx_confirmar;
        private System.Windows.Forms.PictureBox pbx_fala;
    }
}
