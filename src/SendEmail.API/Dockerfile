# Acesse https://aka.ms/customizecontainer para saber como personalizar seu contêiner de depuração e como o Visual Studio usa este Dockerfile para criar suas imagens para uma depuração mais rápida.

# Esta fase é usada durante a execução no VS no modo rápido (Padrão para a configuração de Depuração)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080


# Esta fase é usada para compilar o projeto de serviço
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["SendEmail.API/SendEmail.API.csproj", "SendEmail.API/"]
COPY ["SendEmail.CrossCutting/SendEmail.CrossCutting.csproj", "SendEmail.CrossCutting/"]
COPY ["SendEmail.Application/SendEmail.Application.csproj", "SendEmail.Application/"]
COPY ["SendEmail.Domain/SendEmail.Domain.csproj", "SendEmail.Domain/"]
COPY ["SendEmail.Infra.Data/SendEmail.Infra.Data.csproj", "SendEmail.Infra.Data/"]
RUN dotnet restore "./SendEmail.API/SendEmail.API.csproj"
COPY . .
WORKDIR "/src/SendEmail.API"
RUN dotnet build "./SendEmail.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Esta fase é usada para publicar o projeto de serviço a ser copiado para a fase final
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./SendEmail.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Esta fase é usada na produção ou quando executada no VS no modo normal (padrão quando não está usando a configuração de Depuração)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SendEmail.API.dll"]