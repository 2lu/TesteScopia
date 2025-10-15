# Sistema de Gerenciamento de Projetos e Tarefas

## TesteScopia

Este projeto é uma API .NET.

## Rodando com Docker

1. Certifique-se de ter o Docker instalado.
2. No diretório do projeto, construa a imagem:

```sh
docker build -t skopia-api .
```

3. Execute o container:

```sh
docker run -d -p 8080:80 --name skopia-api skopia-api
```

A API estará disponível em `http://localhost:8080`.

API RESTful desenvolvida em .NET 9 com Minimal API sem autenticação.

## Tecnologias Utilizadas

- **.NET 9** - Framework principal
- **Minimal API** - Arquitetura de API leve e moderna
- **MongoDB Atlas** - Banco de dados NoSQL em nuvem
- **Swagger/OpenAPI** - Documentação interativa da API

## Arquitetura

O projeto segue uma arquitetura monolítica em camadas:

### Camadas

1. **Models** - Entidades de domínio e enums
2. **Repositories** - Camada de acesso a dados (MongoDB)
3. **Services** - Lógica de negócio
4. **API** - Endpoints da Minimal API

### Projetos
- ✅ Listar projetos
- ✅ Visualizar um projeto
- ✅ Novo projeto
- ✅ Gravar informações no projeto
- ✅ Remover projeto

### Tarefas
- ✅ Listar todas as tarefas
- ✅ Visualizar tarefas de um projeto específico
- ✅ Criar nova tarefa (limite de 20 por projeto)
- ✅ Atualizar tarefa
- ✅ Remover tarefa
- ✅ Prioridades (baixa, média, alta)
- ✅ Status (pendente, em andamento, concluída)

- **Validações**: 
   - Título do projeto: máximo 100 caracteres (obrigatório)
   - Descrição do projeto: máximo 500 caracteres
   - Título da tarefa: máximo 100 caracteres (obrigatório)
   - Descrição da tarefa: máximo 500 caracteres

## Como Executar

### Pré-requisitos
- .NET 9 SDK
- Conexão com internet (para MongoDB Atlas)

### Passos

1. Clone o repositório  
2. Execute o comando:
    dotnet run oua abra o projeto no Visual Studio e inicie a aplicação.

## Swagger em: `http://localhost:5000/swagger` ou `https://localhost:5001/swagger`

## Endpoints da API

### Projetos

- `GET /api/projetos` - Listar todos os projetos
- `GET /api/projetos/{id}` - Obter projeto por ID
- `POST /api/projetos` - Criar novo projeto
- `PUT /api/projetos/{id}` - Atualizar projeto
- `DELETE /api/projetos/{id}` - Remover projeto

### Tarefas

- `GET /api/tarefas` - Listar todas as tarefas
- `GET /api/tarefas/{id}` - Obter tarefa por ID
- `GET /api/projetos/{projetoId}/tarefas` - Listar tarefas de um projeto
- `POST /api/tarefas` - Criar nova tarefa
- `PUT /api/tarefas/{id}` - Atualizar tarefa
- `DELETE /api/tarefas/{id}` - Remover tarefa

### Coleções MongoDB

- **projetos**: Armazena informações dos projetos
- **tarefas**: Armazena tarefas vinculadas a projetos

### Status da Tarefa

- `1` - Pendente
- `2` - Em Andamento
- `3` - Concluída

### Prioridade da Tarefa

- `1` - Baixa
- `2` - Média
- `3` - Alta

## Configuração

As configurações do MongoDB estão no arquivo `appsettings.json`:

\`\`\`json
{
  "MongoDB": {
    "ConnectionString": "mongodb+srv://Skopia:jzgXc7rmNaFHFhFh@cluster0.ic0yi.mongodb.net/?retryWrites=true&w=majority&appName=Cluster0",
    "DatabaseName": "Skopia"
  }
}
\`\`\`

## Autor

Luis Alberto Silva (doislu.sw@gmail.com).
