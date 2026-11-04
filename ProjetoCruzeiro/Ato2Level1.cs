using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjetoCruzeiro
{
    public partial class Ato2Level1 : UserControl
    {

        #region Variáveis Globais

        List<List<int>> suportes = new List<List<int>>();

        PictureBox[] pratos = new PictureBox[5];
        Panel[] paineis = new Panel[3];

        int movimentos = 0;
        int suporteSelecionado = -1;

        bool jogoFinalizado = false;

        #endregion

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

            iniciarJogo();
        }

        #region Iniciar jogo
        private void iniciarJogo()
        {
            movimentos = 0;
            lbl_movimentos2.Text = movimentos.ToString();
            suporteSelecionado = -1;
            
            suportes.Clear();

            for (int i = 0; i < 3; i++)
            {
                suportes.Add(new List<int>());
            }

            for (int i = 5; i >= 1; i--)
            {
                suportes[0].Add(i);
            }

            atualizarTabuleiro();

        }

        #endregion

        #region Atualizar tabuleiro
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
                        paineis[i].Height - altura - prato.Height
                    );

                    prato.Visible = true;

                    prato.BringToFront();

                    altura += prato.Height;
                }
            }
        }
        #endregion

        #region Clique
        private void Suporte_Click(object sender, EventArgs e)
        {
            if (jogoFinalizado)
            {
                return;
            }

            Control controle = sender as Control; //identifica o painel clicado caso clicar no painel

            Panel painel = controle as Panel ?? controle.Parent as Panel; //identifica o painel clicado caso clicar no prato

            int destino = Array.IndexOf(paineis, painel);

           

            if (destino == -1) return;

            if (suporteSelecionado == -1) 
            {
                if (suportes[destino].Count == 0) return; 

                suporteSelecionado = destino;

                paineis[suporteSelecionado].BackColor = Color.LightBlue;

                return;
            }

            if (suporteSelecionado == destino) //clicou no mesmo suporte
            {
                suporteSelecionado = -1;
                return;
            }

            int origem = suporteSelecionado;

            paineis[origem].BackColor = SystemColors.Control;

            int pratoMovido = suportes[origem][suportes[origem].Count - 1];

            if (suportes[destino].Count > 0)
            {
                int pratoTopo = suportes[destino][suportes[destino].Count - 1];

                if (pratoMovido > pratoTopo)
                {
                    MessageBox.Show("Não é permitido colocar um prato maior sobre um menor.");
                    suporteSelecionado = -1;
                    return;
                }
            }

            suportes[origem].RemoveAt(suportes[origem].Count - 1);
            suportes[destino].Add(pratoMovido);
            movimentos++;
            lbl_movimentos2.Text = movimentos.ToString();
            suporteSelecionado = -1;
            atualizarTabuleiro();
            verificarVitoria();

        }

        #endregion

        #region Verificar vitória

        private void verificarVitoria()
        {
            if (suportes[2].Count == 5)
            {
                MessageBox.Show("Parabéns! Você venceu o jogo.");
                jogoFinalizado = true;
                Cruzeiro cruzeiro = (Cruzeiro)this.FindForm();
                cruzeiro.AbrirTela(new LevelSelectControl(2));
            }
        }

        #endregion

        #region Visuais
        private void Suporte_MouseEnter(object sender, EventArgs e)
        {
            Control controle = sender as Control;

            Panel painel = controle as Panel ?? controle.Parent as Panel;

            if (painel == null) return;

            int indice = Array.IndexOf(paineis, painel);

            if (indice != suporteSelecionado)
            {
                painel.BackColor = Color.LightCyan;
            }
        }

        private void Suporte_MouseLeave(object sender, EventArgs e)
        {
            Control controle = sender as Control;

            Panel painel = controle as Panel ?? controle.Parent as Panel;

            int indice = Array.IndexOf(paineis, painel);

            if (indice != suporteSelecionado)
            {
                painel.BackColor = SystemColors.Control;
            }
        }


        #endregion

        #region Lixeira
        private void suporte_Click(object sender, EventArgs e)
        {

        }

        private void Suporte_MouseHover(object sender, EventArgs e)
        {

        }
        #endregion



       
    }
}
