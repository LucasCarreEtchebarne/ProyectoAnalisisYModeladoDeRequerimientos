using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Modelos.Reservas;

namespace SistemaHotelColibri.Abstract.Interfaces.DataAccess.Reservas;

public interface IObtenerReservaDA
{
    Task<ResultadoPaginado<ReservaDto>> ObtenerAsync(FiltroListado filtro);
}
