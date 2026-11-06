using Microsoft.EntityFrameworkCore;
using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Menus;
using SistemaHotelColibri.Abstract.Modelos.Menus;
using SistemaHotelColibri.DataAccess.Comun;
using SistemaHotelColibri.DataAccess.Contexto;

namespace SistemaHotelColibri.DataAccess.Menus;

public class ObtenerMenuDA : IObtenerMenuDA
{
    private readonly HotelColibriContext _contexto;

    public ObtenerMenuDA(HotelColibriContext contexto)
    {
        _contexto = contexto;
    }

    public async Task<ResultadoPaginado<MenuDto>> ObtenerAsync(FiltroListado filtro)
    {
        var consulta = _contexto.Menu.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(filtro.Estado))
        {
            consulta = consulta.Where(p => p.EstadoProductoMenu == filtro.Estado);
        }

        if (!string.IsNullOrWhiteSpace(filtro.Categoria))
        {
            consulta = consulta.Where(p => p.CategoriaMenu == filtro.Categoria);
        }

        if (!string.IsNullOrWhiteSpace(filtro.Busqueda))
        {
            var texto = filtro.Busqueda;
            consulta = consulta.Where(p => p.NombreProducto.Contains(texto)
                                        || p.CategoriaMenu.Contains(texto)
                                        || (p.Descripcion != null && p.Descripcion.Contains(texto)));
        }

        var pagina = await consulta
            .OrderBy(p => p.CategoriaMenu)
            .ThenBy(p => p.NombreProducto)
            .PaginarAsync(filtro.Pagina, filtro.TamanoPagina);

        return pagina.Mapear(p => p.ADto());
    }
}
