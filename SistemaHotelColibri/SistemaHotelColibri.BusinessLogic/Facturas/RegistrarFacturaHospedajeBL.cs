using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Facturas;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Facturas;

namespace SistemaHotelColibri.BusinessLogic.Facturas;

public class RegistrarFacturaHospedajeBL : IRegistrarFacturaHospedajeBL
{
    private readonly IObtenerReservaFacturableDA _obtenerReservaDA;
    private readonly IRegistrarFacturaDA _registrarDA;

    public RegistrarFacturaHospedajeBL(IObtenerReservaFacturableDA obtenerReservaDA, IRegistrarFacturaDA registrarDA)
    {
        _obtenerReservaDA = obtenerReservaDA;
        _registrarDA = registrarDA;
    }

    public async Task<ResultadoOperacion> RegistrarAsync(int idReserva, int idUsuario)
    {
        var reserva = await _obtenerReservaDA.ObtenerAsync(idReserva);
        var error = ReglasFactura.ValidarReserva(reserva);
        if (error != null)
        {
            return ResultadoOperacion.Error(error);
        }

        var factura = ReglasFactura.Construir(Facturacion.Tipo.Hospedaje, reserva!.IdCliente, idUsuario,
            [ReglasFactura.DetalleHospedaje(reserva)]);

        var idFactura = await _registrarDA.RegistrarAsync(factura);
        return ReglasFactura.Emitida(idFactura, factura.MontoTotal);
    }
}
