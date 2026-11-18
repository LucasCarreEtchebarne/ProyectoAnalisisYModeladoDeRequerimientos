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
            .Where(pedido => (pedido.EstadoPedido == Estados.Pedido.Pendiente || pedido.EstadoPedido == Estados.Pedido.Confirmado)
                          && !pedido.DetalleFactura.Any())
            .OrderBy(pedido => pedido.FechaHoraPedido)
            .ToListAsync();

        return pedidos.Select(pedido => pedido.ADto()).ToList();
    }
}
