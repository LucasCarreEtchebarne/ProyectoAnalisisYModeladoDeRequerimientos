using Microsoft.EntityFrameworkCore;
using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Mesas;
using SistemaHotelColibri.DataAccess.Contexto;

namespace SistemaHotelColibri.DataAccess.Mesas;

public class TienePedidoActivoMesaDA : ITienePedidoActivoMesaDA
{
    private readonly HotelColibriContext _contexto;

    public TienePedidoActivoMesaDA(HotelColibriContext contexto)
    {
        _contexto = contexto;
    }

    public async Task<bool> TienePedidoActivoAsync(int idMesa)
    {
        return await _contexto.Pedido
            .AnyAsync(p => p.IdMesa == idMesa
                        && (p.EstadoPedido == Estados.Pedido.Pendiente || p.EstadoPedido == Estados.Pedido.Confirmado));
    }
}
