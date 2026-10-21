using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Modelos.Eventos;

namespace SistemaHotelColibri.Abstract.Interfaces.DataAccess.Eventos;

public interface IObtenerEventoDA
{
    Task<ResultadoPaginado<EventoDto>> ObtenerAsync(FiltroListado filtro);
}
