using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using chat.Models;

namespace chat.Services
{
    public class AssistantService
    {
        private readonly KnowledgeService knowledgeService;
        private readonly MemoryService memoryService;
        private readonly OpenAiResponsesService openAiService;
        private readonly ConfigService configService;
        private readonly Random random = new Random();

        public AssistantService(KnowledgeService knowledgeService, MemoryService memoryService, OpenAiResponsesService openAiService, ConfigService configService)
        {
            this.knowledgeService = knowledgeService;
            this.memoryService = memoryService;
            this.openAiService = openAiService;
            this.configService = configService;
        }

        public AssistantMemory LoadMemory()
        {
            return memoryService.LoadMemory();
        }

        public void SaveMemory(AssistantMemory memory)
        {
            memoryService.SaveMemory(memory);
        }

        public bool IsOnlineConfigured()
        {
            return openAiService.IsConfigured();
        }

        public List<KnowledgeItem> LoadKnowledge()
        {
            return knowledgeService.LoadKnowledge();
        }

        public void SaveKnowledge(List<KnowledgeItem> items)
        {
            knowledgeService.SaveKnowledge(items);
        }

        public AppSettings LoadSettings()
        {
            return configService.Load();
        }

        public AssistantProfile LoadProfile()
        {
            return knowledgeService.LoadProfile();
        }

        public void SaveSettings(AppSettings settings)
        {
            configService.Save(settings);
        }

        public async Task<AssistantResponse> GetResponseAsync(string userMessage, string mode, AssistantMemory memory)
        {
            UpdateMemory(memory, userMessage, mode);
            memoryService.AppendHistory("user", userMessage);

            AssistantResponse response;
            string failureReason = string.Empty;

            if (openAiService.IsConfigured())
            {
                try
                {
                    string onlineText = await openAiService.GenerateAsync(
                        BuildInstructions(mode, memory),
                        BuildInput(userMessage, mode, memory));

                    if (!string.IsNullOrWhiteSpace(onlineText))
                    {
                        response = new AssistantResponse
                        {
                            Text = onlineText.Trim(),
                            Source = "OpenAI - " + openAiService.GetModelName(),
                            IsOnline = true
                        };

                        memoryService.AppendHistory("assistant", response.Text);
                        memoryService.SaveMemory(memory);
                        return response;
                    }
                }
                catch (Exception ex)
                {
                    failureReason = SummarizeApiError(ex.Message);
                }
            }

            response = new AssistantResponse
            {
                Text = BuildOfflineResponse(userMessage, mode, memory),
                Source = failureReason == string.Empty ? "Base local" : "Base local (OpenAI indisponivel: " + failureReason + ")",
                IsOnline = false
            };

            memoryService.AppendHistory("assistant", response.Text);
            memoryService.SaveMemory(memory);
            return response;
        }

        public void ClearConversation()
        {
            memoryService.ClearHistory();
        }

        private void UpdateMemory(AssistantMemory memory, string message, string mode)
        {
            if (memory == null)
            {
                return;
            }

            memory.LastMode = mode;
            string lower = (message ?? string.Empty).ToLowerInvariant();
            string marker = "meu nome e ";
            int index = lower.IndexOf(marker);

            if (index >= 0)
            {
                string name = message.Substring(index + marker.Length).Trim();
                if (!string.IsNullOrWhiteSpace(name))
                {
                    memory.UserName = name;
                }
            }

            if (lower.Contains("c#") && !memory.FavoriteTopics.Contains("C#"))
            {
                memory.FavoriteTopics.Add("C#");
            }

            if (lower.Contains("windows forms") && !memory.FavoriteTopics.Contains("Windows Forms"))
            {
                memory.FavoriteTopics.Add("Windows Forms");
            }
        }

