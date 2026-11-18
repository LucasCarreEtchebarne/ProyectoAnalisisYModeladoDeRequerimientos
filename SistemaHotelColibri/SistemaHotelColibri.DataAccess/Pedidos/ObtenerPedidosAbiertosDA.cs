using Microsoft.EntityFrameworkCore;
using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Pedidos;
using SistemaHotelColibri.Abstract.Modelos.Pedidos;
using SistemaHotelColibri.DataAccess.Contexto;

namespace SistemaHotelColibri.DataAccess.Pedidos;

public class ObtenerPedidosAbiertosDA : IObtenerPedidosAbiertosDA
{
    private readonly HotelColibriContext _contexto;

    public ObtenerPedidosAbiertosDA(HotelColibriContext contexto)
    {
        _contexto = contexto;
    }

    public async Task<List<PedidoDto>> ObtenerAsync()
    {
        var pedidos = await _contexto.Pedido
            .AsNoTracking()
            .Include(pedido => pedido.IdClienteNavigation)
            .Include(pedido => pedido.DetallePedido)
                .ThenInclude(detalle => detalle.IdProductoMenuNavigation)
            // Igual que en Facturación: una cuenta abierta es un pedido pendiente sin factura vigente
            // (si su factura se anuló, la cuenta vuelve a estar abierta).
            .Where(pedido => pedido.EstadoPedido == Estados.Pedido.Pendiente
                          && !pedido.DetalleFactura.Any(d => d.IdFacturaNavigation.EstadoFactura != Estados.Factura.Anulada))
            .OrderBy(pedido => pedido.FechaHoraPedido)
            .ToListAsync();

        return pedidos.Select(pedido => pedido.ADto()).ToList();
    }
}
