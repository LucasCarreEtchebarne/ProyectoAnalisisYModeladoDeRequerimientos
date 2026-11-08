using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Modelos.Pedidos;

namespace SistemaHotelColibri.Abstract.Interfaces.DataAccess.Pedidos;

public interface IObtenerPedidoDA
{
    Task<ResultadoPaginado<PedidoDto>> ObtenerAsync(FiltroListado filtro);
}