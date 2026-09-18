namespace Prototipo.Services
{
    public class ResultadoOperacion
    {
        public bool Exito { get; set; }
        public string Mensaje { get; set; }
        public int IdGenerado { get; set; }

        public static ResultadoOperacion Ok(string mensaje, int idGenerado = 0)
        {
            return new ResultadoOperacion { Exito = true, Mensaje = mensaje, IdGenerado = idGenerado };
        }

        public static ResultadoOperacion Error(string mensaje)
        {
            return new ResultadoOperacion { Exito = false, Mensaje = mensaje };
        }
    }
}
