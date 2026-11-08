using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Modelos.Pedidos;

namespace SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Pedidos;

public interface IObtenerPedidoBL
{
    Task<ResultadoPaginado<PedidoDto>> ObtenerAsync(FiltroListado filtro);
}