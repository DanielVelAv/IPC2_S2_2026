public class NodoD
{
    public int id;
    public ListaSimple dato;
    public NodoD siguiente; //apunta al siguiente nodo
    public NodoD anterior; //apunta al nodo anterior

    public NodoD(int id, ListaSimple dato)
    {
        this.id = id;
        this.dato = dato; //al instanciar un nodo, se le asigna una sublista como dato, osea se crea una sublista junto con el nodo
        this.siguiente = null;
        this.anterior = null;
    }

    public int getId()
    {
        return id;
    }
    public void setId(int id)    {
        this.id = id;   
    }

    public ListaSimple getDato()
    {
        return dato;
    }
    public void setDato(ListaSimple dato)    {
        this.dato = dato;   
    }

    public NodoD getSiguiente()
    {
        return siguiente;
    }

    public void setSiguiente(NodoD siguiente)    {
        this.siguiente = siguiente;   
    }

     public NodoD getAnterior()
    {
        return anterior;
    }

    public void setAnterior(NodoD anterior)    {
        this.anterior = anterior;   
    }
}