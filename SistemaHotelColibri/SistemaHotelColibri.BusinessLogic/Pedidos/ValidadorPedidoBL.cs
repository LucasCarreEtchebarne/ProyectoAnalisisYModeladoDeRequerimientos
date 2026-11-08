using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Pedidos;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Clientes;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Menus;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Mesas;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Reservas;
using SistemaHotelColibri.Abstract.Modelos.Pedidos;

namespace SistemaHotelColibri.BusinessLogic.Pedidos;

public class ValidadorPedidoBL : IValidadorPedidoBL
{
    private readonly IObtenerMesaPorIdDA _obtenerMesaDA;
    private readonly IObtenerReservaPorIdDA _obtenerReservaDA;
    private readonly IObtenerClientePorIdDA _obtenerClienteDA;
    private readonly IObtenerMenuPorIdDA _obtenerMenuDA;

    public ValidadorPedidoBL(
        IObtenerMesaPorIdDA obtenerMesaDA,
        IObtenerReservaPorIdDA obtenerReservaDA,
        IObtenerClientePorIdDA obtenerClienteDA,
        IObtenerMenuPorIdDA obtenerMenuDA)
    {
        _obtenerMesaDA = obtenerMesaDA;
        _obtenerReservaDA = obtenerReservaDA;
        _obtenerClienteDA = obtenerClienteDA;
        _obtenerMenuDA = obtenerMenuDA;
    }

    public async Task<string?> ValidarAsync(PedidoDto pedido)
    {
        pedido.Observaciones = string.IsNullOrWhiteSpace(pedido.Observaciones)
            ? null
            : pedido.Observaciones.Trim();

        if (pedido.Observaciones?.Length > 250)
        {
            return "Las observaciones pueden tener como máximo 250 caracteres.";
        }

        if (pedido.Detalles.Count == 0)
        {
            return "Agregá al menos un producto al pedido.";
        }

        if (pedido.Detalles.Any(detalle => detalle.IdProductoMenu <= 0 || detalle.Cantidad <= 0))
        {
            return "Cada línea debe tener un producto y una cantidad mayor a cero.";
        }

        var errorOrigen = await ValidarOrigenAsync(pedido);
        if (errorOrigen != null)
        {
            return errorOrigen;
        }

        if (pedido.IdCliente.HasValue)
        {
            var cliente = await _obtenerClienteDA.ObtenerPorIdAsync(pedido.IdCliente.Value);
            if (cliente == null)
            {
                return "El cliente seleccionado no existe.";
            }
        }

        foreach (var detalle in pedido.Detalles)
        {
            var producto = await _obtenerMenuDA.ObtenerPorIdAsync(detalle.IdProductoMenu);

            if (producto == null)
            {
                return "Uno de los productos seleccionados no existe.";
            }

            if (producto.EstadoProductoMenu != Estados.ProductoMenu.Disponible)
            {
                return $"El producto «{producto.NombreProducto}» no está disponible.";
            }

            detalle.PrecioUnitario = producto.Precio;
        }

        return null;
    }

    private async Task<string?> ValidarOrigenAsync(PedidoDto pedido)
    {
        switch (pedido.TipoPedido)
        {
            case Catalogos.TipoPedido.Mesa:
                {
                    if (pedido.IdMesa is not > 0)
                    {
                        return "Seleccioná una mesa para el pedido.";
                    }

                    var mesa = await _obtenerMesaDA.ObtenerPorIdAsync(pedido.IdMesa.Value);
                    if (mesa == null || mesa.EstadoMesa == Estados.Mesa.Inactiva)
                    {
                        return "La mesa seleccionada no existe o está inactiva.";
                    }

                    pedido.IdHabitacion = null;
                    pedido.IdReserva = null;
                    return null;
                }

            case Catalogos.TipoPedido.Habitacion:
                {
                    if (pedido.IdReserva is not > 0)
                    {
                        return "Seleccioná una reserva con check-in para el pedido.";
                    }

                    var reserva = await _obtenerReservaDA.ObtenerPorIdAsync(pedido.IdReserva.Value);
                    if (reserva == null || reserva.EstadoReserva != Estados.Reserva.CheckIn)
                    {
                        return "La reserva seleccionada debe estar en check-in.";
                    }

                    pedido.IdMesa = null;
                    pedido.IdHabitacion = reserva.IdHabitacion;
                    pedido.IdCliente = reserva.IdCliente;
                    return null;
                }

            case Catalogos.TipoPedido.Llevar:
                pedido.IdMesa = null;
                pedido.IdHabitacion = null;
                pedido.IdReserva = null;
                return null;

            default:
                return "Seleccioná un tipo de pedido válido.";
        }
    }
}