using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Windows.Forms;

namespace ProjetoCruzeiro
{
    public partial class Ato3Level3 : UserControl
    {
        #region Constantes

        private const int IDENTIFICADOR_NIVEL_CONVES = 9;

        private const int TEMPO_MAXIMO_CONVES = 17;
        private const int INTERVALO_CRONOMETRO_CONVES = 100;
        private const int TOTAL_PERGUNTAS_CONVES = 7;
        private const int DURACAO_ANIMACAO_CONVES = 3000;
        private const int DURACAO_TRANSICAO_CONVES = 3000;

        private const int BONUS_CONCLUSAO_CONVES = 500;
        private const int BONUS_VIDAS_CONVES = 2500;
        private const int BONUS_TEMPO_CONVES = 2000;
        private const int DESCONTO_SEGUNDO_CONVES = 30;
        private const int TEMPO_LIMITE_BONUS_CONVES = 60;

        private const string ARQUIVO_CAIXA_NORMAL = "caixa_resposta.png";
        private const string ARQUIVO_CAIXA_ACERTO = "caixa_resposta2.png";
        private const string ARQUIVO_CAIXA_ERRO = "caixa_resposta3.png";

        #endregion

        #region Classes

        private class PerguntaConves
        {
            public string EnunciadoConves;
            public string[] OpcoesConves;
            public int RespostaCorretaConves;

            public PerguntaConves(
                string enunciado,
                string[] opcoes,
                int respostaCorreta)
            {
                EnunciadoConves = enunciado;
                OpcoesConves = opcoes;
                RespostaCorretaConves = respostaCorreta;
            }
        }

        #endregion

        #region Variáveis

        private readonly Random aleatorioConves = new Random();

        private readonly List<PerguntaConves> perguntasConves =
            new List<PerguntaConves>();

        private readonly List<EfeitoBotao> efeitosConves =
            new List<EfeitoBotao>();

        private PictureBox[] opcoesConves;
        private PictureBox[] vidasVisuaisConves;

        private Image imagemGioIdleConves;
        private Image imagemGioAcertoConves;
        private Image imagemGioErroConves;

        private Image imagemCaixaNormalConves;
        private Image imagemCaixaAcertoConves;
        private Image imagemCaixaErroConves;

        private Image imagemCoracaoConves;

        private Image imagemFundoConves;
        private MemoryStream streamFundoConves;

        private int perguntaAtualConves;
        private int vidasConves = 3;
        private int errosConves;
        private int tempoRestanteConves;
        private int pontuacaoFinalConves;

        private bool respondendoConves;
        private bool dueloEncerradoConves;

        private Timer timerAnimacaoConves;
        private Timer timerTempoConves;
        private Timer timerTransicaoConves;

        private readonly Stopwatch cronometroGeralConves =
            new Stopwatch();

        public event Action<bool> DueloFinalizadoConves;

        #endregion

        #region Remover borda da ProgressBar

        private const int GWL_STYLE = -16;
        private const int GWL_EXSTYLE = -20;

        private const int WS_BORDER = 0x00800000;
        private const int WS_EX_CLIENTEDGE = 0x00000200;

        private const uint SWP_NOSIZE = 0x0001;
        private const uint SWP_NOMOVE = 0x0002;
        private const uint SWP_NOZORDER = 0x0004;
        private const uint SWP_FRAMECHANGED = 0x0020;

        [DllImport("user32.dll", EntryPoint = "GetWindowLong")]
        private static extern int GetWindowLong(
            IntPtr hWnd,
            int nIndex);

        [DllImport("user32.dll", EntryPoint = "SetWindowLong")]
        private static extern int SetWindowLong(
            IntPtr hWnd,
            int nIndex,
            int dwNewLong);

        [DllImport("user32.dll")]
        private static extern bool SetWindowPos(
            IntPtr hWnd,
            IntPtr hWndInsertAfter,
            int X,
            int Y,
            int cx,
            int cy,
            uint uFlags);

        private void RemoverBordaTempoConves()
        {
            if (!prg_tempo.IsHandleCreated)
                return;

            int estilo = GetWindowLong(
                prg_tempo.Handle,
                GWL_STYLE);

            SetWindowLong(
                prg_tempo.Handle,
                GWL_STYLE,
                estilo & ~WS_BORDER);

            int estiloEx = GetWindowLong(
                prg_tempo.Handle,
                GWL_EXSTYLE);

            SetWindowLong(
                prg_tempo.Handle,
                GWL_EXSTYLE,
                estiloEx & ~WS_EX_CLIENTEDGE);

            SetWindowPos(
                prg_tempo.Handle,
                IntPtr.Zero,
                0,
                0,
                0,
                0,
                SWP_NOMOVE |
                SWP_NOSIZE |
                SWP_NOZORDER |
                SWP_FRAMECHANGED);
        }

        #endregion

        #region Inicialização

        public Ato3Level3()
        {
            InitializeComponent();

            ConfigurarDoubleBufferingConves();

            ConfigurarControlesConves();

            CarregarRecursosConves();

            ConfigurarEventosConves();

            CriarTimersConves();

            ConfigurarBarraTempoConves();

            PrepararPartidaConves();

            Disposed += (s, e) =>
            {
                LiberarRecursosConves();
            };
        }

