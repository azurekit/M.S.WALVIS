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
    public partial class MenuControl : UserControl
    {
        public MenuControl()
        {
            InitializeComponent();
        }

        #region Botões

        private void btn_iniciar_Click(object sender, EventArgs e)
        {
            Cruzeiro cruzeiro = (Cruzeiro)this.FindForm();

            cruzeiro.AbrirTela(new DificuldadeSelectControl());
        }

        private void btn_sair_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        #endregion

        #region Lixeira

        private void button1_Click(object sender, EventArgs e)
        {

        }

        #endregion

       
    }
}
