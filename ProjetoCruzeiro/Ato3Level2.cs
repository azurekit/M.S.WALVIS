
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ProjetoCruzeiro
{
    public partial class Ato3Level2 : UserControl
    {
        private class Historia
        {
            public string Nome;
            public List<string> Falas;

            public Historia(string nome, List<string> falas)
            {
                Nome = nome;
                Falas = falas;
            }
        }

        private List<Historia> historias = new List<Historia>();
        private List<int> historiasDisponiveis = new List<int>();

        private Historia historiaAtual;

        private List<Panel> cartoes = new List<Panel>();

        private Random random = new Random();

        private bool vitoria = false;

        public Ato3Level2()
        {
            InitializeComponent();

            ConfigurarInterface();
            CarregarHistorias();
            SortearHistoria();
        }

        private void ConfigurarInterface()
        {
            flpFalas.AllowDrop = true;
            flpFalas.AutoScroll = true;
            flpFalas.WrapContents = false;
            flpFalas.FlowDirection = FlowDirection.TopDown;

            flpResposta.AllowDrop = true;
            flpResposta.AutoScroll = true;
            flpResposta.WrapContents = false;
            flpResposta.FlowDirection = FlowDirection.TopDown;

            flpFalas.DragEnter += DragEnterFalas;
            flpFalas.DragDrop += DragDropFalas;

            flpResposta.DragEnter += DragEnterFalas;
            flpResposta.DragDrop += DragDropFalas;

            btnConfirmar.Click += ConfirmarResposta;
            btnReiniciar.Click += ReiniciarResposta;
        }

        private void CarregarHistorias()
        {
            historias = new List<Historia>
            {
                new Historia("O Último Ato", new List<string>
                {
                    "REI: O documento finalmente chegou?",
                    "GUARDA: Sim, majestade. Foi encontrado nos portões.",
                    "REI: E quem o trouxe até aqui?",
                    "GUARDA: Um mensageiro desconhecido.",
                    "CONSELHEIRO: Não devemos confiar em qualquer mensageiro.",
                    "REI: Você reconhece o selo nesta carta?",
                    "CONSELHEIRO: É o símbolo da família real.",
                    "GUARDA: Então devo chamar os outros conselheiros?",
                    "REI: Não. Primeiro precisamos descobrir quem enviou a mensagem.",
                    "CONSELHEIRO: Nesse caso, sugiro que fechemos as portas do castelo."
                }),

                new Historia("A Carta Perdida", new List<string>
                {
                    "ATOR: Você encontrou a minha carta?",
                    "ATRIZ: Encontrei algo sobre a mesa do camarim.",
                    "ATOR: Essa é a carta que estou procurando!",
                    "ATRIZ: Mas o envelope está aberto.",
                    "ATOR: Alguém leu o conteúdo?",
                    "ATRIZ: Não sei. Quando cheguei, ele já estava assim.",
                    "ATOR: Preciso entregar essa carta antes do espetáculo.",
                    "ATRIZ: Então é melhor procurarmos o destinatário.",
                    "ATOR: O nome está escrito no verso."
                }),

                new Historia("O Ensaio Interrompido", new List<string>
                {
                    "DIRETOR: Podemos começar o ensaio?",
                    "ATOR: Ainda não encontrei meu figurino.",
                    "ATRIZ: Ele está pendurado atrás da cortina.",
                    "ATOR: Não é esse. O meu possui uma faixa dourada.",
                    "DIRETOR: A equipe de figurino trouxe uma caixa agora há pouco.",
                    "ATRIZ: Talvez esteja dentro dela.",
                    "ATOR: Encontrei! Estava no fundo da caixa.",
                    "DIRETOR: Ótimo. Todos aos seus lugares, vamos começar."
                }),

                new Historia("O Segredo do Camarim", new List<string>
                {
                    "ATRIZ: Você ouviu esse barulho?",
                    "ATOR: Veio de dentro do camarim.",
                    "ATRIZ: Mas pensei que estivéssemos sozinhos.",
                    "ATOR: A porta está trancada.",
                    "ATRIZ: E a chave desapareceu.",
                    "ATOR: Espere, há um bilhete embaixo da porta.",
                    "ATRIZ: Está escrito que devemos procurar no espelho.",
                    "ATOR: Há uma pequena gaveta atrás dele.",
                    "ATRIZ: E aqui está a chave!",
                    "ATOR: Agora podemos descobrir quem está lá dentro."
                }),

                new Historia("A Substituição", new List<string>
                {
                    "DIRETOR: O ator principal não chegou?",
                    "ASSISTENTE: Infelizmente, ele cancelou a participação.",
                    "DIRETOR: A apresentação começa em poucos minutos!",
                    "ATOR: Eu conheço o texto e posso assumir o papel.",
                    "ASSISTENTE: Mas você nunca interpretou o protagonista.",
                    "ATOR: Já participei de todos os ensaios.",
                    "DIRETOR: Você consegue decorar a cena final?",
                    "ATOR: Consigo. Sei exatamente o que preciso dizer.",
                    "ASSISTENTE: Então vou preparar seu figurino.",
                    "DIRETOR: Muito bem. O espetáculo não será cancelado."
                }),

                new Historia("O Baú do Cenário", new List<string>
                {
                    "CENÓGRAFO: Alguém viu o baú antigo?",
                    "ASSISTENTE: Ele estava atrás do cenário principal.",
                    "CENÓGRAFO: Precisamos dele na próxima cena.",
                    "ATOR: Encontrei um baú perto da escada.",
                    "ASSISTENTE: Esse é o baú que procuramos.",
                    "CENÓGRAFO: A tampa está emperrada.",
                    "ATOR: Há uma fechadura na lateral.",
                    "ASSISTENTE: Talvez a chave esteja na mesa de adereços.",
                    "CENÓGRAFO: Vou procurá-la antes que as cortinas se abram."
                }),

                new Historia("A Última Apresentação", new List<string>
                {
                    "ATRIZ: Esta será nossa última apresentação juntos.",
                    "ATOR: Ainda não consigo acreditar que a companhia vai fechar.",
                    "DIRETOR: Não podemos desistir agora.",
                    "ATRIZ: Mas não temos mais patrocínio.",
                    "ATOR: Podemos realizar uma apresentação beneficente.",
                    "DIRETOR: E como conseguiremos pagar o teatro?",
                    "ATRIZ: Podemos vender os ingressos antecipadamente.",
                    "ATOR: Eu conheço alguém que pode divulgar o evento.",
                    "DIRETOR: Então ainda temos uma chance.",
                    "ATRIZ: Vamos fazer desta apresentação a melhor de todas."
                }),

                new Historia("O Bilhete do Diretor", new List<string>
                {
                    "ASSISTENTE: Diretor, encontrei um bilhete em sua mesa.",
                    "DIRETOR: Quem deixou isso aqui?",
                    "ASSISTENTE: Não havia ninguém quando cheguei.",
                    "DIRETOR: O que está escrito?",
                    "ASSISTENTE: Diz que o espetáculo não deve começar.",
                    "DIRETOR: Isso é absurdo. Temos uma plateia esperando.",
                    "ASSISTENTE: Há mais alguma coisa no papel.",
                    "DIRETOR: Leia em voz alta.",
                    "ASSISTENTE: 'O cenário ainda não está pronto.'",
                    "DIRETOR: Então descubra quem deveria ter terminado a montagem."
                }),

                new Historia("O Figurino Trocado", new List<string>
                {
                    "ATOR: Este não é o meu figurino.",
                    "FIGURINISTA: Como assim? Está com seu nome.",
                    "ATOR: Meu personagem usa uma capa vermelha.",
                    "FIGURINISTA: A capa vermelha foi entregue ao outro ator.",
                    "ATOR: Mas ele interpreta o mensageiro.",
                    "FIGURINISTA: Então alguém trocou as etiquetas.",
                    "ATOR: Precisamos corrigir isso antes da apresentação.",
                    "FIGURINISTA: Vou conferir a lista de figurinos.",
                    "ATOR: Enquanto isso, vou procurar minha capa."
                }),

                new Historia("A Plateia Misteriosa", new List<string>
                {
                    "PORTEIRO: Há alguém sentado na última fileira.",
                    "GERENTE: O teatro ainda está fechado.",
                    "PORTEIRO: Eu tentei conversar, mas a pessoa não respondeu.",
                    "GERENTE: Você verificou o ingresso?",
                    "PORTEIRO: Está segurando um ingresso antigo.",
                    "GERENTE: Qual é a data impressa nele?",
                    "PORTEIRO: É de uma apresentação de dez anos atrás.",
                    "GERENTE: Então precisamos descobrir como essa pessoa entrou.",
                    "PORTEIRO: Vou verificar todas as saídas.",
                    "GERENTE: E eu vou conferir os registros da bilheteria.",
                    "PORTEIRO: Espere... a cadeira está vazia."
                }),

                new Historia("O Texto Esquecido", new List<string>
                {
                    "ATOR: Esqueci completamente minha próxima fala.",
                    "ATRIZ: Você deveria ter estudado o roteiro.",
                    "ATOR: Eu estudei, mas a cena foi modificada.",
                    "DIRETOR: A nova versão está sobre a mesa.",
                    "ATOR: Onde exatamente?",
                    "DIRETOR: Ao lado do roteiro antigo.",
                    "ATRIZ: Este aqui possui as alterações marcadas.",
                    "ATOR: Agora encontrei minha fala.",
                    "DIRETOR: Ótimo. Vamos retomar a cena desde o início."
                }),

                new Historia("A Rosa de Papel", new List<string>
                {
                    "ATRIZ: Quem deixou esta rosa sobre minha cadeira?",
                    "ATOR: Talvez seja um presente de algum admirador.",
                    "ATRIZ: Há um pequeno papel preso ao caule.",
                    "ATOR: O que está escrito?",
                    "ATRIZ: 'Encontre-me depois do terceiro ato.'",
                    "ATOR: Isso parece uma mensagem secreta.",
                    "ATRIZ: Você acha que é uma brincadeira?",
                    "ATOR: Só existe uma maneira de descobrir.",
                    "ATRIZ: Então precisamos terminar a apresentação.",
                    "ATOR: E descobrir quem deixou a rosa."
                }),

                new Historia("A Cena Proibida", new List<string>
                {
                    "DIRETOR: Quem autorizou a retirada desta cena?",
                    "ASSISTENTE: Foi uma solicitação da produção.",
                    "DIRETOR: Mas essa cena é essencial para o final.",
                    "ATOR: Sem ela, meu personagem não tem motivação.",
                    "ASSISTENTE: A produção alegou que o texto era muito longo.",
                    "DIRETOR: Não podemos simplesmente eliminar uma parte da história.",
                    "ATOR: Podemos reduzir algumas falas.",
                    "ASSISTENTE: Assim manteremos o sentido original.",
                    "DIRETOR: Muito bem. Preparem uma nova versão."
                }),

                new Historia("O Relógio do Teatro", new List<string>
                {
                    "GERENTE: O relógio do saguão está atrasado.",
                    "PORTEIRO: Percebi isso quando os espectadores chegaram.",
                    "GERENTE: Que horas ele está marcando?",
                    "PORTEIRO: Sete horas, mas já são oito.",
                    "GERENTE: Precisamos ajustar o mecanismo.",
                    "PORTEIRO: A chave do relógio está no escritório.",
                    "GERENTE: Então vá buscá-la.",
                    "PORTEIRO: A porta do escritório está trancada.",
                    "GERENTE: Use a chave reserva da administração."
                }),

                new Historia("O Convite Real", new List<string>
                {
                    "MENSAGEIRO: Trago um convite para a companhia teatral.",
                    "DIRETOR: De quem é esse convite?",
                    "MENSAGEIRO: Do próprio governador.",
                    "ATOR: Ele pretende assistir à apresentação?",
                    "MENSAGEIRO: Não apenas assistir. Deseja conhecer os atores.",
                    "DIRETOR: Isso é uma grande honra.",
                    "ATRIZ: Precisamos preparar uma recepção.",
                    "DIRETOR: Primeiro devemos confirmar nossa presença.",
                    "MENSAGEIRO: A resposta precisa ser entregue ainda hoje.",
                    "ATOR: Então não podemos perder tempo."
                }),

                new Historia("O Espelho Quebrado", new List<string>
                {
                    "ATRIZ: Quem quebrou o espelho do camarim?",
                    "FIGURINISTA: Encontrei os pedaços no chão.",
                    "ATRIZ: Alguém estava aqui durante o intervalo?",
                    "FIGURINISTA: Vi o assistente entrando.",
                    "ASSISTENTE: Eu só vim buscar os figurinos.",
                    "ATRIZ: Você ouviu algum barulho?",
                    "ASSISTENTE: Ouvi algo cair quando estava saindo.",
                    "FIGURINISTA: Então talvez tenha sido um acidente.",
                    "ATRIZ: Precisamos limpar tudo antes da próxima cena."
                }),

                new Historia("O Enigma do Dramaturgo", new List<string>
                {
                    "DRAMATURGO: Terminei minha nova peça.",
                    "DIRETOR: E qual será o título?",
                    "DRAMATURGO: Ainda não decidi.",
                    "ATOR: Sobre o que é a história?",
                    "DRAMATURGO: Sobre um homem que perde tudo.",
                    "ATRIZ: E o que ele encontra no final?",
                    "DRAMATURGO: Algo que sempre esteve diante dele.",
                    "DIRETOR: Isso parece bastante misterioso.",
                    "DRAMATURGO: O público descobrirá quando assistir.",
                    "ATOR: Então mal posso esperar pela estreia."
                }),

                new Historia("A Máscara Dourada", new List<string>
                {
                    "ATOR: A máscara dourada desapareceu.",
                    "FIGURINISTA: Você verificou o armário de adereços?",
                    "ATOR: Sim, estava vazio.",
                    "FIGURINISTA: Ontem ela estava sobre a prateleira.",
                    "ATOR: Alguém utilizou o armário depois do ensaio?",
                    "FIGURINISTA: Apenas os atores da segunda cena.",
                    "ATOR: Precisamos perguntar a eles.",
                    "FIGURINISTA: Um deles deixou uma caixa no corredor.",
                    "ATOR: Talvez a máscara esteja dentro dela.",
                    "FIGURINISTA: Vamos verificar antes de acusar alguém."
                }),

                new Historia("A Cortina Final", new List<string>
                {
                    "DIRETOR: Por que a cortina não está abrindo?",
                    "OPERADOR: O mecanismo parou de funcionar.",
                    "DIRETOR: A apresentação já terminou.",
                    "OPERADOR: Eu sei, mas o público ainda está esperando.",
                    "ATOR: Podemos agradecer diante da cortina fechada.",
                    "DIRETOR: Não seria o encerramento adequado.",
                    "OPERADOR: Encontrei uma corda solta no mecanismo.",
                    "DIRETOR: Consegue consertá-la?",
                    "OPERADOR: Sim, só preciso de alguns minutos.",
                    "ATRIZ: Então vamos pedir ao público que aguarde."
                }),

                new Historia("O Testamento", new List<string>
                {
                    "ADVOGADO: Encontrei o testamento no escritório.",
                    "HERDEIRO: Meu pai realmente deixou alguma coisa?",
                    "ADVOGADO: Sim, mas há uma condição.",
                    "HERDEIRO: Qual é a condição?",
                    "ADVOGADO: Você precisa encontrar o último documento.",
                    "HERDEIRO: Onde ele está escondido?",
                    "ADVOGADO: Seu pai deixou uma indicação no verso.",
                    "HERDEIRO: Está escrito 'onde a história começa'.",
                    "ADVOGADO: Talvez ele esteja se referindo ao teatro.",
                    "HERDEIRO: Então precisamos procurar no palco."
                }),

                new Historia("O Grande Espetáculo", new List<string>
                {
                    "DIRETOR: Todos estão preparados para a estreia?",
                    "ATOR: Meu figurino está pronto.",
                    "ATRIZ: Os cenários também estão montados.",
                    "OPERADOR: A iluminação foi testada.",
                    "DIRETOR: E os ingressos foram conferidos?",
                    "GERENTE: Todos os lugares estão ocupados.",
                    "ATOR: Parece que teremos uma grande plateia.",
                    "ATRIZ: Espero que gostem da apresentação.",
                    "DIRETOR: Então chegou a hora.",
                    "OPERADOR: As luzes estão diminuindo.",
                    "DIRETOR: Abram as cortinas. O espetáculo vai começar!"
                })
            };
        }

        private void SortearHistoria()
        {
            if (historiasDisponiveis.Count == 0)
            {
                historiasDisponiveis = Enumerable
                    .Range(0, historias.Count)
                    .OrderBy(x => random.Next())
                    .ToList();
            }

            int indice = historiasDisponiveis[0];
            historiasDisponiveis.RemoveAt(0);

            historiaAtual = historias[indice];

            lblNomePeca.Text = historiaAtual.Nome;

            List<string> falasEmbaralhadas;

            do
            {
                falasEmbaralhadas = historiaAtual.Falas
                    .OrderBy(x => random.Next())
                    .ToList();

            } while (falasEmbaralhadas.SequenceEqual(historiaAtual.Falas));

            vitoria = false;

            CriarCartoes(falasEmbaralhadas);
        }

        private void CriarCartoes(List<string> falas)
        {
            flpFalas.Controls.Clear();
            flpResposta.Controls.Clear();

            cartoes.Clear();

            foreach (string fala in falas)
            {
                Panel cartao = new Panel();

                cartao.Height = 56;
                cartao.Width = 400;
                cartao.BackColor = Color.FromArgb(38, 38, 43);
                cartao.BorderStyle = BorderStyle.FixedSingle;
                cartao.Cursor = Cursors.Hand;
                cartao.Margin = new Padding(4);
                cartao.AllowDrop = true;

                Label texto = new Label();

                texto.Text = fala;
                texto.Dock = DockStyle.Fill;
                texto.TextAlign = ContentAlignment.MiddleLeft;
                texto.Padding = new Padding(12, 5, 12, 5);
                texto.ForeColor = Color.White;
                texto.BackColor = Color.Transparent;
                texto.Font = new Font("Segoe UI", 9.5f, FontStyle.Regular);
                texto.Cursor = Cursors.Hand;
                texto.AllowDrop = true;

                cartao.Controls.Add(texto);

                cartao.MouseDown += (s, e) => IniciarArraste(cartao, e);
                texto.MouseDown += (s, e) => IniciarArraste(cartao, e);

                cartao.MouseEnter += (s, e) => AplicarHover(cartao, true);
                texto.MouseEnter += (s, e) => AplicarHover(cartao, true);

                cartao.MouseLeave += (s, e) => VerificarSaidaHover(cartao);
                texto.MouseLeave += (s, e) => VerificarSaidaHover(cartao);

                cartao.DragEnter += DragEnterFalas;
                cartao.DragDrop += DragDropFalas;

                texto.DragEnter += DragEnterFalas;
                texto.DragDrop += DragDropFalas;

                cartoes.Add(cartao);
                flpFalas.Controls.Add(cartao);
            }

            btnConfirmar.Enabled = false;
        }

        private void ReiniciarResposta(object sender, EventArgs e)
        {
            if (vitoria)
                vitoria = false;

            flpFalas.Controls.Clear();
            flpResposta.Controls.Clear();

            List<Panel> falasEmbaralhadas = cartoes
                .OrderBy(x => random.Next())
                .ToList();

            foreach (Panel cartao in falasEmbaralhadas)
            {
                AplicarHover(cartao, false);
                cartao.Cursor = Cursors.Hand;

                foreach (Control controle in cartao.Controls)
                    controle.Cursor = Cursors.Hand;

                flpFalas.Controls.Add(cartao);
            }

            btnConfirmar.Enabled = false;
        }

        private void IniciarArraste(Panel cartao, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left || vitoria)
                return;

            cartao.DoDragDrop(cartao, DragDropEffects.Move);
        }

        private void AplicarHover(Panel cartao, bool ativo)
        {
            if (vitoria)
                return;

            if (ativo)
            {
                cartao.BackColor = Color.FromArgb(65, 65, 73);
                cartao.Cursor = Cursors.Hand;
            }
            else
            {
                cartao.BackColor = Color.FromArgb(38, 38, 43);
            }
        }

        private void VerificarSaidaHover(Panel cartao)
        {
            Point posicao = cartao.PointToClient(Cursor.Position);

            if (!cartao.ClientRectangle.Contains(posicao))
                AplicarHover(cartao, false);
        }

        private void DragEnterFalas(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(Panel)) && !vitoria)
                e.Effect = DragDropEffects.Move;
            else
                e.Effect = DragDropEffects.None;
        }

        private void DragDropFalas(object sender, DragEventArgs e)
        {
            if (vitoria)
                return;

            Panel cartao = e.Data.GetData(typeof(Panel)) as Panel;

            Control controleDestino = sender as Control;

            while (controleDestino != null &&
                   !(controleDestino is FlowLayoutPanel))
            {
                controleDestino = controleDestino.Parent;
            }

            FlowLayoutPanel destino = controleDestino as FlowLayoutPanel;

            if (cartao == null || destino == null)
                return;

            Point posicao = destino.PointToClient(new Point(e.X, e.Y));

            int indice = 0;

            foreach (Control controle in destino.Controls)
            {
                if (controle == cartao)
                    continue;

                if (posicao.Y < controle.Top + controle.Height / 2)
                    break;

                indice++;
            }

            cartao.Parent.Controls.Remove(cartao);

            destino.Controls.Add(cartao);

            indice = Math.Max(0, Math.Min(indice, destino.Controls.Count - 1));

            destino.Controls.SetChildIndex(cartao, indice);

            AplicarHover(cartao, false);

            btnConfirmar.Enabled =
                flpResposta.Controls.Count == historiaAtual.Falas.Count;
        }

        private void ConfirmarResposta(object sender, EventArgs e)
        {
            if (flpResposta.Controls.Count != historiaAtual.Falas.Count)
            {
                MessageBox.Show(
                    "Organize todas as falas antes de confirmar.",
                    "Roteiro incompleto",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                return;
            }

            List<string> resposta = flpResposta.Controls
                .Cast<Panel>()
                .Select(x => x.Controls[0].Text)
                .ToList();

            if (resposta.SequenceEqual(historiaAtual.Falas))
            {
                vitoria = true;

                foreach (Panel cartao in cartoes)
                {
                    cartao.Cursor = Cursors.Default;

                    foreach (Control controle in cartao.Controls)
                        controle.Cursor = Cursors.Default;
                }

                MessageBox.Show(
                    "Excelente! Você organizou o roteiro corretamente.\n\nOs funcionários ficaram convencidos de que vocês são atores.",
                    "Roteiro concluído",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
            else
            {
                MessageBox.Show(
                    "A sequência está incorreta. Revise as falas e tente novamente.",
                    "Roteiro incorreto",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
            }
        }
    }
}
