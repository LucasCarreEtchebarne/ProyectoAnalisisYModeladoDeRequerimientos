using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Facturas;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Clientes;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Facturas;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Pedidos;
using SistemaHotelColibri.Abstract.Modelos.Facturas;

namespace SistemaHotelColibri.BusinessLogic.Facturas;

public class FacturarPedidoBL : IFacturarPedidoBL
{
    private readonly IObtenerPedidoPorIdDA _obtenerPedidoDA;
    private readonly IObtenerClientePorIdDA _obtenerClienteDA;
    private readonly IFacturarPedidoDA _facturarDA;

    public FacturarPedidoBL(
        IObtenerPedidoPorIdDA obtenerPedidoDA,
        IObtenerClientePorIdDA obtenerClienteDA,
        IFacturarPedidoDA facturarDA)
    {
        _obtenerPedidoDA = obtenerPedidoDA;
        _obtenerClienteDA = obtenerClienteDA;
        _facturarDA = facturarDA;
    }

    public async Task<ResultadoOperacion> FacturarAsync(SolicitudCobroDto solicitud)
    {
        if (solicitud.IdUsuario <= 0)
        {
            return ResultadoOperacion.Error("No se pudo identificar al usuario que cobra.");
        }

        var pedido = await _obtenerPedidoDA.ObtenerPorIdAsync(solicitud.IdPedido);
        if (pedido == null)
        {
            return ResultadoOperacion.Error(Mensajes.NoEncontrado);
        }

        if (pedido.EstadoPedido != Estados.Pedido.Pendiente && pedido.EstadoPedido != Estados.Pedido.Confirmado)
        {
            return ResultadoOperacion.Error("Este pedido ya fue cobrado o está cerrado.");
        }

        if (pedido.Detalles.Count == 0 || pedido.Total <= 0)
        {
            return ResultadoOperacion.Error("La cuenta no tiene productos para cobrar.");
        }

        if (solicitud.IdCliente.HasValue && await _obtenerClienteDA.ObtenerPorIdAsync(solicitud.IdCliente.Value) == null)
        {
            return ResultadoOperacion.Error("El cliente seleccionado no existe.");
        }

        // El total siempre se calcula aquí con los precios guardados, no se confía en lo que manda la pantalla.
        solicitud.MontoTotal = pedido.Total;

        var error = ValidarPago(solicitud);
        if (error != null)
        {
            return ResultadoOperacion.Error(error);
        }

        var idFactura = await _facturarDA.FacturarAsync(solicitud, pedido);

        return idFactura.HasValue
            ? ResultadoOperacion.Ok("Cuenta cobrada. La factura se generó correctamente.", idFactura.Value)
            : ResultadoOperacion.Error("Este pedido ya fue cobrado por otra persona.");
    }

    private static string? ValidarPago(SolicitudCobroDto solicitud)
    {
        solicitud.NumeroReferencia = string.IsNullOrWhiteSpace(solicitud.NumeroReferencia) ? null : solicitud.NumeroReferencia.Trim();
        solicitud.UltimosCuatroDigitos = string.IsNullOrWhiteSpace(solicitud.UltimosCuatroDigitos) ? null : solicitud.UltimosCuatroDigitos.Trim();

        switch (solicitud.MetodoPago)
        {
            case Catalogos.MetodoPago.Efectivo:
                if (solicitud.MontoRecibido is not { } recibido || recibido < solicitud.MontoTotal)
                {
                    return "El monto recibido en efectivo debe cubrir el total.";
                }

                solicitud.CambioDevuelto = recibido - solicitud.MontoTotal;
                solicitud.TipoTarjeta = null;
                solicitud.UltimosCuatroDigitos = null;
                solicitud.NumeroReferencia = null;
                return null;

            case Catalogos.MetodoPago.Tarjeta:
                if (solicitud.TipoTarjeta == null || !Catalogos.TiposTarjeta.Contains(solicitud.TipoTarjeta))
                {
                    return "Elegí el tipo de tarjeta.";
                }

                if (solicitud.UltimosCuatroDigitos is not { Length: 4 } digitos || !digitos.All(char.IsAsciiDigit))
                {
                    return "Escribí los últimos 4 dígitos de la tarjeta.";
                }

                solicitud.MontoRecibido = null;
                solicitud.CambioDevuelto = null;
                return null;

            case Catalogos.MetodoPago.Sinpe:
            case Catalogos.MetodoPago.Transferencia:
                if (solicitud.NumeroReferencia == null || solicitud.NumeroReferencia.Length > 50)
                {
                    return "Escribí el número de comprobante (máximo 50 caracteres).";
                }

                solicitud.MontoRecibido = null;
                solicitud.CambioDevuelto = null;
                solicitud.TipoTarjeta = null;
                solicitud.UltimosCuatroDigitos = null;
                return null;

            default:
                return "Elegí un método de pago válido.";
        }
    }
}
