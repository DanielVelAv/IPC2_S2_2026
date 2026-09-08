using System;
using Arboles_CSharp.Estructuras;
using Arboles_CSharp.Modelo;

// Cada estudiante se inserta en los dos arboles a la vez, asi se
// pueden comparar en cualquier momento.
ArbolBinarioBusqueda abb = new ArbolBinarioBusqueda();
ArbolAVL avl = new ArbolAVL();

bool continuar = true;
while (continuar)
{
    MostrarMenu();
    string opcion = Console.ReadLine() ?? "0";
    Console.WriteLine("------------------------------------------------------");

    switch (opcion)
    {
        case "1": CargarDatosEjemplo(); break;
        case "2": InsertarEstudiante(); break;
        case "3": BuscarEstudiante(); break;
        case "4": EliminarEstudiante(); break;
        case "5": MostrarRecorridos(); break;
        case "7": CompararEstructuras(); break;
        case "0": continuar = false; break;
        default: Console.WriteLine("Opcion no valida."); break;
    }
}

void MostrarMenu()
{
    Console.WriteLine();
    Console.WriteLine("======================================================");
    Console.WriteLine("  Registro academico - Arboles binarios (ABB y AVL)");
    Console.WriteLine("======================================================");
    Console.WriteLine($"  Estudiantes: {abb.Cantidad}   |   Altura ABB: {abb.Altura()}   Altura AVL: {avl.Altura()}");
    Console.WriteLine("------------------------------------------------------");
    Console.WriteLine("1. Cargar datos de ejemplo");
    Console.WriteLine("2. Insertar estudiante");
    Console.WriteLine("3. Buscar por carne");
    Console.WriteLine("4. Eliminar por carne");
    Console.WriteLine("5. Recorridos del arbol");
    Console.WriteLine("0. Salir");
    Console.WriteLine("======================================================");
    Console.Write("Opcion: ");
}

// ---------------------------------------------------------------------
void CargarDatosEjemplo()
{
    // Carnes en desorden: asi el ABB queda razonablemente equilibrado
    Insertar(new Estudiante(202112345, "Ana Morales", "Sistemas", 85.4));
    Insertar(new Estudiante(201845112, "Carlos Xicara", "Civil", 78.9));
    Insertar(new Estudiante(202233781, "Maria Chocoj", "Industrial", 91.2));
    Insertar(new Estudiante(201990554, "Luis Perez", "Sistemas", 69.5));
    Insertar(new Estudiante(202305019, "Sofia Ramirez", "Mecanica", 88.0));
    Insertar(new Estudiante(201755320, "Jorge Tzoc", "Civil", 74.3));
    Insertar(new Estudiante(202401877, "Elena Batz", "Sistemas", 95.1));

    Console.WriteLine($"Se cargaron 7 estudiantes.");
    Console.WriteLine($"Altura del ABB: {abb.Altura()}   Altura del AVL: {avl.Altura()}");
}

void Insertar(Estudiante e)
{
    abb.Insertar(e);
    avl.Insertar(e);
}

// ---------------------------------------------------------------------
void InsertarEstudiante()
{
    Console.Write("Carne: ");
    if (!int.TryParse(Console.ReadLine(), out int carnet))
    {
        Console.WriteLine("El carne debe ser un numero entero.");
        return;
    }

    if (abb.Buscar(carnet) != null)
    {
        Console.WriteLine("Ya existe un estudiante con ese carne.");
        return;
    }

    Console.Write("Nombre: ");
    string nombre = Console.ReadLine();

    Console.Write("Carrera: ");
    string carrera = Console.ReadLine();

    Console.Write("Promedio: ");
    double.TryParse(Console.ReadLine(), out double promedio);

    Insertar(new Estudiante(carnet, nombre, carrera, promedio));
    Console.WriteLine("Estudiante insertado en ambos arboles.");
    Console.WriteLine($"Altura del ABB: {abb.Altura()}   Altura del AVL: {avl.Altura()}");
}

// ---------------------------------------------------------------------
void BuscarEstudiante()
{
    Console.Write("Carne a buscar: ");
    if (!int.TryParse(Console.ReadLine(), out int carnet)) return;

    Estudiante encontrado = abb.Buscar(carnet);

    if (encontrado == null)
    {
        Console.WriteLine("No se encontro ningun estudiante con ese carne.");
        return;
    }

    Console.WriteLine("Encontrado:");
    Console.WriteLine("   " + encontrado);
    Console.WriteLine();
    Console.WriteLine("Comparaciones que costo la busqueda:");
    Console.WriteLine($"   En el ABB: {abb.ContarPasosBusqueda(carnet)}");
    Console.WriteLine($"   En el AVL: {avl.ContarPasosBusqueda(carnet)}");
}

