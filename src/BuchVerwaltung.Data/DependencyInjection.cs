using BuchVerwaltung.Data.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BuchVerwaltung.Data;

/// <summary>
/// Registriert die Datenzugriffsschicht der Buchverwaltung im
/// Dependency-Injection-Container.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Fügt den EF-Core-Kontext (als Factory für paralleles Laden) sowie den
    /// <see cref="IBuchService"/> zum DI-Container hinzu.
    /// </summary>
    /// <param name="services">Die Dienstsammlung.</param>
    /// <param name="connectionString">Der PostgreSQL-Verbindungsstring.</param>
    /// <returns>Die Dienstsammlung, um Aufrufe verketten zu können.</returns>
    public static IServiceCollection AddBuchVerwaltungData(this IServiceCollection services, string connectionString)
    {
        // Eine DbContext-Factory statt eines einzelnen Kontextes ermöglicht das
        // parallele Laden, da pro Aufgabe ein eigener Kontext erzeugt wird.
        services.AddDbContextFactory<BuchDbContext>(options =>
            options.UseNpgsql(connectionString, npgsql =>
                // Transiente Verbindungsfehler (z. B. während des Container-Starts
                // der Datenbank) werden automatisch mehrfach wiederholt.
                npgsql.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(5),
                    errorCodesToAdd: null)));

        services.AddScoped<IBuchService, BuchService>();

        return services;
    }
}
