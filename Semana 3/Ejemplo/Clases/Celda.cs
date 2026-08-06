namespace Semana_3.Clases
{
    public class Celda
    {
        private int fila;
        private int columna;

        public Celda(int fila, int columna)
        {
            this.fila = fila;
            this.columna = columna;
        }
        public int GetFila()
        {
            return fila;
        }
        public void SetFila(int fila)
        {
            this.fila = fila;
        }
        public int GetColumna()
        {
            return columna;
        }
        public void SetColumna(int columna)
        {
            this.columna = columna;
        }

        public void ImprimirDatosCelda()
        {
            Console.WriteLine("----------------------------");
            Console.WriteLine($"Fila: {fila}");
            Console.WriteLine($"Columna: {columna}");
        }
    }
}