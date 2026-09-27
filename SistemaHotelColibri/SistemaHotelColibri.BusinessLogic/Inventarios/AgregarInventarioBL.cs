using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Inventarios;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Inventarios;
using SistemaHotelColibri.Abstract.Modelos.Inventarios;

namespace SistemaHotelColibri.BusinessLogic.Inventarios;

public class AgregarInventarioBL : IAgregarInventarioBL
{
    private readonly IAgregarInventarioDA _agregarDA;
    private readonly IExisteNombreInventarioDA _existeNombreDA;

    public AgregarInventarioBL(IAgregarInventarioDA agregarDA, IExisteNombreInventarioDA existeNombreDA)
    {
        _agregarDA = agregarDA;
        _existeNombreDA = existeNombreDA;
    }

    public async Task<ResultadoOperacion> AgregarAsync(InventarioDto producto)
    {
        producto.NombreProducto = producto.NombreProducto.Trim();
        producto.CategoriaProducto = producto.CategoriaProducto.Trim();
        producto.Descripcion = string.IsNullOrWhiteSpace(producto.Descripcion) ? null : producto.Descripcion.Trim();

        if (producto.Stock < 0)
        {
            return ResultadoOperacion.Error(Mensajes.StockNegativo);
        }

        if (producto.StockMinimo < 0)
        {
            return ResultadoOperacion.Error(Mensajes.StockMinimoNegativo);
        }

        if (!Catalogos.UnidadesMedida.Contains(producto.UnidadMedida))
        {
            return ResultadoOperacion.Error(Mensajes.UnidadMedidaInvalida);
        }

        if (await _existeNombreDA.ExisteNombreAsync(producto.NombreProducto, 0))
        {
            return ResultadoOperacion.Error(Mensajes.ProductoDuplicado);
        }

        producto.IdProducto = 0;
        producto.EstadoProducto = Estados.ProductoInventario.Activo;
        producto.FechaIngreso = DateOnly.FromDateTime(DateTime.Today);

        var idGenerado = await _agregarDA.AgregarAsync(producto);

        return ResultadoOperacion.Ok(string.Format(Mensajes.ProductoRegistrado, idGenerado), idGenerado);
    }
}
