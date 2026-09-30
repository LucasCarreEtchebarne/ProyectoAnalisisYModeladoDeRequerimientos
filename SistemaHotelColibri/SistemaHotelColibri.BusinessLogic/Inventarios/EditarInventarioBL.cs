using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Inventarios;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Inventarios;
using SistemaHotelColibri.Abstract.Modelos.Inventarios;

namespace SistemaHotelColibri.BusinessLogic.Inventarios;

public class EditarInventarioBL : IEditarInventarioBL
{
    private readonly IEditarInventarioDA _editarDA;
    private readonly IObtenerInventarioPorIdDA _obtenerPorIdDA;
    private readonly IExisteNombreInventarioDA _existeNombreDA;

    public EditarInventarioBL(
        IEditarInventarioDA editarDA,
        IObtenerInventarioPorIdDA obtenerPorIdDA,
        IExisteNombreInventarioDA existeNombreDA)
    {
        _editarDA = editarDA;
        _obtenerPorIdDA = obtenerPorIdDA;
        _existeNombreDA = existeNombreDA;
    }

    public async Task<ResultadoOperacion> EditarAsync(InventarioDto datos)
    {
        var producto = await _obtenerPorIdDA.ObtenerPorIdAsync(datos.IdProducto);
        if (producto == null)
        {
            return ResultadoOperacion.Error(Mensajes.NoEncontrado);
        }

        ReglasInventario.Normalizar(datos);

        var error = ReglasInventario.Validar(datos);
        if (error != null)
        {
            return ResultadoOperacion.Error(error);
        }

        if (await _existeNombreDA.ExisteNombreAsync(datos.NombreProducto, datos.IdProducto))
        {
            return ResultadoOperacion.Error(Mensajes.ProductoDuplicado);
        }

        producto.NombreProducto = datos.NombreProducto;
        producto.CategoriaProducto = datos.CategoriaProducto;
        producto.UnidadMedida = datos.UnidadMedida;
        producto.Stock = datos.Stock;
        producto.StockMinimo = datos.StockMinimo;
        producto.Descripcion = datos.Descripcion;

        if (!await _editarDA.EditarAsync(producto))
        {
            return ResultadoOperacion.Error(Mensajes.NoEncontrado);
        }

        return ResultadoOperacion.Ok(Mensajes.ActualizadoOk, producto.IdProducto);
    }
}
