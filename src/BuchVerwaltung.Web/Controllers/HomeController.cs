using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BuchVerwaltung.Web.Models;

namespace BuchVerwaltung.Web.Controllers;

/// <summary>
/// Stellt die Fehlerseite bereit, auf die die globale Ausnahmebehandlung
/// verweist.
/// </summary>
public sealed class HomeController : Controller
{
    /// <summary>Zeigt die allgemeine Fehlerseite an.</summary>
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
