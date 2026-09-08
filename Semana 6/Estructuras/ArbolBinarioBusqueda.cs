using System;
using Arboles_CSharp.Modelo;
using Arboles_CSharp.Utilidades;

namespace Arboles_CSharp.Estructuras
{
    // Arbol Binario de Busqueda (ABB).
    // Propiedad que se cumple en TODOS los nodos:
    //   subarbol izquierdo  ->  claves MENORES que la del nodo
    //   subarbol derecho    ->  claves MAYORES que la del nodo
    public class ArbolBinarioBusqueda
    {
        public NodoABB Raiz { get; private set; }
        public int Cantidad { get; private set; }

        public ArbolBinarioBusqueda()
        {
            Raiz = null;
            Cantidad = 0;
        }

        // ---------------------------------------------------------------
        // INSERCION
        // ---------------------------------------------------------------
        public bool Insertar(Estudiante dato)
        {
            if (Buscar(dato.Carnet) != null) return false;   // no se aceptan duplicados

            Raiz = InsertarRec(Raiz, dato);
            Cantidad++;
            return true;
        }

        // Version recursiva: devuelve el subarbol ya modificado
        private NodoABB InsertarRec(NodoABB nodo, Estudiante dato)
        {
            // Caso base: llegamos a un espacio vacio, aqui va el nuevo nodo
            if (nodo == null) return new NodoABB(dato);

            if (dato.Carnet < nodo.Dato.Carnet)
                nodo.Izquierdo = InsertarRec(nodo.Izquierdo, dato);
            else if (dato.Carnet > nodo.Dato.Carnet)
                nodo.Derecho = InsertarRec(nodo.Derecho, dato);

            return nodo;
        }

        // ---------------------------------------------------------------
        // BUSQUEDA
        // ---------------------------------------------------------------
        // Version iterativa: se baja por el arbol comparando en cada nivel
        public Estudiante Buscar(int carnet)
        {
            NodoABB actual = Raiz;

            while (actual != null)
            {
                if (carnet == actual.Dato.Carnet) return actual.Dato;

                actual = carnet < actual.Dato.Carnet
                    ? actual.Izquierdo
                    : actual.Derecho;
            }

            return null;   // no esta en el arbol
        }

        // Devuelve cuantas comparaciones costo llegar a la clave.
        // Lo usamos para comparar el ABB contra el AVL.
        public int ContarPasosBusqueda(int carnet)
        {
            NodoABB actual = Raiz;
            int pasos = 0;

            while (actual != null)
            {
                pasos++;
                if (carnet == actual.Dato.Carnet) return pasos;

                actual = carnet < actual.Dato.Carnet
                    ? actual.Izquierdo
                    : actual.Derecho;
            }

            return pasos;
        }

        // ---------------------------------------------------------------
        // ELIMINACION
        // ---------------------------------------------------------------
        public bool Eliminar(int carnet)
        {
            if (Buscar(carnet) == null) return false;

            Raiz = EliminarRec(Raiz, carnet);
            Cantidad--;
            return true;
        }

        private NodoABB EliminarRec(NodoABB nodo, int carnet)
        {
            if (nodo == null) return null;

            if (carnet < nodo.Dato.Carnet)
            {
                nodo.Izquierdo = EliminarRec(nodo.Izquierdo, carnet);
            }
            else if (carnet > nodo.Dato.Carnet)
            {
                nodo.Derecho = EliminarRec(nodo.Derecho, carnet);
            }
            else
            {
                // Encontramos el nodo. Hay tres casos posibles:

                // CASO 1: es hoja, simplemente se borra
                if (nodo.Izquierdo == null && nodo.Derecho == null)
                    return null;

                // CASO 2: tiene un solo hijo, el hijo ocupa su lugar
                if (nodo.Izquierdo == null) return nodo.Derecho;
                if (nodo.Derecho == null) return nodo.Izquierdo;

                // CASO 3: tiene dos hijos. Se trae el menor del subarbol
                // derecho (su sucesor inorden), se copia aca y se borra alla.
                NodoABB sucesor = MinimoNodo(nodo.Derecho);
                nodo.Dato = sucesor.Dato;
                nodo.Derecho = EliminarRec(nodo.Derecho, sucesor.Dato.Carnet);
            }

            return nodo;
        }

