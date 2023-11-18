FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Handler/Handler.csproj", "Handler/"]
RUN dotnet restore "Handler/Handler.csproj"
COPY . .
WORKDIR "/src/Handler"
RUN dotnet build "Handler.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Handler.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Handler.dll"]
