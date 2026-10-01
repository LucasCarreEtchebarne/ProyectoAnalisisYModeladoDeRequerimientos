namespace SistemaHotelColibri.Abstract.Interfaces.DataAccess.Clientes;

public interface IEliminarClienteDA
{
    Task<bool> EliminarAsync(int idCliente);
}