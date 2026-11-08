using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Pedidos;
using SistemaHotelColibri.Abstract.Modelos.Pedidos;
using SistemaHotelColibri.DataAccess.Contexto;
using SistemaHotelColibri.DataAccess.Entidades;

namespace SistemaHotelColibri.DataAccess.Pedidos;

public class AgregarPedidoDA : IAgregarPedidoDA
{
    private readonly HotelColibriContext _contexto;

    public AgregarPedidoDA(HotelColibriContext contexto)
    {
        _contexto = contexto;
    }

    public async Task<int> AgregarAsync(PedidoDto pedido)
    {
        var entidad = new Pedido
        {
            IdCliente = pedido.IdCliente,
            IdUsuario = pedido.IdUsuario,
            IdMesa = pedido.IdMesa,
            IdHabitacion = pedido.IdHabitacion,
            IdReserva = pedido.IdReserva,
            TipoPedido = pedido.TipoPedido,
            EstadoPedido = Estados.Pedido.Pendiente,
            Observaciones = pedido.Observaciones,
            DetallePedido = pedido.Detalles
                .Select(detalle => new DetallePedido
                {
                    IdProductoMenu = detalle.IdProductoMenu,
                    Cantidad = detalle.Cantidad,
                    PrecioUnitario = detalle.PrecioUnitario
                })
                .ToList()
        };

        _contexto.Pedido.Add(entidad);
        await _contexto.SaveChangesAsync();

        return entidad.IdPedido;
    }
}