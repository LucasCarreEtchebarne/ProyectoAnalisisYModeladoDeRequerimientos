using SistemaHotelColibri.Abstract.Comun;

namespace SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Clientes;

public interface IEliminarClienteBL
{
    Task<ResultadoOperacion> EliminarAsync(int idCliente);
}