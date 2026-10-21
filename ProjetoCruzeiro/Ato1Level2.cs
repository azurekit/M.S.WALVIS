using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjetoCruzeiro
{
    public partial class Ato1Level2 : UserControl
    {

        #region Variáveis Globais

        private int[] dados = new int[3];
        private Random random = new Random();

        #endregion
        public Ato1Level2()
        {
            InitializeComponent();
            IniciarJogo();

        }

        #region Iniciar jogo


        private List<Dica> dicasAtuais = new List<Dica>();

        private void IniciarJogo()
        {

            lbl_dica1.Text = "";
            lbl_dica2.Text = "";
            lbl_dica3.Text = "";
            lbl_dica4.Text = "";
            nud_dadoA.Value = 1;
            nud_dadoB.Value = 1;
            nud_dadoC.Value = 1;

            while (true)
            {
                List<int> Valores = new List<int> { 1, 2, 3, 4, 5, 6 };

                for (int i = 0; i <dados.Length; i++)
                {
                    int indice = random.Next(Valores.Count);
                    dados[i] = Valores[indice];
                    Valores.RemoveAt(indice);
                }

                Console.WriteLine(dados[0] + " " + dados[1] + " " + dados[2]);

                List<Dica> candidatas = GerarDicas(dados[0], dados[1], dados[2]);

                candidatas = candidatas.Where(d => d.Regra(dados[0], dados[1], dados[2])).OrderBy(d => random.Next()).ToList();

                List<Dica> escolhidas = new List<Dica>();

                foreach (Dica dica in candidatas){

                    escolhidas.Add(dica);

                    List<int[]> solucoes = EncontrarSolucoes(escolhidas);

                    if (solucoes.Count == 1)
                    {
                        dicasAtuais = escolhidas;
                        MostrarDicas();
                        return;
                    }
                }

            }




        }
        #endregion

        #region Dicas

        #region Classe
        class Dica
        {
            public string Texto { get; set; }
            public Func<int, int, int, bool> Regra { get; set; }
            
            public Dica (string texto, Func<int, int, int, bool> regra)
            {
                Texto = texto;
                Regra = regra;
            }
            
        }
        #endregion

        #region Gerar dicas

        private List<Dica> GerarDicas(int a, int b, int c)
        {
            List<Dica> dicas = new List<Dica>();

            dicas.Add(new Dica(
                "O dado A é par.",
                (x, y, z) => x % 2 == 0
                ));

            dicas.Add(new Dica(
                "O dado A é ímpar.",
                (x, y, z) => x % 2 == 1
                ));

            dicas.Add(new Dica(
                "O dado B é par.",
                (x, y, z) => y % 2 == 0
                ));

            dicas.Add(new Dica(
                "O dado B é ímpar.",
                (x, y, z) => y % 2 == 1
                ));

            dicas.Add(new Dica(
               "O dado C é par.",
               (x, y, z) => z % 2 == 0
               ));

            dicas.Add(new Dica(
                "O dado C é ímpar.",
                (x, y, z) => z % 2 == 1
                ));

            dicas.Add(new Dica(
                "O dado A é maior que B.",
                (x, y, z) => x > y
                ));

            dicas.Add(new Dica(
                "O dado A é menor que C.",
                (x, y, z) => x < z
                ));

            dicas.Add(new Dica(
                "O dado B é menor que C.",
                (x, y, z) => y < z
                ));
            dicas.Add(new Dica(
                "O dado B é maior que C.",
                (x, y, z) => y > z
                ));

            dicas.Add(new Dica(
                "O dado B é maior que A.",
                (x, y, z) => y > x
                ));

            dicas.Add(new Dica(
                "A soma de A e C é " + (a + c) + ".",
                (x, y, z) => x + z == a + c
                ));

            dicas.Add(new Dica(
                "A soma de A e B é " + (a + b) + ".",
                (x, y, z) => x + y == a + b
                ));

            dicas.Add(new Dica(
                "A soma de B e C é " + (a + c) + ".",
                (x, y, z) => y + z == a + c
                ));

            dicas.Add(new Dica(
                "A soma dos dados é " + (a + b + c) + ".",
                (x, y, z) => x + y + z == a + b + c
                ));

            dicas.Add(new Dica(
                "A diferença de A e C é " + Math.Abs(a - c) + ".",
                (x, y, z) => Math.Abs(x - z) == Math.Abs(a - c)
                ));

            dicas.Add(new Dica(
                "A diferença de A e B é " + Math.Abs(a - b) + ".",
                (x, y, z) => Math.Abs(x - y) == Math.Abs(a - b)
                ));

            dicas.Add(new Dica(
                "A diferença de B e C é " + Math.Abs(a - c) + ".",
                (x, y, z) => Math.Abs(y - z) == Math.Abs(a - c)
                ));
            return dicas;
        }


        #endregion

        #region Verificar combinação
        
        private bool Combina(int a, int b, int c, List<Dica> dicas)
        {
            foreach (Dica dica in dicas)
            {
                if (!dica.Regra(a, b, c))
                {
                    return false;
                }
            }
            return true;
        }

        #endregion

        #region Verificar soluções possíveis

        private List<int[]> EncontrarSolucoes(List<Dica> dicas)
        {
            List<int[]> solucoes = new List<int[]>();

            for (int a = 1; a <= 6; a++) 
            {
                for (int b = 1; b <= 6; b++)
                {
                    for (int c = 1; c <= 6; c++)
                    {
                        if (a== b || a == c || b == c)
                        {
                            continue;
                        }

                        if (Combina(a, b, c, dicas))
                        {
                            solucoes.Add(new int[] { a, b, c });
                        }
                    }
                }
            }
            return solucoes;
        }

        #endregion

        #endregion

        #region Mostrar dicas

        private void MostrarDicas()
        {
            lbl_dica1.Text = dicasAtuais.Count > 0 ? dicasAtuais[0].Texto : "";
            lbl_dica2.Text = dicasAtuais.Count > 1 ? dicasAtuais[1].Texto : "";
            lbl_dica3.Text = dicasAtuais.Count > 2 ? dicasAtuais[2].Texto : "";
            lbl_dica4.Text = dicasAtuais.Count > 3 ? dicasAtuais[3].Texto : "";
        }

        #endregion

        #region Botão de confirmar
        private void btn_confirmar_Click(object sender, EventArgs e)
        {
            int a = Convert.ToInt32(nud_dadoA.Value);
            int b = Convert.ToInt32(nud_dadoB.Value);
            int c = Convert.ToInt32(nud_dadoC.Value);

            if (a == b || b == c || b == c)
            {
                MessageBox.Show("Os dados não podem repetir valores!!");
                return;
            }

            if (a == dados[0] && b == dados[1] && c == dados[2])
            {
                MessageBox.Show("Você acertou!");
                IniciarJogo();
            }
            else
            {
                MessageBox.Show("Combinação incorreta!");
                return;
            }


        }
        #endregion

        #region lixeira

        private void lbl_dica3_Click(object sender, EventArgs e)
        {

        }
        #endregion
    }
}
