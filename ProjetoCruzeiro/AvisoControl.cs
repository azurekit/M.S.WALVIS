using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjetoCruzeiro
{
    public partial class AvisoControl : UserControl
    {
        public AvisoControl()
        {
            InitializeComponent();
            timerAviso.Start();
        }

        #region Timer
        private void timerAviso_Tick_1(object sender, EventArgs e)
        {
            Cruzeiro cruzeiro = (Cruzeiro)this.FindForm();
            cruzeiro.AbrirTela(new MenuControl());
            timerAviso.Stop();
        }

        #endregion

        #region Lixeira
        private void lbl_aviso2_Click(object sender, EventArgs e)
        {

        }

        private void timerAviso_Tick(object sender, EventArgs e)
        {

        }


        #endregion

     
    }
}
