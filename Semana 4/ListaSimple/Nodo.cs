public class Nodo
{
    private int id;
    private int dato;
    private Nodo siguiente; //unicamente apunta al siguiente nodo
    public Nodo(int id, int dato)
    {
        this.id = id;
        this.dato = dato;
        this.siguiente = null;
    }

    public int getId()
    {
        return id;
    }
    public void setId(int id)    {
        this.id = id;   
    }

    public int getDato()
    {
        return dato;
    }
    public void setDato(int dato)    {
        this.dato = dato;   
    }

    public Nodo getSiguiente()
    {
        return siguiente;
    }

    public void setSiguiente(Nodo siguiente)    {
        this.siguiente = siguiente;   
    }
}