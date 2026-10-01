using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Modelos.Clientes;

namespace SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Clientes;

public interface IEditarClienteBL
{
    Task<ResultadoOperacion> EditarAsync(ClienteDto cliente);
}