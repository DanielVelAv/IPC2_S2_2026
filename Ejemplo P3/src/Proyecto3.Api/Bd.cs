// XDocument / XElement: lectura y escritura de XML
using System.Xml.Linq;

namespace Proyecto3.Api;

public class Bd
{
    private readonly string bdPath;
    private readonly string dbDiccionarioPath;

    public Bd(string carpetaDatos)
    {
        // Path.Combine une carpeta y nombre de archivo con el separador correcto
        bdPath = Path.Combine(carpetaDatos, "db.xml");
        dbDiccionarioPath = Path.Combine(carpetaDatos, "dbDiccionario.xml");

        // Crea los archivos la primera vez que se ejecuta
        if (!File.Exists(bdPath) || !File.Exists(dbDiccionarioPath))
            Vaciar();
    }

    public void Vaciar()
    {
        // File.WriteAllText = open(ruta, 'w').write(...)
        File.WriteAllText(bdPath, "");

        string concatenadoDicc = "<diccionario>\n";
        concatenadoDicc += "<sentimientos_positivos>\n\n";
        concatenadoDicc += "</sentimientos_positivos>\n\n";
        concatenadoDicc += "<sentimientos_negativos>\n\n";
        concatenadoDicc += "</sentimientos_negativos>\n\n";
        concatenadoDicc += "<positivas_rechazadas>\n\n";
        concatenadoDicc += "</positivas_rechazadas>\n\n";
        concatenadoDicc += "<negativas_rechazadas>\n\n";
        concatenadoDicc += "</negativas_rechazadas>\n\n";
        concatenadoDicc += "</diccionario>";

        File.WriteAllText(dbDiccionarioPath, concatenadoDicc);
    }

    public string Anadir(string archivoXMl)
    {
        string sinP = QuitarEtiquetasMensajes(archivoXMl);

        // File.ReadAllText = open(ruta).read()
        string infoAlmacenada = File.ReadAllText(bdPath);
        string inforSinE = QuitarEtiquetasMensajes(infoAlmacenada);

        string xml = "<?xml version=\"1.0\"?>";
        string msjA = "\n<MENSAJES>\n";
        string msjC = "\n</MENSAJES>";

        File.WriteAllText(bdPath, xml + msjA + Unidecode.Convertir(inforSinE) + Unidecode.Convertir(sinP) + msjC);

        // XDocument.Parse(...).Root = ET.fromstring(...)
        var txtParseado = XDocument.Parse(archivoXMl).Root!;
        int m = 0;
        string mensaje = "";
        string elementoI = "<?xml version=\"1\"?>\n";
        string elementoI2 = "<MENSAJES_RECIBIDOS>\n";
        string elementoF = "</MENSAJES_RECIBIDOS>";

        foreach (var g in txtParseado.Elements("MENSAJE"))
        {
            m += 1;
            string fechaCompleta = "";
            string textoM = "";

            foreach (var k in g.Elements("FECHA"))
                fechaCompleta = k.Value;
            foreach (var f in g.Elements("TEXTO"))
                textoM = f.Value;

            var (Hashtag, Menciones) = AnalizarMensaje(textoM);
            string fechaSolitaria = ObtenerFecha(fechaCompleta);

            mensaje = "<TIEMPO>\n";
            mensaje += $"<FECHA> {fechaSolitaria} </FECHA>\n";
            mensaje += $"<MSJ_RECIBIDOS> {m} <MSJ_RECIBIDOS>\n";
            mensaje += $"<USR_MENCIONADOS> {Menciones.Count} </USR_MENCIONADOS>\n";
            mensaje += $"<HASH_INCLUIDOS> {Hashtag.Count} </HASH_INCLUIDOS>\n";
            mensaje += "</TIEMPO>\n";
        }

        string unido = elementoI + elementoI2 + mensaje + elementoF;
        return unido;
    }

    // Reemplaza el recorte [33:-11] de Python: devuelve lo que hay entre <MENSAJES> y </MENSAJES>
    private static string QuitarEtiquetasMensajes(string texto)
    {
        int inicio = texto.IndexOf("<MENSAJES>");
        int fin = texto.LastIndexOf("</MENSAJES>");
        if (inicio == -1 || fin == -1)
            return "";

        inicio += "<MENSAJES>".Length;
        return texto.Substring(inicio, fin - inicio);
    }

