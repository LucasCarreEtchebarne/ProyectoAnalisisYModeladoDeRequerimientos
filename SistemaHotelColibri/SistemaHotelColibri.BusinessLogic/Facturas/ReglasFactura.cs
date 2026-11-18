using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Modelos.Facturas;

namespace SistemaHotelColibri.BusinessLogic.Facturas;

internal static class ReglasFactura
{
    public static string? ValidarReserva(ReservaFacturableDto? reserva)
    {
        if (reserva == null)
        {
            return Mensajes.NoEncontrado;
        }

        if (!Facturacion.EstadosReservaFacturables.Contains(reserva.EstadoReserva))
        {
            return Mensajes.ReservaNoFacturable;
        }

        if (reserva.YaFacturada)
        {
            return Mensajes.ReservaYaFacturada;
        }

        return reserva.CantidadNoches <= 0 ? Mensajes.ReservaSinNoches : null;
    }

    public static string? ValidarPedido(PedidoFacturableDto? pedido)
    {
        if (pedido == null)
        {
            return Mensajes.NoEncontrado;
        }

        if (pedido.EstadoPedido != Estados.Pedido.Pendiente)
        {
            return Mensajes.PedidoNoFacturable;
        }

        if (pedido.YaFacturado)
        {
            return Mensajes.PedidoYaFacturado;
        }

        return pedido.Lineas.Count == 0 || pedido.Total <= 0 ? Mensajes.PedidoSinProductos : null;
    }

    public static DetalleFacturaDto DetalleHospedaje(ReservaFacturableDto reserva) => new()
    {
        TipoConcepto = Facturacion.Concepto.Hospedaje,
        IdReserva = reserva.IdReserva,
        Descripcion = $"Hospedaje habitación {reserva.NumeroHabitacion} ({Formato.Fecha(reserva.FechaEntrada)} al {Formato.Fecha(reserva.FechaSalida)})",
        Cantidad = reserva.CantidadNoches,
        PrecioUnitario = reserva.PrecioNoche
    };

    public static IEnumerable<DetalleFacturaDto> DetallesRestaurante(PedidoFacturableDto pedido) =>
        pedido.Lineas.Select(linea => new DetalleFacturaDto
        {
            TipoConcepto = Facturacion.Concepto.Restaurante,
            IdPedido = pedido.IdPedido,
            Descripcion = $"{linea.NombreProducto} ({pedido.Origen}, {Formato.Fecha(pedido.FechaHoraPedido)})",
            Cantidad = linea.Cantidad,
            PrecioUnitario = linea.PrecioUnitario
        });

    public static FacturaDto Construir(string tipoFactura, int? idCliente, int idUsuario, List<DetalleFacturaDto> detalles)
    {
        var subtotal = detalles.Sum(d => d.Subtotal);
        var subtotalRestaurante = detalles.Where(d => d.TipoConcepto == Facturacion.Concepto.Restaurante).Sum(d => d.Subtotal);
        var totales = Facturacion.CalcularTotales(subtotal, subtotalRestaurante);

        return new FacturaDto
        {
            TipoFactura = tipoFactura,
            IdCliente = idCliente,
            IdUsuario = idUsuario,
            FechaEmision = DateTime.Now,
            Subtotal = totales.Subtotal,
            MontoServicio = totales.Servicio,
            MontoIva = totales.Iva,
            MontoTotal = totales.Total,
            EstadoFactura = Estados.Factura.Pendiente,
            Detalles = detalles
        };
    }

    public static ResultadoOperacion Emitida(int idFactura, decimal total) =>
        ResultadoOperacion.Ok(string.Format(Mensajes.FacturaRegistrada, Facturacion.NumeroFactura(idFactura), Formato.Colones(total)), idFactura);
}
