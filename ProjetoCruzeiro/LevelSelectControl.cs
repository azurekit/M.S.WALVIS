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
        public int DificuldadeSelecionada;

        public LevelSelectControl(int dificuldadeSelecionada)
        {
            InitializeComponent();
            this.DificuldadeSelecionada = dificuldadeSelecionada;

    
            switch (dificuldadeSelecionada)
            {
                case 1:
                    EsconderInfoLevels();
                    lbl_caso.Text = "ATO 1";
                    break;

                case 2:
                    EsconderInfoLevels();
                    lbl_caso.Text = "ATO 2";
                    break;
                case 3:
                    EsconderInfoLevels();
                    lbl_caso.Text = "ATO 3";
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
        
        #region Esconder info dos levels
        void EsconderInfoLevels()
        {
            lbl_level_titulo.Text = "";
            lbl_level_desc.Text = "";
            lbl_level_highscore.Text = "";
            lbl_level_local.Text = "";
            pbx_level_estrela1.Visible = false;
            pbx_level_estrela2.Visible = false;
            pbx_level_estrela3.Visible = false;
            pnl_level.BackColor = Color.Transparent;
        }
        #endregion

        #region mudar info dos levels

        #region level 1
        private void lbl_level1_button_MouseHover(object sender, EventArgs e)
            {
                if (DificuldadeSelecionada == 1)
            {
                lbl_level_titulo.Text = "O Furto";
                lbl_level_desc.Text = "Descubra a combinação correta!";
                lbl_level_highscore.Text = "Highscore: 1500 pontos";
                lbl_level_local.Text = "Loja";
                pbx_level_estrela1.Visible = true;
                pbx_level_estrela2.Visible = true;
                pbx_level_estrela3.Visible = true;
                pnl_level.BackColor = Color.FromArgb(100, 0, 0, 0);
            } else if (DificuldadeSelecionada == 2)
            {
                lbl_level_titulo.Text = "Pesadelo na Cozinha";
                lbl_level_desc.Text = "Ajuste as balanças!";
                lbl_level_highscore.Text = "Highscore: 3000 pontos";
                lbl_level_local.Text = "Cozinha";
                pbx_level_estrela1.Visible = true;
                pbx_level_estrela2.Visible = true;
                pbx_level_estrela3.Visible = true;
            }
            else if (DificuldadeSelecionada == 3)
            {
                lbl_level_titulo.Text = "Um Terrível Caso";
                lbl_level_desc.Text = "Acerte a posição das válvulas!";
                lbl_level_highscore.Text = "Highscore: 800 pontos";
                lbl_level_local.Text = "Jacuzzi";
                pbx_level_estrela1.Visible = true;
                pbx_level_estrela2.Visible = true;
                pbx_level_estrela3.Visible = true;
            }
        }

        private void lbl_level1_button_MouseLeave(object sender, EventArgs e)
        {
            EsconderInfoLevels();
        }

        #endregion

        #region level 2
        #endregion

        #region level 3
        #endregion

        #endregion

        #region selecionar level

        #region level 1

        void lbl_level1_button_Click(object sender, EventArgs e)
        {

            if (DificuldadeSelecionada == 1)
            {
                Cruzeiro cruzeiro = (Cruzeiro)this.FindForm();
                cruzeiro.AbrirTela(new Ato1Level1());
            }

            if (DificuldadeSelecionada == 2)
            {
                Cruzeiro cruzeiro = (Cruzeiro)this.FindForm();
                cruzeiro.AbrirTela(new Ato2Level1());
            }

        }

        #endregion

        #region level 2

        private void lbl_level2_button_Click(object sender, EventArgs e)
        {
            if (DificuldadeSelecionada == 1)
            {
                Cruzeiro cruzeiro = (Cruzeiro)this.FindForm();
                cruzeiro.AbrirTela(new Ato1Level2());
            }

            if (DificuldadeSelecionada == 2)
            {
                Cruzeiro cruzeiro = (Cruzeiro)this.FindForm();
                cruzeiro.AbrirTela(new Ato2Level2());
            }

        }

        #endregion

        #region level 3

        private void lbl_level3_button_Click(object sender, EventArgs e)
        {
            if (DificuldadeSelecionada == 2)
            {
                Cruzeiro cruzeiro = (Cruzeiro)this.FindForm();
                cruzeiro.AbrirTela(new Ato2Level3());
            }
        }

        #endregion

        #endregion



    }
}
