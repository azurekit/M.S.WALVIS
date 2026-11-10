using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace ProjetoCruzeiro
{
    public partial class Ato1Level2 : UserControl
    {
        #region Variáveis Globais

        private int[] dados = new int[3];
        private Random random = new Random();

        private Stopwatch cronometro = new Stopwatch();

        private int tentativas = 0;

        private int pontuacaoTempo = 0;
        private int pontuacaoTentativas = 0;
        private int pontuacaoFinal = 0;

        private int bonusConclusao = 500;

        private TimeSpan tempoDecorrido;

        private bool jogoFinalizado = false;

        private List<Dica> dicasAtuais = new List<Dica>();

        private List<EfeitoBotao> efeitos = new List<EfeitoBotao>();

        private Timer timerVitoria;
        private Timer timerAviso;

        private Point posicaoOriginalCopoa;
        private Point posicaoOriginalCopob;
        private Point posicaoOriginalCopoc;

        private Size tamanhoOriginalCopoa;
        private Size tamanhoOriginalCopob;
        private Size tamanhoOriginalCopoc;

        private Point posicaoOriginalDadoa;
        private Point posicaoOriginalDadob;
        private Point posicaoOriginalDadoc;

        #endregion

        public Ato1Level2()
        {
            InitializeComponent();

            #region Efeitos dos botões

            efeitos.Add(new EfeitoBotao(btn_confirmar));

            btn_confirmar.Cursor = Cursors.Hand;

            #endregion

            nud_dadoA.Cursor = Cursors.Default;
            nud_dadoB.Cursor = Cursors.Default;
            nud_dadoC.Cursor = Cursors.Default;

            posicaoOriginalCopoa = pbx_copoa.Location;
            posicaoOriginalCopob = pbx_copob.Location;
            posicaoOriginalCopoc = pbx_copoc.Location;

            tamanhoOriginalCopoa = pbx_copoa.Size;
            tamanhoOriginalCopob = pbx_copob.Size;
            tamanhoOriginalCopoc = pbx_copoc.Size;

            timerVitoria = new Timer();
            timerVitoria.Interval = 6000;
            timerVitoria.Tick += TimerVitoria_Tick;

            timerAviso = new Timer();
            timerAviso.Interval = 3000;
            timerAviso.Tick += TimerAviso_Tick;

            Disposed += Ato1Level2_Disposed;

            CarregarFundo("ato1level2bg.png");

            VincularDadosAosCopos();

            posicaoOriginalDadoa = pbx_dadoa.Location;
            posicaoOriginalDadob = pbx_dadob.Location;
            posicaoOriginalDadoc = pbx_dadoc.Location;

            IniciarJogo();
        }

        #region Finalização

        private void Ato1Level2_Disposed(object sender, EventArgs e)
        {
            timerVitoria.Stop();
            timerVitoria.Dispose();

            timerAviso.Stop();
            timerAviso.Dispose();

            cronometro.Stop();

            if (BackgroundImage != null)
            {
                BackgroundImage.Dispose();
                BackgroundImage = null;
            }
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

        private void CarregarFundo(string arquivo)
        {
            string caminho = Path.Combine(
                Application.StartupPath,
                "imagens",
                arquivo
            );

            if (!File.Exists(caminho))
                return;

            Image imagemAnterior = BackgroundImage;

            BackgroundImage = CarregarImagem(caminho);
            BackgroundImageLayout = ImageLayout.Stretch;

            if (imagemAnterior != null)
                imagemAnterior.Dispose();

            Invalidate();
        }

        private void DefinirImagem(Control controle, string arquivo)
        {
            string caminho = Path.Combine(
                Application.StartupPath,
                "imagens",
                arquivo
            );

            if (!File.Exists(caminho))
                return;

            Image imagemAnterior = controle.BackgroundImage;

            controle.BackgroundImage = CarregarImagem(caminho);
            controle.BackgroundImageLayout = ImageLayout.Stretch;

            if (imagemAnterior != null)
                imagemAnterior.Dispose();
        }

        #endregion

        #region Vincular dados aos copos

        private void VincularDadosAosCopos()
        {
            VincularDadoAoCopo(pbx_dadoa, pbx_copoa);
            VincularDadoAoCopo(pbx_dadob, pbx_copob);
            VincularDadoAoCopo(pbx_dadoc, pbx_copoc);
        }

        private void VincularDadoAoCopo(PictureBox dado, PictureBox copo)
        {
            Point posicaoNaTela = dado.Parent.PointToScreen(dado.Location);

            Point posicaoNoCopo = copo.PointToClient(posicaoNaTela);

            dado.Parent = copo;

            dado.Location = posicaoNoCopo;

            dado.BackColor = Color.Transparent;

            dado.Visible = false;

            dado.BringToFront();
        }

        #endregion

        #region Configurar imagens iniciais

        private void ConfigurarImagensIniciais()
        {
            DefinirImagem(pbx_copoa, "copoa.png");
            DefinirImagem(pbx_copob, "copob.png");
            DefinirImagem(pbx_copoc, "copoc.png");

            DefinirImagem(pbx_dadoa, "dado" + dados[0] + ".png");
            DefinirImagem(pbx_dadob, "dado" + dados[1] + ".png");
            DefinirImagem(pbx_dadoc, "dado" + dados[2] + ".png");

            pbx_dadoa.Visible = false;
            pbx_dadob.Visible = false;
            pbx_dadoc.Visible = false;
        }

        #endregion

        #region Avisos

        private void MostrarAviso(string mensagem)
        {
            timerAviso.Stop();

            lbl_dica1.Visible = false;
            lbl_dica2.Visible = false;
            lbl_dica3.Visible = false;
            lbl_dica4.Visible = false;

            lbl_aviso.Text = mensagem;
            lbl_aviso.Visible = true;

            timerAviso.Start();
        }

        private void TimerAviso_Tick(object sender, EventArgs e)
        {
            timerAviso.Stop();

            lbl_aviso.Visible = false;

            lbl_dica1.Visible = true;
            lbl_dica2.Visible = true;
            lbl_dica3.Visible = true;
            lbl_dica4.Visible = true;
        }

        private void OcultarAviso()
        {
            timerAviso.Stop();

            lbl_aviso.Visible = false;

            lbl_dica1.Visible = true;
            lbl_dica2.Visible = true;
            lbl_dica3.Visible = true;
            lbl_dica4.Visible = true;
        }

        #endregion

        #region Iniciar jogo

        private void IniciarJogo()
        {
            CarregarFundo("ato1level2bg.png");

            lbl_dica1.Text = "";
            lbl_dica2.Text = "";
            lbl_dica3.Text = "";
            lbl_dica4.Text = "";

            lbl_aviso.Text = "";
            lbl_aviso.Visible = false;

            lbl_dica1.Visible = true;
            lbl_dica2.Visible = true;
            lbl_dica3.Visible = true;
            lbl_dica4.Visible = true;

            nud_dadoA.Value = 1;
            nud_dadoB.Value = 1;
            nud_dadoC.Value = 1;

            nud_dadoA.Enabled = true;
            nud_dadoB.Enabled = true;
            nud_dadoC.Enabled = true;

            btn_confirmar.Enabled = true;

            tentativas = 0;

            pontuacaoTempo = 0;
            pontuacaoTentativas = 0;
            pontuacaoFinal = 0;

            jogoFinalizado = false;

            dicasAtuais.Clear();

            timerVitoria.Stop();
            timerAviso.Stop();

            pbx_copoa.Location = posicaoOriginalCopoa;
            pbx_copob.Location = posicaoOriginalCopob;
            pbx_copoc.Location = posicaoOriginalCopoc;

            pbx_copoa.Size = tamanhoOriginalCopoa;
            pbx_copob.Size = tamanhoOriginalCopob;
            pbx_copoc.Size = tamanhoOriginalCopoc;

            pbx_dadoa.Location = posicaoOriginalDadoa;
            pbx_dadob.Location = posicaoOriginalDadob;
            pbx_dadoc.Location = posicaoOriginalDadoc;

            while (true)
            {
                List<int> valores = new List<int> { 1, 2, 3, 4, 5, 6 };

                for (int i = 0; i < dados.Length; i++)
                {
                    int indice = random.Next(valores.Count);

                    dados[i] = valores[indice];

                    valores.RemoveAt(indice);
                }

                List<Dica> candidatas = GerarDicas(
                    dados[0],
                    dados[1],
                    dados[2]
                );

                candidatas = candidatas
                    .Where(d => d.Regra(dados[0], dados[1], dados[2]))
                    .OrderBy(d => random.Next())
                    .ToList();

                List<Dica> escolhidas = new List<Dica>();

                foreach (Dica dica in candidatas)
                {
                    escolhidas.Add(dica);

                    List<int[]> solucoes = EncontrarSolucoes(escolhidas);

                    if (solucoes.Count == 1)
                    {
                        dicasAtuais = new List<Dica>(escolhidas);
                        break;
                    }
                }

                if (EncontrarSolucoes(dicasAtuais).Count == 1)
                {
                    MostrarDicas();

                    ConfigurarImagensIniciais();

                    cronometro.Restart();

                    Console.WriteLine("========== NOVA COMBINAÇÃO ==========");
                    Console.WriteLine($"Dado A: {dados[0]}");
                    Console.WriteLine($"Dado B: {dados[1]}");
                    Console.WriteLine($"Dado C: {dados[2]}");
                    Console.WriteLine($"Quantidade de dicas: {dicasAtuais.Count}");
                    Console.WriteLine("=====================================");

                    return;
                }
            }
        }

        #endregion

        #region Dicas

        class Dica
        {
            public string Texto { get; set; }

            public Func<int, int, int, bool> Regra { get; set; }

            public Dica(string texto, Func<int, int, int, bool> regra)
            {
                Texto = texto;
                Regra = regra;
            }
        }

        private List<Dica> GerarDicas(int a, int b, int c)
        {
            List<Dica> dicas = new List<Dica>();

            dicas.Add(new Dica("O dado A é par.", (x, y, z) => x % 2 == 0));
            dicas.Add(new Dica("O dado A é ímpar.", (x, y, z) => x % 2 == 1));
            dicas.Add(new Dica("O dado B é par.", (x, y, z) => y % 2 == 0));
            dicas.Add(new Dica("O dado B é ímpar.", (x, y, z) => y % 2 == 1));
            dicas.Add(new Dica("O dado C é par.", (x, y, z) => z % 2 == 0));
            dicas.Add(new Dica("O dado C é ímpar.", (x, y, z) => z % 2 == 1));
            dicas.Add(new Dica("O dado A é maior que B.", (x, y, z) => x > y));
            dicas.Add(new Dica("O dado A é menor que C.", (x, y, z) => x < z));
            dicas.Add(new Dica("O dado B é menor que C.", (x, y, z) => y < z));
            dicas.Add(new Dica("O dado B é maior que C.", (x, y, z) => y > z));
            dicas.Add(new Dica("O dado B é maior que A.", (x, y, z) => y > x));
            dicas.Add(new Dica("A soma de A e C é " + (a + c) + ".", (x, y, z) => x + z == a + c));
            dicas.Add(new Dica("A soma de A e B é " + (a + b) + ".", (x, y, z) => x + y == a + b));
            dicas.Add(new Dica("A soma de B e C é " + (b + c) + ".", (x, y, z) => y + z == b + c));
            dicas.Add(new Dica("A soma dos dados é " + (a + b + c) + ".", (x, y, z) => x + y + z == a + b + c));
            dicas.Add(new Dica("A diferença de A e C é " + Math.Abs(a - c) + ".", (x, y, z) => Math.Abs(x - z) == Math.Abs(a - c)));
            dicas.Add(new Dica("A diferença de A e B é " + Math.Abs(a - b) + ".", (x, y, z) => Math.Abs(x - y) == Math.Abs(a - b)));
            dicas.Add(new Dica("A diferença de B e C é " + Math.Abs(b - c) + ".", (x, y, z) => Math.Abs(y - z) == Math.Abs(b - c)));

            return dicas;
        }

        private bool Combina(int a, int b, int c, List<Dica> dicas)
        {
            foreach (Dica dica in dicas)
            {
                if (!dica.Regra(a, b, c))
                    return false;
            }

            return true;
        }

        private List<int[]> EncontrarSolucoes(List<Dica> dicas)
        {
            List<int[]> solucoes = new List<int[]>();

            for (int a = 1; a <= 6; a++)
            {
                for (int b = 1; b <= 6; b++)
                {
                    for (int c = 1; c <= 6; c++)
                    {
                        if (a == b || a == c || b == c)
                            continue;

                        if (Combina(a, b, c, dicas))
                        {
                            solucoes.Add(new int[] { a, b, c });
                        }
                    }
                }
            }

            return solucoes;
        }

        #endregion

        #region Mostrar dicas

        private void MostrarDicas()
        {
            lbl_dica1.Text = dicasAtuais.Count > 0 ? dicasAtuais[0].Texto : "";
            lbl_dica2.Text = dicasAtuais.Count > 1 ? dicasAtuais[1].Texto : "";
            lbl_dica3.Text = dicasAtuais.Count > 2 ? dicasAtuais[2].Texto : "";
            lbl_dica4.Text = dicasAtuais.Count > 3 ? dicasAtuais[3].Texto : "";
        }

        #endregion

        #region Pontuação

        private void CalcularPontuacao()
        {
            tempoDecorrido = cronometro.Elapsed;

            cronometro.Stop();

            int segundosExcedentes = Math.Max(
                0,
                (int)Math.Ceiling(tempoDecorrido.TotalSeconds) - 20
            );

            pontuacaoTempo = Math.Max(
                0,
                3500 - (segundosExcedentes * 40)
            );

            pontuacaoTentativas = Math.Max(
                0,
                1000 - ((tentativas - 1) * 200)
            );

            pontuacaoFinal =
                bonusConclusao +
                pontuacaoTempo +
                pontuacaoTentativas;

            Console.WriteLine("========== RESULTADO DO NÍVEL ==========");
            Console.WriteLine($"Tentativas: {tentativas}");
            Console.WriteLine($"Tempo: {tempoDecorrido.TotalSeconds:F1} segundos");
            Console.WriteLine($"Bônus de conclusão: {bonusConclusao}");
            Console.WriteLine($"Pontuação por tempo: {pontuacaoTempo}");
            Console.WriteLine($"Pontuação por tentativas: {pontuacaoTentativas}");
            Console.WriteLine($"Pontuação final: {pontuacaoFinal}");
            Console.WriteLine("========================================");
        }

        #endregion

        #region Vitória

        private void VerificarVitoria()
        {
            if (jogoFinalizado)
                return;

            jogoFinalizado = true;

            OcultarAviso();

            lbl_dica1.Visible = false;
            lbl_dica2.Visible = false;
            lbl_dica3.Visible = false;
            lbl_dica4.Visible = false;

            lbl_aviso.Visible = false;

            btn_confirmar.Enabled = false;

            nud_dadoA.Enabled = false;
            nud_dadoB.Enabled = false;
            nud_dadoC.Enabled = false;

            CarregarFundo("ato1level2bg2.png");

            CalcularPontuacao();

            DefinirImagem(pbx_copoa, "copo_a2.png");
            DefinirImagem(pbx_copob, "copo_b2.png");
            DefinirImagem(pbx_copoc, "copo_c2.png");

            pbx_copoa.Location = new Point(
                posicaoOriginalCopoa.X,
                posicaoOriginalCopoa.Y - 30
            );

            pbx_copob.Location = new Point(
                posicaoOriginalCopob.X,
                posicaoOriginalCopob.Y - 30
            );

            pbx_copoc.Location = new Point(
                posicaoOriginalCopoc.X,
                posicaoOriginalCopoc.Y - 30
            );

            pbx_copoa.Size = new Size(
                tamanhoOriginalCopoa.Width,
                tamanhoOriginalCopoa.Height + 30
            );

            pbx_copob.Size = new Size(
                tamanhoOriginalCopob.Width,
                tamanhoOriginalCopob.Height + 30
            );

            pbx_copoc.Size = new Size(
                tamanhoOriginalCopoc.Width,
                tamanhoOriginalCopoc.Height + 30
            );

            pbx_dadoa.Location = new Point(
                posicaoOriginalDadoa.X,
                posicaoOriginalDadoa.Y + 30
            );

            pbx_dadob.Location = new Point(
                posicaoOriginalDadob.X,
                posicaoOriginalDadob.Y + 30
            );

            pbx_dadoc.Location = new Point(
                posicaoOriginalDadoc.X,
                posicaoOriginalDadoc.Y + 30
            );

            pbx_dadoa.Visible = true;
            pbx_dadob.Visible = true;
            pbx_dadoc.Visible = true;

            pbx_dadoa.BringToFront();
            pbx_dadob.BringToFront();
            pbx_dadoc.BringToFront();

            timerVitoria.Start();
        }


        private void TimerVitoria_Tick(object sender, EventArgs e)
        {
            timerVitoria.Stop();

            Control controle = this;

            while (controle != null && !(controle is JogoForm))
            {
                controle = controle.Parent;
            }

            JogoForm jogo = controle as JogoForm;

            if (jogo != null)
            {
                jogo.MostrarVitoria(2, pontuacaoFinal);
            }
        }


        #endregion

        #region Botão de confirmar

        private void btn_confirmar_Click(object sender, EventArgs e)
        {
            if (jogoFinalizado)
                return;

            int a = Convert.ToInt32(nud_dadoA.Value);
            int b = Convert.ToInt32(nud_dadoB.Value);
            int c = Convert.ToInt32(nud_dadoC.Value);

            if (a == b || a == c || b == c)
            {
                tentativas++;

                MostrarAviso("Os dados não podem repetir valores!");
                return;
            }

            tentativas++;

            if (a == dados[0] && b == dados[1] && c == dados[2])
            {
                VerificarVitoria();
            }
            else
            {
                MostrarAviso("Combinação incorreta!");
            }
        }

        #endregion

        #region Lixeira

        private void lbl_dica3_Click(object sender, EventArgs e)
        {

        }

        private void pbx_copoa_Click(object sender, EventArgs e)
        {

        }

        #endregion
    }
}