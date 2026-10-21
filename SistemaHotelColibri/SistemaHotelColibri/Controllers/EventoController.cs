using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Clientes;
using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Eventos;
using SistemaHotelColibri.ViewModels.Comun;
using SistemaHotelColibri.ViewModels.Eventos;

namespace SistemaHotelColibri.Controllers;

[Authorize(Roles = Roles.AccesoHospedaje)]
public class EventoController : ControladorBase
{
    private readonly IAgregarEventoBL _agregarBL;
    private readonly IObtenerEventoBL _obtenerBL;
    private readonly IObtenerEventoPorIdBL _obtenerPorIdBL;
    private readonly IEditarEventoBL _editarBL;
    private readonly IEliminarEventoBL _eliminarBL;
    private readonly IObtenerClienteBL _obtenerClientesBL;
    private readonly IObtenerEspacioEventoBL _obtenerEspaciosBL;

    public EventoController(
        IAgregarEventoBL agregarBL,
        IObtenerEventoBL obtenerBL,
        IObtenerEventoPorIdBL obtenerPorIdBL,
        IEditarEventoBL editarBL,
        IEliminarEventoBL eliminarBL,
        IObtenerClienteBL obtenerClientesBL,
        IObtenerEspacioEventoBL obtenerEspaciosBL)
    {
        _agregarBL = agregarBL;
        _obtenerBL = obtenerBL;
        _obtenerPorIdBL = obtenerPorIdBL;
        _editarBL = editarBL;
        _eliminarBL = eliminarBL;
        _obtenerClientesBL = obtenerClientesBL;
        _obtenerEspaciosBL = obtenerEspaciosBL;
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
            Placeholder = "Evento, cliente, identificación o espacio",
            Filtros =
            [
                FiltroSeleccion.Crear("estado", "Estado",
                    Estados.Evento.Todos.Append(FiltroListado.TodosLosEstados),
                    estadoSeleccionado)
            ]
        };
        return View(await _obtenerBL.ObtenerAsync(filtro));
    }

    public async Task<IActionResult> Detalle(int id)
    {
        var evento = await _obtenerPorIdBL.ObtenerPorIdAsync(id);
        if (evento == null)
        {
            MensajeError(Mensajes.NoEncontrado);
            return RedirectToAction(nameof(Index));
        }

        return View(evento);
    }

    public async Task<IActionResult> Crear()
    {
        await CargarCatalogosAsync();
        return View(new EventoFormViewModel { FechaEvento = DateOnly.FromDateTime(DateTime.Today) });
    }

    [HttpPost]
    public async Task<IActionResult> Crear(EventoFormViewModel modelo)
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

        await CargarCatalogosAsync();
        return View(modelo);
    }

    public async Task<IActionResult> Editar(int id)
    {
        var evento = await _obtenerPorIdBL.ObtenerPorIdAsync(id);
        if (evento == null)
        {
            MensajeError(Mensajes.NoEncontrado);
            return RedirectToAction(nameof(Index));
        }

        await CargarCatalogosAsync();
        return View(EventoFormViewModel.DesdeDto(evento));
    }

    [HttpPost]
    public async Task<IActionResult> Editar(int id, EventoFormViewModel modelo)
    {
        modelo.IdEvento = id;

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

        await CargarCatalogosAsync();
        return View(modelo);
    }

    [HttpPost]
    public async Task<IActionResult> Eliminar(int id)
    {
        MostrarResultado(await _eliminarBL.EliminarAsync(id));
        return RedirectToAction(nameof(Index));
    }

    private async Task CargarCatalogosAsync()
    {
        ViewBag.Clientes = await _obtenerClientesBL.ObtenerAsync(null);
        ViewBag.Espacios = await _obtenerEspaciosBL.ObtenerAsync(null);
        ViewBag.EstadosEvento = Estados.Evento.Todos;
    }
}
