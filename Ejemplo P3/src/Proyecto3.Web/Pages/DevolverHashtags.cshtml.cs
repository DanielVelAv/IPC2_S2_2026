using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Proyecto3.Web.Pages;

public class DevolverHashtagsModel : PageModel // tiene propiedades como Request, ViewData y OnGet y onPost 
{
    private readonly HttpClient http; // solo se asigna en constructor

    public DevolverHashtagsModel(IHttpClientFactory fabrica)
    {
        // el declarado en Program.cs
        http = fabrica.CreateClient("Api"); //pide cliente llamado api
    }

    // null = todavía no se consultó (el HTML muestra el formulario)

    //puente a .cshtml; misma forma que JSON manda a api
    public Dictionary<string, List<string>>? Fechas { get; set; }

    public void OnGet()
    {
    }

    // Los parámetros se llenan con los <input> que tengan el mismo name
    public async Task OnPostAsync(string PrimerRango, string SegundoRango)
    {
        var diccionarioAuxiliar = new Dictionary<string, string>
        {
            ["PrimerRango"] = PrimerRango,
            ["SegundoRango"] = SegundoRango
        };

        var respuesta = await http.PostAsync("devolverHastags", new FormUrlEncodedContent(diccionarioAuxiliar));
        // Convierte el JSON {"fecha": ["#a#", ...]} al tipo indicado entre < >
        Fechas = await respuesta.Content.ReadFromJsonAsync<Dictionary<string, List<string>>>(); //<- tipo de dato de la respuesta json 
    }
}
