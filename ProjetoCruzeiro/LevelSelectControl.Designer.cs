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
            this.btn_menu = new System.Windows.Forms.Button();
            this.lbl_caso = new System.Windows.Forms.Label();
            this.lbl_level_titulo = new System.Windows.Forms.Label();
            this.lbl_level_desc = new System.Windows.Forms.Label();
            this.lbl_level1_button = new System.Windows.Forms.Label();
            this.lbl_level2_button = new System.Windows.Forms.Label();
            this.lbl_level3_button = new System.Windows.Forms.Label();
            this.pbx_level_estrela1 = new System.Windows.Forms.PictureBox();
            this.pbx_level_estrela2 = new System.Windows.Forms.PictureBox();
            this.pbx_level_estrela3 = new System.Windows.Forms.PictureBox();
            this.lbl_level_local = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lbl_level_highscore = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pbx_level_estrela1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbx_level_estrela2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbx_level_estrela3)).BeginInit();
            this.panel1.SuspendLayout();
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
            this.lbl_caso.Location = new System.Drawing.Point(20, 86);
            this.lbl_caso.Name = "lbl_caso";
            this.lbl_caso.Size = new System.Drawing.Size(249, 119);
            this.lbl_caso.TabIndex = 30;
            this.lbl_caso.Text = "ATO 1";
            this.lbl_caso.Click += new System.EventHandler(this.lbl_caso_Click);
            // 
            // lbl_level_titulo
            // 
            this.lbl_level_titulo.AutoSize = true;
            this.lbl_level_titulo.BackColor = System.Drawing.Color.Transparent;
            this.lbl_level_titulo.Font = new System.Drawing.Font("League Spartan ExtraBold", 50F);
            this.lbl_level_titulo.ForeColor = System.Drawing.SystemColors.Control;
            this.lbl_level_titulo.Location = new System.Drawing.Point(11, 18);
            this.lbl_level_titulo.Name = "lbl_level_titulo";
            this.lbl_level_titulo.Size = new System.Drawing.Size(326, 100);
            this.lbl_level_titulo.TabIndex = 32;
            this.lbl_level_titulo.Text = "\"O Furto\"";
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
            // lbl_level1_button
            // 
            this.lbl_level1_button.BackColor = System.Drawing.Color.Transparent;
            this.lbl_level1_button.Font = new System.Drawing.Font("League Spartan ExtraBold", 45F);
            this.lbl_level1_button.ForeColor = System.Drawing.SystemColors.Control;
            this.lbl_level1_button.Image = global::ProjetoCruzeiro.Properties.Resources.botaojogarmanha_Photoroom;
            this.lbl_level1_button.Location = new System.Drawing.Point(25, 222);
            this.lbl_level1_button.Name = "lbl_level1_button";
            this.lbl_level1_button.Size = new System.Drawing.Size(300, 80);
            this.lbl_level1_button.TabIndex = 40;
            this.lbl_level1_button.MouseLeave += new System.EventHandler(this.lbl_level1_button_MouseLeave);
            this.lbl_level1_button.MouseHover += new System.EventHandler(this.lbl_level1_button_MouseHover);
            // 
            // lbl_level2_button
            // 
            this.lbl_level2_button.BackColor = System.Drawing.Color.Transparent;
            this.lbl_level2_button.Font = new System.Drawing.Font("League Spartan ExtraBold", 45F);
            this.lbl_level2_button.ForeColor = System.Drawing.SystemColors.Control;
            this.lbl_level2_button.Image = global::ProjetoCruzeiro.Properties.Resources.botaojogartarde_Photoroom;
            this.lbl_level2_button.Location = new System.Drawing.Point(25, 330);
            this.lbl_level2_button.Name = "lbl_level2_button";
            this.lbl_level2_button.Size = new System.Drawing.Size(300, 80);
            this.lbl_level2_button.TabIndex = 41;
            // 
            // lbl_level3_button
            // 
            this.lbl_level3_button.BackColor = System.Drawing.Color.Transparent;
            this.lbl_level3_button.Font = new System.Drawing.Font("League Spartan ExtraBold", 45F);
            this.lbl_level3_button.ForeColor = System.Drawing.SystemColors.Control;
            this.lbl_level3_button.Image = global::ProjetoCruzeiro.Properties.Resources.botaojogarnoitepng_Photoroom;
            this.lbl_level3_button.Location = new System.Drawing.Point(25, 435);
            this.lbl_level3_button.Name = "lbl_level3_button";
            this.lbl_level3_button.Size = new System.Drawing.Size(300, 80);
            this.lbl_level3_button.TabIndex = 42;
            this.lbl_level3_button.Click += new System.EventHandler(this.lbl_level3_button_Click);
            // 
            // pbx_level_estrela1
            // 
            this.pbx_level_estrela1.BackColor = System.Drawing.Color.Transparent;
            this.pbx_level_estrela1.BackgroundImage = global::ProjetoCruzeiro.Properties.Resources.estrelavazia;
            this.pbx_level_estrela1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pbx_level_estrela1.Location = new System.Drawing.Point(69, 154);
            this.pbx_level_estrela1.Name = "pbx_level_estrela1";
            this.pbx_level_estrela1.Size = new System.Drawing.Size(45, 45);
            this.pbx_level_estrela1.TabIndex = 43;
            this.pbx_level_estrela1.TabStop = false;
            // 
            // pbx_level_estrela2
            // 
            this.pbx_level_estrela2.BackColor = System.Drawing.Color.Transparent;
            this.pbx_level_estrela2.BackgroundImage = global::ProjetoCruzeiro.Properties.Resources.estrelavazia;
            this.pbx_level_estrela2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pbx_level_estrela2.Location = new System.Drawing.Point(142, 170);
            this.pbx_level_estrela2.Name = "pbx_level_estrela2";
            this.pbx_level_estrela2.Size = new System.Drawing.Size(45, 45);
            this.pbx_level_estrela2.TabIndex = 44;
            this.pbx_level_estrela2.TabStop = false;
            // 
            // pbx_level_estrela3
            // 
            this.pbx_level_estrela3.BackColor = System.Drawing.Color.Transparent;
            this.pbx_level_estrela3.BackgroundImage = global::ProjetoCruzeiro.Properties.Resources.estrelavazia;
            this.pbx_level_estrela3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pbx_level_estrela3.Location = new System.Drawing.Point(213, 154);
            this.pbx_level_estrela3.Name = "pbx_level_estrela3";
            this.pbx_level_estrela3.Size = new System.Drawing.Size(45, 45);
            this.pbx_level_estrela3.TabIndex = 45;
            this.pbx_level_estrela3.TabStop = false;
            // 
            // lbl_level_local
            // 
            this.lbl_level_local.AutoSize = true;
            this.lbl_level_local.BackColor = System.Drawing.Color.Transparent;
            this.lbl_level_local.Font = new System.Drawing.Font("League Spartan ExtraBold", 25F);
            this.lbl_level_local.ForeColor = System.Drawing.SystemColors.Control;
            this.lbl_level_local.Location = new System.Drawing.Point(120, 102);
            this.lbl_level_local.Name = "lbl_level_local";
            this.lbl_level_local.Size = new System.Drawing.Size(87, 50);
            this.lbl_level_local.TabIndex = 52;
            this.lbl_level_local.Text = "Loja";
            this.lbl_level_local.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Controls.Add(this.lbl_level_highscore);
            this.panel1.Controls.Add(this.lbl_level_titulo);
            this.panel1.Controls.Add(this.lbl_level_local);
            this.panel1.Controls.Add(this.lbl_level_desc);
            this.panel1.Controls.Add(this.pbx_level_estrela3);
            this.panel1.Controls.Add(this.pbx_level_estrela2);
            this.panel1.Controls.Add(this.pbx_level_estrela1);
            this.panel1.Location = new System.Drawing.Point(878, 365);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(340, 352);
            this.panel1.TabIndex = 53;
            // 
            // lbl_level_highscore
            // 
            this.lbl_level_highscore.AutoSize = true;
            this.lbl_level_highscore.BackColor = System.Drawing.Color.Transparent;
            this.lbl_level_highscore.Font = new System.Drawing.Font("League Spartan SemiBold", 17.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_level_highscore.ForeColor = System.Drawing.SystemColors.Control;
            this.lbl_level_highscore.Location = new System.Drawing.Point(100, 228);
            this.lbl_level_highscore.Name = "lbl_level_highscore";
            this.lbl_level_highscore.Size = new System.Drawing.Size(133, 35);
            this.lbl_level_highscore.TabIndex = 53;
            this.lbl_level_highscore.Text = "*Highscore*";
            this.lbl_level_highscore.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LevelSelectControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Chartreuse;
            this.BackgroundImage = global::ProjetoCruzeiro.Properties.Resources.altomar;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lbl_level3_button);
            this.Controls.Add(this.lbl_level2_button);
            this.Controls.Add(this.lbl_level1_button);
            this.Controls.Add(this.lbl_caso);
            this.Controls.Add(this.btn_menu);
            this.DoubleBuffered = true;
            this.Name = "LevelSelectControl";
            this.Size = new System.Drawing.Size(1280, 720);
            ((System.ComponentModel.ISupportInitialize)(this.pbx_level_estrela1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbx_level_estrela2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbx_level_estrela3)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_menu;
        private System.Windows.Forms.Label lbl_caso;
        private System.Windows.Forms.Label lbl_level_desc;
        private System.Windows.Forms.Label lbl_level1_button;
        private System.Windows.Forms.Label lbl_level2_button;
        private System.Windows.Forms.Label lbl_level3_button;
        private System.Windows.Forms.PictureBox pbx_level_estrela1;
        private System.Windows.Forms.PictureBox pbx_level_estrela2;
        private System.Windows.Forms.PictureBox pbx_level_estrela3;
        private System.Windows.Forms.Label lbl_level_titulo;
        private System.Windows.Forms.Label lbl_level_local;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lbl_level_highscore;
    }
}
