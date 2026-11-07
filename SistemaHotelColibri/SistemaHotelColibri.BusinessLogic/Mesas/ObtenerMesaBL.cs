using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Mesas;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Mesas;
using SistemaHotelColibri.Abstract.Modelos.Mesas;

namespace SistemaHotelColibri.BusinessLogic.Mesas;

public class ObtenerMesaBL : IObtenerMesaBL
{
    private readonly IObtenerMesaDA _obtenerDA;

    public ObtenerMesaBL(IObtenerMesaDA obtenerDA)
    {
        _obtenerDA = obtenerDA;
    }

    public Task<ResultadoPaginado<MesaDto>> ObtenerAsync(FiltroListado filtro)
    {
        filtro.Busqueda = string.IsNullOrWhiteSpace(filtro.Busqueda) ? null : filtro.Busqueda.Trim();
        return _obtenerDA.ObtenerAsync(filtro);
    }
}
