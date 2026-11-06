using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Reservas;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Reservas;
using SistemaHotelColibri.Abstract.Modelos.Reservas;

namespace SistemaHotelColibri.BusinessLogic.Reservas;

public class ObtenerReservaBL : IObtenerReservaBL
{
    private readonly IObtenerReservaDA _obtenerDA;

    public ObtenerReservaBL(IObtenerReservaDA obtenerDA)
    {
        _obtenerDA = obtenerDA;
    }

    public Task<ResultadoPaginado<ReservaDto>> ObtenerAsync(FiltroListado filtro)
    {
        filtro.Busqueda = filtro.Busqueda?.Trim();
        return _obtenerDA.ObtenerAsync(filtro);
    }
}
