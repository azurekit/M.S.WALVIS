
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
        private Timer timerVerificacao;
        private Timer timerExpressoes;
        private Random random = new Random();
        private string pastaExpressoes;

        #endregion

        #region Inicialização

        public MenuLateralControl()
        {
            InitializeComponent();

            pastaExpressoes = Path.Combine(
                Application.StartupPath,
                "imagens",
                "MenuLateral",
                "Coala"
            );

            pbx_espelho.BackgroundImageLayout = ImageLayout.Stretch;

            efeitos.Add(new EfeitoBotao(pbx_voltarmenu));
            efeitos.Add(new EfeitoBotao(pbx_mute));

            pbx_voltarmenu.Cursor = Cursors.Hand;
            pbx_mute.Cursor = Cursors.Hand;

            pbx_voltarmenu.Click -= pbx_voltarmenu_Click;
            pbx_voltarmenu.Click += pbx_voltarmenu_Click;

            VisibleChanged += MenuLateralControl_VisibleChanged;

            InicializarTimerExpressoes();
            IniciarMonitoramento();
        }

        #endregion

        #region Definir nível

        public void DefinirNivel(int ato, int nivel)
        {
            AtoAtual = ato;
            NivelAtual = nivel;

            Identificador = ((ato - 1) * 3) + nivel;

            CarregarBackground();
            AtualizarInstrucoes();

            // Carrega imediatamente a skin atualmente salva.
            AtualizarExpressao();
        }

        #endregion

        #region Expressões do personagem

        private void InicializarTimerExpressoes()
        {
            timerExpressoes = new Timer();
            timerExpressoes.Interval = 15000;

            timerExpressoes.Tick += (sender, e) =>
            {
                AtualizarExpressao();
            };

            AtualizarEstadoTimerExpressoes();
        }

        private void MenuLateralControl_VisibleChanged(
            object sender,
            EventArgs e)
        {
            AtualizarEstadoTimerExpressoes();
        }

        private void AtualizarEstadoTimerExpressoes()
        {
            if (timerExpressoes == null)
                return;

            if (Visible)
            {
                AtualizarExpressao();

                if (!timerExpressoes.Enabled)
                    timerExpressoes.Start();
            }
            else
            {
                timerExpressoes.Stop();
            }
        }

        private void AtualizarExpressao()
        {
            int skin = ObterSkinEquipada();
            int expressao = random.Next(1, 9);

            string caminho = Path.Combine(
                pastaExpressoes,
                "item" + skin,
                "expressao" + expressao + "-" + skin + ".png"
            );

            if (!File.Exists(caminho))
                return;

            Image imagem;

            using (Image original = Image.FromFile(caminho))
            {
                imagem = new Bitmap(original);
            }

            if (pbx_espelho.BackgroundImage != null)
            {
                Image anterior = pbx_espelho.BackgroundImage;
                pbx_espelho.BackgroundImage = null;
                anterior.Dispose();
            }

            pbx_espelho.BackgroundImage = imagem;
            pbx_espelho.BackgroundImageLayout = ImageLayout.Stretch;
        }

        private int ObterSkinEquipada()
        {
            string equipada = SaveManager.ObterCosmeticoEquipado();

            int skin;

            if (int.TryParse(equipada, out skin))
            {
                if (skin >= 1 && skin <= 9)
                    return skin;
            }

            return 0;
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

        public bool EstaBloqueado()
        {
            return voltarBloqueado;
        }

        #endregion

        #region Monitoramento do controle ativo

        private void IniciarMonitoramento()
        {
            timerVerificacao = new Timer();
            timerVerificacao.Interval = 100;

            timerVerificacao.Tick += (sender, e) =>
            {
                VerificarControleAtivo();
            };

            timerVerificacao.Start();
        }

        private void VerificarControleAtivo()
        {
            Control controle = this;

            while (controle != null && !(controle is JogoForm))
            {
                controle = controle.Parent;
            }

            JogoForm jogo = controle as JogoForm;

            if (jogo == null)
                return;

            bool bloquear = ControlePossuiVitoriaOuDerrota(jogo);

            if (bloquear)
            {
                if (!voltarBloqueado)
                    BloquearVoltarMenu();
            }
            else
            {
                if (voltarBloqueado)
                    PermitirVoltarMenu();
            }
        }

        private bool ControlePossuiVitoriaOuDerrota(Control controle)
        {
            foreach (Control filho in controle.Controls)
            {
                if (!filho.Visible)
                    continue;

                if (filho is VitoriaControl || filho is DerrotaControl)
                    return true;

                if (filho.HasChildren)
                {
                    if (ControlePossuiVitoriaOuDerrota(filho))
                        return true;
                }
            }

            return false;
        }

        #endregion

        #region Carregar imagens

        private void CarregarBackground()
        {
            string caminho = Path.Combine(
                Application.StartupPath,
                "imagens",
                "MenuLateral",
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
                lbl_instrucao1.Text = "Concerte os canos conectando-os!";
                lbl_instrucao2.Text = "Clique nos canos para virá-los!";
                lbl_instrucao3.Text = "Cuidado com o limite de movimentos!";
            }

            if (Identificador == 4)
            {
                lbl_instrucao1.Text = "Ordene os pratos corretamente na pilha da direita!";
                lbl_instrucao2.Text = "Clique nos pratos para selecioná-los e movê-los!";
                lbl_instrucao3.Text = "Faça no menor tempo e movimentos possíveis!";
            }

            if (Identificador == 5)
            {
                lbl_instrucao1.Text = "Bola branca = Algum ingrediente certo na posição errada";
                lbl_instrucao2.Text = "Bola preta = Algum ingrediente certo na posição correta";
                lbl_instrucao3.Text = "Faça a combinação correta de ingredientes!";
            }

            if (Identificador == 6)
            {
                lbl_instrucao1.Text = "Clique nos painéis para acender as luzes!";
                lbl_instrucao2.Text = "Acenda todas as luzes!";
                lbl_instrucao3.Text = "Derrote Zelo!";
            }

            if (Identificador == 7)
            {
                lbl_instrucao1.Text = "Clique nas válvulas para mudar a pressão!";
                lbl_instrucao2.Text = "Atinja as metas de cada painel!";
                lbl_instrucao3.Text = "Faça no menor tempo e movimentos possíveis!";
            }

            if (Identificador == 8)
            {
                lbl_instrucao1.Text = "Organize corretamente o roteiro!";
                lbl_instrucao2.Text = "Arraste os cartões para ordená-los!";
                lbl_instrucao3.Text = "Faça no menor tempo possível!";
            }

            if (Identificador == 9)
            {
                lbl_instrucao1.Text = "Responda as perguntas corretamente!";
                lbl_instrucao2.Text = "Clique no cartão para selecioná-lo!";
                lbl_instrucao3.Text = "Derrote Gio!";
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
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2
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
