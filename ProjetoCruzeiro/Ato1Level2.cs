
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Diagnostics;
using System.Windows.Forms;

namespace ProjetoCruzeiro
{
    public partial class Ato1Level2 : UserControl
    {
        #region Variáveis Globais

        private int[] dados = new int[3];
        private Random random = new Random();

        private Stopwatch cronometro = new Stopwatch();

        private int tentativas = 0;

        private int pontuacaoTempo = 0;
        private int pontuacaoTentativas = 0;
        private int pontuacaoFinal = 0;

        private int bonusConclusao = 500;

        private TimeSpan tempoDecorrido;

        private bool jogoFinalizado = false;

        private List<Dica> dicasAtuais = new List<Dica>();

        #endregion

        public Ato1Level2()
        {
            InitializeComponent();
            IniciarJogo();
        }

        #region Iniciar jogo

        private void IniciarJogo()
        {
            lbl_dica1.Text = "";
            lbl_dica2.Text = "";
            lbl_dica3.Text = "";
            lbl_dica4.Text = "";

            nud_dadoA.Value = 1;
            nud_dadoB.Value = 1;
            nud_dadoC.Value = 1;

            tentativas = 0;

            pontuacaoTempo = 0;
            pontuacaoTentativas = 0;
            pontuacaoFinal = 0;

            jogoFinalizado = false;

            dicasAtuais.Clear();

            while (true)
            {
                List<int> valores = new List<int> { 1, 2, 3, 4, 5, 6 };

                for (int i = 0; i < dados.Length; i++)
                {
                    int indice = random.Next(valores.Count);
                    dados[i] = valores[indice];
                    valores.RemoveAt(indice);
                }

                List<Dica> candidatas = GerarDicas(
                    dados[0],
                    dados[1],
                    dados[2]
                );

                candidatas = candidatas
                    .Where(d => d.Regra(dados[0], dados[1], dados[2]))
                    .OrderBy(d => random.Next())
                    .ToList();

                List<Dica> escolhidas = new List<Dica>();

                foreach (Dica dica in candidatas)
                {
                    escolhidas.Add(dica);

                    List<int[]> solucoes = EncontrarSolucoes(escolhidas);

                    if (solucoes.Count == 1)
                    {
                        dicasAtuais = new List<Dica>(escolhidas);
                        break;
                    }
                }

                if (EncontrarSolucoes(dicasAtuais).Count == 1)
                {
                    MostrarDicas();

                    cronometro.Restart();

                    Console.WriteLine("========== NOVA COMBINAÇÃO ==========");
                    Console.WriteLine($"Dado A: {dados[0]}");
                    Console.WriteLine($"Dado B: {dados[1]}");
                    Console.WriteLine($"Dado C: {dados[2]}");
                    Console.WriteLine($"Quantidade de dicas: {dicasAtuais.Count}");
                    Console.WriteLine("=====================================");

                    return;
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

            public Dica(string texto, Func<int, int, int, bool> regra)
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
                "A soma de B e C é " + (b + c) + ".",
                (x, y, z) => y + z == b + c
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
                "A diferença de B e C é " + Math.Abs(b - c) + ".",
                (x, y, z) => Math.Abs(y - z) == Math.Abs(b - c)
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
                    return false;
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
                        if (a == b || a == c || b == c)
                            continue;

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

        #region Pontuação

        private void CalcularPontuacao()
        {
            tempoDecorrido = cronometro.Elapsed;

            cronometro.Stop();

            int segundosExcedentes = Math.Max(
                0,
                (int)Math.Ceiling(tempoDecorrido.TotalSeconds) - 20
            );

            pontuacaoTempo = Math.Max(
                0,
                3500 - (segundosExcedentes * 40)
            );

            pontuacaoTentativas = Math.Max(
                0,
                1000 - ((tentativas - 1) * 200)
            );

            pontuacaoFinal =
                bonusConclusao +
                pontuacaoTempo +
                pontuacaoTentativas;

            Console.WriteLine("========== RESULTADO DO NÍVEL ==========");
            Console.WriteLine($"Tentativas: {tentativas}");
            Console.WriteLine($"Tempo: {tempoDecorrido.TotalSeconds:F1} segundos");
            Console.WriteLine($"Bônus de conclusão: {bonusConclusao}");
            Console.WriteLine($"Pontuação por tempo: {pontuacaoTempo}");
            Console.WriteLine($"Pontuação por tentativas: {pontuacaoTentativas}");
            Console.WriteLine($"Pontuação final: {pontuacaoFinal}");
            Console.WriteLine("========================================");
        }

        #endregion

        #region Vitória

        private void VerificarVitoria()
        {
            jogoFinalizado = true;

            CalcularPontuacao();

            Cruzeiro cruzeiro = (Cruzeiro)this.FindForm();

            cruzeiro.AbrirTela(new VitoriaControl(2, pontuacaoFinal));
        }

        #endregion

        #region Botão de confirmar

        private void btn_confirmar_Click(object sender, EventArgs e)
        {
            if (jogoFinalizado)
                return;

            int a = Convert.ToInt32(nud_dadoA.Value);
            int b = Convert.ToInt32(nud_dadoB.Value);
            int c = Convert.ToInt32(nud_dadoC.Value);

            if (a == b || a == c || b == c)
            {
                tentativas++;

                MessageBox.Show("Os dados não podem repetir valores!");
                return;
            }

            tentativas++;

            if (a == dados[0] && b == dados[1] && c == dados[2])
            {
                VerificarVitoria();
            }
            else
            {
                MessageBox.Show("Combinação incorreta!");
            }
        }

        #endregion

        #region Lixeira

        private void lbl_dica3_Click(object sender, EventArgs e)
        {

        }

        #endregion
    }
}
