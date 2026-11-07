using SistemaHotelColibri.Abstract.Modelos.Menus;

namespace SistemaHotelColibri.Abstract.Interfaces.DataAccess.Menus;

public interface IObtenerMenuPorIdDA
{
    Task<MenuDto?> ObtenerPorIdAsync(int idProductoMenu);
}
