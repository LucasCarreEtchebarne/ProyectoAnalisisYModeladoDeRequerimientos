using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Modelos.Inventarios;

namespace SistemaHotelColibri.BusinessLogic.Inventarios;

internal static class ReglasInventario
{
    public static void Normalizar(InventarioDto producto)
    {
        producto.NombreProducto = producto.NombreProducto.Trim();
        producto.CategoriaProducto = producto.CategoriaProducto.Trim();
        producto.Descripcion = string.IsNullOrWhiteSpace(producto.Descripcion) ? null : producto.Descripcion.Trim();
    }

    public static string? Validar(InventarioDto producto)
    {
        if (producto.Stock < 0)
        {
            return Mensajes.StockNegativo;
        }

        if (producto.StockMinimo < 0)
        {
            return Mensajes.StockMinimoNegativo;
        }

        if (!Catalogos.UnidadesMedida.Contains(producto.UnidadMedida))
        {
            return Mensajes.UnidadMedidaInvalida;
        }

        return null;
    }
}
