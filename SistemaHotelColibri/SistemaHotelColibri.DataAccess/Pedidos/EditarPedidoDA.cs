using Microsoft.EntityFrameworkCore;
using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Pedidos;
using SistemaHotelColibri.Abstract.Modelos.Pedidos;
using SistemaHotelColibri.DataAccess.Contexto;
using SistemaHotelColibri.DataAccess.Entidades;

namespace SistemaHotelColibri.DataAccess.Pedidos;

public class EditarPedidoDA : IEditarPedidoDA
{
    private readonly HotelColibriContext _contexto;

    public EditarPedidoDA(HotelColibriContext contexto)
    {
        _contexto = contexto;
    }

    public async Task<bool> EditarAsync(PedidoDto pedido)
    {
        await using var transaccion = await _contexto.Database.BeginTransactionAsync();

        var afectados = await PendienteSinFactura(pedido.IdPedido)
            .ExecuteUpdateAsync(cambios => cambios
                .SetProperty(p => p.TipoPedido, pedido.TipoPedido)
                .SetProperty(p => p.IdCliente, pedido.IdCliente)
                .SetProperty(p => p.IdMesa, pedido.IdMesa)
                .SetProperty(p => p.IdHabitacion, pedido.IdHabitacion)
                .SetProperty(p => p.IdReserva, pedido.IdReserva)
                .SetProperty(p => p.Observaciones, pedido.Observaciones));

        if (afectados == 0)
        {
            return false;
        }

        await _contexto.DetallePedido
            .Where(detalle => detalle.IdPedido == pedido.IdPedido)
            .ExecuteDeleteAsync();

        _contexto.DetallePedido.AddRange(pedido.Detalles.Select(detalle => new DetallePedido
        {
            IdPedido = pedido.IdPedido,
            IdProductoMenu = detalle.IdProductoMenu,
            Cantidad = detalle.Cantidad,
            PrecioUnitario = detalle.PrecioUnitario
        }));

        await _contexto.SaveChangesAsync();
        await transaccion.CommitAsync();

        return true;
    }

    public async Task<bool> InactivarAsync(int idPedido)
    {
        var afectados = await PendienteSinFactura(idPedido)
            .ExecuteUpdateAsync(cambios => cambios
                .SetProperty(p => p.EstadoPedido, Estados.Pedido.Inactivo));

        return afectados > 0;
    }

    private IQueryable<Pedido> PendienteSinFactura(int idPedido)
    {
        return _contexto.Pedido.Where(pedido =>
            pedido.IdPedido == idPedido &&
            pedido.EstadoPedido == Estados.Pedido.Pendiente &&
            !pedido.DetalleFactura.Any());
    }
}