        private void ConfigurarDoubleBufferingConves()
        {
            this.DoubleBuffered = true;

            this.SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.UserPaint |
                ControlStyles.ResizeRedraw,
                true);

            this.UpdateStyles();

            HabilitarDoubleBufferingConves(this);
        }

        private void HabilitarDoubleBufferingConves(Control controle)
        {
            PropertyInfo propriedade = typeof(Control).GetProperty(
                "DoubleBuffered",
                BindingFlags.Instance | BindingFlags.NonPublic);

            if (propriedade != null)
            {
                propriedade.SetValue(controle, true, null);
            }

            foreach (Control filho in controle.Controls)
            {
                HabilitarDoubleBufferingConves(filho);
            }
        }

        private void ConfigurarControlesConves()
        {
            opcoesConves = new PictureBox[]
            {
                pbx_opcaoA,
                pbx_opcaoB,
                pbx_opcaoC,
                pbx_opcaoD
            };

            vidasVisuaisConves = new PictureBox[]
            {
                pbx_vida1,
                pbx_vida2,
                pbx_vida3
            };

            foreach (PictureBox opcao in opcoesConves)
            {
                opcao.Cursor = Cursors.Hand;
                opcao.BackColor = Color.Transparent;

                opcao.BackgroundImageLayout =
                    ImageLayout.Stretch;

                opcao.Tag = "";

                opcao.Paint += DesenharTextoOpcaoConves;

                efeitosConves.Add(
                    new EfeitoBotao(opcao));
            }

            foreach (PictureBox vida in vidasVisuaisConves)
            {
                vida.BackColor = Color.Transparent;

                vida.BackgroundImageLayout =
                    ImageLayout.Stretch;
            }

            pbx_gio.BackColor = Color.Transparent;

            pbx_gio.BackgroundImageLayout =
                ImageLayout.Stretch;

            pbx_fundo.SizeMode =
                PictureBoxSizeMode.StretchImage;

            ConfigurarTransparenciasConves();
        }

        private void DesenharTextoOpcaoConves(
            object sender,
            PaintEventArgs e)
        {
            PictureBox opcao = sender as PictureBox;

            if (opcao == null)
                return;

            string texto = opcao.Tag as string;

            if (string.IsNullOrEmpty(texto))
                return;

            using (Font fonte = new Font(
                "Times New Roman",
                9.75f,
                FontStyle.Regular))
            using (Brush pincel = new SolidBrush(Color.White))
            using (StringFormat formato = new StringFormat())
            {
                formato.Alignment = StringAlignment.Center;
                formato.LineAlignment = StringAlignment.Center;
                formato.Trimming = StringTrimming.EllipsisWord;
                formato.FormatFlags = StringFormatFlags.LineLimit;

                RectangleF areaTexto = new RectangleF(
                    5,
                    5,
                    opcao.ClientSize.Width - 10,
                    opcao.ClientSize.Height - 10);

                e.Graphics.DrawString(
                    texto,
                    fonte,
                    pincel,
                    areaTexto,
                    formato);
            }
        }

        private void ConfigurarTransparenciasConves()
        {
            DefinirPaiTransparenteConves(pnl_painel, pbx_fundo);

            DefinirPaiTransparenteConves(pbx_vida1, pbx_fundo);
            DefinirPaiTransparenteConves(pbx_vida2, pbx_fundo);
            DefinirPaiTransparenteConves(pbx_vida3, pbx_fundo);
            DefinirPaiTransparenteConves(pbx_gio, pbx_fundo);

            DefinirPaiTransparenteConves(pbx_opcaoA, pnl_painel);
            DefinirPaiTransparenteConves(pbx_opcaoB, pnl_painel);
            DefinirPaiTransparenteConves(pbx_opcaoC, pnl_painel);
            DefinirPaiTransparenteConves(pbx_opcaoD, pnl_painel);

            DefinirPaiTransparenteConves(lbl_pergunta, pnl_painel);
            DefinirPaiTransparenteConves(lbl_numero, pnl_painel);
            DefinirPaiTransparenteConves(prg_tempo, pnl_painel);

            pnl_painel.BackColor = Color.Transparent;

            foreach (Control controle in pnl_painel.Controls)
            {
                if (!(controle is ProgressBar))
                    controle.BackColor = Color.Transparent;
            }

            pbx_gio.BackColor = Color.Transparent;

            pbx_vida1.BackColor = Color.Transparent;
            pbx_vida2.BackColor = Color.Transparent;
            pbx_vida3.BackColor = Color.Transparent;

            pbx_gio.SendToBack();

            pnl_painel.BringToFront();

            pbx_vida1.BringToFront();
            pbx_vida2.BringToFront();
            pbx_vida3.BringToFront();
            
            pbx_fundo.SendToBack();
        }

        private void DefinirPaiTransparenteConves(
            Control controle,
            Control novoPai)
        {
            if (controle.Parent == novoPai)
            {
                if (!(controle is ProgressBar))
                    controle.BackColor = Color.Transparent;

                return;
            }

            Point posicaoTela =
                controle.Parent.PointToScreen(controle.Location);

            Point novaPosicao =
                novoPai.PointToClient(posicaoTela);

            controle.Parent = novoPai;

            controle.Location = novaPosicao;

            if (!(controle is ProgressBar))
                controle.BackColor = Color.Transparent;
        }

