FROM ghcr.io/rahim373/utube/aspnet-ffmpeg:v8 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM ghcr.io/rahim373/utube/aspnet-ffmpeg:v8 AS build
WORKDIR /src
COPY ["ProcessService.Worker/ProcessService.Worker.csproj", "ProcessService.Worker/"]
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