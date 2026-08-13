public class ListaSimple
{
    public int id = 0;
    private Nodo nodoInicial;
    private Nodo nodoFinal;
    private int size;

    public ListaSimple()
    {
        this.nodoInicial = null;
        this.nodoFinal = null;
        this.size = 0;
    }

    public Nodo getInicio()
    {
        return this.nodoInicial;
    }

    public int getSize()
    {
        return this.size;
    }

    public bool isEmpty()
    {
        return this.nodoInicial == null;
    }

    public void agregarNodo(int dato)
    {

        Nodo nuevoNodo = new Nodo(this.id, dato); //crea un nuevo nodo con el id actual (el contador) y el dato proporcionado
        this.id++; //se incrementa el contador de id para el siguiente nodo

        if (this.isEmpty()) //si es el primer nodo que se agrega
        {
            this.nodoInicial = nuevoNodo; //sera el nodo inicial y final al ser el unico
            this.nodoFinal = nuevoNodo;
        }
        else //si ya hay nodos en la lista O -> O
        {
            this.nodoFinal.setSiguiente(nuevoNodo); //el nodo final actual (nodo izquierdo) apunta al nuevo nodo (nodo derecho)
            this.nodoFinal = nuevoNodo; //actualizamos el nodo final para que sea el nuevo nodo agregado
        }
        size++; // la lista aumenta
    }

    public void CrearFilas(int m) //funcion para crear las filas para la matriz
    {
        for (int i = 0; i < m; i++)
        {
            this.agregarNodo(0); //crea por defecto nodos con dato 0
        }
    }

    /*No necesario (creo)*/
    public void eliminarDato(int id)
    {
        Nodo temp = this.getInicio();
        Nodo anterior = null;

        while (temp != null)
        {
            if (temp.getId() == id)
            {
                if (anterior == null)
                {
                    this.nodoInicial = temp.getSiguiente();
                    temp.setSiguiente(null);
                }
                else
                {
                    anterior.setSiguiente(temp.getSiguiente());
                }

                if (temp == this.nodoFinal)
                {
                    this.nodoFinal = anterior;
                }

                this.size--;
                return;
            }
            anterior = temp;
            temp = temp.getSiguiente();
        }
    }

    public void imprimirLista()
    {
        Nodo temp = this.getInicio(); //obtenemos el nodo inicial para empezar a recorrer
        while (temp != null) //mientras haya nodos por recorrer
        {
            Console.Write(temp.getDato() + " "); //imprimimos el dato
            temp = temp.getSiguiente(); // vamos al siguiente nodo
        }
        Console.WriteLine(); // salto de linea
    }

    public string crearNodos(string medio)
    {
        medio += "<tr>\n";
        Nodo temp = this.getInicio(); //obtenemos el nodo inicial para empezar a recorrer
        while (temp != null) //mientras haya nodos por recorrer
        {
            medio += "<td>" + temp.getDato() + "</td>\n";
            temp = temp.getSiguiente(); // vamos al siguiente nodo
        }
        medio += "</tr>\n";
        return medio;
    }
}