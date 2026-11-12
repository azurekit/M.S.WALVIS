
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
            this.FormClosing += Cruzeiro_FormClosing;

            AbrirTela(new AvisoControl());
        }

        #region Funções base

        public void AbrirTela(UserControl tela)
        {
            panelPrincipal.Controls.Clear();

            tela.Dock = DockStyle.None;
            panelPrincipal.Controls.Add(tela);

            AtualizarIdentidade(tela);
        }

        private void Cruzeiro_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult resultado = MessageBox.Show(
                "Tem certeza de que deseja sair do jogo?",
                "Sair",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (resultado == DialogResult.No)
            {
                e.Cancel = true;
            }
        }

        public void AtualizarIdentidade(UserControl controle)
        {
            if (controle is DificuldadeSelectControl ||
                controle is AvisoControl ||
                controle is MenuControl)
            {
                this.Text = "Cruzeiro";
            }

            else if (controle is JogoForm)
            {
                JogoForm jogo = (JogoForm)controle;

                if (jogo.AtoAtual == 1)
                {
                    switch (jogo.NivelAtual)
                    {
                        case 1: this.Text = "Cruzeiro - Loja"; break;
                        case 2: this.Text = "Cruzeiro - Cassino"; break;
                        case 3: this.Text = "Cruzeiro - Área Restrita"; break;
                    }
                }
                else if (jogo.AtoAtual == 2)
                {
                    switch (jogo.NivelAtual)
                    {
                        case 1: this.Text = "Cruzeiro - Cozinha"; break;
                        case 2: this.Text = "Cruzeiro - Bar"; break;
                        case 3: this.Text = "Cruzeiro - Pista de Dança"; break;
                    }
                }
                else if (jogo.AtoAtual == 3)
                {
                    switch (jogo.NivelAtual)
                    {
                        case 1: this.Text = "Cruzeiro - Piscina"; break;
                        case 2: this.Text = "Cruzeiro - Teatro"; break;
                        case 3: this.Text = "Cruzeiro - Convés Externo"; break;
                    }
                }
            }

            else if (controle is Ato1Level1)
            {
                this.Text = "Cruzeiro - Loja";
            }
            else if (controle is Ato1Level2)
            {
                this.Text = "Cruzeiro - Cassino";
            }
            else if (controle is Ato1Level3)
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
            else if (controle is Ato2Level3)
            {
                this.Text = "Cruzeiro - Pista de Dança";
            }
            else if (controle is Ato3Level1)
            {
                this.Text = "Cruzeiro - Piscina";
            }
            else if (controle is Ato3Level2)
            {
                this.Text = "Cruzeiro - Teatro";
            }
            else if (controle is Ato3Level3)
            {
                this.Text = "Cruzeiro - Convés Externo";
            }

            else if (controle is LevelSelectControl &&
                ((LevelSelectControl)controle).DificuldadeSelecionada == 1)
            {
                this.Text = "Cruzeiro - Ato 1";
            }
            else if (controle is LevelSelectControl &&
                ((LevelSelectControl)controle).DificuldadeSelecionada == 2)
            {
                this.Text = "Cruzeiro - Ato 2";
            }
            else if (controle is LevelSelectControl &&
                ((LevelSelectControl)controle).DificuldadeSelecionada == 3)
            {
                this.Text = "Cruzeiro - Ato 3";
            }
        }

        #endregion

        #region Iniciar jogo
        #endregion

        #region Lixeira

        private void panelPrincipal_Paint(object sender, PaintEventArgs e)
        {

        }

        #endregion
    }
}
