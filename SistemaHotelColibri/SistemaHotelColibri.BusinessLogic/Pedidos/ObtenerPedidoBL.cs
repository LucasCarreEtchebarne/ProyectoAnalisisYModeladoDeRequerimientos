using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Pedidos;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Pedidos;
using SistemaHotelColibri.Abstract.Modelos.Pedidos;

namespace SistemaHotelColibri.BusinessLogic.Pedidos;

public class ObtenerPedidoBL : IObtenerPedidoBL
{
    private readonly IObtenerPedidoDA _obtenerPedidoDA;

    public ObtenerPedidoBL(IObtenerPedidoDA obtenerPedidoDA)
    {
        _obtenerPedidoDA = obtenerPedidoDA;
    }

    public Task<ResultadoPaginado<PedidoDto>> ObtenerAsync(FiltroListado filtro)
    {
        return _obtenerPedidoDA.ObtenerAsync(filtro);
    }
}