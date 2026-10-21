using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Inventarios;
using SistemaHotelColibri.ViewModels.Comun;
using SistemaHotelColibri.ViewModels.Inventarios;

namespace SistemaHotelColibri.Controllers;

[Authorize(Roles = Roles.AccesoInventario)]
public class InventarioController : ControladorBase
{
    private readonly IAgregarInventarioBL _agregarBL;
    private readonly IObtenerInventarioBL _obtenerBL;
    private readonly IObtenerInventarioPorIdBL _obtenerPorIdBL;
    private readonly IEditarInventarioBL _editarBL;
    private readonly IEliminarInventarioBL _eliminarBL;
    private readonly IObtenerAlertasInventarioBL _obtenerAlertasBL;
    private readonly IObtenerCategoriaInventarioBL _obtenerCategoriasBL;

    public InventarioController(
        IAgregarInventarioBL agregarBL,
        IObtenerInventarioBL obtenerBL,
        IObtenerInventarioPorIdBL obtenerPorIdBL,
        IEditarInventarioBL editarBL,
        IEliminarInventarioBL eliminarBL,
        IObtenerAlertasInventarioBL obtenerAlertasBL,
        IObtenerCategoriaInventarioBL obtenerCategoriasBL)
    {
        _agregarBL = agregarBL;
        _obtenerBL = obtenerBL;
        _obtenerPorIdBL = obtenerPorIdBL;
        _editarBL = editarBL;
        _eliminarBL = eliminarBL;
        _obtenerAlertasBL = obtenerAlertasBL;
        _obtenerCategoriasBL = obtenerCategoriasBL;
    }

    private async Task CargarCatalogosAsync()
    {
        ViewBag.UnidadesMedida = Catalogos.UnidadesMedida;
        ViewBag.Categorias = await _obtenerCategoriasBL.ObtenerAsync(Estados.CategoriaInventario.Activo);
    }

    public async Task<IActionResult> Index(string? busqueda, string? estado, int pagina = 1)
    {
        var estadoSeleccionado = estado ?? Estados.ProductoInventario.Activo;

        var filtro = new FiltroListado
        {
            Busqueda = busqueda,
            Estado = estadoSeleccionado == FiltroListado.TodosLosEstados ? null : estadoSeleccionado,
            Pagina = pagina
        };

        ViewBag.Busqueda = new BarraBusquedaViewModel
        {
            Busqueda = busqueda,
            Placeholder = "Nombre, categoría, proveedor o descripción",
            Filtros =
            [
                FiltroSeleccion.Crear("estado", "Estado",
                    Estados.ProductoInventario.Todos.Append(FiltroListado.TodosLosEstados), estadoSeleccionado)
            ]
        };
        ViewBag.TotalAlertas = (await _obtenerAlertasBL.ObtenerAlertasAsync()).Count;
        return View(await _obtenerBL.ObtenerAsync(filtro));
    }

    public async Task<IActionResult> Alertas()
    {
        return View(await _obtenerAlertasBL.ObtenerAlertasAsync());
    }

    public async Task<IActionResult> Detalle(int id)
    {
        var producto = await _obtenerPorIdBL.ObtenerPorIdAsync(id);
        if (producto == null)
        {
            MensajeError(Mensajes.NoEncontrado);
            return RedirectToAction(nameof(Index));
        }

        return View(producto);
    }

    public async Task<IActionResult> Crear()
    {
        await CargarCatalogosAsync();
        return View(new InventarioFormViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> Crear(InventarioFormViewModel modelo)
    {
        if (!ModelState.IsValid)
        {
            await CargarCatalogosAsync();
            return View(modelo);
        }

        var resultado = await _agregarBL.AgregarAsync(modelo.ADto());
        if (!resultado.Exito)
        {
            ModelState.AddModelError(string.Empty, resultado.Mensaje);
            await CargarCatalogosAsync();
            return View(modelo);
        }

        MensajeExito(resultado.Mensaje);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Editar(int id)
    {
        var producto = await _obtenerPorIdBL.ObtenerPorIdAsync(id);
        if (producto == null)
        {
            MensajeError(Mensajes.NoEncontrado);
            return RedirectToAction(nameof(Index));
        }

        await CargarCatalogosAsync();
        return View(InventarioFormViewModel.DesdeDto(producto));
    }

    [HttpPost]
    public async Task<IActionResult> Editar(int id, InventarioFormViewModel modelo)
    {
        modelo.IdProducto = id;

        if (!ModelState.IsValid)
        {
            await CargarCatalogosAsync();
            return View(modelo);
        }

        var resultado = await _editarBL.EditarAsync(modelo.ADto());
        if (!resultado.Exito)
        {
            ModelState.AddModelError(string.Empty, resultado.Mensaje);
            await CargarCatalogosAsync();
            return View(modelo);
        }

        MensajeExito(resultado.Mensaje);
        return RedirectToAction(nameof(Detalle), new { id });
    }

    [HttpPost]
    public async Task<IActionResult> Eliminar(int id)
    {
        MostrarResultado(await _eliminarBL.EliminarAsync(id));
        return RedirectToAction(nameof(Index));
    }
}
