
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;

namespace ProjetoCruzeiro
{
    public partial class Ato1Level3 : UserControl
    {
        #region Configurações

        const int TAMANHO_TABULEIRO = 9;
        const int TAMANHO_CELULA = 50;

        const int NORTE = 1;
        const int LESTE = 2;
        const int SUL = 4;
        const int OESTE = 8;

        Random random = new Random();

        #endregion

        #region Variáveis Globais

        List<Peca> pecas = new List<Peca>();

        Bitmap imagemReta;
        Bitmap imagemL1;
        Bitmap imagemL2;
        Bitmap imagemT;
        Bitmap imagemTotal;

        Panel painelTabuleiro;

        Label lblStatus;
        Label lblMovimentos;

        int movimentos = 0;
        int movimentosLimite = 0;
        int mapaAtual = -1;

        public int MovimentosRestantes { get; private set; }

        bool jogoFinalizado = false;

        Stopwatch cronometro = new Stopwatch();

        TimeSpan tempoDecorrido;

        int pontuacaoMovimentos = 0;
        int pontuacaoTempo = 0;
        int bonusConclusao = 500;
        int pontuacaoFinal = 0;

        #endregion

        #region Mapas

        string[] mapas =
        {
            "AC66AAAEC" +
            "553BC6C95" +
            "53C39556D" +
            "7A56A9393" +
            "5693AAEAC" +
            "556AAC3C5" +
            "7956C5395" +
            "5AD7C3AAD" +
            "3A993AAAB",

            "ACC6AAAEC" +
            "C53FAAA55" +
            "53C56AA95" +
            "569356C69" +
            "796C533D5" +
            "56D53AC3D" +
            "5553AA965" +
            "3A96EAA79" +
            "3AA93AABA",

            "AC3EAACAC" +
            "63C7CABAD" +
            "56953C569" +
            "556B95795" +
            "5555697A9" +
            "793D5696C" +
            "3AC53AAD5" +
            "6C56C6C35" +
            "33B9393AB",

            "C6EAC3AAC" +
            "3C3C3C6AD" +
            "C3C5697C3" +
            "56953C97C" +
            "7969C3C55" +
            "56D3FA3C5" +
            "555696A95" +
            "3A95696AD" +
            "6AABBA93B",

            "AAAC6EAAC" +
            "6CC595C69" +
            "5553C553C" +
            "53BC397A9" +
            "56AC6C3CC" +
            "555553C55" +
            "53D39C395" +
            "5693AFAAD" +
            "3BAAA96AB",

            "C6C6EC6AC" +
            "3959553AC" +
            "6C3A93AC5" +
            "53AEAA955" +
            "3AC56AC55" +
            "6EA93C555" +
            "556AC5395" +
            "7C56553AD" +
            "53B93BAAB",

            "CAEC6AEA9" +
            "3C539396C" +
            "693AAAA95" +
            "3C6AAAC6D" +
            "539C6A955" +
            "7AC796A95" +
            "56956D6A9" +
            "536953AAC" +
            "3ABAAAAAB",

            "AC6C6C6EC" +
            "C553A9555" +
            "553AC6955" +
            "53AC3BA55" +
            "7AC3AAC3C" +
            "7AAAEC3C5" +
            "56AC56A95" +
            "69AB93AAD" +
            "3AAAAAAAB",

            "AAC3EAAC5" +
            "6A9696C3D" +
            "3AC569369" +
            "69397AA9C" +
            "56EC3C6C5" +
            "5553C393D" +
            "5556AC6C5" +
            "5539C3939" +
            "3BAABAAAA",

            "C6AAEC3AC" +
            "3AAC556C5" +
            "6AC395539" +
            "3C3C6956C" +
            "696956B95" +
            "56AA93CC5" +
            "53AAC6B95" +
            "7AC69396D" +
            "3A3BAAA93"
        };

        #endregion

        #region Classe Peca

        class Peca
        {
            public int Linha;
            public int Coluna;

            public int ConexaoSolucao;
            public int ConexaoAtual;

            public PictureBox Imagem;
        }

        #endregion

        #region Construtor

