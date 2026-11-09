
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ProjetoCruzeiro
{
    public partial class Ato3Level3 : UserControl
    {
        #region Constantes

        private const int TEMPO_MAXIMO_CONVES = 20;
        private const int INTERVALO_CRONOMETRO_CONVES = 100;
        private const int TOTAL_PERGUNTAS_CONVES = 7;

        #endregion

        #region Classes

        private class PerguntaConves
        {
            public string EnunciadoConves;
            public string[] OpcoesConves;
            public int RespostaCorretaConves;

            public PerguntaConves(
                string enunciado,
                string[] opcoes,
                int respostaCorreta)
            {
                EnunciadoConves = enunciado;
                OpcoesConves = opcoes;
                RespostaCorretaConves = respostaCorreta;
            }
        }

        private class GifAssetConves
        {
            public Image ImagemConves;
            public MemoryStream StreamConves;
            public int DuracaoConves;

            public GifAssetConves(string caminho)
            {
                if (!File.Exists(caminho))
                    return;

                StreamConves = new MemoryStream(File.ReadAllBytes(caminho));
                ImagemConves = Image.FromStream(StreamConves);
                DuracaoConves = CalcularDuracaoConves(ImagemConves);
            }

            private int CalcularDuracaoConves(Image imagem)
            {
                try
                {
                    FrameDimension dimensao =
                        new FrameDimension(imagem.FrameDimensionsList[0]);

                    int quadros = imagem.GetFrameCount(dimensao);
                    int duracao = 0;

                    PropertyItem propriedade = imagem.GetPropertyItem(0x5100);

                    for (int i = 0; i < quadros; i++)
                    {
                        int centesimos =
                            BitConverter.ToInt32(propriedade.Value, i * 4);

                        duracao += centesimos * 10;
                    }

                    return Math.Max(500, Math.Min(duracao, 10000));
                }
                catch
                {
                    return 1800;
                }
            }

            public void LiberarConves()
            {
                ImagemConves?.Dispose();
                StreamConves?.Dispose();
            }
        }

        #endregion

        #region Variáveis

        private readonly Random aleatorioConves = new Random();

        private readonly List<PerguntaConves> perguntasConves =
            new List<PerguntaConves>();

        private int perguntaAtualConves;
        private int vidasConves = 3;
        private int errosConves;
        private int tempoRestanteConves;

        private bool respondendoConves;
        private bool dueloEncerradoConves;

        private GifAssetConves gioIdleConves;
        private GifAssetConves gioAcertoConves;
        private GifAssetConves gioErroConves;

        private Button[] botoesOpcoesConves;

        private Timer timerAnimacaoConves;
        private Timer timerTempoConves;

        public event Action<bool> DueloFinalizadoConves;

        #endregion

        #region Inicialização

        public Ato3Level3()
        {
            InitializeComponent();

            botoesOpcoesConves = new Button[]
            {
                btnOpcaoAConves,
                btnOpcaoBConves,
                btnOpcaoCConves,
                btnOpcaoDConves
            };

            ConfigurarEventosConves();
            CriarTimersConves();
            ConfigurarBarraTempoConves();
            CarregarRecursosConves();
            PrepararPartidaConves();

            Disposed += (s, e) =>
            {
                timerAnimacaoConves?.Stop();
                timerAnimacaoConves?.Dispose();

                timerTempoConves?.Stop();
                timerTempoConves?.Dispose();

                gioIdleConves?.LiberarConves();
                gioAcertoConves?.LiberarConves();
                gioErroConves?.LiberarConves();
            };
        }

        private void ConfigurarEventosConves()
        {
            btnOpcaoAConves.Click += (s, e) => ResponderConves(0);
            btnOpcaoBConves.Click += (s, e) => ResponderConves(1);
            btnOpcaoCConves.Click += (s, e) => ResponderConves(2);
            btnOpcaoDConves.Click += (s, e) => ResponderConves(3);
        }

        private void CriarTimersConves()
        {
            timerAnimacaoConves = new Timer();
            timerAnimacaoConves.Tick += FinalizarAnimacaoConves;

            timerTempoConves = new Timer();
            timerTempoConves.Interval = INTERVALO_CRONOMETRO_CONVES;
            timerTempoConves.Tick += AtualizarTempoConves;
        }

        private void ConfigurarBarraTempoConves()
        {
            prgTempoConves.Minimum = 0;
            prgTempoConves.Maximum = TEMPO_MAXIMO_CONVES * 10;
            prgTempoConves.Value = prgTempoConves.Maximum;
            prgTempoConves.Style = ProgressBarStyle.Continuous;
            prgTempoConves.RightToLeft = RightToLeft.Yes;
        }

        private void CarregarRecursosConves()
        {
            string pastaConves = Path.Combine(
                Application.StartupPath,
                "imagens"
            );

            gioIdleConves = new GifAssetConves(
                Path.Combine(pastaConves, "gioidle.gif")
            );

            gioAcertoConves = new GifAssetConves(
                Path.Combine(pastaConves, "gioacerto.gif")
            );

            gioErroConves = new GifAssetConves(
                Path.Combine(pastaConves, "gioerro.gif")
            );

            MostrarIdleConves();
            AtualizarVidasConves();
        }

        #endregion

        #region Interface e cronômetro

        private void MostrarIdleConves()
        {
            pbxGioConves.Image = gioIdleConves?.ImagemConves;
        }

        private void AtualizarVidasConves()
        {
            string caminhoConves = Path.Combine(
                Application.StartupPath,
                "imagens",
                "vidas" + vidasConves + ".png"
            );

            if (!File.Exists(caminhoConves))
            {
                pbxVidasConves.Image = null;
                return;
            }

            using (Image imagemConves = Image.FromFile(caminhoConves))
            {
                Image imagemAnteriorConves = pbxVidasConves.Image;

                pbxVidasConves.Image = new Bitmap(imagemConves);

                imagemAnteriorConves?.Dispose();
            }
        }

        private void IniciarCronometroConves()
        {
            timerTempoConves.Stop();

            tempoRestanteConves = TEMPO_MAXIMO_CONVES * 10;

            prgTempoConves.Value = prgTempoConves.Maximum;

            timerTempoConves.Start();
        }

        private void PararCronometroConves()
        {
            timerTempoConves.Stop();
        }

        private void AtualizarTempoConves(object sender, EventArgs e)
        {
            if (respondendoConves || dueloEncerradoConves)
            {
                timerTempoConves.Stop();
                return;
            }

            tempoRestanteConves--;

            if (tempoRestanteConves < 0)
                tempoRestanteConves = 0;

            prgTempoConves.Value = tempoRestanteConves;

            if (tempoRestanteConves <= 0)
            {
                PararCronometroConves();
                ProcessarRespostaConves(-1);
            }
        }

        private void ReproduzirAnimacaoConves(bool acertouConves)
        {
            GifAssetConves gifConves =
                acertouConves ? gioAcertoConves : gioErroConves;

            if (gifConves?.ImagemConves == null)
                gifConves = gioIdleConves;

            if (gifConves?.ImagemConves != null)
            {
                pbxGioConves.Image = gifConves.ImagemConves;
                timerAnimacaoConves.Interval = gifConves.DuracaoConves;
            }
            else
            {
                timerAnimacaoConves.Interval = 4000;
            }

            timerAnimacaoConves.Start();
        }

        #endregion

        #region Gerenciamento da partida

        private void PrepararPartidaConves()
        {
            timerAnimacaoConves.Stop();
            timerTempoConves.Stop();

            perguntaAtualConves = 0;
            vidasConves = 3;
            errosConves = 0;
            tempoRestanteConves = TEMPO_MAXIMO_CONVES * 10;

            respondendoConves = false;
            dueloEncerradoConves = false;

            perguntasConves.Clear();

            GerarPerguntasConves();

            foreach (Button botaoConves in botoesOpcoesConves)
            {
                botaoConves.Visible = true;
                botaoConves.Enabled = true;
            }

            painelOpcoesConves.Visible = true;

            lblResultadoConves.Text = "";

            MostrarIdleConves();
            AtualizarIndicadoresConves();
            MostrarPerguntaConves();
        }

        private void GerarPerguntasConves()
        {
            List<PerguntaConves> bancoConves = new List<PerguntaConves>
            {
                CriarPerguntaSequenciaNumericaConves(),
                CriarPerguntaSequenciaAlternadaConves(),
                CriarPerguntaLetrasConves(),
                CriarPerguntaUltrapassagemConves(),
                CriarPerguntaOrientacaoConves(),
                CriarPerguntaOrdenacaoConves(),
                CriarPerguntaCondicionalConves(),
                CriarPerguntaDeducaoConves(),
                CriarPerguntaClassificacaoConves(),
                CriarPerguntaFamiliaConves(),
                CriarPerguntaCalendarioConves(),
                CriarPerguntaRelacaoNumericaConves(),
                CriarPerguntaOperacaoConves(),
                CriarPerguntaPosicaoConves(),
                CriarPerguntaContagemConves(),
                CriarPerguntaCategoriasConves(),
                CriarPerguntaSequenciaDiferencasConves(),
                CriarPerguntaLogicaConves()
            };

            perguntasConves.AddRange(
                bancoConves
                    .OrderBy(x => aleatorioConves.Next())
                    .Take(TOTAL_PERGUNTAS_CONVES)
            );
        }

        private void MostrarPerguntaConves()
        {
            if (perguntaAtualConves >= perguntasConves.Count)
            {
                FinalizarDueloConves(true);
                return;
            }

            PerguntaConves perguntaConves = perguntasConves[perguntaAtualConves];

            lblNumeroPerguntaConves.Text =
                "PERGUNTA " + (perguntaAtualConves + 1) + " DE 7";

            lblPerguntaConves.Text = perguntaConves.EnunciadoConves;

            lblDialogoConves.Text =
                "Gio: \"Vamos ver se consegue resolver esta.\"";

            foreach (Button botaoConves in botoesOpcoesConves)
            {
                botaoConves.Enabled = true;
                botaoConves.BackColor = Color.FromArgb(200, 200, 200);
            }

            for (int i = 0; i < 4; i++)
            {
                botoesOpcoesConves[i].Text =
                    ((char)('A' + i)) + ") " + perguntaConves.OpcoesConves[i];
            }

            lblResultadoConves.Text = "";

            AtualizarIndicadoresConves();
            MostrarIdleConves();
            IniciarCronometroConves();
        }

        private void AtualizarIndicadoresConves()
        {
            AtualizarVidasConves();

            lblNumeroPerguntaConves.Text =
                "PERGUNTA " + Math.Min(perguntaAtualConves + 1, 7) + " DE 7";
        }

        private void ResponderConves(int indiceConves)
        {
            if (respondendoConves || dueloEncerradoConves)
                return;

            ProcessarRespostaConves(indiceConves);
        }

        private void ProcessarRespostaConves(int indiceConves)
        {
            if (respondendoConves || dueloEncerradoConves ||
                perguntaAtualConves >= perguntasConves.Count)
                return;

            respondendoConves = true;

            PararCronometroConves();

            PerguntaConves perguntaConves = perguntasConves[perguntaAtualConves];

            bool acertouConves =
                indiceConves >= 0 &&
                indiceConves == perguntaConves.RespostaCorretaConves;

            foreach (Button botaoConves in botoesOpcoesConves)
                botaoConves.Enabled = false;

            if (acertouConves)
            {
                botoesOpcoesConves[indiceConves].BackColor =
                    Color.FromArgb(35, 115, 75);

                lblDialogoConves.Text =
                    "Gio: \"Não esperava que você acertasse essa...\"";

                lblResultadoConves.Text = "RESPOSTA CORRETA!";
            }
            else
            {
                vidasConves--;
                errosConves++;

                if (indiceConves >= 0)
                {
                    botoesOpcoesConves[indiceConves].BackColor =
                        Color.FromArgb(140, 45, 45);
                }

                botoesOpcoesConves[
                    perguntaConves.RespostaCorretaConves
                ].BackColor = Color.FromArgb(35, 115, 75);

                if (indiceConves == -1)
                {
                    lblDialogoConves.Text =
                        "Gio: \"O tempo acabou, Coala. Você demorou demais.\"";

                    lblResultadoConves.Text =
                        "TEMPO ESGOTADO! VOCÊ PERDEU UMA VIDA.";
                }
                else
                {
                    lblDialogoConves.Text =
                        "Gio: \"Parece que você não é tão esperto assim.\"";

                    lblResultadoConves.Text =
                        "RESPOSTA INCORRETA! VOCÊ PERDEU UMA VIDA.";
                }

                AtualizarIndicadoresConves();
            }

            ReproduzirAnimacaoConves(acertouConves);
        }

        private void FinalizarAnimacaoConves(object sender, EventArgs e)
        {
            timerAnimacaoConves.Stop();

            if (dueloEncerradoConves)
                return;

            if (errosConves >= 3)
            {
                FinalizarDueloConves(false);
                return;
            }

            perguntaAtualConves++;

            if (perguntaAtualConves >= TOTAL_PERGUNTAS_CONVES)
            {
                FinalizarDueloConves(true);
                return;
            }

            respondendoConves = false;

            lblResultadoConves.Text = "";

            MostrarPerguntaConves();
        }

        private void FinalizarDueloConves(bool venceuConves)
        {
            if (dueloEncerradoConves)
                return;

            dueloEncerradoConves = true;
            respondendoConves = true;

            timerAnimacaoConves.Stop();
            timerTempoConves.Stop();

            foreach (Button botaoConves in botoesOpcoesConves)
                botaoConves.Visible = false;

            painelOpcoesConves.Visible = false;

            prgTempoConves.Value = 0;

            if (venceuConves)
            {
                lblPerguntaConves.Text = "VOCÊ VENCEU O DUELO!";

                lblDialogoConves.Text =
                    "Gio: \"Impressionante... você realmente conseguiu me superar.\"";

                lblResultadoConves.Text = "VITÓRIA!";

                ReproduzirAnimacaoConves(true);
            }
            else
            {
                lblPerguntaConves.Text = "VOCÊ PERDEU O DUELO!";

                lblDialogoConves.Text =
                    "Gio: \"Fim de jogo, Coala. Você não foi páreo para mim.\"";

                lblResultadoConves.Text = "DERROTA!";

                ReproduzirAnimacaoConves(false);
            }

            DueloFinalizadoConves?.Invoke(venceuConves);
        }

        #endregion

        #region Banco de perguntas

        private PerguntaConves CriarPerguntaSequenciaNumericaConves()
        {
            return PerguntaTextoConves(
                "Complete a sequência:\n\n2, 6, 12, 20, 30, ?",
                "42",
                new[] { "40", "41", "44" }
            );
        }

        private PerguntaConves CriarPerguntaSequenciaAlternadaConves()
        {
            return PerguntaTextoConves(
                "Qual número completa a sequência?\n\n2, 5, 4, 10, 6, 15, 8, ?",
                "20",
                new[] { "16", "18", "24" }
            );
        }

        private PerguntaConves CriarPerguntaLetrasConves()
        {
            return PerguntaTextoConves(
                "Qual letra vem a seguir?\n\nA, C, F, J, O, ?",
                "U",
                new[] { "T", "V", "W" }
            );
        }

        private PerguntaConves CriarPerguntaUltrapassagemConves()
        {
            return PerguntaTextoConves(
                "Em uma corrida, você ultrapassa o segundo colocado. Em qual posição fica?",
                "Segundo",
                new[] { "Primeiro", "Terceiro", "Último" }
            );
        }

        private PerguntaConves CriarPerguntaOrientacaoConves()
        {
            string[] direcoesConves =
            {
                "Norte", "Leste", "Sul", "Oeste"
            };

            int direcaoInicialConves = aleatorioConves.Next(0, 4);

            int rotacaoConves = 0;

            string[] movimentosConves =
            {
                "direita", "direita", "esquerda"
            };

            foreach (string movimentoConves in movimentosConves)
            {
                if (movimentoConves == "direita")
                    rotacaoConves++;
                else
                    rotacaoConves--;
            }

            int resultadoConves =
                (direcaoInicialConves + rotacaoConves + 4) % 4;

            string[] opcoesConves =
            {
                direcoesConves[(resultadoConves + 1) % 4],
                direcoesConves[(resultadoConves + 2) % 4],
                direcoesConves[(resultadoConves + 3) % 4]
            };

            return PerguntaTextoConves(
                "Você está olhando para " +
                direcoesConves[direcaoInicialConves] +
                ". Vira à direita duas vezes e depois à esquerda. Para onde olha?",
                direcoesConves[resultadoConves],
                opcoesConves
            );
        }

        private PerguntaConves CriarPerguntaOrdenacaoConves()
        {
            string[] nomesConves =
            {
                "Alice", "Bruno", "Caio", "Diana"
            };

            nomesConves = nomesConves
                .OrderBy(x => aleatorioConves.Next())
                .ToArray();

            string primeiroConves = nomesConves[0];
            string segundoConves = nomesConves[1];
            string terceiroConves = nomesConves[2];
            string quartoConves = nomesConves[3];

            string enunciadoConves =
                "Quatro pessoas chegaram ao navio.\n" +
                primeiroConves + " chegou antes de " + segundoConves + ".\n" +
                segundoConves + " chegou antes de " + terceiroConves + ".\n" +
                terceiroConves + " chegou antes de " + quartoConves + ".\n\n" +
                "Quem chegou em terceiro lugar?";

            return PerguntaTextoConves(
                enunciadoConves,
                terceiroConves,
                new[]
                {
                    primeiroConves,
                    segundoConves,
                    quartoConves
                }
            );
        }

        private PerguntaConves CriarPerguntaCondicionalConves()
        {
            string[] objetosConves =
            {
                "alarme", "motor", "elevador", "sistema"
            };

            string objetoConves =
                objetosConves[aleatorioConves.Next(objetosConves.Length)];

            return PerguntaTextoConves(
                "Se o " + objetoConves +
                " está ligado, a luz fica acesa.\n" +
                "A luz está apagada. O que podemos concluir?",
                "O " + objetoConves + " está desligado.",
                new[]
                {
                    "O " + objetoConves + " está ligado.",
                    "A luz está com defeito.",
                    "Não é possível concluir nada."
                }
            );
        }


        private PerguntaConves CriarPerguntaDeducaoConves()
        {
            string[][] gruposConves =
            {
        new[] { "músicos", "marinheiros", "mecânicos", "cozinheiros",
                "atores", "pintores", "pilotos", "detetives" }
    };

            string[] sorteadosConves = gruposConves[0]
                .OrderBy(x => aleatorioConves.Next())
                .Take(4)
                .ToArray();

            string grupoAConves = sorteadosConves[0];
            string grupoBConves = sorteadosConves[1];
            string grupoCConves = sorteadosConves[2];
            string grupoDConves = sorteadosConves[3];

            string respostaConves =
                "Alguns " + grupoDConves + " não são " + grupoCConves + ".";

            return PerguntaTextoConves(
                "Todos os " + grupoAConves + " também são " + grupoBConves + ".\n" +
                "Nenhum " + grupoBConves + " pertence ao grupo dos " + grupoCConves + ".\n" +
                "Alguns " + grupoDConves + " também são " + grupoAConves + ".\n\n" +
                "Qual conclusão é necessariamente verdadeira?",
                respostaConves,
                new[]
                {
            "Todos os " + grupoDConves + " são " + grupoBConves + ".",
            "Nenhum dos " + grupoDConves + " pertence ao grupo dos " + grupoBConves + ".",
            "Todos os " + grupoAConves + " também são " + grupoDConves + "."
                }
            );
        }


        private PerguntaConves CriarPerguntaClassificacaoConves()
        {
            return PerguntaTextoConves(
                "Qual número não pertence ao grupo?\n\n16, 25, 36, 49, 63, 64",
                "63",
                new[] { "36", "49", "64" }
            );
        }

        private PerguntaConves CriarPerguntaFamiliaConves()
        {
            string[] relacoesConves =
            {
                "irmão da sua mãe",
                "pai do seu pai",
                "filho da sua irmã",
                "irmã do seu pai"
            };

            int indiceConves = aleatorioConves.Next(0, 4);

            string[] respostasConves =
            {
                "Tio",
                "Avô",
                "Sobrinho",
                "Tia"
            };

            return PerguntaTextoConves(
                "O que é seu " + relacoesConves[indiceConves] + "?",
                respostasConves[indiceConves],
                respostasConves
                    .Where(x => x != respostasConves[indiceConves])
                    .ToArray()
            );
        }

        private PerguntaConves CriarPerguntaCalendarioConves()
        {
            string[] diasConves =
            {
                "domingo", "segunda-feira", "terça-feira",
                "quarta-feira", "quinta-feira", "sexta-feira", "sábado"
            };

            int diaInicialConves = aleatorioConves.Next(0, 7);
            int diasAdianteConves = aleatorioConves.Next(30, 151);

            int resultadoConves =
                (diaInicialConves + diasAdianteConves % 7) % 7;

            List<string> erradasConves = new List<string>();

            for (int i = 1; i <= 3; i++)
            {
                erradasConves.Add(
                    diasConves[(resultadoConves + i) % 7]
                );
            }

            return PerguntaTextoConves(
                "Se hoje é " + diasConves[diaInicialConves] +
                ", que dia será daqui a " + diasAdianteConves + " dias?",
                diasConves[resultadoConves],
                erradasConves.ToArray()
            );
        }

        private PerguntaConves CriarPerguntaRelacaoNumericaConves()
        {
            int numeroConves = aleatorioConves.Next(3, 10);

            int respostaConves =
                numeroConves * numeroConves + 1;

            return PerguntaTextoConves(
                "Descubra o padrão:\n\n" +
                "3 → 10\n5 → 26\n7 → 50\n" +
                numeroConves + " → ?",
                respostaConves.ToString(),
                new[]
                {
                    (respostaConves - 2).ToString(),
                    (respostaConves + 2).ToString(),
                    (numeroConves * 2 + 1).ToString()
                }
            );
        }

        private PerguntaConves CriarPerguntaOperacaoConves()
        {
            int primeiroConves = aleatorioConves.Next(2, 7);

            int segundoConves = primeiroConves + 1;

            int respostaConves =
                primeiroConves * primeiroConves +
                segundoConves * segundoConves;

            return PerguntaTextoConves(
                "Uma operação segue sempre a mesma regra:\n\n" +
                "2 ★ 3 = 13\n" +
                "3 ★ 4 = 25\n" +
                "4 ★ 5 = 41\n\n" +
                primeiroConves + " ★ " + segundoConves + " = ?",
                respostaConves.ToString(),
                new[]
                {
                    (respostaConves - 2).ToString(),
                    (respostaConves + 2).ToString(),
                    (primeiroConves * segundoConves).ToString()
                }
            );
        }

        private PerguntaConves CriarPerguntaPosicaoConves()
        {
            string[] nomesConves =
            {
                "Ana", "Bia", "Caio", "Davi", "Eva"
            };

            nomesConves = nomesConves
                .OrderBy(x => aleatorioConves.Next())
                .ToArray();

            string[] posicoesConves =
            {
                "primeiro", "segundo", "terceiro", "quarto", "quinto"
            };

            int indiceConves = aleatorioConves.Next(0, 5);

            return PerguntaTextoConves(
                "Em uma fila, " + nomesConves[0] +
                " está antes de " + nomesConves[1] +
                ", que está antes de " + nomesConves[2] +
                ". " + nomesConves[3] +
                " está depois de todos eles.\n\n" +
                "Quem está em " + posicoesConves[2] + "?",
                nomesConves[2],
                new[]
                {
                    nomesConves[0],
                    nomesConves[1],
                    nomesConves[3]
                }
            );
        }

        private PerguntaConves CriarPerguntaContagemConves()
        {
            int pessoasConves = aleatorioConves.Next(4, 7);

            int apertosConves =
                pessoasConves * (pessoasConves - 1) / 2;

            return PerguntaTextoConves(
                pessoasConves +
                " pessoas se cumprimentam uma única vez, " +
                "cada uma com todas as outras.\n" +
                "Quantos cumprimentos acontecem?",
                apertosConves.ToString(),
                new[]
                {
                    (apertosConves - 1).ToString(),
                    (apertosConves + pessoasConves).ToString(),
                    (pessoasConves * (pessoasConves - 1)).ToString()
                }
            );
        }

        private PerguntaConves CriarPerguntaCategoriasConves()
        {
            string[][] categoriasConves =
            {
                new[] { "Tubarão", "Golfinho", "Baleia", "Sardinha" },
                new[] { "Mercúrio", "Vênus", "Marte", "Lua" },
                new[] { "Triângulo", "Quadrado", "Pentágono", "Círculo" },
                new[] { "Ferro", "Ouro", "Prata", "Madeira" }
            };

            string[][] respostasConves =
            {
                new[] { "Sardinha", "Tubarão", "Golfinho", "Baleia" },
                new[] { "Lua", "Mercúrio", "Vênus", "Marte" },
                new[] { "Círculo", "Triângulo", "Quadrado", "Pentágono" },
                new[] { "Madeira", "Ferro", "Ouro", "Prata" }
            };

            int indiceConves = aleatorioConves.Next(0, 4);

            string[] categoriaConves = categoriasConves[indiceConves];

            string[] alternativasConves = categoriaConves
                .OrderBy(x => aleatorioConves.Next())
                .ToArray();

            return PerguntaTextoConves(
                "Qual elemento não pertence ao grupo?\n\n" +
                string.Join(", ", categoriaConves),
                respostasConves[indiceConves][0],
                alternativasConves
                    .Where(x => x != respostasConves[indiceConves][0])
                    .Take(3)
                    .ToArray()
            );
        }

        private PerguntaConves CriarPerguntaSequenciaDiferencasConves()
        {
            int inicioConves = aleatorioConves.Next(2, 10);

            int incrementoConves = aleatorioConves.Next(3, 8);

            int primeiroConves = inicioConves;
            int segundoConves = primeiroConves + incrementoConves;
            int terceiroConves = segundoConves + incrementoConves * 2;
            int quartoConves = terceiroConves + incrementoConves * 3;
            int respostaConves = quartoConves + incrementoConves * 4;

            return PerguntaTextoConves(
                "Complete a sequência:\n\n" +
                primeiroConves + ", " +
                segundoConves + ", " +
                terceiroConves + ", " +
                quartoConves + ", ?",
                respostaConves.ToString(),
                new[]
                {
                    (respostaConves - incrementoConves).ToString(),
                    (respostaConves + incrementoConves).ToString(),
                    (respostaConves + 2).ToString()
                }
            );
        }

        private PerguntaConves CriarPerguntaLogicaConves()
        {
            string[] nomesConves =
            {
                "A", "B", "C", "D"
            };

            nomesConves = nomesConves
                .OrderBy(x => aleatorioConves.Next())
                .ToArray();

            string aConves = nomesConves[0];
            string bConves = nomesConves[1];
            string cConves = nomesConves[2];
            string dConves = nomesConves[3];

            return PerguntaTextoConves(
                "Se " + aConves + " é maior que " + bConves +
                " e " + bConves + " é maior que " + cConves +
                ", qual afirmação é obrigatoriamente verdadeira?",
                aConves + " é maior que " + cConves + ".",
                new[]
                {
                    cConves + " é maior que " + aConves + ".",
                    aConves + " é igual a " + cConves + ".",
                    dConves + " é maior que " + aConves + "."
                }
            );
        }

        #endregion

        #region Montagem das alternativas

        private PerguntaConves PerguntaTextoConves(
            string enunciadoConves,
            string respostaConves,
            string[] erradasConves)
        {
            List<string> alternativasConves =
                new List<string> { respostaConves };

            foreach (string erradaConves in erradasConves)
            {
                if (!alternativasConves.Contains(erradaConves))
                    alternativasConves.Add(erradaConves);
            }

            alternativasConves = alternativasConves
                .OrderBy(x => aleatorioConves.Next())
                .ToList();

            return new PerguntaConves(
                enunciadoConves,
                alternativasConves.ToArray(),
                alternativasConves.IndexOf(respostaConves)
            );
        }

        #endregion
    }
}
