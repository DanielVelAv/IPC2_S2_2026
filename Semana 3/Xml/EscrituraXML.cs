using System.Xml;
using Semana_3.Clases;

namespace Semana_3.Xml
{
    public static class EscribirXML
    {
        // Crea y guarda un archivo XML con la estructura de pacientes
        public static void EscribirArchivoXML(string path)
        {
            try
            {
                // Crear el documento XML vacío
                XmlDocument docXml = new XmlDocument();

                // Crear la declaración XML
                XmlDeclaration declaracion = docXml.CreateXmlDeclaration("1.0", "UTF-8", null);
                docXml.AppendChild(declaracion);

                // Crear el nodo raíz <pacientes>
                XmlElement pacientes = docXml.CreateElement("pacientes");
                docXml.AppendChild(pacientes); // Agregar la raíz al documento

                // Crear un paciente de ejemplo y agregarlo como hijo de la raíz
                XmlElement paciente = CrearPacienteEjemplo(docXml, "María García", "25", 5, 10);
                pacientes.AppendChild(paciente);

                // Guardar el documento XML en el archivo especificado
                docXml.Save(path);
                Console.WriteLine($"Archivo XML creado exitosamente en: {path}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al escribir el archivo XML: {ex.Message}");
            }
        }

        // Crea un nodo paciente completo con todos sus datos y rejilla
        private static XmlElement CrearPacienteEjemplo(XmlDocument doc, string nombre, string edad, int periodos, int m)
        {
            // Crear el elemento <paciente>
            XmlElement paciente = doc.CreateElement("paciente");

            // Crear el elemento <datospersonales>
            XmlElement datosPersonales = doc.CreateElement("datospersonales");
            
            // Crear el elemento <nombre> y asignar su contenido interno
            XmlElement nombreElem = doc.CreateElement("nombre");
            nombreElem.InnerText = nombre; // Agregar el texto dentro de <nombre>
            datosPersonales.AppendChild(nombreElem); // Agregar <nombre> como hijo de <datospersonales>

            // Crear el elemento <edad> y asignar su contenido interno
            XmlElement edadElem = doc.CreateElement("edad");
            edadElem.InnerText = edad; // Agregar el texto dentro de <edad>
            datosPersonales.AppendChild(edadElem); // Agregar <edad> como hijo de <datospersonales>

            // Agregar <datospersonales> completo como hijo de <paciente>
            paciente.AppendChild(datosPersonales);

            // Crear el elemento <periodos> con su contenido
            XmlElement periodosElem = doc.CreateElement("periodos");
            periodosElem.InnerText = periodos.ToString(); // Convertir el número a texto
            paciente.AppendChild(periodosElem); // Agregar <periodos> como hijo de <paciente>

            // Crear el elemento <m> (tamaño de la matriz) con su contenido
            XmlElement mElem = doc.CreateElement("m");
            mElem.InnerText = m.ToString(); // Convertir el número a texto
            paciente.AppendChild(mElem); // Agregar <m> como hijo de <paciente>

            // Crear el elemento <rejilla>
            XmlElement rejilla = doc.CreateElement("rejilla");
            
            // Agregar celdas de ejemplo como hijos de <rejilla>
            rejilla.AppendChild(CrearCelda(doc, 1, 1)); // Primera celda
            rejilla.AppendChild(CrearCelda(doc, 1, 2)); // Segunda celda
            rejilla.AppendChild(CrearCelda(doc, 2, 1)); // Tercera celda

            // Agregar <rejilla> completa como hijo de <paciente>
            paciente.AppendChild(rejilla);

            // Retornar el elemento <paciente> completo
            return paciente;
        }

        // Crea un nodo celda individual con sus atributos de fila y columna
        private static XmlElement CrearCelda(XmlDocument doc, int fila, int columna)
        {
            // Crear el elemento <celda>
            XmlElement celda = doc.CreateElement("celda");
            // Agregar el atributo "f" con el valor de la fila
            celda.SetAttribute("f", fila.ToString());
            // Agregar el atributo "c" con el valor de la columna
            celda.SetAttribute("c", columna.ToString());
            
            return celda;
        }
    }
}