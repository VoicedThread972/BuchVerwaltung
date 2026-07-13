using BuchVerwaltung.Data.Services;
using BuchVerwaltung.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace BuchVerwaltung.Web.Controllers;

/// <summary>
/// Steuert die Anzeige der beiden Buchbestände (aktuell und archiviert) sowie
/// das Verschieben einzelner Bücher zwischen den Beständen.
/// </summary>
public sealed class BuecherController : Controller
{
    private readonly IBuchService _buchService;

    /// <summary>Der Buch-Dienst wird per Dependency Injection bereitgestellt.</summary>
    public BuecherController(IBuchService buchService)
    {
        _buchService = buchService;
    }

    /// <summary>
    /// Lädt beide Bestände parallel (in je einem eigenen Task) und stellt sie
    /// nebeneinander dar.
    /// </summary>
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        // Beide Tabellen werden parallel zum aufrufenden Thread geladen.
        var aktuelleTask = _buchService.GetAktuelleBuecherAsync(cancellationToken);
        var archivierteTask = _buchService.GetArchivierteBuecherAsync(cancellationToken);
        await Task.WhenAll(aktuelleTask, archivierteTask);

        var viewModel = new BuecherViewModel
        {
            AktuelleBuecher = await aktuelleTask,
            ArchivierteBuecher = await archivierteTask
        };

        return View(viewModel);
    }

    /// <summary>
    /// Verschiebt ein Buch aus dem aktuellen Bestand ins Archiv und lädt danach
    /// die Startseite (mit erneut parallel geladenen Beständen) erneut.
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> InsArchiv(int id, CancellationToken cancellationToken)
    {
        await _buchService.VerschiebeInsArchivAsync(id, cancellationToken);
        return RedirectToAction(nameof(Index));
    }

    /// <summary>
    /// Verschiebt ein archiviertes Buch zurück in den aktuellen Bestand und lädt
    /// danach die Startseite erneut.
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> InsAktuell(int id, CancellationToken cancellationToken)
    {
        await _buchService.VerschiebeInsAktuellAsync(id, cancellationToken);
        return RedirectToAction(nameof(Index));
    }
}
