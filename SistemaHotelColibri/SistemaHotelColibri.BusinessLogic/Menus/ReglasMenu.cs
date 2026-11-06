using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Modelos.Menus;

namespace SistemaHotelColibri.BusinessLogic.Menus;

internal static class ReglasMenu
{
    public static void Normalizar(MenuDto producto)
    {
        producto.NombreProducto = producto.NombreProducto.Trim();
        producto.CategoriaMenu = producto.CategoriaMenu.Trim();
        producto.Descripcion = string.IsNullOrWhiteSpace(producto.Descripcion) ? null : producto.Descripcion.Trim();
    }

    public static string? Validar(MenuDto producto)
    {
        if (string.IsNullOrWhiteSpace(producto.NombreProducto)
            || string.IsNullOrWhiteSpace(producto.CategoriaMenu)
            || string.IsNullOrWhiteSpace(producto.EstadoProductoMenu))
        {
            return Mensajes.MenuCamposIncompletos;
        }

        if (producto.Precio < 0)
        {
            return Mensajes.PrecioMenuInvalido;
        }

        if (!Estados.ProductoMenu.Todos.Contains(producto.EstadoProductoMenu))
        {
            return Mensajes.EstadoMenuInvalido;
        }

        return null;
    }
}
