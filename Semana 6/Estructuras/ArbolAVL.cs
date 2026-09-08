using System;
using Arboles_CSharp.Modelo;
using Arboles_CSharp.Utilidades;

namespace Arboles_CSharp.Estructuras
{
    // ABB que se reacomoda solo: despues de cada insercion o borrado
    // ningun nodo queda con factor de equilibrio fuera de -1, 0 o 1.
    public class ArbolAVL
    {
        public NodoAVL Raiz { get; private set; }
        public int Cantidad { get; private set; }
        public int Rotaciones { get; private set; }

        public ArbolAVL()
        {
            Raiz = null;
            Cantidad = 0;
            Rotaciones = 0;
        }

        // ---------------------------------------------------------------
        // MEDIDAS AUXILIARES
        // ---------------------------------------------------------------
        private int AlturaDe(NodoAVL nodo)
        {
            return nodo == null ? 0 : nodo.Altura;
        }

        private int Maximo(int a, int b)
        {
            return a > b ? a : b;
        }

        private void ActualizarAltura(NodoAVL nodo)
        {
            nodo.Altura = 1 + Maximo(AlturaDe(nodo.Izquierdo), AlturaDe(nodo.Derecho));
        }

        // Factor de equilibrio = altura izquierda - altura derecha
        //   > 1  : el arbol esta cargado a la IZQUIERDA
        //   < -1 : el arbol esta cargado a la DERECHA
        public int FactorEquilibrio(NodoAVL nodo)
        {
            return nodo == null ? 0 : AlturaDe(nodo.Izquierdo) - AlturaDe(nodo.Derecho);
        }

        // ---------------------------------------------------------------
        // ROTACIONES
        // ---------------------------------------------------------------
        // Sube el hijo izquierdo (x) y baja "y" hacia la derecha.
        // El subarbol B deja de colgar de x y pasa a colgar de y.
        private NodoAVL RotarDerecha(NodoAVL y)
        {
            NodoAVL x = y.Izquierdo;
            NodoAVL B = x.Derecho;

            x.Derecho = y;
            y.Izquierdo = B;

            ActualizarAltura(y);   // y quedo mas abajo, va primero
            ActualizarAltura(x);

            Rotaciones++;
            return x;              // nueva raiz del subarbol
        }

        // El caso espejo: sube el hijo derecho (y) y baja "x" hacia
        // la izquierda. B cambia de padre, ahora cuelga de x.
        private NodoAVL RotarIzquierda(NodoAVL x)
        {
            NodoAVL y = x.Derecho;
            NodoAVL B = y.Izquierdo;

            y.Izquierdo = x;
            x.Derecho = B;

            ActualizarAltura(x);
            ActualizarAltura(y);

            Rotaciones++;
            return y;
        }

        // ---------------------------------------------------------------
        // INSERCION
        // ---------------------------------------------------------------
        public bool Insertar(Estudiante dato)
        {
            if (Buscar(dato.Carnet) != null) return false;

            Raiz = InsertarRec(Raiz, dato);
            Cantidad++;
            return true;
        }

        private NodoAVL InsertarRec(NodoAVL nodo, Estudiante dato)
        {
            // Se inserta igual que en un ABB normal
            if (nodo == null) return new NodoAVL(dato);

            if (dato.Carnet < nodo.Dato.Carnet)
                nodo.Izquierdo = InsertarRec(nodo.Izquierdo, dato);
            else if (dato.Carnet > nodo.Dato.Carnet)
                nodo.Derecho = InsertarRec(nodo.Derecho, dato);
            else
                return nodo;

            // Ya de regreso de la recursion: se recalcula la altura
            // del nodo y se revisa si quedo desequilibrado
            ActualizarAltura(nodo);
            int fe = FactorEquilibrio(nodo);

            // CASO IZQUIERDA-IZQUIERDA: una rotacion simple a la derecha
            if (fe > 1 && dato.Carnet < nodo.Izquierdo.Dato.Carnet)
                return RotarDerecha(nodo);

            // CASO DERECHA-DERECHA: una rotacion simple a la izquierda
            if (fe < -1 && dato.Carnet > nodo.Derecho.Dato.Carnet)
                return RotarIzquierda(nodo);

            // CASO IZQUIERDA-DERECHA: rotacion doble
            if (fe > 1 && dato.Carnet > nodo.Izquierdo.Dato.Carnet)
            {
                nodo.Izquierdo = RotarIzquierda(nodo.Izquierdo);
                return RotarDerecha(nodo);
            }

            // CASO DERECHA-IZQUIERDA: rotacion doble
            if (fe < -1 && dato.Carnet < nodo.Derecho.Dato.Carnet)
            {
                nodo.Derecho = RotarDerecha(nodo.Derecho);
                return RotarIzquierda(nodo);
            }

            return nodo;
        }

