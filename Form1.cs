using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using chat.Models;
using chat.Services;

namespace chat
{
    public partial class Form1 : Form
    {
        private readonly AssistantService assistantService;
        private AssistantMemory memory;
        private AppSettings settings;
        private AssistantProfile profile;
        private List<KnowledgeItem> knowledgeItems = new List<KnowledgeItem>();
        private bool temaClaro = true;

        public Form1()
        {
            InitializeComponent();

            JsonStorage storage = new JsonStorage();
            KnowledgeService knowledgeService = new KnowledgeService(storage);
            MemoryService memoryService = new MemoryService(storage);
            ConfigService configService = new ConfigService(storage);
            OpenAiResponsesService openAiService = new OpenAiResponsesService(storage, configService);
            assistantService = new AssistantService(knowledgeService, memoryService, openAiService, configService);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cboModo.Items.AddRange(new object[] { "Geral", "C#", "Windows Forms", "Ideias", "Debug", "Professor" });
            cboConhecimentoCategoria.Items.AddRange(new object[] { "C#", "Windows Forms", "Ideias", "Debug", "Professor", "Geral" });

            memory = assistantService.LoadMemory();
            settings = assistantService.LoadSettings();
            profile = assistantService.LoadProfile();
            knowledgeItems = assistantService.LoadKnowledge();

            Text = profile.Name;
            lblTitulo.Text = profile.Name;

            int modeIndex = cboModo.Items.IndexOf(memory.LastMode);
            cboModo.SelectedIndex = modeIndex >= 0 ? modeIndex : 0;
            cboConhecimentoCategoria.SelectedIndex = 0;

            txtApiKey.Text = settings.OpenAiApiKey;
            txtModelo.Text = settings.OpenAiModel;

            RecarregarListaConhecimento();

            AdicionarMensagem(profile.Name, "Assistente iniciado.");
            AdicionarMensagem(profile.Name, assistantService.IsOnlineConfigured()
                ? "Modo online disponivel. Respostas podem usar IA online junto com memoria e base local."
                : "Modo local ativo. Abra Configuracao para colar sua API key sem sair do app.");

            if (!string.IsNullOrWhiteSpace(memory.UserName) && memory.UserName != "amigo")
            {
                AdicionarMensagem(profile.Name, "Bem-vindo de volta, " + memory.UserName + ".");
            }

            AtualizarStatusInicial();
            txtMensagem.Focus();
        }

        private void AtualizarStatusInicial()
        {
            lblStatus.Text = assistantService.IsOnlineConfigured()
                ? "Fonte principal: OpenAI + memoria local"
                : "Fonte principal: base local";
        }

        private async void btnEnviar_Click(object sender, EventArgs e)
        {
            await EnviarMensagemAsync();
        }

