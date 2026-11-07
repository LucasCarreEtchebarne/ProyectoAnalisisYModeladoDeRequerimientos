using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Menus;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Menus;

namespace SistemaHotelColibri.BusinessLogic.Menus;

public class EliminarMenuBL : IEliminarMenuBL
{
    private readonly IEliminarMenuDA _eliminarDA;
    private readonly IObtenerMenuPorIdDA _obtenerPorIdDA;

    public EliminarMenuBL(IEliminarMenuDA eliminarDA, IObtenerMenuPorIdDA obtenerPorIdDA)
    {
        _eliminarDA = eliminarDA;
        _obtenerPorIdDA = obtenerPorIdDA;
    }

    public async Task<ResultadoOperacion> EliminarAsync(int idProductoMenu)
    {
        var producto = await _obtenerPorIdDA.ObtenerPorIdAsync(idProductoMenu);
        if (producto == null)
        {
            return ResultadoOperacion.Error(Mensajes.NoEncontrado);
        }

        if (producto.EstadoProductoMenu == Estados.ProductoMenu.Inactivo)
        {
            return ResultadoOperacion.Error(Mensajes.YaInactivo);
        }

        if (!await _eliminarDA.EliminarAsync(idProductoMenu))
        {
            return ResultadoOperacion.Error(Mensajes.NoEncontrado);
        }

        return ResultadoOperacion.Ok(Mensajes.EliminadoOk, idProductoMenu);
    }
}
