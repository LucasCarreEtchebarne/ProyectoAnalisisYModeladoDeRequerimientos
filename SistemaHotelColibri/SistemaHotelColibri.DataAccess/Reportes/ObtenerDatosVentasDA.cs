using Microsoft.EntityFrameworkCore;
using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Reportes;
using SistemaHotelColibri.Abstract.Modelos.Reportes;
using SistemaHotelColibri.DataAccess.Contexto;

namespace SistemaHotelColibri.DataAccess.Reportes;

// REP-002: líneas de los pedidos facturados en el rango (por fecha de la factura, sin facturas anuladas).
public class ObtenerDatosVentasDA : IObtenerDatosVentasDA
{
    private readonly HotelColibriContext _contexto;

    public ObtenerDatosVentasDA(HotelColibriContext contexto)
    {
        _contexto = contexto;
    }

    public async Task<List<LineaVentaDto>> ObtenerAsync(FiltroReporte filtro)
    {
        var desde = filtro.Desde.ToDateTime(TimeOnly.MinValue);
        var hastaExclusivo = filtro.Hasta.AddDays(1).ToDateTime(TimeOnly.MinValue);

        var pedidos = await _contexto.Pedido.AsNoTracking()
            .Where(p => p.DetalleFactura.Any(d => d.IdFacturaNavigation.EstadoFactura != Estados.Factura.Anulada
                                               && d.IdFacturaNavigation.FechaEmision >= desde
                                               && d.IdFacturaNavigation.FechaEmision < hastaExclusivo))
            .Select(p => new
            {
                p.IdPedido,
                p.TipoPedido,
                FechaFactura = p.DetalleFactura
                    .Where(d => d.IdFacturaNavigation.EstadoFactura != Estados.Factura.Anulada)
                    .Min(d => d.IdFacturaNavigation.FechaEmision),
                Lineas = p.DetallePedido.Select(d => new
                {
                    d.IdProductoMenuNavigation.NombreProducto,
                    d.IdProductoMenuNavigation.CategoriaMenu,
                    d.Cantidad,
                    d.PrecioUnitario
                })
            })
            .ToListAsync();

        return pedidos
            .SelectMany(p => p.Lineas.Select(l => new LineaVentaDto
            {
                IdPedido = p.IdPedido,
                FechaFactura = p.FechaFactura,
                TipoPedido = p.TipoPedido,
                Producto = l.NombreProducto,
                Categoria = l.CategoriaMenu,
                Cantidad = l.Cantidad,
                PrecioUnitario = l.PrecioUnitario
            }))
            .ToList();
    }
}
