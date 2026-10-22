using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Eventos;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Eventos;
using SistemaHotelColibri.Abstract.Modelos.Eventos;

namespace SistemaHotelColibri.BusinessLogic.Eventos;

public class ObtenerEventoBL : IObtenerEventoBL
{
    private readonly IObtenerEventoDA _obtenerDA;

    public ObtenerEventoBL(IObtenerEventoDA obtenerDA)
    {
        _obtenerDA = obtenerDA;
    }

    public Task<ResultadoPaginado<EventoDto>> ObtenerAsync(FiltroListado filtro)
    {
        filtro.Busqueda = string.IsNullOrWhiteSpace(filtro.Busqueda) ? null : filtro.Busqueda.Trim();
        return _obtenerDA.ObtenerAsync(filtro);
    }
}
