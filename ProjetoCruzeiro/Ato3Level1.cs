using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace ProjetoCruzeiro
{
    public partial class Ato3Level1 : UserControl
    {
        #region Variáveis Globais

        private Random random = new Random();

        private int pressaoA;
        private int pressaoB;
        private int pressaoC;
        private int pressaoD;

        private const int metaA = 5;
        private const int metaB = 5;
        private const int metaC = 3;
        private const int metaD = 4;

        private const int pressaoMinima = 1;
        private const int pressaoMaxima = 9;

        private const int IDENTIFICADOR = 7;

        private int movimentosUtilizados = 0;
        private int pontuacaoFinal = 0;

        private bool concluido = false;
        private bool venceu = false;

        private List<string> sequenciaEmbaralhamento = new List<string>();
        private List<EfeitoBotao> efeitos = new List<EfeitoBotao>();

        private Stopwatch cronometro = new Stopwatch();

        private Timer timerVitoria = new Timer();

        private PictureBox pbxAtualA;
        private PictureBox pbxAtualB;
        private PictureBox pbxAtualC;
        private PictureBox pbxAtualD;

        private PictureBox pbxMetaA;
        private PictureBox pbxMetaB;
        private PictureBox pbxMetaC;
        private PictureBox pbxMetaD;

        private PictureBox pbxRosa;
        private PictureBox pbxVermelha;
        private PictureBox pbxVerde;
        private PictureBox pbxAmarela;
        private PictureBox pbxBranca;
        private PictureBox pbxRoxa;
        private PictureBox pbxReiniciar;

        #endregion

        #region Propriedades

        public int MovimentosUtilizados
        {
            get { return movimentosUtilizados; }
        }

        public bool Concluido
        {
            get { return concluido; }
        }

        public bool Venceu
        {
            get { return venceu; }
        }

        #endregion

        #region Construtor

        public Ato3Level1()
        {
            InitializeComponent();

            AplicarDoubleBuffering(this);

            ConfigurarControles();
            ConfigurarEventos();
            ConfigurarAnimacoes();

            GerarConfiguracaoAleatoria();

            cronometro.Start();

            timerVitoria.Interval = 3000;
            timerVitoria.Tick += TimerVitoria_Tick;

            AtualizarInterface();
        }

        #endregion

        #region Configuração dos Controles

        private void ConfigurarControles()
        {
            pbxAtualA = BuscarControle<PictureBox>("pbx_atualA");
            pbxAtualB = BuscarControle<PictureBox>("pbx_atualB");
            pbxAtualC = BuscarControle<PictureBox>("pbx_atualC");
            pbxAtualD = BuscarControle<PictureBox>("pbx_atualD");

            pbxMetaA = BuscarControle<PictureBox>("pbx_metaA");
            pbxMetaB = BuscarControle<PictureBox>("pbx_metaB");
            pbxMetaC = BuscarControle<PictureBox>("pbx_metaC");
            pbxMetaD = BuscarControle<PictureBox>("pbx_metaD");

            pbxRosa = BuscarControle<PictureBox>("pbx_rosa");
            pbxVermelha = BuscarControle<PictureBox>("pbx_vermelha");
            pbxVerde = BuscarControle<PictureBox>("pbx_verde");
            pbxAmarela = BuscarControle<PictureBox>("pbx_amarela");
            pbxBranca = BuscarControle<PictureBox>("pbx_branca");
            pbxRoxa = BuscarControle<PictureBox>("pbx_roxa");

            pbxReiniciar = BuscarControle<PictureBox>("pbx_reiniciar");

            AplicarDoubleBuffering(this);
        }

        private T BuscarControle<T>(string nome) where T : Control
        {
            Control controle = Controls.Find(nome, true).FirstOrDefault();

            if (controle == null)
                throw new Exception("Controle não encontrado: " + nome);

            return controle as T;
        }

        #endregion

        #region Double Buffering

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;

                return cp;
            }
        }

        private void AplicarDoubleBuffering(Control controle)
        {
            if (controle == null)
                return;

            PropertyInfo propriedade = typeof(Control).GetProperty(
                "DoubleBuffered",
                BindingFlags.Instance | BindingFlags.NonPublic
            );

            if (propriedade != null)
                propriedade.SetValue(controle, true, null);

            foreach (Control filho in controle.Controls)
            {
                AplicarDoubleBuffering(filho);
            }
        }

        #endregion

        #region Associação de Eventos

        private void ConfigurarEventos()
        {
            pbxRosa.Click += (s, e) => AplicarValvula(2, -1, 0, 0);
            pbxVermelha.Click += (s, e) => AplicarValvula(-1, 2, 0, 0);
            pbxVerde.Click += (s, e) => AplicarValvula(0, -1, 2, 0);
            pbxAmarela.Click += (s, e) => AplicarValvula(0, 0, -2, 2);
            pbxBranca.Click += (s, e) => AplicarValvula(1, 0, -1, 1);
            pbxRoxa.Click += (s, e) => AplicarValvula(0, 1, 0, -2);

            pbxReiniciar.Click += (s, e) => ReiniciarJogo();
        }

        #endregion

        #region Animações dos Botões

        private void ConfigurarAnimacoes()
        {
            PictureBox[] botoes =
            {
                pbxRosa,
                pbxVermelha,
                pbxVerde,
                pbxAmarela,
                pbxBranca,
                pbxRoxa,
                pbxReiniciar
            };

            foreach (PictureBox botao in botoes)
            {
                efeitos.Add(new EfeitoBotao(botao));
                botao.Cursor = Cursors.Hand;
            }
        }

        #endregion

        #region Geração Aleatória

        private void GerarConfiguracaoAleatoria()
        {
            int[][] movimentosInversos =
            {
                new int[] { -2,  1,  0,  0 },
                new int[] {  1, -2,  0,  0 },
                new int[] {  0,  1, -2,  0 },
                new int[] {  0,  0,  2, -2 },
                new int[] { -1,  0,  1, -1 },
                new int[] {  0, -1,  0,  2 }
            };

            string[] nomesBotoes =
            {
                "ROSA",
                "VERMELHA",
                "VERDE",
                "AMARELA",
                "BRANCA",
                "ROXA"
            };

            while (true)
            {
                int a = metaA;
                int b = metaB;
                int c = metaC;
                int d = metaD;

                List<string> novaSequencia = new List<string>();

                int quantidadeMovimentos = random.Next(8, 13);

                for (int i = 0; i < quantidadeMovimentos; i++)
                {
                    List<int> movimentosValidos = new List<int>();

                    for (int j = 0; j < movimentosInversos.Length; j++)
                    {
                        int novoA = a + movimentosInversos[j][0];
                        int novoB = b + movimentosInversos[j][1];
                        int novoC = c + movimentosInversos[j][2];
                        int novoD = d + movimentosInversos[j][3];

                        if (novoA >= pressaoMinima && novoA <= pressaoMaxima &&
                            novoB >= pressaoMinima && novoB <= pressaoMaxima &&
                            novoC >= pressaoMinima && novoC <= pressaoMaxima &&
                            novoD >= pressaoMinima && novoD <= pressaoMaxima)
                        {
                            movimentosValidos.Add(j);
                        }
                    }

                    if (movimentosValidos.Count == 0)
                        break;

                    int escolhido = movimentosValidos[
                        random.Next(movimentosValidos.Count)
                    ];

                    a += movimentosInversos[escolhido][0];
                    b += movimentosInversos[escolhido][1];
                    c += movimentosInversos[escolhido][2];
                    d += movimentosInversos[escolhido][3];

                    novaSequencia.Add(nomesBotoes[escolhido]);
                }

                if (a == metaA && b == metaB &&
                    c == metaC && d == metaD)
                {
                    continue;
                }

                if (novaSequencia.Count < 8)
                    continue;

                pressaoA = a;
                pressaoB = b;
                pressaoC = c;
                pressaoD = d;

                sequenciaEmbaralhamento = novaSequencia;

                ImprimirSequencia(novaSequencia);

                break;
            }
        }

        private void ImprimirSequencia(List<string> movimentosEmbaralhamento)
        {
            Console.WriteLine("");
            Console.WriteLine("========== NOVA CONFIGURACAO ==========");

            Console.WriteLine(
                "PRESSOES INICIAIS: A=" + pressaoA +
                " | B=" + pressaoB +
                " | C=" + pressaoC +
                " | D=" + pressaoD
            );

            Console.WriteLine("");
            Console.WriteLine("========== EMBARALHAMENTO ==========");

            for (int i = 0; i < movimentosEmbaralhamento.Count; i++)
            {
                Console.WriteLine(
                    (i + 1) + " - INVERSO DA VALVULA: " +
                    movimentosEmbaralhamento[i]
                );
            }

            Console.WriteLine("");
            Console.WriteLine("========== SEQUENCIA PARA RESOLVER ==========");

            int contador = 1;

            for (int i = sequenciaEmbaralhamento.Count - 1; i >= 0; i--)
            {
                Console.WriteLine(
                    contador + " - CLICAR NA VALVULA: " +
                    sequenciaEmbaralhamento[i]
                );

                contador++;
            }

            Console.WriteLine("");
            Console.WriteLine(
                "TOTAL DE MOVIMENTOS DA SEQUENCIA: " +
                sequenciaEmbaralhamento.Count
            );

            Console.WriteLine("========================================");
            Console.WriteLine("");
        }

        #endregion

        #region Mecânica das Válvulas

        private void AplicarValvula(int a, int b, int c, int d)
        {
            if (concluido)
                return;

            int novoA = pressaoA + a;
            int novoB = pressaoB + b;
            int novoC = pressaoC + c;
            int novoD = pressaoD + d;

            if (novoA < pressaoMinima || novoA > pressaoMaxima ||
                novoB < pressaoMinima || novoB > pressaoMaxima ||
                novoC < pressaoMinima || novoC > pressaoMaxima ||
                novoD < pressaoMinima || novoD > pressaoMaxima)
            {
                return;
            }

            pressaoA = novoA;
            pressaoB = novoB;
            pressaoC = novoC;
            pressaoD = novoD;

            movimentosUtilizados++;

            AtualizarInterface();
            VerificarResultado();
        }

        #endregion

        #region Atualização da Interface

        private void AtualizarInterface()
        {
            AtualizarImagem(pbxAtualA, pressaoA);
            AtualizarImagem(pbxAtualB, pressaoB);
            AtualizarImagem(pbxAtualC, pressaoC);
            AtualizarImagem(pbxAtualD, pressaoD);

            AtualizarImagem(pbxMetaA, metaA);
            AtualizarImagem(pbxMetaB, metaB);
            AtualizarImagem(pbxMetaC, metaC);
            AtualizarImagem(pbxMetaD, metaD);

            HabilitarValvulas(!concluido);
        }

        #endregion

        #region Carregamento e Atualização de Imagens   

        private void AtualizarImagem(
            PictureBox pictureBox,
            int pressao,
            bool vitoria = false
        )
        {
            if (pictureBox == null)
                return;

            string sufixo = vitoria ? "w" : "";

            string caminho = Path.Combine(
                Application.StartupPath,
                "imagens",
                "Level7",
                "pressao" + pressao + sufixo + ".png"
            );

            if (!File.Exists(caminho))
            {
                throw new FileNotFoundException(
                    "Imagem de pressão não encontrada: " + caminho
                );
            }

            Image novaImagem;

            using (Image imagemOriginal = Image.FromFile(caminho))
            {
                novaImagem = new Bitmap(imagemOriginal);
            }

            Image imagemAnterior = pictureBox.BackgroundImage;

            pictureBox.BackgroundImage = novaImagem;
            pictureBox.BackgroundImageLayout = ImageLayout.Stretch;

            if (imagemAnterior != null)
                imagemAnterior.Dispose();

            pictureBox.Invalidate();
            pictureBox.Refresh();
        }

        private void AtualizarImagensVitoria()
        {
            AtualizarImagem(pbxAtualA, pressaoA, true);
            AtualizarImagem(pbxAtualB, pressaoB, true);
            AtualizarImagem(pbxAtualC, pressaoC, true);
            AtualizarImagem(pbxAtualD, pressaoD, true);

            AtualizarImagem(pbxMetaA, metaA, true);
            AtualizarImagem(pbxMetaB, metaB, true);
            AtualizarImagem(pbxMetaC, metaC, true);
            AtualizarImagem(pbxMetaD, metaD, true);
        }

        #endregion

        #region Verificação de Vitória

        private void VerificarResultado()
        {
            if (pressaoA == metaA &&
                pressaoB == metaB &&
                pressaoC == metaC &&
                pressaoD == metaD)
            {
                concluido = true;
                venceu = true;

                cronometro.Stop();

                pontuacaoFinal = CalcularPontuacao();

                HabilitarValvulas(false);
                HabilitarReiniciar(false);

                AtualizarImagensVitoria();

                timerVitoria.Start();
            }
        }

        #endregion

        #region Sistema de Pontuação

        private int CalcularPontuacao()
        {
            int pontosConclusao = 500;

            int pontosMovimentos = 2500;

            if (movimentosUtilizados > 40)
            {
                pontosMovimentos -= (movimentosUtilizados - 40) * 30;
            }

            pontosMovimentos = Math.Max(0, pontosMovimentos);

            int tempoDecorrido = (int)Math.Ceiling(
                cronometro.Elapsed.TotalSeconds
            );

            int pontosTempo = 2000;

            if (tempoDecorrido > 40)
            {
                pontosTempo -= (tempoDecorrido - 40) * 25;
            }

            pontosTempo = Math.Max(0, pontosTempo);

            int total = pontosConclusao +
                        pontosMovimentos +
                        pontosTempo;

            total = Math.Min(5000, total);

            Console.WriteLine("");
            Console.WriteLine("========== PONTUACAO FINAL ==========");
            Console.WriteLine("MOVIMENTOS: " + movimentosUtilizados);
            Console.WriteLine("TEMPO: " + tempoDecorrido + " SEGUNDOS");
            Console.WriteLine("BONUS DE CONCLUSAO: " + pontosConclusao);
            Console.WriteLine("PONTOS DE MOVIMENTOS: " + pontosMovimentos);
            Console.WriteLine("PONTOS DE TEMPO: " + pontosTempo);
            Console.WriteLine("PONTUACAO TOTAL: " + total);
            Console.WriteLine("=====================================");

            return total;
        }

        #endregion

        #region Animação e Redirecionamento de Vitória

        private void TimerVitoria_Tick(object sender, EventArgs e)
        {
            timerVitoria.Stop();

            AbrirTelaVitoria();
        }

        private void AbrirTelaVitoria()
        {
            JogoForm jogo = EncontrarPai<JogoForm>();

            if (jogo == null)
                throw new Exception("JogoForm não encontrado.");

            jogo.AbrirTela(
                new VitoriaControl(IDENTIFICADOR, pontuacaoFinal)
            );
        }

        #endregion

        #region Habilitação dos Botões

        private void HabilitarValvulas(bool habilitar)
        {
            PictureBox[] botoes =
            {
                pbxRosa,
                pbxVermelha,
                pbxVerde,
                pbxAmarela,
                pbxBranca,
                pbxRoxa
            };

            foreach (PictureBox botao in botoes)
            {
                botao.Enabled = habilitar;
                botao.Cursor = habilitar
                    ? Cursors.Hand
                    : Cursors.No;
            }
        }

        private void HabilitarReiniciar(bool habilitar)
        {
            pbxReiniciar.Enabled = habilitar;

            pbxReiniciar.Cursor = habilitar
                ? Cursors.Hand
                : Cursors.No;
        }

        #endregion

        #region Reiniciar Jogo

        private void ReiniciarJogo()
        {
            if (concluido)
                return;

            movimentosUtilizados = 0;
            pontuacaoFinal = 0;

            concluido = false;
            venceu = false;

            timerVitoria.Stop();

            cronometro.Reset();

            GerarConfiguracaoAleatoria();

            HabilitarValvulas(true);
            HabilitarReiniciar(true);

            cronometro.Start();

            AtualizarInterface();
        }

        #endregion

        #region Lixeira

        private T EncontrarPai<T>() where T : Control
        {
            Control atual = Parent;

            while (atual != null)
            {
                T resultado = atual as T;

                if (resultado != null)
                    return resultado;

                atual = atual.Parent;
            }

            return null;
        }

        private T EncontrarControleRecursivo<T>(Control controle)
            where T : Control
        {
            if (controle == null)
                return null;

            T resultado = controle as T;

            if (resultado != null)
                return resultado;

            foreach (Control filho in controle.Controls)
            {
                resultado = EncontrarControleRecursivo<T>(filho);

                if (resultado != null)
                    return resultado;
            }

            return null;
        }

        #endregion
    }
}