        private void ConfigurarEventosConves()
        {
            for (int i = 0; i < opcoesConves.Length; i++)
            {
                int indice = i;

                opcoesConves[i].Click +=
                    (s, e) => ResponderConves(indice);
            }
        }

        private void CriarTimersConves()
        {
            timerAnimacaoConves = new Timer();

            timerAnimacaoConves.Tick +=
                FinalizarAnimacaoConves;

            timerTempoConves = new Timer();

            timerTempoConves.Interval =
                INTERVALO_CRONOMETRO_CONVES;

            timerTempoConves.Tick +=
                AtualizarTempoConves;

            timerTransicaoConves = new Timer();

            timerTransicaoConves.Interval =
                DURACAO_TRANSICAO_CONVES;

            timerTransicaoConves.Tick +=
                ExecutarTransicaoConves;
        }

        private void ConfigurarBarraTempoConves()
        {
            prg_tempo.Minimum = 0;

            prg_tempo.Maximum =
                TEMPO_MAXIMO_CONVES * 10;

            prg_tempo.Value =
                prg_tempo.Maximum;

            prg_tempo.Style =
                ProgressBarStyle.Continuous;

            prg_tempo.RightToLeft =
                RightToLeft.No;

            prg_tempo.RightToLeftLayout = false;

            RemoverBordaTempoConves();
        }

        #endregion

        #region Carregamento de recursos

        private string ObterCaminhoConves(string arquivo)
        {
            return Path.Combine(
                Application.StartupPath,
                "imagens",
                "Level9",
                arquivo);
        }

        private Image CarregarImagemConves(string arquivo)
        {
            string caminho = ObterCaminhoConves(arquivo);

            if (!File.Exists(caminho))
            {
                throw new FileNotFoundException(
                    "Imagem não encontrada: " + caminho);
            }

            using (Image original = Image.FromFile(caminho))
            {
                return new Bitmap(original);
            }
        }

        private void CarregarFundoConves()
        {
            string caminho = ObterCaminhoConves("fundo.gif");

            if (!File.Exists(caminho))
            {
                string[] gifs = Directory.GetFiles(
                    Path.GetDirectoryName(caminho),
                    "*.gif");

                if (gifs.Length == 0)
                {
                    throw new FileNotFoundException(
                        "Nenhum GIF encontrado na pasta Level9.");
                }

                caminho = gifs[0];
            }

            streamFundoConves = new MemoryStream(
                File.ReadAllBytes(caminho));

            imagemFundoConves =
                Image.FromStream(streamFundoConves);

            pbx_fundo.Image = imagemFundoConves;
        }

        private void CarregarRecursosConves()
        {
            CarregarFundoConves();

            imagemGioIdleConves =
                CarregarImagemConves("gio_1.png");

            imagemGioAcertoConves =
                CarregarImagemConves("gio_2.png");

            imagemGioErroConves =
                CarregarImagemConves("gio_3.png");

            imagemCaixaNormalConves =
                CarregarImagemConves(ARQUIVO_CAIXA_NORMAL);

            imagemCaixaAcertoConves =
                CarregarImagemConves(ARQUIVO_CAIXA_ACERTO);

            imagemCaixaErroConves =
                CarregarImagemConves(ARQUIVO_CAIXA_ERRO);

            imagemCoracaoConves =
                CarregarImagemConves("coracao.png");

            MostrarIdleConves();

            RestaurarVidasVisuaisConves();

            RestaurarOpcoesConves();
        }

        private void LiberarRecursosConves()
        {
            timerAnimacaoConves?.Stop();
            timerAnimacaoConves?.Dispose();

            timerTempoConves?.Stop();
            timerTempoConves?.Dispose();

            timerTransicaoConves?.Stop();
            timerTransicaoConves?.Dispose();

            cronometroGeralConves.Stop();

            foreach (Image imagem in new Image[]
            {
                imagemGioIdleConves,
                imagemGioAcertoConves,
                imagemGioErroConves,
                imagemCaixaNormalConves,
                imagemCaixaAcertoConves,
                imagemCaixaErroConves,
                imagemCoracaoConves
            })
            {
                imagem?.Dispose();
            }

            imagemFundoConves?.Dispose();

            streamFundoConves?.Dispose();
        }

        #endregion

        #region Personagem e interface

        private void MostrarIdleConves()
        {
            pbx_gio.BackgroundImage =
                imagemGioIdleConves;
        }

        private void MostrarAcertoConves()
        {
            pbx_gio.BackgroundImage =
                imagemGioAcertoConves;
        }

        private void MostrarErroConves()
        {
            pbx_gio.BackgroundImage =
                imagemGioErroConves;
        }

        private void AtualizarVidasConves()
        {
            for (int i = 0; i < 3; i++)
            {
                vidasVisuaisConves[i].BackgroundImage =
                    i >= (3 - vidasConves)
                        ? imagemCoracaoConves
                        : null;
            }
        }

        private void RestaurarVidasVisuaisConves()
        {
            foreach (PictureBox vida in vidasVisuaisConves)
            {
                vida.BackgroundImage =
                    imagemCoracaoConves;
            }
        }

