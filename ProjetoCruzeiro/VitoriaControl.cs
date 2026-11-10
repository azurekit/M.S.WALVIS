
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;

namespace ProjetoCruzeiro
{
    public partial class VitoriaControl : UserControl
    {
        #region Variáveis Globais

        public static int Moedas
        {
            get { return SaveManager.ObterMoedas(); }
        }

        public int DiferencaRecorde { get; private set; }

        private int identificador;
        private int pontuacaoFinal;
        private int nivelEstrelasAtual = -1;

        private List<EfeitoBotao> efeitos = new List<EfeitoBotao>();

        private Timer timerAnimacao;
        private Timer timerPontuacao;

        private Stopwatch cronometroAnimacao;
        private Stopwatch cronometroPontuacao;

        private const int DURACAO_ANIMACAO = 650;
        private const int DURACAO_PONTUACAO = 3000;
        private const int INTERVALO_ANIMACAO = 16;

        private Point posicaoFinalPainel = new Point(123, 3);
        private Point posicaoInicialPainel = new Point(123, 720);

        private bool animacaoIniciada;
        private bool menuBloqueado;

        #endregion

        #region Configuração inicial

        public VitoriaControl(int identificador, int pontuacao)
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
            AtivarDoubleBuffer(pnl_vitoria);

            CarregarFundo();

            ProcessarPontuacao(pontuacao);

            ConfigurarEfeitosBotoes();

            PrepararAnimacao();

            ParentChanged += VitoriaControl_ParentChanged;

            Disposed += VitoriaControl_Disposed;
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

