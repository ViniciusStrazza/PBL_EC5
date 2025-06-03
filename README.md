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
- Integração planejada via **protocolo MQTT** com a plataforma FIWARE para:
  - Recebimento e exibição de temperatura atual;
  - Recebimento e exibição de temperatura histórica, considerando a data de alteração mais recente (integrado com o CRUD!);
- O sistema é ideal para exibir dashboards com dados em tempo real provenientes de sensores conectados a FIWARE (hospedado em VM na AWS).

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

---

## 📁 Estrutura de Pastas das Views

```
Views/
├── Shared/        => Layout padrão, mensagens de erro, etc.
├── Login/         => Tela de autenticação
├── Usuarios/      => Cadastro e listagem de usuários
├── Alteracoes/    => Controle de setpoints
├── Kits/          => Gerenciamento de equipamentos
├── Home/          => Página inicial com dashboard e temperatura atual
```

---

## 🔒 Permissões e Regras

| Recurso                 | Usuário Comum | Administrador |
|-------------------------|---------------|---------------|
| Login                   | ✅            | ✅           |
| Cadastro                | ✅            | ✅           |
| Visualizar kits         | ✅            | ✅           |
| Filtrar dados           | ✅            | ✅           |
| Criar alteração         | ❌            | ✅           |
| Editar/Excluir kits     | ❌            | ✅           |
| Editar/Excluir usuários | ❌            | ✅           |
| Listagem de usuários    | ❌            | ✅           |

---

## 💾 Banco de Dados

### Funcional, com Stored Procedures
- Utiliza procedures (`spInsert`, `spUpdate`, `spDelete`, etc.) para garantir segurança (evita SQL Injection) e desempenho no acesso aos dados.

### Tabelas
- Estruturadas com relacionamentos lógicos entre usuários, kits e alterações.
- Suporte à recuperação de identidade, listagens ordenadas e consultas filtradas.

---

## 🚀 Como Rodar o Projeto

1. Configure o banco de dados SQL Server conforme a `connection string`:
```csharp
Data Source=(Seu servidor banco de dados); Database=(nome do seu database); user id=(seu id); password=(sua senha);
```

2. Crie as stored procedures e tabelas mencionadas (vide arquivo `query_cruds.sql`).

3. Compile o projeto no Visual Studio.

4. Execute e acesse via navegador em: `http://localhost:xxxx`. (quando _hosteado_ em nuvem)

---

## 👨‍💻 Autores

- 🧠 **Alex Saif de Souza**
- 💻 **Arthur Destro Gabrielli**
- 🛠️ **Gustavo Mauriz Silva**
- 🔬 **Vinicius Strazza Santos**

---

Projeto desenvolvido com foco educacional, integrando conceitos de IoT, automação e sistemas supervisórios com tecnologias modernas da web.
