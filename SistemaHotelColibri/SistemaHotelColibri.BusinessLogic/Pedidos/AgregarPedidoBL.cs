using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Pedidos;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Pedidos;
using SistemaHotelColibri.Abstract.Modelos.Pedidos;

namespace SistemaHotelColibri.BusinessLogic.Pedidos;

public class AgregarPedidoBL : IAgregarPedidoBL
{
    private readonly IAgregarPedidoDA _agregarPedidoDA;
    private readonly IValidadorPedidoBL _validador;

    public AgregarPedidoBL(IAgregarPedidoDA agregarPedidoDA, IValidadorPedidoBL validador)
    {
        _agregarPedidoDA = agregarPedidoDA;
        _validador = validador;
    }

    public async Task<ResultadoOperacion> AgregarAsync(PedidoDto pedido)
    {
        if (pedido.IdUsuario <= 0)
        {
            return ResultadoOperacion.Error(
                "No se pudo identificar al usuario que registra el pedido.");
        }

        var error = await _validador.ValidarAsync(pedido);
        if (error != null)
        {
            return ResultadoOperacion.Error(error);
        }

        var idPedido = await _agregarPedidoDA.AgregarAsync(pedido);

        return ResultadoOperacion.Ok(
            "El pedido se registró correctamente.", idPedido);
    }
}