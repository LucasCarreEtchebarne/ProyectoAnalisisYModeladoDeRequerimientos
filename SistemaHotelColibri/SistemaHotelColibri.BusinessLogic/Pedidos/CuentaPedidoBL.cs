using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Pedidos;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Pedidos;
using SistemaHotelColibri.Abstract.Modelos.Pedidos;

namespace SistemaHotelColibri.BusinessLogic.Pedidos;

// Agregar, quitar o anotar algo en una cuenta reutiliza AgregarPedidoBL y EditarPedidoBL,
// que ya validan la mesa o habitación, los productos y los precios.
public class CuentaPedidoBL : ICuentaPedidoBL
{
    private readonly IObtenerPedidosAbiertosDA _abiertasDA;
    private readonly IAgregarPedidoBL _agregarBL;
    private readonly IEditarPedidoBL _editarBL;

    public CuentaPedidoBL(IObtenerPedidosAbiertosDA abiertasDA, IAgregarPedidoBL agregarBL, IEditarPedidoBL editarBL)
    {
        _abiertasDA = abiertasDA;
        _agregarBL = agregarBL;
        _editarBL = editarBL;
    }

    public Task<List<PedidoDto>> ObtenerAbiertasAsync() => _abiertasDA.ObtenerAsync();

    public async Task<ResultadoOperacion> AbrirAsync(PedidoDto nueva, int idProductoMenu)
    {
        nueva.IdPedido = 0;
        nueva.Detalles = [new DetallePedidoDto { IdProductoMenu = idProductoMenu, Cantidad = 1 }];
        return await _agregarBL.AgregarAsync(nueva);
    }

    public async Task<ResultadoOperacion> AgregarProductoAsync(int idPedido, int idProductoMenu)
    {
        var cuenta = await ObtenerAbiertaAsync(idPedido);
        if (cuenta == null)
        {
            return CuentaCerrada();
        }

        var lineas = Agrupar(cuenta);
        lineas[idProductoMenu] = lineas.GetValueOrDefault(idProductoMenu) + 1;
        return await GuardarAsync(cuenta, lineas, cuenta.Observaciones);
    }

    public async Task<ResultadoOperacion> CambiarCantidadAsync(int idPedido, int idProductoMenu, int cambio)
    {
        var cuenta = await ObtenerAbiertaAsync(idPedido);
        if (cuenta == null)
        {
            return CuentaCerrada();
        }

        var lineas = Agrupar(cuenta);
        if (!lineas.ContainsKey(idProductoMenu))
        {
            return ResultadoOperacion.Error("Ese producto no está en la cuenta.");
        }

        lineas[idProductoMenu] += cambio;
        if (lineas[idProductoMenu] <= 0)
        {
            lineas.Remove(idProductoMenu);
        }

        // Si se quitó todo, la cuenta se cierra y la mesa o habitación queda libre.
        if (lineas.Count == 0)
        {
            var inactivado = await _editarBL.InactivarAsync(idPedido);
            return inactivado.Exito
                ? ResultadoOperacion.Ok("Se quitaron todos los productos y la cuenta se cerró.")
                : inactivado;
        }

        return await GuardarAsync(cuenta, lineas, cuenta.Observaciones);
    }

    public async Task<ResultadoOperacion> GuardarNotaAsync(int idPedido, string? observaciones)
    {
        var cuenta = await ObtenerAbiertaAsync(idPedido);
        if (cuenta == null)
        {
            return CuentaCerrada();
        }

        var resultado = await GuardarAsync(cuenta, Agrupar(cuenta), observaciones);
        return resultado.Exito ? ResultadoOperacion.Ok("Nota guardada.", idPedido) : resultado;
    }

    private async Task<PedidoDto?> ObtenerAbiertaAsync(int idPedido)
    {
        var abiertas = await _abiertasDA.ObtenerAsync();
        return abiertas.FirstOrDefault(pedido => pedido.IdPedido == idPedido);
    }

    private static ResultadoOperacion CuentaCerrada() =>
        ResultadoOperacion.Error("Esta cuenta ya se cobró o se cerró.");

    private static Dictionary<int, int> Agrupar(PedidoDto pedido) =>
        pedido.Detalles
            .GroupBy(detalle => detalle.IdProductoMenu)
            .ToDictionary(grupo => grupo.Key, grupo => grupo.Sum(detalle => detalle.Cantidad));

    private async Task<ResultadoOperacion> GuardarAsync(PedidoDto actual, Dictionary<int, int> lineas, string? observaciones)
    {
        // Se respeta el orden en que se fueron agregando los productos; los nuevos van al final.
        var orden = actual.Detalles.Select(detalle => detalle.IdProductoMenu).Distinct().ToList();

        var editado = new PedidoDto
        {
            IdPedido = actual.IdPedido,
            IdUsuario = actual.IdUsuario,
            IdCliente = actual.IdCliente,
            TipoPedido = actual.TipoPedido,
            IdMesa = actual.IdMesa,
            IdHabitacion = actual.IdHabitacion,
            IdReserva = actual.IdReserva,
            Observaciones = observaciones,
            Detalles = lineas
                .OrderBy(linea => orden.IndexOf(linea.Key) is var i && i >= 0 ? i : int.MaxValue)
                .Select(linea => new DetallePedidoDto { IdProductoMenu = linea.Key, Cantidad = linea.Value })
                .ToList()
        };

        var resultado = await _editarBL.EditarAsync(editado);
        return resultado.Exito ? ResultadoOperacion.Ok("Cuenta actualizada.", actual.IdPedido) : resultado;
    }
}
