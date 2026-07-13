# ---------------------------------------------------------------------------
# Multi-Stage-Dockerfile für die ASP.NET-Core-MVC-Anwendung.
# Der Build-Kontext ist das Repository-Wurzelverzeichnis (siehe docker-compose).
# ---------------------------------------------------------------------------

# 1) Build-Stage: stellt Abhängigkeiten wieder her und veröffentlicht die App.
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Zuerst nur die Projektdateien kopieren, damit "restore" gecacht werden kann.
COPY ["src/BuchVerwaltung.Web/BuchVerwaltung.Web.csproj", "src/BuchVerwaltung.Web/"]
COPY ["src/BuchVerwaltung.Data/BuchVerwaltung.Data.csproj", "src/BuchVerwaltung.Data/"]
RUN dotnet restore "src/BuchVerwaltung.Web/BuchVerwaltung.Web.csproj"

# Danach den restlichen Quellcode kopieren und im Release-Modus veröffentlichen.
COPY . .
RUN dotnet publish "src/BuchVerwaltung.Web/BuchVerwaltung.Web.csproj" \
    -c Release -o /app/publish /p:UseAppHost=false

# 2) Laufzeit-Stage: schlankes Runtime-Image ohne SDK.
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "BuchVerwaltung.Web.dll"]
