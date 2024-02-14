FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY sd.ApiGateway.Web.csproj .
RUN dotnet restore
COPY . .
RUN dotnet publish -c release -o /app
 
FROM mcr.microsoft.com/dotnet/sdk:6.0
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "sd.ApiGateway.Web.dll"]