        // El menor de un subarbol se encuentra bajando siempre a la izquierda
        private NodoABB MinimoNodo(NodoABB nodo)
        {
            while (nodo.Izquierdo != null) nodo = nodo.Izquierdo;
            return nodo;
        }

        // ---------------------------------------------------------------
        // RECORRIDOS EN PROFUNDIDAD
        // ---------------------------------------------------------------
        // INORDEN: izquierdo -> RAIZ -> derecho   (sale ordenado)
        public void InOrden()
        {
            InOrdenRec(Raiz);
        }

        private void InOrdenRec(NodoABB nodo)
        {
            if (nodo == null) return;

            InOrdenRec(nodo.Izquierdo);
            Console.WriteLine("   " + nodo.Dato);
            InOrdenRec(nodo.Derecho);
        }

        // PREORDEN: RAIZ -> izquierdo -> derecho  (sirve para copiar el arbol)
        public void PreOrden()
        {
            PreOrdenRec(Raiz);
        }

        private void PreOrdenRec(NodoABB nodo)
        {
            if (nodo == null) return;

            Console.WriteLine("   " + nodo.Dato);
            PreOrdenRec(nodo.Izquierdo);
            PreOrdenRec(nodo.Derecho);
        }

        // POSORDEN: izquierdo -> derecho -> RAIZ  (sirve para liberar el arbol)
        public void PosOrden()
        {
            PosOrdenRec(Raiz);
        }

        private void PosOrdenRec(NodoABB nodo)
        {
            if (nodo == null) return;

            PosOrdenRec(nodo.Izquierdo);
            PosOrdenRec(nodo.Derecho);
            Console.WriteLine("   " + nodo.Dato);
        }

        // ---------------------------------------------------------------
        // RECORRIDO EN ANCHURA (por niveles)
        // ---------------------------------------------------------------
        // Este NO es recursivo: necesita una cola para ir guardando
        // los nodos pendientes de visitar.
        public void PorNiveles()
        {
            if (Raiz == null) return;

            ColaEnlazada cola = new ColaEnlazada();
            cola.Encolar(Raiz);

            while (!cola.EstaVacia())
            {
                NodoABB actual = (NodoABB)cola.Desencolar();
                Console.WriteLine("   " + actual.Dato);

                if (actual.Izquierdo != null) cola.Encolar(actual.Izquierdo);
                if (actual.Derecho != null) cola.Encolar(actual.Derecho);
            }
        }

        // ---------------------------------------------------------------
        // MEDIDAS DEL ARBOL
        // ---------------------------------------------------------------
        public int Altura()
        {
            return AlturaRec(Raiz);
        }

        private int AlturaRec(NodoABB nodo)
        {
            if (nodo == null) return 0;

            int izq = AlturaRec(nodo.Izquierdo);
            int der = AlturaRec(nodo.Derecho);

            return 1 + (izq > der ? izq : der);
        }

        public int ContarHojas()
        {
            return ContarHojasRec(Raiz);
        }

        private int ContarHojasRec(NodoABB nodo)
        {
            if (nodo == null) return 0;
            if (nodo.EsHoja()) return 1;

            return ContarHojasRec(nodo.Izquierdo) + ContarHojasRec(nodo.Derecho);
        }

        // ---------------------------------------------------------------
        // DIBUJO EN CONSOLA (el arbol se ve acostado, la raiz a la izquierda)
        // ---------------------------------------------------------------
        public void Dibujar()
        {
            if (Raiz == null)
            {
                Console.WriteLine("   (arbol vacio)");
                return;
            }

            DibujarRec(Raiz, "", true);
        }

        private void DibujarRec(NodoABB nodo, string prefijo, bool esIzquierdo)
        {
            if (nodo == null) return;

            DibujarRec(nodo.Derecho, prefijo + (esIzquierdo ? "|   " : "    "), false);
            Console.WriteLine(prefijo + (esIzquierdo ? "\\-- " : "/-- ") + nodo.Dato.Carnet);
            DibujarRec(nodo.Izquierdo, prefijo + (esIzquierdo ? "    " : "|   "), true);
        }
    }
}
