
using System.Data;

public static class Program
{
    public static void Main()
    {
        

        ListaDoble listaDoble = new ListaDoble();
        listaDoble.crearMatriz(10);
        listaDoble.recorrerListaDeLista();
        listaDoble.cambiarEstado(2, 3); // Cambia el estado de la fila 2, columna 3
        listaDoble.cambiarEstado(2, 4); // Cambia el estado de la fila 2, columna 3
        listaDoble.cambiarEstado(2, 5); // Cambia el estado de la fila 2, columna 3
        Console.WriteLine("Luego de cambiar el estado de la fila 2, columna 3:");
        Console.WriteLine("\n");
        Console.WriteLine("\n");
        
        listaDoble.recorrerListaDeLista();
        Console.WriteLine("\n");
        listaDoble.graficar();
    }
}