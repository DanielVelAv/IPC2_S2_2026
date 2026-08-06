namespace Semana_3.Clases
{
    public class Paciente
    {
        private string nombre;
        private string edad;
        private int periodos;
        private int matriz;
        public Paciente(string nombre, string edad, int periodos, int matriz)
        {
            this.nombre = nombre;
            this.edad = edad;
            this.periodos = periodos;
            this.matriz = matriz;
        }
        public string GetNombre()
        {
            return nombre;
        }
        public void SetNombre(string nombre)
        {
            this.nombre = nombre;
        }
        public string GetEdad()
        {
            return edad;
        }

        public void SetEdad(string edad)
        {
            this.edad = edad;
        }

        public int GetPeriodos()
        {
            return periodos;
        }

        public void SetPeriodos(int periodos)
        {
            this.periodos = periodos;
        }

        public void ImprimirDatosPaciente()
        {
            Console.WriteLine("----------------------------");
            Console.WriteLine($"Nombre: {nombre}");
            Console.WriteLine($"Edad: {edad}");
            Console.WriteLine($"Periodos: {periodos}");
            Console.WriteLine($"Matriz: {matriz}");
        }
    }

}