        private string BuildInstructions(string mode, AssistantMemory memory)
        {
            AssistantProfile profile = knowledgeService.LoadProfile();
            List<KnowledgeItem> knowledge = knowledgeService.LoadKnowledge().Take(4).ToList();

            StringBuilder builder = new StringBuilder();
            builder.AppendLine("Voce e " + profile.Name + ".");
            builder.AppendLine("Papel: " + profile.Role + ".");
            builder.AppendLine("Tom: " + profile.Tone + ".");
            builder.AppendLine("Objetivo: " + profile.Goal + ".");
            builder.AppendLine("Usuario atual: " + memory.UserName + ".");
            builder.AppendLine("Modo atual: " + mode + ".");
            builder.AppendLine("Regras:");

            foreach (string rule in profile.Rules)
            {
                builder.AppendLine("- " + rule);
            }

            builder.AppendLine("Quando a duvida for tecnica, responda com passos claros e exemplos pequenos.");
            builder.AppendLine("Se nao souber, diga o que falta e ofereca um caminho pratico.");
            builder.AppendLine("Nao finja acesso a coisas que voce nao tem.");
            builder.AppendLine("Trechos de conhecimento local relevantes:");

            foreach (KnowledgeItem item in knowledge)
            {
                builder.AppendLine(item.Title + ": " + item.Content);
            }

            return builder.ToString();
        }

        private string BuildInput(string userMessage, string mode, AssistantMemory memory)
        {
            List<KnowledgeItem> relevantKnowledge = knowledgeService.Search(userMessage, mode);
            List<ConversationTurn> history = memoryService.LoadHistory();
            List<ConversationTurn> recent = history.Skip(Math.Max(0, history.Count - 8)).ToList();

            StringBuilder builder = new StringBuilder();
            builder.AppendLine("Contexto do app:");
            builder.AppendLine("- nome do usuario: " + memory.UserName);
            builder.AppendLine("- modo selecionado: " + mode);
            builder.AppendLine("- topicos favoritos: " + string.Join(", ", memory.FavoriteTopics));
            builder.AppendLine();
            builder.AppendLine("Historico recente:");

            foreach (ConversationTurn turn in recent)
            {
                builder.AppendLine(turn.Role + ": " + turn.Message);
            }

            builder.AppendLine();
            builder.AppendLine("Conhecimento local relevante:");

            foreach (KnowledgeItem item in relevantKnowledge)
            {
                builder.AppendLine(item.Title + " - " + item.Content);
            }

            builder.AppendLine();
            builder.AppendLine("Pergunta atual do usuario:");
            builder.AppendLine(userMessage);

            return builder.ToString();
        }

