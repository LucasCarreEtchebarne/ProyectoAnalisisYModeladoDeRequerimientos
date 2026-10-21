using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Modelos.Inventarios;

namespace SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Inventarios;

public interface IObtenerInventarioBL
{
    Task<ResultadoPaginado<InventarioDto>> ObtenerAsync(FiltroListado filtro);
}
