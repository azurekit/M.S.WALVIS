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
            axCutsceneIntro.uiMode = "none";
            axCutsceneIntro.URL = intro;
            axCutsceneIntro.Ctlcontrols.play();


        }

       

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
