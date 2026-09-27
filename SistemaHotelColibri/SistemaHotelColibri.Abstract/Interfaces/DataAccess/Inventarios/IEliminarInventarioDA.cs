namespace SistemaHotelColibri.Abstract.Interfaces.DataAccess.Inventarios;

public interface IEliminarInventarioDA
{
    Task<bool> EliminarAsync(int idProducto);
}
