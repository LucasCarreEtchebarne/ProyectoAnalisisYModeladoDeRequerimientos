using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Reservas;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Reservas;
using SistemaHotelColibri.Abstract.Modelos.Reservas;

namespace SistemaHotelColibri.BusinessLogic.Reservas;

public class ObtenerReservaPorIdBL : IObtenerReservaPorIdBL
{
    private readonly IObtenerReservaPorIdDA _obtenerPorIdDA;

    public ObtenerReservaPorIdBL(IObtenerReservaPorIdDA obtenerPorIdDA)
    {
        _obtenerPorIdDA = obtenerPorIdDA;
    }

    public Task<ReservaDto?> ObtenerPorIdAsync(int idReserva) => _obtenerPorIdDA.ObtenerPorIdAsync(idReserva);
}
