namespace SistemaHotelColibri.Abstract.Comun;

public class ResultadoOperacion
{
    public bool Exito { get; private init; }
    public string Mensaje { get; private init; } = string.Empty;
    public int IdGenerado { get; private init; }

    public static ResultadoOperacion Ok(string mensaje, int idGenerado = 0) =>
        new() { Exito = true, Mensaje = mensaje, IdGenerado = idGenerado };

    public static ResultadoOperacion Error(string mensaje) =>
        new() { Exito = false, Mensaje = mensaje };
}
