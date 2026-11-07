using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Modelos.Mesas;

namespace SistemaHotelColibri.Abstract.Interfaces.DataAccess.Mesas;

public interface IObtenerMesaDA
{
    Task<ResultadoPaginado<MesaDto>> ObtenerAsync(FiltroListado filtro);
}
