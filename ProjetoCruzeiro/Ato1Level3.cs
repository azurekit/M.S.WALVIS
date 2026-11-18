
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;

namespace ProjetoCruzeiro
{
    public partial class Ato1Level3 : UserControl
    {
        #region Configurações

        private const int TAMANHO_TABULEIRO = 9;
        private const int TAMANHO_CELULA = 38;

        private const int POSICAO_TABULEIRO_X = 302;
        private const int POSICAO_TABULEIRO_Y = 375;

        private const int NORTE = 1;
        private const int LESTE = 2;
        private const int SUL = 4;
        private const int OESTE = 8;

        private const int DURACAO_ANIMACAO_VITORIA = 7800;

        private static readonly Color COR_FUNDO_CANOS =
            Color.FromArgb(12, 30, 33);

        private readonly Random random = new Random();

        #endregion

        #region Variáveis globais

        private readonly List<Peca> pecas = new List<Peca>();

        private Bitmap imagemReta;
        private Bitmap imagemL1;
        private Bitmap imagemL2;
        private Bitmap imagemT;
        private Bitmap imagemTotal;

        private Image imagemFumaca;
        private Image imagemVitoria;
        private Image imagemAnimadaFundo;

        private EventHandler eventoFrameAlterado;

        private Panel painelTabuleiro;

        private Timer timerVitoria;

        private int movimentos;
        private int movimentosLimite;
        private int mapaAtual;

        private bool jogoFinalizado;

        private int pontuacaoMovimentos;
        private int pontuacaoTempo;
        private int pontuacaoFinal;

        private readonly Stopwatch cronometro = new Stopwatch();

        #endregion

        #region Mapas

        private readonly string[] mapas =
        {
            "AC66AAAEC" +
            "553BC6C95" +
            "53C39556D" +
            "7A56A9393" +
            "5693AAEAC" +
            "556AAC3C5" +
            "7956C5395" +
            "5AD7C3AAD" +
            "3A993AAAB",

            "ACC6AAAEC" +
            "C53FAAA55" +
            "53C56AA95" +
            "569356C69" +
            "796C533D5" +
            "56D53AC3D" +
            "5553AA965" +
            "3A96EAA79" +
            "3AA93AABA",

            "AC3EAACAC" +
            "63C7CABAD" +
            "56953C569" +
            "556B95795" +
            "5555697A9" +
            "793D5696C" +
            "3AC53AAD5" +
            "6C56C6C35" +
            "33B9393AB",

            "C6EAC3AAC" +
            "3C3C3C6AD" +
            "C3C5697C3" +
            "56953C97C" +
            "7969C3C55" +
            "56D3FA3C5" +
            "555696A95" +
            "3A95696AD" +
            "6AABBA93B",

            "AAAC6EAAC" +
            "6CC595C69" +
            "5553C553C" +
            "53BC397A9" +
            "56AC6C3CC" +
            "555553C55" +
            "53D39C395" +
            "5693AFAAD" +
            "3BAAA96AB",

            "C6C6EC6AC" +
            "3959553AC" +
            "6C3A93AC5" +
            "53AEAA955" +
            "3AC56AC55" +
            "6EA93C555" +
            "556AC5395" +
            "7C56553AD" +
            "53B93BAAB",

            "CAEC6AEA9" +
            "3C539396C" +
            "693AAAA95" +
            "3C6AAAC6D" +
            "539C6A955" +
            "7AC796A95" +
            "56956D6A9" +
            "536953AAC" +
            "3ABAAAAAB",

            "AC6C6C6EC" +
            "C553A9555" +
            "553AC6955" +
            "53AC3BA55" +
            "7AC3AAC3C" +
            "7AAAEC3C5" +
            "56AC56A95" +
            "69AB93AAD" +
            "3AAAAAAAB",

            "AAC3EAAC5" +
            "6A9696C3D" +
            "3AC569369" +
            "69397AA9C" +
            "56EC3C6C5" +
            "5553C393D" +
            "5556AC6C5" +
            "5539C3939" +
            "3BAABAAAA",

            "C6AAEC3AC" +
            "3AAC556C5" +
            "6AC395539" +
            "3C3C6956C" +
            "696956B95" +
            "56AA93CC5" +
            "53AAC6B95" +
            "7AC69396D" +
            "3A3BAAA93"
        };