        private string BuildOfflineResponse(string userMessage, string mode, AssistantMemory memory)
        {
            string lower = (userMessage ?? string.Empty).ToLowerInvariant();
            List<KnowledgeItem> knowledge = knowledgeService.Search(userMessage, mode);
            List<ConversationTurn> recent = memoryService.LoadHistory().Skip(Math.Max(0, memoryService.LoadHistory().Count - 6)).ToList();
            string assistantName = LoadProfile().Name;

            if (IsGreeting(lower))
            {
                return GetGreetingResponse(memory.UserName);
            }

            if (IsShortCheckIn(lower))
            {
                return "To aqui com voce, " + memory.UserName + ". Se quiser, a gente pode bater papo normal ou entrar em codigo, projeto, estudo, ideia ou debug.";
            }

            if (lower.Contains("seu nome"))
            {
                return "Eu sou a " + assistantName + ". Sou a assistente desse projeto, com memoria local, historico e base de conhecimento editavel.";
            }

            if (lower.Contains("meu nome"))
            {
                return "Beleza, " + memory.UserName + ". Vou lembrar disso enquanto usamos o aplicativo.";
            }

            if (lower.Contains("quem e voce") || lower.Contains("o que voce faz") || lower.Contains("o que vc faz"))
            {
                return "Eu sou a " + assistantName + ". Posso conversar de forma geral, ajudar com ideias, organizar explicacoes, responder duvidas de estudo e dar suporte em C# e Windows Forms.";
            }

            if (lower.Contains("obrigad") || lower.Contains("valeu"))
            {
                return "Tamo junto. Se quiser, continuo daqui sem enrolar.";
            }

            if (lower.Contains("tchau") || lower.Contains("falou"))
            {
                return "Fechou. Quando voltar, eu continuo de onde paramos.";
            }

            if (lower.Contains("boa tarde") || lower.Contains("bom dia") || lower.Contains("boa noite"))
            {
                return "Boa. Como voce quer seguir agora: conversa normal, projeto novo ou ajuda tecnica?";
            }

            if (lower.Contains("tudo bem") || lower.Contains("como voce ta") || lower.Contains("como voce esta"))
            {
                return "Estou bem e pronta para ajudar. E voce, quer descontrair um pouco ou resolver alguma coisa agora?";
            }

            if (lower.Contains("triste") || lower.Contains("desanimado") || lower.Contains("ansioso"))
            {
                return "Entendo. Quando a cabeca pesa, vale baixar a meta do momento. Em vez de pensar no projeto inteiro, escolhe uma coisa pequena para fechar agora e eu te acompanho nela.";
            }

            if (lower.Contains("o que eu faço") || lower.Contains("to perdido") || lower.Contains("estou perdido"))
            {
                return "Se voce estiver perdido, faz assim:\r\n1. me diga o objetivo\r\n2. me diga o que ja tem\r\n3. me diga onde travou\r\n\r\nCom isso eu consigo te puxar para frente sem te enrolar.";
            }

            if (lower.Contains("opa"))
            {
                return "Opa. To por aqui. Manda o que voce precisa que eu entro junto.";
            }

            if (lower.Contains("if") && lower.Contains("else"))
            {
                return "if testa uma condicao. else e o caminho alternativo.\r\n\r\nExemplo:\r\nif (txtNome.Text == \"\")\r\n{\r\n    MessageBox.Show(\"Digite o nome\");\r\n}\r\nelse\r\n{\r\n    lblResultado.Text = \"Ola\";\r\n}";
            }

            if (lower.Contains("debug") || lower.Contains("erro") || lower.Contains("bug"))
            {
                return "Roteiro de debug:\r\n1. Leia a mensagem completa.\r\n2. Veja em qual evento aconteceu.\r\n3. Confirme se algum controle esta vazio.\r\n4. Teste com valor simples.\r\n5. Use breakpoint e F10.\r\n\r\nSe quiser, cola o erro exato e eu monto uma analise melhor.";
            }

            if (lower.Contains("ideia") || lower.Contains("projeto"))
            {
                return GetProjectIdea();
            }

            if (lower.Contains("c#") || lower.Contains("windows forms") || lower.Contains("winforms"))
            {
                return "Para estudar melhor no WinForms: controles, eventos, validacao, listagem e pequenos projetos completos. Posso te explicar um por vez ou montar codigo pronto.";
            }

            if (lower.Contains("memoria") || lower.Contains("lembrar"))
            {
                return "Hoje eu lembro do seu nome, do modo atual, de alguns topicos que voce curte e de um historico recente da conversa. Isso ja ajuda a resposta a ficar menos solta.";
            }

            if (lower.Contains("como funciona"))
            {
                return "Eu funciono em camadas: perfil, memoria, historico, base de conhecimento e motor de resposta. Quando a API online nao esta disponivel, eu uso tudo isso para responder offline.";
            }

            if (knowledge.Count > 0)
            {
                StringBuilder builder = new StringBuilder();
                builder.AppendLine("Encontrei isso na base local:");

                foreach (KnowledgeItem item in knowledge)
                {
                    builder.AppendLine();
                    builder.AppendLine(item.Title);
                    builder.AppendLine(item.Content);
                }

                builder.AppendLine();
                builder.Append("Se quiser, eu tambem posso transformar isso em passo a passo.");
                return builder.ToString();
            }

            if (lower.Contains("isso") || lower.Contains("esse") || lower.Contains("essa"))
            {
                string contextHint = GetLastUserTopic(recent);
                if (contextHint != string.Empty)
                {
                    return "Acho que voce esta se referindo a `" + contextHint + "`. Se quiser, eu aprofundo nisso de um jeito mais simples ou mais tecnico.";
                }
            }

            switch (mode)
            {
                case "Professor":
                    return "Modo Professor:\r\nQuebre o problema em entrada, processamento e saida. Me diga o que entra no programa, o que ele faz e o que mostra na tela.";

                case "Debug":
                    return "Modo Debug:\r\nMe diga o texto do erro, em qual botao ele acontece e o que voce esperava que acontecesse.";

                case "C#":
                    return "Modo C#:\r\nPosso te responder com exemplo de variavel, if, metodo, evento, classe e loop. Seja especifico e eu monto um exemplo.";

                case "Windows Forms":
                    return "Modo Windows Forms:\r\nPosso te ajudar com Name dos controles, eventos Click, MessageBox, Label, TextBox, ListBox e Designer.";

                case "Ideias":
                    return GetProjectIdea();

                default:
                    return GetGeneralFallback(memory, recent);
            }
        }

