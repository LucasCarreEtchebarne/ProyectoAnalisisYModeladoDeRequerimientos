using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Menus;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Menus;
using SistemaHotelColibri.Abstract.Modelos.Menus;

namespace SistemaHotelColibri.BusinessLogic.Menus;

public class ObtenerMenuPorIdBL : IObtenerMenuPorIdBL
{
    private readonly IObtenerMenuPorIdDA _obtenerPorIdDA;

    public ObtenerMenuPorIdBL(IObtenerMenuPorIdDA obtenerPorIdDA)
    {
        _obtenerPorIdDA = obtenerPorIdDA;
    }

    public Task<MenuDto?> ObtenerPorIdAsync(int idProductoMenu)
    {
        return _obtenerPorIdDA.ObtenerPorIdAsync(idProductoMenu);
    }
}
