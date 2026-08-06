using System.Data;
using System.Diagnostics;
using Semana_3.Xml;
bool continuar = true;
while (continuar)
{
    MostrarMenu();
    string opcion = Console.ReadLine() ?? "0";
    Console.WriteLine("--------------------------------");
    
    switch (opcion)
    {
        case "1":
            LeerArchivo();
            break;
        case "2":
            EscribirArchivo();
            break;
        case "0":
            continuar = false;
            break;
        default:
            Console.WriteLine("Opción no válida. Intenta de nuevo.");
            break;
    }
}

// Muestra el menú principal de opciones
static void MostrarMenu()
{
    Console.WriteLine("--------------------------------");
    Console.WriteLine("Menú Principal");
    Console.WriteLine("--------------------------------");
    Console.WriteLine("1. Leer Archivo XML");
    Console.WriteLine("2. Escribir Archivo XML");
    Console.WriteLine("0. Salir");
    Console.WriteLine("--------------------------------");
    Console.Write("Opción: ");
}

static void LeerArchivo()
{
    string path = @"./Entrada1.xml";
    LeerXML.LeerArchivoXML(path);
}

// Llama a la función que escribe un archivo XML de salida
static void EscribirArchivo()
{
    string path = @"./Salida.xml";
    EscribirXML.EscribirArchivoXML(path);
}
