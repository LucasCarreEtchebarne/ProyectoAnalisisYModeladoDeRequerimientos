using SistemaHotelColibri.Abstract.Modelos.Clientes;

namespace SistemaHotelColibri.Abstract.Interfaces.DataAccess.Clientes;

public interface IObtenerClienteDA
{
    Task<List<ClienteDto>> ObtenerAsync(string? estado);
}