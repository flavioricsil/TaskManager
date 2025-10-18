# TaskManager API

API RESTful para gerenciamento de projetos e tarefas, desenvolvida em .NET 8 (C#). Permite criar projetos, adicionar tarefas com prioridades, acompanhar o progresso, colaborar com comentários e gerar relatórios de desempenho. A arquitetura segue os princípios de Clean Architecture e DDD.

---

## Sumário

1. Tecnologias
2. Arquitetura e Padrões
3. Funcionalidades
4. Regras de Negócio
5. Execução com Docker
6. Endpoints da API
7. Autenticação e Autorização
8. Testes
9. Roadmap
10. Licença

---

## Tecnologias

| Categoria         | Tecnologia           | Descrição                                      |
|------------------|----------------------|------------------------------------------------|
| Backend           | .NET 8 (C#)          | Framework principal da API                     |
| Banco de Dados    | PostgreSQL           | Persistência de dados                          |
| ORM               | Entity Framework Core| Mapeamento Objeto-Relacional                   |
| Containerização   | Docker               | Execução isolada e padronizada                 |
| Testes            | xUnit                | Testes unitários e de integração               |
| Documentação      | Swagger/OpenAPI      | Interface interativa dos endpoints             |
| Padrões           | MediatR              | Implementação do padrão Mediator               |

---

## Arquitetura e Padrões

- Separação em camadas: Controller, Service, Repository e Domain
- Aplicação dos princípios SOLID
- Padrões utilizados:
  - Repository e Unit of Work
  - Mediator (via MediatR) para comandos e consultas desacoplados

---

## Funcionalidades

- Criar, listar e excluir projetos
- Criar, visualizar, atualizar e remover tarefas
- Adicionar comentários em tarefas
- Histórico de alterações por tarefa (data, usuário, tipo)
- Relatórios de desempenho por usuário

---

## Regras de Negócio

1. Prioridade da tarefa (baixa, média, alta) não pode ser alterada após criação
2. Projetos com tarefas pendentes ou em andamento não podem ser removidos
3. Limite de 20 tarefas por projeto
4. Relatórios de desempenho acessíveis apenas por usuários com papel de gerente

---

## Execução com Docker

Pré-requisitos:
- Docker e Git instalados
- (Opcional) .NET SDK 8 para testes locais

Passos:

1. Clonar o repositório:

```bash
git clone https://github.com/flavioricsil/TaskManager.git
cd taskmanager-api
```

2. Executar com Docker Compose:

```bash
docker-compose up --build -d
```

Ou apenas a API:

```bash
docker build -t taskmanager-api .
docker run -d -p 5000:80 --name taskmanager taskmanager-api
```

3. Acessar:
- API: http://localhost:5000/api
- Swagger: http://localhost:5000/swagger

---

## Endpoints da API

Base: http://localhost:5000/api

### Projetos

| Método  | Caminho             | Descrição                  |
|---------|---------------------|----------------------------|
| GET     | /projects           | Lista todos os projetos    |
| POST    | /projects           | Cria um novo projeto       |
| DELETE  | /projects/{id}      | Remove um projeto          |

### Tarefas

| Método  | Caminho                     | Descrição                          |
|---------|-----------------------------|------------------------------------|
| GET     | /projects/{id}/tasks        | Lista tarefas de um projeto        |
| POST    | /tasks                      | Cria uma nova tarefa               |
| PUT     | /tasks/{id}                 | Atualiza status ou descrição       |
| DELETE  | /tasks/{id}                 | Remove uma tarefa                  |
| POST    | /tasks/{id}/comments        | Adiciona comentário à tarefa       |

### Relatórios

| Método  | Caminho               | Descrição                          |
|---------|-----------------------|------------------------------------|
| GET     | /reports/performance  | Relatório de desempenho (gerente)  |

---

## Autenticação e Autorização

- Autenticação via Azure Active Directory (AAD) com tokens JWT
- Autorização por roles (Membro e Gerente)

---

## Testes

- Cobertura de testes acima de 80%
- Foco nas regras de negócio
- Execução local:

```bash
dotnet test
```

---

## Roadmap

### Arquitetura

- Implementar CQRS
- Migrar para microserviços
- Adicionar cache com Redis

### DevOps

- CI/CD com GitHub Actions ou Azure DevOps
- Deploy em Azure (App Service ou AKS)
- Monitoramento com Application Insights

### Funcionalidades

- Notificações em tempo real com WebSockets
- Controle de tempo por tarefa
- Integrações com Jira, Trello e Slack via Webhooks

---

## Licença

Este projeto é destinado exclusivamente para avaliação técnica e demonstração de habilidades em desenvolvimento de software.

