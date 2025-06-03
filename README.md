# 🌡️ SECA LINK: Sistema de Controle de Setpoints e Kits com FIWARE

Este é um sistema web ASP.NET MVC desenvolvido com propósito didático e funcional para controle de alterações de setpoint de temperatura, gerenciamento de kits e usuários. A aplicação também integra com a plataforma **FIWARE** para controle remoto de temperatura (via MQTT). Desenvolvido para ambientes acadêmicos e laboratoriais.

## 📌 Funcionalidades

### 🔐 Autenticação e Sessões
- Login com verificação de credenciais (usuário e senha).
- Controle de sessão persistente para verificar se o usuário está logado.
- Diferenciação de **usuário comum** e **administrador**, com permissões distintas:
  - **Usuário comum:** pode visualizar e registrar alterações.
  - **Administrador:** pode editar, excluir e visualizar dados sensíveis (usuários e kits).

### 🧑‍💼 Gerenciamento de Usuários
- Cadastro de novos usuários (liberado sem login).
- Somente administradores podem editar e excluir usuários.
- Visualização da lista de usuários disponível apenas para administradores.

### 🧰 Controle de Kits
- Cadastro, edição, exclusão e visualização de kits de equipamento.
- Upload de imagem obrigatória (.jpg, .jpeg ou .png).
- Filtros dinâmicos por situação e descrição do equipamento.
- Controle da situação do kit (Em Funcionamento, Com Defeito, Em Manutenção).

### 🌡️ Registro de Alterações (Setpoints)
- Cadastro de alterações de temperatura (setpoint) com:
  - Validação de faixa permitida (0°C a 100°C).
  - Identificação do usuário responsável.
  - Escolha entre modos **MA** (Malha Aberta) e **MF** (Malha Fechada).
  - Descrição personalizada da alteração.
- Visualização de histórico com filtros dinâmicos por data e configuração.

### 📡 Integração com FIWARE
- Integração planejada via **protocolo HTTP e/ou MQTT** com a plataforma FIWARE para:
  - Envio de dados de setpoint.
  - Recebimento e exibição de temperatura atual.
- O sistema é ideal para exibir dashboards com dados em tempo real provenientes de sensores conectados a FIWARE (usualmente hospedado em VM na AWS).

### 📊 Interface Amigável
- _Layout_ padronizado com renderização de menus personalizados conforme tipo de usuário.
- Todas as páginas (Usuários, Kits, Alterações) com estrutura responsiva e organizada.
- Filtros em AJAX para listagens dinâmicas e ágeis.

---

## 🧠 Estrutura da Solução

### 🔎 Camadas
- **Models:** Representações de dados para Usuários, Kits e Alterações.
- **Controllers:** Lógica para processamento de views, sessões e permissões.
- **DAO:** Acesso ao banco via stored procedures (Insert, Update, Delete, Consulta, Listagem).
- **Views:** Páginas HTML organizadas em pastas por funcionalidade (com _Layout_ compartilhado).

### 📁 Estrutura de Pastas das Views

```text
Views/
├── Shared/        => Layout padrão, mensagens de erro, etc.
├── Login/         => Tela de autenticação
├── Usuarios/      => Cadastro e listagem de usuários
├── Alteracoes/    => Controle de setpoints
├── Kits/          => Gerenciamento de equipamentos
├── Home/          => Página inicial com dashboard e temperatura atual

---

## 🧪 Tecnologias Utilizadas

- ASP.NET Core MVC
- C# com Entity-Like Pattern (DAO customizado com Stored Procedures)
- SQL Server (banco de dados relacional)
- Bootstrap 4+
- AJAX para filtros assíncronos
- FIWARE (HTTP/MQTT - integração com sensores e atuadores)

---

## 🔒 Permissões e Regras

| Recurso                 | Usuário Comum | Administrador |
|-------------------------|---------------|---------------|
| Login                   | ✅            | ✅           |
| Cadastro                | ✅            | ✅           |
| Criar alteração         | ✅            | ✅           |
| Visualizar kits         | ✅            | ✅           |
| Filtrar dados           | ✅            | ✅           |
| Editar/Excluir kits     | ❌            | ✅           |
| Editar/Excluir usuários | ❌            | ✅           |
| Listagem de usuários    | ❌            | ✅           |

---

## 💾 Banco de Dados

### Stored Procedures Esperadas:
- `spInsert_[tabela]`, `spUpdate_[tabela]`, `spDelete`
- `spConsulta`, `spProximoId`, `spListagem[tabela]`
- `spConsultaPorLogin`

### Tabelas:
- **Usuarios:** `id`, `nome_usuario`, `login_usuario`, `senha_usuario`, `flag_admin`
- **Kits:** `id`, `nome`, `situacao`, `data_ultima_manutencao`, `descricao_equipamento`, `preco_equipamento`, `imagem`
- **Alteracoes:** `id`, `id_usuario`, `data_alteracao`, `setpoint`, `config_ma_mf`, `descricao_alteracao`

---

## 🚀 Como Rodar o Projeto

1. Configure o banco de dados SQL Server conforme a connection string:
```csharp
Data Source=(Seu banco de dados); Database=(nome do seu datasabe); user id=(seu id); password=(sua senha);

2. Crie as stored procedures e tabelas mencionadas;

3. Compile o projeto no Visual Studio.

Execute e acesse via navegador em: http://localhost:xxxx

## 👨‍💻 Autores

- 🧠 **Alex Saif de Souza**
- 💻 **Arthur Destro Gabrielli**
- 🛠️ **Gustavo Mauriz Silva**
- 🔬 **Vinicius Strazza Santos**