        private void RestaurarOpcoesConves()
        {
            foreach (PictureBox opcao in opcoesConves)
            {
                opcao.BackgroundImage =
                    imagemCaixaNormalConves;

                opcao.Enabled = true;
                opcao.Visible = true;

                opcao.Invalidate();
            }
        }

        private void AtualizarIndicadoresConves()
        {
            AtualizarVidasConves();

            lbl_numero.Text =
                perguntaAtualConves + "/" +
                TOTAL_PERGUNTAS_CONVES;
        }

        private void ReproduzirAnimacaoConves(bool acertouConves)
        {
            if (acertouConves)
                MostrarAcertoConves();
            else
                MostrarErroConves();

            timerAnimacaoConves.Stop();

            timerAnimacaoConves.Interval =
                DURACAO_ANIMACAO_CONVES;

            timerAnimacaoConves.Start();
        }

        #endregion

        #region Pontuação

        private int CalcularBonusVidasConves()
        {
            switch (vidasConves)
            {
                case 3:
                    return 2500;

                case 2:
                    return 1667;

                case 1:
                    return 834;

                default:
                    return 0;
            }
        }

        private int CalcularBonusTempoConves()
        {
            int segundosTotais =
                (int)Math.Ceiling(cronometroGeralConves.Elapsed.TotalSeconds);

            int segundosExcedentes =
                Math.Max(
                    0,
                    segundosTotais - TEMPO_LIMITE_BONUS_CONVES);

            int bonus =
                BONUS_TEMPO_CONVES -
                (segundosExcedentes * DESCONTO_SEGUNDO_CONVES);

            return Math.Max(0, bonus);
        }

        private int CalcularPontuacaoConves()
        {
            int bonusConclusao = BONUS_CONCLUSAO_CONVES;
            int bonusVidas = CalcularBonusVidasConves();
            int bonusTempo = CalcularBonusTempoConves();

            return bonusConclusao +
                   bonusVidas +
                   bonusTempo;
        }

        #endregion

        #region Cronômetro

        private void IniciarCronometroConves()
        {
            timerTempoConves.Stop();

            tempoRestanteConves =
                TEMPO_MAXIMO_CONVES * 10;

            prg_tempo.Value =
                prg_tempo.Maximum;

            timerTempoConves.Start();
        }

        private void PararCronometroConves()
        {
            timerTempoConves.Stop();
        }

        private void AtualizarTempoConves(object sender, EventArgs e)
        {
            if (respondendoConves || dueloEncerradoConves)
            {
                timerTempoConves.Stop();
                return;
            }

            tempoRestanteConves--;

            if (tempoRestanteConves < 0)
                tempoRestanteConves = 0;

            prg_tempo.Value =
                tempoRestanteConves;

            if (tempoRestanteConves <= 0)
            {
                PararCronometroConves();

                ProcessarRespostaConves(-1);
            }
        }

        #endregion

        #region Gerenciamento da partida

        private void PrepararPartidaConves()
        {
            timerAnimacaoConves.Stop();
            timerTempoConves.Stop();
            timerTransicaoConves.Stop();

            cronometroGeralConves.Reset();

            perguntaAtualConves = 0;
            vidasConves = 3;
            errosConves = 0;
            pontuacaoFinalConves = 0;

            tempoRestanteConves =
                TEMPO_MAXIMO_CONVES * 10;

            respondendoConves = false;
            dueloEncerradoConves = false;

            perguntasConves.Clear();

            GerarPerguntasConves();

            RestaurarOpcoesConves();

            MostrarIdleConves();

            RestaurarVidasVisuaisConves();

            lbl_dialogo.Text =
                "Vamos ver se consegue resolver esta.";

            lbl_pergunta.Text = "";

            lbl_numero.Text = "0/7";

            AtualizarIndicadoresConves();

            MostrarPerguntaConves();

            cronometroGeralConves.Start();
        }

        private void GerarPerguntasConves()
        {
            List<PerguntaConves> bancoConves =
                new List<PerguntaConves>
            {
                CriarPerguntaSequenciaNumericaConves(),
                CriarPerguntaSequenciaAlternadaConves(),
                CriarPerguntaLetrasConves(),
                CriarPerguntaUltrapassagemConves(),
                CriarPerguntaOrientacaoConves(),
                CriarPerguntaOrdenacaoConves(),
                CriarPerguntaCondicionalConves(),
                CriarPerguntaDeducaoConves(),
                CriarPerguntaClassificacaoConves(),
                CriarPerguntaFamiliaConves(),
                CriarPerguntaCalendarioConves(),
                CriarPerguntaRelacaoNumericaConves(),
                CriarPerguntaOperacaoConves(),
                CriarPerguntaPosicaoConves(),
                CriarPerguntaContagemConves(),
                CriarPerguntaCategoriasConves(),
                CriarPerguntaSequenciaDiferencasConves(),
                CriarPerguntaLogicaConves()
            };

            perguntasConves.AddRange(
                bancoConves
                    .OrderBy(x => aleatorioConves.Next())
                    .Take(TOTAL_PERGUNTAS_CONVES)
            );
        }

