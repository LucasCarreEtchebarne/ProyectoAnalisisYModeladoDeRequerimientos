using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Facturas;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Facturas;
using SistemaHotelColibri.Abstract.Modelos.Facturas;

namespace SistemaHotelColibri.BusinessLogic.Facturas;

public class RegistrarPagoBL : IRegistrarPagoBL
{
    private readonly IObtenerFacturaPorIdDA _obtenerPorIdDA;
    private readonly IRegistrarPagoDA _registrarPagoDA;

    public RegistrarPagoBL(IObtenerFacturaPorIdDA obtenerPorIdDA, IRegistrarPagoDA registrarPagoDA)
    {
        _obtenerPorIdDA = obtenerPorIdDA;
        _registrarPagoDA = registrarPagoDA;
    }

    public async Task<ResultadoOperacion> RegistrarAsync(PagoDto pago, int idUsuario)
    {
        var factura = await _obtenerPorIdDA.ObtenerPorIdAsync(pago.IdFactura);
        if (factura == null)
        {
            return ResultadoOperacion.Error(Mensajes.NoEncontrado);
        }

        if (factura.EstadoFactura != Estados.Factura.Pendiente)
        {
            return ResultadoOperacion.Error(Mensajes.FacturaNoAdmitePagos);
        }

        ReglasPago.Normalizar(pago);
        var error = ReglasPago.Validar(pago, factura.Saldo);
        if (error != null)
        {
            return ResultadoOperacion.Error(error);
        }

        pago.IdUsuario = idUsuario;
        pago.FechaHoraPago = DateTime.Now;
        pago.EstadoPago = Estados.Pago.Aprobado;

        var quedaPagada = factura.TotalPagado + pago.MontoPagado >= factura.MontoTotal;
        var idPago = await _registrarPagoDA.RegistrarAsync(pago, quedaPagada);

        var mensaje = quedaPagada ? Mensajes.PagoRegistradoFacturaPagada : Mensajes.PagoRegistrado;
        return ResultadoOperacion.Ok(string.Format(mensaje, Formato.Colones(pago.MontoPagado)), idPago);
    }
}
