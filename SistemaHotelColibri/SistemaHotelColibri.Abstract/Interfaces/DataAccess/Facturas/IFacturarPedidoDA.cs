using SistemaHotelColibri.Abstract.Modelos.Facturas;
using SistemaHotelColibri.Abstract.Modelos.Pedidos;

namespace SistemaHotelColibri.Abstract.Interfaces.DataAccess.Facturas;

public interface IFacturarPedidoDA
{
    // Devuelve el id de la factura creada, o null si el pedido ya no estaba abierto.
    Task<int?> FacturarAsync(SolicitudCobroDto solicitud, PedidoDto pedido);
}
