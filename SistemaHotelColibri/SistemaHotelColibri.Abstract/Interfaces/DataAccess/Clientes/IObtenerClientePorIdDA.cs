using SistemaHotelColibri.Abstract.Modelos.Clientes;

namespace SistemaHotelColibri.Abstract.Interfaces.DataAccess.Clientes;

public interface IObtenerClientePorIdDA
{
    Task<ClienteDto?> ObtenerPorIdAsync(int idCliente);
}