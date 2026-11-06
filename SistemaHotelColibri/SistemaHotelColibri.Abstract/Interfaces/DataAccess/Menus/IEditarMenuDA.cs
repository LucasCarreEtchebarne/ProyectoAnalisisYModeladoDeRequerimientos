using SistemaHotelColibri.Abstract.Modelos.Menus;

namespace SistemaHotelColibri.Abstract.Interfaces.DataAccess.Menus;

public interface IEditarMenuDA
{
    Task<bool> EditarAsync(MenuDto producto);
}
