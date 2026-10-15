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
    public partial class LevelSelectControl : UserControl
    {
        private int DificuldadeSelecionada;

        public LevelSelectControl(int dificuldadeSelecionada)
        {
            InitializeComponent();
            this.DificuldadeSelecionada = dificuldadeSelecionada;

            switch (dificuldadeSelecionada)
            {
                case 1:
                    lbl_caso.Text = "Caso 1";

                    





                    break;
                case 2:
                    lbl_caso.Text = "Caso 2";
                    break;
                case 3:
                    lbl_caso.Text = "Caso 3";
                    break;

                default:
                    lbl_caso.Text = "Nível Desconhecido";
                    break;
            }
        }

        #region Botão de voltar
        private void btn_menu_Click(object sender, EventArgs e)
        {

            Cruzeiro cruzeiro = (Cruzeiro)this.FindForm();
            cruzeiro.AbrirTela(new DificuldadeSelectControl());
        }


        #endregion



        #region Lixeira
        private void lbl_caso_Click(object sender, EventArgs e)
        {

        }
        #endregion
    }
}
