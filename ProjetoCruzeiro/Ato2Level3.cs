using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace ProjetoCruzeiro
{
    public partial class Ato2Level3 : UserControl
    {
        #region Variáveis Globais

        private bool[,] tabuleiro = new bool[3, 3];

        private Random random = new Random();

        private int movimentos = 0;
        private int round = 1;
        private int resets = 3;

        private bool finalizando = false;

        private List<EfeitoBotao> efeitos = new List<EfeitoBotao>();

        private Stopwatch cronometro = new Stopwatch();
        private Stopwatch cronometroAnimacao = new Stopwatch();

        private Timer timerTrocaFundo = new Timer();
        private Timer timerAnimacaoPainel = new Timer();
        private Timer timerEsperaVitoria = new Timer();

        private int indiceFundo = 1;

        private int painelTopInicial;
        private int painelTopFinal;

        private const int IDENTIFICADOR_NIVEL = 6;

        private string CaminhoImagens
        {
            get
            {
                return Path.Combine(
                    Application.StartupPath,
                    "imagens",
                    "Level6"
                );
            }
        }

        #endregion

        #region Construtor

        public Ato2Level3()
        {
            InitializeComponent();

            ConfigurarDoubleBuffering();
            OrganizarCamadas();

            round = 1;
            movimentos = 0;
            resets = 3;
            finalizando = false;

            ConfigurarInterface();
            ConfigurarImagensIniciais();
            AssociarEventos();
            ConfigurarEfeitosBotoes();
            ConfigurarTimers();

            cronometro.Start();

            IniciarJogo();
        }

        #endregion

        #region Configuração inicial

        private void ConfigurarInterface()
        {
            lbl_resets2.Text = resets.ToString();
            lbl_resets2.ForeColor = Color.White;

            pbx_resetar.Enabled = true;
            pbx_resetar.Cursor = Cursors.Hand;
        }

        private void ConfigurarImagensIniciais()
        {
            DefinirImagem(this, "pista1.png");
            DefinirImagem(pbx_zelo, "zelo1.png");

            DefinirImagem(pbx_luz1, "luz_pista1.png");
            DefinirImagem(pbx_luz2, "luz_pista1.png");
        }

        private void ConfigurarEfeitosBotoes()
        {
            efeitos.Add(new EfeitoBotao(pbx_resetar));
        }

        private void ConfigurarTimers()
        {
            timerTrocaFundo.Interval = 2000;
            timerTrocaFundo.Tick += TimerTrocaFundo_Tick;
            timerTrocaFundo.Start();

            timerAnimacaoPainel.Interval = 15;
            timerAnimacaoPainel.Tick += TimerAnimacaoPainel_Tick;

            timerEsperaVitoria.Interval = 3000;
            timerEsperaVitoria.Tick += TimerEsperaVitoria_Tick;
        }

        #endregion

        #region Hierarquia e Double Buffering

        private void OrganizarCamadas()
        {
            Point posicaoTela = pnl_painel.Parent.PointToScreen(
                pnl_painel.Location
            );

            if (pnl_painel.Parent != pbx_zelo)
            {
                pnl_painel.Parent = pbx_zelo;
            }

            pnl_painel.Location = pbx_zelo.PointToClient(posicaoTela);

            pnl_painel.BackColor = Color.Transparent;

            pbx_zelo.BringToFront();
            pnl_painel.BringToFront();

            foreach (Control controle in pnl_painel.Controls)
            {
                controle.BringToFront();
            }
        }

        private void ConfigurarDoubleBuffering()
        {
            SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw,
                true
            );

            AtivarDoubleBuffering(this);
        }

        private void AtivarDoubleBuffering(Control controle)
        {
            try
            {
                PropertyInfo propriedade =
                    typeof(Control).GetProperty(
                        "DoubleBuffered",
                        BindingFlags.Instance | BindingFlags.NonPublic
                    );

                if (propriedade != null)
                {
                    propriedade.SetValue(controle, true, null);
                }
            }
            catch
            {
            }

            foreach (Control filho in controle.Controls)
            {
                AtivarDoubleBuffering(filho);
            }
        }

        #endregion

        #region Associação dos eventos

        private void AssociarEventos()
        {
            btn_00.Click -= btn_00_Click;
            btn_00.Click += btn_00_Click;

            btn_01.Click -= btn_01_Click;
            btn_01.Click += btn_01_Click;

            btn_02.Click -= btn_02_Click;
            btn_02.Click += btn_02_Click;

            btn_10.Click -= btn_10_Click;
            btn_10.Click += btn_10_Click;

            btn_11.Click -= btn_11_Click;
            btn_11.Click += btn_11_Click;

            btn_12.Click -= btn_12_Click;
            btn_12.Click += btn_12_Click;

            btn_20.Click -= btn_20_Click;
            btn_20.Click += btn_20_Click;

            btn_21.Click -= btn_21_Click;
            btn_21.Click += btn_21_Click;

            btn_22.Click -= btn_22_Click;
            btn_22.Click += btn_22_Click;

            pbx_resetar.Click -= btn_resetar_Click;
            pbx_resetar.Click += btn_resetar_Click;
        }

        #endregion

        #region Carregamento de imagens

        private Image CarregarImagem(string nome)
        {
            string caminho = Path.Combine(CaminhoImagens, nome);

            if (!File.Exists(caminho))
                return null;

            using (Image imagem = Image.FromFile(caminho))
            {
                return new Bitmap(imagem);
            }
        }

        private void DefinirImagem(Control controle, string nome)
        {
            Image imagem = CarregarImagem(nome);

            if (imagem == null)
                return;

            Image anterior = controle.BackgroundImage;

            controle.BackgroundImage = imagem;

            if (anterior != null)
                anterior.Dispose();
        }

        #endregion

        #region Alternância do fundo

        private void TimerTrocaFundo_Tick(object sender, EventArgs e)
        {
            if (finalizando)
                return;

            indiceFundo++;

            if (indiceFundo > 4)
                indiceFundo = 1;

            DefinirImagem(this, $"pista{indiceFundo}.png");
        }

        #endregion

        #region Inicialização do jogo

        private void IniciarJogo()
        {
            do
            {
                for (int linha = 0; linha < 3; linha++)
                {
                    for (int coluna = 0; coluna < 3; coluna++)
                    {
                        tabuleiro[linha, coluna] = true;
                    }
                }

                int quantidadeMovimentos = random.Next(5, 10);

                for (int i = 0; i < quantidadeMovimentos; i++)
                {
                    int linha = random.Next(3);
                    int coluna = random.Next(3);

                    AlternarCasa(linha, coluna);
                    AlternarCasa(linha - 1, coluna);
                    AlternarCasa(linha + 1, coluna);
                    AlternarCasa(linha, coluna - 1);
                    AlternarCasa(linha, coluna + 1);
                }

            } while (Vencer());

            AtualizarBotoes();
        }

        #endregion

        #region Interação com o tabuleiro

        private void Clique(int linha, int coluna)
        {
            if (finalizando)
                return;

            AlternarCasa(linha, coluna);
            AlternarCasa(linha - 1, coluna);
            AlternarCasa(linha + 1, coluna);
            AlternarCasa(linha, coluna - 1);
            AlternarCasa(linha, coluna + 1);

            movimentos++;

            AtualizarBotoes();

            if (Vencer())
            {
                if (round == 1)
                {
                    round++;

                    DefinirImagem(pbx_luz1, "luz_pista2.png");
                    DefinirImagem(pbx_zelo, "zelo2.png");

                    IniciarJogo();
                }
                else if (round == 2)
                {
                    round++;

                    DefinirImagem(pbx_luz2, "luz_pista2.png");
                    DefinirImagem(pbx_zelo, "zelo3.png");

                    FinalizarJogo();
                }
            }
        }

        private void AlternarCasa(int linha, int coluna)
        {
            if (linha < 0 || linha >= 3 ||
                coluna < 0 || coluna >= 3)
            {
                return;
            }

            tabuleiro[linha, coluna] = !tabuleiro[linha, coluna];
        }

        #endregion

        #region Atualização visual do tabuleiro

        private void AtualizarBotoes()
        {
            btn_00.BackColor = tabuleiro[0, 0] ? Color.FromArgb(253, 154, 255) : Color.Gray;
            btn_01.BackColor = tabuleiro[0, 1] ? Color.FromArgb(112, 255, 255) : Color.Gray;
            btn_02.BackColor = tabuleiro[0, 2] ? Color.FromArgb(249, 238, 166) : Color.Gray;

            btn_10.BackColor = tabuleiro[1, 0] ? Color.FromArgb(80, 150, 240) : Color.Gray;
            btn_11.BackColor = tabuleiro[1, 1] ? Color.FromArgb(194, 113, 255) : Color.Gray;
            btn_12.BackColor = tabuleiro[1, 2] ? Color.FromArgb(254, 125, 178) : Color.Gray;

            btn_20.BackColor = tabuleiro[2, 0] ? Color.FromArgb(255, 203, 113) : Color.Gray;
            btn_21.BackColor = tabuleiro[2, 1] ? Color.FromArgb(255, 255, 0) : Color.Gray;
            btn_22.BackColor = tabuleiro[2, 2] ? Color.FromArgb(175, 140, 255) : Color.Gray;
        }

        #endregion

        #region Verificação de vitória

        private bool Vencer()
        {
            for (int linha = 0; linha < 3; linha++)
            {
                for (int coluna = 0; coluna < 3; coluna++)
                {
                    if (!tabuleiro[linha, coluna])
                        return false;
                }
            }

            return true;
        }

        #endregion

        #region Resetar tentativa

        private void btn_resetar_Click(object sender, EventArgs e)
        {
            if (finalizando || resets <= 0)
                return;

            resets--;

            lbl_resets2.Text = resets.ToString();

            if (resets == 0)
            {
                pbx_resetar.Enabled = false;
                lbl_resets2.ForeColor = Color.Red;
            }

            IniciarJogo();
        }

        #endregion

        #region Finalização do jogo

        private void FinalizarJogo()
        {
            if (finalizando)
                return;

            finalizando = true;

            timerTrocaFundo.Stop();

            cronometro.Stop();

            btn_00.Enabled = false;
            btn_01.Enabled = false;
            btn_02.Enabled = false;

            btn_10.Enabled = false;
            btn_11.Enabled = false;
            btn_12.Enabled = false;

            btn_20.Enabled = false;
            btn_21.Enabled = false;
            btn_22.Enabled = false;

            pbx_resetar.Enabled = false;

            painelTopInicial = pnl_painel.Top;

            painelTopFinal =
                pbx_zelo.ClientSize.Height + pnl_painel.Height;

            cronometroAnimacao.Restart();

            timerAnimacaoPainel.Start();
        }

        #endregion

        #region Animação do painel

        private void TimerAnimacaoPainel_Tick(object sender, EventArgs e)
        {
            double progresso =
                cronometroAnimacao.Elapsed.TotalMilliseconds / 2500.0;

            if (progresso >= 1.0)
                progresso = 1.0;

            double movimentoSuave =
                1 - Math.Pow(1 - progresso, 3);

            pnl_painel.Top =
                painelTopInicial +
                (int)((painelTopFinal - painelTopInicial) * movimentoSuave);

            if (progresso >= 1.0)
            {
                timerAnimacaoPainel.Stop();

                cronometroAnimacao.Stop();

                DefinirImagem(pbx_zelo, "zelo4.png");

                timerEsperaVitoria.Start();
            }
        }

        #endregion

        #region Redirecionamento para vitória

        private void TimerEsperaVitoria_Tick(object sender, EventArgs e)
        {
            timerEsperaVitoria.Stop();

            int pontuacaoFinal = CalcularPontuacao();

            JogoForm jogo = null;

            Control atual = this;

            while (atual != null)
            {
                if (atual is JogoForm)
                {
                    jogo = (JogoForm)atual;
                    break;
                }

                atual = atual.Parent;
            }

            if (jogo != null)
            {
                jogo.AbrirTela(
                    new VitoriaControl(
                        IDENTIFICADOR_NIVEL,
                        pontuacaoFinal
                    )
                );
            }
            else
            {
                Cruzeiro cruzeiro = this.FindForm() as Cruzeiro;

                if (cruzeiro != null)
                {
                    cruzeiro.AbrirTela(
                        new VitoriaControl(
                            IDENTIFICADOR_NIVEL,
                            pontuacaoFinal
                        )
                    );
                }
            }
        }

        #endregion

        #region Sistema de pontuação

        private int CalcularPontuacao()
        {
            int bonusConclusao = 500;

            int excessoMovimentos = Math.Max(0, movimentos - 30);

            int pontosMovimentos =
                Math.Max(0, 3000 - (excessoMovimentos * 30));

            int segundos = (int)Math.Ceiling(
                cronometro.Elapsed.TotalSeconds
            );

            int excessoTempo = Math.Max(0, segundos - 60);

            int pontosTempo =
                Math.Max(0, 1500 - (excessoTempo * 50));

            return bonusConclusao +
                   pontosMovimentos +
                   pontosTempo;
        }

        #endregion

        #region Cliques dos botões

        private void btn_00_Click(object sender, EventArgs e)
        {
            Clique(0, 0);
        }

        private void btn_01_Click(object sender, EventArgs e)
        {
            Clique(0, 1);
        }

        private void btn_02_Click(object sender, EventArgs e)
        {
            Clique(0, 2);
        }

        private void btn_10_Click(object sender, EventArgs e)
        {
            Clique(1, 0);
        }

        private void btn_11_Click(object sender, EventArgs e)
        {
            Clique(1, 1);
        }

        private void btn_12_Click(object sender, EventArgs e)
        {
            Clique(1, 2);
        }

        private void btn_20_Click(object sender, EventArgs e)
        {
            Clique(2, 0);
        }

        private void btn_21_Click(object sender, EventArgs e)
        {
            Clique(2, 1);
        }

        private void btn_22_Click(object sender, EventArgs e)
        {
            Clique(2, 2);
        }

        #endregion

        #region Lixeira

        private void lbl_movimentos_Click(object sender, EventArgs e)
        {
        }

        private void lbl_resets2_Click(object sender, EventArgs e)
        {
        }

        private MenuLateralControl EncontrarMenuLateral()
        {
            Control atual = this;

            while (atual != null && !(atual is JogoForm))
            {
                atual = atual.Parent;
            }

            if (atual == null)
                return null;

            return EncontrarMenuLateralRecursivo(atual);
        }

        private MenuLateralControl EncontrarMenuLateralRecursivo(
            Control controle
        )
        {
            if (controle is MenuLateralControl)
                return (MenuLateralControl)controle;

            foreach (Control filho in controle.Controls)
            {
                MenuLateralControl encontrado =
                    EncontrarMenuLateralRecursivo(filho);

                if (encontrado != null)
                    return encontrado;
            }

            return null;
        }

        #endregion
    }
}