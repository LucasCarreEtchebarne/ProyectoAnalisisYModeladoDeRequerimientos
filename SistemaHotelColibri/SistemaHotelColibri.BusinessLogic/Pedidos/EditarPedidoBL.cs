using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Pedidos;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Pedidos;
using SistemaHotelColibri.Abstract.Modelos.Pedidos;

namespace SistemaHotelColibri.BusinessLogic.Pedidos;

public class EditarPedidoBL : IEditarPedidoBL
{
    private readonly IEditarPedidoDA _editarPedidoDA;
    private readonly IObtenerPedidoPorIdDA _obtenerPedidoPorIdDA;
    private readonly IValidadorPedidoBL _validador;

    public EditarPedidoBL(
        IEditarPedidoDA editarPedidoDA,
        IObtenerPedidoPorIdDA obtenerPedidoPorIdDA,
        IValidadorPedidoBL validador)
    {
        _editarPedidoDA = editarPedidoDA;
        _obtenerPedidoPorIdDA = obtenerPedidoPorIdDA;
        _validador = validador;
    }

    public async Task<ResultadoOperacion> EditarAsync(PedidoDto pedido)
    {
        var actual = await _obtenerPedidoPorIdDA.ObtenerPorIdAsync(pedido.IdPedido);
        if (actual == null)
        {
            return ResultadoOperacion.Error(Mensajes.NoEncontrado);
        }

        if (actual.EstadoPedido != Estados.Pedido.Pendiente)
        {
            return ResultadoOperacion.Error("Solo se pueden modificar pedidos en estado Pendiente.");
        }

        var error = await _validador.ValidarAsync(pedido);
        if (error != null)
        {
            return ResultadoOperacion.Error(error);
        }

        var editado = await _editarPedidoDA.EditarAsync(pedido);

        return editado
            ? ResultadoOperacion.Ok($"El pedido #{pedido.IdPedido} se actualizó correctamente.", pedido.IdPedido)
            : ResultadoOperacion.Error("No se pudo modificar el pedido porque ya fue facturado.");
    }

    public async Task<ResultadoOperacion> InactivarAsync(int idPedido)
    {
        var actual = await _obtenerPedidoPorIdDA.ObtenerPorIdAsync(idPedido);
        if (actual == null)
        {
            return ResultadoOperacion.Error(Mensajes.NoEncontrado);
        }

        if (actual.EstadoPedido == Estados.Pedido.Inactivo)
        {
            return ResultadoOperacion.Error("El pedido ya está inactivo.");
        }

        if (actual.EstadoPedido != Estados.Pedido.Pendiente)
        {
            return ResultadoOperacion.Error("Solo se pueden inactivar pedidos en estado Pendiente.");
        }

        var inactivado = await _editarPedidoDA.InactivarAsync(idPedido);

        return inactivado
            ? ResultadoOperacion.Ok($"El pedido #{idPedido} se inactivó correctamente.")
            : ResultadoOperacion.Error("No se pudo inactivar el pedido porque ya fue facturado.");
    }
}