#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/azure-functions/dotnet:4 AS base
WORKDIR /home/site/wwwroot
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Warehouse/Infrastructure/MyCompany.MyApp.Warehouse.Infrastructure.Azure/MyCompany.MyApp.Warehouse.Infrastructure.Azure.csproj", "src/Warehouse/Infrastructure/MyCompany.MyApp.Warehouse.Infrastructure.Azure/"]
RUN dotnet restore "src/Warehouse/Infrastructure/MyCompany.MyApp.Warehouse.Infrastructure.Azure/MyCompany.MyApp.Warehouse.Infrastructure.Azure.csproj"
COPY . .
WORKDIR "/src/src/Warehouse/Infrastructure/MyCompany.MyApp.Warehouse.Infrastructure.Azure"
RUN dotnet build "MyCompany.MyApp.Warehouse.Infrastructure.Azure.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MyCompany.MyApp.Warehouse.Infrastructure.Azure.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /home/site/wwwroot
COPY --from=publish /app/publish .
ENV AzureWebJobsScriptRoot=/home/site/wwwroot \
    AzureFunctionsJobHost__Logging__Console__IsEnabled=true