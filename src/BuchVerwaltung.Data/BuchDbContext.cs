using BuchVerwaltung.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace BuchVerwaltung.Data;

/// <summary>
/// Entity-Framework-Core-Kontext der Buchverwaltung. Er bildet die beiden
/// Tabellen <c>aktuelle_buecher</c> und <c>archivierte_buecher</c> auf jeweils
/// eine eigene Entität ab.
/// </summary>
public sealed class BuchDbContext : DbContext
{
    /// <summary>
    /// Erzeugt einen neuen Kontext. Die Optionen (u. a. der Verbindungsstring)
    /// werden per Dependency Injection übergeben.
    /// </summary>
    public BuchDbContext(DbContextOptions<BuchDbContext> options)
        : base(options)
    {
    }

    /// <summary>Bücher im aktuellen Bestand.</summary>
    public DbSet<AktuellesBuch> AktuelleBuecher => Set<AktuellesBuch>();

    /// <summary>Archivierte Bücher.</summary>
    public DbSet<ArchiviertesBuch> ArchivierteBuecher => Set<ArchiviertesBuch>();

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Beide Tabellen besitzen denselben Aufbau, daher wird die Abbildung
        // generisch definiert, um Codeverdopplung zu vermeiden.
        ConfigureBuch<AktuellesBuch>(modelBuilder, "aktuelle_buecher");
        ConfigureBuch<ArchiviertesBuch>(modelBuilder, "archivierte_buecher");
    }

    /// <summary>
    /// Konfiguriert die Abbildung einer Buch-Entität auf ihre Tabelle und
    /// Spalten. Da beide Tabellen identisch aufgebaut sind, kann diese Methode
    /// generisch für beide Buch-Arten verwendet werden.
    /// </summary>
    /// <typeparam name="TBuch">Die konkrete Buch-Art.</typeparam>
    /// <param name="modelBuilder">Der Model-Builder von EF Core.</param>
    /// <param name="tabelle">Der Name der Datenbanktabelle.</param>
    private static void ConfigureBuch<TBuch>(ModelBuilder modelBuilder, string tabelle)
        where TBuch : Buch
    {
        modelBuilder.Entity<TBuch>(entity =>
        {
            entity.ToTable(tabelle);
            entity.HasKey(b => b.Id);
            entity.Property(b => b.Id).HasColumnName("id");
            entity.Property(b => b.Titel).HasColumnName("titel").HasMaxLength(100).IsRequired();
            entity.Property(b => b.Autor).HasColumnName("autor").HasMaxLength(100).IsRequired();
        });
    }
}
