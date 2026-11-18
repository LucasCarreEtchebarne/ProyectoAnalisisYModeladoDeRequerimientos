using System.Text.RegularExpressions;
using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Modelos.Facturas;

namespace SistemaHotelColibri.BusinessLogic.Facturas;

internal static partial class ReglasPago
{
    private const int LongitudMaximaReferencia = 50;

    // Deja solo los datos que corresponden al método elegido y calcula el vuelto en efectivo.
    public static void Normalizar(PagoDto pago)
    {
        pago.MontoPagado = Facturacion.Redondear(pago.MontoPagado);
        pago.TipoTarjeta = string.IsNullOrWhiteSpace(pago.TipoTarjeta) ? null : pago.TipoTarjeta.Trim();
        pago.UltimosCuatroDigitos = string.IsNullOrWhiteSpace(pago.UltimosCuatroDigitos) ? null : pago.UltimosCuatroDigitos.Trim();
        pago.NumeroReferencia = string.IsNullOrWhiteSpace(pago.NumeroReferencia) ? null : pago.NumeroReferencia.Trim();

        if (pago.MetodoPago == Facturacion.MetodoPago.Efectivo)
        {
            pago.MontoRecibido = pago.MontoRecibido.HasValue ? Facturacion.Redondear(pago.MontoRecibido.Value) : null;
            pago.CambioDevuelto = pago.MontoRecibido - pago.MontoPagado;
            pago.TipoTarjeta = null;
            pago.UltimosCuatroDigitos = null;
            pago.NumeroReferencia = null;
            return;
        }

        pago.MontoRecibido = null;
        pago.CambioDevuelto = null;

        if (pago.MetodoPago != Facturacion.MetodoPago.Tarjeta)
        {
            pago.TipoTarjeta = null;
            pago.UltimosCuatroDigitos = null;
        }
    }

    public static string? Validar(PagoDto pago, decimal saldo)
    {
        if (!Facturacion.MetodoPago.Todos.Contains(pago.MetodoPago))
        {
            return Mensajes.MetodoPagoInvalido;
        }

        if (pago.MontoPagado <= 0)
        {
            return Mensajes.MontoPagoInvalido;
        }

        if (pago.MontoPagado > saldo)
        {
            return string.Format(Mensajes.MontoPagoExcedeSaldo, Formato.Colones(saldo));
        }

        switch (pago.MetodoPago)
        {
            case Facturacion.MetodoPago.Efectivo when pago.MontoRecibido is null || pago.MontoRecibido < pago.MontoPagado:
                return Mensajes.MontoRecibidoInsuficiente;

            case Facturacion.MetodoPago.Tarjeta when !Facturacion.TiposTarjeta.Contains(pago.TipoTarjeta):
                return Mensajes.TipoTarjetaInvalido;

            case Facturacion.MetodoPago.Tarjeta when pago.UltimosCuatroDigitos is null || !CuatroDigitos().IsMatch(pago.UltimosCuatroDigitos):
                return Mensajes.UltimosDigitosInvalidos;

            case Facturacion.MetodoPago.Transferencia or Facturacion.MetodoPago.Sinpe when pago.NumeroReferencia is null:
                return Mensajes.ReferenciaObligatoria;
        }

        if (pago.NumeroReferencia?.Length > LongitudMaximaReferencia)
        {
            return string.Format(Mensajes.LongitudMaxima, "número de referencia", LongitudMaximaReferencia);
        }

        return null;
    }

    [GeneratedRegex(@"^\d{4}$")]
    private static partial Regex CuatroDigitos();
}
