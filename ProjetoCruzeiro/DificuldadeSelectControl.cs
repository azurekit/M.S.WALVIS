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
    public partial class DificuldadeSelectControl : UserControl
    {
        #region Variáveis Globais

        int dificuldadeSelecionada = 0; // 1 = Fácil, 2 = Médio, 3 = Difícil

        #endregion

        public DificuldadeSelectControl()
        {
            InitializeComponent();
        }

        #region Botão de voltar
        private void btn_menu_Click(object sender, EventArgs e)
        {
            Cruzeiro cruzeiro = (Cruzeiro)this.FindForm();
            cruzeiro.AbrirTela(new MenuControl());
        }
        #endregion

        #region Botões de dificuldade
        private void btn_caso1_Click(object sender, EventArgs e)
        {
            dificuldadeSelecionada = 1; // Fácil
            Cruzeiro cruzeiro = (Cruzeiro)this.FindForm();
            cruzeiro.AbrirTela(new LevelSelectControl(dificuldadeSelecionada));
        }

        private void btn_caso2_Click(object sender, EventArgs e)
        {
            dificuldadeSelecionada = 2; // Médio
            Cruzeiro cruzeiro = (Cruzeiro)this.FindForm();
            cruzeiro.AbrirTela(new LevelSelectControl(dificuldadeSelecionada));
        }

        private void btn_caso3_Click(object sender, EventArgs e)
        {
            dificuldadeSelecionada = 3; // Difícil
            Cruzeiro cruzeiro = (Cruzeiro)this.FindForm();
            cruzeiro.AbrirTela(new LevelSelectControl(dificuldadeSelecionada));
        }

        #endregion

        #region lixeira
        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click_1(object sender, EventArgs e)
        {

        }

        private void DificuldadeControl_Load(object sender, EventArgs e)
        {

        }


        #endregion


    }
}
