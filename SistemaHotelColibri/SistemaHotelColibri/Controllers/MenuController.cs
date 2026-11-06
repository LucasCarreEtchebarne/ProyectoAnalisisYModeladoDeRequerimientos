using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Menus;
using SistemaHotelColibri.ViewModels.Comun;
using SistemaHotelColibri.ViewModels.Menus;

namespace SistemaHotelColibri.Controllers;

[Authorize(Roles = Roles.AccesoMenu)]
public class MenuController : ControladorBase
{
    private readonly IAgregarMenuBL _agregarBL;
    private readonly IObtenerMenuBL _obtenerBL;
    private readonly IObtenerMenuPorIdBL _obtenerPorIdBL;
    private readonly IEditarMenuBL _editarBL;
    private readonly IEliminarMenuBL _eliminarBL;

    public MenuController(
        IAgregarMenuBL agregarBL,
        IObtenerMenuBL obtenerBL,
        IObtenerMenuPorIdBL obtenerPorIdBL,
        IEditarMenuBL editarBL,
        IEliminarMenuBL eliminarBL)
    {
        _agregarBL = agregarBL;
        _obtenerBL = obtenerBL;
        _obtenerPorIdBL = obtenerPorIdBL;
        _editarBL = editarBL;
        _eliminarBL = eliminarBL;
    }

    private void CargarCatalogos()
    {
        ViewBag.Categorias = Catalogos.CategoriasMenu;
        ViewBag.Estados = Estados.ProductoMenu.Todos;
    }

    private bool EsAdministrador => User.IsInRole(Roles.Administrador);

    public async Task<IActionResult> Index(string? busqueda, string? estado, string? categoria, int pagina = 1)
    {
        var estadoSeleccionado = estado ?? FiltroListado.TodosLosEstados;
        var categoriaSeleccionada = categoria ?? FiltroListado.TodosLosEstados;

        var filtro = new FiltroListado
        {
            Busqueda = busqueda,
            Estado = estadoSeleccionado == FiltroListado.TodosLosEstados ? null : estadoSeleccionado,
            Categoria = categoriaSeleccionada == FiltroListado.TodosLosEstados ? null : categoriaSeleccionada,
            Pagina = pagina
        };

        ViewBag.Busqueda = new BarraBusquedaViewModel
        {
            Busqueda = busqueda,
            Placeholder = "Nombre, categoría o descripción",
            Filtros =
            [
                FiltroSeleccion.Crear("categoria", "Categoría",
                    Catalogos.CategoriasMenu.Append(FiltroListado.TodosLosEstados), categoriaSeleccionada),
                FiltroSeleccion.Crear("estado", "Estado",
                    Estados.ProductoMenu.Todos.Append(FiltroListado.TodosLosEstados), estadoSeleccionado)
            ]
        };
        ViewBag.EsAdministrador = EsAdministrador;
        return View(await _obtenerBL.ObtenerAsync(filtro));
    }

    public async Task<IActionResult> Detalle(int id)
    {
        var producto = await _obtenerPorIdBL.ObtenerPorIdAsync(id);
        if (producto == null)
        {
            MensajeError(Mensajes.NoEncontrado);
            return RedirectToAction(nameof(Index));
        }

        ViewBag.EsAdministrador = EsAdministrador;
        return View(producto);
    }

    [Authorize(Roles = Roles.Administrador)]
    public IActionResult Crear()
    {
        CargarCatalogos();
        return View(new MenuFormViewModel());
    }

    [HttpPost]
    [Authorize(Roles = Roles.Administrador)]
    public async Task<IActionResult> Crear(MenuFormViewModel modelo)
    {
        if (!ModelState.IsValid)
        {
            CargarCatalogos();
            return View(modelo);
        }

        var resultado = await _agregarBL.AgregarAsync(modelo.ADto());
        if (!resultado.Exito)
        {
            ModelState.AddModelError(string.Empty, resultado.Mensaje);
            CargarCatalogos();
            return View(modelo);
        }

        MensajeExito(resultado.Mensaje);
        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = Roles.Administrador)]
    public async Task<IActionResult> Editar(int id)
    {
        var producto = await _obtenerPorIdBL.ObtenerPorIdAsync(id);
        if (producto == null)
        {
            MensajeError(Mensajes.NoEncontrado);
            return RedirectToAction(nameof(Index));
        }

        CargarCatalogos();
        return View(MenuFormViewModel.DesdeDto(producto));
    }

    [HttpPost]
    [Authorize(Roles = Roles.Administrador)]
    public async Task<IActionResult> Editar(int id, MenuFormViewModel modelo)
    {
        modelo.IdProductoMenu = id;

        if (!ModelState.IsValid)
        {
            CargarCatalogos();
            return View(modelo);
        }

        var resultado = await _editarBL.EditarAsync(modelo.ADto());
        if (!resultado.Exito)
        {
            ModelState.AddModelError(string.Empty, resultado.Mensaje);
            CargarCatalogos();
            return View(modelo);
        }

        MensajeExito(resultado.Mensaje);
        return RedirectToAction(nameof(Detalle), new { id });
    }

    [HttpPost]
    [Authorize(Roles = Roles.Administrador)]
    public async Task<IActionResult> Eliminar(int id)
    {
        MostrarResultado(await _eliminarBL.EliminarAsync(id));
        return RedirectToAction(nameof(Index));
    }
}
