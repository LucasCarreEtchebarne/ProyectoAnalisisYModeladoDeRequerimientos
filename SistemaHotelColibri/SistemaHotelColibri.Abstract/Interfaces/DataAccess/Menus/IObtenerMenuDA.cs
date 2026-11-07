using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Modelos.Menus;

namespace SistemaHotelColibri.Abstract.Interfaces.DataAccess.Menus;

public interface IObtenerMenuDA
{
    Task<ResultadoPaginado<MenuDto>> ObtenerAsync(FiltroListado filtro);
}
