namespace BuchVerwaltung.Data.Entities;

/// <summary>
/// Abstrakte Basisklasse für ein Buch. Sie kapselt die gemeinsamen
/// Eigenschaften aller Buch-Arten (aktueller Bestand bzw. Archiv) und
/// demonstriert die Wiederverwendung von Code per Vererbung.
/// </summary>
public abstract class Buch
{
    /// <summary>Eindeutiger Primärschlüssel (wird von der Datenbank vergeben).</summary>
    public int Id { get; set; }

    /// <summary>Der Titel des Buches.</summary>
    public string Titel { get; set; } = string.Empty;

    /// <summary>Der Autor des Buches.</summary>
    public string Autor { get; set; } = string.Empty;
}