        private void VitoriaControl_Disposed(object sender, EventArgs e)
        {
            if (timerAnimacao != null)
            {
                timerAnimacao.Stop();
                timerAnimacao.Dispose();
                timerAnimacao = null;
            }

            if (timerPontuacao != null)
            {
                timerPontuacao.Stop();
                timerPontuacao.Dispose();
                timerPontuacao = null;
            }

            if (cronometroAnimacao != null)
            {
                cronometroAnimacao.Stop();
                cronometroAnimacao = null;
            }

            if (cronometroPontuacao != null)
            {
                cronometroPontuacao.Stop();
                cronometroPontuacao = null;
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
            string pasta = Path.Combine(
                Application.StartupPath,
                "imagens"
            );

            string caminho = Path.Combine(
                pasta,
                "level" + identificador + "bgblur.png"
            );

            if (File.Exists(caminho))
            {
                if (BackgroundImage != null)
                    BackgroundImage.Dispose();

                BackgroundImage = CarregarImagem(caminho);
                BackgroundImageLayout = ImageLayout.Stretch;
            }
        }

        #endregion

        #region Pontuação

        private void ProcessarPontuacao(int pontuacao)
        {
            pontuacaoFinal = Math.Max(0, pontuacao);

            lbl_score.Text = "0";

            AtualizarEstrelas(0);

            DiferencaRecorde = EnviarPontuacao(
                identificador,
                pontuacaoFinal
            );
        }

        private void AtualizarEstrelas(int pontuacao)
        {
            int novoNivel;

            if (pontuacao >= 5000)
                novoNivel = 4;
            else if (pontuacao >= 3500)
                novoNivel = 3;
            else if (pontuacao >= 2500)
                novoNivel = 2;
            else if (pontuacao >= 1500)
                novoNivel = 1;
            else
                novoNivel = 0;

            if (novoNivel == nivelEstrelasAtual)
                return;

            nivelEstrelasAtual = novoNivel;

            string caminho = Path.Combine(
                Application.StartupPath,
                "imagens",
                "estrelas" + novoNivel + ".png"
            );

            if (!File.Exists(caminho))
                return;

            Image imagemAnterior = pbx_estrelas.BackgroundImage;

            pbx_estrelas.BackgroundImage = CarregarImagem(caminho);
            pbx_estrelas.BackgroundImageLayout = ImageLayout.Stretch;

            if (imagemAnterior != null)
                imagemAnterior.Dispose();
        }

        private void TimerPontuacao_Tick(object sender, EventArgs e)
        {
            double progresso =
                cronometroPontuacao.Elapsed.TotalMilliseconds /
                DURACAO_PONTUACAO;

            if (progresso >= 1)
            {
                lbl_score.Text = pontuacaoFinal.ToString();

                AtualizarEstrelas(pontuacaoFinal);

                FinalizarAnimacaoPontuacao();

                return;
            }

            int valorAtual = (int)Math.Round(
                pontuacaoFinal * progresso
            );

            lbl_score.Text = valorAtual.ToString();

            AtualizarEstrelas(valorAtual);
        }

        private void FinalizarAnimacaoPontuacao()
        {
            if (timerPontuacao != null)
            {
                timerPontuacao.Stop();
                timerPontuacao.Dispose();
                timerPontuacao = null;
            }

            if (cronometroPontuacao != null)
            {
                cronometroPontuacao.Stop();
                cronometroPontuacao = null;
            }
        }

        public static bool EstaConcluido(int identificador)
        {
            return SaveManager.EstaConcluido(identificador);
        }

        public static int ObterHighscore(int identificador)
        {
            return SaveManager.ObterHighscore(identificador);
        }

        public static int QuantidadeNiveisConcluidos()
        {
            return SaveManager.QuantidadeNiveisConcluidos();
        }

        public static bool TodosNiveisConcluidos()
        {
            return QuantidadeNiveisConcluidos() == 9;
        }

        private int EnviarPontuacao(int identificador, int pontuacao)
        {
            if (identificador < 1 || identificador > 9)
                return 0;

            int recordeAnterior =
                SaveManager.ObterHighscore(identificador);

            int diferencaRecorde = 0;

            if (pontuacao > recordeAnterior)
            {
                diferencaRecorde = pontuacao - recordeAnterior;
            }

            SaveManager.AtualizarLevel(
                identificador,
                true,
                pontuacao
            );

            if (diferencaRecorde > 0)
            {
                SaveManager.DefinirMoedas(
                    SaveManager.ObterMoedas() + diferencaRecorde
                );
            }

            int novoRecorde =
                SaveManager.ObterHighscore(identificador);

            Console.WriteLine("========== PROGRESSO ==========");
            Console.WriteLine($"Nível: {identificador}");
            Console.WriteLine($"Concluído: {EstaConcluido(identificador)}");
            Console.WriteLine($"Pontuação recebida: {pontuacao}");
            Console.WriteLine($"Recorde anterior: {recordeAnterior}");
            Console.WriteLine($"Novo recorde: {novoRecorde}");
            Console.WriteLine($"Moedas recebidas: {diferencaRecorde}");
            Console.WriteLine($"Total de moedas: {Moedas}");
            Console.WriteLine($"Níveis concluídos: {QuantidadeNiveisConcluidos()}/9");
            Console.WriteLine("===============================");

            return diferencaRecorde;
        }

        #endregion

        #region Bloquear menu lateral

        private void VitoriaControl_ParentChanged(object sender, EventArgs e)
        {
            if (Visible)
                BloquearMenuLateral();
        }

        private void BloquearMenuLateral()
        {
            if (menuBloqueado)
                return;

            Control controle = this;

            while (controle != null && !(controle is JogoForm))
            {
                controle = controle.Parent;
            }

            JogoForm jogo = controle as JogoForm;

            if (jogo == null)
                return;

            MenuLateralControl menu = EncontrarMenuLateral(jogo);

            if (menu == null)
                return;

            menu.BloquearVoltarMenu();

            menuBloqueado = true;
        }

        private MenuLateralControl EncontrarMenuLateral(Control controle)
        {
            foreach (Control filho in controle.Controls)
            {
                if (filho is MenuLateralControl)
                    return filho as MenuLateralControl;

                MenuLateralControl encontrado =
                    EncontrarMenuLateral(filho);

                if (encontrado != null)
                    return encontrado;
            }

            return null;
        }

        private void DesbloquearMenuLateral(JogoForm jogo)
        {
            MenuLateralControl menu = EncontrarMenuLateral(jogo);

            if (menu == null)
                return;

            menu.PermitirVoltarMenu();

            menuBloqueado = false;
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
            pnl_vitoria.Location = posicaoInicialPainel;

            timerAnimacao = new Timer();
            timerAnimacao.Interval = INTERVALO_ANIMACAO;
            timerAnimacao.Tick += TimerAnimacao_Tick;

            timerPontuacao = new Timer();
            timerPontuacao.Interval = INTERVALO_ANIMACAO;
            timerPontuacao.Tick += TimerPontuacao_Tick;

            VisibleChanged += VitoriaControl_VisibleChanged;

            if (Visible)
            {
                BloquearMenuLateral();
                IniciarAnimacao();
            }
        }

        private void VitoriaControl_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible)
            {
                BloquearMenuLateral();

                if (!animacaoIniciada)
                    IniciarAnimacao();
            }
        }

        private void IniciarAnimacao()
        {
            if (animacaoIniciada || timerAnimacao == null)
                return;

            animacaoIniciada = true;

            cronometroAnimacao = Stopwatch.StartNew();
            cronometroPontuacao = Stopwatch.StartNew();

            timerAnimacao.Start();
            timerPontuacao.Start();
        }

        private void TimerAnimacao_Tick(object sender, EventArgs e)
        {
            double progresso =
                cronometroAnimacao.Elapsed.TotalMilliseconds /
                DURACAO_ANIMACAO;

            if (progresso >= 1)
            {
                pnl_vitoria.Location = posicaoFinalPainel;

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

            pnl_vitoria.Location = new Point(
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
                Control controle = this;

                while (controle != null && !(controle is JogoForm))
                {
                    controle = controle.Parent;
                }

                JogoForm jogo = controle as JogoForm;

                if (jogo != null)
                    DesbloquearMenuLateral(jogo);

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

            DesbloquearMenuLateral(jogo);

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