        public Ato1Level3()
        {
            InitializeComponent();

            Size = new Size(550, 590);
            BackColor = Color.FromArgb(30, 30, 35);

            ConfigurarInterface();

            if (!CarregarImagens())
                return;

            GerarTabuleiro();
        }

        #endregion

        #region Configurar interface

        void ConfigurarInterface()
        {
            lblStatus = new Label();

            lblStatus.Text = "Conecte a entrada à saída.";
            lblStatus.ForeColor = Color.White;
            lblStatus.Font = new Font("Arial", 11, FontStyle.Bold);
            lblStatus.AutoSize = true;
            lblStatus.Location = new Point(20, 12);

            Controls.Add(lblStatus);

            lblMovimentos = new Label();

            lblMovimentos.Text = "MOVIMENTOS ATÉ FALHA CRÍTICA: 0";
            lblMovimentos.ForeColor = Color.White;
            lblMovimentos.Font = new Font("Arial", 10, FontStyle.Bold);
            lblMovimentos.AutoSize = true;
            lblMovimentos.Location = new Point(50, 225);

            Controls.Add(lblMovimentos);

            painelTabuleiro = new Panel();

            painelTabuleiro.Size = new Size(
                TAMANHO_TABULEIRO * TAMANHO_CELULA,
                TAMANHO_TABULEIRO * TAMANHO_CELULA
            );

            painelTabuleiro.Location = new Point(50, 250);
            painelTabuleiro.BackColor = Color.FromArgb(40, 40, 45);

            Controls.Add(painelTabuleiro);
        }

        #endregion

        #region Carregar imagens

        bool CarregarImagens()
        {
            string pasta = Path.Combine(
                Application.StartupPath,
                "imagens"
            );

            try
            {
                imagemReta = new Bitmap(
                    Path.Combine(pasta, "canoreto.png")
                );

                imagemL1 = new Bitmap(
                    Path.Combine(pasta, "canol1.png")
                );

                imagemL2 = new Bitmap(
                    Path.Combine(pasta, "canol2.png")
                );

                imagemT = new Bitmap(
                    Path.Combine(pasta, "canot.png")
                );

                imagemTotal = new Bitmap(
                    Path.Combine(pasta, "canototal.png")
                );

                return true;
            }
            catch (Exception)
            {
                MessageBox.Show(
                    "Não foi possível carregar as imagens dos canos.\n\n" +
                    "Verifique se a pasta 'imagens' está ao lado do executável " +
                    "e contém os cinco arquivos PNG necessários.",
                    "Erro ao carregar imagens",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );

                return false;
            }
        }

        #endregion

        #region Gerar tabuleiro

        void GerarTabuleiro()
        {
            LimparTabuleiro();

            movimentos = 0;
            MovimentosRestantes = 0;
            jogoFinalizado = false;

            mapaAtual = random.Next(mapas.Length);

            string mapa = mapas[mapaAtual];

            for (int linha = 0; linha < TAMANHO_TABULEIRO; linha++)
            {
                for (int coluna = 0; coluna < TAMANHO_TABULEIRO; coluna++)
                {
                    int indice = linha * TAMANHO_TABULEIRO + coluna;

                    int conexao = Convert.ToInt32(
                        mapa[indice].ToString(),
                        16
                    );

                    Peca peca = new Peca();

                    peca.Linha = linha;
                    peca.Coluna = coluna;

                    peca.ConexaoSolucao = conexao;

                    peca.ConexaoAtual = Rotacionar(
                        conexao,
                        random.Next(4)
                    );

                    PictureBox imagem = new PictureBox();

                    imagem.Size = new Size(
                        TAMANHO_CELULA,
                        TAMANHO_CELULA
                    );

                    imagem.Location = new Point(
                        coluna * TAMANHO_CELULA,
                        linha * TAMANHO_CELULA
                    );

                    imagem.SizeMode = PictureBoxSizeMode.StretchImage;
                    imagem.BackColor = Color.FromArgb(40, 40, 45);
                    imagem.Cursor = Cursors.Hand;

                    imagem.Tag = peca;
                    imagem.Click += Peca_Click;

                    peca.Imagem = imagem;

                    pecas.Add(peca);

                    painelTabuleiro.Controls.Add(imagem);

                    AtualizarImagem(peca);
                }
            }

            int tentativas = 0;

            while (VerificarConexao() && tentativas < 10000)
            {
                foreach (Peca peca in pecas)
                {
                    peca.ConexaoAtual = Rotacionar(
                        peca.ConexaoSolucao,
                        random.Next(4)
                    );

                    AtualizarImagem(peca);
                }

                tentativas++;
            }

            movimentosLimite = CalcularLimiteMovimentos();

            MovimentosRestantes = movimentosLimite;

            lblMovimentos.Text =
                "MOVIMENTOS ATÉ FALHA CRÍTICA: " + MovimentosRestantes;

            lblStatus.Text = "Conecte a entrada à saída.";
            lblStatus.ForeColor = Color.White;

            cronometro.Restart();
        }

