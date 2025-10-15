# Dockerfile para API .NET
# Build da aplicação
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copia os arquivos do projeto e restaura dependências
COPY . ./
RUN dotnet restore

# Compila o projeto
RUN dotnet publish -c Release -o out

# Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

# Copia os arquivos publicados do build
COPY --from=build /app/out ./

# Expõe a porta padrão (ajuste conforme necessário)
EXPOSE 80

# Comando para iniciar a aplicação
ENTRYPOINT ["dotnet", "Skopia.Api.dll"]

# Instruções para execução com Docker
# Para rodar o contêiner, execute o seguinte comando:
# docker run -p 8080:80 --env ASPNETCORE_ENVIRONMENT=Development skopia-api