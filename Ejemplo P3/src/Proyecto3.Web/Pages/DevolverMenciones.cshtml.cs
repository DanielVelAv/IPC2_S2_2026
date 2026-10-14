using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Proyecto3.Web.Pages;

public class DevolverMencionesModel : PageModel
{
    private readonly HttpClient http;

    public DevolverMencionesModel(IHttpClientFactory fabrica)
    {
        http = fabrica.CreateClient("Api");
    }

    public Dictionary<string, List<string>>? Fechas { get; set; }

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

        var respuesta = await http.PostAsync("devolverMenciones", new FormUrlEncodedContent(diccionarioAuxiliar));
        Fechas = await respuesta.Content.ReadFromJsonAsync<Dictionary<string, List<string>>>();
    }
}
