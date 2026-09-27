using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Inventarios;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Inventarios;

namespace SistemaHotelColibri.BusinessLogic.Inventarios;

public class EliminarInventarioBL : IEliminarInventarioBL
{
    private readonly IEliminarInventarioDA _eliminarDA;
    private readonly IObtenerInventarioPorIdDA _obtenerPorIdDA;

    public EliminarInventarioBL(IEliminarInventarioDA eliminarDA, IObtenerInventarioPorIdDA obtenerPorIdDA)
    {
        _eliminarDA = eliminarDA;
        _obtenerPorIdDA = obtenerPorIdDA;
    }

    public async Task<ResultadoOperacion> EliminarAsync(int idProducto)
    {
        var producto = await _obtenerPorIdDA.ObtenerPorIdAsync(idProducto);
        if (producto == null)
        {
            return ResultadoOperacion.Error(Mensajes.NoEncontrado);
        }

        if (producto.EstadoProducto == Estados.ProductoInventario.Inactivo)
        {
            return ResultadoOperacion.Error(Mensajes.YaInactivo);
        }

        if (!await _eliminarDA.EliminarAsync(idProducto))
        {
            return ResultadoOperacion.Error(Mensajes.NoEncontrado);
        }

        return ResultadoOperacion.Ok(Mensajes.EliminadoOk, idProducto);
    }
}
