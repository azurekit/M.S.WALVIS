
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace ProjetoCruzeiro
{
    public partial class MenuControl : UserControl
    {
        private List<EfeitoBotao> efeitos = new List<EfeitoBotao>();

        public MenuControl()
        {
            InitializeComponent();

            this.DoubleBuffered = true;
            this.SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.UserPaint,
                true
            );

            efeitos.Add(new EfeitoBotao(pbx_jogar));
            efeitos.Add(new EfeitoBotao(pbx_loja));
            efeitos.Add(new EfeitoBotao(pbx_sair));
        }

        #region Botões

        private void btn_iniciar_Click(object sender, EventArgs e)
        {
            Cruzeiro cruzeiro = (Cruzeiro)this.FindForm();

            cruzeiro.AbrirTela(new DificuldadeSelectControl());
        }

        private void btn_loja_Click(object sender, EventArgs e)
        {
            Cruzeiro cruzeiro = (Cruzeiro)this.FindForm();

            cruzeiro.AbrirTela(new LojaControl());
        }

        private void btn_sair_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        #endregion

        #region Lixeira

        private void button1_Click(object sender, EventArgs e)
        {

        }

        #endregion
    }

    public class EfeitoBotao
    {
        private PictureBox botao;
        private Timer timer;
        private Size tamanhoOriginal;
        private Point posicaoOriginal;
        private bool expandir;

        public EfeitoBotao(PictureBox botao)
        {
            this.botao = botao;

            tamanhoOriginal = botao.Size;
            posicaoOriginal = botao.Location;

            timer = new Timer();
            timer.Interval = 10;
            timer.Tick += Animar;

            botao.MouseEnter += (s, e) =>
            {
                expandir = true;
                timer.Start();
            };

            botao.MouseLeave += (s, e) =>
            {
                expandir = false;
                timer.Start();
            };
        }

        private void Animar(object sender, EventArgs e)
        {
            int aumento = 7;
            int velocidade = 4;

            int larguraAlvo = tamanhoOriginal.Width + (expandir ? aumento : 0);
            int alturaAlvo = tamanhoOriginal.Height + (expandir ? aumento : 0);

            int largura = botao.Width;
            int altura = botao.Height;

            if (largura < larguraAlvo)
                largura = Math.Min(largura + velocidade, larguraAlvo);
            else if (largura > larguraAlvo)
                largura = Math.Max(largura - velocidade, larguraAlvo);

            if (altura < alturaAlvo)
                altura = Math.Min(altura + velocidade, alturaAlvo);
            else if (altura > alturaAlvo)
                altura = Math.Max(altura - velocidade, alturaAlvo);

            botao.Size = new Size(largura, altura);

            botao.Location = new Point(
                posicaoOriginal.X - (largura - tamanhoOriginal.Width) / 2,
                posicaoOriginal.Y - (altura - tamanhoOriginal.Height) / 2
            );

            if (largura == larguraAlvo && altura == alturaAlvo)
                timer.Stop();
        }
    }
}
