using SistemaHotelColibri.Abstract.Modelos.Clientes;

namespace SistemaHotelColibri.Abstract.Interfaces.DataAccess.Clientes;

public interface IBuscarClienteDA
{
    Task<List<ClienteDto>> BuscarAsync(string texto, string? estado);
}