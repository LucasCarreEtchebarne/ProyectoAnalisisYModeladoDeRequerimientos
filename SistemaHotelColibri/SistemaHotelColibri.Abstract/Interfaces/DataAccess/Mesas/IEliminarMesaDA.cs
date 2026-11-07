namespace SistemaHotelColibri.Abstract.Interfaces.DataAccess.Mesas;

public interface IEliminarMesaDA
{
    Task<bool> EliminarAsync(int idMesa);
}
