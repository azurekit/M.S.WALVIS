namespace ProjetoCruzeiro
{
    partial class CutsceneControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CutsceneControl));
            this.axCutsceneIntro = new AxWMPLib.AxWindowsMediaPlayer();
            this.pbx_pular = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.axCutsceneIntro)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbx_pular)).BeginInit();
            this.SuspendLayout();
            // 
            // axCutsceneIntro
            // 
            this.axCutsceneIntro.Enabled = true;
            this.axCutsceneIntro.Location = new System.Drawing.Point(0, 0);
            this.axCutsceneIntro.Name = "axCutsceneIntro";
            this.axCutsceneIntro.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axCutsceneIntro.OcxState")));
            this.axCutsceneIntro.Size = new System.Drawing.Size(1280, 720);
            this.axCutsceneIntro.TabIndex = 0;
            this.axCutsceneIntro.PlayStateChange += new AxWMPLib._WMPOCXEvents_PlayStateChangeEventHandler(this.axCutsceneIntro_PlayStateChange);
            this.axCutsceneIntro.Enter += new System.EventHandler(this.axCutsceneIntro_Enter);
            // 
            // pbx_pular
            // 
            this.pbx_pular.BackColor = System.Drawing.Color.Transparent;
            this.pbx_pular.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pbx_pular.BackgroundImage")));
            this.pbx_pular.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pbx_pular.ErrorImage = null;
            this.pbx_pular.Location = new System.Drawing.Point(1209, 659);
            this.pbx_pular.Name = "pbx_pular";
            this.pbx_pular.Size = new System.Drawing.Size(71, 61);
            this.pbx_pular.TabIndex = 2;
            this.pbx_pular.TabStop = false;
            this.pbx_pular.Click += new System.EventHandler(this.btn_pular_Click);
            // 
            // CutsceneControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pbx_pular);
            this.Controls.Add(this.axCutsceneIntro);
            this.Name = "CutsceneControl";
            this.Size = new System.Drawing.Size(1280, 720);
            this.Load += new System.EventHandler(this.CutsceneIntroControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.axCutsceneIntro)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbx_pular)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private AxWMPLib.AxWindowsMediaPlayer axCutsceneIntro;
        private System.Windows.Forms.PictureBox pbx_pular;
    }
}
