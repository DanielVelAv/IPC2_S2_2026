using Microsoft.AspNetCore.Mvc;
using Proyecto3.Api;

// Prepara la aplicación (lee appsettings.json y launchSettings.json)
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// Carpeta de los archivos XML, configurada en appsettings.json ("CarpetaDatos")
string carpetaDatos = Path.Combine(app.Environment.ContentRootPath, app.Configuration["CarpetaDatos"] ?? "App_Data");
// Crea la carpeta si no existe
Directory.CreateDirectory(carpetaDatos);

// MapGet/MapPost = @app.route(..., methods=['GET'/'POST']) de Flask
app.MapGet("/", () =>
{
    // Results.Json = jsonify
    return Results.Json(new { message = "Funcionando Jefe" });
});

// [FromForm] = request.form['archivoMsg']
app.MapPost("/entradaMensaje", ([FromForm] string archivoMsg) =>
{
    var bd = new Bd(carpetaDatos);
    string unido = bd.Anadir(archivoMsg);

    string rutaTemporal = Path.Combine(carpetaDatos, "Temporal.xml");
    File.WriteAllText(rutaTemporal, unido);
    // Results.File = send_file
    return Results.File(rutaTemporal, "application/xml");
})
// Quita el token de seguridad de formularios (la API no la llama un navegador)
.DisableAntiforgery();

app.MapPost("/CargaArchivosConfiguracion", ([FromForm] string archivoDicc) =>
{
    var bd = new Bd(carpetaDatos);
    bd.AnadirDiccionario(archivoDicc);
    return Results.Json(new { message = "El archivo se recibio exitosamente" });
})
.DisableAntiforgery();

app.MapPost("/LimpiarDatos", () =>
{
    var gg = new Bd(carpetaDatos);
    gg.Vaciar();
    return Results.Json(new { message = "Se han limpiado los datos correctamente" });
});

app.MapPost("/devolverHastags", ([FromForm] string PrimerRango, [FromForm] string SegundoRango) =>
{
    var mnE = new ER(carpetaDatos);
    var DiccionarioConDatos = mnE.GetDatos();
    var hash = new Hastags(DiccionarioConDatos, PrimerRango, SegundoRango);
    var Filtrado = hash.Filtar();
    return Results.Json(Filtrado);
})
.DisableAntiforgery();

app.MapPost("/devolverMenciones", ([FromForm] string PrimerRango, [FromForm] string SegundoRango) =>
{
    var mnE = new ER(carpetaDatos);
    var DiccionarioConDatos = mnE.GetDatos();
    var menciones = new Menciones(DiccionarioConDatos, PrimerRango, SegundoRango);
    var MenciFilt = menciones.FiltrarMenciones();
    return Results.Json(MenciFilt);
})
.DisableAntiforgery();

app.MapPost("/devolverSentimientos", ([FromForm] string PrimerRango, [FromForm] string SegundoRango) =>
{
    var mnE = new ER(carpetaDatos);
    var DiccionarioConDatos = mnE.GetDatos();
    var sentimientos = new Sentimientos(DiccionarioConDatos, PrimerRango, SegundoRango);
    // Separa la tupla que devuelve el método en tres variables
    var (SentiFiltPosi, SentiFiltNega, SentiFiltNeutro) = sentimientos.FiltrarSentimientos();

    // Los tres diccionarios se envían juntos en un solo JSON
    return Results.Json(new { positivos = SentiFiltPosi, negativos = SentiFiltNega , neutros = SentiFiltNeutro });
})
.DisableAntiforgery();

// Arranca la API y se queda escuchando peticiones
app.Run();
