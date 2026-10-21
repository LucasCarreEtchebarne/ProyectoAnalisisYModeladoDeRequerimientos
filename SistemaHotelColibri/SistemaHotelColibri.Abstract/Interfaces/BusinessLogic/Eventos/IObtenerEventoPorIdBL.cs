using SistemaHotelColibri.Abstract.Modelos.Eventos;

namespace SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Eventos;

public interface IObtenerEventoPorIdBL
{
    Task<EventoDto?> ObtenerPorIdAsync(int idEvento);
}
