namespace SistemaHotelColibri.Abstract.Modelos.Eventos;

public class EventoDto
{
    public int IdEvento { get; set; }
    public int IdCliente { get; set; }
    public int IdEspacio { get; set; }
    public string NombreEvento { get; set; } = string.Empty;
    public DateOnly FechaEvento { get; set; }
    public TimeOnly HoraInicio { get; set; }
    public TimeOnly HoraFin { get; set; }
    public int Participantes { get; set; }
    public decimal MontoAcordado { get; set; }
    public string EstadoEvento { get; set; } = string.Empty;
    public string? Descripcion { get; set; }

    public string NombreCliente { get; set; } = string.Empty;
    public string IdentificacionCliente { get; set; } = string.Empty;
    public string NombreEspacio { get; set; } = string.Empty;
    public int CapacidadEspacio { get; set; }
}
