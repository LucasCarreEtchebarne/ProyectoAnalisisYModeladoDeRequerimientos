using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Reportes;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Reportes;
using SistemaHotelColibri.Abstract.Modelos.Reportes;

namespace SistemaHotelColibri.BusinessLogic.Reportes;

// REP-003: lo facturado (hospedaje, restaurante y eventos), lo cobrado y lo pendiente en el rango.
public class ObtenerReporteIngresosBL : IObtenerReporteIngresosBL
{
    private readonly IObtenerDatosIngresosDA _datosDA;

    public ObtenerReporteIngresosBL(IObtenerDatosIngresosDA datosDA)
    {
        _datosDA = datosDA;
    }

    public async Task<ReporteIngresosDto> ObtenerAsync(FiltroReporte filtro)
    {
        ReglasReporte.Normalizar(filtro);
        var datos = await _datosDA.ObtenerAsync(filtro);

        // Las facturas anuladas se cuentan aparte y no suman a los ingresos.
        var vigentes = datos.Facturas.Where(f => f.EstadoFactura != Estados.Factura.Anulada).ToList();
        var subtotal = vigentes.Sum(f => f.Subtotal);
        var totalCobrado = datos.Pagos.Sum(p => p.MontoPagado);

        var porConcepto = vigentes
            .SelectMany(f => f.Detalles)
            .GroupBy(d => d.TipoConcepto)
            .Select(g => new IngresoConceptoReporteDto
            {
                Concepto = g.Key,
                Lineas = g.Count(),
                Monto = g.Sum(d => d.Subtotal),
                Porcentaje = ReglasReporte.Porcentaje(g.Sum(d => d.Subtotal), subtotal)
            })
            .OrderByDescending(c => c.Monto)
            .ToList();

        var porMetodo = datos.Pagos
            .GroupBy(p => p.MetodoPago)
            .Select(g => new IngresoMetodoReporteDto
            {
                MetodoPago = g.Key,
                Pagos = g.Count(),
                Monto = g.Sum(p => p.MontoPagado),
                Porcentaje = ReglasReporte.Porcentaje(g.Sum(p => p.MontoPagado), totalCobrado)
            })
            .OrderByDescending(m => m.Monto)
            .ToList();

        var facturadoPorDia = vigentes
            .GroupBy(f => DateOnly.FromDateTime(f.FechaEmision))
            .ToDictionary(g => g.Key, g => g.Sum(f => f.MontoTotal));
        var cobradoPorDia = datos.Pagos
            .GroupBy(p => DateOnly.FromDateTime(p.FechaHoraPago))
            .ToDictionary(g => g.Key, g => g.Sum(p => p.MontoPagado));

        var porDia = facturadoPorDia.Keys.Union(cobradoPorDia.Keys)
            .Order()
            .Select(fecha => new IngresoDiaReporteDto
            {
                Fecha = fecha,
                Facturado = facturadoPorDia.GetValueOrDefault(fecha),
                Cobrado = cobradoPorDia.GetValueOrDefault(fecha)
            })
            .ToList();

        return new ReporteIngresosDto
        {
            Desde = filtro.Desde,
            Hasta = filtro.Hasta,
            FacturasEmitidas = vigentes.Count,
            FacturasAnuladas = datos.Facturas.Count - vigentes.Count,
            Subtotal = subtotal,
            Servicio = vigentes.Sum(f => f.MontoServicio),
            Iva = vigentes.Sum(f => f.MontoIva),
            TotalFacturado = vigentes.Sum(f => f.MontoTotal),
            TotalCobrado = totalCobrado,
            PendientePorCobrar = vigentes.Sum(f => f.Saldo),
            PorConcepto = porConcepto,
            PorMetodoPago = porMetodo,
            PorDia = porDia,
            Facturas = datos.Facturas.OrderBy(f => f.FechaEmision).ToList()
        };
    }
}
