using SistemaHotelColibri.Abstract.Modelos.Eventos;

namespace SistemaHotelColibri.Abstract.Interfaces.DataAccess.Eventos;

public interface IObtenerEspacioEventoDA
{
    Task<List<EspacioEventoDto>> ObtenerAsync(string? estado);
}
