using SistemaHotelColibri.Abstract.Modelos.Facturas;

namespace SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Facturas;

public interface IObtenerPedidoFacturableBL
{
    Task<List<PedidoFacturableDto>> ListarAsync(string? busqueda);
    Task<List<PedidoFacturableDto>> ListarParaReservaAsync(int idReserva);
}
