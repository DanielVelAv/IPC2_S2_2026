using Arboles_CSharp.Modelo;

namespace Arboles_CSharp.Estructuras
{
    // Nodo de arbol binario. Igual que el de una lista enlazada,
    // pero con dos enlaces (izquierdo y derecho) en vez de uno.
    public class NodoABB
    {
        public Estudiante Dato { get; set; }
        public NodoABB Izquierdo { get; set; }
        public NodoABB Derecho { get; set; }

        public NodoABB(Estudiante dato)
        {
            Dato = dato;
            Izquierdo = null;
            Derecho = null;
        }

        public bool EsHoja()
        {
            return Izquierdo == null && Derecho == null;
        }
    }
}
