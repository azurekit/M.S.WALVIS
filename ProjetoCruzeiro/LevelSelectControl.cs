using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace ProjetoCruzeiro
{
    public partial class LevelSelectControl : UserControl
    {
        public int DificuldadeSelecionada;

        private Timer timerPontuacao;
        private Stopwatch cronometroPontuacao;

        private int pontuacaoAtual;
        private int pontuacaoFinal;

        private const int DURACAO_ANIMACAO = 400;

        private List<EfeitoBotao> efeitos = new List<EfeitoBotao>();

        public LevelSelectControl(int dificuldadeSelecionada)
        {
            InitializeComponent();

            DificuldadeSelecionada = dificuldadeSelecionada;

            ConfigurarLabelPontuacao();
            ConfigurarBotoes();
            ConfigurarAnimacao();
            ConfigurarAto();
            EsconderInfoLevels();
        }

        #region Configuração

        private void ConfigurarLabelPontuacao()
        {
            lbl_level_highscore.AutoSize = false;
            lbl_level_highscore.TextAlign = ContentAlignment.MiddleCenter;
        }

        private void ConfigurarBotoes()
        {
            efeitos.Add(new EfeitoBotao(pbx_level1_button));
            efeitos.Add(new EfeitoBotao(pbx_level2_button));
            efeitos.Add(new EfeitoBotao(pbx_level3_button));

            pbx_level1_button.Cursor = Cursors.Hand;
            pbx_level2_button.Cursor = Cursors.Hand;
            pbx_level3_button.Cursor = Cursors.Hand;

            pbx_level1_button.MouseEnter += Level1_MouseEnter;
            pbx_level2_button.MouseEnter += Level2_MouseEnter;
            pbx_level3_button.MouseEnter += Level3_MouseEnter;

            pbx_level1_button.MouseLeave += Level_MouseLeave;
            pbx_level2_button.MouseLeave += Level_MouseLeave;
            pbx_level3_button.MouseLeave += Level_MouseLeave;

            pbx_level1_button.Click += lbl_level1_button_Click;
            pbx_level2_button.Click += lbl_level2_button_Click;
            pbx_level3_button.Click += lbl_level3_button_Click;
        }

        private void ConfigurarAnimacao()
        {
            cronometroPontuacao = new Stopwatch();

            timerPontuacao = new Timer();
            timerPontuacao.Interval = 15;
            timerPontuacao.Tick += TimerPontuacao_Tick;
        }

        private Image CarregarImagem(string nome)
        {
            string caminho = Path.Combine(
                Application.StartupPath,
                "imagens",
                nome
            );

            if (!File.Exists(caminho))
                return null;

            using (Image imagem = Image.FromFile(caminho))
            {
                return new Bitmap(imagem);
            }
        }

        private void ConfigurarAto()
        {
            switch (DificuldadeSelecionada)
            {
                case 1:
                    lbl_caso.Text = "ATO 1";
                    ConfigurarBackgroundAto(1);
                    break;

                case 2:
                    lbl_caso.Text = "ATO 2";
                    ConfigurarBackgroundAto(4);
                    break;

                case 3:
                    lbl_caso.Text = "ATO 3";
                    ConfigurarBackgroundAto(7);
                    break;
            }
        }

        private void ConfigurarBackgroundAto(int primeiroNivel)
        {
            bool nivel1 = VitoriaControl.EstaConcluido(primeiroNivel);
            bool nivel2 = VitoriaControl.EstaConcluido(primeiroNivel + 1);

            string imagem;

            if (!nivel1)
                imagem = "cruzeiro_dia.png";
            else if (!nivel2)
                imagem = "cruzeiro_tarde.png";
            else
                imagem = "cruzeiro_noite.png";

            BackgroundImage = CarregarImagem(imagem);
            BackgroundImageLayout = ImageLayout.Stretch;
        }

        #endregion

        #region Informações dos níveis

        private void ExibirInfoLevel(int numeroLevel)
        {
            int identificador = ((DificuldadeSelecionada - 1) * 3) + numeroLevel;

            EsconderInfoLevels();

            pnl_painel.BackgroundImage = CarregarImagem(
                $"level{identificador}info.png"
            );

            pnl_painel.BackgroundImageLayout = ImageLayout.Stretch;

            int highscore = SaveManager.ObterHighscore(identificador);

            lbl_level_highscore.Text = "0";

            pbx_estrelas.BackgroundImage = CarregarImagem(
                $"estrelas{ObterQuantidadeEstrelas(highscore)}.png"
            );

            pbx_estrelas.BackgroundImageLayout = ImageLayout.Stretch;

            lbl_level_highscore.Visible = true;
            pbx_estrelas.Visible = true;

            IniciarAnimacaoPontuacao(highscore);
        }

        private void EsconderInfoLevels()
        {
            PararAnimacaoPontuacao();

            pnl_painel.BackgroundImage = null;
            pbx_estrelas.BackgroundImage = null;

            lbl_level_highscore.Visible = false;
            pbx_estrelas.Visible = false;
        }

        #endregion

        #region Animação da pontuação

        private void IniciarAnimacaoPontuacao(int pontuacao)
        {
            PararAnimacaoPontuacao();

            pontuacaoAtual = 0;
            pontuacaoFinal = pontuacao;

            lbl_level_highscore.Text = "0";

            if (pontuacaoFinal <= 0)
                return;

            cronometroPontuacao.Restart();
            timerPontuacao.Start();
        }

        private void TimerPontuacao_Tick(object sender, EventArgs e)
        {
            double progresso = cronometroPontuacao.Elapsed.TotalMilliseconds /
                               DURACAO_ANIMACAO;

            if (progresso >= 1)
            {
                pontuacaoAtual = pontuacaoFinal;

                timerPontuacao.Stop();
                cronometroPontuacao.Stop();
            }
            else
            {
                pontuacaoAtual = (int)(pontuacaoFinal * progresso);
            }

            lbl_level_highscore.Text = $"{pontuacaoAtual:N0}";
        }

        private void PararAnimacaoPontuacao()
        {
            if (timerPontuacao != null)
                timerPontuacao.Stop();

            if (cronometroPontuacao != null)
                cronometroPontuacao.Stop();
        }

        private int ObterQuantidadeEstrelas(int pontuacao)
        {
            if (pontuacao >= 5000)
                return 4;

            if (pontuacao >= 3500)
                return 3;

            if (pontuacao >= 2500)
                return 2;

            if (pontuacao >= 1500)
                return 1;

            return 0;
        }

        #endregion

        #region Mouse dos níveis

        private void Level1_MouseEnter(object sender, EventArgs e)
        {
            ExibirInfoLevel(1);
        }

        private void Level2_MouseEnter(object sender, EventArgs e)
        {
            ExibirInfoLevel(2);
        }

        private void Level3_MouseEnter(object sender, EventArgs e)
        {
            ExibirInfoLevel(3);
        }

        private void Level_MouseLeave(object sender, EventArgs e)
        {
            EsconderInfoLevels();
        }

        #endregion

        #region Botão de voltar

        private void btn_menu_Click(object sender, EventArgs e)
        {
            Cruzeiro cruzeiro = (Cruzeiro)FindForm();
            cruzeiro.AbrirTela(new DificuldadeSelectControl());
        }

        #endregion

        #region Selecionar nível

        private void lbl_level1_button_Click(object sender, EventArgs e)
        {
            Cruzeiro cruzeiro = (Cruzeiro)FindForm();

            cruzeiro.AbrirTela(new JogoForm(DificuldadeSelecionada, 1));
        }

        private void lbl_level2_button_Click(object sender, EventArgs e)
        {
            Cruzeiro cruzeiro = (Cruzeiro)FindForm();

            cruzeiro.AbrirTela(new JogoForm(DificuldadeSelecionada, 2));
        }

        private void lbl_level3_button_Click(object sender, EventArgs e)
        {
            Cruzeiro cruzeiro = (Cruzeiro)FindForm();

            cruzeiro.AbrirTela(new JogoForm(DificuldadeSelecionada, 3));
        }

        #endregion

        #region Eventos auxiliares

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);

            if (!Visible)
                PararAnimacaoPontuacao();
        }

        #endregion

        #region lixeira


        private void lbl_caso_Click(object sender, EventArgs e)
        {

        }
        #endregion
    }
}