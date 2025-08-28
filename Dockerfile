FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Otimiza copiando apenas os csproj primeiro.
COPY MgFinanceiro.sln .
COPY MgFinanceiro/MgFinanceiro.csproj MgFinanceiro/
COPY MgFinanceiro.Application/MgFinanceiro.Application.csproj MgFinanceiro.Application/
COPY MgFinanceiro.Domain/MgFinanceiro.Domain.csproj MgFinanceiro.Domain/
COPY MgFinanceiro.Infrastructure/MgFinanceiro.Infrastructure.csproj MgFinanceiro.Infrastructure/

RUN dotnet restore MgFinanceiro/MgFinanceiro.csproj

COPY . .

RUN dotnet publish MgFinanceiro/MgFinanceiro.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080

ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "MgFinanceiro.dll"]
