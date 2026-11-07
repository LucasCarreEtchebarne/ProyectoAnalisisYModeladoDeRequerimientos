using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Modelos.Menus;

namespace SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Menus;

public interface IObtenerMenuBL
{
    Task<ResultadoPaginado<MenuDto>> ObtenerAsync(FiltroListado filtro);
}
