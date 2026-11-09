using SistemaHotelColibri.Abstract.Modelos.Pedidos;

namespace SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Pedidos;

public interface IValidadorPedidoBL
{
    Task<string?> ValidarAsync(PedidoDto pedido);
}