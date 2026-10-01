using SistemaHotelColibri.Abstract.Modelos.Clientes;

namespace SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Clientes;

public interface IObtenerClientePorIdBL
{
    Task<ClienteDto?> ObtenerPorIdAsync(int idCliente);
}