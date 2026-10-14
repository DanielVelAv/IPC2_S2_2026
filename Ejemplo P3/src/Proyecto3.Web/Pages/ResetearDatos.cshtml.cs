using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Proyecto3.Web.Pages;

public class ResetearDatosModel : PageModel
{
    private readonly HttpClient http;

    public ResetearDatosModel(IHttpClientFactory fabrica)
    {
        http = fabrica.CreateClient("Api");
    }

    public string? Mensaje { get; set; }

    public void OnGet()
    {
    }

    public async Task OnPostAsync()
    {
        // POST sin datos (null)
        var respuesta = await http.PostAsync("LimpiarDatos", null);
        var json = await respuesta.Content.ReadFromJsonAsync<Dictionary<string, string>>();
        Mensaje = json!["message"];
    }
}
