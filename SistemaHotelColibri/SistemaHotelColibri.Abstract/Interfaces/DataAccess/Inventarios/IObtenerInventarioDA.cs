using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Modelos.Inventarios;

namespace SistemaHotelColibri.Abstract.Interfaces.DataAccess.Inventarios;

public interface IObtenerInventarioDA
{
    Task<ResultadoPaginado<InventarioDto>> ObtenerAsync(FiltroListado filtro);
}
