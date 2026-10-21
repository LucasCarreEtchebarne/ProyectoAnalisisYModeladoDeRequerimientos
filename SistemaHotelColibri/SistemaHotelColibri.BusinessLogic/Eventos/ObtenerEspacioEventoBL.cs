using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Eventos;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Eventos;
using SistemaHotelColibri.Abstract.Modelos.Eventos;

namespace SistemaHotelColibri.BusinessLogic.Eventos;

public class ObtenerEspacioEventoBL : IObtenerEspacioEventoBL
{
    private readonly IObtenerEspacioEventoDA _obtenerDA;

    public ObtenerEspacioEventoBL(IObtenerEspacioEventoDA obtenerDA)
    {
        _obtenerDA = obtenerDA;
    }

    public Task<List<EspacioEventoDto>> ObtenerAsync(string? estado)
    {
        return _obtenerDA.ObtenerAsync(estado);
    }
}
