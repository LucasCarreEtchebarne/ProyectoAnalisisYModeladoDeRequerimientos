namespace SistemaHotelColibri.Abstract.Modelos.Reservas;

// Cómo está una habitación hoy según sus reservas.
public class OcupacionHabitacionDto
{
    public const string Libre = "Libre";
    public const string Reservada = "Reservada";
    public const string Hospedada = "Hospedada";

    public int IdHabitacion { get; set; }

    // Reserva en check-in, o reserva pendiente/confirmada cuyas noches incluyen hoy.
    public ReservaDto? ReservaActual { get; set; }

    // Siguiente reserva pendiente/confirmada que empieza después de hoy.
    public ReservaDto? ProximaReserva { get; set; }

    public string Estado => ReservaActual == null ? Libre
        : ReservaActual.EstadoReserva == Comun.Estados.Reserva.CheckIn ? Hospedada
        : Reservada;
}
