using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Modelos.Mesas;

namespace SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Mesas;

public interface IObtenerMesaBL
{
    Task<ResultadoPaginado<MesaDto>> ObtenerAsync(FiltroListado filtro);
}