        private async void txtMensagem_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                await EnviarMensagemAsync();
            }
        }

        private void btnLimpar_Click(object sender, EventArgs e)
        {
            rtbConversa.Clear();
            assistantService.ClearConversation();
            AdicionarMensagem(profile.Name, "Historico limpo. Podemos recomecar.");
            lblStatus.Text = "Conversa local apagada.";
            txtMensagem.Clear();
            txtMensagem.Focus();
        }

        private void btnTema_Click(object sender, EventArgs e)
        {
            temaClaro = !temaClaro;

            if (temaClaro)
            {
                AplicarTemaClaro();
                lblStatus.Text = "Tema claro ativado.";
            }
            else
            {
                AplicarTemaEscuro();
                lblStatus.Text = "Tema escuro ativado.";
            }
        }

        private void btnCopiar_Click(object sender, EventArgs e)
        {
            if (rtbConversa.TextLength == 0)
            {
                MessageBox.Show("Nao ha conteudo para copiar.", "Aviso");
                return;
            }

            Clipboard.SetText(rtbConversa.Text);
            lblStatus.Text = "Conversa copiada.";
        }

        private async void btnAcao1_Click(object sender, EventArgs e)
        {
            txtMensagem.Text = "Explica if e else para iniciante";
            await EnviarMensagemAsync();
        }

        private async void btnAcao2_Click(object sender, EventArgs e)
        {
            txtMensagem.Text = "Cria um exemplo em C#";
            await EnviarMensagemAsync();
        }

        private async void btnAcao3_Click(object sender, EventArgs e)
        {
            txtMensagem.Text = "Me da uma ideia de app";
            await EnviarMensagemAsync();
        }

        private async void btnAcao4_Click(object sender, EventArgs e)
        {
            txtMensagem.Text = "Me ajuda a debugar";
            await EnviarMensagemAsync();
        }

        private async Task EnviarMensagemAsync()
        {
            string mensagem = txtMensagem.Text.Trim();

            if (mensagem == "")
            {
                MessageBox.Show("Digite alguma mensagem antes de enviar.", "Aviso");
                txtMensagem.Focus();
                return;
            }

            AlternarEntrada(false);
            AdicionarMensagem("Voce", mensagem);
            txtMensagem.Clear();
            lblStatus.Text = "Pensando...";

            AssistantResponse resposta = await assistantService.GetResponseAsync(mensagem, cboModo.Text, memory);
            memory.LastMode = cboModo.Text;
            assistantService.SaveMemory(memory);

            AdicionarMensagem(profile.Name, resposta.Text + Environment.NewLine + "[fonte: " + resposta.Source + "]");
            lblStatus.Text = resposta.IsOnline
                ? "Resposta online recebida em " + DateTime.Now.ToString("HH:mm")
                : "Resposta local recebida em " + DateTime.Now.ToString("HH:mm");

            AlternarEntrada(true);
            txtMensagem.Focus();
        }

        private void AdicionarMensagem(string autor, string mensagem)
        {
            rtbConversa.SelectionStart = rtbConversa.TextLength;
            rtbConversa.SelectionLength = 0;
            rtbConversa.SelectionColor = autor == "Voce" ? Color.FromArgb(37, 99, 235) : Color.FromArgb(15, 23, 42);
            rtbConversa.AppendText("[" + DateTime.Now.ToString("HH:mm") + "] " + autor + Environment.NewLine);
            rtbConversa.SelectionColor = rtbConversa.ForeColor;
            rtbConversa.AppendText(mensagem + Environment.NewLine + Environment.NewLine);
            rtbConversa.ScrollToCaret();
        }

        private void AlternarEntrada(bool habilitado)
        {
            btnEnviar.Enabled = habilitado;
            btnLimpar.Enabled = habilitado;
            btnTema.Enabled = habilitado;
            btnCopiar.Enabled = habilitado;
            btnAcao1.Enabled = habilitado;
            btnAcao2.Enabled = habilitado;
            btnAcao3.Enabled = habilitado;
            btnAcao4.Enabled = habilitado;
            txtMensagem.Enabled = habilitado;
            cboModo.Enabled = habilitado;
        }

        private void RecarregarListaConhecimento()
        {
            lstConhecimento.Items.Clear();

            foreach (KnowledgeItem item in knowledgeItems)
            {
                lstConhecimento.Items.Add(item.Category + " - " + item.Title);
            }
        }

        private void lstConhecimento_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = lstConhecimento.SelectedIndex;

            if (index < 0 || index >= knowledgeItems.Count)
            {
                return;
            }

            KnowledgeItem item = knowledgeItems[index];
            txtConhecimentoTitulo.Text = item.Title;
            txtConhecimentoPalavras.Text = string.Join(", ", item.Keywords);
            txtConhecimentoConteudo.Text = item.Content;

            int categoryIndex = cboConhecimentoCategoria.Items.IndexOf(item.Category);
            cboConhecimentoCategoria.SelectedIndex = categoryIndex >= 0 ? categoryIndex : 0;
        }

        private void btnNovoConhecimento_Click(object sender, EventArgs e)
        {
            lstConhecimento.ClearSelected();
            txtConhecimentoTitulo.Clear();
            txtConhecimentoPalavras.Clear();
            txtConhecimentoConteudo.Clear();
            cboConhecimentoCategoria.SelectedIndex = 0;
            txtConhecimentoTitulo.Focus();
        }

        private void btnSalvarConhecimento_Click(object sender, EventArgs e)
        {
            string titulo = txtConhecimentoTitulo.Text.Trim();
            string conteudo = txtConhecimentoConteudo.Text.Trim();

            if (titulo == "" || conteudo == "")
            {
                MessageBox.Show("Titulo e conteudo sao obrigatorios.", "Aviso");
                return;
            }

            KnowledgeItem item = new KnowledgeItem
            {
                Title = titulo,
                Category = Convert.ToString(cboConhecimentoCategoria.SelectedItem),
                Keywords = txtConhecimentoPalavras.Text
                    .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(keyword => keyword.Trim())
                    .Where(keyword => keyword != "")
                    .ToList(),
                Content = conteudo
            };

            int index = lstConhecimento.SelectedIndex;

            if (index >= 0 && index < knowledgeItems.Count)
            {
                knowledgeItems[index] = item;
            }
            else
            {
                knowledgeItems.Add(item);
            }

            assistantService.SaveKnowledge(knowledgeItems);
            RecarregarListaConhecimento();
            lblStatus.Text = "Base de conhecimento salva.";
        }

        private void btnExcluirConhecimento_Click(object sender, EventArgs e)
        {
            int index = lstConhecimento.SelectedIndex;

            if (index < 0 || index >= knowledgeItems.Count)
            {
                MessageBox.Show("Selecione um item para excluir.", "Aviso");
                return;
            }

            knowledgeItems.RemoveAt(index);
            assistantService.SaveKnowledge(knowledgeItems);
            RecarregarListaConhecimento();
            btnNovoConhecimento_Click(sender, e);
            lblStatus.Text = "Item removido da base.";
        }

        private void btnSalvarConfig_Click(object sender, EventArgs e)
        {
            settings.OpenAiApiKey = txtApiKey.Text.Trim();
            settings.OpenAiModel = txtModelo.Text.Trim() == "" ? "gpt-5-mini" : txtModelo.Text.Trim();
            assistantService.SaveSettings(settings);
            AtualizarStatusInicial();
            MessageBox.Show("Configuracao salva.", profile.Name);
        }

        private void AplicarTemaClaro()
        {
            BackColor = Color.FromArgb(241, 245, 249);
            tabChat.BackColor = BackColor;
            tabConhecimento.BackColor = BackColor;
            tabConfiguracao.BackColor = BackColor;
            panelHeader.BackColor = Color.FromArgb(15, 23, 42);
            lblTitulo.ForeColor = Color.White;
            lblSubtitulo.ForeColor = Color.FromArgb(191, 219, 254);
            rtbConversa.BackColor = Color.White;
            rtbConversa.ForeColor = Color.Black;
            panelEntrada.BackColor = Color.White;
            panelLateral.BackColor = Color.White;
            lblStatus.ForeColor = Color.FromArgb(75, 85, 99);
            lblDicas.ForeColor = Color.Black;
            lblModo.ForeColor = Color.Black;
        }

        private void AplicarTemaEscuro()
        {
            BackColor = Color.FromArgb(15, 23, 42);
            tabChat.BackColor = BackColor;
            tabConhecimento.BackColor = BackColor;
            tabConfiguracao.BackColor = BackColor;
            panelHeader.BackColor = Color.FromArgb(2, 6, 23);
            lblTitulo.ForeColor = Color.FromArgb(125, 211, 252);
            lblSubtitulo.ForeColor = Color.FromArgb(186, 230, 253);
            rtbConversa.BackColor = Color.FromArgb(30, 41, 59);
            rtbConversa.ForeColor = Color.WhiteSmoke;
            panelEntrada.BackColor = Color.FromArgb(30, 41, 59);
            panelLateral.BackColor = Color.FromArgb(30, 41, 59);
            lblStatus.ForeColor = Color.Gainsboro;
            lblDicas.ForeColor = Color.WhiteSmoke;
            lblModo.ForeColor = Color.WhiteSmoke;
        }
    }
}
