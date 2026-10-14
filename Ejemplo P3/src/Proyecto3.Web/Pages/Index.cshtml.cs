using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Proyecto3.Web.Pages;

// PageModel: la clase con la lógica de la página (como la función de view.py)
public class IndexModel : PageModel
{
    private readonly HttpClient http;

    // ASP.NET Core entrega la fábrica y se pide el cliente "Api" registrado en Program.cs
    public IndexModel(IHttpClientFactory fabrica)
    {
        http = fabrica.CreateClient("Api");
    }

    // Propiedad que lee el HTML con Model.Fireworks
    public string? Fireworks { get; set; }

    // Se ejecuta con GET (al abrir la página)
    public void OnGet()
    {
    }

    // Se ejecuta con POST (al enviar el formulario).
    // IFormFile = request.FILES; el parámetro se llama igual que el <input name>
    public async Task OnPostAsync(IFormFile archivoMsg)
    {
        // Lee el archivo subido como texto ("using" lo cierra al terminar)
        using var lector = new StreamReader(archivoMsg.OpenReadStream());
        string contenido = await lector.ReadToEndAsync();

        var diccionarioPrueba = new Dictionary<string, string>
        {
            ["archivoMsg"] = contenido
        };

        // POST a la API enviando el diccionario como formulario (= post(url, dicc))
        var respuesta = await http.PostAsync("entradaMensaje", new FormUrlEncodedContent(diccionarioPrueba));

        // Lee la respuesta como texto (= respuesta.content.decode())
        Fireworks = await respuesta.Content.ReadAsStringAsync();
    }
}
