using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Modelos.Clientes;
using SistemaHotelColibri.Abstract.Modelos.Habitaciones;
using SistemaHotelColibri.Abstract.Modelos.Reservas;

namespace SistemaHotelColibri.BusinessLogic.Reservas;

internal static class ReglasReserva
{
    public static void Normalizar(ReservaDto reserva)
    {
        reserva.Observaciones = string.IsNullOrWhiteSpace(reserva.Observaciones) ? null : reserva.Observaciones.Trim();
        reserva.PlacaVehiculo = string.IsNullOrWhiteSpace(reserva.PlacaVehiculo) ? null : reserva.PlacaVehiculo.Trim().ToUpperInvariant();
    }

    public static string? Validar(ReservaDto reserva, ReservaDto? original, ClienteDto? cliente, HabitacionDto? habitacion)
    {
        if (reserva.IdCliente <= 0 || reserva.IdHabitacion <= 0
            || reserva.FechaEntrada == default || reserva.FechaSalida == default
            || string.IsNullOrWhiteSpace(reserva.EstadoReserva))
        {
            return Mensajes.ReservaCamposIncompletos;
        }

        if (!Estados.Reserva.Todos.Contains(reserva.EstadoReserva))
        {
            return Mensajes.EstadoReservaInvalido;
        }

        if (reserva.FechaSalida <= reserva.FechaEntrada)
        {
            return Mensajes.FechasReservaInvalidas;
        }

        if (reserva.CantidadHuespedes <= 0)
        {
            return Mensajes.CantidadHuespedesInvalida;
        }

        if (reserva.PrecioNoche < 0)
        {
            return Mensajes.PrecioInvalido;
        }

        var fechasCambiaron = original == null
            || original.FechaEntrada != reserva.FechaEntrada
            || original.IdHabitacion != reserva.IdHabitacion;
        if (fechasCambiaron && reserva.FechaEntrada < DateOnly.FromDateTime(DateTime.Today))
        {
            return Mensajes.FechaEntradaPasada;
        }

        if (original != null
            && reserva.EstadoReserva == Estados.Reserva.Cancelada
            && original.EstadoReserva is Estados.Reserva.CheckIn or Estados.Reserva.CheckOut)
        {
            return Mensajes.ReservaYaEmpezo;
        }

        if (cliente == null)
        {
            return Mensajes.ClienteNoRegistrado;
        }

        if (cliente.EstadoCliente == Estados.Cliente.Inactivo && (original == null || original.IdCliente != reserva.IdCliente))
        {
            return Mensajes.ClienteReservaInactivo;
        }

        if (habitacion == null)
        {
            return Mensajes.NoEncontrado;
        }

        if (habitacion.EstadoHabitacion == Estados.Habitacion.Inactiva && (original == null || original.IdHabitacion != reserva.IdHabitacion))
        {
            return Mensajes.HabitacionReservaInactiva;
        }

        if (reserva.CantidadHuespedes > habitacion.Capacidad)
        {
            return string.Format(Mensajes.HuespedesExcedenCapacidad, habitacion.Capacidad);
        }

        return null;
    }
}
