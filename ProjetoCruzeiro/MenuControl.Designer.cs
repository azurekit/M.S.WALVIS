namespace ProjetoCruzeiro
{
    partial class MenuControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MenuControl));
            this.pbx_jogar = new System.Windows.Forms.PictureBox();
            this.pbx_loja = new System.Windows.Forms.PictureBox();
            this.pbx_sair = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbx_jogar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbx_loja)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbx_sair)).BeginInit();
            this.SuspendLayout();
            // 
            // pbx_jogar
            // 
            this.pbx_jogar.BackColor = System.Drawing.Color.Transparent;
            this.pbx_jogar.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pbx_jogar.BackgroundImage")));
            this.pbx_jogar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pbx_jogar.Location = new System.Drawing.Point(916, 78);
            this.pbx_jogar.Name = "pbx_jogar";
            this.pbx_jogar.Size = new System.Drawing.Size(300, 133);
            this.pbx_jogar.TabIndex = 5;
            this.pbx_jogar.TabStop = false;
            this.pbx_jogar.Click += new System.EventHandler(this.btn_iniciar_Click);
            // 
            // pbx_loja
            // 
            this.pbx_loja.BackColor = System.Drawing.Color.Transparent;
            this.pbx_loja.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pbx_loja.BackgroundImage")));
            this.pbx_loja.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pbx_loja.Location = new System.Drawing.Point(916, 233);
            this.pbx_loja.Name = "pbx_loja";
            this.pbx_loja.Size = new System.Drawing.Size(300, 133);
            this.pbx_loja.TabIndex = 7;
            this.pbx_loja.TabStop = false;
            this.pbx_loja.Click += new System.EventHandler(this.btn_loja_Click);
            // 
            // pbx_sair
            // 
            this.pbx_sair.BackColor = System.Drawing.Color.Transparent;
            this.pbx_sair.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pbx_sair.BackgroundImage")));
            this.pbx_sair.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pbx_sair.Location = new System.Drawing.Point(916, 393);
            this.pbx_sair.Name = "pbx_sair";
            this.pbx_sair.Size = new System.Drawing.Size(300, 133);
            this.pbx_sair.TabIndex = 9;
            this.pbx_sair.TabStop = false;
            this.pbx_sair.Click += new System.EventHandler(this.btn_sair_Click);
            // 
            // MenuControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.Controls.Add(this.pbx_sair);
            this.Controls.Add(this.pbx_loja);
            this.Controls.Add(this.pbx_jogar);
            this.Name = "MenuControl";
            this.Size = new System.Drawing.Size(1280, 720);
            ((System.ComponentModel.ISupportInitialize)(this.pbx_jogar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbx_loja)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbx_sair)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.PictureBox pbx_jogar;
        private System.Windows.Forms.PictureBox pbx_loja;
        private System.Windows.Forms.PictureBox pbx_sair;
    }
}
