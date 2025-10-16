# Etapa 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia os arquivos de projeto
COPY TaskManager.Domain/TaskManager.Domain.csproj TaskManager.Domain/
COPY TaskManager.Application/TaskManager.Application.csproj TaskManager.Application/
COPY TaskManager.Infrastructure/TaskManager.Infrastructure.csproj TaskManager.Infrastructure/
COPY TaskManager.API/TaskManager.API.csproj TaskManager.API/

# Restaura dependências
RUN dotnet restore TaskManager.API/TaskManager.API.csproj

# Copia o restante dos arquivos
COPY . .

# Publica a aplicação
RUN dotnet publish TaskManager.API/TaskManager.API.csproj -c Release -o /app

# Etapa 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "TaskManager.API.dll"]