using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Facturas;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Facturas;

namespace SistemaHotelColibri.BusinessLogic.Facturas;

public class RegistrarFacturaRestauranteBL : IRegistrarFacturaRestauranteBL
{
    private readonly IObtenerPedidoFacturableDA _obtenerPedidoDA;
    private readonly IRegistrarFacturaDA _registrarDA;

    public RegistrarFacturaRestauranteBL(IObtenerPedidoFacturableDA obtenerPedidoDA, IRegistrarFacturaDA registrarDA)
    {
        _obtenerPedidoDA = obtenerPedidoDA;
        _registrarDA = registrarDA;
    }

    public async Task<ResultadoOperacion> RegistrarAsync(int idPedido, int idUsuario)
    {
        var pedido = (await _obtenerPedidoDA.ObtenerAsync([idPedido])).FirstOrDefault();
        var error = ReglasFactura.ValidarPedido(pedido);
        if (error != null)
        {
            return ResultadoOperacion.Error(error);
        }

        var factura = ReglasFactura.Construir(Facturacion.Tipo.Restaurante, pedido!.IdCliente, idUsuario,
            ReglasFactura.DetallesRestaurante(pedido).ToList());

        var idFactura = await _registrarDA.RegistrarAsync(factura);
        return ReglasFactura.Emitida(idFactura, factura.MontoTotal);
    }
}
