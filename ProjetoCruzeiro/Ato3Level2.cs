using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
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

        private readonly List<Historia> historias = new List<Historia>();
        private readonly List<int> historiasDisponiveis = new List<int>();
        private readonly List<Panel> cartoes = new List<Panel>();
        private readonly List<EfeitoBotao> efeitos = new List<EfeitoBotao>();

        private readonly Random random = new Random();
        private readonly Stopwatch cronometro = new Stopwatch();
        private readonly Timer timerFala = new Timer();

        private Historia historiaAtual;

        private bool vitoria;
        private bool vitoriaAposFala;
        private int tentativas;

        private const int IDENTIFICADOR_NIVEL = 8;

        public Ato3Level2()
        {
            InitializeComponent();

            AtivarDoubleBuffering(this);

            ConfigurarInterface();
            CarregarHistorias();
            SortearHistoria();

            cronometro.Start();
        }

        private void AtivarDoubleBuffering(Control controle)
        {
            if (controle == null)
                return;

            PropertyInfo propriedade = typeof(Control).GetProperty(
                "DoubleBuffered",
                BindingFlags.Instance | BindingFlags.NonPublic
            );

            if (propriedade != null)
                propriedade.SetValue(controle, true, null);

            foreach (Control filho in controle.Controls)
                AtivarDoubleBuffering(filho);
        }

        private string CaminhoImagem(string arquivo)
        {
            return Path.Combine(
                Application.StartupPath,
                "imagens",
                "Level8",
                arquivo
            );
        }

        private Image CarregarImagem(string arquivo)
        {
            string caminho = CaminhoImagem(arquivo);

            if (!File.Exists(caminho))
                return null;

            using (Image imagem = Image.FromFile(caminho))
            {
                return new Bitmap(imagem);
            }
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

            pbx_fala.BackgroundImage = null;
            pbx_fala.BackgroundImageLayout = ImageLayout.Stretch;

            pbx_confirmar.Cursor = Cursors.Hand;
            pbx_reiniciar.Cursor = Cursors.Hand;

            efeitos.Add(new EfeitoBotao(pbx_reiniciar));
            efeitos.Add(new EfeitoBotao(pbx_confirmar));

            pbx_confirmar.Click -= ConfirmarResposta;
            pbx_confirmar.Click += ConfirmarResposta;

            pbx_reiniciar.Click -= ReiniciarResposta;
            pbx_reiniciar.Click += ReiniciarResposta;

            timerFala.Interval = 3000;
            timerFala.Tick += TimerFala_Tick;
        }

        private void CarregarHistorias()
        {
            historias.Clear();

            historias.AddRange(new Historia[]
            {
                new Historia("'Romance de Sessão da Tarde'", new List<string>
                {
                    "Garota: 'Que desastre! Sendo a garota mais desajeitada e invisível deste colégio, derrubei meus livros bem nos pés do capitão do time!'",
                    "Garoto: 'Deixe-me recolher os seus cadernos... Nossa! Agora que nossos olhos se cruzaram, notei que você é linda só porque tirou os óculos!'",
                    "Garota: 'Isso é um absurdo biológico, minha miopia continua exatamente a mesma e eu mal consigo enxergar o seu rosto sem eles.'",
                    "*O sinal da escola toca alto e o cenário muda instantaneamente para a Pista de Dança do Baile de Formatura*",
                    "Garota: 'Espere um segundo... Como passamos do corredor da escola diretamente para a noite do grande baile de gala final?'",
                    "Garoto: 'O poder da montagem musical com música pop dos anos 2000 acelerou os três meses de desenvolvimento do nosso namoro!'",
                    "Garota: 'Mas nós nem conversamos sobre o nosso primeiro encontro ou sobre o fato de você namorar a patricinha mais popular da escola!'",
                    "*O holofote principal foca exclusivamente nos dois enquanto a multidão de alunos abre espaço aplaudindo*",
                    "Garoto: 'Isso não importa mais, veja como meu terno combina perfeitamente com o seu vestido que era da sua avó'",
                    "Garota: 'Eu nem queria estar aqui, eu deveria estar estudando para o teste de admissão da faculdade de medicina!'",
                    "Garoto: 'Esqueça os livros! Dance comigo sob esta chuva artificial de confetes prateados enquanto o DJ aumenta o som!'",
                    "*A música para abruptamente quando o cenário se transforma no Portão de Embarque de um Aeroporto Internacional*",
                    "Garoto: 'Não vá embora para Paris! Eu percebi que meu amor por você é maior do que minha carreira no futebol americano universitário!'",
                    "Garota: 'Eu estou apenas indo visitar minha tia por duas semanas, por que você trouxe toda a torcida organizada para o portão?'",
                    "*O capitão do time se ajoelha no saguão, ergue um buquê de rosas e o público do aeroporto começa a aplaudir em coro*"
                }),

                new Historia("'O Policial à Beira da Aposentadoria'", new List<string>
                {
                    "Capitão: 'Detetive, preciso que lidere a invasão ao quartel-general da máfia ao lado deste recém-formado totalmente rebelde e imprevisível.'",
                    "Detetive: 'Nem pense nisso, Capitão! Minha farda de capitão honorário já está limpa e meu iate de pesca de águas profundas sai amanhã bem cedo.'",
                    "Capitão: 'É apenas uma averiguação de rotina simples em um galpão industrial cercado por atiradores de elite fortemente armados.'",
                    "Detetive: 'Que cenário perfeito para o meu último dia. Vou deixar meu colete balístico na gaveta para o destino cumprir seu papel trágico.'",
                    "*A porta do galpão é arrombada e a cena muda para o interior de um Cativeiro Subterrâneo Escuro*",
                    "Detetive: 'Parado aí, novato! Não avance sem verificar as armadilhas de fio de aço esticadas rente ao chão de concreto úmido!'",
                    "Novato: 'Relaxa, vovô! A velha escola de investigação está ultrapassada, eu resolvo isso na base do instinto e da pura audácia!'",
                    "*O parceiro novato chuta uma caixa de madeira disparando um alarme ensurdecedor conectado a uma carga de dinamite*",
                    "Detetive: 'Eu sabia! Faltam exatamente doze segundos para esta fábrica clandestina ir pelos ares e eu ser eternizado como herói morto!'",
                    "Novato: 'Segure na minha jaqueta de couro! Vamos saltar através daquela janela de vidro duplo antes que a faísca atinja o estopim!'",
                    "Detetive: 'Não dá tempo! Vá você e diga para a minha esposa que o dinheiro do seguro de vida está na conta poupança conjunta!'",
                    "*A explosão ocorre e o impacto arremessa os dois diretamente para o Meio de um Deserto de Areia Escaldante*",
                    "Novato: 'Como a força da detonação nos jogou em um bioma desértico completamente isolado da civilização urbana?'",
                    "Detetive: 'Não tente entender a geografia dos filmes policiais de ação, apenas me ajude a caminhar com este ferimento de raspão no ombro.'",
                    "Novato: 'Olha lá no horizonte... o Capitão está vindo nos resgatar pilotando um helicóptero militar de ataque sem nenhuma explicação.'"
                }),

                new Historia("'Teatro de Vanguarda'", new List<string>
                {
                    "Assistente: 'Por que aquele ator está de terno, completamente nu da cintura para baixo, recitando poesia expressionista para uma beterraba?'",
                    "Diretor: 'Silêncio! Essa encenação abstrata representa o colapso do homem burguês diante da inflação e da escassez de recursos agrícolas!'",
                    "Assistente: 'Tenho quase certeza de que ele esqueceu o texto original, entrou em surto psicótico e está improvisando com o cenário do almoço.'",
                    "Diretor: 'Seja como for, esse choro real de desespero existencial trouxe uma camada genial de metalinguagem psicológica para a nossa peça.'",
                    "*O holofote vermelho se apaga e os atores surgem sentados no meio de uma Fábrica Abandonada Com Névoa de Gelo Seco*",
                    "Diretor: 'Mudança de ato! Agora os intérpretes estão fingindo ser engrenagens industriais obsoletas operadas por um mímico melancólico.'",
                    "Assistente: 'Aquele mímico acabou de derrubar a estrutura de andaimes de ferro bem em cima da mesa de som do teatro, não foi?'",
                    "Diretor: 'Magnífico! O estrondo do metal retorcido simboliza a destruição das barreiras sonoras impostas pela opressão corporativa!'",
                    "*Os espectadores da primeira fileira começam a tossir devido ao excesso de fumaça química lançada no recinto*",
                    "Assistente: 'Essa fumaça está sufocante, acho que o efeito especial falhou e o palco está genuinamente pegando fogo nos bastidores!'",
                    "Diretor: 'Não seja ignorante, o calor das chamas reais faz o público experimentar a verdadeira agonia do aquecimento global na pele!'",
                    "*O teto do teatro cede e o cenário se transforma instantaneamente em uma Galeria de Arte Contemporânea Minimalista*",
                    "Assistente: 'Fomos evacuados para a galeria vizinha ou isso ainda faz parte da experiência imersiva e disruptiva do diretor?'",
                    "Diretor: 'Veja aquela instalação de cinzas humanas no chão... é o ápice da performance minimalista sobre a impermanência do ser!'",
                    "*Os críticos de arte aplaudem de pé enquanto os bombeiros tentam apagar o incêndio no prédio ao lado com sirenes altas*"
                }),

                new Historia("'Hacker de Cinema'", new List<string>
                {
                    "'Eles detectaram o nosso IP e acionaram o firewall! Consegue quebrar a criptografia quântica do servidor central do Pentágono?'",
                    "*digita freneticamente no teclado mecânico enquanto telas com códigos em verde neon piscam rapidamente* 'Consegui, invadi o sistema!'",
                    "'Incrível! Como você conseguiu burlar a barreira de segurança militar mais complexa e impenetrável do planeta em cinco segundos?'",
                    "'A senha padrão do administrador ainda estava definida como 'admin123'.'",
                    "*As telas de computador apagam e a dupla ressurge trancada no Interior de um Servidor de Dados Blindado*",
                    "'Alerta vermelho! O sistema operacional físico iniciou o protocolo de purga térmica e a sala está virando um forno!'",
                    "'Rápido! Conecte seu dispositivo USB diretamente no barramento central da placa-mãe para injetar o vírus de resfriamento!'",
                    "'As portas de conexão física são todas de um formato proprietário que não aceita o meu conector universal padrão!'",
                    "*O hacker puxa um clipe de papel do bolso e começa a violar os circuitos integrados gerando faíscas elétricas*",
                    "'Você vai fritar os nossos cérebros com essa descarga estática ou vai conseguir burlar o hardware usando um clipe?'",
                    "'A engenharia reversa de cinema permite que qualquer objeto metálico reescreva o código de firmware se tocado no lugar certo!'",
                    "*Os coolers gigantes param de girar e a parede divisória se abre para uma Central de Comando Espacial da NASA*",
                    "'Conseguimos o acesso total! Veja, o mapa orbital mostra que o satélite militar já está sob o nosso controle absoluto!'",
                    "'Rápido, digite o comando para desorbitar o satélite antes que o agente do FBI arrombe a porta de aço da central!'",
                    "*Hacker aperta a tecla Enter com força dramática, um bipe longo ecoa e a animação de 'Acesso Concedido' surge em tela cheia*"
                }),

                new Historia("'O Monólogo'", new List<string>
                {
                    "Diretor: 'Corta! Você acabou de levar uma facada fatal no coração! Precisa desabar no palco e simular o rigor tremendo da morte agora!'",
                    "Ator: 'Mas o roteirista me garantiu três páginas de solilóquio existencialista shakespeariano sobre a finitude humana antes do meu último suspiro!'",
                    "Diretor: 'Se o público aguentar mais um verso decassílabo dessa sua agonia poética, teremos um necrotério real de puro tédio na plateia.'",
                    "Ator: 'Compreendo o clamor da direção... encerro aqui meu ato dramático com este espasmo final!' *morre estatelado no chão*",
                    "*Cenário muda bruscamente para o Tribunal do Purgatório Teatral*",
                    "Ator: 'Espera... onde estou? Por que há um comitê de dramaturgos mortos me julgando vestindo togas?'",
                    "Dramaturgo: 'Você violou a lei máxima do ritmo cênico ao tentar discursar com a aorta rompida, alma penada!'",
                    "Ator: 'Eu apenas clamei pelo meu direito artístico de morrer dramaticamente sob a luz do holofote principal!'",
                    "Dramaturgo: 'Seu egocentrismo quase faliu a companhia de teatro! Sua sentença é passar a eternidade como figurante mudo.'",
                    "*Ator tenta gritar em protesto, mas sua voz desaparece completamente*",
                    "Dramaturgo: 'Vejam só, o grande orador finalmente aprendeu a fazer uma pausa dramática de verdade no roteiro.'",
                    "*O juiz bate o martelo e o cenário se transforma na coxia de um teatro de ópera lotado*",
                    "Diretor: 'Rápido! Vista esta fantasia de árvore de fundo e entre no palco de forma absolutamente silenciosa!'",
                    "Ator: 'Mmmph! Mmmph!' *aponta desesperadamente para a própria garganta demonstrando pânico*",
                    "Diretor: 'Excelente expressividade corporal! Vá lá para trás e não ouse emitir um único ruído até a cortina cair.'"
                }),

                new Historia("'O Vilão Explicativo'", new List<string>
                {
                    "Herói: 'Chega de blefar, Doutor Caos! O seu plano de aniquilação planetária foi completamente desmantelado pelo meu esquadrão!'",
                    "Doutor Caos: 'Ria enquanto pode, Herói! Você subestimou meu intelecto e não conseguirá deter o disparo do meu Canhão de Prótons Antimatéria!'",
                    "Herói: 'Na verdade, desativei os fusíveis desse canhão há vinte minutos, enquanto você fazia um PowerPoint detalhando seu plano maligno.'",
                    "Doutor Caos: 'Maldito seja! Como eu saberia que você invadiria a fortaleza antes de eu concluir a exposição didática da minha vingança?'",
                    "*Doutor Caos aperta um botão de emergência e ambos são teleportados para o topo de um Vulcão em Erupção*",
                    "Doutor Caos: 'Achou que seria tão fácil? Agora estamos isolados no epicentro do magma, onde ninguém interromperá meus slides explicativos!'",
                    "Herói: 'Você realmente nos trouxe para a beira da morte térmica só para terminar de ler o manual do seu raio destrutivo?'",
                    "Doutor Caos: 'A clareza narrativa da minha vilania exige que você compreenda perfeitamente a matemática complexa por trás do meu triunfo!'",
                    "*Herói boceja ostensivamente e finge verificar as unhas enquanto o vilão projeta hologramas na fumaça*",
                    "Doutor Caos: 'Veja este gráfico de pizza! Ele ilustra perfeitamente como dividirei os continentes após a poeira cósmica baixar!'",
                    "Herói: 'Seu gráfico está desatualizado, os capangas que controlavam a pressão do magma acabaram de fugir usando o helicóptero de resgate.'",
                    "*O chão racha ao meio e uma plataforma flutuante de metal os transporta para o interior de uma cela de Prisão de Segurança Máxima*",
                    "Doutor Caos: 'Como viemos parar aqui? Meu monólogo sobre a dominação global ainda tinha pelo menos quarenta minutos de duração!'",
                    "Herói: 'O tempo de tela acabou e fomos direto para a cena pós-créditos. O guarda da prisão está esperando você assinar a confissão.'",
                    "Doutor Caos: 'Eu me recuso a assinar um documento tão sintetizado! Exijo três páginas de adendos para explicar minhas motivações de infância!'"
                }),

                new Historia("'Clichê de Terror'", new List<string>
                {
                    "Garota 1: 'Amiga, você ouviu esse rangido horripilante de motosserra vindo do porão escuro desta cabana isolada e sem sinal de celular?'",
                    "Garota 2: 'Ouvi perfeitamente! A estratégia ideal agora é nos separarmos no breu total e investigar o subsolo sem nenhuma lanterna!'",
                    "Garota 1: 'Excelente ideia! Vou aproveitar o pânico para tropeçar na escada do porão, machucar o tornozelo e perder as chaves do carro.'",
                    "Garota 2: 'Ótimo! De qualquer forma, se o maníaco mascarado surgir, o motor do nosso veículo certamente iria falhar, não precisamos delas.'",
                    "*A porta do porão se bate sozinha e o chão desaba, jogando as duas diretamente em um Cemitério de Automóveis Abandonados*",
                    "Garota 1: 'Nossa, que transição bizarra. Como viemos parar neste labirinto de carcaças de metal no meio da névoa densa?'",
                    "Garota 2: 'Não importa o sentido geográfico! Veja, há uma cabine telefônica antiga ali adiante que claramente exala uma aura de armadilha mortal!'",
                    "Garota 1: 'Vamos correndo até lá para tentar ligar para a polícia, ignorando o fato óbvio de que os fios telefônicos estão visivelmente cortados!'",
                    "*Um vulto alto com uma máscara de porco surge detrás de um guindaste enferrujado segurando um machado banhado a sangue*",
                    "Garota 1: 'Olha lá! Um homem hostil com trajes medonhos! Vamos gritar histericamente em vez de correr na direção oposta!'",
                    "Garota 2: 'Socorro! Minhas pernas esqueceram instantaneamente como correr em linha reta e eu vou cair de maduro neste chão plano!'",
                    "*O assassino caminha em câmera lenta, mas misteriosamente aparece colado às duas quando o cenário muda para o Sótão da Cabana*",
                    "Garota 1: 'Como voltamos para a cabana? Estamos encurraladas contra a janela de vidro do sótão sem nenhuma rota de fuga plausível!'",
                    "Garota 2: 'Rápido, feche os olhos e reze para o roteirista decidir que isso tudo foi apenas um pesadelo clichê de introdução de filme!'",
                    "*O assassino ergue o machado, a tela corta para o preto e um estrondo agudo de violino ecoa no silêncio*"
                }),

                new Historia("'Filme de Ação Realista'", new List<string>
                {
                    "Ator: 'Rápido! Salte do teto deste arranha-céu de cinquenta andares e caia exatamente na caçamba desse caminhão de lixo em movimento!'",
                    "Protagonista: 'Você perdeu o juízo? A física básica diz que meu esqueleto será completamente esmagado pelo impacto dessa queda livre!'",
                    "Ator: 'Não se preocupe, a página quarenta do roteiro diz explicitamente que você é o protagonista e sairá andando sem nenhum arranhão.'",
                    "Protagonista: 'Pois avise ao autor que a gravidade ignora o estrelato e o plano de saúde não cobre traumatismo por conveniência de roteiro!'",
                    "*O protagonista corre, salta e a cena corta para o teto de um Trem-Bala cruzando uma ponte metálica em alta velocidade*",
                    "Ator: 'Viu só? O corte de câmera salvou suas pernas! Agora lute contra o mercenário russo enquanto o vento de trezentos por hora nos atinge!'",
                    "Protagonista: 'Eu mal consigo abrir os olhos por causa da pressão aerodinâmica! Como você espera que eu desfie socos coreografados aqui em cima?'",
                    "Ator: 'Apenas siga a marcação do dublê! Quando ele chutar, você finge que voa três metros para trás e segura na borda do vagão!'",
                    "*Protagonista escorrega na lataria molhada e fica pendurado por apenas um dedo enquanto o trem entra em um túnel estreito*",
                    "Ator: 'Cuidado com a estrutura de concreto vindo a toda velocidade! Agache a cabeça ou a decapitação será real e fora do script!'",
                    "Protagonista: 'Eu não assinei contrato para morrer esmagado por um pilar de sustentação ferroviária! Cadê os cabos de segurança de computação gráfica?'",
                    "*O trem atravessa o túnel e freia bruscamente, arremessando-os diretamente para o meio de uma Perseguição de Carros na Avenida*",
                    "Ator: 'Rápido, caia no banco do carona deste conversível em movimento! O pneu esquerdo acabou de estourar após levar um tiro de bazuca!'",
                    "Protagonista: 'Chega de insanidade automotiva! Eu vou puxar o freio de mão e esperar a polícia chegar para registrar este boletim de ocorrência!'",
                    "*O carro capota três vezes no ar, explode em câmera lenta e o protagonista sai dos escombros apenas limpando a poeira do paletó*"
                }),

                new Historia("'O Paradoxo da Conveniência'", new List<string>
                {
                    "Professor: 'Rápido! Ative os circuits do capacitor cronológico e viaje para 1930 para impedir que o ditador construa a bomba!'",
                    "Jovem: 'Espere! Se temos uma máquina que rasga o tecido do espaço-tempo, por que não voltamos para ontem à tarde antes dele almoçar?'",
                    "Professor: 'Porque resolver a linha temporal de forma simples destruiria o conflito dramático de três atos necessário para vender ingressos!'",
                    "Jovem: 'Entendi a lógica comercial... Só espero que o exército do passado espere eu calibrar o GPS quântico antes de abrir fogo.'",
                    "*A alavanca temporal é puxada e a cabine ressurge no meio de uma Trincheira de Guerra Revestida de Arame Farpado*",
                    "Professor: 'Cuidado! Um projétil de artilharia pesada acabou de explodir a dois metros da nossa cabine temporal recém-estacionada!'",
                    "Jovem: 'Por que raios o GPS quântico nos jogou exatamente no meio do bombardeio em vez de nos deixar em um beco seguro e calmo?'",
                    "Professor: 'Porque o clímax da narrativa exige perigo iminente para justificar os efeitos visuais que consumiram metade do orçamento!'",
                    "*Soldados com fardas históricas avançam com baionetas em punho gritando ordens em um idioma incompreensível*",
                    "Professor: 'Eles estão avançando! Rápido, use o dispositivo de dobra espacial para nos tirar deste front de batalha caótico!'",
                    "Jovem: 'O dispositivo entrou em modo de atualização de firmware obrigatório e vai demorar dez minutos para reiniciar o sistema!'",
                    "*Um tanque de guerra avança esmagando a cabine e os transporta instantaneamente para o Salão Oval da Casa Branca em 2026*",
                    "Jovem: 'Conseguimos voltar? O paradoxo temporal foi resolvido ou nós quebramos a continuidade da nossa própria realidade de vez?'",
                    "Professor: 'Olhe pela janela... O monumento de Washington foi substituído por uma estátua gigante daquele ditador de mil novecentos e trinta.'",
                    "*Ambos se entreolham em silêncio enquanto a trilha sonora de suspense dramático atinge o volume máximo*"
                })
            });
        }

        private void SortearHistoria()
        {
            if (historiasDisponiveis.Count == 0)
            {
                historiasDisponiveis.AddRange(
                    Enumerable.Range(0, historias.Count)
                        .OrderBy(x => random.Next())
                );
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
            }
            while (falasEmbaralhadas.SequenceEqual(historiaAtual.Falas));

            vitoria = false;
            vitoriaAposFala = false;
            tentativas = 0;

            pbx_fala.BackgroundImage = null;

            CriarCartoes(falasEmbaralhadas);
        }

        private void CriarCartoes(List<string> falas)
        {
            flpFalas.Controls.Clear();
            flpResposta.Controls.Clear();
            cartoes.Clear();

            foreach (string fala in falas)
            {
                Panel cartao = new Panel
                {
                    Height = 56,
                    Width = 400,
                    BackColor = Color.FromArgb(60, 38, 38, 43),
                    BorderStyle = BorderStyle.FixedSingle,
                    Cursor = Cursors.Hand,
                    Margin = new Padding(4),
                    AllowDrop = true
                };

                Label texto = new Label
                {
                    Text = fala,
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleLeft,
                    Padding = new Padding(12, 5, 12, 5),
                    ForeColor = Color.Black,
                    BackColor = Color.Transparent,
                    Font = new Font("Times New Roman", 8.5f, FontStyle.Regular),
                    Cursor = Cursors.Hand,
                    AllowDrop = true
                };

                cartao.Controls.Add(texto);

                // Ativa DoubleBuffering também nos cartões e seus Labels.
                AtivarDoubleBuffering(cartao);

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

            pbx_confirmar.Enabled = false;
        }

        private void ReiniciarResposta(object sender, EventArgs e)
        {
            if (vitoria)
                return;

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

            pbx_confirmar.Enabled = false;
            pbx_fala.BackgroundImage = null;

            timerFala.Stop();
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
                cartao.BackColor = Color.FromArgb(60, 65, 65, 73);
                cartao.Cursor = Cursors.Hand;
            }
            else
            {
                cartao.BackColor = Color.FromArgb(60, 38, 38, 43);
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

            pbx_confirmar.Enabled =
                flpResposta.Controls.Count == historiaAtual.Falas.Count;
        }

        private void ConfirmarResposta(object sender, EventArgs e)
        {
            if (vitoria)
                return;

            // Roteiro incompleto: apenas exibe fala1.png.
            // Nenhum botão ou cartão é bloqueado.
            if (flpResposta.Controls.Count != historiaAtual.Falas.Count)
            {
                pbx_fala.BackgroundImage = CarregarImagem("fala1.png");

                vitoriaAposFala = false;

                timerFala.Stop();
                timerFala.Start();

                return;
            }

            tentativas++;

            List<string> resposta = flpResposta.Controls
                .Cast<Panel>()
                .Select(x => x.Controls[0].Text)
                .ToList();

            if (resposta.SequenceEqual(historiaAtual.Falas))
            {
                // Resposta correta: exibe fala2.png e bloqueia tudo.
                vitoria = true;

                cronometro.Stop();

                foreach (Panel cartao in cartoes)
                {
                    cartao.Cursor = Cursors.Default;

                    foreach (Control controle in cartao.Controls)
                        controle.Cursor = Cursors.Default;
                }

                pbx_fala.BackgroundImage = CarregarImagem("fala2.png");

                BloquearControlesDuranteVitoria();

                vitoriaAposFala = true;
            }
            else
            {
                // Resposta errada: apenas exibe fala1.png.
                // As interações continuam liberadas.
                pbx_fala.BackgroundImage = CarregarImagem("fala1.png");

                vitoriaAposFala = false;
            }

            timerFala.Stop();
            timerFala.Start();
        }

        private void TimerFala_Tick(object sender, EventArgs e)
        {
            timerFala.Stop();
            pbx_fala.BackgroundImage = null;

            if (vitoriaAposFala)
            {
                int pontuacaoFinal = CalcularPontuacao();

                JogoForm jogo = EncontrarJogoForm(FindForm());

                if (jogo != null)
                {
                    jogo.MostrarVitoria(IDENTIFICADOR_NIVEL, pontuacaoFinal);
                }
                else
                {
                    Cruzeiro tela = FindForm() as Cruzeiro;

                    if (tela != null)
                        tela.AbrirTela(new VitoriaControl(IDENTIFICADOR_NIVEL, pontuacaoFinal));
                }

                return;
            }

            // fala1.png terminou.
            // Nenhum desbloqueio é necessário, pois nada foi bloqueado.
        }

        private int CalcularPontuacao()
        {
            int bonusConclusao = 500;

            int bonusTentativa = Math.Max(
                0,
                2000 - ((tentativas - 1) * 200)
            );

            int segundosDecorridos = (int)Math.Floor(cronometro.Elapsed.TotalSeconds);
            int segundosExcedidos = Math.Max(0, segundosDecorridos - 120);

            int bonusTempo = Math.Max(
                0,
                2500 - (segundosExcedidos * 30)
            );

            return bonusConclusao + bonusTentativa + bonusTempo;
        }

        private void BloquearControlesDuranteVitoria()
        {
            pbx_reiniciar.Enabled = false;
            pbx_confirmar.Enabled = false;

            pbx_reiniciar.Cursor = Cursors.No;
            pbx_confirmar.Cursor = Cursors.No;

            MenuLateralControl menu = EncontrarMenuLateral(FindForm());

            if (menu != null)
                menu.BloquearVoltarMenu();
        }

        private JogoForm EncontrarJogoForm(Control controle)
        {
            if (controle == null)
                return null;

            JogoForm jogo = controle as JogoForm;

            if (jogo != null)
                return jogo;

            foreach (Control filho in controle.Controls)
            {
                JogoForm encontrado = EncontrarJogoForm(filho);

                if (encontrado != null)
                    return encontrado;
            }

            return null;
        }

        private MenuLateralControl EncontrarMenuLateral(Control controle)
        {
            if (controle == null)
                return null;

            MenuLateralControl menu = controle as MenuLateralControl;

            if (menu != null)
                return menu;

            foreach (Control filho in controle.Controls)
            {
                MenuLateralControl encontrado = EncontrarMenuLateral(filho);

                if (encontrado != null)
                    return encontrado;
            }

            return null;
        }
    }
}