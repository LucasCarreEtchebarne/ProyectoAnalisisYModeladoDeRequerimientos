using SistemaHotelColibri.Abstract.Modelos.Clientes;

namespace SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Clientes;

public interface IBuscarClienteBL
{
    Task<List<ClienteDto>> BuscarAsync(string? texto, string? estado);
}