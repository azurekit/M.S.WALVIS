
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace ProjetoCruzeiro
{
    public partial class DificuldadeSelectControl : UserControl
    {
        #region Variáveis Globais

        int dificuldadeSelecionada = 0;

        private List<EfeitoBotao> efeitos = new List<EfeitoBotao>();

        private Image ticket1Original;
        private Image ticket2Original;
        private Image ticket3Original;

        private Image ticket1Open;
        private Image ticket2Open;
        private Image ticket3Open;

        private Image ticket2Bloqueado;
        private Image ticket3Bloqueado;

        private Image caso2Preview;
        private Image caso2BloqPreview;

        private Image caso3Preview;
        private Image caso3BloqPreview;

        private bool caso2Desbloqueado;
        private bool caso3Desbloqueado;

        #endregion

        public DificuldadeSelectControl()
        {
            InitializeComponent();

            DoubleBuffered = true;

            #region Efeitos dos botões

            efeitos.Add(new EfeitoBotao(pbx_voltar));
            efeitos.Add(new EfeitoBotao(pbx_selecionar1));
            efeitos.Add(new EfeitoBotao(pbx_selecionar2));
            efeitos.Add(new EfeitoBotao(pbx_selecionar3));

            #endregion

            CarregarImagens();

            ConfigurarAnimacaoTickets();

            AtualizarDesbloqueios();
        }

        #region Carregar imagens

        private Image CarregarImagem(string nome)
        {
            string caminho = Path.Combine(
                Application.StartupPath,
                "imagens",
                "SeletorCaso",
                nome
            );

            if (!File.Exists(caminho))
            {
                MessageBox.Show(
                    "Não foi possível encontrar a imagem:\n" + caminho,
                    "Erro ao carregar imagem",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );

                return null;
            }

            using (Image imagem = Image.FromFile(caminho))
            {
                return new Bitmap(imagem);
            }
        }

        private void CarregarImagens()
        {
            ticket1Original = CarregarImagem("ticket1.png");
            ticket2Original = CarregarImagem("ticket2.png");
            ticket3Original = CarregarImagem("ticket3.png");

            ticket1Open = CarregarImagem("ticket1open.png");
            ticket2Open = CarregarImagem("ticket2open.png");
            ticket3Open = CarregarImagem("ticket3open.png");

            ticket2Bloqueado = CarregarImagem("ticket2bloqueado.png");
            ticket3Bloqueado = CarregarImagem("ticket3bloqueado.png");

            caso2Preview = CarregarImagem("caso2preview.png");
            caso2BloqPreview = CarregarImagem("caso2bloqpreview.png");

            caso3Preview = CarregarImagem("caso3preview.png");
            caso3BloqPreview = CarregarImagem("caso3bloqpreview.png");
        }

        #endregion

        #region Animação dos tickets

        private void ConfigurarAnimacaoTickets()
        {
            pbx_selecionar1.BackgroundImage = ticket1Original;
            pbx_selecionar2.BackgroundImage = ticket2Original;
            pbx_selecionar3.BackgroundImage = ticket3Original;

            pbx_selecionar1.MouseEnter += (s, e) =>
            {
                pbx_selecionar1.BackgroundImage = ticket1Open;
            };

            pbx_selecionar1.MouseLeave += (s, e) =>
            {
                pbx_selecionar1.BackgroundImage = ticket1Original;
            };

            pbx_selecionar2.MouseEnter += (s, e) =>
            {
                if (caso2Desbloqueado)
                    pbx_selecionar2.BackgroundImage = ticket2Open;
            };

            pbx_selecionar2.MouseLeave += (s, e) =>
            {
                if (caso2Desbloqueado)
                    pbx_selecionar2.BackgroundImage = ticket2Original;
            };

            pbx_selecionar3.MouseEnter += (s, e) =>
            {
                if (caso3Desbloqueado)
                    pbx_selecionar3.BackgroundImage = ticket3Open;
            };

            pbx_selecionar3.MouseLeave += (s, e) =>
            {
                if (caso3Desbloqueado)
                    pbx_selecionar3.BackgroundImage = ticket3Original;
            };
        }

        #endregion

        #region Desbloqueio dos casos

        private void AtualizarDesbloqueios()
        {
            caso2Desbloqueado =
                SaveManager.EstaConcluido(1) &&
                SaveManager.EstaConcluido(2) &&
                SaveManager.EstaConcluido(3);

            caso3Desbloqueado =
                caso2Desbloqueado &&
                SaveManager.EstaConcluido(4) &&
                SaveManager.EstaConcluido(5) &&
                SaveManager.EstaConcluido(6);

            AtualizarVisualCaso2();
            AtualizarVisualCaso3();
        }

        private void AtualizarVisualCaso2()
        {
            if (caso2Desbloqueado)
            {
                pbx_selecionar2.BackgroundImage = ticket2Original;
                pbx_selecionar2.Enabled = true;
                pbx_selecionar2.Cursor = Cursors.Hand;

                pbx_caso2.BackgroundImage = caso2Preview;
            }
            else
            {
                pbx_selecionar2.BackgroundImage = ticket2Bloqueado;
                pbx_selecionar2.Enabled = false;
                pbx_selecionar2.Cursor = Cursors.No;

                pbx_caso2.BackgroundImage = caso2BloqPreview;
            }

            pbx_selecionar2.BackgroundImageLayout = ImageLayout.Stretch;
            pbx_caso2.BackgroundImageLayout = ImageLayout.Stretch;
        }

        private void AtualizarVisualCaso3()
        {
            if (caso3Desbloqueado)
            {
                pbx_selecionar3.BackgroundImage = ticket3Original;
                pbx_selecionar3.Enabled = true;
                pbx_selecionar3.Cursor = Cursors.Hand;

                pbx_caso3.BackgroundImage = caso3Preview;
            }
            else
            {
                pbx_selecionar3.BackgroundImage = ticket3Bloqueado;
                pbx_selecionar3.Enabled = false;
                pbx_selecionar3.Cursor = Cursors.No;

                pbx_caso3.BackgroundImage = caso3BloqPreview;
            }

            pbx_selecionar3.BackgroundImageLayout = ImageLayout.Stretch;
            pbx_caso3.BackgroundImageLayout = ImageLayout.Stretch;
        }

        #endregion

        #region Botão de voltar

        private void btn_menu_Click(object sender, EventArgs e)
        {
            Cruzeiro cruzeiro = (Cruzeiro)FindForm();
            cruzeiro.AbrirTela(new MenuControl());
        }

        #endregion

        #region Botões de dificuldade

        private void btn_caso1_Click(object sender, EventArgs e)
        {
            dificuldadeSelecionada = 1;

            Cruzeiro cruzeiro = (Cruzeiro)FindForm();
            cruzeiro.AbrirTela(new LevelSelectControl(dificuldadeSelecionada));
        }

        private void btn_caso2_Click(object sender, EventArgs e)
        {
            if (!caso2Desbloqueado)
                return;

            dificuldadeSelecionada = 2;

            Cruzeiro cruzeiro = (Cruzeiro)FindForm();
            cruzeiro.AbrirTela(new LevelSelectControl(dificuldadeSelecionada));
        }

        private void btn_caso3_Click(object sender, EventArgs e)
        {
            if (!caso3Desbloqueado)
                return;

            dificuldadeSelecionada = 3;

            Cruzeiro cruzeiro = (Cruzeiro)FindForm();
            cruzeiro.AbrirTela(new LevelSelectControl(dificuldadeSelecionada));
        }

        #endregion

        #region Lixeira

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click_1(object sender, EventArgs e)
        {

        }

        private void DificuldadeControl_Load(object sender, EventArgs e)
        {

        }

        #endregion
    }
}
