using BuchVerwaltung.Data.Entities;

namespace BuchVerwaltung.Data.Services;

/// <summary>
/// Abstraktion für den Zugriff auf die beiden Buchbestände. Ermöglicht das
/// parallele Laden der Bestände sowie das Verschieben einzelner Bücher
/// zwischen aktuellem Bestand und Archiv.
/// </summary>
public interface IBuchService
{
    /// <summary>Lädt alle Bücher aus dem aktuellen Bestand.</summary>
    /// <param name="cancellationToken">Token zum Abbrechen der Operation.</param>
    Task<IReadOnlyList<AktuellesBuch>> GetAktuelleBuecherAsync(CancellationToken cancellationToken = default);

    /// <summary>Lädt alle archivierten Bücher.</summary>
    /// <param name="cancellationToken">Token zum Abbrechen der Operation.</param>
    Task<IReadOnlyList<ArchiviertesBuch>> GetArchivierteBuecherAsync(CancellationToken cancellationToken = default);

    /// <summary>Verschiebt ein Buch aus dem aktuellen Bestand ins Archiv.</summary>
    /// <param name="id">Der Primärschlüssel des zu verschiebenden Buches.</param>
    /// <param name="cancellationToken">Token zum Abbrechen der Operation.</param>
    Task VerschiebeInsArchivAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>Verschiebt ein archiviertes Buch zurück in den aktuellen Bestand.</summary>
    /// <param name="id">Der Primärschlüssel des zu verschiebenden Buches.</param>
    /// <param name="cancellationToken">Token zum Abbrechen der Operation.</param>
    Task VerschiebeInsAktuellAsync(int id, CancellationToken cancellationToken = default);
}
