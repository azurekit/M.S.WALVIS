
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;

namespace ProjetoCruzeiro
{
    public partial class Ato2Level1 : UserControl
    {
        #region Variáveis Globais

        private List<List<int>> suportes = new List<List<int>>();

        private PictureBox[] pratos = new PictureBox[5];
        private Panel[] paineis = new Panel[3];

        private int movimentos = 0;
        private int suporteSelecionado = -1;

        private bool jogoFinalizado = false;

        private Image imagemNormal;
        private Image imagemHover;
        private Image imagemSelecionado;
        private Image imagemVitoria;

        private Stopwatch cronometro = new Stopwatch();

        private int pontuacaoFinal = 0;

        private Timer timerVitoria = new Timer();

        #endregion

        #region Construtor

        public Ato2Level1()
        {
            InitializeComponent();

            paineis[0] = pnl_suporteA;
            paineis[1] = pnl_suporteB;
            paineis[2] = pnl_suporteC;

            pratos[0] = pbx_prato1;
            pratos[1] = pbx_prato2;
            pratos[2] = pbx_prato3;
            pratos[3] = pbx_prato4;
            pratos[4] = pbx_prato5;

            CarregarImagens();
            ConfigurarVisuais();

            timerVitoria.Interval = 2000;
            timerVitoria.Tick += TimerVitoria_Tick;

            iniciarJogo();
        }

        #endregion

        #region Carregamento de imagens

        private void CarregarImagens()
        {
            string pasta = Path.Combine(
                Application.StartupPath,
                "imagens",
                "Level4"
            );

            imagemNormal = Image.FromFile(
                Path.Combine(pasta, "cozinhaarea1.png")
            );

            imagemHover = Image.FromFile(
                Path.Combine(pasta, "cozinhaarea2.png")
            );

            imagemSelecionado = Image.FromFile(
                Path.Combine(pasta, "cozinhaarea3.png")
            );

            imagemVitoria = Image.FromFile(
                Path.Combine(pasta, "cozinhaarea4.png")
            );
        }

        #endregion

        #region Configuração visual

        private void ConfigurarVisuais()
        {
            this.BackColor = Color.Transparent;

            foreach (Panel painel in paineis)
            {
                painel.BackColor = Color.Transparent;
                painel.BackgroundImage = imagemNormal;
                painel.BackgroundImageLayout = ImageLayout.Stretch;

                painel.Click -= Suporte_Click;
                painel.Click += Suporte_Click;

                painel.MouseEnter += Suporte_MouseEnter;
                painel.MouseLeave += Suporte_MouseLeave;
            }

            foreach (PictureBox prato in pratos)
            {
                prato.BackColor = Color.Transparent;
                prato.SizeMode = PictureBoxSizeMode.AutoSize;

                prato.Click -= Suporte_Click;
                prato.Click += Suporte_Click;

                prato.MouseEnter += Suporte_MouseEnter;
                prato.MouseLeave += Suporte_MouseLeave;
            }
        }

        #endregion

        #region Identificação dos suportes

        private Panel ObterPainel(Control controle)
        {
            while (controle != null)
            {
                Panel painel = controle as Panel;

                if (painel != null &&
                    Array.IndexOf(paineis, painel) != -1)
                {
                    return painel;
                }

                controle = controle.Parent;
            }

            return null;
        }

        #endregion

        #region Inicialização do jogo

        private void iniciarJogo()
        {
            movimentos = 0;

            lbl_movimentos2.Text = movimentos.ToString();

            suporteSelecionado = -1;
            jogoFinalizado = false;

            pontuacaoFinal = 0;

            suportes.Clear();

            for (int i = 0; i < 3; i++)
            {
                suportes.Add(new List<int>());
            }

            for (int i = 5; i >= 1; i--)
            {
                suportes[0].Add(i);
            }

            AtualizarImagensSuportes();
            atualizarTabuleiro();

            cronometro.Reset();
            cronometro.Start();
        }

        #endregion

        #region Atualização do tabuleiro

        private void atualizarTabuleiro()
        {
            foreach (PictureBox prato in pratos)
            {
                if (prato.Parent != null)
                {
                    prato.Parent.Controls.Remove(prato);
                }

                prato.Visible = false;
            }

            for (int i = 0; i < 3; i++)
            {
                int altura = 0;

                foreach (int tamanho in suportes[i])
                {
                    PictureBox prato = pratos[tamanho - 1];

                    paineis[i].Controls.Add(prato);

                    prato.Location = new Point(
                        (paineis[i].Width - prato.Width) / 2,
                        paineis[i].Height - altura - prato.Height - 20
                    );

                    prato.Visible = true;
                    prato.BringToFront();

                    altura += prato.Height - 2;
                }
            }
        }

        #endregion

        #region Interação com os suportes

