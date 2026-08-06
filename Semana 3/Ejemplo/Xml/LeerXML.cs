using System.Xml;
using Semana_3.Clases;

namespace Semana_3.Xml
{
    public static class LeerXML
    {
        // Lee el archivo XML y procesa todos los pacientes que contiene
        public static void LeerArchivoXML(string path)
        {
            if (!File.Exists(path))
            {
                Console.WriteLine("El archivo XML no existe.");
                return;
            }
            try
            {
                XmlDocument docXml = new XmlDocument();
                docXml.Load(path);
                // Obtener el nodo raíz "pacientes"
                XmlNode? pacientes = docXml.DocumentElement;
                if (pacientes == null)
                {
                    Console.WriteLine("El archivo XML está vacío o mal formado.");
                    return;
                }
                Console.WriteLine($"Verificando {pacientes.ChildNodes.Count} Pacientes en el Archivo XML.");
                foreach (XmlNode nodoPaciente in pacientes.ChildNodes)
                {
                    if (nodoPaciente.Name == "paciente")
                    {
                        // Procesar cada paciente individualmente
                        ProcesarPaciente(nodoPaciente);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al leer el archivo XML: {ex.Message}");
            }
        }

        // Extrae los datos de un paciente individual y crea el objeto Paciente
        private static void ProcesarPaciente(XmlNode nodoPaciente)
        {
            try
            {
                XmlNode? datosPersonales = nodoPaciente.SelectSingleNode("datospersonales");
                // Extraer datos personales del paciente
                string nombre = datosPersonales?.SelectSingleNode("nombre")?.InnerText ?? "";
                string edad = datosPersonales?.SelectSingleNode("edad")?.InnerText ?? "0";
                string periodos = nodoPaciente.SelectSingleNode("periodos")?.InnerText ?? "0";
                string matriz = nodoPaciente.SelectSingleNode("m")?.InnerText ?? "0";
                // Crear el objeto Paciente con los datos extraídos
                Paciente paciente = new Paciente(nombre, edad, int.Parse(periodos), int.Parse(matriz));
                paciente.ImprimirDatosPaciente();

                XmlNode? rejilla = nodoPaciente.SelectSingleNode("rejilla");
                
                if (rejilla != null && rejilla.HasChildNodes)
                {
                    // Procesar las celdas contagiadas del paciente
                    ProcesarCeldas(rejilla);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al procesar paciente: {ex.Message}");
            }
        }

        // Recorre y procesa cada celda contagiada de la rejilla
        private static void ProcesarCeldas(XmlNode rejilla)
        {
            int contador = 0;
            
            foreach (XmlNode nodoCelda in rejilla.ChildNodes)
            {
                if (nodoCelda.Name == "celda" && nodoCelda.Attributes != null)
                {
                    string? fila = nodoCelda.Attributes["f"]?.Value;
                    string? columna = nodoCelda.Attributes["c"]?.Value;

                    if (fila != null && columna != null)
                    {
                        Celda celda = new Celda(int.Parse(fila), int.Parse(columna));
                        celda.ImprimirDatosCelda();
                        contador++;
                    }
                }
            }
            Console.WriteLine($"Total de celdas procesadas: {contador}");
        }
    }
}