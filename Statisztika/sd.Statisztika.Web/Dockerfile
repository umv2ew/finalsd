#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5031

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Statisztika/sd.Statisztika.Web/sd.Statisztika.Web.csproj", "Statisztika/sd.Statisztika.Web/"]
COPY ["Statisztika/sd.Statisztika.Application/sd.Statisztika.Application.csproj", "Statisztika/sd.Statisztika.Application/"]
COPY ["Jatek/sd.Jatek.Integration/sd.Jatek.Integration.csproj", "Jatek/sd.Jatek.Integration/"]
COPY ["Statisztika/sd.Statisztika.Infrastructure/sd.Statisztika.Infrastructure.csproj", "Statisztika/sd.Statisztika.Infrastructure/"]
COPY ["Statisztika/sd.Statisztika.Domain/sd.Statisztika.Domain.csproj", "Statisztika/sd.Statisztika.Domain/"]
RUN dotnet restore "./Statisztika/sd.Statisztika.Web/./sd.Statisztika.Web.csproj"
COPY . .
WORKDIR "/src/Statisztika/sd.Statisztika.Web"
RUN dotnet build "./sd.Statisztika.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet dev-certs https
RUN dotnet dev-certs https --trust
RUN dotnet publish "./sd.Statisztika.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
HEALTHCHECK CMD curl --fail http://0.0.0.0:5031/health || exit
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "sd.Statisztika.Web.dll"]