    // Devuelve una tupla: dos valores a la vez (como return a, b en Python)
    public (List<string>, List<string>) AnalizarMensaje(string textoCompleto)
    {
        var Hastag = new List<string>();
        var mencion = new List<string>();

        int j = 0;
        while (j < textoCompleto.Length)
        {
            char caracterAnalizado = textoCompleto[j];

            if (caracterAnalizado == '#')
            {
                int PosFinal = textoCompleto.IndexOf('#', j + 1);
                // Evita un ciclo infinito si el hashtag no tiene # de cierre
                if (PosFinal == -1)
                    break;
                string texto = textoCompleto.Substring(j + 1, PosFinal - (j + 1));
                j = PosFinal + 1;
                Hastag.Add("#" + texto + "#");
            }
            else if (caracterAnalizado == '@')
            {
                j += 1;
                int carI = j;
                while (j < textoCompleto.Length &&
                       (char.IsLetter(textoCompleto[j]) || char.IsDigit(textoCompleto[j]) || textoCompleto[j] == '_'))
                {
                    j += 1;
                }
                string stringMencion = textoCompleto.Substring(carI, j - carI);
                mencion.Add("@" + stringMencion);
            }
            else
            {
                j += 1;
            }
        }
        return (Hastag, mencion);
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

    public void AnadirDiccionario(string archivoDiccionario)
    {
        var baseDatosDiccionarios = XDocument.Load(dbDiccionarioPath);
        var infoIntento = XDocument.Parse(archivoDiccionario).Root!;

        foreach (var i in infoIntento.Elements("sentimientos_positivos"))
        {
            foreach (var j in i.Elements("palabra"))
            {
                string palabraPositiva = j.Value;
                string palabraPSinT = Unidecode.Convertir(palabraPositiva);
                var palabra = new XElement("palabra", palabraPSinT);

                bool VF = VerificarExistePositivos(palabraPSinT);
                bool VFEP = VerificarExisteNegativos(palabraPSinT);
                bool VFER = VerificarExisteRechazadas(palabraPSinT);
                bool VFEOL = VerificarExisteArchivoN(palabraPSinT, infoIntento);

                if (VF)
                {
                }
                else if (VF == false && VFEP == false && VFER == false && VFEOL == false)
                {
                    // Add = append; Save = write (guarda el XML en el archivo)
                    baseDatosDiccionarios.Root!.Element("sentimientos_positivos")!.Add(palabra);
                    baseDatosDiccionarios.Save(dbDiccionarioPath);
                }
                else
                {
                    baseDatosDiccionarios.Root!.Element("positivas_rechazadas")!.Add(palabra);
                    baseDatosDiccionarios.Save(dbDiccionarioPath);
                }
            }
        }

        foreach (var k in infoIntento.Elements("sentimientos_negativos"))
        {
            foreach (var l in k.Elements("palabra"))
            {
                string palabraNegativa = l.Value;
                string nP = Unidecode.Convertir(palabraNegativa);
                var palabra2 = new XElement("palabra", nP);

                bool VF = VerificarExisteNegativos(nP);
                bool VFEN = VerificarExistePositivos(nP);
                bool VFER = VerificarExisteRechazadas(nP);
                bool VFEOP = VerificarExisteArchivoP(nP, infoIntento);

                if (VF)
                {
                }
                else if (VF == false && VFEN == false && VFER == false && VFEOP == false)
                {
                    baseDatosDiccionarios.Root!.Element("sentimientos_negativos")!.Add(palabra2);
                    baseDatosDiccionarios.Save(dbDiccionarioPath);
                }
                else
                {
                    baseDatosDiccionarios.Root!.Element("negativas_rechazadas")!.Add(palabra2);
                    baseDatosDiccionarios.Save(dbDiccionarioPath);
                }
            }
        }
    }

    public bool VerificarExistePositivos(string palabra)
    {
        bool Estado = false;
        var bd = XDocument.Load(dbDiccionarioPath).Root!;
        foreach (var i in bd.Elements("sentimientos_positivos"))
        {
            foreach (var j in i.Elements("palabra"))
            {
                if (j.Value == palabra)
                    Estado = true;
            }
        }
        return Estado;
    }

    public bool VerificarExisteNegativos(string palabra2)
    {
        bool Estado = false;
        var bd = XDocument.Load(dbDiccionarioPath).Root!;
        foreach (var i in bd.Elements("sentimientos_negativos"))
        {
            foreach (var j in i.Elements("palabra"))
            {
                if (j.Value == palabra2)
                    Estado = true;
            }
        }
        return Estado;
    }

    public bool VerificarExisteRechazadas(string palabra3)
    {
        bool Estado = false;
        var bd = XDocument.Load(dbDiccionarioPath).Root!;
        foreach (var i in bd.Elements("negativas_rechazadas"))
        {
            foreach (var j in i.Elements("palabra"))
            {
                if (j.Value == palabra3)
                    Estado = true;
            }
        }
        return Estado;
    }

    public bool VerificarExisteArchivoN(string palabra4, XElement info)
    {
        bool Estate = false;
        foreach (var h in info.Elements("sentimientos_negativos"))
        {
            foreach (var t in h.Elements("palabra"))
            {
                if (t.Value == palabra4)
                    Estate = true;
            }
        }
        return Estate;
    }

    public bool VerificarExisteArchivoP(string palabra5, XElement info)
    {
        bool Estate = false;
        foreach (var h in info.Elements("sentimientos_positivos"))
        {
            foreach (var t in h.Elements("palabra"))
            {
                if (t.Value == palabra5)
                    Estate = true;
            }
        }
        return Estate;
    }
}
