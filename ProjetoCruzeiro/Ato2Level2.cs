
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using System.Windows.Forms;

namespace ProjetoCruzeiro
{
    public partial class Ato2Level2 : UserControl
    {
        #region Variáveis Globais

        Random random = new Random();

        int[] codigo = new int[4];
        int[] tentativa = new int[4];

        int posicaoAtual = 0;
        int tentativas = 0;

        private const int IDENTIFICADOR_NIVEL = 5;

        private Stopwatch cronometro = new Stopwatch();

        private List<EfeitoBotao> efeitos = new List<EfeitoBotao>();

        private Image pistaPreta;
        private Image pistaBranca;

        private Timer timerFinalizacao;

        private bool finalizacaoIniciada = false;
        private bool vitoriaPendente = false;

        private int pontuacaoPendente = 0;
        private string mensagemDerrota = "";

        #endregion

        public Ato2Level2()
        {
            InitializeComponent();

            #region Double Buffering

            this.DoubleBuffered = true;

            this.SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.UserPaint |
                ControlStyles.ResizeRedraw,
                true
            );

            this.UpdateStyles();

            HabilitarDoubleBuffering(this);

            #endregion

            #region Timer de finalização

            timerFinalizacao = new Timer();
            timerFinalizacao.Interval = 2000;
            timerFinalizacao.Tick += TimerFinalizacao_Tick;

            #endregion

            #region Animação dos botões

            efeitos.Add(new EfeitoBotao(btn_voltar));
            efeitos.Add(new EfeitoBotao(btn_confirmar));

            #endregion

            #region Hover das bebidas

            ConfigurarHoverBebida(pbx_rosa, "rosa");
            ConfigurarHoverBebida(pbx_azul, "azul");
            ConfigurarHoverBebida(pbx_amarelo, "amarelo");
            ConfigurarHoverBebida(pbx_roxo, "roxo");
            ConfigurarHoverBebida(pbx_laranja, "laranja");
            ConfigurarHoverBebida(pbx_verde, "verde");

            #endregion

            #region Carregar pistas

            pistaPreta = CarregarImagem("pistaPreta.png");
            pistaBranca = CarregarImagem("pistaBranca.png");

            #endregion

            #region Gerar código aleatório

            List<int> cores = new List<int>
            {
                0, 1, 2, 3, 4, 5
            };

            for (int i = 0; i < codigo.Length; i++)
            {
                int posicao = random.Next(cores.Count);

                codigo[i] = cores[posicao];

                cores.RemoveAt(posicao);

                Console.WriteLine($"Código gerado: {codigo[i]}");
            }

            #endregion

            #region Esconder tentativas

            for (int i = 1; i <= 4; i++)
            {
                for (int j = 1; j <= 8; j++)
                {
                    PictureBox pbx = (PictureBox)this.Controls.Find(
                        $"pbx_tentativa{j}_casa{i}", true
                    )[0];

                    pbx.Visible = false;
                }
            }

            #endregion

            #region Iniciar cronômetro

            cronometro.Start();

            #endregion
        }

        #region Double Buffering

        private void HabilitarDoubleBuffering(Control controle)
        {
            PropertyInfo propriedade = typeof(Control).GetProperty(
                "DoubleBuffered",
                BindingFlags.Instance | BindingFlags.NonPublic
            );

            if (propriedade != null)
            {
                propriedade.SetValue(controle, true, null);
            }

            foreach (Control filho in controle.Controls)
            {
                HabilitarDoubleBuffering(filho);
            }
        }

        #endregion

        #region Carregar imagens

        private Image CarregarImagem(string nome)
        {
            string caminho = Path.Combine(
                Application.StartupPath,
                "imagens",
                "Level5",
                nome
            );

            using (Image imagem = Image.FromFile(caminho))
            {
                return new Bitmap(imagem);
            }
        }

        #endregion

        #region Hover das bebidas

        private void ConfigurarHoverBebida(PictureBox pbx, string cor)
        {
            Image imagemNormal = CarregarImagem(
                $"bebida_{cor}1.png"
            );

            Image imagemHover = CarregarImagem(
                $"bebida_{cor}2.png"
            );

            pbx.Cursor = Cursors.Hand;

            pbx.BackgroundImage = imagemNormal;

            pbx.MouseEnter += (s, e) =>
            {
                pbx.BackgroundImage = imagemHover;
            };

            pbx.MouseLeave += (s, e) =>
            {
                pbx.BackgroundImage = imagemNormal;
            };
        }

        #endregion

        #region Selecionar cores

        private void btn_rosa_Click(object sender, EventArgs e)
        {
            SelecionarCor(0);
        }

