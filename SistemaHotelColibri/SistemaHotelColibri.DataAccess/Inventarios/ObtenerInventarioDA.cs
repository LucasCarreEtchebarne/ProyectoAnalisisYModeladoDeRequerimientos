using Microsoft.EntityFrameworkCore;
using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Inventarios;
using SistemaHotelColibri.Abstract.Modelos.Inventarios;
using SistemaHotelColibri.DataAccess.Comun;
using SistemaHotelColibri.DataAccess.Contexto;

namespace SistemaHotelColibri.DataAccess.Inventarios;

public class ObtenerInventarioDA : IObtenerInventarioDA
{
    private readonly HotelColibriContext _contexto;

    public ObtenerInventarioDA(HotelColibriContext contexto)
    {
        _contexto = contexto;
    }

    public async Task<ResultadoPaginado<InventarioDto>> ObtenerAsync(FiltroListado filtro)
    {
        var consulta = _contexto.Inventario.AsNoTracking().ConCategoriaYProveedor();

        if (!string.IsNullOrWhiteSpace(filtro.Estado))
        {
            consulta = consulta.Where(p => p.EstadoProducto == filtro.Estado);
        }

        if (!string.IsNullOrWhiteSpace(filtro.Busqueda))
        {
            var texto = filtro.Busqueda;
            consulta = consulta.Where(p => p.NombreProducto.Contains(texto)
                                        || p.IdCategoriaNavigation.NombreCategoria.Contains(texto)
                                        || p.IdCategoriaNavigation.Area.Contains(texto)
                                        || (p.Descripcion != null && p.Descripcion.Contains(texto))
                                        || (p.IdProveedorNavigation != null && p.IdProveedorNavigation.NombreProveedor.Contains(texto)));
        }

        var pagina = await consulta
            .OrderBy(p => p.NombreProducto)
            .PaginarAsync(filtro.Pagina, filtro.TamanoPagina);

        return pagina.Mapear(p => p.ADto());
    }
}
