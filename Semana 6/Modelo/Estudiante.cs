namespace Arboles_CSharp.Modelo
{
    // El dato que se guarda en cada nodo del arbol.
    // El carne funciona como CLAVE: es el valor que se compara para
    // decidir si un estudiante va a la izquierda o a la derecha.
    public class Estudiante
    {
        public int Carnet { get; set; }
        public string Nombre { get; set; }
        public string Carrera { get; set; }
        public double Promedio { get; set; }

        public Estudiante(int carnet, string nombre, string carrera, double promedio)
        {
            Carnet = carnet;
            Nombre = nombre;
            Carrera = carrera;
            Promedio = promedio;
        }

        public override string ToString()
        {
            return $"{Carnet}  {Nombre,-22} {Carrera,-14} Promedio: {Promedio:F2}";
        }
    }
}
