#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 6060
ENV ASPNETCORE_URLS=http://*:6060

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["src/VivirSeguros.CRM.WebApi/VivirSeguros.CRM.WebApi.csproj", "src/VivirSeguros.CRM.WebApi/"]
COPY ["src/VivirSeguros.CRM.Application/VivirSeguros.CRM.Application.csproj", "src/VivirSeguros.CRM.Application/"]
COPY ["src/VivirSeguros.CRM.Domain/VivirSeguros.CRM.Domain.csproj", "src/VivirSeguros.CRM.Domain/"]
COPY ["src/VivirSeguros.CRM.Infrastructure/VivirSeguros.CRM.Infrastructure.csproj", "src/VivirSeguros.CRM.Infrastructure/"]
RUN dotnet restore "src/VivirSeguros.CRM.WebApi/VivirSeguros.CRM.WebApi.csproj"
COPY . .
WORKDIR "/src/src/VivirSeguros.CRM.WebApi"
RUN dotnet build "VivirSeguros.CRM.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "VivirSeguros.CRM.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "VivirSeguros.CRM.WebApi.dll"]