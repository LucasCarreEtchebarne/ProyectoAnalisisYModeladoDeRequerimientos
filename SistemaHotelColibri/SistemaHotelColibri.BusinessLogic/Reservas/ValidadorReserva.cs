using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Clientes;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Habitaciones;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Reservas;
using SistemaHotelColibri.Abstract.Modelos.Reservas;

namespace SistemaHotelColibri.BusinessLogic.Reservas;

internal class ValidadorReserva
{
    private readonly IObtenerClientePorIdDA _clienteDA;
    private readonly IObtenerHabitacionPorIdDA _habitacionDA;
    private readonly IHabitacionDisponibleReservaDA _disponibleDA;

    public ValidadorReserva(
        IObtenerClientePorIdDA clienteDA,
        IObtenerHabitacionPorIdDA habitacionDA,
        IHabitacionDisponibleReservaDA disponibleDA)
    {
        _clienteDA = clienteDA;
        _habitacionDA = habitacionDA;
        _disponibleDA = disponibleDA;
    }

    public static void Normalizar(ReservaDto reserva)
    {
        reserva.Observaciones = string.IsNullOrWhiteSpace(reserva.Observaciones) ? null : reserva.Observaciones.Trim();
        reserva.PlacaVehiculo = string.IsNullOrWhiteSpace(reserva.PlacaVehiculo) ? null : reserva.PlacaVehiculo.Trim().ToUpperInvariant();
    }

    public async Task<string?> ValidarAsync(ReservaDto reserva, ReservaDto? original)
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

        var cliente = await _clienteDA.ObtenerPorIdAsync(reserva.IdCliente);
        if (cliente == null)
        {
            return Mensajes.ClienteNoRegistrado;
        }

        if (cliente.EstadoCliente == Estados.Cliente.Inactivo && (original == null || original.IdCliente != reserva.IdCliente))
        {
            return Mensajes.ClienteReservaInactivo;
        }

        var habitacion = await _habitacionDA.ObtenerPorIdAsync(reserva.IdHabitacion);
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

        if (reserva.EstadoReserva != Estados.Reserva.Cancelada && reserva.EstadoReserva != Estados.Reserva.CheckOut)
        {
            var disponible = await _disponibleDA.EstaDisponibleAsync(
                reserva.IdHabitacion, reserva.FechaEntrada, reserva.FechaSalida, reserva.IdReserva);
            if (!disponible)
            {
                return Mensajes.ReservaHabitacionNoDisponible;
            }
        }

        return null;
    }
}
