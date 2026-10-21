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
    public int CantidadCamas { get; set; } = 1;
    public string TipoCama { get; set; } = "Matrimonial";
    public bool BanoPrivado { get; set; }
    public string? Vista { get; set; }
    public bool AireAcondicionado { get; set; }
    public bool Television { get; set; }
    public bool Wifi { get; set; }
    public string? Descripcion { get; set; }
}
