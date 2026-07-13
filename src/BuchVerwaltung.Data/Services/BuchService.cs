using BuchVerwaltung.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace BuchVerwaltung.Data.Services;

/// <summary>
/// Standardimplementierung von <see cref="IBuchService"/> auf Basis von
/// Entity Framework Core.
/// </summary>
/// <remarks>
/// Für jede Operation wird über die <see cref="IDbContextFactory{TContext}"/>
/// ein eigener, kurzlebiger <see cref="BuchDbContext"/> erzeugt. Ein
/// <see cref="DbContext"/> ist nicht threadsicher; durch je einen eigenen
/// Kontext pro Aufgabe können die beiden Bestände gefahrlos parallel geladen
/// werden.
/// </remarks>
public sealed class BuchService : IBuchService
{
    private readonly IDbContextFactory<BuchDbContext> _contextFactory;

    /// <summary>Erzeugt den Dienst mit der per DI bereitgestellten Kontext-Factory.</summary>
    public BuchService(IDbContextFactory<BuchDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<AktuellesBuch>> GetAktuelleBuecherAsync(CancellationToken cancellationToken = default)
    {
        await using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
        return await context.AktuelleBuecher
            .AsNoTracking()
            .OrderBy(buch => buch.Id)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<ArchiviertesBuch>> GetArchivierteBuecherAsync(CancellationToken cancellationToken = default)
    {
        await using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
        return await context.ArchivierteBuecher
            .AsNoTracking()
            .OrderBy(buch => buch.Id)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task VerschiebeInsArchivAsync(int id, CancellationToken cancellationToken = default)
    {
        await using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
        var buch = await context.AktuelleBuecher.FindAsync([id], cancellationToken);
        if (buch is null)
        {
            return;
        }

        // Das Verschieben (Löschen aus der Quelle + Anlegen im Ziel) erfolgt
        // innerhalb eines einzigen SaveChanges und damit als eine Transaktion.
        context.AktuelleBuecher.Remove(buch);
        context.ArchivierteBuecher.Add(new ArchiviertesBuch
        {
            Titel = buch.Titel,
            Autor = buch.Autor
        });
        await context.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task VerschiebeInsAktuellAsync(int id, CancellationToken cancellationToken = default)
    {
        await using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
        var buch = await context.ArchivierteBuecher.FindAsync([id], cancellationToken);
        if (buch is null)
        {
            return;
        }

        context.ArchivierteBuecher.Remove(buch);
        context.AktuelleBuecher.Add(new AktuellesBuch
        {
            Titel = buch.Titel,
            Autor = buch.Autor
        });
        await context.SaveChangesAsync(cancellationToken);
    }
}
