using System.Globalization;
using System.Text;

namespace Proyecto3.Api;

// Reemplazo de la librería unidecode de Python: quita las tildes
public static class Unidecode
{
    public static string Convertir(string texto)
    {
        // FormD separa cada letra de su tilde ("é" -> "e" + "´")
        string separado = texto.Normalize(NormalizationForm.FormD);
        var resultado = new StringBuilder();

        foreach (char c in separado)
        {
            // Copia todo menos las tildes
            if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                resultado.Append(c);
        }
        // Vuelve a unir los caracteres a su forma normal
        return resultado.ToString().Normalize(NormalizationForm.FormC);
    }
}