        private void MostrarPerguntaConves()
        {
            if (perguntaAtualConves >= perguntasConves.Count)
            {
                FinalizarDueloConves(true);
                return;
            }

            RestaurarOpcoesConves();

            PerguntaConves perguntaConves =
                perguntasConves[perguntaAtualConves];

            lbl_pergunta.Text =
                perguntaConves.EnunciadoConves;

            lbl_dialogo.Text =
                "Vamos ver se consegue resolver esta.";

            Console.WriteLine(
                "[Level 9] Pergunta " +
                (perguntaAtualConves + 1) +
                ": " +
                perguntaConves.OpcoesConves[
                    perguntaConves.RespostaCorretaConves
                ]
            );

            for (int i = 0; i < 4; i++)
            {
                opcoesConves[i].Tag =
                    perguntaConves.OpcoesConves[i];

                opcoesConves[i].Enabled = true;

                opcoesConves[i].BackgroundImage =
                    imagemCaixaNormalConves;

                opcoesConves[i].Invalidate();
            }

            AtualizarIndicadoresConves();

            MostrarIdleConves();

            IniciarCronometroConves();
        }

        private void ResponderConves(int indiceConves)
        {
            if (respondendoConves || dueloEncerradoConves)
                return;

            ProcessarRespostaConves(indiceConves);
        }

        private void ProcessarRespostaConves(int indiceConves)
        {
            if (respondendoConves ||
                dueloEncerradoConves ||
                perguntaAtualConves >= perguntasConves.Count)
            {
                return;
            }

            respondendoConves = true;

            PararCronometroConves();

            PerguntaConves perguntaConves =
                perguntasConves[perguntaAtualConves];

            bool acertouConves =
                indiceConves >= 0 &&
                indiceConves == perguntaConves.RespostaCorretaConves;

            foreach (PictureBox opcao in opcoesConves)
            {
                opcao.Enabled = false;
            }

            if (acertouConves)
            {
                opcoesConves[indiceConves].BackgroundImage =
                    imagemCaixaAcertoConves;

                lbl_dialogo.Text =
                    "Não esperava que você acertasse essa...";
            }
            else
            {
                vidasConves--;
                errosConves++;

                if (indiceConves >= 0)
                {
                    opcoesConves[indiceConves].BackgroundImage =
                        imagemCaixaErroConves;
                }

                if (indiceConves == -1)
                {
                    lbl_dialogo.Text =
                        "O tempo acabou, Koala. Você é muito lerdo.";
                }
                else
                {
                    lbl_dialogo.Text =
                        "Parece que você não é tão esperto assim.";
                }

                AtualizarVidasConves();
            }

            ReproduzirAnimacaoConves(acertouConves);
        }

        private void FinalizarAnimacaoConves(object sender, EventArgs e)
        {
            timerAnimacaoConves.Stop();

            if (dueloEncerradoConves)
                return;

            if (errosConves >= 3)
            {
                FinalizarDueloConves(false);
                return;
            }

            perguntaAtualConves++;

            if (perguntaAtualConves >= TOTAL_PERGUNTAS_CONVES)
            {
                FinalizarDueloConves(true);
                return;
            }

            respondendoConves = false;

            RestaurarOpcoesConves();

            MostrarPerguntaConves();
        }

        private void BloquearOpcoesConves()
        {
            foreach (PictureBox opcao in opcoesConves)
            {
                opcao.Enabled = false;
                opcao.Cursor = Cursors.Default;
            }
        }

        private void FinalizarDueloConves(bool venceuConves)
        {
            if (dueloEncerradoConves)
                return;

            dueloEncerradoConves = true;
            respondendoConves = true;

            timerAnimacaoConves.Stop();
            timerTempoConves.Stop();

            cronometroGeralConves.Stop();

            BloquearOpcoesConves();

            prg_tempo.Value = 0;

            perguntaAtualConves = venceuConves
                ? TOTAL_PERGUNTAS_CONVES
                : perguntaAtualConves;

            lbl_numero.Text =
                perguntaAtualConves + "/" +
                TOTAL_PERGUNTAS_CONVES;

            if (venceuConves)
            {
                pontuacaoFinalConves =
                    CalcularPontuacaoConves();

                lbl_pergunta.Text =
                    "VOCÊ VENCEU O DUELO!";

                lbl_dialogo.Text =
                    "Hmf, vamos direto ao ponto.";

                MostrarAcertoConves();
            }
            else
            {
                pontuacaoFinalConves = 0;

                lbl_pergunta.Text =
                    "VOCÊ PERDEU O DUELO!";

                lbl_dialogo.Text =
                    "Fim de jogo, Koala!";

                MostrarErroConves();
            }

            timerTransicaoConves.Stop();
            timerTransicaoConves.Interval =
                DURACAO_TRANSICAO_CONVES;

            timerTransicaoConves.Start();

            DueloFinalizadoConves?.Invoke(venceuConves);
        }

        private void ExecutarTransicaoConves(object sender, EventArgs e)
        {
            timerTransicaoConves.Stop();

            JogoForm jogo = EncontrarJogoFormConves();

            if (jogo == null)
                return;

            if (pontuacaoFinalConves > 0)
            {
                jogo.AbrirTela(
                    new VitoriaControl(
                        IDENTIFICADOR_NIVEL_CONVES,
                        pontuacaoFinalConves));
            }
            else
            {
                jogo.AbrirTela(
                    new DerrotaControl(
                        IDENTIFICADOR_NIVEL_CONVES,
                        "Você não conseguiu vencer o duelo contra Gio."));
            }
        }

