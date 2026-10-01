namespace SistemaHotelColibri.Abstract.Interfaces.DataAccess.Clientes;

public interface IExisteIdentificacionClienteDA
{
    Task<bool> ExisteIdentificacionAsync(string identificacion, int idExcluir);
}