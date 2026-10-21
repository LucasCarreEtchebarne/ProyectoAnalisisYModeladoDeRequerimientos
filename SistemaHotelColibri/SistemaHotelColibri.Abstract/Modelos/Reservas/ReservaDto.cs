namespace SistemaHotelColibri.Abstract.Modelos.Reservas;

public class ReservaDto
{
    public int IdReserva { get; set; }
    public int IdCliente { get; set; }
    public int IdHabitacion { get; set; }
    public DateOnly FechaEntrada { get; set; }
    public DateOnly FechaSalida { get; set; }
    public int CantidadHuespedes { get; set; }
    public decimal PrecioNoche { get; set; }
    public int CantidadNoches { get; set; }
    public decimal MontoHospedaje { get; set; }
    public string EstadoReserva { get; set; } = string.Empty;
    public string? PlacaVehiculo { get; set; }
    public DateTime FechaRegistro { get; set; }
    public string? Observaciones { get; set; }

    public string NombreCliente { get; set; } = string.Empty;
    public string IdentificacionCliente { get; set; } = string.Empty;
    public string NumeroHabitacion { get; set; } = string.Empty;
    public string TipoHabitacion { get; set; } = string.Empty;
    public int CapacidadHabitacion { get; set; }
}
