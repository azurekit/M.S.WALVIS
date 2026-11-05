
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ProjetoCruzeiro
{
    public partial class Ato3Level1 : UserControl
    {
        Random random = new Random();

        int pressaoA;
        int pressaoB;
        int pressaoC;
        int pressaoD;

        const int metaA = 5;
        const int metaB = 5;
        const int metaC = 3;
        const int metaD = 4;

        const int pressaoMinima = 0;
        const int pressaoMaxima = 9;

        int movimentosUtilizados = 0;

        bool concluido = false;
        bool venceu = false;

        List<string> sequenciaEmbaralhamento = new List<string>();

        PictureBox pbxAtualA;
        PictureBox pbxAtualB;
        PictureBox pbxAtualC;
        PictureBox pbxAtualD;

        PictureBox pbxMetaA;
        PictureBox pbxMetaB;
        PictureBox pbxMetaC;
        PictureBox pbxMetaD;

        Label lblMovimentos;
        Label lblStatus;

        Button btnAzul;
        Button btnVermelha;
        Button btnVerde;
        Button btnAmarela;
        Button btnBranca;
        Button btnRoxa;
        Button btnReiniciar;

        public int MovimentosUtilizados
        {
            get { return movimentosUtilizados; }
        }

        public bool Concluido
        {
            get { return concluido; }
        }

        public bool Venceu
        {
            get { return venceu; }
        }

        public Ato3Level1()
        {
            InitializeComponent();

            ConfigurarControles();
            ConfigurarEventos();

            GerarConfiguracaoAleatoria();

            lblStatus.Text = "AJUSTE AS PRESSÕES DOS SETORES.";
            AtualizarInterface();
        }

        void ConfigurarControles()
        {
            pbxAtualA = BuscarControle<PictureBox>("pbx_atualA");
            pbxAtualB = BuscarControle<PictureBox>("pbx_atualB");
            pbxAtualC = BuscarControle<PictureBox>("pbx_atualC");
            pbxAtualD = BuscarControle<PictureBox>("pbx_atualD");

            pbxMetaA = BuscarControle<PictureBox>("pbx_metaA");
            pbxMetaB = BuscarControle<PictureBox>("pbx_metaB");
            pbxMetaC = BuscarControle<PictureBox>("pbx_metaC");
            pbxMetaD = BuscarControle<PictureBox>("pbx_metaD");

            lblMovimentos = BuscarControle<Label>("lbl_movimentos");
            lblStatus = BuscarControle<Label>("lbl_status");

            btnAzul = BuscarControle<Button>("btn_azul");
            btnVermelha = BuscarControle<Button>("btn_Vermelho");
            btnVerde = BuscarControle<Button>("btn_verde");
            btnAmarela = BuscarControle<Button>("btn_amarelo");
            btnBranca = BuscarControle<Button>("btn_branco");
            btnRoxa = BuscarControle<Button>("btn_roxo");

            btnReiniciar = BuscarControle<Button>("btn_reiniciar");
        }

        T BuscarControle<T>(string nome) where T : Control
        {
            Control controle = Controls.Find(nome, true).FirstOrDefault();

            if (controle == null)
                throw new Exception("Controle não encontrado: " + nome);

            return controle as T;
        }

        void ConfigurarEventos()
        {
            btnAzul.Click += (s, e) => AplicarValvula(2, -1, 0, 0);
            btnVermelha.Click += (s, e) => AplicarValvula(-1, 2, 0, 0);
            btnVerde.Click += (s, e) => AplicarValvula(0, -1, 2, 0);
            btnAmarela.Click += (s, e) => AplicarValvula(0, 0, -2, 2);
            btnBranca.Click += (s, e) => AplicarValvula(1, 0, -1, 1);
            btnRoxa.Click += (s, e) => AplicarValvula(0, 1, 0, -2);

            btnReiniciar.Click += (s, e) => ReiniciarJogo();
        }

        void GerarConfiguracaoAleatoria()
        {
            int[][] movimentosInversos =
            {
                new int[] { -2,  1,  0,  0 },
                new int[] {  1, -2,  0,  0 },
                new int[] {  0,  1, -2,  0 },
                new int[] {  0,  0,  2, -2 },
                new int[] { -1,  0,  1, -1 },
                new int[] {  0, -1,  0,  2 }
            };

            string[] nomesBotoes =
            {
                "AZUL",
                "VERMELHA",
                "VERDE",
                "AMARELA",
                "BRANCA",
                "ROXA"
            };

            while (true)
            {
                int a = metaA;
                int b = metaB;
                int c = metaC;
                int d = metaD;

                List<string> novaSequencia = new List<string>();
                List<string> movimentosEmbaralhamento = new List<string>();

                int quantidadeMovimentos = random.Next(8, 13);

                for (int i = 0; i < quantidadeMovimentos; i++)
                {
                    List<int> movimentosValidos = new List<int>();

                    for (int j = 0; j < movimentosInversos.Length; j++)
                    {
                        int novoA = a + movimentosInversos[j][0];
                        int novoB = b + movimentosInversos[j][1];
                        int novoC = c + movimentosInversos[j][2];
                        int novoD = d + movimentosInversos[j][3];

                        if (novoA >= pressaoMinima && novoA <= pressaoMaxima &&
                            novoB >= pressaoMinima && novoB <= pressaoMaxima &&
                            novoC >= pressaoMinima && novoC <= pressaoMaxima &&
                            novoD >= pressaoMinima && novoD <= pressaoMaxima)
                        {
                            movimentosValidos.Add(j);
                        }
                    }

                    if (movimentosValidos.Count == 0)
                        break;

                    int escolhido = movimentosValidos[random.Next(movimentosValidos.Count)];

                    a += movimentosInversos[escolhido][0];
                    b += movimentosInversos[escolhido][1];
                    c += movimentosInversos[escolhido][2];
                    d += movimentosInversos[escolhido][3];

                    movimentosEmbaralhamento.Add(nomesBotoes[escolhido]);
                    novaSequencia.Add(nomesBotoes[escolhido]);
                }

                if (a == metaA && b == metaB && c == metaC && d == metaD)
                    continue;

                if (novaSequencia.Count < 8)
                    continue;

                pressaoA = a;
                pressaoB = b;
                pressaoC = c;
                pressaoD = d;

                sequenciaEmbaralhamento = novaSequencia;

                ImprimirSequencia(movimentosEmbaralhamento);

                break;
            }
        }

        void ImprimirSequencia(List<string> movimentosEmbaralhamento)
        {
            Console.WriteLine("");
            Console.WriteLine("========== NOVA CONFIGURACAO ==========");

            Console.WriteLine(
                "PRESSOES INICIAIS: A=" + pressaoA +
                " | B=" + pressaoB +
                " | C=" + pressaoC +
                " | D=" + pressaoD
            );

            Console.WriteLine("");
            Console.WriteLine("========== EMBARALHAMENTO ==========");

            for (int i = 0; i < movimentosEmbaralhamento.Count; i++)
            {
                Console.WriteLine(
                    (i + 1) + " - INVERSO DA VALVULA: " +
                    movimentosEmbaralhamento[i]
                );
            }

            Console.WriteLine("");
            Console.WriteLine("========== SEQUENCIA PARA RESOLVER ==========");

            int contador = 1;

            for (int i = sequenciaEmbaralhamento.Count - 1; i >= 0; i--)
            {
                Console.WriteLine(
                    contador + " - CLICAR NA VALVULA: " +
                    sequenciaEmbaralhamento[i]
                );

                contador++;
            }

            Console.WriteLine("");
            Console.WriteLine(
                "TOTAL DE MOVIMENTOS DA SEQUENCIA: " +
                sequenciaEmbaralhamento.Count
            );

            Console.WriteLine("========================================");
            Console.WriteLine("");
        }

        void AplicarValvula(int a, int b, int c, int d)
        {
            if (concluido)
                return;

            int novoA = pressaoA + a;
            int novoB = pressaoB + b;
            int novoC = pressaoC + c;
            int novoD = pressaoD + d;

            if (novoA < pressaoMinima || novoA > pressaoMaxima ||
                novoB < pressaoMinima || novoB > pressaoMaxima ||
                novoC < pressaoMinima || novoC > pressaoMaxima ||
                novoD < pressaoMinima || novoD > pressaoMaxima)
            {
                lblStatus.Text = "PRESSÃO FORA DOS LIMITES!";
                return;
            }

            pressaoA = novoA;
            pressaoB = novoB;
            pressaoC = novoC;
            pressaoD = novoD;

            movimentosUtilizados++;

            lblStatus.Text = "PRESSÕES ATUALIZADAS.";

            AtualizarInterface();
            VerificarResultado();
        }

        void AtualizarInterface()
        {
            AtualizarImagem(pbxAtualA, pressaoA);
            AtualizarImagem(pbxAtualB, pressaoB);
            AtualizarImagem(pbxAtualC, pressaoC);
            AtualizarImagem(pbxAtualD, pressaoD);

            AtualizarImagem(pbxMetaA, metaA);
            AtualizarImagem(pbxMetaB, metaB);
            AtualizarImagem(pbxMetaC, metaC);
            AtualizarImagem(pbxMetaD, metaD);

            lblMovimentos.Text = "MOVIMENTOS UTILIZADOS: " + movimentosUtilizados;

            HabilitarValvulas(!concluido);
        }

        void AtualizarImagem(PictureBox pictureBox, int pressao)
        {
            string caminho = Path.Combine(
                Application.StartupPath,
                "imagens",
                "pressao" + pressao + ".png"
            );

            using (Image imagem = Image.FromFile(caminho))
            {
                Image imagemAnterior = pictureBox.Image;

                pictureBox.Image = new Bitmap(imagem);

                if (imagemAnterior != null)
                    imagemAnterior.Dispose();
            }
        }

        void VerificarResultado()
        {
            if (pressaoA == metaA &&
                pressaoB == metaB &&
                pressaoC == metaC &&
                pressaoD == metaD)
            {
                concluido = true;
                venceu = true;

                lblStatus.Text = "SISTEMA RESTABELECIDO!";
                HabilitarValvulas(false);
            }
        }

        void HabilitarValvulas(bool habilitar)
        {
            btnAzul.Enabled = habilitar;
            btnVermelha.Enabled = habilitar;
            btnVerde.Enabled = habilitar;
            btnAmarela.Enabled = habilitar;
            btnBranca.Enabled = habilitar;
            btnRoxa.Enabled = habilitar;
        }

        void ReiniciarJogo()
        {
            movimentosUtilizados = 0;
            concluido = false;
            venceu = false;

            GerarConfiguracaoAleatoria();

            lblStatus.Text = "AJUSTE AS PRESSÕES DOS SETORES.";

            AtualizarInterface();
        }
    }
}
