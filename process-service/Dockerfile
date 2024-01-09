FROM ghcr.io/rahim373/utube/aspnet-ffmpeg:v8 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

COPY ["ProcessService.Worker/ProcessService.Worker.csproj", "ProcessService.Worker/"]

RUN --mount=type=secret,id=github_token \
    dotnet nuget add source --username rahim373 --password $(cat /run/secrets/github_token) --store-password-in-clear-text --name github "https://nuget.pkg.github.com/rahim373/index.json"

RUN dotnet restore "ProcessService.Worker/ProcessService.Worker.csproj"
COPY . .
WORKDIR "/src/ProcessService.Worker"
RUN dotnet build "ProcessService.Worker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ProcessService.Worker.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ProcessService.Worker.dll"]