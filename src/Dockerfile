FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 5000


FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Gateway.API/Gateway.API.csproj", "Gateway.API/"]
COPY ["Gateway.CrossCutting/Gateway.CrossCutting.csproj", "Gateway.CrossCutting/"]
COPY ["Gateway.Application/Gateway.Application.csproj", "Gateway.Application/"]
COPY ["Gateway.Domain/Gateway.Domain.csproj", "Gateway.Domain/"]
COPY ["Gateway.Infra.Data/Gateway.Infra.Data.csproj", "Gateway.Infra.Data/"]
RUN dotnet restore "./Gateway.API/Gateway.API.csproj"
COPY . .
WORKDIR "/src/Gateway.API"
RUN dotnet build "./Gateway.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Gateway.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

CMD ASPNETCORE_URLS="http://*:$PORT" dotnet Gateway.API.dll