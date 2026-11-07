using SistemaHotelColibri.Abstract.Comun;

namespace SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Menus;

public interface IEliminarMenuBL
{
    Task<ResultadoOperacion> EliminarAsync(int idProductoMenu);
}
