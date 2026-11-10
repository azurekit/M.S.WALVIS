namespace ProjetoCruzeiro
{
    partial class Ato1Level3
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Ato1Level3));
            this.pbx_canos = new System.Windows.Forms.PictureBox();
            this.pbx_movimentos = new System.Windows.Forms.PictureBox();
            this.lbl_movimentos = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pbx_canos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbx_movimentos)).BeginInit();
            this.SuspendLayout();
            // 
            // pbx_canos
            // 
            this.pbx_canos.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pbx_canos.BackgroundImage")));
            this.pbx_canos.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pbx_canos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbx_canos.Location = new System.Drawing.Point(0, 0);
            this.pbx_canos.Name = "pbx_canos";
            this.pbx_canos.Size = new System.Drawing.Size(960, 720);
            this.pbx_canos.TabIndex = 0;
            this.pbx_canos.TabStop = false;
            // 
            // pbx_movimentos
            // 
            this.pbx_movimentos.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pbx_movimentos.BackgroundImage")));
            this.pbx_movimentos.Location = new System.Drawing.Point(619, 64);
            this.pbx_movimentos.Name = "pbx_movimentos";
            this.pbx_movimentos.Size = new System.Drawing.Size(341, 68);
            this.pbx_movimentos.TabIndex = 1;
            this.pbx_movimentos.TabStop = false;
            // 
            // lbl_movimentos
            // 
            this.lbl_movimentos.AutoSize = true;
            this.lbl_movimentos.Font = new System.Drawing.Font("Eras Medium ITC", 18F);
            this.lbl_movimentos.ForeColor = System.Drawing.Color.Red;
            this.lbl_movimentos.Location = new System.Drawing.Point(917, 84);
            this.lbl_movimentos.Name = "lbl_movimentos";
            this.lbl_movimentos.Size = new System.Drawing.Size(40, 27);
            this.lbl_movimentos.TabIndex = 2;
            this.lbl_movimentos.Text = "99";
            // 
            // Ato1Level3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.Controls.Add(this.lbl_movimentos);
            this.Controls.Add(this.pbx_movimentos);
            this.Controls.Add(this.pbx_canos);
            this.Name = "Ato1Level3";
            this.Size = new System.Drawing.Size(960, 720);
            ((System.ComponentModel.ISupportInitialize)(this.pbx_canos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbx_movimentos)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pbx_canos;
        private System.Windows.Forms.PictureBox pbx_movimentos;
        private System.Windows.Forms.Label lbl_movimentos;
    }
}
