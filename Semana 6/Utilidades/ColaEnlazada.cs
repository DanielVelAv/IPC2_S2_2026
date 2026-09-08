namespace Arboles_CSharp.Utilidades
{
    // Cola hecha a mano con nodos enlazados. El recorrido por niveles
    // necesita una cola y en el curso no usamos Queue<T> del framework.
    public class NodoCola
    {
        public object Valor { get; set; }
        public NodoCola Siguiente { get; set; }

        public NodoCola(object valor)
        {
            Valor = valor;
            Siguiente = null;
        }
    }

    public class ColaEnlazada
    {
        private NodoCola primero;
        private NodoCola ultimo;
        public int Cantidad { get; private set; }

        public ColaEnlazada()
        {
            primero = null;
            ultimo = null;
            Cantidad = 0;
        }

        public bool EstaVacia()
        {
            return primero == null;
        }

        // Agrega al final
        public void Encolar(object valor)
        {
            NodoCola nuevo = new NodoCola(valor);

            if (primero == null)
            {
                primero = nuevo;
                ultimo = nuevo;
            }
            else
            {
                ultimo.Siguiente = nuevo;
                ultimo = nuevo;
            }

            Cantidad++;
        }

        // Saca el primero
        public object Desencolar()
        {
            if (primero == null) return null;

            object valor = primero.Valor;
            primero = primero.Siguiente;

            if (primero == null) ultimo = null;

            Cantidad--;
            return valor;
        }
    }
}