        private void btn_azul_Click(object sender, EventArgs e)
        {
            SelecionarCor(1);
        }

        private void btn_amarelo_Click(object sender, EventArgs e)
        {
            SelecionarCor(2);
        }

        private void btn_roxo_Click(object sender, EventArgs e)
        {
            SelecionarCor(3);
        }

        private void btn_laranja_Click(object sender, EventArgs e)
        {
            SelecionarCor(4);
        }

        private void btn_verde_Click(object sender, EventArgs e)
        {
            SelecionarCor(5);
        }

        #endregion

        #region Selecionar cor

        private void SelecionarCor(int cor)
        {
            if (finalizacaoIniciada)
                return;

            if (posicaoAtual >= tentativa.Length)
            {
                MessageBox.Show(
                    "Você já selecionou 4 cores! Clique em Verificar para conferir sua tentativa."
                );

                return;
            }

            Color botao = BackColor;

            switch (cor)
            {
                case 0:
                    botao = Color.FromArgb(243, 145, 162);
                    break;

                case 1:
                    botao = Color.FromArgb(130, 213, 229);
                    break;

                case 2:
                    botao = Color.FromArgb(240, 193, 72);
                    break;

                case 3:
                    botao = Color.FromArgb(128, 0, 128);
                    break;

                case 4:
                    botao = Color.FromArgb(195, 73, 0);
                    break;

                case 5:
                    botao = Color.FromArgb(50, 163, 45);
                    break;
            }

            PictureBox pbx = (PictureBox)this.Controls.Find(
                $"pbx_tentativa0_casa{posicaoAtual + 1}", true
            )[0];

            PictureBox pbx2 = (PictureBox)this.Controls.Find(
                $"pbx_tentativa{tentativas + 1}_casa{posicaoAtual + 1}", true
            )[0];

            pbx.BackColor = botao;
            pbx2.BackColor = botao;

            tentativa[posicaoAtual] = cor;

            posicaoAtual++;
        }

        #endregion

        #region Sistema de pontuação

        private int CalcularPontuacao()
        {
            int pontuacaoConclusao = 500;

            int pontuacaoTentativas = Math.Max(
                0,
                3000 - (Math.Max(2, tentativas) - 2) * 500
            );

            int segundos = (int)Math.Floor(
                cronometro.Elapsed.TotalSeconds
            );

            int pontuacaoTempo = Math.Max(
                0,
                1500 - Math.Max(0, segundos - 40) * 50
            );

            int pontuacaoFinal =
                pontuacaoConclusao +
                pontuacaoTentativas +
                pontuacaoTempo;

            return pontuacaoFinal;
        }

        #endregion

        #region Localizar controles

        private JogoForm ObterJogoForm()
        {
            Control atual = this;

            while (atual != null)
            {
                JogoForm jogo = atual as JogoForm;

                if (jogo != null)
                    return jogo;

                atual = atual.Parent;
            }

            throw new InvalidOperationException(
                "Não foi possível localizar o JogoForm."
            );
        }

        private MenuLateralControl EncontrarMenuLateral(Control raiz)
        {
            MenuLateralControl menu = raiz as MenuLateralControl;

            if (menu != null)
                return menu;

            foreach (Control filho in raiz.Controls)
            {
                menu = EncontrarMenuLateral(filho);

                if (menu != null)
                    return menu;
            }

            return null;
        }

        #endregion

        #region Bloquear controles

        private void BloquearControles()
        {
            btn_voltar.Enabled = false;
            btn_confirmar.Enabled = false;

            btn_voltar.Cursor = Cursors.No;
            btn_confirmar.Cursor = Cursors.No;

            pbx_rosa.Enabled = false;
            pbx_azul.Enabled = false;
            pbx_amarelo.Enabled = false;
            pbx_roxo.Enabled = false;
            pbx_laranja.Enabled = false;
            pbx_verde.Enabled = false;

            pbx_rosa.Cursor = Cursors.No;
            pbx_azul.Cursor = Cursors.No;
            pbx_amarelo.Cursor = Cursors.No;
            pbx_roxo.Cursor = Cursors.No;
            pbx_laranja.Cursor = Cursors.No;
            pbx_verde.Cursor = Cursors.No;

            
        }

        #endregion

        #region Finalização com espera

        private void PrepararFinalizacao(
            bool venceu,
            int pontuacao = 0,
            string mensagem = ""
        )
        {
            if (finalizacaoIniciada)
                return;

            finalizacaoIniciada = true;

            cronometro.Stop();

            vitoriaPendente = venceu;

            pontuacaoPendente = pontuacao;

            mensagemDerrota = mensagem;

            BloquearControles();

            timerFinalizacao.Start();
        }

