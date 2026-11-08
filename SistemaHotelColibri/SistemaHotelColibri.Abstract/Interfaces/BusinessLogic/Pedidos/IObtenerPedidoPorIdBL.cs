using SistemaHotelColibri.Abstract.Modelos.Pedidos;

namespace SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Pedidos;

public interface IObtenerPedidoPorIdBL
{
    Task<PedidoDto?> ObtenerPorIdAsync(int idPedido);
}