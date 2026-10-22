using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Reservas;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Clientes;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Habitaciones;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Reservas;
using SistemaHotelColibri.Abstract.Modelos.Reservas;

namespace SistemaHotelColibri.BusinessLogic.Reservas;

public class EditarReservaBL : IEditarReservaBL
{
    private readonly IEditarReservaDA _da;
    private readonly IObtenerReservaPorIdDA _obtenerDA;
    private readonly ValidadorReserva _validador;

    public EditarReservaBL(
        IEditarReservaDA da,
        IObtenerReservaPorIdDA obtenerDA,
        IObtenerClientePorIdDA clienteDA,
        IObtenerHabitacionPorIdDA habitacionDA,
        IHabitacionDisponibleReservaDA disponibleDA)
    {
        _da = da;
        _obtenerDA = obtenerDA;
        _validador = new ValidadorReserva(clienteDA, habitacionDA, disponibleDA);
    }

    public async Task<ResultadoOperacion> EditarAsync(ReservaDto reserva)
    {
        var original = await _obtenerDA.ObtenerPorIdAsync(reserva.IdReserva);
        if (original == null)
        {
            return ResultadoOperacion.Error(Mensajes.NoEncontrado);
        }

        ValidadorReserva.Normalizar(reserva);
        var error = await _validador.ValidarAsync(reserva, original);
        if (error != null)
        {
            return ResultadoOperacion.Error(error);
        }

        try
        {
            await _da.EditarAsync(reserva);
            return ResultadoOperacion.Ok(Mensajes.ReservaModificada, reserva.IdReserva);
        }
        catch (Exception)
        {
            return ResultadoOperacion.Error(Mensajes.ErrorModificarReserva);
        }
    }
}