        // ---------------------------------------------------------------
        // BUSQUEDA (identica a la del ABB: un AVL sigue siendo un ABB)
        // ---------------------------------------------------------------
        public Estudiante Buscar(int carnet)
        {
            NodoAVL actual = Raiz;

            while (actual != null)
            {
                if (carnet == actual.Dato.Carnet) return actual.Dato;

                actual = carnet < actual.Dato.Carnet
                    ? actual.Izquierdo
                    : actual.Derecho;
            }

            return null;
        }

        public int ContarPasosBusqueda(int carnet)
        {
            NodoAVL actual = Raiz;
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

        private NodoAVL EliminarRec(NodoAVL nodo, int carnet)
        {
            if (nodo == null) return null;

            // Se borra igual que en un ABB normal
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
                if (nodo.Izquierdo == null) return nodo.Derecho;
                if (nodo.Derecho == null) return nodo.Izquierdo;

                NodoAVL sucesor = MinimoNodo(nodo.Derecho);
                nodo.Dato = sucesor.Dato;
                nodo.Derecho = EliminarRec(nodo.Derecho, sucesor.Dato.Carnet);
            }

            if (nodo == null) return null;

            // Al volver se recalcula la altura y se corrige el equilibrio.
            // A diferencia de la insercion, aca se decide mirando el
            // factor de equilibrio del hijo, no una clave.
            ActualizarAltura(nodo);
            int fe = FactorEquilibrio(nodo);

            if (fe > 1 && FactorEquilibrio(nodo.Izquierdo) >= 0)
                return RotarDerecha(nodo);

            if (fe > 1 && FactorEquilibrio(nodo.Izquierdo) < 0)
            {
                nodo.Izquierdo = RotarIzquierda(nodo.Izquierdo);
                return RotarDerecha(nodo);
            }

            if (fe < -1 && FactorEquilibrio(nodo.Derecho) <= 0)
                return RotarIzquierda(nodo);

            if (fe < -1 && FactorEquilibrio(nodo.Derecho) > 0)
            {
                nodo.Derecho = RotarDerecha(nodo.Derecho);
                return RotarIzquierda(nodo);
            }

            return nodo;
        }

        private NodoAVL MinimoNodo(NodoAVL nodo)
        {
            while (nodo.Izquierdo != null) nodo = nodo.Izquierdo;
            return nodo;
        }

        // ---------------------------------------------------------------
        // RECORRIDOS
        // ---------------------------------------------------------------
        public void InOrden()
        {
            InOrdenRec(Raiz);
        }

        private void InOrdenRec(NodoAVL nodo)
        {
            if (nodo == null) return;

            InOrdenRec(nodo.Izquierdo);
            Console.WriteLine($"   {nodo.Dato}   [FE: {FactorEquilibrio(nodo),2}]");
            InOrdenRec(nodo.Derecho);
        }

        public void PorNiveles()
        {
            if (Raiz == null) return;

            ColaEnlazada cola = new ColaEnlazada();
            cola.Encolar(Raiz);

            while (!cola.EstaVacia())
            {
                NodoAVL actual = (NodoAVL)cola.Desencolar();
                Console.WriteLine($"   {actual.Dato}   [FE: {FactorEquilibrio(actual),2}]");

                if (actual.Izquierdo != null) cola.Encolar(actual.Izquierdo);
                if (actual.Derecho != null) cola.Encolar(actual.Derecho);
            }
        }

        public int Altura()
        {
            return AlturaDe(Raiz);
        }

        // Verifica que el arbol siga cumpliendo la condicion AVL
        public bool EstaEquilibrado()
        {
            return EquilibradoRec(Raiz);
        }

        private bool EquilibradoRec(NodoAVL nodo)
        {
            if (nodo == null) return true;

            int fe = FactorEquilibrio(nodo);
            if (fe < -1 || fe > 1) return false;

            return EquilibradoRec(nodo.Izquierdo) && EquilibradoRec(nodo.Derecho);
        }

        // ---------------------------------------------------------------
        // DIBUJO EN CONSOLA
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

        private void DibujarRec(NodoAVL nodo, string prefijo, bool esIzquierdo)
        {
            if (nodo == null) return;

            DibujarRec(nodo.Derecho, prefijo + (esIzquierdo ? "|   " : "    "), false);
            Console.WriteLine($"{prefijo}{(esIzquierdo ? "\\-- " : "/-- ")}{nodo.Dato.Carnet} (FE {FactorEquilibrio(nodo)})");
            DibujarRec(nodo.Izquierdo, prefijo + (esIzquierdo ? "    " : "|   "), true);
        }
    }
}
