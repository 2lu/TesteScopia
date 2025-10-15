# Dockerfile para API .NET
# Build da aplica��o
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copia os arquivos do projeto e restaura depend�ncias
COPY . ./
RUN dotnet restore

# Compila o projeto
RUN dotnet publish -c Release -o out

# Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

# Copia os arquivos publicados do build
COPY --from=build /app/out ./

# Exp�e a porta padr�o (ajuste conforme necess�rio)
EXPOSE 80

# Comando para iniciar a aplica��o
ENTRYPOINT ["dotnet", "Skopia.Api.dll"]

# Instru��es para execu��o com Docker
# Para rodar o cont�iner, execute o seguinte comando:
# docker run -p 8080:80 --env ASPNETCORE_ENVIRONMENT=Development skopia-api