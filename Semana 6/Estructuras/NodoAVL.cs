using Arboles_CSharp.Modelo;

namespace Arboles_CSharp.Estructuras
{
    // Igual que NodoABB, pero cada nodo guarda ademas su altura.
    // Asi no hay que recorrer el subarbol para recalcularla cada vez
    // que se necesita el factor de equilibrio.
    public class NodoAVL
    {
        public Estudiante Dato { get; set; }
        public NodoAVL Izquierdo { get; set; }
        public NodoAVL Derecho { get; set; }
        public int Altura { get; set; }

        public NodoAVL(Estudiante dato)
        {
            Dato = dato;
            Izquierdo = null;
            Derecho = null;
            Altura = 1;          // un nodo recien creado es una hoja
        }
    }
}
