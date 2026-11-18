using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Script.Serialization;

namespace ProjetoCruzeiro
{
    public static class SaveManager
    {
        #region Caminhos

        private static readonly string diretorio =
            Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "Cruzeiro"
            );

        private static readonly string caminhoSave =
            Path.Combine(diretorio, "save.json");

        #endregion

        #region Modelos de Dados

        public class DadosSave
        {
            public ConfiguracoesSave configuracoes { get; set; }
            public ProgressoSave progresso { get; set; }
            public CosmeticosSave cosmeticos { get; set; }

            public List<string> historiasAto3Level2Concluidas { get; set; }

            public DadosSave()
            {
                configuracoes = new ConfiguracoesSave();
                progresso = new ProgressoSave();
                cosmeticos = new CosmeticosSave();
                historiasAto3Level2Concluidas = new List<string>();
            }
        }

        public class ConfiguracoesSave
        {
            public int volume { get; set; }
            public bool mutado { get; set; }

            public ConfiguracoesSave()
            {
                volume = 80;
                mutado = false;
            }
        }

        public class ProgressoSave
        {
            public int moedas { get; set; }
            public Dictionary<string, LevelSave> levels { get; set; }

            public ProgressoSave()
            {
                moedas = 0;
                levels = new Dictionary<string, LevelSave>();

                for (int i = 1; i <= 9; i++)
                    levels.Add(i.ToString(), new LevelSave());
            }
        }

        public class LevelSave
        {
            public bool completo { get; set; }
            public int melhorPontuacao { get; set; }

            public LevelSave()
            {
                completo = false;
                melhorPontuacao = 0;
            }
        }

        public class CosmeticosSave
        {
            public List<string> possuidos { get; set; }
            public string equipado { get; set; }

            public CosmeticosSave()
            {
                possuidos = new List<string>();
                equipado = "";
            }
        }

        #endregion

        #region Variáveis e Propriedades

        public static DadosSave Dados { get; private set; }

        public static string CaminhoArquivo
        {
            get { return caminhoSave; }
        }

        #endregion

        #region Inicialização

        public static void Inicializar()
        {
            if (!Directory.Exists(diretorio))
                Directory.CreateDirectory(diretorio);

            if (File.Exists(caminhoSave))
            {
                string json = File.ReadAllText(caminhoSave);

                JavaScriptSerializer serializer =
                    new JavaScriptSerializer();

                Dados = serializer.Deserialize<DadosSave>(json);

                if (Dados == null)
                    Dados = new DadosSave();
            }
            else
            {
                Dados = new DadosSave();
            }

            PrepararDados();
            Salvar();
        }

        private static void GarantirInicializado()
        {
            if (Dados == null)
                Inicializar();
        }

        #endregion

        #region Preparação e Validação dos Dados

        private static void PrepararDados()
        {
            if (Dados.configuracoes == null)
                Dados.configuracoes = new ConfiguracoesSave();

            if (Dados.progresso == null)
                Dados.progresso = new ProgressoSave();

            if (Dados.cosmeticos == null)
                Dados.cosmeticos = new CosmeticosSave();

            if (Dados.progresso.levels == null)
                Dados.progresso.levels = new Dictionary<string, LevelSave>();

            if (Dados.cosmeticos.possuidos == null)
                Dados.cosmeticos.possuidos = new List<string>();

            if (Dados.cosmeticos.equipado == null)
                Dados.cosmeticos.equipado = "";

            if (Dados.historiasAto3Level2Concluidas == null)
                Dados.historiasAto3Level2Concluidas = new List<string>();

            for (int i = 1; i <= 9; i++)
            {
                string id = i.ToString();

                if (!Dados.progresso.levels.ContainsKey(id) ||
                    Dados.progresso.levels[id] == null)
                {
                    Dados.progresso.levels[id] = new LevelSave();
                }
            }

            if (Dados.configuracoes.volume < 0)
                Dados.configuracoes.volume = 0;

            if (Dados.configuracoes.volume > 100)
                Dados.configuracoes.volume = 100;

            if (!string.IsNullOrEmpty(Dados.cosmeticos.equipado) &&
                !Dados.cosmeticos.possuidos.Contains(Dados.cosmeticos.equipado))
            {
                Dados.cosmeticos.equipado = "";
            }
        }

        #endregion

        #region Salvamento e Persistência

        public static void Salvar()
        {
            if (Dados == null)
                return;

            if (!Directory.Exists(diretorio))
                Directory.CreateDirectory(diretorio);

            JavaScriptSerializer serializer =
                new JavaScriptSerializer();

            serializer.MaxJsonLength = int.MaxValue;

            string json = serializer.Serialize(Dados);

            File.WriteAllText(caminhoSave, json);
        }

        #endregion

        #region Histórias - Ato 3, Nível 2

        public static List<string> ObterHistoriasAto3Level2Concluidas()
        {
            GarantirInicializado();

            if (Dados.historiasAto3Level2Concluidas == null)
                Dados.historiasAto3Level2Concluidas = new List<string>();

            return new List<string>(
                Dados.historiasAto3Level2Concluidas
            );
        }

