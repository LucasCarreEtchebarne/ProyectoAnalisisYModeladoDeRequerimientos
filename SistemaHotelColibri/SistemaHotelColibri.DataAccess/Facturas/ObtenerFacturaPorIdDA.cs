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
            .Include(f => f.Pago)
            .Include(f => f.DetalleFactura)
                .ThenInclude(d => d.IdPedidoNavigation!)
                    .ThenInclude(p => p.IdMesaNavigation)
            .Include(f => f.DetalleFactura)
                .ThenInclude(d => d.IdPedidoNavigation!)
                    .ThenInclude(p => p.IdHabitacionNavigation)
            .FirstOrDefaultAsync(f => f.IdFactura == idFactura);

        if (factura == null)
        {
            return null;
        }

        var nombres = await _identidad.ObtenerNombresUsuarioAsync(new[] { factura.IdUsuario });
        var cliente = factura.IdClienteNavigation;
        var pedido = factura.DetalleFactura.Select(d => d.IdPedidoNavigation).FirstOrDefault(p => p != null);

        return new FacturaDto
        {
            IdFactura = factura.IdFactura,
            NumeroFactura = factura.NumeroFactura,
            IdCliente = factura.IdCliente,
            NombreCliente = cliente == null
                ? null
                : $"{cliente.Nombre} {cliente.PrimerApellido} {cliente.SegundoApellido}".Trim(),
            IdentificacionCliente = cliente?.Identificacion,
            IdUsuario = factura.IdUsuario,
            NombreUsuario = nombres.GetValueOrDefault(factura.IdUsuario, "Usuario #" + factura.IdUsuario),
            FechaEmision = factura.FechaEmision,
            TipoFactura = factura.TipoFactura,
            MontoTotal = factura.MontoTotal,
            EstadoFactura = factura.EstadoFactura,
            IdPedido = pedido?.IdPedido,
            NumeroMesa = pedido?.IdMesaNavigation?.NumeroMesa,
            NumeroHabitacion = pedido?.IdHabitacionNavigation?.NumeroHabitacion,
            TipoPedido = pedido?.TipoPedido,
            Detalles = factura.DetalleFactura
                .OrderBy(d => d.IdDetalleFactura)
                .Select(d => new DetalleFacturaDto
                {
                    Descripcion = d.Descripcion,
                    Cantidad = d.Cantidad,
                    PrecioUnitario = d.PrecioUnitario
                })
                .ToList(),
            Pagos = factura.Pago
                .OrderBy(p => p.IdPago)
                .Select(p => new PagoDto
                {
                    MetodoPago = p.MetodoPago,
                    MontoPagado = p.MontoPagado,
                    MontoRecibido = p.MontoRecibido,
                    CambioDevuelto = p.CambioDevuelto,
                    TipoTarjeta = p.TipoTarjeta,
                    UltimosCuatroDigitos = p.UltimosCuatroDigitos,
                    NumeroReferencia = p.NumeroReferencia,
                    FechaHoraPago = p.FechaHoraPago
                })
                .ToList()
        };
    }
}
