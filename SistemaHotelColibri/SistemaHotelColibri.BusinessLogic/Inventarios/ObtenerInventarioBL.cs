using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Inventarios;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Inventarios;
using SistemaHotelColibri.Abstract.Modelos.Inventarios;

namespace SistemaHotelColibri.BusinessLogic.Inventarios;

public class ObtenerInventarioBL : IObtenerInventarioBL
{
    private readonly IObtenerInventarioDA _obtenerDA;

    public ObtenerInventarioBL(IObtenerInventarioDA obtenerDA)
    {
        _obtenerDA = obtenerDA;
    }

    public Task<ResultadoPaginado<InventarioDto>> ObtenerAsync(FiltroListado filtro)
    {
        filtro.Busqueda = string.IsNullOrWhiteSpace(filtro.Busqueda) ? null : filtro.Busqueda.Trim();
        return _obtenerDA.ObtenerAsync(filtro);
    }
}
