using SistemaHotelColibri.Abstract.Modelos.Pedidos;
using SistemaHotelColibri.DataAccess.Entidades;

namespace SistemaHotelColibri.DataAccess.Pedidos;

internal static class PedidoMapeo
{
    public static PedidoDto ADto(this Pedido pedido)
    {
        var cliente = pedido.IdClienteNavigation;

        return new PedidoDto
        {
            IdPedido = pedido.IdPedido,
            IdCliente = pedido.IdCliente,
            IdUsuario = pedido.IdUsuario,
            IdMesa = pedido.IdMesa,
            IdHabitacion = pedido.IdHabitacion,
            IdReserva = pedido.IdReserva,
            TipoPedido = pedido.TipoPedido,
            EstadoPedido = pedido.EstadoPedido,
            FechaHoraPedido = pedido.FechaHoraPedido,
            Observaciones = pedido.Observaciones,
            NombreCliente = cliente == null
                ? null
                : $"{cliente.Nombre} {cliente.PrimerApellido} {cliente.SegundoApellido}".Trim(),
            Detalles = pedido.DetallePedido
                .OrderBy(detalle => detalle.IdDetallePedido)
                .Select(detalle => new DetallePedidoDto
                {
                    IdDetallePedido = detalle.IdDetallePedido,
                    IdProductoMenu = detalle.IdProductoMenu,
                    NombreProducto = detalle.IdProductoMenuNavigation.NombreProducto,
                    Cantidad = detalle.Cantidad,
                    PrecioUnitario = detalle.PrecioUnitario
                })
                .ToList()
        };
    }
}