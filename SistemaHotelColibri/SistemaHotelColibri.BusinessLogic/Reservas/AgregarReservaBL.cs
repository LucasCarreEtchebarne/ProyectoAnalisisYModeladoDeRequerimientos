using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Reservas;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Clientes;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Habitaciones;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Reservas;
using SistemaHotelColibri.Abstract.Modelos.Reservas;

namespace SistemaHotelColibri.BusinessLogic.Reservas;

public class AgregarReservaBL : IAgregarReservaBL
{
    private readonly IAgregarReservaDA _agregarDA;
    private readonly IObtenerClientePorIdDA _obtenerClientePorIdDA;
    private readonly IObtenerHabitacionPorIdDA _obtenerHabitacionPorIdDA;
    private readonly IHabitacionDisponibleReservaDA _habitacionDisponibleDA;

    public AgregarReservaBL(
        IAgregarReservaDA agregarDA,
        IObtenerClientePorIdDA obtenerClientePorIdDA,
        IObtenerHabitacionPorIdDA obtenerHabitacionPorIdDA,
        IHabitacionDisponibleReservaDA habitacionDisponibleDA)
    {
        _agregarDA = agregarDA;
        _obtenerClientePorIdDA = obtenerClientePorIdDA;
        _obtenerHabitacionPorIdDA = obtenerHabitacionPorIdDA;
        _habitacionDisponibleDA = habitacionDisponibleDA;
    }

    public async Task<ResultadoOperacion> AgregarAsync(ReservaDto reserva)
    {
        ReglasReserva.Normalizar(reserva);

        var cliente = await _obtenerClientePorIdDA.ObtenerPorIdAsync(reserva.IdCliente);
        var habitacion = await _obtenerHabitacionPorIdDA.ObtenerPorIdAsync(reserva.IdHabitacion);
        var error = ReglasReserva.Validar(reserva, null, cliente, habitacion);
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
            var id = await _agregarDA.AgregarAsync(reserva);
            return ResultadoOperacion.Ok(Mensajes.ReservaRegistrada, id);
        }
        catch (Exception)
        {
            return ResultadoOperacion.Error(Mensajes.ErrorRegistrarReserva);
        }
    }
}
