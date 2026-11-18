namespace SistemaHotelColibri.Abstract.Modelos.Reservas;

public class OpcionesReservaDto
{
    public List<ClienteOpcionDto> Clientes { get; set; } = [];
    public List<HabitacionOpcionDto> Habitaciones { get; set; } = [];
}

public class ClienteOpcionDto
{
    public int IdCliente { get; set; }
    public string Texto { get; set; } = string.Empty;
}

public class HabitacionOpcionDto
{
    public int IdHabitacion { get; set; }
    public string Texto { get; set; } = string.Empty;
    public decimal Precio { get; set; }
    public int Capacidad { get; set; }
    public List<RangoOcupadoDto> Ocupado { get; set; } = [];
}

public class RangoOcupadoDto
{
    public int IdReserva { get; set; }
    public DateOnly FechaEntrada { get; set; }
    public DateOnly FechaSalida { get; set; }
    public string Cliente { get; set; } = string.Empty;
}
