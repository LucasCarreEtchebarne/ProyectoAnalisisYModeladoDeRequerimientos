using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Modelos.Menus;

namespace SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Menus;

public interface IEditarMenuBL
{
    Task<ResultadoOperacion> EditarAsync(MenuDto producto);
}
