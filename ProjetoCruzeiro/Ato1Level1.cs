using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjetoCruzeiro
{
    public partial class Ato1Level1 : UserControl
    {

        #region Variáveis Globais
        Random random = new Random();
        int[] codigo = new int[4];
        int[] tentativa = new int[4];

        int posicaoAtual = 0;
        int tentativas = 0;

        #endregion

        public Ato1Level1()
        {
            InitializeComponent();
            #region Gerar código aleatório

            List<int> cores = new List<int> { 0, 1, 2, 3, 4, 5 }; // 0 = vermelho, 1 = azul, 2 = amarelo, 3 = roxo, 4 = laranja, 5 = verde; usando list pq permite remover elementos facilmente



            for (int i = 0; i < codigo.Length; i++)
            {
                int posicao = random.Next(cores.Count);
                codigo[i] = cores[posicao];
                cores.RemoveAt(posicao); // Remove a cor escolhida para evitar repetições
                Console.WriteLine($"Código gerado: {codigo[i]}"); // Para debug, mostra o código gerado no console
            }

            #endregion


        }

        #region Selecionar cores

        //0
        #region vermelho
        private void btn_vermelho_Click(object sender, EventArgs e)
        {

            SelecionarCor(0); // 0 = vermelho
        }
        #endregion

        //1
        #region azul
        private void btn_azul_Click(object sender, EventArgs e)
        {
            SelecionarCor(1); // 1 = azul
        }

        #endregion

        //2
        #region amarelo
        private void btn_amarelo_Click(object sender, EventArgs e)
        {
            SelecionarCor(2); // 2 = amarelo
        }
        #endregion

        //3
        #region roxo
        private void btn_roxo_Click(object sender, EventArgs e)
        {
            SelecionarCor(3); // 3 = roxo
        }
        #endregion

        //4
        #region laranja
        private void btn_laranja_Click(object sender, EventArgs e)
        {
            SelecionarCor(4); // 4 = laranja
        }
        #endregion

        //5
        #region verde
        private void btn_verde_Click(object sender, EventArgs e)
        {
            SelecionarCor(5); // 5 = verde
        }
        #endregion

        #region selecionar cor
        private void SelecionarCor(int cor)
        {
            if (posicaoAtual >= tentativa.Length)
            {
                MessageBox.Show("Você já selecionou 4 cores! Clique em Verificar para conferir sua tentativa.");
                return;
            }

            Image botao = null;

            switch (cor)
            {
                case 0:
                    botao = Image.FromFile(Path.Combine(Application.StartupPath, "imagens", "botaovermelho.png"));
                    break;
                case 1:
                    botao = Image.FromFile(Path.Combine(Application.StartupPath, "imagens", "botaoazul.png"));
                    break;
                case 2:
                    botao = Image.FromFile(Path.Combine(Application.StartupPath, "imagens", "botaoamarelo.png"));
                    break;
                case 3:
                    botao = Image.FromFile(Path.Combine(Application.StartupPath, "imagens", "botaoroxo.png"));
                    break;
                case 4:
                    botao = Image.FromFile(Path.Combine(Application.StartupPath, "imagens", "botaoLaranja.png"));
                    break;
                case 5:
                    botao = Image.FromFile(Path.Combine(Application.StartupPath, "imagens", "botaoverde.png"));
                    break;
            }

            PictureBox pbx = (PictureBox)this.Controls.Find(
                $"pbx_tentativa{tentativas + 1}_casa{posicaoAtual+1}", true
                )[0];   

            pbx.BackgroundImage = botao;

        

        tentativa[posicaoAtual] = cor;
            posicaoAtual++;


        }
        #endregion

        #endregion

        #region pistas

        private void MostrarPistas(int certas, int foraDeLugar)
        {
            List<Image> pistas = new List<Image>();

            Image preta = Image.FromFile(Path.Combine(Application.StartupPath, "imagens", "pistaPreta.png"));
            Image branca = Image.FromFile(Path.Combine(Application.StartupPath, "imagens", "pistaBranca.png"));

            for (int i = 0; i < certas; i++)
            {
                pistas.Add(preta);
            }

            for (int i = 0; i < foraDeLugar; i++)
            {
                pistas.Add(branca);
            }

            PictureBox[] casas = new PictureBox[4];

            for (int i = 0; i < 4; i++)
            {

                casas[i] = (PictureBox)this.Controls.Find(
                $"pbx_pista{tentativas + 1}_{i + 1}", true
                )[0];
            }

            for (int i = 0; i < 4; i++)
            {
                casas[i].BackgroundImage = null;
            }

            for (int i = 0; i < pistas.Count; i++)
            {
                casas[i].BackgroundImage = pistas[i];
            }
        }

        #endregion

        #region Verificar tentativa

        private void btn_confirmar_Click(object sender, EventArgs e)
        {
            
            if (posicaoAtual < tentativa.Length)
            {
                MessageBox.Show("Você precisa selecionar 4 cores antes de verificar sua tentativa.");
                return;
            }

            int certas = 0;
            int foraDeLugar = 0;

            int[] codigoRestante = (int[])codigo.Clone();
            int[] tentativaRestante = (int[])tentativa.Clone();

            for (int i = 0; i < codigo.Length; i++)
            {
                if (tentativaRestante[i] == codigoRestante[i])
                {
                    certas++;
                    codigoRestante[i] = -1; //validacao
                    tentativaRestante[i] = -1; //validacao
                }
            }

            for (int i = 0; i < tentativaRestante.Length; i++)
            {
                if (tentativaRestante[i] == -1)
                    continue;
                
                    for (int j = 0; j < codigoRestante.Length; j++)
                    {
                        if (tentativaRestante[i] != -1 && tentativaRestante[i] == codigoRestante[j])
                        {
                            foraDeLugar++;
                            codigoRestante[j] = -1;
                            break;
                        }
                }
            }

            MostrarPistas(certas, foraDeLugar);

            if (certas == 4)
            {
                MessageBox.Show("Parabéns! Você acertou o código!");
                Cruzeiro cruzeiro = (Cruzeiro)this.FindForm();
                cruzeiro.AbrirTela(new LevelSelectControl(1));
            }

            tentativas++;

            if (tentativas >= 8)
            {
                MessageBox.Show("Você atingiu o número máximo de tentativas! O código correto era: " + string.Join(", ", codigo));
                Cruzeiro cruzeiro = (Cruzeiro)this.FindForm();
                cruzeiro.AbrirTela(new LevelSelectControl(1));

            }

            Array.Clear(tentativa, 0, tentativa.Length);
            posicaoAtual = 0;
            
        }

        #endregion

        #region voltar

        private void btn_voltar_Click(object sender, EventArgs e)
        { 
            
            if (posicaoAtual <= 0)
            {
                return;
            }

         
            PictureBox pbx = (PictureBox)this.Controls.Find(
                $"pbx_tentativa{tentativas+1}_casa{posicaoAtual}", true
                )[0];

            pbx.BackgroundImage = null;

            posicaoAtual--;


        }

        #endregion


    }
}
