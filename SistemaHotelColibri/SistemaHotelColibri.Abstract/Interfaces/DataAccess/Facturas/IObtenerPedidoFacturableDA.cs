using SistemaHotelColibri.Abstract.Modelos.Facturas;

namespace SistemaHotelColibri.Abstract.Interfaces.DataAccess.Facturas;

public interface IObtenerPedidoFacturableDA
{
    Task<List<PedidoFacturableDto>> ListarAsync(string? busqueda);
    Task<List<PedidoFacturableDto>> ListarParaReservaAsync(int idReserva, int idCliente);
    Task<List<PedidoFacturableDto>> ObtenerAsync(IReadOnlyCollection<int> idPedidos);
}
