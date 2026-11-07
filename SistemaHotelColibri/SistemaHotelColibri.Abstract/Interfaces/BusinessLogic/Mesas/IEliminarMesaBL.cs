using SistemaHotelColibri.Abstract.Comun;

namespace SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Mesas;

public interface IEliminarMesaBL
{
    Task<ResultadoOperacion> EliminarAsync(int idMesa);
}
