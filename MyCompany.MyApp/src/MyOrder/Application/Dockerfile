#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app
#EXPOSE 80
#EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
# FIXME: Use --build-arg in project file
ARG MODULE="MyCompany.MyApp.MyOrder.Application"
WORKDIR /src
COPY . "${MODULE}/"
RUN dotnet restore "${MODULE}/${MODULE}.fsproj"
WORKDIR "/src/${MODULE}"
RUN dotnet build -c Release -o /app/build

FROM build AS publish
WORKDIR "/src/${MODULE}"
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false
RUN find /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
# FIXME: Use a variable?
ENTRYPOINT ["dotnet", "MyCompany.MyApp.MyOrder.Application.dll"]