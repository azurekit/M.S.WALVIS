
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace ProjetoCruzeiro
{
    public partial class MenuLateralControl : UserControl
    {
        #region Variáveis Globais

        public int AtoAtual { get; private set; }
        public int NivelAtual { get; private set; }
        public int Identificador { get; private set; }

        private List<EfeitoBotao> efeitos = new List<EfeitoBotao>();

        private bool voltarBloqueado;

        #endregion

        public MenuLateralControl()
        {
            InitializeComponent();

            #region Efeitos dos botões

            efeitos.Add(new EfeitoBotao(pbx_voltarmenu));
            efeitos.Add(new EfeitoBotao(pbx_mute));

            pbx_voltarmenu.Cursor = Cursors.Hand;
            pbx_mute.Cursor = Cursors.Hand;

            #endregion

            #region Eventos

            pbx_voltarmenu.Click -= pbx_voltarmenu_Click;
            pbx_voltarmenu.Click += pbx_voltarmenu_Click;

            #endregion
        }

        #region Definir nível

        public void DefinirNivel(int ato, int nivel)
        {
            AtoAtual = ato;
            NivelAtual = nivel;

            Identificador = ((ato - 1) * 3) + nivel;

            PermitirVoltarMenu();

            CarregarBackground();
            AtualizarInstrucoes();
        }

        #endregion

        #region Estado do botão voltar

        public void BloquearVoltarMenu()
        {
            voltarBloqueado = true;

            pbx_voltarmenu.Enabled = false;
            pbx_voltarmenu.Cursor = Cursors.No;
        }

        public void PermitirVoltarMenu()
        {
            voltarBloqueado = false;

            pbx_voltarmenu.Enabled = true;
            pbx_voltarmenu.Cursor = Cursors.Hand;
        }

        #endregion

        #region Carregar imagens

        private void CarregarBackground()
        {
            string caminho = Path.Combine(
                Application.StartupPath,
                "imagens",
                "menulateral" + Identificador + ".png"
            );

            if (!File.Exists(caminho))
            {
                MessageBox.Show(
                    "Não foi possível encontrar a imagem:\n" + caminho,
                    "Erro ao carregar menu",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );

                return;
            }

            Image imagem;

            using (Image original = Image.FromFile(caminho))
            {
                imagem = new Bitmap(original);
            }

            if (BackgroundImage != null)
                BackgroundImage.Dispose();

            BackgroundImage = imagem;
            BackgroundImageLayout = ImageLayout.Stretch;
        }

        #endregion

        #region Instruções

        private void AtualizarInstrucoes()
        {
            if (Identificador == 1)
            {
                lbl_instrucao1.Text = "Ordene os monitores corretamente para religar o sistema!";
                lbl_instrucao2.Text = "Clique nos monitores para trocarem de posição!";
                lbl_instrucao3.Text = "Faça no menor tempo possível!";
            }

            if (Identificador == 2)
            {
                lbl_instrucao1.Text = "Desvende os valores dos dados!";
                lbl_instrucao2.Text = "Clique nas caixas de entrada para selecionar o valor!";
                lbl_instrucao3.Text = "Faça no menor tempo possível!";
            }

            if (Identificador == 3)
            {
                lbl_instrucao1.Text = "Conecte os canos!";
                lbl_instrucao2.Text = "Clique nos canos para virá-los!";
                lbl_instrucao3.Text = "Cuidado com o limite de movimentos!";
            }

            if (Identificador == 4)
            {
                lbl_instrucao1.Text = "Ordene os pratos corretamente na pilha da direita!";
                lbl_instrucao2.Text = "Clique nos pratos para seleciona-los e movê-los!";
                lbl_instrucao3.Text = "Faça no menor tempo e movimentos possíveis!";
            }
        }

        #endregion

        #region Botão voltar ao menu

        private void pbx_voltarmenu_Click(object sender, EventArgs e)
        {
            if (voltarBloqueado || !pbx_voltarmenu.Enabled)
                return;

            DialogResult resposta = MessageBox.Show(
                "Deseja realmente voltar ao menu?",
                "Voltar ao menu",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (resposta == DialogResult.Yes)
            {
                Cruzeiro cruzeiro = FindForm() as Cruzeiro;

                if (cruzeiro != null)
                {
                    cruzeiro.AbrirTela(
                        new LevelSelectControl(AtoAtual)
                    );
                }
            }
        }

        #endregion
    }
}
