
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace ProjetoCruzeiro
{
    public partial class LojaControl : UserControl
    {
        #region Variáveis Globais

        private List<EfeitoBotao> efeitos = new List<EfeitoBotao>();

        private PictureBox[] itens = new PictureBox[9];

        private string[] nomes =
        {
            "Chapéu de Palha",
            "Boné Aventureiro",
            "Óculos Chavoso",
            "Máscara Personal",
            "Detetive",
            "Óculos Suspeito",
            "Disfarce",
            "Professor da Unisanta",
            "Bandana Poderosa"
        };

        private int[] precos =
        {
            3000,
            3000,
            8000,
            4000,
            8000,
            3000,
            4000,
            5000,
            3000
        };

        private int cosmeticoSelecionado = 0;

        private string pastaImagens;

        #endregion

        #region Inicialização

        public LojaControl()
        {
            InitializeComponent();

            this.DoubleBuffered = true;

            pastaImagens = Path.Combine(
                Application.StartupPath,
                "imagens",
                "Loja"
            );

            InicializarItens();
            InicializarEfeitos();

            pbx_botao.Click += pbx_botao_Click;
            pbx_voltar.Click += pbx_voltar_Click;

            AtualizarMoedas();

            SelecionarCosmetico(0);
        }

        #endregion

        #region Carregar imagens

        private Image CarregarImagem(string nome)
        {
            string caminho = Path.Combine(pastaImagens, nome);

            if (!File.Exists(caminho))
            {
                MessageBox.Show(
                    "Não foi possível encontrar a imagem:\n" + caminho,
                    "Erro ao carregar imagem",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );

                return null;
            }

            using (Image imagem = Image.FromFile(caminho))
            {
                return new Bitmap(imagem);
            }
        }

        #endregion

        #region Inicializar itens

        private void InicializarItens()
        {
            itens[0] = pbx_item1;
            itens[1] = pbx_item2;
            itens[2] = pbx_item3;
            itens[3] = pbx_item4;
            itens[4] = pbx_item5;
            itens[5] = pbx_item6;
            itens[6] = pbx_item7;
            itens[7] = pbx_item8;
            itens[8] = pbx_item9;

            for (int i = 0; i < itens.Length; i++)
            {
                int identificador = i + 1;

                itens[i].BackgroundImage =
                    CarregarImagem("cosmetico" + identificador + ".png");

                itens[i].BackgroundImageLayout =
                    ImageLayout.Stretch;

                itens[i].Cursor = Cursors.Hand;

                itens[i].Click += (s, e) =>
                {
                    SelecionarCosmetico(identificador);
                };
            }

            pbx_botao.BackgroundImageLayout = ImageLayout.Stretch;
            pbx_voltar.BackgroundImageLayout = ImageLayout.Stretch;
        }

        #endregion

        #region Efeitos dos botões

        private void InicializarEfeitos()
        {
            efeitos.Add(new EfeitoBotao(pbx_voltar));
            efeitos.Add(new EfeitoBotao(pbx_botao));

            foreach (PictureBox item in itens)
            {
                efeitos.Add(new EfeitoBotao(item));
            }
        }

        #endregion

        #region Seleção de cosméticos

        private void SelecionarCosmetico(int identificador)
        {
            cosmeticoSelecionado = identificador;

            for (int i = 0; i < itens.Length; i++)
            {
                int id = i + 1;

                if (id == identificador)
                {
                    itens[i].BackgroundImage =
                        CarregarImagem("cosmetico" + id + "click.png");
                }
                else
                {
                    itens[i].BackgroundImage =
                        CarregarImagem("cosmetico" + id + ".png");
                }
            }

            if (identificador == 0)
            {
                lbl_nomecosmetico.Text = "";
                lbl_precocosmetico.Text = "";
                pbx_botao.BackgroundImage = null;
                return;
            }

            int indice = identificador - 1;

            lbl_nomecosmetico.Text = nomes[indice];

            if (SaveManager.PossuiCosmetico(identificador.ToString()))
            {
                lbl_precocosmetico.Text = "Já adquirido";
            }
            else
            {
                lbl_precocosmetico.Text =
                    precos[indice].ToString("N0") + " moedas";
            }

            AtualizarBotao();
        }

        #endregion

        #region Atualizar botão

        private void AtualizarBotao()
        {
            if (cosmeticoSelecionado == 0)
                return;

            string id = cosmeticoSelecionado.ToString();

            if (SaveManager.PossuiCosmetico(id))
            {
                if (SaveManager.ObterCosmeticoEquipado() == id)
                {
                    pbx_botao.BackgroundImage =
                        CarregarImagem("lojaequipado.png");
                }
                else
                {
                    pbx_botao.BackgroundImage =
                        CarregarImagem("lojaequipar.png");
                }
            }
            else
            {
                pbx_botao.BackgroundImage =
                    CarregarImagem("lojacomprar.png");
            }

            pbx_botao.Cursor = Cursors.Hand;
        }

        #endregion

        #region Comprar e equipar

        private void pbx_botao_Click(object sender, EventArgs e)
        {
            if (cosmeticoSelecionado == 0)
                return;

            string id = cosmeticoSelecionado.ToString();

            if (!SaveManager.PossuiCosmetico(id))
            {
                int preco = precos[cosmeticoSelecionado - 1];

                int moedas = SaveManager.ObterMoedas();

                if (moedas < preco)
                {
                    MessageBox.Show(
                        "Você não possui moedas suficientes para comprar este cosmético!",
                        "Saldo insuficiente",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );

                    return;
                }

                DialogResult resposta = MessageBox.Show(
                    "Deseja realmente comprar " +
                    nomes[cosmeticoSelecionado - 1] +
                    " por " + preco.ToString("N0") + " moedas?",
                    "Confirmar compra",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (resposta != DialogResult.Yes)
                    return;

                SaveManager.DefinirMoedas(moedas - preco);

                SaveManager.AdicionarCosmetico(id);

                AtualizarMoedas();
                SelecionarCosmetico(cosmeticoSelecionado);

                return;
            }

            SaveManager.EquiparCosmetico(id);

            AtualizarBotao();
        }

        #endregion

        #region Moedas

        private void AtualizarMoedas()
        {
            lbl_moedas.Text =
                SaveManager.ObterMoedas().ToString("N0") + " moedas";
        }

        #endregion

        #region Voltar ao menu

        private void pbx_voltar_Click(object sender, EventArgs e)
        {
            Cruzeiro cruzeiro = (Cruzeiro)this.FindForm();

            cruzeiro.AbrirTela(new MenuControl());
        }

        #endregion
    }
}
