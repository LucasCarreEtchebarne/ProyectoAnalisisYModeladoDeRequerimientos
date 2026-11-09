using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Modelos.Pedidos;

namespace SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Pedidos;

public interface IAgregarPedidoBL
{
    Task<ResultadoOperacion> AgregarAsync(PedidoDto pedido);
}