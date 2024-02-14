FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app

COPY sd.Auth.Web/sd.Auth.Web.csproj ./sd.Auth.Web/
COPY sd.Auth.Application/*.csproj ./sd.Auth.Application/
COPY sd.Auth.Domain/*.csproj ./sd.Auth.Domain/
COPY sd.Auth.Infrastructure/*.csproj ./sd.Auth.Infrastructure/

RUN dotnet restore sd.Auth.Web/sd.Auth.Web.csproj

COPY . .

WORKDIR /app/sd.Auth.Web
RUN dotnet publish -c release -o out

RUN ls -la /app/sd.Auth.Web/out
 
FROM mcr.microsoft.com/dotnet/sdk:6.0

WORKDIR /app
RUN dotnet dev-certs https
RUN dotnet dev-certs https --trust
COPY --from=build /app/sd.Auth.Web/out ./
EXPOSE 5011
ENTRYPOINT ["dotnet", "sd.Auth.Web.dll"]