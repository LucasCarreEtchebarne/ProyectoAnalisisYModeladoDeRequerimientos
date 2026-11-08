using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Pedidos;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Pedidos;
using SistemaHotelColibri.Abstract.Modelos.Pedidos;

namespace SistemaHotelColibri.BusinessLogic.Pedidos;

public class ObtenerPedidoPorIdBL : IObtenerPedidoPorIdBL
{
    private readonly IObtenerPedidoPorIdDA _obtenerPedidoPorIdDA;

    public ObtenerPedidoPorIdBL(IObtenerPedidoPorIdDA obtenerPedidoPorIdDA)
    {
        _obtenerPedidoPorIdDA = obtenerPedidoPorIdDA;
    }

    public Task<PedidoDto?> ObtenerPorIdAsync(int idPedido)
    {
        return _obtenerPedidoPorIdDA.ObtenerPorIdAsync(idPedido);
    }
}