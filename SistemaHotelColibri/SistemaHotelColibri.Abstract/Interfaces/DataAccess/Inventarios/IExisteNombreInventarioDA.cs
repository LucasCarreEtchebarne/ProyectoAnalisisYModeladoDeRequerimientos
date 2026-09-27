namespace SistemaHotelColibri.Abstract.Interfaces.DataAccess.Inventarios;

public interface IExisteNombreInventarioDA
{
    Task<bool> ExisteNombreAsync(string nombreProducto, int idExcluir);
}
