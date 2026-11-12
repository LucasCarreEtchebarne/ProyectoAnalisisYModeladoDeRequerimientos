using Microsoft.EntityFrameworkCore;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Facturas;
using SistemaHotelColibri.Abstract.Modelos.Facturas;
using SistemaHotelColibri.DataAccess.Contexto;
using SistemaHotelColibri.DataAccess.Identidad;
using SistemaHotelColibri.DataAccess.TareasHousekeeping;

namespace SistemaHotelColibri.DataAccess.Facturas;

public class ObtenerFacturaPorIdDA : IObtenerFacturaPorIdDA
{
    private readonly HotelColibriContext _contexto;
    private readonly IdentidadContext _identidad;

    public ObtenerFacturaPorIdDA(HotelColibriContext contexto, IdentidadContext identidad)
    {
        _contexto = contexto;
        _identidad = identidad;
    }

    public async Task<FacturaDto?> ObtenerPorIdAsync(int idFactura)
    {
        var factura = await _contexto.Factura
            .AsNoTracking()
            .Include(f => f.IdClienteNavigation)
            .Include(f => f.DetalleFactura)
            .Include(f => f.Pago)
            .FirstOrDefaultAsync(f => f.IdFactura == idFactura);

        if (factura == null)
        {
            return null;
        }

        var nombres = await _identidad.ObtenerNombresUsuarioAsync([factura.IdUsuario]);
        return factura.ADto(nombres.GetValueOrDefault(factura.IdUsuario));
    }
}
