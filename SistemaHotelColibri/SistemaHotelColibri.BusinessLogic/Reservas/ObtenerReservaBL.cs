using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Reservas;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Reservas;
using SistemaHotelColibri.Abstract.Modelos.Reservas;

namespace SistemaHotelColibri.BusinessLogic.Reservas;

public class ObtenerReservaBL : IObtenerReservaBL
{
    private readonly IObtenerReservaDA _da;

    public ObtenerReservaBL(IObtenerReservaDA da)
    {
        _da = da;
    }

    public Task<ResultadoPaginado<ReservaDto>> ObtenerAsync(FiltroListado filtro)
    {
        filtro.Busqueda = filtro.Busqueda?.Trim();
        return _da.ObtenerAsync(filtro);
    }
}

public class ObtenerReservaPorIdBL : IObtenerReservaPorIdBL
{
    private readonly IObtenerReservaPorIdDA _da;

    public ObtenerReservaPorIdBL(IObtenerReservaPorIdDA da)
    {
        _da = da;
    }

    public Task<ReservaDto?> ObtenerPorIdAsync(int idReserva) => _da.ObtenerPorIdAsync(idReserva);
}

public class ObtenerOpcionesReservaBL : IObtenerOpcionesReservaBL
{
    private readonly IObtenerOpcionesReservaDA _da;

    public ObtenerOpcionesReservaBL(IObtenerOpcionesReservaDA da)
    {
        _da = da;
    }

    public Task<OpcionesReservaDto> ObtenerAsync(int idClienteActual = 0, int idHabitacionActual = 0) =>
        _da.ObtenerAsync(idClienteActual, idHabitacionActual);
}
