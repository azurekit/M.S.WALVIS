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
    public partial class Ato2Level3 : UserControl
    {

        #region Variáveis Globais


        private bool[,] tabuleiro = new bool[3, 3]; // 3x3 tabuleiro
        private Random random = new Random();

        private int movimentos = 0;

        private int round = 1;

        private int resets = 3;

        #endregion
        public Ato2Level3()
        {
            InitializeComponent();

    

            round = 1;
            movimentos = 0;
            resets = 3;
            lbl_resets2.Text = resets.ToString();
            lbl_resets2.ForeColor = Color.White;
            lbl_movimentos.Text = "0";
            pbx_luz1.BackColor = Color.Red;
            pbx_luz2.BackColor = Color.Red;
            pbx_luz3.BackColor = Color.Red;
            IniciarJogo();
        }

        #region Resetar tentativa

        private void btn_resetar_Click(object sender, EventArgs e)
        {
            if (resets > 1)
            {
                resets--;
                lbl_resets2.Text = resets.ToString();
                IniciarJogo();
            }
            else if (resets <=1)
            {
                resets--;
                IniciarJogo();
                lbl_resets2.Text = resets.ToString();
                btn_resetar.Enabled = false;
                lbl_resets2.ForeColor = Color.Red;
            }
        }

        #endregion

        #region Iniciar jogo

        private void IniciarJogo()
        {

            do { 
            // Gera um tabuleiro aleatório, possível de ser resolvido

            for (int linha = 0; linha < 3; linha++)
            {
                for (int coluna = 0; coluna < 3; coluna++)
                {
                    tabuleiro[linha, coluna] = true;
                }
            }

            Console.WriteLine($"Round {round}:");

            for (int i = 0; i < random.Next(5, 10); i++) // Gera entre 5 e 10 movimentos aleatórios
            {
                int linha = random.Next(3);
                int coluna = random.Next(3);
                Console.WriteLine($"Movimento aleatório: ({linha}, {coluna})");
                AlternarCasa(linha, coluna); // Inverte a casa clicada
                AlternarCasa(linha - 1, coluna); // Cima
                AlternarCasa(linha + 1, coluna); // Baixo
                AlternarCasa(linha, coluna - 1); // Esquerda
                AlternarCasa(linha, coluna + 1); // Direita
            }

            } while (Vencer()); // garante que o tabuleiro inicial não seja uma vitória

            AtualizarBotoes();
        }

        #endregion

        #region Finalizar jogo

        private void FinalizarJogo()
        {
            MessageBox.Show($"Parabéns! Você venceu o jogo em {movimentos} movimentos!");
            Cruzeiro cruzeiro = (Cruzeiro)this.FindForm();
            cruzeiro.AbrirTela(new LevelSelectControl(2));
        }

        #endregion

        #region Alternar casas do tabuleiro
        private void Clique (int linha, int coluna)
        {
            AlternarCasa(linha, coluna); // Inverte a casa clicada
            AlternarCasa(linha-1, coluna); // Cima
            AlternarCasa(linha + 1, coluna); // Baixo
            AlternarCasa(linha, coluna - 1); // Esquerda
            AlternarCasa(linha, coluna + 1); // Direita

            movimentos++;
            lbl_movimentos.Text = movimentos.ToString();

            AtualizarBotoes();

            if (Vencer())
            {
                if (round == 1)
                {
                    MessageBox.Show($"Parabéns! Você venceu o round {round} em {movimentos} movimentos!");
                    round++;
                    pbx_luz1.BackColor = Color.Green;
                    IniciarJogo();
                }

                else if (round == 2)
                {
                    MessageBox.Show($"Parabéns! Você venceu o round {round} em {movimentos} movimentos!");
                    round++;
                    pbx_luz2.BackColor = Color.Green;
                    IniciarJogo();
                }

                else if (round == 3)
                {
                    MessageBox.Show($"Parabéns! Você venceu o round {round} em {movimentos} movimentos!");
                    pbx_luz3.BackColor = Color.Green;
                    FinalizarJogo();
                }
                
            }

        }

        private void AlternarCasa (int linha, int coluna)
        {
            if (linha<0 || linha>= 3 || coluna < 0 || coluna >= 3)
            {
                return; // Sai do método se a posição estiver fora dos limites
            }

            tabuleiro[linha, coluna] = !tabuleiro[linha, coluna]; // Inverte o estado da casa
        }

        private void AtualizarBotoes()
        {
            btn_00.BackColor = tabuleiro[0, 0] ? Color.Yellow : Color.Gray;
            btn_01.BackColor = tabuleiro[0, 1] ? Color.Yellow : Color.Gray;
            btn_02.BackColor = tabuleiro[0, 2] ? Color.Yellow : Color.Gray;
            btn_10.BackColor = tabuleiro[1, 0] ? Color.Yellow : Color.Gray;
            btn_11.BackColor = tabuleiro[1, 1] ? Color.Yellow : Color.Gray;
            btn_12.BackColor = tabuleiro[1, 2] ? Color.Yellow : Color.Gray;
            btn_20.BackColor = tabuleiro[2, 0] ? Color.Yellow : Color.Gray;
            btn_21.BackColor = tabuleiro[2, 1] ? Color.Yellow : Color.Gray;
            btn_22.BackColor = tabuleiro[2, 2] ? Color.Yellow : Color.Gray;
        }

        private bool Vencer()
        {
            for (int linha = 0; linha < 3; linha++)
            {
                for (int coluna = 0; coluna < 3; coluna++)
                {
                    if (!tabuleiro[linha, coluna])
                    {
                        return false; // Se alguma casa estiver apagada, o jogador não venceu
                    }
                }
            }
            return true; // Todas as casas estão acesas, o jogador venceu
        }
        #endregion

        #region Cliques dos botões
        private void btn_00_Click(object sender, EventArgs e)
        {
            Clique(0,0);
        }

        private void btn_01_Click(object sender, EventArgs e)
        {
            Clique(0, 1);
        }

        private void btn_02_Click(object sender, EventArgs e)
        {
            Clique(0,2);
        }

        private void btn_10_Click(object sender, EventArgs e)
        {
            Clique(1,0);
        }

        private void btn_11_Click(object sender, EventArgs e)
        {
            Clique(1,1);
        }

        private void btn_12_Click(object sender, EventArgs e)
        {
            Clique(1,2);
        }

        private void btn_20_Click(object sender, EventArgs e)
        {
            Clique(2, 0);
        }

        private void btn_21_Click(object sender, EventArgs e)
        {
            Clique(2, 1);
        }

        private void btn_22_Click(object sender, EventArgs e)
        {
            Clique(2, 2);
        }
        #endregion



        #region lixeira
        private void lbl_movimentos_Click(object sender, EventArgs e)
        {

        }

        private void lbl_resets2_Click(object sender, EventArgs e)
        {

        }

        #endregion


    }
}
