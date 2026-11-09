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
            this.flpFalas = new System.Windows.Forms.FlowLayoutPanel();
            this.flpResposta = new System.Windows.Forms.FlowLayoutPanel();
            this.lblNomePeca = new System.Windows.Forms.Label();
            this.btnConfirmar = new System.Windows.Forms.Button();
            this.btnReiniciar = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // flpFalas
            // 
            this.flpFalas.AllowDrop = true;
            this.flpFalas.AutoScroll = true;
            this.flpFalas.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.flpFalas.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flpFalas.Location = new System.Drawing.Point(15, 48);
            this.flpFalas.Name = "flpFalas";
            this.flpFalas.Size = new System.Drawing.Size(443, 651);
            this.flpFalas.TabIndex = 0;
            // 
            // flpResposta
            // 
            this.flpResposta.AllowDrop = true;
            this.flpResposta.AutoScroll = true;
            this.flpResposta.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.flpResposta.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flpResposta.Location = new System.Drawing.Point(474, 48);
            this.flpResposta.Name = "flpResposta";
            this.flpResposta.Size = new System.Drawing.Size(443, 651);
            this.flpResposta.TabIndex = 1;
            // 
            // lblNomePeca
            // 
            this.lblNomePeca.AutoSize = true;
            this.lblNomePeca.Location = new System.Drawing.Point(495, 19);
            this.lblNomePeca.Name = "lblNomePeca";
            this.lblNomePeca.Size = new System.Drawing.Size(35, 13);
            this.lblNomePeca.TabIndex = 0;
            this.lblNomePeca.Text = "label1";
            // 
            // btnConfirmar
            // 
            this.btnConfirmar.Location = new System.Drawing.Point(775, 8);
            this.btnConfirmar.Name = "btnConfirmar";
            this.btnConfirmar.Size = new System.Drawing.Size(142, 34);
            this.btnConfirmar.TabIndex = 3;
            this.btnConfirmar.Text = "button1";
            this.btnConfirmar.UseVisualStyleBackColor = true;
            // 
            // btnReiniciar
            // 
            this.btnReiniciar.Location = new System.Drawing.Point(316, 3);
            this.btnReiniciar.Name = "btnReiniciar";
            this.btnReiniciar.Size = new System.Drawing.Size(142, 34);
            this.btnReiniciar.TabIndex = 4;
            this.btnReiniciar.Text = "button1";
            this.btnReiniciar.UseVisualStyleBackColor = true;
            // 
            // Ato3Level2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnReiniciar);
            this.Controls.Add(this.btnConfirmar);
            this.Controls.Add(this.lblNomePeca);
            this.Controls.Add(this.flpResposta);
            this.Controls.Add(this.flpFalas);
            this.Name = "Ato3Level2";
            this.Size = new System.Drawing.Size(960, 720);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flpFalas;
        private System.Windows.Forms.FlowLayoutPanel flpResposta;
        private System.Windows.Forms.Label lblNomePeca;
        private System.Windows.Forms.Button btnConfirmar;
        private System.Windows.Forms.Button btnReiniciar;
    }
}
