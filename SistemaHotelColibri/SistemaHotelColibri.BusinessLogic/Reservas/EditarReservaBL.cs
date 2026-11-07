using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Reservas;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Clientes;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Habitaciones;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Reservas;
using SistemaHotelColibri.Abstract.Modelos.Reservas;

namespace SistemaHotelColibri.BusinessLogic.Reservas;

public class EditarReservaBL : IEditarReservaBL
{
    private readonly IEditarReservaDA _editarDA;
    private readonly IObtenerReservaPorIdDA _obtenerPorIdDA;
    private readonly IObtenerClientePorIdDA _obtenerClientePorIdDA;
    private readonly IObtenerHabitacionPorIdDA _obtenerHabitacionPorIdDA;
    private readonly IHabitacionDisponibleReservaDA _habitacionDisponibleDA;

    public EditarReservaBL(
        IEditarReservaDA editarDA,
        IObtenerReservaPorIdDA obtenerPorIdDA,
        IObtenerClientePorIdDA obtenerClientePorIdDA,
        IObtenerHabitacionPorIdDA obtenerHabitacionPorIdDA,
        IHabitacionDisponibleReservaDA habitacionDisponibleDA)
    {
        _editarDA = editarDA;
        _obtenerPorIdDA = obtenerPorIdDA;
        _obtenerClientePorIdDA = obtenerClientePorIdDA;
        _obtenerHabitacionPorIdDA = obtenerHabitacionPorIdDA;
        _habitacionDisponibleDA = habitacionDisponibleDA;
    }

    public async Task<ResultadoOperacion> EditarAsync(ReservaDto reserva)
    {
        var original = await _obtenerPorIdDA.ObtenerPorIdAsync(reserva.IdReserva);
        if (original == null)
        {
            return ResultadoOperacion.Error(Mensajes.NoEncontrado);
        }

        ReglasReserva.Normalizar(reserva);

        var cliente = await _obtenerClientePorIdDA.ObtenerPorIdAsync(reserva.IdCliente);
        var habitacion = await _obtenerHabitacionPorIdDA.ObtenerPorIdAsync(reserva.IdHabitacion);
        var error = ReglasReserva.Validar(reserva, original, cliente, habitacion);
        if (error != null)
        {
            return ResultadoOperacion.Error(error);
        }

        if (reserva.EstadoReserva != Estados.Reserva.Cancelada && reserva.EstadoReserva != Estados.Reserva.CheckOut
            && !await _habitacionDisponibleDA.EstaDisponibleAsync(
                reserva.IdHabitacion, reserva.FechaEntrada, reserva.FechaSalida, reserva.IdReserva))
        {
            return ResultadoOperacion.Error(Mensajes.ReservaHabitacionNoDisponible);
        }

        try
        {
            await _editarDA.EditarAsync(reserva);
            return ResultadoOperacion.Ok(Mensajes.ReservaModificada, reserva.IdReserva);
        }
        catch (Exception)
        {
            return ResultadoOperacion.Error(Mensajes.ErrorModificarReserva);
        }
    }
}
