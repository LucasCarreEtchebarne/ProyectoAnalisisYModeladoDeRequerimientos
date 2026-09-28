namespace SistemaHotelColibri.Abstract.Modelos.Habitaciones;

public class HabitacionDto
{
    public int IdHabitacion { get; set; }
    public string NumeroHabitacion { get; set; } = string.Empty;
    public string TipoHabitacion { get; set; } = string.Empty;
    public int Capacidad { get; set; }
    public decimal Precio { get; set; }
    public string EstadoHabitacion { get; set; } = string.Empty;
    public int Piso { get; set; }
    public string? Descripcion { get; set; }
}
