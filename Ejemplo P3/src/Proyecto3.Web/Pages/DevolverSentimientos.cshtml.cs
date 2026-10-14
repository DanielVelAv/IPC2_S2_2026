using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Proyecto3.Web.Pages;

// Misma forma que el JSON de la API: {"positivos": {...}, "negativos": {...}, "neutros": {...}}
public class RespuestaSentimientos
{
    public Dictionary<string, int> Positivos { get; set; } = new();
    public Dictionary<string, int> Negativos { get; set; } = new();
    public Dictionary<string, int> Neutros { get; set; } = new();

}

public class DevolverSentimientosModel : PageModel
{
    private readonly HttpClient http;

    public DevolverSentimientosModel(IHttpClientFactory fabrica)
    {
        http = fabrica.CreateClient("Api");
    }

    public RespuestaSentimientos? SentP { get; set; }

    public void OnGet()
    {
    }

    public async Task OnPostAsync(string PrimerRango, string SegundoRango)
    {
        var diccionarioAuxiliar = new Dictionary<string, string>
        {
            ["PrimerRango"] = PrimerRango,
            ["SegundoRango"] = SegundoRango
        };

        var respuesta = await http.PostAsync("devolverSentimientos", new FormUrlEncodedContent(diccionarioAuxiliar));
        SentP = await respuesta.Content.ReadFromJsonAsync<RespuestaSentimientos>();
    }
}
