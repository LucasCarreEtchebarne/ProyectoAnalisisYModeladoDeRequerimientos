using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Modelos.Menus;

namespace SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Menus;

public interface IAgregarMenuBL
{
    Task<ResultadoOperacion> AgregarAsync(MenuDto producto);
}
