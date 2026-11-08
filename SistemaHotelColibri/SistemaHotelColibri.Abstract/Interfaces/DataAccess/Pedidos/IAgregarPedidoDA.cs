using SistemaHotelColibri.Abstract.Modelos.Pedidos;

namespace SistemaHotelColibri.Abstract.Interfaces.DataAccess.Pedidos;

public interface IAgregarPedidoDA
{
    Task<int> AgregarAsync(PedidoDto pedido);
}