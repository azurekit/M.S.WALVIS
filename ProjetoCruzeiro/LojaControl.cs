
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
            3000, 3000, 8000, 4000, 8000,
            3000, 4000, 5000, 3000
        };

        private int cosmeticoSelecionado = 0;
        private string pastaImagens;
        private string pastaExpressoes;
        private Random random = new Random();

        #endregion

        #region Inicialização

        public LojaControl()
        {
            InitializeComponent();

            DoubleBuffered = true;

            pastaImagens = Path.Combine(
                Application.StartupPath,
                "imagens",
                "Loja"
            );

            pastaExpressoes = Path.Combine(
                Application.StartupPath,
                "imagens",
                "MenuLateral",
                "Coala"
            );

            InicializarItens();
            InicializarEfeitos();

            // Evita que o evento seja executado duas vezes.
            pbx_botao.Click -= pbx_botao_Click;
            pbx_botao.Click += pbx_botao_Click;

            pbx_espelho.BackgroundImageLayout = ImageLayout.Stretch;

            AtualizarMoedas();
            AtualizarEspelho(ObterSkinEquipada());

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

                itens[i].BackgroundImageLayout = ImageLayout.Stretch;
                itens[i].Cursor = Cursors.Hand;

                itens[i].Click += (s, e) =>
                {
                    SelecionarCosmetico(identificador);
                    AtualizarEspelho(identificador);
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

        #region Espelho do personagem

        private int ObterSkinEquipada()
        {
            string equipada = SaveManager.ObterCosmeticoEquipado();

            int skin;

            if (int.TryParse(equipada, out skin))
            {
                if (skin >= 1 && skin <= 9)
                    return skin;
            }

            return 0;
        }

        private void AtualizarEspelho(int skin)
        {
            if (skin < 0 || skin > 9)
                skin = 0;

            int expressao = random.Next(1, 9);

            string caminho = Path.Combine(
                pastaExpressoes,
                "item" + skin,
                "expressao" + expressao + "-" + skin + ".png"
            );

            if (!File.Exists(caminho))
            {
                MessageBox.Show(
                    "Não foi possível encontrar a expressão:\n" + caminho,
                    "Erro ao carregar expressão",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );

                return;
            }

            Image imagem;

            using (Image original = Image.FromFile(caminho))
            {
                imagem = new Bitmap(original);
            }

            if (pbx_espelho.BackgroundImage != null)
            {
                Image anterior = pbx_espelho.BackgroundImage;
                pbx_espelho.BackgroundImage = null;
                anterior.Dispose();
            }

            pbx_espelho.BackgroundImage = imagem;
            pbx_espelho.BackgroundImageLayout = ImageLayout.Stretch;
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

            // Equipa ou desequipa a skin selecionada.
            SaveManager.EquiparCosmetico(id);

            // Atualiza usando o estado realmente salvo.
            int skinEquipada = ObterSkinEquipada();

            AtualizarEspelho(skinEquipada);
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
            Cruzeiro cruzeiro = (Cruzeiro)FindForm();
            cruzeiro.AbrirTela(new MenuControl());
        }

        #endregion

        #region lixeira
        private void LojaControl_Load(object sender, EventArgs e)
        {
        }

        private void lbl_moedas_Click(object sender, EventArgs e)
        {

        }

        #endregion
    }
}
