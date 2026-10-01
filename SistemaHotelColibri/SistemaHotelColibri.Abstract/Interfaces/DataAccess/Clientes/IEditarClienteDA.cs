using SistemaHotelColibri.Abstract.Modelos.Clientes;

namespace SistemaHotelColibri.Abstract.Interfaces.DataAccess.Clientes;

public interface IEditarClienteDA
{
    Task<bool> EditarAsync(ClienteDto cliente);
}