        void LimparTabuleiro()
        {
            foreach (Peca peca in pecas)
            {
                if (peca.Imagem.Image != null)
                {
                    peca.Imagem.Image.Dispose();
                    peca.Imagem.Image = null;
                }

                peca.Imagem.Dispose();
            }

            pecas.Clear();

            painelTabuleiro.Controls.Clear();
        }

        #endregion

        #region Rotação das peças

        int Rotacionar(int conexoes, int vezes)
        {
            for (int i = 0; i < vezes; i++)
            {
                int novasConexoes = 0;

                if ((conexoes & NORTE) != 0)
                    novasConexoes |= LESTE;

                if ((conexoes & LESTE) != 0)
                    novasConexoes |= SUL;

                if ((conexoes & SUL) != 0)
                    novasConexoes |= OESTE;

                if ((conexoes & OESTE) != 0)
                    novasConexoes |= NORTE;

                conexoes = novasConexoes;
            }

            return conexoes;
        }

        void AtualizarImagem(Peca peca)
        {
            int tipo;
            int rotacao;

            IdentificarPeca(
                peca.ConexaoAtual,
                out tipo,
                out rotacao
            );

            Bitmap imagemOriginal = null;

            switch (tipo)
            {
                case 0:
                    imagemOriginal = imagemReta;
                    break;

                case 1:
                    imagemOriginal = imagemL1;
                    break;

                case 2:
                    imagemOriginal = imagemL2;
                    break;

                case 3:
                    imagemOriginal = imagemT;
                    break;

                case 4:
                    imagemOriginal = imagemTotal;
                    break;
            }

            Bitmap novaImagem = new Bitmap(imagemOriginal);

            for (int i = 0; i < rotacao; i++)
            {
                novaImagem.RotateFlip(
                    RotateFlipType.Rotate90FlipNone
                );
            }

            if (peca.Imagem.Image != null)
                peca.Imagem.Image.Dispose();

            peca.Imagem.Image = novaImagem;
        }

        void IdentificarPeca(
            int conexoes,
            out int tipo,
            out int rotacao)
        {
            int[] bases =
            {
                NORTE | SUL,
                NORTE | OESTE,
                NORTE | LESTE,
                NORTE | LESTE | OESTE,
                NORTE | LESTE | SUL | OESTE
            };

            tipo = 0;
            rotacao = 0;

            for (int i = 0; i < bases.Length; i++)
            {
                for (int giro = 0; giro < 4; giro++)
                {
                    if (Rotacionar(bases[i], giro) == conexoes)
                    {
                        tipo = i;
                        rotacao = giro;
                        return;
                    }
                }
            }
        }

        #endregion

        #region Movimentação

        void Peca_Click(object sender, EventArgs e)
        {
            if (jogoFinalizado)
                return;

            PictureBox imagem = sender as PictureBox;

            Peca peca = imagem.Tag as Peca;

            peca.ConexaoAtual = Rotacionar(
                peca.ConexaoAtual,
                1
            );

            AtualizarImagem(peca);

            movimentos++;

            MovimentosRestantes = Math.Max(
                0,
                movimentosLimite - movimentos
            );

            lblMovimentos.Text =
                "MOVIMENTOS ATÉ FALHA CRÍTICA: " + MovimentosRestantes;

            if (VerificarConexao())
            {
                FinalizarJogo();
                return;
            }

            if (movimentos >= movimentosLimite)
            {
                FinalizarDerrota();
            }
        }

