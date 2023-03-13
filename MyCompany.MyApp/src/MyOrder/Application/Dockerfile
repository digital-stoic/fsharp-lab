#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.
FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app
#EXPOSE 80
#EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG MODULE="MyCompany.MyApp.MyOrder.Application"
WORKDIR /src
#COPY "${MODULE}/${FULL_MODULE}.fsproj" "${FULL_MODULE}/"
COPY . "${MODULE}/"
RUN dotnet restore "${MODULE}/${MODULE}.fsproj"
RUN find /src
WORKDIR "/src/${MODULE}"
RUN dotnet build -c Release -o /app/build

#FROM build AS publish
#RUN dotnet publish "ContainerTest.fsproj" -c Release -o /app/publish /p:UseAppHost=false
#
#FROM base AS final
#WORKDIR /app
#COPY --from=publish /app/publish .
#ENTRYPOINT ["dotnet", "ContainerTest.dll"]