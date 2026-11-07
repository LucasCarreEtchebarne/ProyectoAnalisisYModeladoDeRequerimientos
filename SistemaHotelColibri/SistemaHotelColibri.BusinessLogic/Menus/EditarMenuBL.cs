using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Menus;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Menus;
using SistemaHotelColibri.Abstract.Modelos.Menus;

namespace SistemaHotelColibri.BusinessLogic.Menus;

public class EditarMenuBL : IEditarMenuBL
{
    private readonly IEditarMenuDA _editarDA;
    private readonly IObtenerMenuPorIdDA _obtenerPorIdDA;
    private readonly IExisteNombreMenuDA _existeNombreDA;

    public EditarMenuBL(
        IEditarMenuDA editarDA,
        IObtenerMenuPorIdDA obtenerPorIdDA,
        IExisteNombreMenuDA existeNombreDA)
    {
        _editarDA = editarDA;
        _obtenerPorIdDA = obtenerPorIdDA;
        _existeNombreDA = existeNombreDA;
    }

    public async Task<ResultadoOperacion> EditarAsync(MenuDto datos)
    {
        var producto = await _obtenerPorIdDA.ObtenerPorIdAsync(datos.IdProductoMenu);
        if (producto == null)
        {
            return ResultadoOperacion.Error(Mensajes.NoEncontrado);
        }

        ReglasMenu.Normalizar(datos);

        var error = ReglasMenu.Validar(datos);
        if (error != null)
        {
            return ResultadoOperacion.Error(error);
        }

        if (await _existeNombreDA.ExisteNombreAsync(datos.NombreProducto, datos.IdProductoMenu))
        {
            return ResultadoOperacion.Error(Mensajes.MenuDuplicado);
        }

        producto.NombreProducto = datos.NombreProducto;
        producto.CategoriaMenu = datos.CategoriaMenu;
        producto.Precio = datos.Precio;
        producto.EstadoProductoMenu = datos.EstadoProductoMenu;
        producto.Descripcion = datos.Descripcion;

        try
        {
            if (!await _editarDA.EditarAsync(producto))
            {
                return ResultadoOperacion.Error(Mensajes.NoEncontrado);
            }
        }
        catch (Exception)
        {
            return ResultadoOperacion.Error(Mensajes.ErrorModificarMenu);
        }

        return ResultadoOperacion.Ok(Mensajes.ActualizadoOk, producto.IdProductoMenu);
    }
}
