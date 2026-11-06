namespace SistemaHotelColibri.Abstract.Interfaces.DataAccess.Menus;

public interface IEliminarMenuDA
{
    Task<bool> EliminarAsync(int idProductoMenu);
}
