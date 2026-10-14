// Prepara la aplicación (lee appsettings.json y launchSettings.json)
var builder = WebApplication.CreateBuilder(args);

// Activa Razor Pages (busca las páginas en la carpeta /Pages)
builder.Services.AddRazorPages();

// Registra un HttpClient llamado "Api" que ya apunta a la dirección del backend (appsettings.json)
builder.Services.AddHttpClient("Api", cliente =>
{
    cliente.BaseAddress = new Uri(builder.Configuration["UrlApi"]!);
});

var app = builder.Build();

app.UseStaticFiles();   // Sirve la carpeta wwwroot (equivale a static/ de Django)
app.UseRouting();       // Decide qué página atiende cada URL
app.MapRazorPages();    // Crea una ruta por cada .cshtml que tenga @page

// Arranca el sitio y se queda escuchando peticiones
app.Run();