        private void Suporte_Click(object sender, EventArgs e)
        {
            if (jogoFinalizado)
                return;

            Control controle = sender as Control;

            Panel painel = ObterPainel(controle);

            if (painel == null)
                return;

            int destino = Array.IndexOf(paineis, painel);

            if (destino == -1)
                return;

            if (suporteSelecionado == -1)
            {
                if (suportes[destino].Count == 0)
                    return;

                suporteSelecionado = destino;

                AtualizarImagensSuportes();

                return;
            }

            if (suporteSelecionado == destino)
            {
                suporteSelecionado = -1;

                AtualizarImagensSuportes();

                return;
            }

            int origem = suporteSelecionado;

            int pratoMovido =
                suportes[origem][suportes[origem].Count - 1];

            if (suportes[destino].Count > 0)
            {
                int pratoTopo =
                    suportes[destino][suportes[destino].Count - 1];

                if (pratoMovido > pratoTopo)
                {
                    MessageBox.Show(
                        "Não é permitido colocar um prato maior sobre um menor."
                    );

                    suporteSelecionado = -1;

                    AtualizarImagensSuportes();

                    return;
                }
            }

            suportes[origem].RemoveAt(
                suportes[origem].Count - 1
            );

            suportes[destino].Add(pratoMovido);

            movimentos++;

            lbl_movimentos2.Text = movimentos.ToString();

            suporteSelecionado = -1;

            AtualizarImagensSuportes();

            atualizarTabuleiro();

            verificarVitoria();
        }

        #endregion

        #region Atualização visual dos suportes

        private void AtualizarImagemSuporte(int indice)
        {
            if (indice < 0 || indice >= paineis.Length)
                return;

            if (indice == suporteSelecionado)
            {
                paineis[indice].BackgroundImage = imagemSelecionado;
            }
            else if (paineis[indice].ClientRectangle.Contains(
                paineis[indice].PointToClient(Cursor.Position)))
            {
                paineis[indice].BackgroundImage = imagemHover;
            }
            else
            {
                paineis[indice].BackgroundImage = imagemNormal;
            }
        }

        private void AtualizarImagensSuportes()
        {
            for (int i = 0; i < paineis.Length; i++)
            {
                AtualizarImagemSuporte(i);
            }
        }

        private void Suporte_MouseEnter(object sender, EventArgs e)
        {
            if (jogoFinalizado)
                return;

            Control controle = sender as Control;

            Panel painel = ObterPainel(controle);

            if (painel == null)
                return;

            int indice = Array.IndexOf(paineis, painel);

            AtualizarImagemSuporte(indice);
        }

        private void Suporte_MouseLeave(object sender, EventArgs e)
        {
            if (jogoFinalizado)
                return;

            Control controle = sender as Control;

            Panel painel = ObterPainel(controle);

            if (painel == null)
                return;

            int indice = Array.IndexOf(paineis, painel);

            AtualizarImagemSuporte(indice);
        }

        #endregion

        #region Verificação de vitória

        private void verificarVitoria()
        {
            if (suportes[2].Count == 5)
            {
                jogoFinalizado = true;

                // Parar cronômetro.
                cronometro.Stop();

                // Calcular pontuação.
                CalcularPontuacao();

                // Exibir imagem de vitória.
                pnl_suporteC.BackgroundImage = imagemVitoria;

                // Bloquear interação com todos os suportes.
                foreach (Panel painel in paineis)
                {
                    painel.Enabled = false;
                }

                // Bloquear interação com todos os pratos.
                foreach (PictureBox prato in pratos)
                {
                    prato.Enabled = false;
                }

                // Iniciar espera de 2 segundos.
                timerVitoria.Start();
            }
        }

        #endregion

        #region Pontuação

        private void CalcularPontuacao()
        {
            int segundos = (int)Math.Floor(
                cronometro.Elapsed.TotalSeconds
            );

            int bonusConclusao = 500;

            int penalidadeTempo = Math.Max(0, segundos - 40);

            int pontuacaoTempo = Math.Max(
                0,
                2250 - (penalidadeTempo * 50)
            );

            int movimentosExtras = Math.Max(
                0,
                movimentos - 35
            );

            int pontuacaoMovimentos = Math.Max(
                0,
                2250 - (movimentosExtras * 25)
            );

            pontuacaoFinal =
                bonusConclusao +
                pontuacaoTempo +
                pontuacaoMovimentos;
        }

        #endregion

        #region Transição para vitória

        private void TimerVitoria_Tick(object sender, EventArgs e)
        {
            timerVitoria.Stop();

            JogoForm jogo = EncontrarJogoForm();

            if (jogo != null)
            {
                jogo.AbrirTela(
                    new VitoriaControl(4, pontuacaoFinal)
                );
            }
            else
            {
                Cruzeiro cruzeiro =
                    this.FindForm() as Cruzeiro;

                if (cruzeiro != null)
                {
                    cruzeiro.AbrirTela(
                        new VitoriaControl(4, pontuacaoFinal)
                    );
                }
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

        #region Lixeira
        // método que eu usava pra localizar o menu lateral, mas não é mais necessário, pois agora o menu lateral é passado como parâmetro para o construtor do Ato2Level1.
        // ele tá aí pq remover ele pode quebrar o código, então vou deixar por enquanto.
        private MenuLateralControl BuscarMenuLateral(Control pai) 
        {
            foreach (Control controle in pai.Controls)
            {
                MenuLateralControl menu =
                    controle as MenuLateralControl;

                if (menu != null)
                    return menu;

                MenuLateralControl encontrado =
                    BuscarMenuLateral(controle);

                if (encontrado != null)
                    return encontrado;
            }

            return null;
        }

        private void lbl_movimentos2_Click(object sender, EventArgs e)
        {
        }

        private void Suporte_MouseHover(object sender, EventArgs e)
        {
        }

        #endregion
    }
}
