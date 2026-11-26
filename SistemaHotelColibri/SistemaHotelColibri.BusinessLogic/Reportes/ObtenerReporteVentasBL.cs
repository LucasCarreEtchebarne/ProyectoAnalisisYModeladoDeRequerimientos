using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Reportes;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Reportes;
using SistemaHotelColibri.Abstract.Modelos.Reportes;

namespace SistemaHotelColibri.BusinessLogic.Reportes;

// REP-002: ventas del restaurante a partir de los pedidos ya facturados.
public class ObtenerReporteVentasBL : IObtenerReporteVentasBL
{
    private readonly IObtenerDatosVentasDA _datosDA;

    public ObtenerReporteVentasBL(IObtenerDatosVentasDA datosDA)
    {
        _datosDA = datosDA;
    }

    public async Task<ReporteVentasDto> ObtenerAsync(FiltroReporte filtro)
    {
        ReglasReporte.Normalizar(filtro);
        var lineas = await _datosDA.ObtenerAsync(filtro);

        decimal Monto(LineaVentaDto l) => Facturacion.Redondear(l.Cantidad * l.PrecioUnitario);
        var ventas = lineas.Sum(Monto);

        var productos = lineas
            .GroupBy(l => new { l.Producto, l.Categoria })
            .Select(g => new VentaProductoReporteDto
            {
                Producto = g.Key.Producto,
                Categoria = g.Key.Categoria,
                Cantidad = g.Sum(l => l.Cantidad),
                Monto = g.Sum(Monto),
                Porcentaje = ReglasReporte.Porcentaje(g.Sum(Monto), ventas)
            })
            .OrderByDescending(p => p.Monto)
            .ThenByDescending(p => p.Cantidad)
            .ToList();

        List<VentaGrupoReporteDto> Agrupar(Func<LineaVentaDto, string> clave) => lineas
            .GroupBy(clave)
            .Select(g => new VentaGrupoReporteDto
            {
                Nombre = g.Key,
                Pedidos = g.Select(l => l.IdPedido).Distinct().Count(),
                Cantidad = g.Sum(l => l.Cantidad),
                Monto = g.Sum(Monto),
                Porcentaje = ReglasReporte.Porcentaje(g.Sum(Monto), ventas)
            })
            .OrderByDescending(g => g.Monto)
            .ToList();

        return new ReporteVentasDto
        {
            Desde = filtro.Desde,
            Hasta = filtro.Hasta,
            CantidadPedidos = lineas.Select(l => l.IdPedido).Distinct().Count(),
            ProductosVendidos = lineas.Sum(l => l.Cantidad),
            Ventas = ventas,
            Servicio = Facturacion.Redondear(ventas * Facturacion.TasaServicio),
            Productos = productos,
            PorCategoria = Agrupar(l => l.Categoria),
            PorOrigen = Agrupar(l => NombreOrigen(l.TipoPedido)),
            PorDia = lineas
                .GroupBy(l => DateOnly.FromDateTime(l.FechaFactura))
                .Select(g => new VentaDiaReporteDto
                {
                    Fecha = g.Key,
                    Pedidos = g.Select(l => l.IdPedido).Distinct().Count(),
                    Monto = g.Sum(Monto)
                })
                .OrderBy(d => d.Fecha)
                .ToList()
        };
    }

    private static string NombreOrigen(string tipoPedido) => tipoPedido switch
    {
        Catalogos.TipoPedido.Mesa => "Mesas",
        Catalogos.TipoPedido.Habitacion => "Servicio a la habitación",
        Catalogos.TipoPedido.Llevar => "Para llevar",
        _ => tipoPedido
    };
}