        #endregion

        #region Classe Peca

        private class Peca
        {
            public int Linha;
            public int Coluna;

            public int ConexaoSolucao;
            public int ConexaoAtual;

            public PictureBox Imagem;
        }

        #endregion

        #region Construtor

        public Ato1Level3()
        {
            InitializeComponent();

            eventoFrameAlterado = EventoFrameAlterado;

            ConfigurarInterface();
            ConfigurarTimerVitoria();

            Disposed += Ato1Level3_Disposed;

            if (!CarregarImagens())
                return;

            GerarTabuleiro();
        }

        #endregion

        #region Configuração da interface

        private void ConfigurarInterface()
        {
            SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint |
                ControlStyles.DoubleBuffer |
                ControlStyles.OptimizedDoubleBuffer,
                true
            );

            UpdateStyles();

            pbx_canos.BackColor = Color.Transparent;
            pbx_movimentos.BackColor = Color.Transparent;

            lbl_movimentos.BackColor = COR_FUNDO_CANOS;

            painelTabuleiro = new Panel();

            painelTabuleiro.Size = new Size(
                TAMANHO_TABULEIRO * TAMANHO_CELULA,
                TAMANHO_TABULEIRO * TAMANHO_CELULA
            );

            painelTabuleiro.Parent = pbx_canos;

            painelTabuleiro.Location = new Point(
                POSICAO_TABULEIRO_X - pbx_canos.Left,
                POSICAO_TABULEIRO_Y - pbx_canos.Top
            );

            painelTabuleiro.BackColor = COR_FUNDO_CANOS;
            painelTabuleiro.BorderStyle = BorderStyle.None;

            pbx_canos.BringToFront();
            pbx_movimentos.BringToFront();
            lbl_movimentos.BringToFront();

            painelTabuleiro.BringToFront();

            lbl_movimentos.Text = "0";
        }

        private void ConfigurarTimerVitoria()
        {
            timerVitoria = new Timer();

            timerVitoria.Interval = DURACAO_ANIMACAO_VITORIA;

            timerVitoria.Tick += TimerVitoria_Tick;
        }

        #endregion

        #region Carregamento de imagens

