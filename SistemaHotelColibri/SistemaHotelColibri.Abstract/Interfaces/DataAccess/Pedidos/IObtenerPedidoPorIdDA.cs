using SistemaHotelColibri.Abstract.Modelos.Pedidos;

namespace SistemaHotelColibri.Abstract.Interfaces.DataAccess.Pedidos;

public interface IObtenerPedidoPorIdDA
{
    Task<PedidoDto?> ObtenerPorIdAsync(int idPedido);
}