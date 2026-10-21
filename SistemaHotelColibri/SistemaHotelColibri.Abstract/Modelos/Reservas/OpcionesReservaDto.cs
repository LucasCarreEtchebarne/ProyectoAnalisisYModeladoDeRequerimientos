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
}
