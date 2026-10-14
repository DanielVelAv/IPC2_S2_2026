// CultureInfo: para leer fechas sin depender del idioma de la computadora
using System.Globalization;

namespace Proyecto3.Api;

public class Hastags
{
    private readonly Dictionary<int, DatosMensaje> diccionario;
    private readonly string PrimerRango;
    private readonly string SegundoRango;

    public Hastags(Dictionary<int, DatosMensaje> diccionario, string PrimerRango, string SegundoRango)
    {
        this.diccionario = diccionario;
        this.PrimerRango = PrimerRango;
        this.SegundoRango = SegundoRango;
    }

    public Dictionary<string, List<string>> Filtar()
    {
        // ParseExact = time.strptime (convierte texto a fecha con un formato fijo)
        DateTime primerR = DateTime.ParseExact(PrimerRango, "yyyy-MM-dd", CultureInfo.InvariantCulture);
        DateTime segundoR = DateTime.ParseExact(SegundoRango, "yyyy-MM-dd", CultureInfo.InvariantCulture);

        var DiccionarioF = new Dictionary<string, List<string>>();

        foreach (int dicc in diccionario.Keys)
        {
            string fechaEnDicc = diccionario[dicc].Fecha;
            DateTime fechaEnDiccTD = DateTime.ParseExact(fechaEnDicc, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            bool VerificacionRango = EstaEntre(primerR, segundoR, fechaEnDiccTD);
            if (VerificacionRango == true)
            {
                DiccionarioF[fechaEnDicc] = diccionario[dicc].Hashtags;
            }
        }
        return DiccionarioF;
    }

    public bool EstaEntre(DateTime RangoInicial, DateTime RangoFinal, DateTime Fecha)
    {
        bool Desicion = false;
        if (Fecha >= RangoInicial && Fecha <= RangoFinal)
            Desicion = true;
        return Desicion;
    }
}

public class Menciones
{
    private readonly Dictionary<int, DatosMensaje> diccionario;
    private readonly string PrimerRango;
    private readonly string SegundoRango;

    public Menciones(Dictionary<int, DatosMensaje> diccionario, string PrimerRango, string SegundoRango)
    {
        this.diccionario = diccionario;
        this.PrimerRango = PrimerRango;
        this.SegundoRango = SegundoRango;
    }

    public Dictionary<string, List<string>> FiltrarMenciones()
    {
        DateTime primerR = DateTime.ParseExact(PrimerRango, "yyyy-MM-dd", CultureInfo.InvariantCulture);
        DateTime segundoR = DateTime.ParseExact(SegundoRango, "yyyy-MM-dd", CultureInfo.InvariantCulture);
        var DiccionarioM = new Dictionary<string, List<string>>();

        foreach (int dicc in diccionario.Keys)
        {
            string fechaEnDicc = diccionario[dicc].Fecha;
            DateTime fechaEnDiccTD = DateTime.ParseExact(fechaEnDicc, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            bool VerificacionRango = EstaEntre(primerR, segundoR, fechaEnDiccTD);
            if (VerificacionRango == true)
            {
                if (diccionario[dicc].Menciones.Count != 0)
                    DiccionarioM[fechaEnDicc] = diccionario[dicc].Menciones;
            }
        }
        return DiccionarioM;
    }

    public bool EstaEntre(DateTime RangoInicial, DateTime RangoFinal, DateTime Fecha)
    {
        bool Desicion = false;
        if (Fecha >= RangoInicial && Fecha <= RangoFinal)
            Desicion = true;
        return Desicion;
    }
}

public class Sentimientos
{
    private readonly Dictionary<int, DatosMensaje> diccionario;
    private readonly string PrimerRango;
    private readonly string SegundoRango;

    public Sentimientos(Dictionary<int, DatosMensaje> diccionario, string PrimerRango, string SegundoRango)
    {
        this.diccionario = diccionario;
        this.PrimerRango = PrimerRango;
        this.SegundoRango = SegundoRango;
    }

    // Devuelve una tupla: tres valores a la vez (como return a, b, c en Python)
    public (Dictionary<string, int>, Dictionary<string, int>,Dictionary<string, int>) FiltrarSentimientos()
    {
        DateTime primerR = DateTime.ParseExact(PrimerRango, "yyyy-MM-dd", CultureInfo.InvariantCulture);
        DateTime segundoR = DateTime.ParseExact(SegundoRango, "yyyy-MM-dd", CultureInfo.InvariantCulture);
        var DiccionarioSP = new Dictionary<string, int>();
        var DiccionarioSN = new Dictionary<string, int>();
        var DiccionarioSNeutro = new Dictionary<string, int>();

        foreach (int dicc in diccionario.Keys)
        {
            string fechaEnDicc = diccionario[dicc].Fecha;
            DateTime fechaEnDiccTD = DateTime.ParseExact(fechaEnDicc, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            bool VerificacionRango = EstaEntre(primerR, segundoR, fechaEnDiccTD);
            if (VerificacionRango == true)
            {
                if (!DiccionarioSP.ContainsKey(fechaEnDicc))
                {
                    DiccionarioSP[fechaEnDicc] = 0;
                    DiccionarioSN[fechaEnDicc] = 0;
                    DiccionarioSNeutro[fechaEnDicc] = 0;
                }
                int positivas = diccionario[dicc].Positivas;
                int negativas = diccionario[dicc].Negativas;

                if (positivas > negativas)
                    DiccionarioSP[fechaEnDicc] += 1;
                else if (negativas > positivas)
                    DiccionarioSN[fechaEnDicc] += 1;
                else
                    DiccionarioSNeutro[fechaEnDicc] += 1;
                
            }
        }
        return (DiccionarioSP, DiccionarioSN, DiccionarioSNeutro);
    }

    public bool EstaEntre(DateTime RangoInicial, DateTime RangoFinal, DateTime Fecha)
    {
        bool Desicion = false;
        if (Fecha >= RangoInicial && Fecha <= RangoFinal)
            Desicion = true;
        return Desicion;
    }
}
