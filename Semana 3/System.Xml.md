# System.Xml

```csharp
XmlDocument doc = new XmlDocument();
doc.Load("archivo.xml");              // Cargar XML desde archivo
doc.LoadXml("<root></root>");         // Cargar XML desde string
doc.Save("archivo.xml");              // Guardar documento
doc.DocumentElement;                  // Nodo raíz
doc.SelectSingleNode("//nodo");       // Buscar un nodo (XPath)
doc.SelectNodes("//nodo");            // Buscar varios nodos (XPath)
doc.CreateElement("nombre");          // Crear nuevo elemento

XmlReader reader = XmlReader.Create("archivo.xml");
reader.Read();                        // Avanzar al siguiente nodo
reader.NodeType;                      // Tipo de nodo actual
reader.Name;                          // Nombre del nodo actual
reader.Value;                         // Valor del nodo actual
reader.GetAttribute("attr");          // Obtener atributo
reader.ReadElementContentAsString();  // Leer contenido como texto

XmlWriter writer = XmlWriter.Create("salida.xml");
writer.WriteStartDocument();          // Iniciar documento
writer.WriteEndDocument();            // Finalizar documento
writer.WriteStartElement("nodo");     // Abrir etiqueta
writer.WriteEndElement();             // Cerrar etiqueta
writer.WriteElementString("n", "v");  // Elemento con texto
writer.WriteAttributeString("a", "v");// Escribir atributo

XmlNode nodo;
nodo.InnerText;                       // Texto del nodo
nodo.InnerXml;                        // XML interno
nodo.OuterXml;                        // XML completo del nodo
nodo.Attributes;                      // Atributos del nodo
nodo.ChildNodes;                      // Nodos hijos
nodo.AppendChild(otroNodo);           // Agregar nodo hijo

XmlWriterSettings settings = new XmlWriterSettings
{
    Indent = true,                    // Formato con sangría
    Encoding = Encoding.UTF8          // Codificación
};
```
