namespace ProjetoCruzeiro
{
    partial class LevelSelectControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LevelSelectControl));
            this.btn_menu = new System.Windows.Forms.Button();
            this.lbl_caso = new System.Windows.Forms.Label();
            this.lbl_level_desc = new System.Windows.Forms.Label();
            this.pnl_painel = new System.Windows.Forms.Panel();
            this.lbl_level_highscore = new System.Windows.Forms.Label();
            this.pbx_estrelas = new System.Windows.Forms.PictureBox();
            this.pbx_level1_button = new System.Windows.Forms.PictureBox();
            this.pbx_level2_button = new System.Windows.Forms.PictureBox();
            this.pbx_level3_button = new System.Windows.Forms.PictureBox();
            this.pnl_painel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbx_estrelas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbx_level1_button)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbx_level2_button)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbx_level3_button)).BeginInit();
            this.SuspendLayout();
            // 
            // btn_menu
            // 
            this.btn_menu.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.btn_menu.Font = new System.Drawing.Font("League Spartan SemiBold", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_menu.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.btn_menu.Location = new System.Drawing.Point(14, 14);
            this.btn_menu.Name = "btn_menu";
            this.btn_menu.Size = new System.Drawing.Size(144, 48);
            this.btn_menu.TabIndex = 29;
            this.btn_menu.Text = "Voltar";
            this.btn_menu.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btn_menu.UseVisualStyleBackColor = false;
            this.btn_menu.Click += new System.EventHandler(this.btn_menu_Click);
            // 
            // lbl_caso
            // 
            this.lbl_caso.AutoSize = true;
            this.lbl_caso.BackColor = System.Drawing.Color.Transparent;
            this.lbl_caso.Font = new System.Drawing.Font("League Spartan ExtraBold", 60F);
            this.lbl_caso.ForeColor = System.Drawing.SystemColors.Control;
            this.lbl_caso.Location = new System.Drawing.Point(40, 243);
            this.lbl_caso.Name = "lbl_caso";
            this.lbl_caso.Size = new System.Drawing.Size(249, 119);
            this.lbl_caso.TabIndex = 30;
            this.lbl_caso.Text = "ATO 1";
            this.lbl_caso.Click += new System.EventHandler(this.lbl_caso_Click);
            // 
            // lbl_level_desc
            // 
            this.lbl_level_desc.AutoSize = true;
            this.lbl_level_desc.BackColor = System.Drawing.Color.Transparent;
            this.lbl_level_desc.Font = new System.Drawing.Font("League Spartan SemiBold", 17.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_level_desc.ForeColor = System.Drawing.SystemColors.Control;
            this.lbl_level_desc.Location = new System.Drawing.Point(22, 304);
            this.lbl_level_desc.Name = "lbl_level_desc";
            this.lbl_level_desc.Size = new System.Drawing.Size(308, 35);
            this.lbl_level_desc.TabIndex = 33;
            this.lbl_level_desc.Text = "Encontre a combinação certa!";
            this.lbl_level_desc.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnl_painel
            // 
            this.pnl_painel.BackColor = System.Drawing.Color.Transparent;
            this.pnl_painel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pnl_painel.BackgroundImage")));
            this.pnl_painel.Controls.Add(this.lbl_level_highscore);
            this.pnl_painel.Controls.Add(this.lbl_level_desc);
            this.pnl_painel.Controls.Add(this.pbx_estrelas);
            this.pnl_painel.Location = new System.Drawing.Point(513, 512);
            this.pnl_painel.Name = "pnl_painel";
            this.pnl_painel.Size = new System.Drawing.Size(764, 205);
            this.pnl_painel.TabIndex = 53;
            // 
            // lbl_level_highscore
            // 
            this.lbl_level_highscore.AutoSize = true;
            this.lbl_level_highscore.BackColor = System.Drawing.Color.Transparent;
            this.lbl_level_highscore.Font = new System.Drawing.Font("Times New Roman", 35.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_level_highscore.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lbl_level_highscore.Location = new System.Drawing.Point(55, 124);
            this.lbl_level_highscore.Name = "lbl_level_highscore";
            this.lbl_level_highscore.Size = new System.Drawing.Size(275, 53);
            this.lbl_level_highscore.TabIndex = 53;
            this.lbl_level_highscore.Text = "*Highscore*";
            this.lbl_level_highscore.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pbx_estrelas
            // 
            this.pbx_estrelas.BackColor = System.Drawing.Color.Transparent;
            this.pbx_estrelas.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pbx_estrelas.BackgroundImage")));
            this.pbx_estrelas.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pbx_estrelas.Location = new System.Drawing.Point(355, 71);
            this.pbx_estrelas.Name = "pbx_estrelas";
            this.pbx_estrelas.Size = new System.Drawing.Size(284, 117);
            this.pbx_estrelas.TabIndex = 43;
            this.pbx_estrelas.TabStop = false;
            // 
            // pbx_level1_button
            // 
            this.pbx_level1_button.Location = new System.Drawing.Point(60, 386);
            this.pbx_level1_button.Name = "pbx_level1_button";
            this.pbx_level1_button.Size = new System.Drawing.Size(299, 70);
            this.pbx_level1_button.TabIndex = 54;
            this.pbx_level1_button.TabStop = false;
            // 
            // pbx_level2_button
            // 
            this.pbx_level2_button.Location = new System.Drawing.Point(60, 510);
            this.pbx_level2_button.Name = "pbx_level2_button";
            this.pbx_level2_button.Size = new System.Drawing.Size(299, 70);
            this.pbx_level2_button.TabIndex = 55;
            this.pbx_level2_button.TabStop = false;
            // 
            // pbx_level3_button
            // 
            this.pbx_level3_button.Location = new System.Drawing.Point(60, 608);
            this.pbx_level3_button.Name = "pbx_level3_button";
            this.pbx_level3_button.Size = new System.Drawing.Size(299, 70);
            this.pbx_level3_button.TabIndex = 56;
            this.pbx_level3_button.TabStop = false;
            // 
            // LevelSelectControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Chartreuse;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.Controls.Add(this.pbx_level3_button);
            this.Controls.Add(this.pbx_level2_button);
            this.Controls.Add(this.pbx_level1_button);
            this.Controls.Add(this.pnl_painel);
            this.Controls.Add(this.lbl_caso);
            this.Controls.Add(this.btn_menu);
            this.DoubleBuffered = true;
            this.Name = "LevelSelectControl";
            this.Size = new System.Drawing.Size(1280, 720);
            this.pnl_painel.ResumeLayout(false);
            this.pnl_painel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbx_estrelas)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbx_level1_button)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbx_level2_button)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbx_level3_button)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_menu;
        private System.Windows.Forms.Label lbl_caso;
        private System.Windows.Forms.Label lbl_level_desc;
        private System.Windows.Forms.Panel pnl_painel;
        private System.Windows.Forms.Label lbl_level_highscore;
        private System.Windows.Forms.PictureBox pbx_estrelas;
        private System.Windows.Forms.PictureBox pbx_level1_button;
        private System.Windows.Forms.PictureBox pbx_level2_button;
        private System.Windows.Forms.PictureBox pbx_level3_button;
    }
}
