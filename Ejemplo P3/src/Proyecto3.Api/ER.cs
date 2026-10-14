// XDocument / XElement: lectura de XML (equivale a xml.etree.ElementTree)
using System.Xml.Linq;

namespace Proyecto3.Api;

// Reemplaza la lista de Python [fecha, hashtags, menciones, positivas, negativas]
public class DatosMensaje
{
    public string Fecha { get; set; } = "";
    public List<string> Hashtags { get; set; } = new();
    public List<string> Menciones { get; set; } = new();
    public int Positivas { get; set; }
    public int Negativas { get; set; }
}

public class ER
{
    private readonly XElement? raiz;
    private readonly XElement raizDiccionario;

    private readonly Dictionary<int, DatosMensaje> Twitter = new();
    private readonly Dictionary<string, string> Diccionario = new();

    public ER(string carpetaDatos)
    {
        // Lee el archivo completo como texto
        var textoBd = File.ReadAllText(Path.Combine(carpetaDatos, "db.xml"));
        // Si db.xml está vacío no hay mensajes (null); si no, lo convierte a XML
        raiz = textoBd.Trim() == "" ? null : XDocument.Parse(textoBd).Root;

        // XDocument.Load(...).Root = ET.parse(...).getroot()
        raizDiccionario = XDocument.Load(Path.Combine(carpetaDatos, "dbDiccionario.xml")).Root!;
    }

    public Dictionary<int, DatosMensaje> GetDatos()
    {
        // Elements("x") = findall('x')
        foreach (var h in raizDiccionario.Elements("sentimientos_negativos"))
        {
            foreach (var g in h.Elements("palabra"))
            {
                string SinE = g.Value.Trim();
                string Sinupper = SinE.ToLower();
                Diccionario[Sinupper] = "Negativo";
            }
        }

        foreach (var o in raizDiccionario.Elements("sentimientos_positivos"))
        {
            foreach (var y in o.Elements("palabra"))
            {
                string sinEs = y.Value.Trim();
                string SinUpp = sinEs.ToLower();
                 if (Diccionario.ContainsKey(SinUpp) && Diccionario[SinUpp] == "Negativo")
                {
                    Diccionario.Remove(SinUpp);
                }
                else
                {
                    Diccionario[SinUpp] = "Positivo";
                }
            }
        }

        if (raiz == null)
            return Twitter;

        int m = 0;
        foreach (var i in raiz.Elements("MENSAJE"))
        {
            m += 1;
            string fechasCompletas = "";
            string textoBD = "";

            foreach (var j in i.Elements("FECHA"))
                fechasCompletas = j.Value;
            foreach (var k in i.Elements("TEXTO"))
                textoBD = k.Value;

            var mensaje = AnalizarMensaje(textoBD);
            mensaje.Fecha = ObtenerFecha(fechasCompletas);
            Twitter[m] = mensaje;
        }

        return Twitter;
    }

    public DatosMensaje AnalizarMensaje(string txtCompleto)
    {
        var resultado = new DatosMensaje();

        int j = 0;
        while (j < txtCompleto.Length)
        {
            char caracterAnalizado = txtCompleto[j];

            if (caracterAnalizado == '#')
            {
                int PosFinal = txtCompleto.IndexOf('#', j + 1);
                // Evita un ciclo infinito si el hashtag no tiene # de cierre
                if (PosFinal == -1)
                    break;

                // Substring(inicio, cantidad) = txtCompleto[j+1:PosFinal]
                string texto = txtCompleto.Substring(j + 1, PosFinal - (j + 1));
                j = PosFinal + 1;
                resultado.Hashtags.Add("#" + texto + "#");
            }
            else if (caracterAnalizado == '@')
            {
                j += 1;
                int carI = j;
                while (j < txtCompleto.Length &&
                       (char.IsLetter(txtCompleto[j]) || char.IsDigit(txtCompleto[j]) || txtCompleto[j] == '_'))
                {
                    j += 1;
                }
                string StringMencion = txtCompleto.Substring(carI, j - carI);
                resultado.Menciones.Add("@" + StringMencion);
            }
            else if (char.IsLetter(caracterAnalizado))
            {
                int k = j;
                while (k < txtCompleto.Length && char.IsLetter(txtCompleto[k]))
                    k += 1;

                string palabra = txtCompleto.Substring(j, k - j);
                string StringSinM = palabra.ToLower();

                if (Diccionario.ContainsKey(StringSinM))
                {
                    if (Diccionario[StringSinM] == "Negativo")
                        resultado.Negativas += 1;
                    else if (Diccionario[StringSinM] == "Positivo")
                        resultado.Positivas += 1;
                }
                j = k;
            }
            else
            {
                j += 1;
            }
        }

        return resultado;
    }

    public string ObtenerFecha(string fecha)
    {
        string fcompleta = fecha;
        string fechaApartada = "";
        int i = 0;
        while (i < fcompleta.Length)
        {
            // Evita leer fuera del texto si termina en un número
            if (i + 9 < fcompleta.Length &&
                char.IsDigit(fcompleta[i]) && char.IsDigit(fcompleta[i + 1]) && fcompleta[i + 2] == '/' &&
                char.IsDigit(fcompleta[i + 3]) && char.IsDigit(fcompleta[i + 4]) && fcompleta[i + 5] == '/' &&
                char.IsDigit(fcompleta[i + 6]) && char.IsDigit(fcompleta[i + 7]) &&
                char.IsDigit(fcompleta[i + 8]) && char.IsDigit(fcompleta[i + 9]))
            {
                fechaApartada = fcompleta.Substring(i, 10);
                i += 10;
            }
            else
            {
                i += 1;
            }
        }
        return fechaApartada;
    }
}
