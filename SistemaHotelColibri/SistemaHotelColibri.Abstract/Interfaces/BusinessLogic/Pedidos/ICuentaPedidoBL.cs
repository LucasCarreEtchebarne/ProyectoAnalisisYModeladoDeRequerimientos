using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Modelos.Pedidos;

namespace SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Pedidos;

// La "cuenta" es el pedido abierto de una mesa, de una habitación o de un pedido para llevar.
public interface ICuentaPedidoBL
{
    Task<List<PedidoDto>> ObtenerAbiertasAsync();

    // Crea la cuenta con su primer producto. IdGenerado es el id del pedido.
    Task<ResultadoOperacion> AbrirAsync(PedidoDto nueva, int idProductoMenu);

    Task<ResultadoOperacion> AgregarProductoAsync(int idPedido, int idProductoMenu);
    Task<ResultadoOperacion> CambiarCantidadAsync(int idPedido, int idProductoMenu, int cambio);
    Task<ResultadoOperacion> GuardarNotaAsync(int idPedido, string? observaciones);
}
