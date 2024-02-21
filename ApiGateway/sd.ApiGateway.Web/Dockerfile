#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ApiGateway/sd.ApiGateway.Web/sd.ApiGateway.Web.csproj", "ApiGateway/sd.ApiGateway.Web/"]
RUN dotnet restore "./ApiGateway/sd.ApiGateway.Web/./sd.ApiGateway.Web.csproj"
COPY . .
WORKDIR "/src/ApiGateway/sd.ApiGateway.Web"
RUN dotnet build "./sd.ApiGateway.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet dev-certs https
RUN dotnet dev-certs https --trust
RUN dotnet publish "./sd.ApiGateway.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "sd.ApiGateway.Web.dll"]