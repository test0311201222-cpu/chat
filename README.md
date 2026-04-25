# Codex Local

Assistente de estudo em Windows Forms com:

- chat local
- memoria persistente
- historico persistente
- base de conhecimento editavel
- configuracao de API no proprio app
- integracao opcional com OpenAI pela Responses API

## Como usar

1. Abra o projeto no Visual Studio.
2. Execute em `Debug`.
3. Use a aba `Configuracao` para colar sua API key.
4. Use a aba `Conhecimento` para ensinar novas respostas.
5. Converse na aba `Chat`.

## Estrutura

- `Form1.cs`: interface principal
- `Models/`: classes de memoria, configuracao e conhecimento
- `Services/`: logica do assistente, persistencia e integracao online
- `Data/`: perfil do assistente e base inicial de conhecimento

## Observacao

Sem API key, o app funciona em modo local usando memoria e base de conhecimento.
