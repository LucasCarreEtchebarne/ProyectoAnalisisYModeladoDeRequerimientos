using SistemaHotelColibri.Abstract.Modelos.Menus;

namespace SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Menus;

public interface IObtenerMenuPorIdBL
{
    Task<MenuDto?> ObtenerPorIdAsync(int idProductoMenu);
}
