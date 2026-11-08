using Microsoft.EntityFrameworkCore;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Pedidos;
using SistemaHotelColibri.Abstract.Modelos.Pedidos;
using SistemaHotelColibri.DataAccess.Contexto;

namespace SistemaHotelColibri.DataAccess.Pedidos;

public class ObtenerPedidoPorIdDA : IObtenerPedidoPorIdDA
{
    private readonly HotelColibriContext _contexto;

    public ObtenerPedidoPorIdDA(HotelColibriContext contexto)
    {
        _contexto = contexto;
    }

    public async Task<PedidoDto?> ObtenerPorIdAsync(int idPedido)
    {
        var pedido = await _contexto.Pedido
            .AsNoTracking()
            .Include(pedido => pedido.IdClienteNavigation)
            .Include(pedido => pedido.DetallePedido)
                .ThenInclude(detalle => detalle.IdProductoMenuNavigation)
            .FirstOrDefaultAsync(pedido => pedido.IdPedido == idPedido);

        return pedido?.ADto();
    }
}