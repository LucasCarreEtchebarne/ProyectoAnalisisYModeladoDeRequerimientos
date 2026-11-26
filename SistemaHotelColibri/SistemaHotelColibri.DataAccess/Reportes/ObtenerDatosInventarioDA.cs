using Microsoft.EntityFrameworkCore;
using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Reportes;
using SistemaHotelColibri.Abstract.Modelos.Reportes;
using SistemaHotelColibri.DataAccess.Contexto;
using SistemaHotelColibri.DataAccess.Inventarios;

namespace SistemaHotelColibri.DataAccess.Reportes;

// REP-004: productos activos del inventario y sus movimientos dentro del rango.
public class ObtenerDatosInventarioDA : IObtenerDatosInventarioDA
{
    private readonly HotelColibriContext _contexto;

    public ObtenerDatosInventarioDA(HotelColibriContext contexto)
    {
        _contexto = contexto;
    }

    public async Task<DatosInventarioDto> ObtenerAsync(FiltroReporte filtro)
    {
        var productos = _contexto.Inventario.AsNoTracking()
            .ConCategoriaYProveedor()
            .Where(p => p.EstadoProducto == Estados.ProductoInventario.Activo);

        if (!string.IsNullOrWhiteSpace(filtro.Area))
        {
            productos = productos.Where(p => p.IdCategoriaNavigation.Area == filtro.Area);
        }

        if (!string.IsNullOrWhiteSpace(filtro.Busqueda))
        {
            var texto = filtro.Busqueda.Trim();
            productos = productos.Where(p => p.NombreProducto.Contains(texto)
                                          || p.IdCategoriaNavigation.NombreCategoria.Contains(texto));
        }

        if (filtro.SoloStockBajo)
        {
            productos = productos.Where(p => p.Stock <= p.StockMinimo);
        }

        var lista = await productos
            .OrderBy(p => p.IdCategoriaNavigation.NombreCategoria)
            .ThenBy(p => p.NombreProducto)
            .ToListAsync();

        var ids = lista.Select(p => p.IdProducto).ToList();
        var desde = filtro.Desde.ToDateTime(TimeOnly.MinValue);
        var hastaExclusivo = filtro.Hasta.AddDays(1).ToDateTime(TimeOnly.MinValue);

        var movimientos = await _contexto.MovimientoInventario.AsNoTracking()
            .Where(m => ids.Contains(m.IdProducto) && m.FechaHora >= desde && m.FechaHora < hastaExclusivo)
            .GroupBy(m => new { m.IdProducto, m.TipoMovimiento })
            .Select(g => new MovimientoReporteDto
            {
                IdProducto = g.Key.IdProducto,
                TipoMovimiento = g.Key.TipoMovimiento,
                Cantidad = g.Sum(m => m.Cantidad)
            })
            .ToListAsync();

        return new DatosInventarioDto
        {
            Productos = lista.Select(p => p.ADto()).ToList(),
            Movimientos = movimientos
        };
    }
}