        private JogoForm EncontrarJogoFormConves()
        {
            Control atual = this;

            while (atual != null)
            {
                if (atual is JogoForm)
                    return atual as JogoForm;

                atual = atual.Parent;
            }

            return null;
        }

        #endregion

        #region Banco de perguntas

        private PerguntaConves CriarPerguntaSequenciaNumericaConves()
        {
            return PerguntaTextoConves(
                "Complete a sequência:\n\n2, 6, 12, 20, 30, ?",
                "42",
                new[] { "40", "41", "44" }
            );
        }

        private PerguntaConves CriarPerguntaSequenciaAlternadaConves()
        {
            return PerguntaTextoConves(
                "Qual número completa a sequência?\n\n2, 5, 4, 10, 6, 15, 8, ?",
                "20",
                new[] { "16", "18", "24" }
            );
        }

        private PerguntaConves CriarPerguntaLetrasConves()
        {
            return PerguntaTextoConves(
                "Qual letra vem a seguir?\n\nA, C, F, J, O, ?",
                "U",
                new[] { "T", "V", "W" }
            );
        }

        private PerguntaConves CriarPerguntaUltrapassagemConves()
        {
            switch (aleatorioConves.Next(0, 4))
            {
                case 0:
                    return PerguntaTextoConves(
                        "Em uma corrida, você ultrapassa o segundo colocado. Em qual posição fica?",
                        "Segundo",
                        new[]
                        {
                    "Primeiro",
                    "Terceiro",
                    "Último"
                        }
                    );

                case 1:
                    return PerguntaTextoConves(
                        "Um médico entrega 3 comprimidos e diz para tomar um a cada meia hora. Quanto tempo você leva para tomar todos?",
                        "1 hora",
                        new[]
                        {
                    "30 minutos",
                    "1 hora e 30 minutos",
                    "2 horas"
                        }
                    );

                case 2:
                    return PerguntaTextoConves(
                        "Em um aquário há 10 peixes. Dois deles morrem. Quantos peixes continuam dentro do aquário?",
                        "10 peixes",
                        new[]
                        {
                    "8 peixes",
                    "2 peixes",
                    "Nenhum peixe"
                        }
                    );

                default:
                    return PerguntaTextoConves(
                        "Se 5 máquinas fabricam 5 peças em 5 minutos, quanto tempo 100 máquinas levam para fabricar 100 peças?",
                        "5 minutos",
                        new[]
                        {
                    "100 minutos",
                    "20 minutos",
                    "1 minuto"
                        }
                    );
            }
        }

        private PerguntaConves CriarPerguntaOrientacaoConves()
        {
            string[] direcoesConves =
            {
                "Norte", "Leste", "Sul", "Oeste"
            };

            int direcaoInicialConves =
                aleatorioConves.Next(0, 4);

            int rotacaoConves = 0;

            string[] movimentosConves =
            {
                "direita", "direita", "esquerda"
            };

            foreach (string movimentoConves in movimentosConves)
            {
                if (movimentoConves == "direita")
                    rotacaoConves++;
                else
                    rotacaoConves--;
            }

            int resultadoConves =
                (direcaoInicialConves + rotacaoConves + 4) % 4;

            string[] opcoesConves =
            {
                direcoesConves[(resultadoConves + 1) % 4],
                direcoesConves[(resultadoConves + 2) % 4],
                direcoesConves[(resultadoConves + 3) % 4]
            };

            return PerguntaTextoConves(
                "Você está olhando para " +
                direcoesConves[direcaoInicialConves] +
                ". Vira à direita duas vezes e depois à esquerda. Para onde olha?",
                direcoesConves[resultadoConves],
                opcoesConves
            );
        }

        private PerguntaConves CriarPerguntaOrdenacaoConves()
        {
            string[] nomesConves =
            {
                "Alice", "Bruno", "Caio", "Diana"
            };

            nomesConves = nomesConves
                .OrderBy(x => aleatorioConves.Next())
                .ToArray();

            string primeiroConves = nomesConves[0];
            string segundoConves = nomesConves[1];
            string terceiroConves = nomesConves[2];
            string quartoConves = nomesConves[3];

            string enunciadoConves =
                "Quatro pessoas chegaram ao navio.\n" +
                primeiroConves + " chegou antes de " + segundoConves + ".\n" +
                segundoConves + " chegou antes de " + terceiroConves + ".\n" +
                terceiroConves + " chegou antes de " + quartoConves + ".\n\n" +
                "Quem chegou em terceiro lugar?";

            return PerguntaTextoConves(
                enunciadoConves,
                terceiroConves,
                new[]
                {
                    primeiroConves,
                    segundoConves,
                    quartoConves
                }
            );
        }

        private PerguntaConves CriarPerguntaCondicionalConves()
        {
            string[] objetosConves =
            {
                "alarme", "motor", "elevador", "sistema"
            };

            string objetoConves =
                objetosConves[
                    aleatorioConves.Next(objetosConves.Length)
                ];

            return PerguntaTextoConves(
                "Se o " + objetoConves +
                " está ligado, a luz fica acesa.\n" +
                "A luz está apagada. O que podemos concluir?",

                "O " + objetoConves + " está desligado.",

                new[]
                {
                    "O " + objetoConves + " está ligado.",
                    "A luz está com defeito.",
                    "Não é possível concluir nada."
                }
            );
        }

