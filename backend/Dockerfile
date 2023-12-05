FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app
COPY ./ ./

COPY nesprex-shared/Shared.Domain/Shared.Domain.csproj /nesprex-shared/Shared.Domain/
COPY nesprex-shared/Shared.Extensions/Shared.Extensions.csproj /nesprex-shared/Shared.Extensions/
COPY nesprex-api/Infra.MongoDb/Infra.MongoDb.csproj /Infra.MongoDb/
COPY nesprex-api/Api/Api.csproj /Api/

RUN dotnet restore /Api/Api.csproj

RUN dotnet build nesprex-api/Api/Api.csproj -c Release -o /app/build

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0    
WORKDIR /app
EXPOSE 80
COPY --from=build-env /app/build .
ENTRYPOINT ["dotnet", "Api.dll"]
