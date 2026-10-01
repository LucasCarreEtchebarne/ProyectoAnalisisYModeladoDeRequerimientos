using SistemaHotelColibri.Abstract.Modelos.Clientes;

namespace SistemaHotelColibri.Abstract.Interfaces.DataAccess.Clientes;

public interface IAgregarClienteDA
{
    Task<int> AgregarAsync(ClienteDto cliente);
}