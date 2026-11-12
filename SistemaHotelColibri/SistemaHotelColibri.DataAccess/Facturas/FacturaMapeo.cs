using SistemaHotelColibri.Abstract.Modelos.Facturas;
using SistemaHotelColibri.DataAccess.Entidades;

namespace SistemaHotelColibri.DataAccess.Facturas;

internal static class FacturaMapeo
{
    public static string NombreCompleto(this Cliente cliente) =>
        $"{cliente.Nombre} {cliente.PrimerApellido} {cliente.SegundoApellido}".Trim();

    public static FacturaDto ADto(this Factura factura, string? nombreUsuario = null) => new()
    {
        IdFactura = factura.IdFactura,
        NumeroFactura = factura.NumeroFactura ?? string.Empty,
        IdCliente = factura.IdCliente,
        NombreCliente = factura.IdClienteNavigation?.NombreCompleto(),
        IdentificacionCliente = factura.IdClienteNavigation?.Identificacion,
        IdUsuario = factura.IdUsuario,
        NombreUsuario = nombreUsuario,
        FechaEmision = factura.FechaEmision,
        TipoFactura = factura.TipoFactura,
        Subtotal = factura.Subtotal,
        MontoServicio = factura.MontoServicio,
        MontoIva = factura.MontoIva,
        MontoTotal = factura.MontoTotal,
        EstadoFactura = factura.EstadoFactura,
        MotivoAnulacion = factura.MotivoAnulacion,
        FechaAnulacion = factura.FechaAnulacion,
        Detalles = factura.DetalleFactura
            .OrderBy(d => d.IdDetalleFactura)
            .Select(d => new DetalleFacturaDto
            {
                IdDetalleFactura = d.IdDetalleFactura,
                TipoConcepto = d.TipoConcepto,
                IdReserva = d.IdReserva,
                IdPedido = d.IdPedido,
                IdEvento = d.IdEvento,
                Descripcion = d.Descripcion,
                Cantidad = d.Cantidad,
                PrecioUnitario = d.PrecioUnitario
            })
            .ToList(),
        Pagos = factura.Pago
            .OrderBy(p => p.FechaHoraPago)
            .Select(p => p.ADto())
            .ToList()
    };

    public static PagoDto ADto(this Pago pago) => new()
    {
        IdPago = pago.IdPago,
        IdFactura = pago.IdFactura,
        IdUsuario = pago.IdUsuario,
        MetodoPago = pago.MetodoPago,
        MontoPagado = pago.MontoPagado,
        MontoRecibido = pago.MontoRecibido,
        CambioDevuelto = pago.CambioDevuelto,
        TipoTarjeta = pago.TipoTarjeta,
        UltimosCuatroDigitos = pago.UltimosCuatroDigitos,
        NumeroReferencia = pago.NumeroReferencia,
        FechaHoraPago = pago.FechaHoraPago,
        EstadoPago = pago.EstadoPago
    };
}
