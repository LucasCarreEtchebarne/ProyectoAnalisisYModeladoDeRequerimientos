using SistemaHotelColibri.Abstract.Modelos.Pedidos;

namespace SistemaHotelColibri.Abstract.Interfaces.DataAccess.Pedidos;

public interface IEditarPedidoDA
{
    Task<bool> EditarAsync(PedidoDto pedido);
    Task<bool> InactivarAsync(int idPedido);
}