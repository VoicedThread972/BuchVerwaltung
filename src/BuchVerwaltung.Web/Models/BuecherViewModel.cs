using BuchVerwaltung.Data.Entities;

namespace BuchVerwaltung.Web.Models;

/// <summary>
/// View-Model für die Startseite. Bündelt beide Buchbestände, damit sie
/// nebeneinander dargestellt werden können.
/// </summary>
public sealed class BuecherViewModel
{
    /// <summary>Bücher im aktuellen Bestand.</summary>
    public IReadOnlyList<AktuellesBuch> AktuelleBuecher { get; init; } = Array.Empty<AktuellesBuch>();

    /// <summary>Archivierte Bücher.</summary>
    public IReadOnlyList<ArchiviertesBuch> ArchivierteBuecher { get; init; } = Array.Empty<ArchiviertesBuch>();
}
