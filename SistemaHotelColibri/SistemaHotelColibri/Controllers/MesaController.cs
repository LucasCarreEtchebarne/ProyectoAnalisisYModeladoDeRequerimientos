using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Mesas;
using SistemaHotelColibri.ViewModels.Comun;
using SistemaHotelColibri.ViewModels.Mesas;

namespace SistemaHotelColibri.Controllers;

[Authorize(Roles = Roles.AccesoMesas)]
public class MesaController : ControladorBase
{
    private readonly IAgregarMesaBL _agregarBL;
    private readonly IObtenerMesaBL _obtenerBL;
    private readonly IObtenerMesaPorIdBL _obtenerPorIdBL;
    private readonly IEditarMesaBL _editarBL;
    private readonly IEliminarMesaBL _eliminarBL;

    public MesaController(
        IAgregarMesaBL agregarBL,
        IObtenerMesaBL obtenerBL,
        IObtenerMesaPorIdBL obtenerPorIdBL,
        IEditarMesaBL editarBL,
        IEliminarMesaBL eliminarBL)
    {
        _agregarBL = agregarBL;
        _obtenerBL = obtenerBL;
        _obtenerPorIdBL = obtenerPorIdBL;
        _editarBL = editarBL;
        _eliminarBL = eliminarBL;
    }

    public async Task<IActionResult> Index(string? busqueda, string? estado, int pagina = 1)
    {
        var estadoSeleccionado = estado ?? Estados.Mesa.Activas;

        var filtro = new FiltroListado
        {
            Busqueda = busqueda,
            Estado = estadoSeleccionado == FiltroListado.TodosLosEstados ? null : estadoSeleccionado,
            Pagina = pagina
        };

        ViewBag.Busqueda = new BarraBusquedaViewModel
        {
            Busqueda = busqueda,
            Placeholder = "Número, estado o descripción",
            Filtros =
            [
                FiltroSeleccion.Crear("estado", "Estado",
                    Estados.Mesa.Todos.Prepend(Estados.Mesa.Activas).Append(FiltroListado.TodosLosEstados),
                    estadoSeleccionado)
            ]
        };
        return View(await _obtenerBL.ObtenerAsync(filtro));
    }

    public async Task<IActionResult> Detalle(int id)
    {
        var mesa = await _obtenerPorIdBL.ObtenerPorIdAsync(id);
        if (mesa == null)
        {
            MensajeError(Mensajes.NoEncontrado);
            return RedirectToAction(nameof(Index));
        }

        return View(mesa);
    }

    public IActionResult Crear()
    {
        ViewBag.EstadosMesa = Estados.Mesa.Editables;
        return View(new MesaFormViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> Crear(MesaFormViewModel modelo)
    {
        ViewBag.EstadosMesa = Estados.Mesa.Editables;
        if (!ModelState.IsValid)
        {
            return View(modelo);
        }

        var resultado = await _agregarBL.AgregarAsync(modelo.ADto());
        if (!resultado.Exito)
        {
            ModelState.AddModelError(string.Empty, resultado.Mensaje);
            return View(modelo);
        }

        MensajeExito(resultado.Mensaje);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Editar(int id)
    {
        var mesa = await _obtenerPorIdBL.ObtenerPorIdAsync(id);
        if (mesa == null)
        {
            MensajeError(Mensajes.NoEncontrado);
            return RedirectToAction(nameof(Index));
        }

        ViewBag.EstadosMesa = Estados.Mesa.Todos;
        return View(MesaFormViewModel.DesdeDto(mesa));
    }

    [HttpPost]
    public async Task<IActionResult> Editar(int id, MesaFormViewModel modelo)
    {
        modelo.IdMesa = id;
        ViewBag.EstadosMesa = Estados.Mesa.Todos;

        if (!ModelState.IsValid)
        {
            return View(modelo);
        }

        var resultado = await _editarBL.EditarAsync(modelo.ADto());
        if (!resultado.Exito)
        {
            ModelState.AddModelError(string.Empty, resultado.Mensaje);
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