        private void TimerFinalizacao_Tick(object sender, EventArgs e)
        {
            timerFinalizacao.Stop();

            timerFinalizacao.Dispose();

            JogoForm jogo = ObterJogoForm();

            if (vitoriaPendente)
            {
                jogo.AbrirTela(
                    new VitoriaControl(
                        IDENTIFICADOR_NIVEL,
                        pontuacaoPendente
                    )
                );
            }
            else
            {
                jogo.AbrirTela(
                    new DerrotaControl(
                        IDENTIFICADOR_NIVEL,
                        mensagemDerrota
                    )
                );
            }
        }

        #endregion

        #region Pistas

        private void MostrarPistas(int certas, int foraDeLugar)
        {
            List<Image> pistas = new List<Image>();

            for (int i = 0; i < certas; i++)
            {
                pistas.Add(pistaPreta);
            }

            for (int i = 0; i < foraDeLugar; i++)
            {
                pistas.Add(pistaBranca);
            }

            PictureBox[] casas = new PictureBox[4];

            for (int i = 0; i < 4; i++)
            {
                casas[i] = (PictureBox)this.Controls.Find(
                    $"pbx_pista{tentativas + 1}_{i + 1}", true
                )[0];
            }

            for (int i = 0; i < 4; i++)
            {
                casas[i].BackgroundImage = null;
            }

            for (int i = 0; i < pistas.Count; i++)
            {
                casas[i].BackgroundImage = pistas[i];
            }
        }

        #endregion

        #region Verificar tentativa

        private void btn_confirmar_Click(object sender, EventArgs e)
        {
            if (finalizacaoIniciada)
                return;

            if (posicaoAtual < tentativa.Length)
            {
                MessageBox.Show(
                    "Você precisa selecionar 4 cores antes de verificar sua tentativa."
                );

                return;
            }

            int certas = 0;
            int foraDeLugar = 0;

            int[] codigoRestante = (int[])codigo.Clone();
            int[] tentativaRestante = (int[])tentativa.Clone();

            for (int i = 0; i < codigo.Length; i++)
            {
                if (tentativaRestante[i] == codigoRestante[i])
                {
                    certas++;

                    codigoRestante[i] = -1;
                    tentativaRestante[i] = -1;
                }
            }

            for (int i = 0; i < tentativaRestante.Length; i++)
            {
                if (tentativaRestante[i] == -1)
                    continue;

                for (int j = 0; j < codigoRestante.Length; j++)
                {
                    if (tentativaRestante[i] != -1 &&
                        tentativaRestante[i] == codigoRestante[j])
                    {
                        foraDeLugar++;

                        codigoRestante[j] = -1;

                        break;
                    }
                }
            }

            MostrarPistas(certas, foraDeLugar);

            for (int i = 1; i <= 4; i++)
            {
                PictureBox pbx = (PictureBox)this.Controls.Find(
                    $"pbx_tentativa{tentativas + 1}_casa{i}", true
                )[0];

                pbx.Visible = true;
            }

            if (certas == 4)
            {
                tentativas++;

                int pontuacaoFinal = CalcularPontuacao();

                PrepararFinalizacao(
                    true,
                    pontuacaoFinal
                );

                return;
            }

            tentativas++;

            if (tentativas >= 8)
            {
                PrepararFinalizacao(
                    false,
                    0,
                    "Você atingiu o limite de tentativas."
                );

                return;
            }

            Array.Clear(tentativa, 0, tentativa.Length);

            posicaoAtual = 0;

            for (int i = 1; i <= 4; i++)
            {
                PictureBox pbx = (PictureBox)this.Controls.Find(
                    $"pbx_tentativa0_casa{i}", true
                )[0];

                pbx.BackColor = Color.Transparent;
            }
        }

        #endregion

        #region Voltar

        private void btn_voltar_Click(object sender, EventArgs e)
        {
            if (finalizacaoIniciada)
                return;

            if (posicaoAtual <= 0)
                return;

            PictureBox pbx = (PictureBox)this.Controls.Find(
                $"pbx_tentativa0_casa{posicaoAtual}", true
            )[0];

            PictureBox pbx2 = (PictureBox)this.Controls.Find(
                $"pbx_tentativa{tentativas + 1}_casa{posicaoAtual}", true
            )[0];

            pbx.BackColor = Color.Transparent;
            pbx2.BackColor = Color.Transparent;

            tentativa[posicaoAtual - 1] = 0;

            posicaoAtual--;
        }

        #endregion

        #region Lixeira

        private void pbx_tentativa0_casa3_Click(object sender, EventArgs e)
        {

        }

        #endregion
    }
}
