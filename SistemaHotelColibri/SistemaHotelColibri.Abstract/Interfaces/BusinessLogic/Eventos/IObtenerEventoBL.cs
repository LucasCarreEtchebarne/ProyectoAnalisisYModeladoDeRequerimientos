using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Modelos.Eventos;

namespace SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Eventos;

public interface IObtenerEventoBL
{
    Task<ResultadoPaginado<EventoDto>> ObtenerAsync(FiltroListado filtro);
}
