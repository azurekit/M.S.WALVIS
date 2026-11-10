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
            AbrirTela(new AvisoControl());
        }

        #region Funções base

        // Trocar telas
        public void AbrirTela(UserControl tela)
        {
            panelPrincipal.Controls.Clear();
            tela.Dock = DockStyle.None;
            panelPrincipal.Controls.Add(tela);
            AtualizarIdentidade(tela);
        }

        public void AtualizarIdentidade(UserControl controle)
        {
            if (controle is DificuldadeSelectControl || controle is AvisoControl || controle is MenuControl)
            {
                this.Text = "Cruzeiro";

            }

            else if (controle is LevelSelectControl && ((LevelSelectControl)controle).DificuldadeSelecionada == 1)
            {
                this.Text = "Cruzeiro - Ato 1";
            }
            else if (controle is LevelSelectControl && ((LevelSelectControl)controle).DificuldadeSelecionada == 2)
            {
                this.Text = "Cruzeiro - Ato 2";
            }
            else if (controle is LevelSelectControl && ((LevelSelectControl)controle).DificuldadeSelecionada == 3)
            {
                this.Text = "Cruzeiro - Ato 3";
            }

            else if (controle is Ato1Level1)
            {
                this.Text = "Cruzeiro - Loja";
            }
            else if (controle is Ato1Level2)
            {
                this.Text = "Cruzeiro - Cassino";
            }
            else if (controle is Ato2Level3)
            {
                this.Text = "Cruzeiro - Área Restrita";
            }
            else if (controle is Ato2Level1)
            {
                this.Text = "Cruzeiro - Cozinha";
            }
            else if (controle is Ato2Level2)
            {
                this.Text = "Cruzeiro - Bar";
            }
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
