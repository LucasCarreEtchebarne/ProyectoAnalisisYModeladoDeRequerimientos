using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Modelos.Eventos;

namespace SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Eventos;

public interface IEditarEventoBL
{
    Task<ResultadoOperacion> EditarAsync(EventoDto evento);
}