        private PerguntaConves CriarPerguntaDeducaoConves()
        {
            string[] gruposConves =
            {
                "músicos",
                "marinheiros",
                "mecânicos",
                "cozinheiros",
                "atores",
                "pintores",
                "pilotos",
                "detetives"
            };

            string[] sorteadosConves = gruposConves
                .OrderBy(x => aleatorioConves.Next())
                .Take(4)
                .ToArray();

            string grupoAConves = sorteadosConves[0];
            string grupoBConves = sorteadosConves[1];
            string grupoCConves = sorteadosConves[2];
            string grupoDConves = sorteadosConves[3];

            string respostaConves =
                "Alguns " + grupoDConves +
                " não são " + grupoCConves + ".";

            return PerguntaTextoConves(
                "Todos os " + grupoAConves +
                " também são " + grupoBConves + ".\n" +

                "Nenhum dos " + grupoBConves +
                " pertencem ao grupo dos " + grupoCConves + ".\n" +

                "Alguns " + grupoDConves +
                " também são " + grupoAConves + ".\n\n" +

                "Qual conclusão é necessariamente verdadeira?",

                respostaConves,

                new[]
                {
                    "Todos os " + grupoDConves + " são " + grupoBConves + ".",

                    "Nenhum dos " + grupoDConves +
                    " pertencem ao grupo dos " + grupoBConves + ".",

                    "Todos os " + grupoAConves +
                    " também são " + grupoDConves + "."
                }
            );
        }

        private PerguntaConves CriarPerguntaClassificacaoConves()
        {
            return PerguntaTextoConves(
                "Qual número não pertence ao grupo?\n\n16, 25, 36, 49, 63, 64",
                "63",
                new[] { "36", "49", "64" }
            );
        }

        private PerguntaConves CriarPerguntaFamiliaConves()
        {
            string[] relacoesConves =
            {
                "o irmão da sua mãe",
                "o pai do seu pai",
                "o filho da sua irmã",
                "a irmã do seu pai"
            };

            string[] respostasConves =
            {
                "Tio",
                "Avô",
                "Sobrinho",
                "Tia"
            };

            int indiceConves =
                aleatorioConves.Next(0, relacoesConves.Length);

            return PerguntaTextoConves(
                "O que é para você " + relacoesConves[indiceConves] + "?",

                respostasConves[indiceConves],

                respostasConves
                    .Where(x => x != respostasConves[indiceConves])
                    .ToArray()
            );
        }

        private PerguntaConves CriarPerguntaCalendarioConves()
        {
            string[] diasConves =
            {
                "domingo",
                "segunda-feira",
                "terça-feira",
                "quarta-feira",
                "quinta-feira",
                "sexta-feira",
                "sábado"
            };

            int diaInicialConves =
                aleatorioConves.Next(0, 7);

            int diasAdianteConves =
                aleatorioConves.Next(30, 151);

            int resultadoConves =
                (diaInicialConves + diasAdianteConves % 7) % 7;

            List<string> erradasConves =
                new List<string>();

            for (int i = 1; i <= 3; i++)
            {
                erradasConves.Add(
                    diasConves[(resultadoConves + i) % 7]
                );
            }

            return PerguntaTextoConves(
                "Se hoje é " + diasConves[diaInicialConves] +
                ", que dia será daqui a " + diasAdianteConves + " dias?",

                diasConves[resultadoConves],

                erradasConves.ToArray()
            );
        }

        private PerguntaConves CriarPerguntaRelacaoNumericaConves()
        {
            int[] numerosPossiveisConves =
            {
        4, 6, 8, 9
    };

            int numeroConves =
                numerosPossiveisConves[
                    aleatorioConves.Next(numerosPossiveisConves.Length)
                ];

            int respostaConves =
                numeroConves * numeroConves + 1;

            return PerguntaTextoConves(
                "Descubra o padrão:\n\n" +
                "3 → 10\n" +
                "5 → 26\n" +
                "7 → 50\n" +
                numeroConves + " → ?",

                respostaConves.ToString(),

                new[]
                {
            (respostaConves - 2).ToString(),
            (respostaConves + 2).ToString(),
            (numeroConves * 2 + 1).ToString()
                }
            );
        }

        private PerguntaConves CriarPerguntaOperacaoConves()
        {
            int primeiroConves =
                aleatorioConves.Next(5, 7);

            int segundoConves =
                primeiroConves + 1;

            int respostaConves =
                primeiroConves * primeiroConves +
                segundoConves * segundoConves;

            return PerguntaTextoConves(
                "Uma operação secreta segue sempre a mesma regra:\n\n" +
                "2 ★ 3 = 13\n" +
                "3 ★ 4 = 25\n" +
                "4 ★ 5 = 41\n\n" +
                primeiroConves + " ★ " + segundoConves + " = ?",

                respostaConves.ToString(),

                new[]
                {
            (respostaConves - 2).ToString(),
            (respostaConves + 2).ToString(),
            (primeiroConves * segundoConves).ToString()
                }
            );
        }

