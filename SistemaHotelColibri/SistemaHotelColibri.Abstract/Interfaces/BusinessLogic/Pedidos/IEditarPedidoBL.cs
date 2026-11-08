using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Modelos.Pedidos;

namespace SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Pedidos;

public interface IEditarPedidoBL
{
    Task<ResultadoOperacion> EditarAsync(PedidoDto pedido);
    Task<ResultadoOperacion> InactivarAsync(int idPedido);
}