using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace ProjetoCruzeiro
{
    public partial class Cruzeiro : Form
    {
        #region Construtor

        public Cruzeiro()
        {
            InitializeComponent();

            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.FormClosing += Cruzeiro_FormClosing;

            AbrirTela(new AvisoControl());
        }

        #endregion

        #region Navegação entre Telas

        public void AbrirTela(UserControl tela)
        {
            panelPrincipal.Controls.Clear();

            tela.Dock = DockStyle.None;
            panelPrincipal.Controls.Add(tela);

            AtualizarIdentidade(tela);
        }

        #endregion

        #region Identidade da Janela

        public void AtualizarIdentidade(UserControl controle)
        {
            this.Icon = new Icon(
                Path.Combine(Application.StartupPath, "imagens", "iconecruzeiro.ico")
            );

            if (controle is DificuldadeSelectControl ||
                controle is AvisoControl ||
                controle is MenuControl)
            {
                this.Text = "M.S. Walvis";
            }

            else if (controle is JogoForm)
            {
                JogoForm jogo = (JogoForm)controle;

                if (jogo.AtoAtual == 1)
                {
                    switch (jogo.NivelAtual)
                    {
                        case 1: this.Text = "M.S. Walvis - Loja"; break;
                        case 2: this.Text = "M.S. Walvis - Cassino"; break;
                        case 3: this.Text = "M.S. Walvis - Área Restrita"; break;
                    }
                }
                else if (jogo.AtoAtual == 2)
                {
                    switch (jogo.NivelAtual)
                    {
                        case 1: this.Text = "M.S. Walvis - Cozinha"; break;
                        case 2: this.Text = "M.S. Walvis - Bar"; break;
                        case 3: this.Text = "M.S. Walvis - Pista de Dança"; break;
                    }
                }
                else if (jogo.AtoAtual == 3)
                {
                    switch (jogo.NivelAtual)
                    {
                        case 1: this.Text = "M.S. Walvis - Piscina"; break;
                        case 2: this.Text = "M.S. Walvis - Teatro"; break;
                        case 3: this.Text = "M.S. Walvis - Convés Externo"; break;
                    }
                }
            }
            else if (controle is LojaControl)
            {
                this.Text = "M.S. Walvis - Loja";
            }
            else if (controle is Ato1Level1)
            {
                this.Text = "M.S. Walvis - Loja";
            }
            else if (controle is Ato1Level2)
            {
                this.Text = "M.S. Walvis - Cassino";
            }
            else if (controle is Ato1Level3)
            {
                this.Text = "M.S. Walvis - Área Restrita";
            }
            else if (controle is Ato2Level1)
            {
                this.Text = "M.S. Walvis - Cozinha";
            }
            else if (controle is Ato2Level2)
            {
                this.Text = "M.S. Walvis - Bar";
            }
            else if (controle is Ato2Level3)
            {
                this.Text = "M.S. Walvis - Pista de Dança";
            }
            else if (controle is Ato3Level1)
            {
                this.Text = "M.S. Walvis - Piscina";
            }
            else if (controle is Ato3Level2)
            {
                this.Text = "M.S. Walvis - Teatro";
            }
            else if (controle is Ato3Level3)
            {
                this.Text = "M.S. Walvis - Convés Externo";
            }

            else if (controle is LevelSelectControl &&
                ((LevelSelectControl)controle).DificuldadeSelecionada == 1)
            {
                this.Text = "M.S. Walvis - Ato 1";
            }
            else if (controle is LevelSelectControl &&
                ((LevelSelectControl)controle).DificuldadeSelecionada == 2)
            {
                this.Text = "M.S. Walvis - Ato 2";
            }
            else if (controle is LevelSelectControl &&
                ((LevelSelectControl)controle).DificuldadeSelecionada == 3)
            {
                this.Text = "M.S. Walvis - Ato 3";
            }
        }

        #endregion

        #region Encerramento do Jogo

        private void Cruzeiro_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult resultado = MessageBox.Show(
                "Tem certeza de que deseja sair do jogo?",
                "Sair",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2
            );

            if (resultado == DialogResult.No)
            {
                e.Cancel = true;
            }
        }

        #endregion

        #region Lixeira

        private void panelPrincipal_Paint(object sender, PaintEventArgs e)
        {

        }

        #endregion
    }
}