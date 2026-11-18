using Microsoft.EntityFrameworkCore;
using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.DataAccess.Contexto;

namespace SistemaHotelColibri.DataAccess.Pedidos;

// Mantiene el estado de la mesa igual a sus cuentas: con cuenta abierta queda Ocupada,
// y cuando se cobra o se cierra la última cuenta vuelve a Disponible.
internal static class SincronizadorMesaPedido
{
    public static async Task OcuparAsync(HotelColibriContext contexto, int? idMesa)
    {
        if (idMesa is not > 0)
        {
            return;
        }

        await contexto.Mesa
            .Where(m => m.IdMesa == idMesa && m.EstadoMesa != Estados.Mesa.Inactiva && m.EstadoMesa != Estados.Mesa.Ocupada)
            .ExecuteUpdateAsync(cambios => cambios.SetProperty(m => m.EstadoMesa, Estados.Mesa.Ocupada));
    }

    public static async Task LiberarAsync(HotelColibriContext contexto, int? idMesa, int idPedidoQueCierra)
    {
        if (idMesa is not > 0)
        {
            return;
        }

        await contexto.Mesa
            .Where(m => m.IdMesa == idMesa
                     && m.EstadoMesa == Estados.Mesa.Ocupada
                     && !contexto.Pedido.Any(p => p.IdMesa == idMesa
                                              && p.IdPedido != idPedidoQueCierra
                                              && (p.EstadoPedido == Estados.Pedido.Pendiente || p.EstadoPedido == Estados.Pedido.Confirmado)
                                              && !p.DetalleFactura.Any()))
            .ExecuteUpdateAsync(cambios => cambios.SetProperty(m => m.EstadoMesa, Estados.Mesa.Disponible));
    }
}
