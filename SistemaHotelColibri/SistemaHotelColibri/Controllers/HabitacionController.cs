using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Habitaciones;
using SistemaHotelColibri.ViewModels.Comun;
using SistemaHotelColibri.ViewModels.Habitaciones;

namespace SistemaHotelColibri.Controllers;

[Authorize(Roles = Roles.AccesoHospedaje)]
public class HabitacionController : ControladorBase
{
    private readonly IAgregarHabitacionBL _agregarBL;
    private readonly IObtenerHabitacionBL _obtenerBL;
    private readonly IObtenerHabitacionPorIdBL _obtenerPorIdBL;
    private readonly IEditarHabitacionBL _editarBL;
    private readonly IEliminarHabitacionBL _eliminarBL;

    public HabitacionController(
        IAgregarHabitacionBL agregarBL,
        IObtenerHabitacionBL obtenerBL,
        IObtenerHabitacionPorIdBL obtenerPorIdBL,
        IEditarHabitacionBL editarBL,
        IEliminarHabitacionBL eliminarBL)
    {
        _agregarBL = agregarBL;
        _obtenerBL = obtenerBL;
        _obtenerPorIdBL = obtenerPorIdBL;
        _editarBL = editarBL;
        _eliminarBL = eliminarBL;
    }

    public async Task<IActionResult> Index(string? busqueda, string? estado, int pagina = 1)
    {
        var estadoSeleccionado = estado ?? Estados.Habitacion.Activas;

        var filtro = new FiltroListado
        {
            Busqueda = busqueda,
            Estado = estadoSeleccionado == FiltroListado.TodosLosEstados ? null : estadoSeleccionado,
            Pagina = pagina
        };

        ViewBag.Busqueda = new BarraBusquedaViewModel
        {
            Busqueda = busqueda,
            Placeholder = "Número, tipo, tipo de cama, vista o descripción",
            Filtros =
            [
                FiltroSeleccion.Crear("estado", "Estado",
                    Estados.Habitacion.Todos.Prepend(Estados.Habitacion.Activas).Append(FiltroListado.TodosLosEstados),
                    estadoSeleccionado)
            ]
        };
        return View(await _obtenerBL.ObtenerAsync(filtro));
    }

    public async Task<IActionResult> Detalle(int id)
    {
        var habitacion = await _obtenerPorIdBL.ObtenerPorIdAsync(id);
        if (habitacion == null)
        {
            MensajeError(Mensajes.NoEncontrado);
            return RedirectToAction(nameof(Index));
        }

        return View(habitacion);
    }

    public IActionResult Crear()
    {
        ViewBag.TiposHabitacion = Catalogos.TiposHabitacion;
        ViewBag.EstadosHabitacion = Estados.Habitacion.Editables;
        return View(new HabitacionFormViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> Crear(HabitacionFormViewModel modelo)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.TiposHabitacion = Catalogos.TiposHabitacion;
            ViewBag.EstadosHabitacion = Estados.Habitacion.Editables;
            return View(modelo);
        }

        var resultado = await _agregarBL.AgregarAsync(modelo.ADto());
        if (!resultado.Exito)
        {
            ModelState.AddModelError(string.Empty, resultado.Mensaje);
            ViewBag.TiposHabitacion = Catalogos.TiposHabitacion;
            ViewBag.EstadosHabitacion = Estados.Habitacion.Editables;
            return View(modelo);
        }

        MensajeExito(resultado.Mensaje);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Editar(int id)
    {
        var habitacion = await _obtenerPorIdBL.ObtenerPorIdAsync(id);
        if (habitacion == null)
        {
            MensajeError(Mensajes.NoEncontrado);
            return RedirectToAction(nameof(Index));
        }

        ViewBag.TiposHabitacion = Catalogos.TiposHabitacion;
        ViewBag.EstadosHabitacion = Estados.Habitacion.Todos;
        return View(HabitacionFormViewModel.DesdeDto(habitacion));
    }

    [HttpPost]
    public async Task<IActionResult> Editar(int id, HabitacionFormViewModel modelo)
    {
        modelo.IdHabitacion = id;

        if (!ModelState.IsValid)
        {
            ViewBag.TiposHabitacion = Catalogos.TiposHabitacion;
            ViewBag.EstadosHabitacion = Estados.Habitacion.Todos;
            return View(modelo);
        }

        var resultado = await _editarBL.EditarAsync(modelo.ADto());
        if (!resultado.Exito)
        {
            ModelState.AddModelError(string.Empty, resultado.Mensaje);
            ViewBag.TiposHabitacion = Catalogos.TiposHabitacion;
            ViewBag.EstadosHabitacion = Estados.Habitacion.Todos;
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
