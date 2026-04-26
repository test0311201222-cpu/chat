using System;
using System.Collections.Generic;
using System.Globalization;
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
            string lower = Normalize(message);
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

            if (lower.Contains("c#"))
            {
                AddUnique(memory.FavoriteTopics, "C#");
                AddUnique(memory.RecentTopics, "codigo");
            }

            if (lower.Contains("windows forms") || lower.Contains("winforms"))
            {
                AddUnique(memory.FavoriteTopics, "Windows Forms");
                AddUnique(memory.RecentTopics, "interface");
            }

            if (lower.Contains("estudo"))
            {
                AddUnique(memory.RecentTopics, "estudo");
            }

            if (lower.Contains("projeto") || lower.Contains("ideia"))
            {
                AddUnique(memory.RecentTopics, "projeto");
            }

            if (lower.Contains("trabalho"))
            {
                AddUnique(memory.RecentTopics, "trabalho");
            }

            if (lower.Contains("triste"))
            {
                memory.LastMood = "triste";
            }
            else if (lower.Contains("ansioso"))
            {
                memory.LastMood = "ansioso";
            }
            else if (lower.Contains("desanimado"))
            {
                memory.LastMood = "desanimado";
            }
            else if (lower.Contains("feliz") || lower.Contains("animado"))
            {
                memory.LastMood = "animado";
            }

            if (memory.RecentTopics.Count > 8)
            {
                memory.RecentTopics = memory.RecentTopics.Skip(memory.RecentTopics.Count - 8).ToList();
            }
        }

        private string BuildInstructions(string mode, AssistantMemory memory)
        {
            AssistantProfile profile = knowledgeService.LoadProfile();
            List<KnowledgeItem> knowledge = knowledgeService.LoadKnowledge().Take(6).ToList();

            StringBuilder builder = new StringBuilder();
            builder.AppendLine("Voce e " + profile.Name + ".");
            builder.AppendLine("Papel: " + profile.Role + ".");
            builder.AppendLine("Tom: " + profile.Tone + ".");
            builder.AppendLine("Objetivo: " + profile.Goal + ".");
            builder.AppendLine("Usuario atual: " + memory.UserName + ".");
            builder.AppendLine("Modo atual: " + mode + ".");
            builder.AppendLine("Temas favoritos do usuario: " + string.Join(", ", memory.FavoriteTopics) + ".");
            builder.AppendLine("Regras:");

            foreach (string rule in profile.Rules)
            {
                builder.AppendLine("- " + rule);
            }

            builder.AppendLine("Quando a duvida for tecnica, responda com passos claros e exemplos pequenos.");
            builder.AppendLine("Quando a conversa for casual, responda de forma humana e natural.");
            builder.AppendLine("Se nao souber, diga isso com sinceridade e peca contexto.");
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
            builder.AppendLine("- assuntos recentes: " + string.Join(", ", memory.RecentTopics));
            builder.AppendLine("- ultimo humor percebido: " + memory.LastMood);
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
            string lower = Normalize(userMessage);
            List<KnowledgeItem> knowledge = knowledgeService.Search(userMessage, mode);
            List<ConversationTurn> history = memoryService.LoadHistory();
            List<ConversationTurn> recent = history.Skip(Math.Max(0, history.Count - 6)).ToList();
            string assistantName = LoadProfile().Name;

            if (IsGreeting(lower))
            {
                return GetGreetingResponse(memory.UserName);
            }

            if (IsShortCheckIn(lower))
            {
                return "To aqui com voce, " + memory.UserName + ". Se quiser, a gente pode bater papo normal, falar de vida, estudar ou mexer em projeto.";
            }

            if (lower.Contains("seu nome"))
            {
                return "Eu sou a " + assistantName + ". Sou a assistente local desse projeto e tento conversar de forma natural, alem de ajudar em estudo e codigo.";
            }

            if (lower.Contains("meu nome"))
            {
                return "Beleza, " + memory.UserName + ". Vou lembrar disso enquanto usamos o aplicativo.";
            }

            if (lower.Contains("quem e voce") || lower.Contains("o que voce faz") || lower.Contains("o que vc faz"))
            {
                return "Eu sou a " + assistantName + ". Posso conversar de forma geral, ajudar com ideias, organizacao, estudo, codigo e projetos.";
            }

            if (lower.Contains("quem sou eu") || lower.Contains("o que voce lembra"))
            {
                return BuildMemorySummary(memory);
            }

            if (lower.Contains("obrigad") || lower.Contains("valeu"))
            {
                return "Tamo junto. Se quiser, continuo daqui sem enrolar.";
            }

            if (lower.Contains("tchau") || lower.Contains("falou"))
            {
                return "Fechou. Quando voltar, eu continuo de onde paramos.";
            }

            if (lower.Contains("tudo bem") || lower.Contains("como voce ta") || lower.Contains("como voce esta"))
            {
                return "Estou bem e pronta para ajudar. E voce, quer conversar de boa ou resolver alguma coisa agora?";
            }

            if (lower.Contains("triste") || lower.Contains("desanimado") || lower.Contains("ansioso") || lower.Contains("cansado"))
            {
                return "Entendo. Quando a cabeca pesa, vale baixar a meta do momento. Em vez de pensar em tudo de uma vez, escolhe uma acao pequena e eu te acompanho nela.";
            }

            if (lower.Contains("o que eu faco") || lower.Contains("to perdido") || lower.Contains("estou perdido"))
            {
                return "Se voce estiver perdido, faz assim:\r\n1. me diga o objetivo\r\n2. me diga o que ja tem\r\n3. me diga onde travou\r\n\r\nCom isso eu consigo te puxar para frente sem te enrolar.";
            }

            if (lower.Contains("memoria") || lower.Contains("lembrar"))
            {
                return "Hoje eu lembro do seu nome, do modo atual, de temas que voce costuma tocar, de alguns assuntos recentes e do clima geral da conversa.";
            }

            if (lower.Contains("como funciona"))
            {
                return "Eu funciono em camadas: perfil, memoria, historico, base de conhecimento e motor de resposta. Sem API online, eu fico totalmente offline usando essas camadas.";
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

            if (knowledge.Count > 0)
            {
                return BuildKnowledgeResponse(knowledge, lower);
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

        private string BuildKnowledgeResponse(List<KnowledgeItem> knowledge, string lower)
        {
            KnowledgeItem first = knowledge.First();

            if (knowledge.Count == 1)
            {
                return first.Content;
            }

            if (lower.Contains("dicas") || lower.Contains("melhorar") || lower.Contains("ajuda"))
            {
                StringBuilder tips = new StringBuilder();
                tips.AppendLine("Posso te orientar por este caminho:");

                foreach (KnowledgeItem item in knowledge)
                {
                    tips.AppendLine("- " + item.Content);
                }

                return tips.ToString().Trim();
            }

            return first.Content + "\r\n\r\nSe quiser, eu tambem posso aprofundar por este lado: " + string.Join(", ", knowledge.Skip(1).Select(item => item.Title.ToLowerInvariant())) + ".";
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
                "oi", "ola", "opa", "e ai", "eae", "salve", "hey", "hello", "bom dia", "boa tarde", "boa noite"
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

        private string BuildMemorySummary(AssistantMemory memory)
        {
            StringBuilder summary = new StringBuilder();
            summary.AppendLine("O que eu lembro agora:");
            summary.AppendLine("- seu nome: " + memory.UserName);
            summary.AppendLine("- ultimo modo: " + memory.LastMode);

            if (memory.FavoriteTopics.Count > 0)
            {
                summary.AppendLine("- temas que voce costuma tocar: " + string.Join(", ", memory.FavoriteTopics));
            }

            if (memory.RecentTopics.Count > 0)
            {
                summary.AppendLine("- assuntos recentes: " + string.Join(", ", memory.RecentTopics));
            }

            if (!string.IsNullOrWhiteSpace(memory.LastMood))
            {
                summary.AppendLine("- clima recente: " + memory.LastMood);
            }

            return summary.ToString().Trim();
        }

        private string GetGeneralFallback(AssistantMemory memory, List<ConversationTurn> recent)
        {
            string previousTopic = GetLastUserTopic(recent);

            if (previousTopic != string.Empty && !IsGreeting(Normalize(previousTopic)))
            {
                return "Ainda nao saquei totalmente o que voce quis dizer, " + memory.UserName + ". A ultima pista que eu tenho e `" + previousTopic + "`. Se voce reformular em uma frase, eu tento responder melhor.";
            }

            return "Consigo conversar de forma geral tambem. Me fala o que voce esta pensando, sentindo ou tentando fazer, que eu respondo num tom mais humano ou mais tecnico.";
        }

        private void AddUnique(List<string> list, string value)
        {
            if (!list.Contains(value))
            {
                list.Add(value);
            }
        }

        private string Normalize(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return string.Empty;
            }

            string normalized = value.ToLowerInvariant().Normalize(NormalizationForm.FormD);
            StringBuilder builder = new StringBuilder();

            foreach (char character in normalized)
            {
                UnicodeCategory category = CharUnicodeInfo.GetUnicodeCategory(character);

                if (category != UnicodeCategory.NonSpacingMark)
                {
                    builder.Append(character);
                }
            }

            return builder.ToString().Normalize(NormalizationForm.FormC);
        }
    }
}
