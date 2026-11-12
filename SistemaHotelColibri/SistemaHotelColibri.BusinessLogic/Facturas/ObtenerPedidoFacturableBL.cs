using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Facturas;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Facturas;
using SistemaHotelColibri.Abstract.Modelos.Facturas;

namespace SistemaHotelColibri.BusinessLogic.Facturas;

public class ObtenerPedidoFacturableBL : IObtenerPedidoFacturableBL
{
    private readonly IObtenerPedidoFacturableDA _obtenerPedidoDA;
    private readonly IObtenerReservaFacturableDA _obtenerReservaDA;

    public ObtenerPedidoFacturableBL(IObtenerPedidoFacturableDA obtenerPedidoDA, IObtenerReservaFacturableDA obtenerReservaDA)
    {
        _obtenerPedidoDA = obtenerPedidoDA;
        _obtenerReservaDA = obtenerReservaDA;
    }

    public Task<List<PedidoFacturableDto>> ListarAsync(string? busqueda) =>
        _obtenerPedidoDA.ListarAsync(string.IsNullOrWhiteSpace(busqueda) ? null : busqueda.Trim());

    public async Task<List<PedidoFacturableDto>> ListarParaReservaAsync(int idReserva)
    {
        var reserva = await _obtenerReservaDA.ObtenerAsync(idReserva);
        return reserva == null ? [] : await _obtenerPedidoDA.ListarParaReservaAsync(reserva.IdReserva, reserva.IdCliente);
    }
}
