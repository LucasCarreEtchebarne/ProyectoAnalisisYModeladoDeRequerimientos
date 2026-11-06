using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Menus;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Menus;
using SistemaHotelColibri.Abstract.Modelos.Menus;

namespace SistemaHotelColibri.BusinessLogic.Menus;

public class ObtenerMenuBL : IObtenerMenuBL
{
    private readonly IObtenerMenuDA _obtenerDA;

    public ObtenerMenuBL(IObtenerMenuDA obtenerDA)
    {
        _obtenerDA = obtenerDA;
    }

    public Task<ResultadoPaginado<MenuDto>> ObtenerAsync(FiltroListado filtro)
    {
        filtro.Busqueda = string.IsNullOrWhiteSpace(filtro.Busqueda) ? null : filtro.Busqueda.Trim();
        filtro.Categoria = string.IsNullOrWhiteSpace(filtro.Categoria) ? null : filtro.Categoria.Trim();
        return _obtenerDA.ObtenerAsync(filtro);
    }
}
