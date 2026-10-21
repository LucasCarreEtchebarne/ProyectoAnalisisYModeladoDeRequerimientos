using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Reservas;
using SistemaHotelColibri.ViewModels.Comun;
using SistemaHotelColibri.ViewModels.Reservas;

namespace SistemaHotelColibri.Controllers;

[Authorize(Roles = Roles.AccesoHospedaje)]
public class ReservaController : ControladorBase
{
    private readonly IAgregarReservaBL _agregarBL;
    private readonly IObtenerReservaBL _obtenerBL;
    private readonly IObtenerReservaPorIdBL _obtenerPorIdBL;
    private readonly IEditarReservaBL _editarBL;
    private readonly IEliminarReservaBL _eliminarBL;
    private readonly IObtenerOpcionesReservaBL _opcionesBL;

    public ReservaController(
        IAgregarReservaBL agregarBL,
        IObtenerReservaBL obtenerBL,
        IObtenerReservaPorIdBL obtenerPorIdBL,
        IEditarReservaBL editarBL,
        IEliminarReservaBL eliminarBL,
        IObtenerOpcionesReservaBL opcionesBL)
    {
        _agregarBL = agregarBL;
        _obtenerBL = obtenerBL;
        _obtenerPorIdBL = obtenerPorIdBL;
        _editarBL = editarBL;
        _eliminarBL = eliminarBL;
        _opcionesBL = opcionesBL;
    }

    public async Task<IActionResult> Index(string? busqueda, string? estado, int pagina = 1)
    {
        var estadoSeleccionado = estado ?? FiltroListado.TodosLosEstados;

        var filtro = new FiltroListado
        {
            Busqueda = busqueda,
            Estado = estadoSeleccionado == FiltroListado.TodosLosEstados ? null : estadoSeleccionado,
            Pagina = pagina
        };

        ViewBag.Busqueda = new BarraBusquedaViewModel
        {
            Busqueda = busqueda,
            Placeholder = "Cliente, identificación, habitación o código",
            Filtros =
            [
                FiltroSeleccion.Crear("estado", "Estado",
                    Estados.Reserva.Todos.Append(FiltroListado.TodosLosEstados),
                    estadoSeleccionado)
            ]
        };
        return View(await _obtenerBL.ObtenerAsync(filtro));
    }

    public async Task<IActionResult> Detalle(int id)
    {
        var reserva = await _obtenerPorIdBL.ObtenerPorIdAsync(id);
        if (reserva == null)
        {
            MensajeError(Mensajes.NoEncontrado);
            return RedirectToAction(nameof(Index));
        }

        return View(reserva);
    }

    public async Task<IActionResult> Crear()
    {
        await CargarListasAsync(0, 0);
        return View(new ReservaFormViewModel
        {
            FechaEntrada = DateOnly.FromDateTime(DateTime.Today),
            FechaSalida = DateOnly.FromDateTime(DateTime.Today).AddDays(1)
        });
    }

    [HttpPost]
    public async Task<IActionResult> Crear(ReservaFormViewModel modelo)
    {
        if (ModelState.IsValid)
        {
            var resultado = await _agregarBL.AgregarAsync(modelo.ADto());
            if (resultado.Exito)
            {
                MensajeExito(resultado.Mensaje);
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError(string.Empty, resultado.Mensaje);
        }

        await CargarListasAsync(modelo.IdCliente ?? 0, modelo.IdHabitacion ?? 0);
        return View(modelo);
    }

    public async Task<IActionResult> Editar(int id)
    {
        var reserva = await _obtenerPorIdBL.ObtenerPorIdAsync(id);
        if (reserva == null)
        {
            MensajeError(Mensajes.NoEncontrado);
            return RedirectToAction(nameof(Index));
        }

        await CargarListasAsync(reserva.IdCliente, reserva.IdHabitacion);
        return View(ReservaFormViewModel.DesdeDto(reserva));
    }

    [HttpPost]
    public async Task<IActionResult> Editar(int id, ReservaFormViewModel modelo)
    {
        modelo.IdReserva = id;

        if (ModelState.IsValid)
        {
            var resultado = await _editarBL.EditarAsync(modelo.ADto());
            if (resultado.Exito)
            {
                MensajeExito(resultado.Mensaje);
                return RedirectToAction(nameof(Detalle), new { id });
            }

            ModelState.AddModelError(string.Empty, resultado.Mensaje);
        }

        await CargarListasAsync(modelo.IdCliente ?? 0, modelo.IdHabitacion ?? 0);
        return View(modelo);
    }

    [HttpPost]
    public async Task<IActionResult> Eliminar(int id)
    {
        MostrarResultado(await _eliminarBL.EliminarAsync(id));
        return RedirectToAction(nameof(Index));
    }

    private async Task CargarListasAsync(int idCliente, int idHabitacion)
    {
        ViewBag.Opciones = await _opcionesBL.ObtenerAsync(idCliente, idHabitacion);
        ViewBag.EstadosReserva = Estados.Reserva.Todos;
    }
}
