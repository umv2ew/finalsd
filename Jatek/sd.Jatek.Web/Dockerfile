#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 5005

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Jatek/sd.Jatek.Web/sd.Jatek.Web.csproj", "Jatek/sd.Jatek.Web/"]
COPY ["Jatek/sd.Jatek.Application/sd.Jatek.Application.csproj", "Jatek/sd.Jatek.Application/"]
COPY ["Jatek/sd.Jatek.Infrastructure/sd.Jatek.Infrastructure.csproj", "Jatek/sd.Jatek.Infrastructure/"]
COPY ["Jatek/sd.Jatek.Domain/sd.Jatek.Domain.csproj", "Jatek/sd.Jatek.Domain/"]
COPY ["Jatek/sd.Jatek.Integration/sd.Jatek.Integration.csproj", "Jatek/sd.Jatek.Integration/"]
RUN dotnet restore "./Jatek/sd.Jatek.Web/./sd.Jatek.Web.csproj"
COPY . .
WORKDIR "/src/Jatek/sd.Jatek.Web"
RUN dotnet build "./sd.Jatek.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet dev-certs https
RUN dotnet dev-certs https --trust
RUN dotnet publish "./sd.Jatek.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "sd.Jatek.Web.dll"]