
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;

namespace ProjetoCruzeiro
{
    public partial class DerrotaControl : UserControl
    {
        #region Variáveis Globais

        private int identificador;

        private List<EfeitoBotao> efeitos = new List<EfeitoBotao>();

        private Timer timerAnimacao;
        private Stopwatch cronometroAnimacao;

        private const int DURACAO_ANIMACAO = 650;
        private const int INTERVALO_ANIMACAO = 16;

        private Point posicaoFinalPainel = new Point(123, 3);
        private Point posicaoInicialPainel = new Point(123, 720);

        private bool animacaoIniciada;

        #endregion

        #region Configuração inicial

        public DerrotaControl(int identificador, string textoPersonalizado = null)
        {
            InitializeComponent();

            this.identificador = identificador;

            SetStyle(
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.ResizeRedraw,
                true
            );

            UpdateStyles();

            AtivarDoubleBuffer(this);
            AtivarDoubleBuffer(pnl_derrota);

            CarregarFundo();
            CarregarPainelDerrota();
            CarregarEstrelas();

            ConfigurarTexto(textoPersonalizado);
            ConfigurarEfeitosBotoes();

            PrepararAnimacao();

            pbx_voltar.Click -= pbx_voltar_Click;
            pbx_voltar.Click += pbx_voltar_Click;

            pbx_continuar.Click -= btn_continuar_Click;
            pbx_continuar.Click += btn_continuar_Click;

            VisibleChanged += DerrotaControl_VisibleChanged;

            Disposed += DerrotaControl_Disposed;
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams parametros = base.CreateParams;
                parametros.ExStyle |= 0x02000000;
                return parametros;
            }
        }

        private void DerrotaControl_Disposed(object sender, EventArgs e)
        {
            if (timerAnimacao != null)
            {
                timerAnimacao.Stop();
                timerAnimacao.Dispose();
                timerAnimacao = null;
            }

            if (cronometroAnimacao != null)
            {
                cronometroAnimacao.Stop();
                cronometroAnimacao = null;
            }
        }

        #endregion

        #region Double Buffer

        private void AtivarDoubleBuffer(Control controle)
        {
            typeof(Control).GetProperty(
                "DoubleBuffered",
                System.Reflection.BindingFlags.NonPublic |
                System.Reflection.BindingFlags.Instance
            ).SetValue(controle, true, null);
        }

        #endregion

        #region Carregar imagens

        private Image CarregarImagem(string caminho)
        {
            using (Image imagem = Image.FromFile(caminho))
            {
                return new Bitmap(imagem);
            }
        }

        private void CarregarFundo()
        {
            string caminho = Path.Combine(
                Application.StartupPath,
                "imagens",
                "level" + identificador + "derrotabg.png"
            );

            if (!File.Exists(caminho))
            {
                MessageBox.Show(
                    "Não foi possível encontrar a imagem:\n" + caminho,
                    "Erro ao carregar derrota",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );

                return;
            }

            if (BackgroundImage != null)
                BackgroundImage.Dispose();

            BackgroundImage = CarregarImagem(caminho);
            BackgroundImageLayout = ImageLayout.Stretch;
        }

        private void CarregarPainelDerrota()
        {
            string caminho = Path.Combine(
                Application.StartupPath,
                "imagens",
                "derrotabase.png"
            );

            if (!File.Exists(caminho))
            {
                MessageBox.Show(
                    "Não foi possível encontrar a imagem:\n" + caminho,
                    "Erro ao carregar painel de derrota",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );

                return;
            }

            if (pnl_derrota.BackgroundImage != null)
                pnl_derrota.BackgroundImage.Dispose();

            pnl_derrota.BackgroundImage = CarregarImagem(caminho);
            pnl_derrota.BackgroundImageLayout = ImageLayout.Stretch;
        }

        private void CarregarEstrelas()
        {
            string caminho = Path.Combine(
                Application.StartupPath,
                "imagens",
                "estrelas0.png"
            );

            if (!File.Exists(caminho))
            {
                MessageBox.Show(
                    "Não foi possível encontrar a imagem:\n" + caminho,
                    "Erro ao carregar estrelas",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );

                return;
            }

            if (pbx_estrelas.BackgroundImage != null)
                pbx_estrelas.BackgroundImage.Dispose();

            pbx_estrelas.BackgroundImage = CarregarImagem(caminho);
            pbx_estrelas.BackgroundImageLayout = ImageLayout.Stretch;
        }

        #endregion

        #region Texto da derrota

