using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoCruzeiro
{
    public class ProgressoJogador
    {
        public int Pontuacaogeral { get; set; } = 0;
        
        public int[] HighscoreLevels { get; set; } = new int[9]; // Array para armazenar a pontuação de cada nível

        public List<int> LevelsDesbloqueados { get; set; } = new List<int> { 1 }; // Lista para armazenar os níveis desbloqueados

        public List<string> CosmeticosDesbloqueados { get; set; } = new List<string> (); // Lista para armazenar os cosméticos desbloqueados por ID

        public void RegistrarPontuacao(int level, int pontuacao)
        {
            if (level < 1 || level > 9 || pontuacao < 0)
            {
                return;
            }

            Pontuacaogeral += pontuacao;

            if (pontuacao > HighscoreLevels[level - 1])
            {
                HighscoreLevels[level - 1] = pontuacao;
            }
            else
            {   
                return;
            }
        }


    }
}
