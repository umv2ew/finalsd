#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 5011

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Auth/sd.Auth.Web/sd.Auth.Web.csproj", "Auth/sd.Auth.Web/"]
COPY ["Auth/sd.Auth.Domain/sd.Auth.Domain.csproj", "Auth/sd.Auth.Domain/"]
COPY ["Auth/sd.Auth.Infrastructure/sd.Auth.Infrastructure.csproj", "Auth/sd.Auth.Infrastructure/"]
RUN dotnet restore "./Auth/sd.Auth.Web/./sd.Auth.Web.csproj"
COPY . .
WORKDIR "/src/Auth/sd.Auth.Web"
RUN dotnet build "./sd.Auth.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./sd.Auth.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
RUN dotnet dev-certs https
RUN dotnet dev-certs https --trust
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "sd.Auth.Web.dll"]