        private void ConfigurarTexto(string textoPersonalizado)
        {
            if (!string.IsNullOrWhiteSpace(textoPersonalizado))
            {
                lbl_texto.Text = textoPersonalizado;
                return;
            }

            switch (identificador)
            {
                case 1:
                    lbl_texto.Text =
                        "As câmeras continuam fora de ordem! Você não conseguiu identificar a sequência correta.";
                    break;

                case 2:
                    lbl_texto.Text =
                        "Os dados não revelaram a combinação esperada. Você não conseguiu resolver o desafio.";
                    break;

                case 3:
                    lbl_texto.Text =
                        "O sistema entrou em colapso! Os canos não foram conectados a tempo.";
                    break;

                case 4:
                    lbl_texto.Text =
                        "Os pratos não foram organizados corretamente. O desafio não foi concluído.";
                    break;

                case 5:
                    lbl_texto.Text =
                        "A combinação das bebidas não foi descoberta. Você não conseguiu concluir o desafio.";
                    break;

                case 6:
                    lbl_texto.Text =
                        "As luzes da pista foram apagadas na sequência errada. O truque não foi desvendado.";
                    break;

                case 7:
                    lbl_texto.Text =
                        "A pressão da piscina não foi estabilizada. As evidências não puderam ser recuperadas.";
                    break;

                case 8:
                    lbl_texto.Text =
                        "As falas da apresentação ficaram fora de ordem. Você não conseguiu reconstruir a sequência.";
                    break;

                case 9:
                    lbl_texto.Text =
                        "As pistas foram perdidas! Você não conseguiu resolver o desafio a tempo.";
                    break;

                default:
                    lbl_texto.Text =
                        "Você não conseguiu concluir o desafio!";
                    break;
            }
        }

        #endregion

        #region Efeitos dos botões

        private void ConfigurarEfeitosBotoes()
        {
            efeitos.Add(new EfeitoBotao(pbx_voltar));
            efeitos.Add(new EfeitoBotao(pbx_continuar));

            pbx_voltar.Cursor = Cursors.Hand;
            pbx_continuar.Cursor = Cursors.Hand;
        }

        #endregion

        #region Animação do painel

        private void PrepararAnimacao()
        {
            pnl_derrota.Location = posicaoInicialPainel;

            timerAnimacao = new Timer();
            timerAnimacao.Interval = INTERVALO_ANIMACAO;
            timerAnimacao.Tick += TimerAnimacao_Tick;

            VisibleChanged += DerrotaControl_VisibleChanged;

            if (Visible)
                IniciarAnimacao();
        }

        private void DerrotaControl_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible && !animacaoIniciada)
                IniciarAnimacao();
        }

        private void IniciarAnimacao()
        {
            if (animacaoIniciada || timerAnimacao == null)
                return;

            animacaoIniciada = true;

            cronometroAnimacao = Stopwatch.StartNew();

            timerAnimacao.Start();
        }

        private void TimerAnimacao_Tick(object sender, EventArgs e)
        {
            double progresso =
                cronometroAnimacao.Elapsed.TotalMilliseconds /
                DURACAO_ANIMACAO;

            if (progresso >= 1)
            {
                pnl_derrota.Location = posicaoFinalPainel;

                FinalizarAnimacao();

                return;
            }

            double suavizacao =
                progresso * progresso * (3 - 2 * progresso);

            int novaPosicaoY = (int)Math.Round(
                posicaoInicialPainel.Y +
                (posicaoFinalPainel.Y -
                posicaoInicialPainel.Y) * suavizacao
            );

            pnl_derrota.Location = new Point(
                posicaoFinalPainel.X,
                novaPosicaoY
            );
        }

        private void FinalizarAnimacao()
        {
            if (timerAnimacao != null)
            {
                timerAnimacao.Stop();
                timerAnimacao.Dispose();
                timerAnimacao = null;
            }

            if (cronometroAnimacao != null)
            {
                cronometroAnimacao.Stop();
                cronometroAnimacao = null;
            }
        }

        #endregion

        #region Navegação

        private void btn_continuar_Click(object sender, EventArgs e)
        {
            Cruzeiro cruzeiro = this.FindForm() as Cruzeiro;

            if (cruzeiro == null)
                return;

            if (identificador >= 1 && identificador <= 9)
            {
                int ato = identificador <= 3 ? 1 :
                          identificador <= 6 ? 2 : 3;

                cruzeiro.AbrirTela(new LevelSelectControl(ato));
            }
        }

        private void pbx_voltar_Click(object sender, EventArgs e)
        {
            Control controle = this;

            while (controle != null && !(controle is JogoForm))
            {
                controle = controle.Parent;
            }

            JogoForm jogo = controle as JogoForm;

            if (jogo == null)
                return;

            switch (identificador)
            {
                case 1:
                    jogo.AbrirTela(new Ato1Level1());
                    break;

                case 2:
                    jogo.AbrirTela(new Ato1Level2());
                    break;

                case 3:
                    jogo.AbrirTela(new Ato1Level3());
                    break;

                case 4:
                    jogo.AbrirTela(new Ato2Level1());
                    break;

                case 5:
                    jogo.AbrirTela(new Ato2Level2());
                    break;

                case 6:
                    jogo.AbrirTela(new Ato2Level3());
                    break;

                case 7:
                    jogo.AbrirTela(new Ato3Level1());
                    break;

                case 8:
                    jogo.AbrirTela(new Ato3Level2());
                    break;

                case 9:
                    jogo.AbrirTela(new Ato3Level3());
                    break;
            }
        }

        #endregion
    }
}
