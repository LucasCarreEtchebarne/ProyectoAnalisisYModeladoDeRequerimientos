using Microsoft.EntityFrameworkCore;
using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Facturas;
using SistemaHotelColibri.Abstract.Modelos.Facturas;
using SistemaHotelColibri.Abstract.Modelos.Pedidos;
using SistemaHotelColibri.DataAccess.Contexto;
using SistemaHotelColibri.DataAccess.Entidades;
using SistemaHotelColibri.DataAccess.Pedidos;

namespace SistemaHotelColibri.DataAccess.Facturas;

public class FacturarPedidoDA : IFacturarPedidoDA
{
    private const string TipoRestaurante = "Restaurante";

    private readonly HotelColibriContext _contexto;

    public FacturarPedidoDA(HotelColibriContext contexto)
    {
        _contexto = contexto;
    }

    public async Task<int?> FacturarAsync(SolicitudCobroDto solicitud, PedidoDto pedido)
    {
        await using var transaccion = await _contexto.Database.BeginTransactionAsync();

        // El pedido pasa a Entregado solo si sigue abierto y sin factura (evita cobrar dos veces).
        var afectados = await _contexto.Pedido
            .Where(p => p.IdPedido == pedido.IdPedido
                     && (p.EstadoPedido == Estados.Pedido.Pendiente || p.EstadoPedido == Estados.Pedido.Confirmado)
                     && !p.DetalleFactura.Any())
            .ExecuteUpdateAsync(cambios => cambios
                .SetProperty(p => p.EstadoPedido, Estados.Pedido.Entregado)
                .SetProperty(p => p.IdCliente, p => solicitud.IdCliente ?? p.IdCliente));

        if (afectados == 0)
        {
            return null;
        }

        var factura = new Factura
        {
            IdCliente = solicitud.IdCliente ?? pedido.IdCliente,
            IdUsuario = solicitud.IdUsuario,
            TipoFactura = TipoRestaurante,
            MontoTotal = solicitud.MontoTotal,
            EstadoFactura = Estados.Factura.Pagada,
            DetalleFactura = pedido.Detalles
                .Select(detalle => new DetalleFactura
                {
                    TipoConcepto = TipoRestaurante,
                    IdPedido = pedido.IdPedido,
                    Descripcion = detalle.NombreProducto,
                    Cantidad = detalle.Cantidad,
                    PrecioUnitario = detalle.PrecioUnitario
                })
                .ToList(),
            Pago =
            [
                new Pago
                {
                    IdUsuario = solicitud.IdUsuario,
                    MetodoPago = solicitud.MetodoPago,
                    MontoPagado = solicitud.MontoTotal,
                    MontoRecibido = solicitud.MontoRecibido,
                    CambioDevuelto = solicitud.CambioDevuelto,
                    TipoTarjeta = solicitud.TipoTarjeta,
                    UltimosCuatroDigitos = solicitud.UltimosCuatroDigitos,
                    NumeroReferencia = solicitud.NumeroReferencia,
                    EstadoPago = Estados.Pago.Aprobado
                }
            ]
        };

        _contexto.Factura.Add(factura);
        await _contexto.SaveChangesAsync();

        // Cobrada la cuenta, la mesa vuelve a quedar disponible.
        await SincronizadorMesaPedido.LiberarAsync(_contexto, pedido.IdMesa, pedido.IdPedido);
        await transaccion.CommitAsync();

        return factura.IdFactura;
    }
}
