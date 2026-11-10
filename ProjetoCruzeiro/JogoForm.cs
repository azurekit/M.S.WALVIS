
using System;
using System.Drawing;
using System.Windows.Forms;

namespace ProjetoCruzeiro
{
    public partial class JogoForm : UserControl
    {
        private int atoAtual;
        private int nivelAtual;

        private MenuLateralControl menuLateral;

        public JogoForm(int ato, int nivel)
        {
            InitializeComponent();

            atoAtual = ato;
            nivelAtual = nivel;

            menuLateral = new MenuLateralControl();
            menuLateral.Dock = DockStyle.Fill;
            menuLateral.DefinirNivel(atoAtual, nivelAtual);

            panelJogo2.Controls.Clear();
            panelJogo2.Controls.Add(menuLateral);

            AbrirNivel();
        }

        private void AbrirNivel()
        {
            UserControl tela = null;

            switch (atoAtual)
            {
                case 1:
                    switch (nivelAtual)
                    {
                        case 1: tela = new Ato1Level1(); break;
                        case 2: tela = new Ato1Level2(); break;
                        case 3: tela = new Ato1Level3(); break;
                    }
                    break;

                case 2:
                    switch (nivelAtual)
                    {
                        case 1: tela = new Ato2Level1(); break;
                        case 2: tela = new Ato2Level2(); break;
                        case 3: tela = new Ato2Level3(); break;
                    }
                    break;

                case 3:
                    switch (nivelAtual)
                    {
                        case 1: tela = new Ato3Level1(); break;
                        case 2: tela = new Ato3Level2(); break;
                        case 3: tela = new Ato3Level3(); break;
                    }
                    break;
            }

            if (tela != null)
                AbrirTela(tela);
        }

        public void AbrirTela(UserControl tela)
        {
            menuLateral.PermitirVoltarMenu();
            ExibirTela(tela);
        }

        public void MostrarVitoria(int identificador, int pontuacao)
        {
            menuLateral.BloquearVoltarMenu();

            ExibirTela(new VitoriaControl(identificador, pontuacao));
        }

        private void ExibirTela(UserControl tela)
        {
            panelJogo1.Controls.Clear();

            tela.Dock = DockStyle.Fill;
            panelJogo1.Controls.Add(tela);

            panelJogo2.BringToFront();
        }

        #region Lixeira

        private void JogoForm_Load(object sender, EventArgs e)
        {

        }

        private void panelJogo1_Paint(object sender, PaintEventArgs e)
        {

        }

        #endregion
    }
}