        public static bool HistoriaAto3Level2FoiConcluida(string nome)
        {
            if (string.IsNullOrWhiteSpace(nome))
                return false;

            return ObterHistoriasAto3Level2Concluidas().Contains(nome);
        }

        public static void MarcarHistoriaAto3Level2Concluida(
            string nome,
            int totalHistorias)
        {
            GarantirInicializado();

            if (string.IsNullOrWhiteSpace(nome))
                return;

            if (Dados.historiasAto3Level2Concluidas == null)
                Dados.historiasAto3Level2Concluidas = new List<string>();

            if (!Dados.historiasAto3Level2Concluidas.Contains(nome))
                Dados.historiasAto3Level2Concluidas.Add(nome);

            if (totalHistorias > 0 &&
                Dados.historiasAto3Level2Concluidas.Count >= totalHistorias)
            {
                Dados.historiasAto3Level2Concluidas.Clear();
            }

            Salvar();
        }

        public static void ReiniciarHistoriasAto3Level2()
        {
            GarantirInicializado();

            Dados.historiasAto3Level2Concluidas.Clear();
            Salvar();
        }

        #endregion

        #region Pontuação e Progresso dos Níveis

        public static void AtualizarLevel(
            int identificador,
            bool completo,
            int melhorPontuacao)
        {
            GarantirInicializado();

            if (identificador < 1 || identificador > 9)
                return;

            string id = identificador.ToString();

            LevelSave level = Dados.progresso.levels[id];

            if (completo)
                level.completo = true;

            if (melhorPontuacao > level.melhorPontuacao)
                level.melhorPontuacao = melhorPontuacao;

            Salvar();
        }

        public static bool EstaConcluido(int identificador)
        {
            GarantirInicializado();

            if (identificador < 1 || identificador > 9)
                return false;

            return Dados.progresso.levels[
                identificador.ToString()
            ].completo;
        }

        public static int ObterHighscore(int identificador)
        {
            GarantirInicializado();

            if (identificador < 1 || identificador > 9)
                return 0;

            return Dados.progresso.levels[
                identificador.ToString()
            ].melhorPontuacao;
        }

        public static int QuantidadeNiveisConcluidos()
        {
            GarantirInicializado();

            int quantidade = 0;

            foreach (LevelSave level in Dados.progresso.levels.Values)
            {
                if (level.completo)
                    quantidade++;
            }

            return quantidade;
        }

        #endregion

        #region Sistema de Moedas

        public static void DefinirMoedas(int quantidade)
        {
            GarantirInicializado();

            Dados.progresso.moedas = Math.Max(0, quantidade);
            Salvar();
        }

        public static int ObterMoedas()
        {
            GarantirInicializado();
            return Dados.progresso.moedas;
        }

        #endregion

        #region Configurações de Áudio

        public static void DefinirVolume(int volume)
        {
            GarantirInicializado();

            Dados.configuracoes.volume =
                Math.Max(0, Math.Min(100, volume));

            Salvar();
        }

        public static int ObterVolume()
        {
            GarantirInicializado();
            return Dados.configuracoes.volume;
        }

        public static void DefinirMutado(bool mutado)
        {
            GarantirInicializado();

            Dados.configuracoes.mutado = mutado;
            Salvar();
        }

        public static bool EstaMutado()
        {
            GarantirInicializado();
            return Dados.configuracoes.mutado;
        }

        #endregion

        #region Sistema de Cosméticos

        public static void AdicionarCosmetico(string identificador)
        {
            GarantirInicializado();

            if (string.IsNullOrWhiteSpace(identificador))
                return;

            if (!Dados.cosmeticos.possuidos.Contains(identificador))
            {
                Dados.cosmeticos.possuidos.Add(identificador);
                Salvar();
            }
        }

        public static bool PossuiCosmetico(string identificador)
        {
            GarantirInicializado();

            return Dados.cosmeticos.possuidos.Contains(identificador);
        }

        public static void RemoverCosmetico(string identificador)
        {
            GarantirInicializado();

            if (Dados.cosmeticos.possuidos.Remove(identificador))
            {
                if (Dados.cosmeticos.equipado == identificador)
                    Dados.cosmeticos.equipado = "";

                Salvar();
            }
        }

        public static List<string> ObterCosmeticos()
        {
            GarantirInicializado();

            return new List<string>(Dados.cosmeticos.possuidos);
        }

        public static void EquiparCosmetico(string identificador)
        {
            GarantirInicializado();

            if (string.IsNullOrWhiteSpace(identificador))
                return;

            if (!PossuiCosmetico(identificador))
                return;

            // Clicar na skin equipada novamente remove o equipamento.
            if (Dados.cosmeticos.equipado == identificador)
            {
                Dados.cosmeticos.equipado = "";
            }
            else
            {
                Dados.cosmeticos.equipado = identificador;
            }

            Salvar();
        }

        public static string ObterCosmeticoEquipado()
        {
            GarantirInicializado();

            return Dados.cosmeticos.equipado;
        }

        #endregion
    }
}