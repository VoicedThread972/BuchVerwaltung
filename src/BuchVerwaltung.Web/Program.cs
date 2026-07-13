using BuchVerwaltung.Data;

var builder = WebApplication.CreateBuilder(args);

// MVC-Controller mit Views registrieren.
builder.Services.AddControllersWithViews();

// Den Verbindungsstring aus der Konfiguration (appsettings.json) lesen und
// die Datenzugriffsschicht per Dependency Injection registrieren.
var connectionString = builder.Configuration.GetConnectionString("BuchDb")
    ?? throw new InvalidOperationException(
        "Der Verbindungsstring 'BuchDb' wurde in der Konfiguration nicht gefunden.");
builder.Services.AddBuchVerwaltungData(connectionString);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Buecher}/{action=Index}/{id?}");

app.Run();