        private bool CarregarImagens()
        {
            string pasta = Path.Combine(
                Application.StartupPath,
                "imagens",
                "Level3"
            );

            try
            {
                imagemReta = new Bitmap(
                    Path.Combine(pasta, "canoreto.png")
                );

                imagemL1 = new Bitmap(
                    Path.Combine(pasta, "canol1.png")
                );

                imagemL2 = new Bitmap(
                    Path.Combine(pasta, "canol2.png")
                );

                imagemT = new Bitmap(
                    Path.Combine(pasta, "canot.png")
                );

                imagemTotal = new Bitmap(
                    Path.Combine(pasta, "canototal.png")
                );

                imagemFumaca = Image.FromFile(
                    Path.Combine(pasta, "canosfumaça.gif")
                );

                imagemVitoria = Image.FromFile(
                    Path.Combine(pasta, "canosvitoria.gif")
                );

                DefinirAnimacaoFundo(imagemFumaca);

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Não foi possível carregar as imagens do nível.\n\n" +
                    "Verifique a pasta imagens/Level3.\n\n" +
                    ex.Message,
                    "Erro",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );

                return false;
            }
        }

        #endregion

        #region Animação do fundo

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);

            if (imagemAnimadaFundo == null || pbx_canos == null)
                return;

            ImageAnimator.UpdateFrames(imagemAnimadaFundo);

            Rectangle area = pbx_canos.Bounds;

            e.Graphics.DrawImage(
                imagemAnimadaFundo,
                area
            );
        }

        private void DefinirAnimacaoFundo(Image imagem)
        {
            if (imagemAnimadaFundo != null && eventoFrameAlterado != null)
            {
                ImageAnimator.StopAnimate(
                    imagemAnimadaFundo,
                    eventoFrameAlterado
                );
            }

            imagemAnimadaFundo = imagem;

            if (imagemAnimadaFundo != null)
            {
                ImageAnimator.Animate(
                    imagemAnimadaFundo,
                    eventoFrameAlterado
                );
            }

            Invalidate();
        }

        private void EventoFrameAlterado(object sender, EventArgs e)
        {
            if (IsDisposed || Disposing || !IsHandleCreated)
                return;

            try
            {
                if (InvokeRequired)
                {
                    BeginInvoke(new MethodInvoker(() =>
                    {
                        if (!IsDisposed && !Disposing)
                            Invalidate();
                    }));
                }
                else
                {
                    Invalidate();
                }
            }
            catch (InvalidOperationException)
            {
            }
        }

        #endregion

        #region Geração e limpeza do tabuleiro

        private void GerarTabuleiro()
        {
            LimparTabuleiro();

            movimentos = 0;
            jogoFinalizado = false;

            mapaAtual = random.Next(mapas.Length);

            string mapa = mapas[mapaAtual];

            for (int linha = 0; linha < TAMANHO_TABULEIRO; linha++)
            {
                for (int coluna = 0; coluna < TAMANHO_TABULEIRO; coluna++)
                {
                    int indice = linha * TAMANHO_TABULEIRO + coluna;

                    int conexao = Convert.ToInt32(
                        mapa[indice].ToString(),
                        16
                    );

                    Peca peca = new Peca();

                    peca.Linha = linha;
                    peca.Coluna = coluna;
                    peca.ConexaoSolucao = conexao;

                    peca.ConexaoAtual = Rotacionar(
                        conexao,
                        random.Next(4)
                    );

                    PictureBox imagem = new PictureBox();

                    imagem.Size = new Size(
                        TAMANHO_CELULA,
                        TAMANHO_CELULA
                    );

                    imagem.Location = new Point(
                        coluna * TAMANHO_CELULA,
                        linha * TAMANHO_CELULA
                    );

                    imagem.SizeMode = PictureBoxSizeMode.StretchImage;
                    imagem.BackColor = COR_FUNDO_CANOS;
                    imagem.Cursor = Cursors.Hand;
                    imagem.Tag = peca;

                    imagem.Click += Peca_Click;

                    peca.Imagem = imagem;

                    pecas.Add(peca);

                    imagem.Parent = painelTabuleiro;

                    AtualizarImagem(peca);
                }
            }

            int tentativas = 0;

            while (VerificarConexao() && tentativas < 10000)
            {
                foreach (Peca peca in pecas)
                {
                    peca.ConexaoAtual = Rotacionar(
                        peca.ConexaoSolucao,
                        random.Next(4)
                    );

                    AtualizarImagem(peca);
                }

                tentativas++;
            }

            movimentosLimite = CalcularLimiteMovimentos();

            lbl_movimentos.Text = movimentosLimite.ToString();

            cronometro.Restart();

            Invalidate();
        }

        private void LimparTabuleiro()
        {
            foreach (Peca peca in pecas)
            {
                if (peca.Imagem.Image != null)
                {
                    peca.Imagem.Image.Dispose();
                    peca.Imagem.Image = null;
                }

                peca.Imagem.Dispose();
            }

            pecas.Clear();

            if (painelTabuleiro != null)
                painelTabuleiro.Controls.Clear();
        }

        #endregion

        #region Rotação e atualização das peças

        private int Rotacionar(int conexoes, int vezes)
        {
            for (int i = 0; i < vezes; i++)
            {
                int novasConexoes = 0;

                if ((conexoes & NORTE) != 0)
                    novasConexoes |= LESTE;

                if ((conexoes & LESTE) != 0)
                    novasConexoes |= SUL;

                if ((conexoes & SUL) != 0)
                    novasConexoes |= OESTE;

                if ((conexoes & OESTE) != 0)
                    novasConexoes |= NORTE;

                conexoes = novasConexoes;
            }

            return conexoes;
        }

        private void AtualizarImagem(Peca peca)
        {
            int tipo;
            int rotacao;

            IdentificarPeca(
                peca.ConexaoAtual,
                out tipo,
                out rotacao
            );

            Bitmap imagemOriginal = null;

            switch (tipo)
            {
                case 0:
                    imagemOriginal = imagemReta;
                    break;

                case 1:
                    imagemOriginal = imagemL1;
                    break;

                case 2:
                    imagemOriginal = imagemL2;
                    break;

                case 3:
                    imagemOriginal = imagemT;
                    break;

                case 4:
                    imagemOriginal = imagemTotal;
                    break;
            }

            if (imagemOriginal == null)
                return;

            Bitmap novaImagem = new Bitmap(imagemOriginal);

            for (int i = 0; i < rotacao; i++)
            {
                novaImagem.RotateFlip(
                    RotateFlipType.Rotate90FlipNone
                );
            }

            if (peca.Imagem.Image != null)
                peca.Imagem.Image.Dispose();

            peca.Imagem.Image = novaImagem;

            peca.Imagem.BackColor = COR_FUNDO_CANOS;
        }

        private void IdentificarPeca(
            int conexoes,
            out int tipo,
            out int rotacao)
        {
            int[] bases =
            {
                NORTE | SUL,
                NORTE | OESTE,
                NORTE | LESTE,
                NORTE | LESTE | OESTE,
                NORTE | LESTE | SUL | OESTE
            };

            tipo = 0;
            rotacao = 0;

            for (int i = 0; i < bases.Length; i++)
            {
                for (int giro = 0; giro < 4; giro++)
                {
                    if (Rotacionar(bases[i], giro) == conexoes)
                    {
                        tipo = i;
                        rotacao = giro;
                        return;
                    }
                }
            }
        }

        #endregion

        #region Interação com as peças

        private void Peca_Click(object sender, EventArgs e)
        {
            if (jogoFinalizado)
                return;

            PictureBox imagem = sender as PictureBox;

            Peca peca = imagem.Tag as Peca;

            peca.ConexaoAtual = Rotacionar(
                peca.ConexaoAtual,
                1
            );

            AtualizarImagem(peca);

            movimentos++;

            lbl_movimentos.Text = MovimentosRestantes.ToString();

            if (VerificarConexao())
            {
                FinalizarJogo();
                return;
            }

            if (movimentos >= movimentosLimite)
                FinalizarDerrota();
        }

        #endregion

        #region Verificação de conexão

        private bool VerificarConexao()
        {
            if (pecas.Count != TAMANHO_TABULEIRO * TAMANHO_TABULEIRO)
                return false;

            Peca entrada = ObterPeca(0, 0);
            Peca saida = ObterPeca(8, 8);

            if (entrada == null || saida == null)
                return false;

            if ((entrada.ConexaoAtual & OESTE) == 0)
                return false;

            bool[,] visitados = new bool[
                TAMANHO_TABULEIRO,
                TAMANHO_TABULEIRO
            ];

            Queue<Peca> fila = new Queue<Peca>();

            fila.Enqueue(entrada);

            visitados[0, 0] = true;

            while (fila.Count > 0)
            {
                Peca atual = fila.Dequeue();

                if (atual.Linha == 8 && atual.Coluna == 8)
                {
                    if ((atual.ConexaoAtual & LESTE) != 0)
                        return true;
                }

                VerificarVizinho(
                    atual,
                    atual.Linha - 1,
                    atual.Coluna,
                    NORTE,
                    SUL,
                    visitados,
                    fila
                );

                VerificarVizinho(
                    atual,
                    atual.Linha,
                    atual.Coluna + 1,
                    LESTE,
                    OESTE,
                    visitados,
                    fila
                );

                VerificarVizinho(
                    atual,
                    atual.Linha + 1,
                    atual.Coluna,
                    SUL,
                    NORTE,
                    visitados,
                    fila
                );

                VerificarVizinho(
                    atual,
                    atual.Linha,
                    atual.Coluna - 1,
                    OESTE,
                    LESTE,
                    visitados,
                    fila
                );
            }

            return false;
        }

        private void VerificarVizinho(
            Peca atual,
            int linha,
            int coluna,
            int conexaoAtual,
            int conexaoVizinha,
            bool[,] visitados,
            Queue<Peca> fila)
        {
            if (linha < 0 || linha >= TAMANHO_TABULEIRO ||
                coluna < 0 || coluna >= TAMANHO_TABULEIRO)
                return;

            if ((atual.ConexaoAtual & conexaoAtual) == 0)
                return;

            if (visitados[linha, coluna])
                return;

            Peca vizinha = ObterPeca(linha, coluna);

            if (vizinha == null)
                return;

            if ((vizinha.ConexaoAtual & conexaoVizinha) == 0)
                return;

            visitados[linha, coluna] = true;

            fila.Enqueue(vizinha);
        }

        private Peca ObterPeca(int linha, int coluna)
        {
            if (linha < 0 || linha >= TAMANHO_TABULEIRO ||
                coluna < 0 || coluna >= TAMANHO_TABULEIRO)
                return null;

            return pecas[linha * TAMANHO_TABULEIRO + coluna];
        }

        #endregion

        #region Caminho da solução e limite de movimentos

        private List<int> ObterCaminhoSolucao()
        {
            int[] anterior = new int[
                TAMANHO_TABULEIRO * TAMANHO_TABULEIRO
            ];

            for (int i = 0; i < anterior.Length; i++)
                anterior[i] = -2;

            Queue<int> fila = new Queue<int>();

            anterior[0] = -1;

            fila.Enqueue(0);

            int[] linhas = { -1, 0, 1, 0 };
            int[] colunas = { 0, 1, 0, -1 };

            int[] conexoes = { NORTE, LESTE, SUL, OESTE };
            int[] opostas = { SUL, OESTE, NORTE, LESTE };

            while (fila.Count > 0)
            {
                int atual = fila.Dequeue();

                if (atual == 80)
                    break;

                int linha = atual / TAMANHO_TABULEIRO;
                int coluna = atual % TAMANHO_TABULEIRO;

                for (int i = 0; i < 4; i++)
                {
                    if ((pecas[atual].ConexaoSolucao & conexoes[i]) == 0)
                        continue;

                    int novaLinha = linha + linhas[i];
                    int novaColuna = coluna + colunas[i];

                    if (novaLinha < 0 || novaLinha >= TAMANHO_TABULEIRO ||
                        novaColuna < 0 || novaColuna >= TAMANHO_TABULEIRO)
                        continue;

                    int vizinho =
                        novaLinha * TAMANHO_TABULEIRO + novaColuna;

                    if ((pecas[vizinho].ConexaoSolucao & opostas[i]) == 0)
                        continue;

                    if (anterior[vizinho] != -2)
                        continue;

                    anterior[vizinho] = atual;

                    fila.Enqueue(vizinho);
                }
            }

            List<int> caminho = new List<int>();

            if (anterior[80] == -2)
                return caminho;

            int posicao = 80;

            while (posicao != -1)
            {
                caminho.Add(posicao);
                posicao = anterior[posicao];
            }

            caminho.Reverse();

            return caminho;
        }

        private int CalcularLimiteMovimentos()
        {
            List<int> caminho = ObterCaminhoSolucao();

            int movimentosNecessarios = 0;

            foreach (int indice in caminho)
            {
                Peca peca = pecas[indice];

                int menorRotacao = 4;

                for (int giro = 0; giro < 4; giro++)
                {
                    if (Rotacionar(peca.ConexaoAtual, giro) ==
                        peca.ConexaoSolucao)
                    {
                        menorRotacao = giro;
                        break;
                    }
                }

                movimentosNecessarios += menorRotacao;
            }

            return Math.Max(
                40,
                movimentosNecessarios + 12
            );
        }

        #endregion

        #region Pontuação

        private int MovimentosRestantes
        {
            get
            {
                return Math.Max(0, movimentosLimite - movimentos);
            }
        }

        private void CalcularPontuacao()
        {
            const int PONTUACAO_MAXIMA_MOVIMENTOS = 3500;
            const int PONTUACAO_MAXIMA_TEMPO = 1000;
            const int BONUS_CONCLUSAO = 500;

            // Calcula o desempenho proporcional aos movimentos disponíveis.
            double aproveitamentoMovimentos = movimentosLimite > 0
                ? (double)MovimentosRestantes / movimentosLimite
                : 0;

            pontuacaoMovimentos = (int)Math.Round(
                aproveitamentoMovimentos * PONTUACAO_MAXIMA_MOVIMENTOS
            );

            // Pontuação por tempo.
            int segundos = (int)Math.Floor(
                cronometro.Elapsed.TotalSeconds
            );

            pontuacaoTempo = Math.Max(
                0,
                PONTUACAO_MAXIMA_TEMPO - Math.Max(0, segundos - 25) * 50
            );

            // Pontuação final.
            pontuacaoFinal =
                BONUS_CONCLUSAO +
                pontuacaoMovimentos +
                pontuacaoTempo;
        }

        #endregion

        #region Vitória

        private void FinalizarJogo()
        {
            if (jogoFinalizado)
                return;

            jogoFinalizado = true;

            cronometro.Stop();

            CalcularPontuacao();

            foreach (Peca peca in pecas)
            {
                peca.Imagem.Enabled = false;
                peca.Imagem.Cursor = Cursors.Default;
            }

            // Inicia a animação de vitória.
            DefinirAnimacaoFundo(imagemVitoria);

            timerVitoria.Stop();
            timerVitoria.Start();
        }

        private void TimerVitoria_Tick(object sender, EventArgs e)
        {
            timerVitoria.Stop();

            JogoForm jogo = EncontrarJogoForm();

            if (jogo != null)
            {
                jogo.AbrirTela(
                    new VitoriaControl(3, pontuacaoFinal)
                );
            }
        }

        #endregion

        #region Derrota

        private void FinalizarDerrota()
        {
            if (jogoFinalizado)
                return;

            jogoFinalizado = true;

            cronometro.Stop();

            foreach (Peca peca in pecas)
            {
                peca.Imagem.Enabled = false;
                peca.Imagem.Cursor = Cursors.Default;
            }

            JogoForm jogo = EncontrarJogoForm();

            if (jogo != null)
            {
                jogo.AbrirTela(
                    new DerrotaControl(
                        3,
                        "Você não conseguiu estabelecer a conexão a tempo!\n\n" +
                        "Movimentos realizados: " + movimentos +
                        "\nMovimentos restantes: 0"
                    )
                );
            }
        }

        #endregion

        #region Localização do JogoForm

        private JogoForm EncontrarJogoForm()
        {
            Control controle = this;

            while (controle != null && !(controle is JogoForm))
            {
                controle = controle.Parent;
            }

            return controle as JogoForm;
        }

        #endregion

        #region Limpeza e descarte

        private void Ato1Level3_Disposed(object sender, EventArgs e)
        {
            cronometro.Stop();

            if (timerVitoria != null)
            {
                timerVitoria.Stop();
                timerVitoria.Tick -= TimerVitoria_Tick;
                timerVitoria.Dispose();
                timerVitoria = null;
            }

            if (imagemAnimadaFundo != null && eventoFrameAlterado != null)
            {
                ImageAnimator.StopAnimate(
                    imagemAnimadaFundo,
                    eventoFrameAlterado
                );

                imagemAnimadaFundo = null;
            }

            if (imagemFumaca != null)
            {
                imagemFumaca.Dispose();
                imagemFumaca = null;
            }

            if (imagemVitoria != null)
            {
                imagemVitoria.Dispose();
                imagemVitoria = null;
            }

            if (imagemReta != null)
                imagemReta.Dispose();

            if (imagemL1 != null)
                imagemL1.Dispose();

            if (imagemL2 != null)
                imagemL2.Dispose();

            if (imagemT != null)
                imagemT.Dispose();

            if (imagemTotal != null)
                imagemTotal.Dispose();
        }

        #endregion

        #region Menu Lateral

        private MenuLateralControl EncontrarMenuLateralRecursivo(
            Control controle)
        {
            foreach (Control filho in controle.Controls)
            {
                if (filho is MenuLateralControl)
                    return filho as MenuLateralControl;

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
