# Sistema de Gerenciamento de Projetos e Tarefas

## Perguntas aos Po's e melhorias (fase 3)

 1 .  Colocar Nlog para geracao de log de requisicoes e auditoria em produção

 2 .  Usar fluentValidator para validação de entrada de dados e resposta com uso de result object

 3 .  Permitir que Projeto não sejam excluidos quando status for 'Em Andamento' ou 'Concluída'

## Rodando com Docker (fase 2)

1. Certifique-se de ter o Docker instalado.
2. No diretório do projeto, construa a imagem:

```sh
docker build -t skopia-api
```

3. Execute o container:

```sh
docker run -p 8080:80 --env ASPNETCORE_ENVIRONMENT=Development skopia-api
```

## Tecnologias Utilizadas

- **.NET 9** - Framework principal
- **Minimal API** - Arquitetura de API leve e moderna
- **MongoDB Atlas** - Banco de dados NoSQL em nuvem
- **Swagger/OpenAPI** - Documentação interativa da API

## Configuração

As configurações do MongoDB estão no arquivo `appsettings.json`:

\`\`\`json
{
  "MongoDB": {
    "ConnectionString": "mongodb+srv://Skopia:teste@cluster0.ic0yi.mongodb.net/?retryWrites=true&w=majority&appName=Cluster0",
    "DatabaseName": "Skopia"
  }
}
\`\`\`