        private PerguntaConves CriarPerguntaPosicaoConves()
        {
            string[] nomesConves =
            {
                "Ana", "Bia", "Caio", "Davi", "Eva"
            };

            nomesConves = nomesConves
                .OrderBy(x => aleatorioConves.Next())
                .ToArray();

            return PerguntaTextoConves(
                "Em uma fila, " + nomesConves[0] +
                " está antes de " + nomesConves[1] +
                ", que está antes de " + nomesConves[2] +
                ". " + nomesConves[3] +
                " está depois de todos eles.\n\n" +
                "Quem está em terceiro lugar?",

                nomesConves[2],

                new[]
                {
                    nomesConves[0],
                    nomesConves[1],
                    nomesConves[3]
                }
            );
        }

        private PerguntaConves CriarPerguntaContagemConves()
        {
            int pessoasConves =
                aleatorioConves.Next(4, 7);

            int apertosConves =
                pessoasConves * (pessoasConves - 1) / 2;

            return PerguntaTextoConves(
                pessoasConves +
                " pessoas se cumprimentam uma única vez, " +
                "cada uma com todas as outras.\n" +
                "Quantos cumprimentos acontecem?",

                apertosConves.ToString(),

                new[]
                {
                    (apertosConves - 1).ToString(),
                    (apertosConves + pessoasConves).ToString(),
                    (pessoasConves * (pessoasConves - 1)).ToString()
                }
            );
        }

        private PerguntaConves CriarPerguntaCategoriasConves()
        {
            string[][] categoriasConves =
            {
                new[] { "Tubarão", "Golfinho", "Baleia", "Sardinha" },
                new[] { "Mercúrio", "Vênus", "Marte", "Lua" },
                new[] { "Triângulo", "Quadrado", "Pentágono", "Círculo" },
                new[] { "Ferro", "Ouro", "Prata", "Madeira" }
            };

            string[][] respostasConves =
            {
                new[] { "Sardinha", "Tubarão", "Golfinho", "Baleia" },
                new[] { "Lua", "Mercúrio", "Vênus", "Marte" },
                new[] { "Círculo", "Triângulo", "Quadrado", "Pentágono" },
                new[] { "Madeira", "Ferro", "Ouro", "Prata" }
            };

            int indiceConves =
                aleatorioConves.Next(0, 4);

            string[] categoriaConves =
                categoriasConves[indiceConves];

            string respostaCorretaConves =
                respostasConves[indiceConves][0];

            string[] alternativasConves = categoriaConves
                .Where(x => x != respostaCorretaConves)
                .ToArray();

            return PerguntaTextoConves(
                "Qual elemento não pertence ao grupo?\n\n" +
                string.Join(", ", categoriaConves),

                respostaCorretaConves,

                alternativasConves
            );
        }

        private PerguntaConves CriarPerguntaSequenciaDiferencasConves()
        {
            int inicioConves =
                aleatorioConves.Next(2, 10);

            int incrementoConves =
                aleatorioConves.Next(3, 8);

            int primeiroConves = inicioConves;

            int segundoConves =
                primeiroConves + incrementoConves;

            int terceiroConves =
                segundoConves + incrementoConves * 2;

            int quartoConves =
                terceiroConves + incrementoConves * 3;

            int respostaConves =
                quartoConves + incrementoConves * 4;

            return PerguntaTextoConves(
                "Complete a sequência:\n\n" +
                primeiroConves + ", " +
                segundoConves + ", " +
                terceiroConves + ", " +
                quartoConves + ", ?",

                respostaConves.ToString(),

                new[]
                {
                    (respostaConves - incrementoConves).ToString(),
                    (respostaConves + incrementoConves).ToString(),
                    (respostaConves + 2).ToString()
                }
            );
        }

        private PerguntaConves CriarPerguntaLogicaConves()
        {
            string[] nomesConves =
            {
                "A", "B", "C", "D"
            };

            nomesConves = nomesConves
                .OrderBy(x => aleatorioConves.Next())
                .ToArray();

            string aConves = nomesConves[0];
            string bConves = nomesConves[1];
            string cConves = nomesConves[2];
            string dConves = nomesConves[3];

            return PerguntaTextoConves(
                "Se " + aConves + " é maior que " + bConves +
                " e " + bConves + " é maior que " + cConves +
                ", qual afirmação é obrigatoriamente verdadeira?",

                aConves + " é maior que " + cConves + ".",

                new[]
                {
                    cConves + " é maior que " + aConves + ".",
                    aConves + " é igual a " + cConves + ".",
                    dConves + " é maior que " + aConves + "."
                }
            );
        }

        #endregion

        #region Montagem das alternativas

        private PerguntaConves PerguntaTextoConves(
            string enunciadoConves,
            string respostaConves,
            string[] erradasConves)
        {
            List<string> alternativasConves =
                new List<string> { respostaConves };

            foreach (string erradaConves in erradasConves)
            {
                if (!alternativasConves.Contains(erradaConves))
                {
                    alternativasConves.Add(erradaConves);
                }
            }

            alternativasConves = alternativasConves
                .OrderBy(x => aleatorioConves.Next())
                .ToList();

            return new PerguntaConves(
                enunciadoConves,
                alternativasConves.ToArray(),
                alternativasConves.IndexOf(respostaConves)
            );
        }

        #endregion
    }
}