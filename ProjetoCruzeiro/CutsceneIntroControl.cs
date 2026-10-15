using AxWMPLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjetoCruzeiro
{
    public partial class CutsceneIntroControl : UserControl
    {

        #region Variáveis Globais
        string intro = Path.Combine(Application.StartupPath, "videos", "intro2.mp4");


        #endregion

        public CutsceneIntroControl()
        {
            InitializeComponent();

            axCutsceneIntro.stretchToFit = true;
            axCutsceneIntro.uiMode = "none"; //proibe pausar com clique duplo, menu já estaria escondido
            axCutsceneIntro.enableContextMenu = false; //proibe clique direito
            axCutsceneIntro.URL = intro;
            axCutsceneIntro.Ctlcontrols.play();
            

        }

        #region Mudar de tela quando o vídeo terminar
        private void axCutsceneIntro_PlayStateChange(object sender, _WMPOCXEvents_PlayStateChangeEvent e)
        {
            if (e.newState == 8) // 8 significa que o vídeo terminou de tocar
            {
                Cruzeiro cruzeiro = (Cruzeiro)this.FindForm();

                cruzeiro.AbrirTela(new MenuControl());

            }
        }
        #endregion

        #region Botão de pular cutscene

        private void btn_pular_Click(object sender, EventArgs e)
        {

            axCutsceneIntro.Ctlcontrols.stop(); //pausa o video

            Cruzeiro cruzeiro = (Cruzeiro)this.FindForm();

            cruzeiro.AbrirTela(new MenuControl());
        }
        #endregion

        #region Lixeira
        private void CutsceneIntroControl_Load(object sender, EventArgs e)
        {

        }
        private void axCutsceneIntro_Enter(object sender, EventArgs e)
        {

        }


        #endregion

        
    }
}
