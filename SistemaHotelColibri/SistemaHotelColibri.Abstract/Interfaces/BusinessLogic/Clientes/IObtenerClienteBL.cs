using SistemaHotelColibri.Abstract.Modelos.Clientes;

namespace SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Clientes;

public interface IObtenerClienteBL
{
    Task<List<ClienteDto>> ObtenerAsync(string? estado);
}