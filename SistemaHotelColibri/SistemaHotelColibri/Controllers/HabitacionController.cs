using Microsoft.AspNetCore.Mvc;
using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Habitaciones;
using SistemaHotelColibri.ViewModels.Habitaciones;

namespace SistemaHotelColibri.Controllers;

public class HabitacionController : ControladorBase
{
    private const string TodosLosEstados = "Todos";

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

    public async Task<IActionResult> Index(string? estado)
    {
        var estadoSeleccionado = estado ?? TodosLosEstados;
        var filtro = estadoSeleccionado == TodosLosEstados ? null : estadoSeleccionado;

        ViewBag.EstadoSeleccionado = estadoSeleccionado;
        ViewBag.OpcionesEstado = Estados.Habitacion.Todos.Append(TodosLosEstados).ToArray();
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
        ViewBag.EstadosHabitacion = Estados.Habitacion.Todos;
        return View(new HabitacionFormViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> Crear(HabitacionFormViewModel modelo)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.TiposHabitacion = Catalogos.TiposHabitacion;
            ViewBag.EstadosHabitacion = Estados.Habitacion.Todos;
            return View(modelo);
        }

        var resultado = await _agregarBL.AgregarAsync(modelo.ADto());
        if (!resultado.Exito)
        {
            ModelState.AddModelError(string.Empty, resultado.Mensaje);
            ViewBag.TiposHabitacion = Catalogos.TiposHabitacion;
            ViewBag.EstadosHabitacion = Estados.Habitacion.Todos;
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
