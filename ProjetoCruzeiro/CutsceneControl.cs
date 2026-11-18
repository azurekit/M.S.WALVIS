
using AxWMPLib;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace ProjetoCruzeiro
{
    public partial class CutsceneControl : UserControl
    {
        #region Variáveis Globais

        private int identificador;
        private string video;
        private bool redirecionando = false;

        private List<EfeitoBotao> efeitos = new List<EfeitoBotao>();

        private string pastaImagens;

        private Image imagemOriginal;

        #endregion

        #region Construtor

        public CutsceneControl(int identificador)
        {
            InitializeComponent();

            SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw,
                true
            );

            UpdateStyles();

            DoubleBuffered = true;

            if (identificador < 1 || identificador > 18)
                throw new ArgumentOutOfRangeException(
                    "identificador",
                    "O identificador da cutscene deve estar entre 1 e 18."
                );

            this.identificador = identificador;

            pastaImagens = Path.Combine(
                Application.StartupPath,
                "imagens",
                "Cutscenes"
            );

            string pastaVideos = Path.Combine(
                Application.StartupPath,
                "videos"
            );

            video = Path.Combine(
                pastaVideos,
                "cutscene" + identificador + ".mp4"
            );

            if (!File.Exists(video))
            {
                video = Path.Combine(
                    pastaVideos,
                    "intro2.mp4"
                );
            }

            axCutsceneIntro.stretchToFit = true;
            axCutsceneIntro.uiMode = "none";
            axCutsceneIntro.enableContextMenu = false;

            ConfigurarBotaoPular();

            axCutsceneIntro.URL = video;
            axCutsceneIntro.Ctlcontrols.play();

            pbx_pular.BringToFront();
        }

        #endregion

        #region Configuração do botão pular

        private Image CarregarImagem(string caminho)
        {
            if (!File.Exists(caminho))
                return null;

            using (Image imagem = Image.FromFile(caminho))
            {
                return new Bitmap(imagem);
            }
        }

        private void ConfigurarBotaoPular()
        {
            Point posicaoOriginal = pbx_pular.Location;
            Size tamanhoOriginal = pbx_pular.Size;

            pbx_pular.Parent = this;

            pbx_pular.Location = posicaoOriginal;
            pbx_pular.Size = tamanhoOriginal;

            pbx_pular.BackColor = Color.Transparent;
            pbx_pular.BackgroundImageLayout = ImageLayout.Stretch;
            pbx_pular.Image = null;

            imagemOriginal = CarregarImagem(
                Path.Combine(pastaImagens, "botaopular.png")
            );

            pbx_pular.BackgroundImage = imagemOriginal;

            pbx_pular.Cursor = Cursors.Hand;

            pbx_pular.Click += btn_pular_Click;

            efeitos.Add(new EfeitoBotao(pbx_pular));

            pbx_pular.BringToFront();
        }

        #endregion

        #region Redirecionamento

        private void Redirecionar()
        {
            if (redirecionando)
                return;

            redirecionando = true;

            axCutsceneIntro.PlayStateChange -=
                axCutsceneIntro_PlayStateChange;

            try
            {
                axCutsceneIntro.Ctlcontrols.stop();
            }
            catch
            {
            }

            Cruzeiro cruzeiro = this.FindForm() as Cruzeiro;

            if (cruzeiro == null)
                return;

            switch (identificador)
            {
                case 1:
                    cruzeiro.AbrirTela(new JogoForm(1, 1));
                    break;

                case 2:
                    cruzeiro.AbrirTela(new LevelSelectControl(1));
                    break;

                case 3:
                    cruzeiro.AbrirTela(new JogoForm(1, 2));
                    break;

                case 4:
                    cruzeiro.AbrirTela(new LevelSelectControl(1));
                    break;

                case 5:
                    cruzeiro.AbrirTela(new JogoForm(1, 3));
                    break;

                case 6:
                    cruzeiro.AbrirTela(new LevelSelectControl(1));
                    break;

                case 7:
                    cruzeiro.AbrirTela(new JogoForm(2, 1));
                    break;

                case 8:
                    cruzeiro.AbrirTela(new LevelSelectControl(2));
                    break;

                case 9:
                    cruzeiro.AbrirTela(new JogoForm(2, 2));
                    break;

                case 10:
                    cruzeiro.AbrirTela(new LevelSelectControl(2));
                    break;

                case 11:
                    cruzeiro.AbrirTela(new JogoForm(2, 3));
                    break;

                case 12:
                    cruzeiro.AbrirTela(new LevelSelectControl(2));
                    break;

                case 13:
                    cruzeiro.AbrirTela(new JogoForm(3, 1));
                    break;

                case 14:
                    cruzeiro.AbrirTela(new LevelSelectControl(3));
                    break;

                case 15:
                    cruzeiro.AbrirTela(new JogoForm(3, 2));
                    break;

                case 16:
                    cruzeiro.AbrirTela(new LevelSelectControl(3));
                    break;

                case 17:
                    cruzeiro.AbrirTela(new JogoForm(3, 3));
                    break;

                case 18:
                    cruzeiro.AbrirTela(new LevelSelectControl(3));
                    break;
            }
        }

        #endregion

        #region Evento de término do vídeo

        private void axCutsceneIntro_PlayStateChange(
            object sender,
            _WMPOCXEvents_PlayStateChangeEvent e)
        {
            if (e.newState == 8)
            {
                Redirecionar();
            }
        }

        #endregion

        #region Botão de pular cutscene

        private void btn_pular_Click(object sender, EventArgs e)
        {
            Redirecionar();
        }

        #endregion

        #region Lixeira

        private void CutsceneIntroControl_Load(object sender, EventArgs e)
        {
        }

        private void axCutsceneIntro_Enter(object sender, EventArgs e)
        {
        }

        #endregion
    }
}
