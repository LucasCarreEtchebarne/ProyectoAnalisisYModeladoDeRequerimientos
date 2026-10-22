using SistemaHotelColibri.Abstract.Modelos.Eventos;

namespace SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Eventos;

public interface IObtenerEspacioEventoBL
{
    Task<List<EspacioEventoDto>> ObtenerAsync(string? estado);
}
