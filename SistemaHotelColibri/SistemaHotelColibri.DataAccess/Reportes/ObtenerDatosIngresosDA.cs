using Microsoft.EntityFrameworkCore;
using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Reportes;
using SistemaHotelColibri.Abstract.Modelos.Reportes;
using SistemaHotelColibri.DataAccess.Contexto;
using SistemaHotelColibri.DataAccess.Facturas;

namespace SistemaHotelColibri.DataAccess.Reportes;

// REP-003: facturas emitidas en el rango (con sus líneas y pagos) y los pagos aprobados hechos en el rango.
public class ObtenerDatosIngresosDA : IObtenerDatosIngresosDA
{
    private readonly HotelColibriContext _contexto;

    public ObtenerDatosIngresosDA(HotelColibriContext contexto)
    {
        _contexto = contexto;
    }

    public async Task<DatosIngresosDto> ObtenerAsync(FiltroReporte filtro)
    {
        var desde = filtro.Desde.ToDateTime(TimeOnly.MinValue);
        var hastaExclusivo = filtro.Hasta.AddDays(1).ToDateTime(TimeOnly.MinValue);

        var facturas = await _contexto.Factura.AsNoTracking()
            .Include(f => f.IdClienteNavigation)
            .Include(f => f.DetalleFactura)
            .Include(f => f.Pago)
            .Where(f => f.FechaEmision >= desde && f.FechaEmision < hastaExclusivo)
            .OrderBy(f => f.FechaEmision)
            .ToListAsync();

        var pagos = await _contexto.Pago.AsNoTracking()
            .Where(p => p.EstadoPago == Estados.Pago.Aprobado
                     && p.FechaHoraPago >= desde && p.FechaHoraPago < hastaExclusivo
                     && p.IdFacturaNavigation.EstadoFactura != Estados.Factura.Anulada)
            .ToListAsync();

        return new DatosIngresosDto
        {
            Facturas = facturas.Select(f => f.ADto()).ToList(),
            Pagos = pagos.Select(p => p.ADto()).ToList()
        };
    }
}
