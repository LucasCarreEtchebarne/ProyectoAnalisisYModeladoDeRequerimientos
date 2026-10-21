using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Reservas;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Reservas;

namespace SistemaHotelColibri.BusinessLogic.Reservas;

public class EliminarReservaBL : IEliminarReservaBL
{
    private readonly IEliminarReservaDA _da;
    private readonly IObtenerReservaPorIdDA _obtenerDA;

    public EliminarReservaBL(IEliminarReservaDA da, IObtenerReservaPorIdDA obtenerDA)
    {
        _da = da;
        _obtenerDA = obtenerDA;
    }

    public async Task<ResultadoOperacion> EliminarAsync(int idReserva)
    {
        var reserva = await _obtenerDA.ObtenerPorIdAsync(idReserva);
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
            await _da.EliminarAsync(idReserva);
            return ResultadoOperacion.Ok(Mensajes.ReservaCancelada);
        }
        catch (Exception)
        {
            return ResultadoOperacion.Error(Mensajes.ErrorCancelarReserva);
        }
    }
}
