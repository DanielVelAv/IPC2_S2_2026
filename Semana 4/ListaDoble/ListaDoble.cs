using System.Diagnostics; // necesario para el ProcessStartInfo y Process
using System.Text; // necesario para el UTF8Encoding

public class ListaDoble
{
    private int id = 0; //contador de id para asignar a cada nodoD creado
    private NodoD primero; 
    private NodoD nodoInicial; 
    private NodoD nodoFinal;
    private int size; //tamaño de la lista
    public ListaDoble()
    {
        this.nodoInicial = null;
        this.nodoFinal = null;
        this.size = 0;
    }

    public NodoD getInicio()
    {
        return this.nodoInicial;
    }

    public int getSize()
    {
        return this.size;
    }

    public bool isEmpty() // esta vacio?
    {
        return this.nodoInicial == null;
    }

    public void agregarNodosM(int m)
    {

        ListaSimple sublista = new ListaSimple();        
        sublista.CrearFilas(m); 


        NodoD nuevoNodo = new NodoD(this.id, sublista);
        this.id++;

        if (this.isEmpty())
        {
            this.nodoInicial = nuevoNodo;
            this.nodoFinal = nuevoNodo;
        }
        else
        {
            nuevoNodo.setAnterior(this.nodoFinal);
            this.nodoFinal.setSiguiente(nuevoNodo);
            this.nodoFinal = nuevoNodo;
        }
        size++;
    }

    public void crearMatriz(int m) //crea una matriz de m x m, es decir, crea m nodosD y cada uno tiene sublistas de m nodos cada una
    {

        for (int i = 0; i < m; i++)
        {
            this.agregarNodosM(m);
        }

    }


    public void recorrerListaDeLista(){
        if (this.nodoInicial == null) //si la lista esta vacia, no hay nada que recorrer
        {
            Console.WriteLine("La lista está vacía.");
            return;
        }
        NodoD actual = this.nodoInicial; // empezamos a recorrer desde el nodo inicial

        while (actual != null) //mientras tenga datos por recorrer
        {
            ListaSimple sublista = actual.getDato(); //devuelve la lista
            //si la sublista no es null, se imprime, de lo contrario imprime
            if (sublista != null) sublista.imprimirLista(); else Console.WriteLine("(fila vacía)"); 
            //actualizamos el nodo actual para que sea el siguiente , asi recorremos toda la lista de nodosD
            actual = actual.getSiguiente();
        }
    }

    public string crearMatriz(string body){
        if (this.nodoInicial == null) //si la lista esta vacia, no hay nada que recorrer
        {
            Console.WriteLine("La lista está vacía.");
            return body;
        }
        NodoD actual = this.nodoInicial; // empezamos a recorrer desde el nodo inicial

        while (actual != null) //mientras tenga datos por recorrer
        {
            ListaSimple sublista = actual.getDato(); //devuelve la lista
            //si la sublista no es null, se imprime, de lo contrario imprime
            if (sublista != null) body = sublista.crearNodos(body); else body += "<tr><td>(fila vacía)</td></tr>\n"; 
            //actualizamos el nodo actual para que sea el siguiente , asi recorremos toda la lista de nodosD
            actual = actual.getSiguiente();
        }
        return body;
    }

    public void graficar()
    {
        string todo = "";
        string header = "digraph Matriz { \n node [shape=plaintext]; \n\n matriz [ \n label=< \n \n <table border=\"1\" cellborder=\"1\" cellspacing=\"0\">";
        string body = "";
        string tail = "\n </table> \n >\n];\n}";

        body = crearMatriz(body);

        todo += header;
        todo += body;
        todo += tail;

        //Console.WriteLine(todo);

        string path = "./dots/matriz.dot";

        try
        {
            // Create the file, or overwrite if the file exists.
            using (FileStream fs = File.Create(path))
            {
                byte[] info = new UTF8Encoding(true).GetBytes(todo);
                // Add some information to the file.
                fs.Write(info, 0, info.Length);
            }
            
        }

        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }

        ProcessStartInfo startInfo = new ProcessStartInfo();
        startInfo.FileName = "dot";
        startInfo.Arguments = "-Tpng " + path + " -o ./grafos/imagen.png";
        startInfo.CreateNoWindow = true;
        startInfo.UseShellExecute = false;

        Process process = Process.Start(startInfo);
        process.WaitForExit();
    }


    public void cambiarEstado(int fil, int colum)
    {
    int fila = fil-1; //restamos 1 para que coincida con el indice de la lista (c# inicia en 0)
    int columna = colum-1; //restamos 1 para que coincida con el indice de la lista (c# inicia en 0)
    NodoD temp = this.getInicio(); // vamos al nodo inicial para recorrer
    while (temp != null) // mientras haya nodos por recorrer
    {
        if (temp.getId() == fila) //si el id del nodoD coincide con la fila que queremos modificar
        {                
            ListaSimple sublista = temp.getDato(); //devuelve la sublista de esa fila
            while (sublista != null && sublista.getInicio() != null) //mientras la sublista no sea null y tenga al menos un nodo
            {
                Nodo tempSub = sublista.getInicio(); //obtenemos el nodo inicial de la sublista (lista simple) para recorrerla
                int contadorColumna = 0; //contador para saber en que columna estamos
                while (tempSub != null) //mientras haya nodos por recorrer en la sublista(lista simple)
                {
                    if (contadorColumna == columna) //si el contador de columna coincide con la columna que queremos modificar
                    {
                        // si el dato es 0 lo cambias a 1 y viceversa
                        int nuevoDato = tempSub.getDato() == 0 ? 1 : 0;
                        tempSub.setDato(nuevoDato);  //setea el dato nuevo
                        return; // Salir después de modificar el estado
                    }
                    tempSub = tempSub.getSiguiente(); // si no es la columna que queremos modificar, seguimos recorriendo la sublista
                    contadorColumna++; //aumentamos el contador de columna para saber en que columna estamos
                }
            }
            if (sublista == null)
            {
                return;
            }
        }
        temp = temp.getSiguiente(); // si no es la fila que queremos modificar, seguimos recorriendo la lista de nodosD
    }
    return; // Retorna -1 si no se encuentra el dato
    }


// no aplica a proyecto (creo)
    public void eliminarDato(int indice)
    {
        NodoD temp = this.getInicio();

        if (id < 0 || indice >= this.size)
        {
            Console.WriteLine("Índice fuera de rango.");
            return;
        }
        
        if (indice == 0){
            // Eliminar el primer nodo
            this.nodoInicial = this.nodoInicial.getSiguiente();
            if (this.nodoInicial != null){
                this.nodoInicial.setAnterior(null);
            }else{
                this.nodoFinal = null; // La lista queda vacía
            }
        }else{
            // Eliminar un nodo intermedio o final
            NodoD temp2 = this.getInicio();
            int contador = 0;
            while (contador < indice)
            {
                temp2 = temp2.getSiguiente();
                contador++;
            }
            NodoD anterior = temp2.getAnterior();
            NodoD siguiente = temp2.getSiguiente();
            
            if (anterior != null)
            {
                anterior.setSiguiente(siguiente);
            }
            
            if (siguiente != null)
            {
                siguiente.setAnterior(anterior);
            }
            
            if (temp2 == this.nodoFinal)
            {
                this.nodoFinal = anterior;
            }
        }
        this.size--;
    }
}