// ---------------------------------------------------------------------
void EliminarEstudiante()
{
    Console.Write("Carne a eliminar: ");
    if (!int.TryParse(Console.ReadLine(), out int carnet)) return;

    if (abb.Eliminar(carnet))
    {
        avl.Eliminar(carnet);
        Console.WriteLine("Estudiante eliminado de ambos arboles.");
        Console.WriteLine($"El AVL sigue equilibrado: {avl.EstaEquilibrado()}");
    }
    else
    {
        Console.WriteLine("No existe ningun estudiante con ese carne.");
    }
}

// ---------------------------------------------------------------------
void MostrarRecorridos()
{
    if (abb.Cantidad == 0)
    {
        Console.WriteLine("El arbol esta vacio. Usa la opcion 1 primero.");
        return;
    }

    Console.WriteLine("INORDEN (izquierdo - raiz - derecho): sale ordenado por carne");
    abb.InOrden();

    Console.WriteLine();
    Console.WriteLine("PREORDEN (raiz - izquierdo - derecho): sirve para copiar el arbol");
    abb.PreOrden();

    Console.WriteLine();
    Console.WriteLine("POSORDEN (izquierdo - derecho - raiz): sirve para liberar el arbol");
    abb.PosOrden();

    Console.WriteLine();
    Console.WriteLine("POR NIVELES (usa una cola, no recursion)");
    abb.PorNiveles();
}

// ---------------------------------------------------------------------
void DibujarArboles()
{
    Console.WriteLine("ARBOL BINARIO DE BUSQUEDA");
    Console.WriteLine($"Altura: {abb.Altura()}   Nodos: {abb.Cantidad}   Hojas: {abb.ContarHojas()}");
    abb.Dibujar();

    Console.WriteLine();
    Console.WriteLine("ARBOL AVL");
    Console.WriteLine($"Altura: {avl.Altura()}   Nodos: {avl.Cantidad}   Rotaciones hechas: {avl.Rotaciones}");
    avl.Dibujar();
}

// ---------------------------------------------------------------------
// Los carnes reales se asignan en orden ascendente, que es
// el peor caso para un ABB sin equilibrar.
void CompararEstructuras()
{
    ArbolBinarioBusqueda abbOrdenado = new ArbolBinarioBusqueda();
    ArbolAVL avlOrdenado = new ArbolAVL();

    int cantidad = 15;
    int carnetBase = 202400001;

    Console.WriteLine($"Insertando {cantidad} carnes CONSECUTIVOS en ambos arboles...");
    Console.WriteLine("(es lo que pasa cuando se registran los estudiantes en orden)");
    Console.WriteLine();

    for (int i = 0; i < cantidad; i++)
    {
        Estudiante e = new Estudiante(carnetBase + i, $"Estudiante {i + 1}", "Sistemas", 70 + i);
        abbOrdenado.Insertar(e);
        avlOrdenado.Insertar(e);
    }

    int ultimo = carnetBase + cantidad - 1;

    Console.WriteLine("RESULTADO");
    Console.WriteLine("------------------------------------------------------");
    Console.WriteLine($"                        {"ABB",8} {"AVL",8}");
    Console.WriteLine($"Altura del arbol        {abbOrdenado.Altura(),8} {avlOrdenado.Altura(),8}");
    Console.WriteLine($"Comparaciones al buscar {abbOrdenado.ContarPasosBusqueda(ultimo),8} {avlOrdenado.ContarPasosBusqueda(ultimo),8}");
    Console.WriteLine($"Rotaciones realizadas   {"-",8} {avlOrdenado.Rotaciones,8}");
    Console.WriteLine("------------------------------------------------------");
    Console.WriteLine();
    Console.WriteLine("El ABB degenero en una lista enlazada: cada nodo tiene");
    Console.WriteLine("un solo hijo y buscar el ultimo cuesta recorrerlos todos.");
    Console.WriteLine("El AVL se reacomodo solo y mantuvo la altura minima.");
    Console.WriteLine();
    Console.WriteLine("Asi quedo el AVL:");
    avlOrdenado.Dibujar();
}
