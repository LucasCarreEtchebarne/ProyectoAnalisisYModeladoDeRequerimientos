using SistemaHotelColibri.Abstract.Modelos.Eventos;

namespace SistemaHotelColibri.Abstract.Interfaces.DataAccess.Eventos;

public interface IObtenerEventoPorIdDA
{
    Task<EventoDto?> ObtenerPorIdAsync(int idEvento);
}
