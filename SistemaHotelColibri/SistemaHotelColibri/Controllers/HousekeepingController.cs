using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.TareasHousekeeping;
using SistemaHotelColibri.ViewModels.TareasHousekeeping;

namespace SistemaHotelColibri.Controllers;

[Authorize(Roles = Roles.AccesoHousekeeping)]
public class HousekeepingController : ControladorBase
{
    private const string TodosLosEstados = "Todos";

    private readonly IAgregarTareaBL _agregarBL;
    private readonly IObtenerTareaBL _obtenerBL;
    private readonly IObtenerTareaPorIdBL _obtenerPorIdBL;
    private readonly IEditarTareaBL _editarBL;
    private readonly ICambiarEstadoTareaBL _cambiarEstadoBL;
    private readonly ICancelarTareaBL _cancelarBL;
    private readonly IObtenerPersonalHousekeepingBL _obtenerPersonalBL;
    private readonly IObtenerHabitacionesTareaBL _obtenerHabitacionesBL;

    public HousekeepingController(
        IAgregarTareaBL agregarBL,
        IObtenerTareaBL obtenerBL,
        IObtenerTareaPorIdBL obtenerPorIdBL,
        IEditarTareaBL editarBL,
        ICambiarEstadoTareaBL cambiarEstadoBL,
        ICancelarTareaBL cancelarBL,
        IObtenerPersonalHousekeepingBL obtenerPersonalBL,
        IObtenerHabitacionesTareaBL obtenerHabitacionesBL)
    {
        _agregarBL = agregarBL;
        _obtenerBL = obtenerBL;
        _obtenerPorIdBL = obtenerPorIdBL;
        _editarBL = editarBL;
        _cambiarEstadoBL = cambiarEstadoBL;
        _cancelarBL = cancelarBL;
        _obtenerPersonalBL = obtenerPersonalBL;
        _obtenerHabitacionesBL = obtenerHabitacionesBL;
    }

    // HSK-003: consulta de tareas
    public async Task<IActionResult> Index(string? estado)
    {
        var estadoSeleccionado = estado ?? TodosLosEstados;
        var filtro = estadoSeleccionado == TodosLosEstados ? null : estadoSeleccionado;

        ViewBag.EstadoSeleccionado = estadoSeleccionado;
        ViewBag.OpcionesEstado = Estados.Tarea.Todos.Append(TodosLosEstados).ToArray();
        return View(await _obtenerBL.ObtenerAsync(filtro));
    }

    public async Task<IActionResult> Detalle(int id)
    {
        var tarea = await _obtenerPorIdBL.ObtenerPorIdAsync(id);
        if (tarea == null)
        {
            MensajeError(Mensajes.NoEncontrado);
            return RedirectToAction(nameof(Index));
        }

        return View(tarea);
    }

    // HSK-001 y HSK-002: registro
    public async Task<IActionResult> Crear()
    {
        await CargarListasAsync();
        return View(new TareaFormViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> Crear(TareaFormViewModel modelo)
    {
        if (!ModelState.IsValid)
        {
            await CargarListasAsync();
            return View(modelo);
        }

        var resultado = await _agregarBL.AgregarAsync(modelo.ADto());
        if (!resultado.Exito)
        {
            ModelState.AddModelError(string.Empty, resultado.Mensaje);
            await CargarListasAsync();
            return View(modelo);
        }

        MensajeExito(resultado.Mensaje);
        return RedirectToAction(nameof(Index));
    }

    // HSK-004: modificación
    public async Task<IActionResult> Editar(int id)
    {
        var tarea = await _obtenerPorIdBL.ObtenerPorIdAsync(id);
        if (tarea == null)
        {
            MensajeError(Mensajes.NoEncontrado);
            return RedirectToAction(nameof(Index));
        }

        if (!tarea.EstaActiva)
        {
            MensajeError(Mensajes.TareaNoModificable);
            return RedirectToAction(nameof(Detalle), new { id });
        }

        await CargarListasAsync();
        return View(TareaFormViewModel.DesdeDto(tarea));
    }

    [HttpPost]
    public async Task<IActionResult> Editar(int id, TareaFormViewModel modelo)
    {
        modelo.IdTarea = id;

        if (!ModelState.IsValid)
        {
            await CargarListasAsync();
            return View(modelo);
        }

        var resultado = await _editarBL.EditarAsync(modelo.ADto());
        if (!resultado.Exito)
        {
            ModelState.AddModelError(string.Empty, resultado.Mensaje);
            await CargarListasAsync();
            return View(modelo);
        }

        MensajeExito(resultado.Mensaje);
        return RedirectToAction(nameof(Detalle), new { id });
    }

    // HSK-005: cambio de estado
    [HttpPost]
    public async Task<IActionResult> CambiarEstado(int id, string nuevoEstado, string? volverA)
    {
        MostrarResultado(await _cambiarEstadoBL.CambiarEstadoAsync(id, nuevoEstado));

        return volverA == nameof(Detalle)
            ? RedirectToAction(nameof(Detalle), new { id })
            : RedirectToAction(nameof(Index));
    }

    // HSK-006: cancelación
    public async Task<IActionResult> Cancelar(int id)
    {
        var tarea = await _obtenerPorIdBL.ObtenerPorIdAsync(id);
        if (tarea == null)
        {
            MensajeError(Mensajes.NoEncontrado);
            return RedirectToAction(nameof(Index));
        }

        if (!tarea.EstaActiva)
        {
            MensajeError(Mensajes.TareaNoCancelable);
            return RedirectToAction(nameof(Detalle), new { id });
        }

        return View(tarea);
    }

    [HttpPost]
    public async Task<IActionResult> Cancelar(int id, string? motivo)
    {
        var resultado = await _cancelarBL.CancelarAsync(id, motivo);
        MostrarResultado(resultado);

        return resultado.Exito
            ? RedirectToAction(nameof(Detalle), new { id })
            : RedirectToAction(nameof(Cancelar), new { id });
    }

    private async Task CargarListasAsync()
    {
        var habitaciones = await _obtenerHabitacionesBL.ObtenerAsync();
        var personal = await _obtenerPersonalBL.ObtenerAsync();

        var opcionesHabitacion = habitaciones
            .Select(h => new SelectListItem
            {
                Value = h.IdHabitacion.ToString(),
                Text = h.NumeroHabitacion + " · " + h.TipoHabitacion + " (" + h.EstadoHabitacion + ")"
            })
            .ToList();

        ViewBag.Habitaciones = new SelectList(opcionesHabitacion, "Value", "Text");
        ViewBag.Personal = new SelectList(personal, "IdUsuario", "NombreCompleto");
        ViewBag.TiposTarea = Catalogos.TiposTareaHousekeeping;
    }
}