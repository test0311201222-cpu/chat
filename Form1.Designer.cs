namespace chat
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.panelHeader = new System.Windows.Forms.Panel();
            this.lblSubtitulo = new System.Windows.Forms.Label();
            this.lblTitulo = new System.Windows.Forms.Label();
            this.tabPrincipal = new System.Windows.Forms.TabControl();
            this.tabChat = new System.Windows.Forms.TabPage();
            this.panelLateral = new System.Windows.Forms.Panel();
            this.btnAcao4 = new System.Windows.Forms.Button();
            this.btnAcao3 = new System.Windows.Forms.Button();
            this.btnAcao2 = new System.Windows.Forms.Button();
            this.btnAcao1 = new System.Windows.Forms.Button();
            this.cboModo = new System.Windows.Forms.ComboBox();
            this.lblModo = new System.Windows.Forms.Label();
            this.lblDicas = new System.Windows.Forms.Label();
            this.rtbConversa = new System.Windows.Forms.RichTextBox();
            this.panelEntrada = new System.Windows.Forms.Panel();
            this.btnCopiar = new System.Windows.Forms.Button();
            this.btnTema = new System.Windows.Forms.Button();
            this.btnLimpar = new System.Windows.Forms.Button();
            this.btnEnviar = new System.Windows.Forms.Button();
            this.txtMensagem = new System.Windows.Forms.TextBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.tabConhecimento = new System.Windows.Forms.TabPage();
            this.btnNovoConhecimento = new System.Windows.Forms.Button();
            this.btnSalvarConhecimento = new System.Windows.Forms.Button();
            this.btnExcluirConhecimento = new System.Windows.Forms.Button();
            this.txtConhecimentoConteudo = new System.Windows.Forms.TextBox();
            this.txtConhecimentoPalavras = new System.Windows.Forms.TextBox();
            this.txtConhecimentoTitulo = new System.Windows.Forms.TextBox();
            this.cboConhecimentoCategoria = new System.Windows.Forms.ComboBox();
            this.lblConteudo = new System.Windows.Forms.Label();
            this.lblPalavras = new System.Windows.Forms.Label();
            this.lblTituloConhecimento = new System.Windows.Forms.Label();
            this.lblCategoria = new System.Windows.Forms.Label();
            this.lstConhecimento = new System.Windows.Forms.ListBox();
            this.tabConfiguracao = new System.Windows.Forms.TabPage();
            this.lblResumo = new System.Windows.Forms.Label();
            this.btnSalvarConfig = new System.Windows.Forms.Button();
            this.txtModelo = new System.Windows.Forms.TextBox();
            this.txtApiKey = new System.Windows.Forms.TextBox();
            this.lblModelo = new System.Windows.Forms.Label();
            this.lblApiKey = new System.Windows.Forms.Label();
            this.panelHeader.SuspendLayout();
            this.tabPrincipal.SuspendLayout();
            this.tabChat.SuspendLayout();
            this.panelLateral.SuspendLayout();
            this.panelEntrada.SuspendLayout();
            this.tabConhecimento.SuspendLayout();
            this.tabConfiguracao.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelHeader
            // 
            this.panelHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.panelHeader.Controls.Add(this.lblSubtitulo);
            this.panelHeader.Controls.Add(this.lblTitulo);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(1164, 96);
            this.panelHeader.TabIndex = 0;
            // 
            // lblSubtitulo
            // 
            this.lblSubtitulo.AutoSize = true;
            this.lblSubtitulo.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblSubtitulo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(254)))));
            this.lblSubtitulo.Location = new System.Drawing.Point(28, 54);
            this.lblSubtitulo.Name = "lblSubtitulo";
            this.lblSubtitulo.Size = new System.Drawing.Size(495, 19);
            this.lblSubtitulo.TabIndex = 1;
            this.lblSubtitulo.Text = "Assistente local com memoria, conhecimento editavel e conexao com IA online";
            // 
            // lblTitulo
            // 
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI Semibold", 22F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.ForeColor = System.Drawing.Color.White;
            this.lblTitulo.Location = new System.Drawing.Point(24, 10);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(181, 41);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "Codex Local";
            // 
            // tabPrincipal
            // 
            this.tabPrincipal.Controls.Add(this.tabChat);
            this.tabPrincipal.Controls.Add(this.tabConhecimento);
            this.tabPrincipal.Controls.Add(this.tabConfiguracao);
            this.tabPrincipal.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.tabPrincipal.Location = new System.Drawing.Point(12, 112);
            this.tabPrincipal.Name = "tabPrincipal";
            this.tabPrincipal.SelectedIndex = 0;
            this.tabPrincipal.Size = new System.Drawing.Size(1140, 657);
            this.tabPrincipal.TabIndex = 1;
            // 
            // tabChat
            // 
            this.tabChat.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            this.tabChat.Controls.Add(this.panelLateral);
            this.tabChat.Controls.Add(this.rtbConversa);
            this.tabChat.Controls.Add(this.panelEntrada);
            this.tabChat.Location = new System.Drawing.Point(4, 26);
            this.tabChat.Name = "tabChat";
            this.tabChat.Padding = new System.Windows.Forms.Padding(3);
            this.tabChat.Size = new System.Drawing.Size(1132, 627);
            this.tabChat.TabIndex = 0;
            this.tabChat.Text = "Chat";
            // 
            // panelLateral
            // 
            this.panelLateral.BackColor = System.Drawing.Color.White;
            this.panelLateral.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelLateral.Controls.Add(this.btnAcao4);
            this.panelLateral.Controls.Add(this.btnAcao3);
            this.panelLateral.Controls.Add(this.btnAcao2);
            this.panelLateral.Controls.Add(this.btnAcao1);
            this.panelLateral.Controls.Add(this.cboModo);
            this.panelLateral.Controls.Add(this.lblModo);
            this.panelLateral.Controls.Add(this.lblDicas);
            this.panelLateral.Location = new System.Drawing.Point(810, 18);
            this.panelLateral.Name = "panelLateral";
            this.panelLateral.Size = new System.Drawing.Size(300, 456);
            this.panelLateral.TabIndex = 0;
            // 
            // btnAcao4
            // 
            this.btnAcao4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(254)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.btnAcao4.FlatAppearance.BorderSize = 0;
            this.btnAcao4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAcao4.Location = new System.Drawing.Point(19, 241);
            this.btnAcao4.Name = "btnAcao4";
            this.btnAcao4.Size = new System.Drawing.Size(258, 46);
            this.btnAcao4.TabIndex = 6;
            this.btnAcao4.Text = "Me ajuda a debugar";
            this.btnAcao4.UseVisualStyleBackColor = false;
            this.btnAcao4.Click += new System.EventHandler(this.btnAcao4_Click);
            // 
            // btnAcao3
            // 
            this.btnAcao3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(252)))), ((int)(((byte)(231)))));
            this.btnAcao3.FlatAppearance.BorderSize = 0;
            this.btnAcao3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAcao3.Location = new System.Drawing.Point(19, 185);
            this.btnAcao3.Name = "btnAcao3";
            this.btnAcao3.Size = new System.Drawing.Size(258, 46);
            this.btnAcao3.TabIndex = 5;
            this.btnAcao3.Text = "Me da uma ideia de app";
            this.btnAcao3.UseVisualStyleBackColor = false;
            this.btnAcao3.Click += new System.EventHandler(this.btnAcao3_Click);
            // 
            // btnAcao2
            // 
            this.btnAcao2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(254)))), ((int)(((byte)(249)))), ((int)(((byte)(195)))));
            this.btnAcao2.FlatAppearance.BorderSize = 0;
            this.btnAcao2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAcao2.Location = new System.Drawing.Point(19, 129);
            this.btnAcao2.Name = "btnAcao2";
            this.btnAcao2.Size = new System.Drawing.Size(258, 46);
            this.btnAcao2.TabIndex = 4;
            this.btnAcao2.Text = "Cria um exemplo em C#";
            this.btnAcao2.UseVisualStyleBackColor = false;
            this.btnAcao2.Click += new System.EventHandler(this.btnAcao2_Click);
            // 
            // btnAcao1
            // 
            this.btnAcao1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(231)))), ((int)(((byte)(255)))));
            this.btnAcao1.FlatAppearance.BorderSize = 0;
            this.btnAcao1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAcao1.Location = new System.Drawing.Point(19, 73);
            this.btnAcao1.Name = "btnAcao1";
            this.btnAcao1.Size = new System.Drawing.Size(258, 46);
            this.btnAcao1.TabIndex = 3;
            this.btnAcao1.Text = "Explica if e else";
            this.btnAcao1.UseVisualStyleBackColor = false;
            this.btnAcao1.Click += new System.EventHandler(this.btnAcao1_Click);
            // 
            // cboModo
            // 
            this.cboModo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboModo.FormattingEnabled = true;
            this.cboModo.Location = new System.Drawing.Point(19, 360);
            this.cboModo.Name = "cboModo";
            this.cboModo.Size = new System.Drawing.Size(258, 25);
            this.cboModo.TabIndex = 2;
            // 
            // lblModo
            // 
            this.lblModo.AutoSize = true;
            this.lblModo.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold);
            this.lblModo.Location = new System.Drawing.Point(15, 329);
            this.lblModo.Name = "lblModo";
            this.lblModo.Size = new System.Drawing.Size(127, 20);
            this.lblModo.TabIndex = 1;
            this.lblModo.Text = "Modo de resposta";
            // 
            // lblDicas
            // 
            this.lblDicas.AutoSize = true;
            this.lblDicas.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.lblDicas.Location = new System.Drawing.Point(15, 20);
            this.lblDicas.Name = "lblDicas";
            this.lblDicas.Size = new System.Drawing.Size(190, 21);
            this.lblDicas.TabIndex = 0;
            this.lblDicas.Text = "Atalhos e modos de teste";
            // 
            // rtbConversa
            // 
            this.rtbConversa.BackColor = System.Drawing.Color.White;
            this.rtbConversa.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.rtbConversa.Font = new System.Drawing.Font("Consolas", 10.5F);
            this.rtbConversa.Location = new System.Drawing.Point(20, 18);
            this.rtbConversa.Name = "rtbConversa";
            this.rtbConversa.ReadOnly = true;
            this.rtbConversa.Size = new System.Drawing.Size(769, 456);
            this.rtbConversa.TabIndex = 1;
            this.rtbConversa.Text = "";
            // 
            // panelEntrada
            // 
            this.panelEntrada.BackColor = System.Drawing.Color.White;
            this.panelEntrada.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelEntrada.Controls.Add(this.btnCopiar);
            this.panelEntrada.Controls.Add(this.btnTema);
            this.panelEntrada.Controls.Add(this.btnLimpar);
            this.panelEntrada.Controls.Add(this.btnEnviar);
            this.panelEntrada.Controls.Add(this.txtMensagem);
            this.panelEntrada.Controls.Add(this.lblStatus);
            this.panelEntrada.Location = new System.Drawing.Point(20, 491);
            this.panelEntrada.Name = "panelEntrada";
            this.panelEntrada.Size = new System.Drawing.Size(1090, 112);
            this.panelEntrada.TabIndex = 2;
            // 
            // btnCopiar
            // 
            this.btnCopiar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.btnCopiar.FlatAppearance.BorderSize = 0;
            this.btnCopiar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCopiar.ForeColor = System.Drawing.Color.White;
            this.btnCopiar.Location = new System.Drawing.Point(738, 58);
            this.btnCopiar.Name = "btnCopiar";
            this.btnCopiar.Size = new System.Drawing.Size(92, 34);
            this.btnCopiar.TabIndex = 5;
            this.btnCopiar.Text = "Copiar";
            this.btnCopiar.UseVisualStyleBackColor = false;
            this.btnCopiar.Click += new System.EventHandler(this.btnCopiar_Click);
            // 
            // btnTema
            // 
            this.btnTema.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(158)))), ((int)(((byte)(11)))));
            this.btnTema.FlatAppearance.BorderSize = 0;
            this.btnTema.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTema.Location = new System.Drawing.Point(838, 58);
            this.btnTema.Name = "btnTema";
            this.btnTema.Size = new System.Drawing.Size(92, 34);
            this.btnTema.TabIndex = 4;
            this.btnTema.Text = "Tema";
            this.btnTema.UseVisualStyleBackColor = false;
            this.btnTema.Click += new System.EventHandler(this.btnTema_Click);
            // 
            // btnLimpar
            // 
            this.btnLimpar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(68)))), ((int)(((byte)(68)))));
            this.btnLimpar.FlatAppearance.BorderSize = 0;
            this.btnLimpar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLimpar.ForeColor = System.Drawing.Color.White;
            this.btnLimpar.Location = new System.Drawing.Point(938, 58);
            this.btnLimpar.Name = "btnLimpar";
            this.btnLimpar.Size = new System.Drawing.Size(92, 34);
            this.btnLimpar.TabIndex = 3;
            this.btnLimpar.Text = "Limpar";
            this.btnLimpar.UseVisualStyleBackColor = false;
            this.btnLimpar.Click += new System.EventHandler(this.btnLimpar_Click);
            // 
            // btnEnviar
            // 
            this.btnEnviar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(99)))), ((int)(((byte)(235)))));
            this.btnEnviar.FlatAppearance.BorderSize = 0;
            this.btnEnviar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEnviar.ForeColor = System.Drawing.Color.White;
            this.btnEnviar.Location = new System.Drawing.Point(1037, 58);
            this.btnEnviar.Name = "btnEnviar";
            this.btnEnviar.Size = new System.Drawing.Size(36, 34);
            this.btnEnviar.TabIndex = 2;
            this.btnEnviar.Text = "Ir";
            this.btnEnviar.UseVisualStyleBackColor = false;
            this.btnEnviar.Click += new System.EventHandler(this.btnEnviar_Click);
            // 
            // txtMensagem
            // 
            this.txtMensagem.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.txtMensagem.Location = new System.Drawing.Point(18, 61);
            this.txtMensagem.Name = "txtMensagem";
            this.txtMensagem.Size = new System.Drawing.Size(702, 27);
            this.txtMensagem.TabIndex = 1;
            this.txtMensagem.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtMensagem_KeyDown);
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(85)))), ((int)(((byte)(99)))));
            this.lblStatus.Location = new System.Drawing.Point(14, 20);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(310, 19);
            this.lblStatus.TabIndex = 0;
            this.lblStatus.Text = "Pergunte, edite sua base ou conecte IA online aqui";
            // 
            // tabConhecimento
            // 
            this.tabConhecimento.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            this.tabConhecimento.Controls.Add(this.btnNovoConhecimento);
            this.tabConhecimento.Controls.Add(this.btnSalvarConhecimento);
            this.tabConhecimento.Controls.Add(this.btnExcluirConhecimento);
            this.tabConhecimento.Controls.Add(this.txtConhecimentoConteudo);
            this.tabConhecimento.Controls.Add(this.txtConhecimentoPalavras);
            this.tabConhecimento.Controls.Add(this.txtConhecimentoTitulo);
            this.tabConhecimento.Controls.Add(this.cboConhecimentoCategoria);
            this.tabConhecimento.Controls.Add(this.lblConteudo);
            this.tabConhecimento.Controls.Add(this.lblPalavras);
            this.tabConhecimento.Controls.Add(this.lblTituloConhecimento);
            this.tabConhecimento.Controls.Add(this.lblCategoria);
            this.tabConhecimento.Controls.Add(this.lstConhecimento);
            this.tabConhecimento.Location = new System.Drawing.Point(4, 26);
            this.tabConhecimento.Name = "tabConhecimento";
            this.tabConhecimento.Padding = new System.Windows.Forms.Padding(3);
            this.tabConhecimento.Size = new System.Drawing.Size(1132, 627);
            this.tabConhecimento.TabIndex = 1;
            this.tabConhecimento.Text = "Conhecimento";
            // 
            // btnNovoConhecimento
            // 
            this.btnNovoConhecimento.Location = new System.Drawing.Point(20, 575);
            this.btnNovoConhecimento.Name = "btnNovoConhecimento";
            this.btnNovoConhecimento.Size = new System.Drawing.Size(128, 32);
            this.btnNovoConhecimento.TabIndex = 11;
            this.btnNovoConhecimento.Text = "Novo";
            this.btnNovoConhecimento.UseVisualStyleBackColor = true;
            this.btnNovoConhecimento.Click += new System.EventHandler(this.btnNovoConhecimento_Click);
            // 
            // btnSalvarConhecimento
            // 
            this.btnSalvarConhecimento.Location = new System.Drawing.Point(749, 575);
            this.btnSalvarConhecimento.Name = "btnSalvarConhecimento";
            this.btnSalvarConhecimento.Size = new System.Drawing.Size(170, 32);
            this.btnSalvarConhecimento.TabIndex = 9;
            this.btnSalvarConhecimento.Text = "Salvar item";
            this.btnSalvarConhecimento.UseVisualStyleBackColor = true;
            this.btnSalvarConhecimento.Click += new System.EventHandler(this.btnSalvarConhecimento_Click);
            // 
            // btnExcluirConhecimento
            // 
            this.btnExcluirConhecimento.Location = new System.Drawing.Point(935, 575);
            this.btnExcluirConhecimento.Name = "btnExcluirConhecimento";
            this.btnExcluirConhecimento.Size = new System.Drawing.Size(170, 32);
            this.btnExcluirConhecimento.TabIndex = 10;
            this.btnExcluirConhecimento.Text = "Excluir item";
            this.btnExcluirConhecimento.UseVisualStyleBackColor = true;
            this.btnExcluirConhecimento.Click += new System.EventHandler(this.btnExcluirConhecimento_Click);
            // 
            // txtConhecimentoConteudo
            // 
            this.txtConhecimentoConteudo.Location = new System.Drawing.Point(369, 168);
            this.txtConhecimentoConteudo.Multiline = true;
            this.txtConhecimentoConteudo.Name = "txtConhecimentoConteudo";
            this.txtConhecimentoConteudo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtConhecimentoConteudo.Size = new System.Drawing.Size(736, 385);
            this.txtConhecimentoConteudo.TabIndex = 8;
            // 
            // txtConhecimentoPalavras
            // 
            this.txtConhecimentoPalavras.Location = new System.Drawing.Point(369, 112);
            this.txtConhecimentoPalavras.Name = "txtConhecimentoPalavras";
            this.txtConhecimentoPalavras.Size = new System.Drawing.Size(736, 25);
            this.txtConhecimentoPalavras.TabIndex = 7;
            // 
            // txtConhecimentoTitulo
            // 
            this.txtConhecimentoTitulo.Location = new System.Drawing.Point(369, 56);
            this.txtConhecimentoTitulo.Name = "txtConhecimentoTitulo";
            this.txtConhecimentoTitulo.Size = new System.Drawing.Size(480, 25);
            this.txtConhecimentoTitulo.TabIndex = 5;
            // 
            // cboConhecimentoCategoria
            // 
            this.cboConhecimentoCategoria.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboConhecimentoCategoria.FormattingEnabled = true;
            this.cboConhecimentoCategoria.Location = new System.Drawing.Point(865, 56);
            this.cboConhecimentoCategoria.Name = "cboConhecimentoCategoria";
            this.cboConhecimentoCategoria.Size = new System.Drawing.Size(240, 25);
            this.cboConhecimentoCategoria.TabIndex = 6;
            // 
            // lblConteudo
            // 
            this.lblConteudo.AutoSize = true;
            this.lblConteudo.Location = new System.Drawing.Point(365, 145);
            this.lblConteudo.Name = "lblConteudo";
            this.lblConteudo.Size = new System.Drawing.Size(68, 19);
            this.lblConteudo.TabIndex = 4;
            this.lblConteudo.Text = "Conteudo";
            // 
            // lblPalavras
            // 
            this.lblPalavras.AutoSize = true;
            this.lblPalavras.Location = new System.Drawing.Point(365, 89);
            this.lblPalavras.Name = "lblPalavras";
            this.lblPalavras.Size = new System.Drawing.Size(154, 19);
            this.lblPalavras.TabIndex = 3;
            this.lblPalavras.Text = "Palavras-chave, separadas";
            // 
            // lblTituloConhecimento
            // 
            this.lblTituloConhecimento.AutoSize = true;
            this.lblTituloConhecimento.Location = new System.Drawing.Point(365, 33);
            this.lblTituloConhecimento.Name = "lblTituloConhecimento";
            this.lblTituloConhecimento.Size = new System.Drawing.Size(42, 19);
            this.lblTituloConhecimento.TabIndex = 2;
            this.lblTituloConhecimento.Text = "Titulo";
            // 
            // lblCategoria
            // 
            this.lblCategoria.AutoSize = true;
            this.lblCategoria.Location = new System.Drawing.Point(861, 33);
            this.lblCategoria.Name = "lblCategoria";
            this.lblCategoria.Size = new System.Drawing.Size(67, 19);
            this.lblCategoria.TabIndex = 1;
            this.lblCategoria.Text = "Categoria";
            // 
            // lstConhecimento
            // 
            this.lstConhecimento.FormattingEnabled = true;
            this.lstConhecimento.ItemHeight = 17;
            this.lstConhecimento.Location = new System.Drawing.Point(20, 20);
            this.lstConhecimento.Name = "lstConhecimento";
            this.lstConhecimento.Size = new System.Drawing.Size(320, 548);
            this.lstConhecimento.TabIndex = 0;
            this.lstConhecimento.SelectedIndexChanged += new System.EventHandler(this.lstConhecimento_SelectedIndexChanged);
            // 
            // tabConfiguracao
            // 
            this.tabConfiguracao.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            this.tabConfiguracao.Controls.Add(this.lblResumo);
            this.tabConfiguracao.Controls.Add(this.btnSalvarConfig);
            this.tabConfiguracao.Controls.Add(this.txtModelo);
            this.tabConfiguracao.Controls.Add(this.txtApiKey);
            this.tabConfiguracao.Controls.Add(this.lblModelo);
            this.tabConfiguracao.Controls.Add(this.lblApiKey);
            this.tabConfiguracao.Location = new System.Drawing.Point(4, 26);
            this.tabConfiguracao.Name = "tabConfiguracao";
            this.tabConfiguracao.Padding = new System.Windows.Forms.Padding(3);
            this.tabConfiguracao.Size = new System.Drawing.Size(1132, 627);
            this.tabConfiguracao.TabIndex = 2;
            this.tabConfiguracao.Text = "Configuracao";
            // 
            // lblResumo
            // 
            this.lblResumo.AutoSize = true;
            this.lblResumo.Location = new System.Drawing.Point(42, 34);
            this.lblResumo.Name = "lblResumo";
            this.lblResumo.Size = new System.Drawing.Size(500, 57);
            this.lblResumo.TabIndex = 5;
            this.lblResumo.Text = "Cole sua chave aqui para ativar respostas online.\r\nSe a chave estiver vazia, o as" +
    "sistente usa a base local.\r\nO modelo padrao sugerido e gpt-5-mini.";
            // 
            // btnSalvarConfig
            // 
            this.btnSalvarConfig.Location = new System.Drawing.Point(45, 250);
            this.btnSalvarConfig.Name = "btnSalvarConfig";
            this.btnSalvarConfig.Size = new System.Drawing.Size(170, 36);
            this.btnSalvarConfig.TabIndex = 4;
            this.btnSalvarConfig.Text = "Salvar configuracao";
            this.btnSalvarConfig.UseVisualStyleBackColor = true;
            this.btnSalvarConfig.Click += new System.EventHandler(this.btnSalvarConfig_Click);
            // 
            // txtModelo
            // 
            this.txtModelo.Location = new System.Drawing.Point(45, 200);
            this.txtModelo.Name = "txtModelo";
            this.txtModelo.Size = new System.Drawing.Size(340, 25);
            this.txtModelo.TabIndex = 3;
            // 
            // txtApiKey
            // 
            this.txtApiKey.Location = new System.Drawing.Point(45, 135);
            this.txtApiKey.Name = "txtApiKey";
            this.txtApiKey.Size = new System.Drawing.Size(682, 25);
            this.txtApiKey.TabIndex = 2;
            // 
            // lblModelo
            // 
            this.lblModelo.AutoSize = true;
            this.lblModelo.Location = new System.Drawing.Point(41, 178);
            this.lblModelo.Name = "lblModelo";
            this.lblModelo.Size = new System.Drawing.Size(56, 19);
            this.lblModelo.TabIndex = 1;
            this.lblModelo.Text = "Modelo";
            // 
            // lblApiKey
            // 
            this.lblApiKey.AutoSize = true;
            this.lblApiKey.Location = new System.Drawing.Point(41, 113);
            this.lblApiKey.Name = "lblApiKey";
            this.lblApiKey.Size = new System.Drawing.Size(66, 19);
            this.lblApiKey.TabIndex = 0;
            this.lblApiKey.Text = "API Key";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            this.ClientSize = new System.Drawing.Size(1164, 781);
            this.Controls.Add(this.tabPrincipal);
            this.Controls.Add(this.panelHeader);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Codex Local";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            this.tabPrincipal.ResumeLayout(false);
            this.tabChat.ResumeLayout(false);
            this.panelLateral.ResumeLayout(false);
            this.panelLateral.PerformLayout();
            this.panelEntrada.ResumeLayout(false);
            this.panelEntrada.PerformLayout();
            this.tabConhecimento.ResumeLayout(false);
            this.tabConhecimento.PerformLayout();
            this.tabConfiguracao.ResumeLayout(false);
            this.tabConfiguracao.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Label lblSubtitulo;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.TabControl tabPrincipal;
        private System.Windows.Forms.TabPage tabChat;
        private System.Windows.Forms.Panel panelLateral;
        private System.Windows.Forms.Button btnAcao4;
        private System.Windows.Forms.Button btnAcao3;
        private System.Windows.Forms.Button btnAcao2;
        private System.Windows.Forms.Button btnAcao1;
        private System.Windows.Forms.ComboBox cboModo;
        private System.Windows.Forms.Label lblModo;
        private System.Windows.Forms.Label lblDicas;
        private System.Windows.Forms.RichTextBox rtbConversa;
        private System.Windows.Forms.Panel panelEntrada;
        private System.Windows.Forms.Button btnCopiar;
        private System.Windows.Forms.Button btnTema;
        private System.Windows.Forms.Button btnLimpar;
        private System.Windows.Forms.Button btnEnviar;
        private System.Windows.Forms.TextBox txtMensagem;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.TabPage tabConhecimento;
        private System.Windows.Forms.ListBox lstConhecimento;
        private System.Windows.Forms.Button btnSalvarConhecimento;
        private System.Windows.Forms.Button btnExcluirConhecimento;
        private System.Windows.Forms.TextBox txtConhecimentoConteudo;
        private System.Windows.Forms.TextBox txtConhecimentoPalavras;
        private System.Windows.Forms.TextBox txtConhecimentoTitulo;
        private System.Windows.Forms.ComboBox cboConhecimentoCategoria;
        private System.Windows.Forms.Label lblConteudo;
        private System.Windows.Forms.Label lblPalavras;
        private System.Windows.Forms.Label lblTituloConhecimento;
        private System.Windows.Forms.Label lblCategoria;
        private System.Windows.Forms.Button btnNovoConhecimento;
        private System.Windows.Forms.TabPage tabConfiguracao;
        private System.Windows.Forms.Label lblResumo;
        private System.Windows.Forms.Button btnSalvarConfig;
        private System.Windows.Forms.TextBox txtModelo;
        private System.Windows.Forms.TextBox txtApiKey;
        private System.Windows.Forms.Label lblModelo;
        private System.Windows.Forms.Label lblApiKey;
    }
}