        #endregion

        #region Verificar conexão

        bool VerificarConexao()
        {
            if (pecas.Count != TAMANHO_TABULEIRO * TAMANHO_TABULEIRO)
                return false;

            Peca entrada = ObterPeca(0, 0);
            Peca saida = ObterPeca(8, 8);

            if (entrada == null || saida == null)
                return false;

            if ((entrada.ConexaoAtual & OESTE) == 0)
                return false;

            bool[,] visitados = new bool[
                TAMANHO_TABULEIRO,
                TAMANHO_TABULEIRO
            ];

            Queue<Peca> fila = new Queue<Peca>();

            fila.Enqueue(entrada);

            visitados[0, 0] = true;

            while (fila.Count > 0)
            {
                Peca atual = fila.Dequeue();

                if (atual.Linha == 8 && atual.Coluna == 8)
                {
                    if ((atual.ConexaoAtual & LESTE) != 0)
                        return true;
                }

                VerificarVizinho(
                    atual,
                    atual.Linha - 1,
                    atual.Coluna,
                    NORTE,
                    SUL,
                    visitados,
                    fila
                );

                VerificarVizinho(
                    atual,
                    atual.Linha,
                    atual.Coluna + 1,
                    LESTE,
                    OESTE,
                    visitados,
                    fila
                );

                VerificarVizinho(
                    atual,
                    atual.Linha + 1,
                    atual.Coluna,
                    SUL,
                    NORTE,
                    visitados,
                    fila
                );

                VerificarVizinho(
                    atual,
                    atual.Linha,
                    atual.Coluna - 1,
                    OESTE,
                    LESTE,
                    visitados,
                    fila
                );
            }

            return false;
        }

        void VerificarVizinho(
            Peca atual,
            int linha,
            int coluna,
            int conexaoAtual,
            int conexaoVizinha,
            bool[,] visitados,
            Queue<Peca> fila)
        {
            if (linha < 0 || linha >= TAMANHO_TABULEIRO ||
                coluna < 0 || coluna >= TAMANHO_TABULEIRO)
                return;

            if ((atual.ConexaoAtual & conexaoAtual) == 0)
                return;

            if (visitados[linha, coluna])
                return;

            Peca vizinha = ObterPeca(linha, coluna);

            if (vizinha == null)
                return;

            if ((vizinha.ConexaoAtual & conexaoVizinha) == 0)
                return;

            visitados[linha, coluna] = true;

            fila.Enqueue(vizinha);
        }

        Peca ObterPeca(int linha, int coluna)
        {
            if (linha < 0 || linha >= TAMANHO_TABULEIRO ||
                coluna < 0 || coluna >= TAMANHO_TABULEIRO)
                return null;

            return pecas[linha * TAMANHO_TABULEIRO + coluna];
        }

        #endregion

        #region Caminho da solução

        List<int> ObterCaminhoSolucao()
        {
            int[] anterior = new int[
                TAMANHO_TABULEIRO * TAMANHO_TABULEIRO
            ];

            for (int i = 0; i < anterior.Length; i++)
                anterior[i] = -2;

            Queue<int> fila = new Queue<int>();

            anterior[0] = -1;

            fila.Enqueue(0);

            int[] linhas =
            {
                -1, 0, 1, 0
            };

            int[] colunas =
            {
                0, 1, 0, -1
            };

            int[] conexoes =
            {
                NORTE, LESTE, SUL, OESTE
            };

            int[] opostas =
            {
                SUL, OESTE, NORTE, LESTE
            };

            while (fila.Count > 0)
            {
                int atual = fila.Dequeue();

                if (atual == 80)
                    break;

                int linha = atual / TAMANHO_TABULEIRO;
                int coluna = atual % TAMANHO_TABULEIRO;

                for (int i = 0; i < 4; i++)
                {
                    if ((pecas[atual].ConexaoSolucao & conexoes[i]) == 0)
                        continue;

                    int novaLinha = linha + linhas[i];
                    int novaColuna = coluna + colunas[i];

                    if (novaLinha < 0 || novaLinha >= TAMANHO_TABULEIRO ||
                        novaColuna < 0 || novaColuna >= TAMANHO_TABULEIRO)
                        continue;

                    int vizinho =
                        novaLinha * TAMANHO_TABULEIRO + novaColuna;

                    if ((pecas[vizinho].ConexaoSolucao & opostas[i]) == 0)
                        continue;

                    if (anterior[vizinho] != -2)
                        continue;

                    anterior[vizinho] = atual;

                    fila.Enqueue(vizinho);
                }
            }

            List<int> caminho = new List<int>();

            if (anterior[80] == -2)
                return caminho;

            int posicao = 80;

            while (posicao != -1)
            {
                caminho.Add(posicao);
                posicao = anterior[posicao];
            }

            caminho.Reverse();

            return caminho;
        }

