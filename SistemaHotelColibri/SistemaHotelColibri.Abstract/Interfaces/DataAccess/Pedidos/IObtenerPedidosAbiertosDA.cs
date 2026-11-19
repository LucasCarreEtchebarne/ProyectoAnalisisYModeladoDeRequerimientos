using SistemaHotelColibri.Abstract.Modelos.Pedidos;

namespace SistemaHotelColibri.Abstract.Interfaces.DataAccess.Pedidos;

// Pedidos que siguen abiertos (pendientes o confirmados y sin factura): mesas, habitaciones y para llevar.
public interface IObtenerPedidosAbiertosDA
{
    Task<List<PedidoDto>> ObtenerAsync();
}
