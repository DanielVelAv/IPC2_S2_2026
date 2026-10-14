using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Proyecto3.Web.Pages;

public class PedirDiccionarioModel : PageModel
{
    private readonly HttpClient http;

    public PedirDiccionarioModel(IHttpClientFactory fabrica)
    {
        http = fabrica.CreateClient("Api");
    }

    public string? Mensaje { get; set; }

    public void OnGet()
    {
    }

    public async Task OnPostAsync(IFormFile archivoDicc)
    {
        using var lector = new StreamReader(archivoDicc.OpenReadStream());
        string contenido = await lector.ReadToEndAsync();

        var dicc = new Dictionary<string, string>
        {
            ["archivoDicc"] = contenido
        };

        var respuesta = await http.PostAsync("CargaArchivosConfiguracion", new FormUrlEncodedContent(dicc));

        // Convierte el JSON {"message": "..."} a un diccionario (= .json())
        var json = await respuesta.Content.ReadFromJsonAsync<Dictionary<string, string>>();
        Mensaje = json!["message"];
    }
}
