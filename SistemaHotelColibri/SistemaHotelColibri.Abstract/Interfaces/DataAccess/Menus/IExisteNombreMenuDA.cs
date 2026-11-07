namespace SistemaHotelColibri.Abstract.Interfaces.DataAccess.Menus;

public interface IExisteNombreMenuDA
{
    Task<bool> ExisteNombreAsync(string nombreProducto, int idExcluir);
}
