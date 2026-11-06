using SistemaHotelColibri.Abstract.Modelos.Menus;

namespace SistemaHotelColibri.Abstract.Interfaces.DataAccess.Menus;

public interface IAgregarMenuDA
{
    Task<int> AgregarAsync(MenuDto producto);
}