        int CalcularLimiteMovimentos()
        {
            List<int> caminho = ObterCaminhoSolucao();

            int movimentosNecessarios = 0;

            foreach (int indice in caminho)
            {
                Peca peca = pecas[indice];

                int menorRotacao = 4;

                for (int giro = 0; giro < 4; giro++)
                {
                    if (Rotacionar(peca.ConexaoAtual, giro) ==
                        peca.ConexaoSolucao)
                    {
                        menorRotacao = giro;
                        break;
                    }
                }

                movimentosNecessarios += menorRotacao;
            }

            return Math.Max(
                40,
                movimentosNecessarios + 12
            );
        }

        #endregion

        #region Pontuação

        void CalcularPontuacao()
        {
            int bonusConclusao = 500;

            pontuacaoMovimentos = (int)Math.Min(
                3500,
                (MovimentosRestantes / (movimentosLimite * 0.6)) * 3500
            );

            int segundos = (int)Math.Floor(cronometro.Elapsed.TotalSeconds);

            pontuacaoTempo = Math.Max(
                0,
                1000 - Math.Max(0, segundos - 25) * 50
            );

            pontuacaoFinal = bonusConclusao +
                             pontuacaoMovimentos +
                             pontuacaoTempo;

            Console.WriteLine("========== RESULTADO DO NÍVEL ==========");
            Console.WriteLine($"Movimentos restantes: {MovimentosRestantes}");
            Console.WriteLine($"Pontuação por movimentos: {pontuacaoMovimentos}");
            Console.WriteLine($"Pontuação por tempo: {pontuacaoTempo}");
            Console.WriteLine($"Bônus de conclusão: {bonusConclusao}");
            Console.WriteLine($"Pontuação final: {pontuacaoFinal}");
            Console.WriteLine("========================================");
        }

        #endregion

        #region Vitória

        void FinalizarJogo()
        {
            if (jogoFinalizado)
                return;

            jogoFinalizado = true;

            CalcularPontuacao();

            lblStatus.Text = "Conexão estabelecida!";
            lblStatus.ForeColor = Color.LightGreen;

            foreach (Peca peca in pecas)
            {
                peca.Imagem.Enabled = false;
                peca.Imagem.Cursor = Cursors.Default;
            }

            Cruzeiro cruzeiro = (Cruzeiro)this.FindForm();

            cruzeiro.AbrirTela(
                new VitoriaControl(3, pontuacaoFinal)
            );
        }

        #endregion

        #region Derrota

        void FinalizarDerrota()
        {
            if (jogoFinalizado)
                return;

            jogoFinalizado = true;

            cronometro.Stop();

            MovimentosRestantes = 0;

            lblStatus.Text = "Limite de movimentos atingido!";
            lblStatus.ForeColor = Color.OrangeRed;

            foreach (Peca peca in pecas)
            {
                peca.Imagem.Enabled = false;
                peca.Imagem.Cursor = Cursors.Default;
            }

            MessageBox.Show(
                "Você não conseguiu estabelecer a conexão a tempo!\n\n" +
                "Movimentos realizados: " + movimentos +
                "\nMovimentos restantes: " + MovimentosRestantes,
                "Desafio não concluído",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            );

            Cruzeiro cruzeiro = (Cruzeiro)this.FindForm();

            cruzeiro.AbrirTela(new LevelSelectControl(1));
        }

        #endregion
    }
}
