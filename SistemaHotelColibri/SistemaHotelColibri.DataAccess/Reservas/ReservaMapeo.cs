using SistemaHotelColibri.Abstract.Modelos.Reservas;
using SistemaHotelColibri.DataAccess.Entidades;

namespace SistemaHotelColibri.DataAccess.Reservas;

internal static class ReservaMapeo
{
    public static ReservaDto ADto(this Reserva r) => new()
    {
        IdReserva = r.IdReserva,
        IdCliente = r.IdCliente,
        IdHabitacion = r.IdHabitacion,
        FechaEntrada = r.FechaEntrada,
        FechaSalida = r.FechaSalida,
        CantidadHuespedes = r.CantidadHuespedes,
        PrecioNoche = r.PrecioNoche,
        CantidadNoches = r.CantidadNoches ?? r.FechaSalida.DayNumber - r.FechaEntrada.DayNumber,
        MontoHospedaje = r.MontoHospedaje ?? r.PrecioNoche * (r.FechaSalida.DayNumber - r.FechaEntrada.DayNumber),
        EstadoReserva = r.EstadoReserva,
        PlacaVehiculo = r.PlacaVehiculo,
        FechaRegistro = r.FechaRegistro,
        Observaciones = r.Observaciones,
        NombreCliente = r.IdClienteNavigation == null ? string.Empty
            : $"{r.IdClienteNavigation.Nombre} {r.IdClienteNavigation.PrimerApellido} {r.IdClienteNavigation.SegundoApellido}".Trim(),
        IdentificacionCliente = r.IdClienteNavigation?.Identificacion ?? string.Empty,
        NumeroHabitacion = r.IdHabitacionNavigation?.NumeroHabitacion ?? string.Empty,
        TipoHabitacion = r.IdHabitacionNavigation?.TipoHabitacion ?? string.Empty,
        CapacidadHabitacion = r.IdHabitacionNavigation?.Capacidad ?? 0
    };
}