        private string GetProjectIdea()
        {
            string[] ideas =
            {
                "App: bloco de notas com salvar e limpar.\r\nTreina TextBox grande, Button e arquivo.",
                "App: quiz de perguntas.\r\nTreina Label, RadioButton, Button e contador.",
                "App: lista de tarefas.\r\nTreina TextBox, ListBox e remocao de itens.",
                "App: gerador de estudos do dia.\r\nTreina ComboBox, Random e Label."
            };

            return ideas[random.Next(ideas.Length)];
        }

        private string SummarizeApiError(string message)
        {
            string text = (message ?? string.Empty).ToLowerInvariant();

            if (text.Contains("insufficient_quota"))
            {
                return "sem quota ou billing";
            }

            if (text.Contains("429"))
            {
                return "limite de requisicoes";
            }

            if (text.Contains("401") || text.Contains("unauthorized"))
            {
                return "api key invalida";
            }

            if (text.Contains("404"))
            {
                return "endpoint ou modelo nao encontrado";
            }

            if (text.Contains("timeout"))
            {
                return "tempo esgotado";
            }

            return "falha de conexao";
        }

        private bool IsGreeting(string lower)
        {
            string[] greetings =
            {
                "oi", "ola", "olá", "opa", "e ai", "eae", "salve", "hey", "hello", "boa tarde", "bom dia", "boa noite"
            };

            return greetings.Any(item => lower == item || lower.StartsWith(item + " "));
        }

        private bool IsShortCheckIn(string lower)
        {
            string[] checks =
            {
                "blz", "beleza", "suave", "tranquilo", "de boa", "ta ai", "ta por ai", "esta ai"
            };

            return checks.Any(item => lower == item || lower.Contains(item));
        }

        private string GetGreetingResponse(string userName)
        {
            string[] responses =
            {
                "Opa, " + userName + ". To aqui. Quer conversar normal ou quer resolver alguma coisa?",
                "Oi, " + userName + ". Manda o que voce tem em mente e eu sigo com voce.",
                "Salve, " + userName + ". Se estiver sem rumo, eu te ajudo a organizar a proxima acao."
            };

            return responses[random.Next(responses.Length)];
        }

        private string GetLastUserTopic(List<ConversationTurn> recent)
        {
            ConversationTurn lastUser = recent.LastOrDefault(item => item.Role == "user");
            if (lastUser == null || string.IsNullOrWhiteSpace(lastUser.Message))
            {
                return string.Empty;
            }

            string message = lastUser.Message.Trim();
            return message.Length > 40 ? message.Substring(0, 40) + "..." : message;
        }

        private string GetGeneralFallback(AssistantMemory memory, List<ConversationTurn> recent)
        {
            string previousTopic = GetLastUserTopic(recent);

            if (previousTopic != string.Empty)
            {
                return "Ainda nao saquei totalmente o que voce quis dizer, " + memory.UserName + ". A ultima pista que eu tenho e `" + previousTopic + "`. Se voce reformular em uma frase, eu tento responder melhor.";
            }

            return "Consigo conversar de forma geral tambem. Se quiser, me fala o que voce esta pensando, sentindo ou tentando fazer, que eu respondo num tom mais humano ou mais tecnico.";
        }
    }
}
