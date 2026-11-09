
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        #endregion

        public DificuldadeSelectControl()
        {
            InitializeComponent();

            this.DoubleBuffered = true;

            #region Efeitos dos botões

            efeitos.Add(new EfeitoBotao(pbx_voltar));
            efeitos.Add(new EfeitoBotao(pbx_selecionar1));
            efeitos.Add(new EfeitoBotao(pbx_selecionar2));
            efeitos.Add(new EfeitoBotao(pbx_selecionar3));

            #endregion

            #region Imagens dos tickets

            string pasta = Path.Combine(Application.StartupPath, "imagens");

            ticket1Original = CarregarImagem(Path.Combine(pasta, "ticket1.png"));
            ticket2Original = CarregarImagem(Path.Combine(pasta, "ticket2.png"));
            ticket3Original = CarregarImagem(Path.Combine(pasta, "ticket3.png"));

            ticket1Open = CarregarImagem(Path.Combine(pasta, "ticket1open.png"));
            ticket2Open = CarregarImagem(Path.Combine(pasta, "ticket2open.png"));
            ticket3Open = CarregarImagem(Path.Combine(pasta, "ticket3open.png"));

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
                pbx_selecionar2.BackgroundImage = ticket2Open;
            };

            pbx_selecionar2.MouseLeave += (s, e) =>
            {
                pbx_selecionar2.BackgroundImage = ticket2Original;
            };

            pbx_selecionar3.MouseEnter += (s, e) =>
            {
                pbx_selecionar3.BackgroundImage = ticket3Open;
            };

            pbx_selecionar3.MouseLeave += (s, e) =>
            {
                pbx_selecionar3.BackgroundImage = ticket3Original;
            };

            #endregion
        }

        #region Carregar imagens

        private Image CarregarImagem(string caminho)
        {
            using (Image imagem = Image.FromFile(caminho))
            {
                return new Bitmap(imagem);
            }
        }

        #endregion

        #region Botão de voltar

        private void btn_menu_Click(object sender, EventArgs e)
        {
            Cruzeiro cruzeiro = (Cruzeiro)this.FindForm();
            cruzeiro.AbrirTela(new MenuControl());
        }

        #endregion

        #region Botões de dificuldade

        private void btn_caso1_Click(object sender, EventArgs e)
        {
            dificuldadeSelecionada = 1;

            Cruzeiro cruzeiro = (Cruzeiro)this.FindForm();
            cruzeiro.AbrirTela(new LevelSelectControl(dificuldadeSelecionada));
        }

        private void btn_caso2_Click(object sender, EventArgs e)
        {
            dificuldadeSelecionada = 2;

            Cruzeiro cruzeiro = (Cruzeiro)this.FindForm();
            cruzeiro.AbrirTela(new LevelSelectControl(dificuldadeSelecionada));
        }

        private void btn_caso3_Click(object sender, EventArgs e)
        {
            dificuldadeSelecionada = 3;

            Cruzeiro cruzeiro = (Cruzeiro)this.FindForm();
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
