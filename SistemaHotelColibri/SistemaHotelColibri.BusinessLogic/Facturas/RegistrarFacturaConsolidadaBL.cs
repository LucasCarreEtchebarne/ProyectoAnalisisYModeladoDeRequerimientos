using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Facturas;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Facturas;
using SistemaHotelColibri.Abstract.Modelos.Facturas;

namespace SistemaHotelColibri.BusinessLogic.Facturas;

public class RegistrarFacturaConsolidadaBL : IRegistrarFacturaConsolidadaBL
{
    private readonly IObtenerReservaFacturableDA _obtenerReservaDA;
    private readonly IObtenerPedidoFacturableDA _obtenerPedidoDA;
    private readonly IRegistrarFacturaDA _registrarDA;

    public RegistrarFacturaConsolidadaBL(
        IObtenerReservaFacturableDA obtenerReservaDA,
        IObtenerPedidoFacturableDA obtenerPedidoDA,
        IRegistrarFacturaDA registrarDA)
    {
        _obtenerReservaDA = obtenerReservaDA;
        _obtenerPedidoDA = obtenerPedidoDA;
        _registrarDA = registrarDA;
    }

    public async Task<ResultadoOperacion> RegistrarAsync(int idReserva, IReadOnlyCollection<int> idPedidos, int idUsuario)
    {
        var reserva = await _obtenerReservaDA.ObtenerAsync(idReserva);
        var error = ReglasFactura.ValidarReserva(reserva);
        if (error != null)
        {
            return ResultadoOperacion.Error(error);
        }

        var ids = idPedidos.Distinct().ToList();
        if (ids.Count == 0)
        {
            return ResultadoOperacion.Error(Mensajes.SeleccioneUnPedido);
        }

        var pedidos = await _obtenerPedidoDA.ObtenerAsync(ids);
        if (pedidos.Count != ids.Count)
        {
            return ResultadoOperacion.Error(Mensajes.NoEncontrado);
        }

        foreach (var pedido in pedidos)
        {
            error = ReglasFactura.ValidarPedido(pedido);
            if (error != null)
            {
                return ResultadoOperacion.Error(error);
            }

            if (pedido.IdReserva != reserva!.IdReserva && pedido.IdCliente != reserva.IdCliente)
            {
                return ResultadoOperacion.Error(Mensajes.PedidoNoPerteneceReserva);
            }
        }

        var detalles = new List<DetalleFacturaDto> { ReglasFactura.DetalleHospedaje(reserva!) };
        detalles.AddRange(pedidos.OrderBy(p => p.FechaHoraPedido).SelectMany(ReglasFactura.DetallesRestaurante));

        var factura = ReglasFactura.Construir(Facturacion.Tipo.Mixta, reserva!.IdCliente, idUsuario, detalles);

        var idFactura = await _registrarDA.RegistrarAsync(factura);
        return ReglasFactura.Emitida(idFactura, factura.MontoTotal);
    }
}
