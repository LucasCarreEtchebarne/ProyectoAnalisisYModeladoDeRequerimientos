using SistemaHotelColibri.Abstract.Comun;

namespace SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Eventos;

public interface IEliminarEventoBL
{
    Task<ResultadoOperacion> EliminarAsync(int idEvento);
}
