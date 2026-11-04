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
            this.btn_pular = new System.Windows.Forms.Button();
            this.axCutsceneIntro = new AxWMPLib.AxWindowsMediaPlayer();
            ((System.ComponentModel.ISupportInitialize)(this.axCutsceneIntro)).BeginInit();
            this.SuspendLayout();
            // 
            // btn_pular
            // 
            this.btn_pular.BackColor = System.Drawing.Color.Silver;
            this.btn_pular.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_pular.Font = new System.Drawing.Font("Microsoft JhengHei UI", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_pular.Location = new System.Drawing.Point(1038, 513);
            this.btn_pular.Name = "btn_pular";
            this.btn_pular.Size = new System.Drawing.Size(242, 59);
            this.btn_pular.TabIndex = 1;
            this.btn_pular.Text = "Pular";
            this.btn_pular.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btn_pular.UseVisualStyleBackColor = false;
            this.btn_pular.Click += new System.EventHandler(this.btn_pular_Click);
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
            // CutsceneIntroControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btn_pular);
            this.Controls.Add(this.axCutsceneIntro);
            this.Name = "CutsceneIntroControl";
            this.Size = new System.Drawing.Size(1280, 720);
            this.Load += new System.EventHandler(this.CutsceneIntroControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.axCutsceneIntro)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private AxWMPLib.AxWindowsMediaPlayer axCutsceneIntro;
        private System.Windows.Forms.Button btn_pular;
    }
}
