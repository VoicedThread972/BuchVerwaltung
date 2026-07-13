# Buchverwaltung

Web-Anwendung (ASP.NET Core MVC, .NET 8) zur Verwaltung zweier Buchbestände
(**aktuelle Bücher** und **archivierte Bücher**). Beide Bestände werden
parallel aus einer PostgreSQL-Datenbank geladen und nebeneinander angezeigt.
Jedes Buch kann in den jeweils anderen Bestand verschoben werden; anschließend
werden beide Listen erneut parallel geladen.

## Projektstruktur

| Projekt                 | Beschreibung                                                            |
| ----------------------- | ---------------------------------------------------------------------- |
| `BuchVerwaltung.Data`   | Klassenbibliothek: Entitäten, EF-Core-`DbContext`, Dienste (Datenzugriff) |
| `BuchVerwaltung.Web`    | ASP.NET Core MVC: Controller, Views, View-Models, DI-Konfiguration      |

## Umgesetzte Anforderungen

- **Eigene Klassenbibliothek mit Entity Framework Core** – der komplette
  Datenbankzugriff liegt in `BuchVerwaltung.Data` (EF Core + Npgsql).
- **Dependency Injection** – durchgängig eingesetzt (Registrierung über
  `AddBuchVerwaltungData`, Konstruktor-Injektion in Dienst und Controller).
- **Architekturmuster (MVC)** – klare Trennung in Model, View und Controller.
- **Parallel geladene Daten** – beide Bestände werden in eigenen `Task`s über
  `Task.WhenAll` geladen. Dank `IDbContextFactory` erhält jede Aufgabe einen
  eigenen `DbContext` (thread-sicher).
- **Vererbung / Schnittstellen** – abstrakte Basisklasse `Buch`, abgeleitete
  Entitäten `AktuellesBuch` / `ArchiviertesBuch`, Dienst hinter `IBuchService`.
- **ConnectionString in Konfigurationsdatei** – siehe `appsettings.json`.
- **Git** – der `.git`-Ordner ist Teil der Abgabe.

## Einrichtung mit Docker (empfohlen)

Die komplette Anwendung (Datenbank **und** Web-App) läuft in Containern. Es wird
nur Docker (Docker Desktop) benötigt – keine lokale .NET- oder PostgreSQL-Installation.

```bash
docker compose up --build
```

Danach ist die Anwendung unter <http://localhost:8080> erreichbar. Beim ersten
Start legt der Datenbank-Container automatisch das Schema samt Beispieldaten aus
[BuchVerwaltungPostgres.sql](BuchVerwaltungPostgres.sql) an.

Beenden und Aufräumen:

```bash
docker compose down          # Container stoppen
docker compose down -v       # zusätzlich das Datenbank-Volume löschen
```

> **Hinweis:** Der Verbindungsstring für den Container wird in
> [docker-compose.yml](docker-compose.yml) per Umgebungsvariable
> `ConnectionStrings__BuchDb` gesetzt (Host = `db`).

## Alternative: lokal starten (nur Datenbank im Container)

Wer die App aus Visual Studio bzw. per `dotnet run` starten möchte, kann nur die
Datenbank containerisieren:

```bash
docker compose up -d db
dotnet run --project src/BuchVerwaltung.Web
```

Der Verbindungsstring dafür steht in
[src/BuchVerwaltung.Web/appsettings.json](src/BuchVerwaltung.Web/appsettings.json)
(Host = `localhost`) und kann bei Bedarf angepasst werden:

```json
"ConnectionStrings": {
  "BuchDb": "Host=localhost;Port=5432;Database=buchdb;Username=postgres;Password=postgres"
}
```
