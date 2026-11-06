using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Reservas;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Reservas;

namespace SistemaHotelColibri.BusinessLogic.Reservas;

public class EliminarReservaBL : IEliminarReservaBL
{
    private readonly IEliminarReservaDA _eliminarDA;
    private readonly IObtenerReservaPorIdDA _obtenerPorIdDA;

    public EliminarReservaBL(IEliminarReservaDA eliminarDA, IObtenerReservaPorIdDA obtenerPorIdDA)
    {
        _eliminarDA = eliminarDA;
        _obtenerPorIdDA = obtenerPorIdDA;
    }

    public async Task<ResultadoOperacion> EliminarAsync(int idReserva)
    {
        var reserva = await _obtenerPorIdDA.ObtenerPorIdAsync(idReserva);
        if (reserva == null)
        {
            return ResultadoOperacion.Error(Mensajes.NoEncontrado);
        }

        if (reserva.EstadoReserva == Estados.Reserva.Cancelada)
        {
            return ResultadoOperacion.Error(Mensajes.ReservaYaCancelada);
        }

        if (reserva.EstadoReserva is Estados.Reserva.CheckIn or Estados.Reserva.CheckOut)
        {
            return ResultadoOperacion.Error(Mensajes.ReservaYaEmpezo);
        }

        try
        {
            await _eliminarDA.EliminarAsync(idReserva);
            return ResultadoOperacion.Ok(Mensajes.ReservaCancelada);
        }
        catch (Exception)
        {
            return ResultadoOperacion.Error(Mensajes.ErrorCancelarReserva);
        }
    }
}
