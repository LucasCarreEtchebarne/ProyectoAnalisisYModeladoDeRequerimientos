namespace SistemaHotelColibri.Abstract.Interfaces.DataAccess.Clientes;

public interface IExisteCorreoClienteDA
{
    Task<bool> ExisteCorreoAsync(string correoElectronico, int idExcluir);
}
