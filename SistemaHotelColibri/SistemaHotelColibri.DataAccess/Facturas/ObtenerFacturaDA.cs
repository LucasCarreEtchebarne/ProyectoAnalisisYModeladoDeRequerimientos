using Microsoft.EntityFrameworkCore;
using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Facturas;
using SistemaHotelColibri.Abstract.Modelos.Facturas;
using SistemaHotelColibri.DataAccess.Comun;
using SistemaHotelColibri.DataAccess.Contexto;

namespace SistemaHotelColibri.DataAccess.Facturas;

public class ObtenerFacturaDA : IObtenerFacturaDA
{
    private readonly HotelColibriContext _contexto;

    public ObtenerFacturaDA(HotelColibriContext contexto)
    {
        _contexto = contexto;
    }

    public async Task<ResultadoPaginado<FacturaDto>> ObtenerAsync(FiltroFactura filtro)
    {
        var consulta = _contexto.Factura.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(filtro.Estado))
        {
            consulta = consulta.Where(f => f.EstadoFactura == filtro.Estado);
        }

        if (!string.IsNullOrWhiteSpace(filtro.Tipo))
        {
            consulta = consulta.Where(f => f.TipoFactura == filtro.Tipo);
        }

        if (filtro.Desde.HasValue)
        {
            var desde = filtro.Desde.Value.ToDateTime(TimeOnly.MinValue);
            consulta = consulta.Where(f => f.FechaEmision >= desde);
        }

        if (filtro.Hasta.HasValue)
        {
            var hasta = filtro.Hasta.Value.AddDays(1).ToDateTime(TimeOnly.MinValue);
            consulta = consulta.Where(f => f.FechaEmision < hasta);
        }

        if (!string.IsNullOrWhiteSpace(filtro.Busqueda))
        {
            var texto = filtro.Busqueda;
            consulta = consulta.Where(f =>
                (f.NumeroFactura != null && f.NumeroFactura.Contains(texto)) ||
                (f.IdClienteNavigation != null &&
                 (f.IdClienteNavigation.Identificacion.Contains(texto) ||
                  (f.IdClienteNavigation.Nombre + " " + f.IdClienteNavigation.PrimerApellido + " " +
                   (f.IdClienteNavigation.SegundoApellido ?? "")).Contains(texto))));
        }

        var pagina = await consulta
            .Include(f => f.IdClienteNavigation)
            .Include(f => f.Pago)
            .OrderByDescending(f => f.FechaEmision)
            .ThenByDescending(f => f.IdFactura)
            .PaginarAsync(filtro.Pagina, filtro.TamanoPagina);

        return pagina.Mapear(f => f.ADto());
    }
}
