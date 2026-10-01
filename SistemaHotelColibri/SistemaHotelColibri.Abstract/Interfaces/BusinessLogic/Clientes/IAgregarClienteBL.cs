using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Modelos.Clientes;

namespace SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Clientes;

public interface IAgregarClienteBL
{
    Task<ResultadoOperacion> AgregarAsync(ClienteDto cliente);
}