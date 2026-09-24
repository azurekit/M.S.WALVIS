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
    public partial class Cruzeiro : Form
    {
        public Cruzeiro()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            AbrirTela(new MenuControl());
        }

        #region Funções base

        // Trocar telas
        public void AbrirTela(UserControl tela)
        {
            panelPrincipal.Controls.Clear();
            tela.Dock = DockStyle.Fill;
            panelPrincipal.Controls.Add(tela);
        }

        #endregion

        #region Iniciar jogo
        #endregion

        #region lixeira
        private void panelPrincipal_Paint(object sender, PaintEventArgs e)
        {

        }
        #endregion
    }
}
