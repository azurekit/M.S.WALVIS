
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ProjetoCruzeiro
{
    public partial class Ato1Level1 : UserControl
    {
        #region Variáveis Globais

        Random random = new Random();

        string[] cameras =
        {
            "Cozinha",
            "Bar",
            "Teatro",
            "Convés",
            "Cassino",
            "Pista de Dança"
        };

        string[] ordemCorreta;
        string[] ordemAtual;

        PictureBox[] monitores;
        Label[] labelsDicas;

        int monitorSelecionado = -1;
        int monitorMouse = -1;

        bool jogoFinalizado = false;

        Timer timerVitoria;

        Stopwatch cronometro = new Stopwatch();

        int movimentos = 0;
        int movimentosMinimos = 0;
        int pontuacaoFinal = 0;

        int pontuacaoMovimentos = 0;
        int pontuacaoTempo = 0;

        TimeSpan tempoDecorrido;

        Dictionary<string, Image[]> imagensMonitores =
            new Dictionary<string, Image[]>();

        enum TipoDica
        {
            Posicao,
            Antes,
            ImediatamenteAntes,
            Extremidade,
            Entre
        }

        class Dica
        {
            public TipoDica Tipo;
            public string Camera;
            public string OutraCamera;
            public string TerceiraCamera;
            public int Posicao;
            public string Texto;
        }

        List<Dica> dicas = new List<Dica>();

        #endregion

        public Ato1Level1()
        {
            InitializeComponent();

            monitores = new PictureBox[]
            {
                pbx_monitor1,
                pbx_monitor2,
                pbx_monitor3,
                pbx_monitor4,
                pbx_monitor5,
                pbx_monitor6
            };

            labelsDicas = new Label[]
            {
                lbl_dica1,
                lbl_dica2,
                lbl_dica3,
                lbl_dica4,
                lbl_dica5
            };

            pbx_monitorloja.Visible = false;
            pbx_monitorloja.Image = null;

            timerVitoria = new Timer();
            timerVitoria.Interval = 13000;
            timerVitoria.Tick += TimerVitoria_Tick;

            CarregarImagens();

            for (int i = 0; i < monitores.Length; i++)
            {
                monitores[i].Cursor = Cursors.Hand;
            }

            IniciarJogo();
        }

        #region Inicialização

        private void IniciarJogo()
        {
            monitorSelecionado = -1;
            monitorMouse = -1;
            jogoFinalizado = false;

            movimentos = 0;
            movimentosMinimos = 0;
            pontuacaoFinal = 0;

            pontuacaoMovimentos = 0;
            pontuacaoTempo = 0;

            GerarOrdem();
            GerarDicas();
            EmbaralharMonitores();

            movimentosMinimos = CalcularMovimentosMinimos();

            ExibirDicas();

            cronometro.Restart();
        }

        #endregion

        #region Imagens dos monitores

        private void CarregarImagens()
        {
            string pasta = Path.Combine(
                Application.StartupPath,
                "imagens",
                "Level1"
            );

            string[] nomes =
            {
                "cameracozinha",
                "camerabar",
                "camerateatro",
                "cameraconves",
                "cameracassino",
                "camerapista"
            };

            foreach (string nome in nomes)
            {
                imagensMonitores.Add(nome, new Image[]
                {
                    Image.FromFile(Path.Combine(pasta, nome + ".gif")),
                    Image.FromFile(Path.Combine(pasta, nome + "hover.gif")),
                    Image.FromFile(Path.Combine(pasta, nome + "click.gif")),
                    Image.FromFile(Path.Combine(pasta, nome + "on.gif"))
                });
            }
        }

        private string NomeArquivo(string camera)
        {
            switch (camera)
            {
                case "Cozinha": return "cameracozinha";
                case "Bar": return "camerabar";
                case "Teatro": return "camerateatro";
                case "Convés": return "cameraconves";
                case "Cassino": return "cameracassino";
                case "Pista de Dança": return "camerapista";
                default: return "";
            }
        }

        private string NomeComArtigo(string camera)
        {
            switch (camera)
            {
                case "Cozinha": return "da Cozinha";
                case "Bar": return "do Bar";
                case "Teatro": return "do Teatro";
                case "Convés": return "do Convés";
                case "Cassino": return "do Cassino";
                case "Pista de Dança": return "da Pista de Dança";
                default: return camera;
            }
        }

        private void AtualizarMonitores()
        {
            if (jogoFinalizado)
                return;

            for (int i = 0; i < monitores.Length; i++)
            {
                string nome = NomeArquivo(ordemAtual[i]);

                monitores[i].Tag = ordemAtual[i];

                int estado;

                if (i == monitorSelecionado)
                {
                    estado = 2;
                }
                else if (i == monitorMouse)
                {
                    estado = 1;
                }
                else
                {
                    estado = 0;
                }

                monitores[i].Image = imagensMonitores[nome][estado];
            }
        }

        #endregion

        #region Hover

        private void Monitor_MouseEnter(object sender, EventArgs e)
        {
            if (jogoFinalizado)
                return;

            PictureBox monitor = sender as PictureBox;

            int indice = Array.IndexOf(monitores, monitor);

            if (indice == -1)
                return;

            monitorMouse = indice;

            AtualizarMonitores();
        }

        private void Monitor_MouseLeave(object sender, EventArgs e)
        {
            if (jogoFinalizado)
                return;

            PictureBox monitor = sender as PictureBox;

            int indice = Array.IndexOf(monitores, monitor);

            if (indice == -1)
                return;

            if (monitorMouse == indice)
                monitorMouse = -1;

            AtualizarMonitores();
        }

        #endregion

        #region Clique e troca de monitores

        private void Monitor_Click(object sender, EventArgs e)
        {
            if (jogoFinalizado)
                return;

            PictureBox monitor = sender as PictureBox;

            int indice = Array.IndexOf(monitores, monitor);

            if (indice == -1)
                return;

            if (monitorSelecionado == -1)
            {
                monitorSelecionado = indice;
            }
            else if (monitorSelecionado == indice)
            {
                monitorSelecionado = -1;
            }
            else
            {
                int primeiro = monitorSelecionado;

                string temp = ordemAtual[primeiro];

                ordemAtual[primeiro] = ordemAtual[indice];
                ordemAtual[indice] = temp;

                movimentos++;

                monitorSelecionado = -1;
            }

            AtualizarMonitores();

            VerificarVitoria();
        }

        #endregion

        #region Gerar e embaralhar ordem

        private void Embaralhar(string[] lista)
        {
            for (int i = lista.Length - 1; i > 0; i--)
            {
                int j = random.Next(0, i + 1);

                string temp = lista[i];
                lista[i] = lista[j];
                lista[j] = temp;
            }
        }

        private void GerarOrdem()
        {
            ordemCorreta = (string[])cameras.Clone();

            Embaralhar(ordemCorreta);

            Console.WriteLine("========== SOLUÇÃO CORRETA ==========");

            for (int i = 0; i < ordemCorreta.Length; i++)
            {
                Console.WriteLine(
                    $"{i + 1}º monitor: {NomeComArtigo(ordemCorreta[i])}"
                );
            }

            Console.WriteLine("=====================================");
            Console.WriteLine();
        }

        private void EmbaralharMonitores()
        {
            ordemAtual = (string[])ordemCorreta.Clone();

            do
            {
                Embaralhar(ordemAtual);

            } while (ordemAtual.SequenceEqual(ordemCorreta));

            AtualizarMonitores();
        }

        #endregion

        #region Cálculo de movimentos mínimos

        private int CalcularMovimentosMinimos()
        {
            bool[] visitados = new bool[ordemAtual.Length];

            int ciclos = 0;

            for (int i = 0; i < ordemAtual.Length; i++)
            {
                if (visitados[i])
                    continue;

                int atual = i;

                while (!visitados[atual])
                {
                    visitados[atual] = true;

                    atual = Array.IndexOf(
                        ordemCorreta,
                        ordemAtual[atual]
                    );
                }

                ciclos++;
            }

            return ordemAtual.Length - ciclos;
        }

        #endregion

        #region Gerar dicas

        private void GerarDicas()
        {
            dicas.Clear();

            List<Dica> candidatas = new List<Dica>();

            for (int i = 0; i < ordemCorreta.Length; i++)
            {
                string camera = ordemCorreta[i];

                candidatas.Add(new Dica
                {
                    Tipo = TipoDica.Posicao,
                    Camera = camera,
                    Posicao = i,
                    Texto = $"A câmera {NomeComArtigo(camera)} está na posição {i + 1}."
                });

                if (i == 0 || i == ordemCorreta.Length - 1)
                {
                    candidatas.Add(new Dica
                    {
                        Tipo = TipoDica.Extremidade,
                        Camera = camera,
                        Texto = $"A câmera {NomeComArtigo(camera)} está em uma das extremidades."
                    });
                }

                for (int j = i + 1; j < ordemCorreta.Length; j++)
                {
                    string outra = ordemCorreta[j];

                    candidatas.Add(new Dica
                    {
                        Tipo = TipoDica.Antes,
                        Camera = camera,
                        OutraCamera = outra,
                        Texto = $"A câmera {NomeComArtigo(camera)} aparece antes {NomeComArtigo(outra)}."
                    });
                }

                if (i < ordemCorreta.Length - 1)
                {
                    string outra = ordemCorreta[i + 1];

                    candidatas.Add(new Dica
                    {
                        Tipo = TipoDica.ImediatamenteAntes,
                        Camera = camera,
                        OutraCamera = outra,
                        Texto = $"A câmera {NomeComArtigo(camera)} aparece imediatamente antes {NomeComArtigo(outra)}."
                    });
                }

                for (int esquerda = 0; esquerda < i; esquerda++)
                {
                    for (int direita = i + 1; direita < ordemCorreta.Length; direita++)
                    {
                        string primeira = ordemCorreta[esquerda];
                        string terceira = ordemCorreta[direita];

                        candidatas.Add(new Dica
                        {
                            Tipo = TipoDica.Entre,
                            Camera = camera,
                            OutraCamera = primeira,
                            TerceiraCamera = terceira,
                            Texto = $"A câmera {NomeComArtigo(camera)} está entre as câmeras {NomeComArtigo(primeira)} e {NomeComArtigo(terceira)}."
                        });
                    }
                }
            }

            for (int tentativa = 0; tentativa < 5000; tentativa++)
            {
                EmbaralharDicas(candidatas);

                dicas.Clear();

                for (int i = 0; i < 5; i++)
                {
                    dicas.Add(candidatas[i]);
                }

                if (ContarSolucoes() == 1)
                    return;
            }

            dicas.Clear();

            for (int i = 0; i < 5; i++)
            {
                string camera = ordemCorreta[i];

                dicas.Add(new Dica
                {
                    Tipo = TipoDica.Posicao,
                    Camera = camera,
                    Posicao = i,
                    Texto = $"A câmera {NomeComArtigo(camera)} está na posição {i + 1}."
                });
            }
        }

        private void EmbaralharDicas(List<Dica> lista)
        {
            for (int i = lista.Count - 1; i > 0; i--)
            {
                int j = random.Next(0, i + 1);

                Dica temp = lista[i];
                lista[i] = lista[j];
                lista[j] = temp;
            }
        }

        #endregion

        #region Verificar dicas e soluções

        private bool RespeitaDica(string[] ordem, Dica dica)
        {
            int posicao = Array.IndexOf(ordem, dica.Camera);
            int outra = Array.IndexOf(ordem, dica.OutraCamera);
            int terceira = Array.IndexOf(ordem, dica.TerceiraCamera);

            switch (dica.Tipo)
            {
                case TipoDica.Posicao:
                    return posicao == dica.Posicao;

                case TipoDica.Antes:
                    return posicao < outra;

                case TipoDica.ImediatamenteAntes:
                    return posicao + 1 == outra;

                case TipoDica.Extremidade:
                    return posicao == 0 || posicao == ordem.Length - 1;

                case TipoDica.Entre:
                    return (outra < posicao && posicao < terceira) ||
                           (terceira < posicao && posicao < outra);
            }

            return false;
        }

        private int ContarSolucoes()
        {
            int quantidade = 0;

            string[] tentativa = new string[cameras.Length];

            bool[] usados = new bool[cameras.Length];

            GerarPermutacoes(tentativa, usados, 0, ref quantidade);

            return quantidade;
        }

        private void GerarPermutacoes(
            string[] tentativa,
            bool[] usados,
            int nivel,
            ref int quantidade)
        {
            if (quantidade >= 2)
                return;

            if (nivel == cameras.Length)
            {
                foreach (Dica dica in dicas)
                {
                    if (!RespeitaDica(tentativa, dica))
                        return;
                }

                quantidade++;

                return;
            }

            for (int i = 0; i < cameras.Length; i++)
            {
                if (usados[i])
                    continue;

                tentativa[nivel] = cameras[i];

                usados[i] = true;

                GerarPermutacoes(
                    tentativa,
                    usados,
                    nivel + 1,
                    ref quantidade
                );

                usados[i] = false;

                if (quantidade >= 2)
                    return;
            }
        }

        #endregion

        #region Exibir dicas

        private void ExibirDicas()
        {
            for (int i = 0; i < labelsDicas.Length; i++)
            {
                labelsDicas[i].Text = dicas[i].Texto;
                labelsDicas[i].Visible = true;
            }
        }

        #endregion

        #region Pontuação

        private void CalcularPontuacao()
        {
            tempoDecorrido = cronometro.Elapsed;

            cronometro.Stop();

            int movimentosExtras = Math.Max(
                0,
                movimentos - movimentosMinimos
            );

            pontuacaoMovimentos = Math.Max(
                0,
                3500 - (movimentosExtras * 200)
            );

            int segundosExcedentes = Math.Max(
                0,
                (int)Math.Ceiling(tempoDecorrido.TotalSeconds) - 20
            );

            pontuacaoTempo = Math.Max(
                0,
                1000 - (segundosExcedentes * 50)
            );

            int bonusConclusao = 500;

            pontuacaoFinal =
                pontuacaoMovimentos +
                bonusConclusao +
                pontuacaoTempo;
        }

        #endregion

        #region Vitória

        private void VerificarVitoria()
        {
            if (!ordemAtual.SequenceEqual(ordemCorreta))
                return;

            jogoFinalizado = true;

            CalcularPontuacao();

            monitorSelecionado = -1;
            monitorMouse = -1;

            foreach (PictureBox monitor in monitores)
            {
                monitor.Cursor = Cursors.Default;
            }

            pbx_monitorloja.Cursor = Cursors.Default;

            for (int i = 0; i < monitores.Length; i++)
            {
                string nome = NomeArquivo(ordemAtual[i]);

                monitores[i].Image = imagensMonitores[nome][3];
            }

            pbx_monitorloja.Image = Image.FromFile(
                Path.Combine(
                    Application.StartupPath,
                    "imagens",
                    "Level1",
                    "cameraloja.gif"
                )
            );

            pbx_monitorloja.Visible = true;

            timerVitoria.Start();
        }

        private void TimerVitoria_Tick(object sender, EventArgs e)
        {
            timerVitoria.Stop();

            Control controle = this;

            while (controle != null && !(controle is JogoForm))
            {
                controle = controle.Parent;
            }

            JogoForm jogo = controle as JogoForm;

            if (jogo != null)
            {
                jogo.MostrarVitoria(1, pontuacaoFinal);
            }

            timerVitoria.Dispose();
        }

        #endregion
    }
}
