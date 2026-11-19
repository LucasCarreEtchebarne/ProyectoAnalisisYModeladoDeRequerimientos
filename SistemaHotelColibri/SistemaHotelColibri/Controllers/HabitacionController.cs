using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Habitaciones;
using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Reservas;
using SistemaHotelColibri.Abstract.Modelos.Habitaciones;
using SistemaHotelColibri.Abstract.Modelos.Reservas;
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
    private readonly IObtenerOcupacionHabitacionBL _ocupacionBL;

    public HabitacionController(
        IAgregarHabitacionBL agregarBL,
        IObtenerHabitacionBL obtenerBL,
        IObtenerHabitacionPorIdBL obtenerPorIdBL,
        IEditarHabitacionBL editarBL,
        IEliminarHabitacionBL eliminarBL,
        IObtenerOcupacionHabitacionBL ocupacionBL)
    {
        _ocupacionBL = ocupacionBL;
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
        async Task<int> Contar(string estadoContar) =>
            (await _obtenerBL.ObtenerAsync(new FiltroListado { Estado = estadoContar, TamanoPagina = 1 })).TotalRegistros;

        // Las reservas de hoy (pendientes o confirmadas) cuentan como "Reservadas" aunque en la base sigan Disponibles.
        var ocupacion = await _ocupacionBL.ObtenerAsync();
        var disponiblesEnBase = (await CargarTodasAsync(Estados.Habitacion.Disponible)).Select(h => h.IdHabitacion).ToHashSet();
        var reservadasHoy = ocupacion.Values.Count(o => o.Estado == OcupacionHabitacionDto.Reservada && disponiblesEnBase.Contains(o.IdHabitacion));
        ViewBag.Ocupacion = ocupacion;

        var detalle = $"de {await Contar(Estados.Habitacion.Activas)} habitaciones";
        ViewBag.Resumen = new List<TarjetaResumenViewModel>
        {
            TarjetaEstado("Disponibles", disponiblesEnBase.Count - reservadasHoy, "bi-door-open", "verde", Estados.Habitacion.Disponible, estadoSeleccionado, detalle),
            TarjetaResumenViewModel.Crear("Reservadas hoy", reservadasHoy, "bi-calendar-check", "violeta", Url.Action("Index", "Reserva"), false, "huéspedes por llegar"),
            TarjetaEstado("Ocupadas", await Contar(Estados.Habitacion.Ocupada), "bi-person-fill", "coral", Estados.Habitacion.Ocupada, estadoSeleccionado, "con huésped en check-in"),
            TarjetaEstado("En limpieza", await Contar(Estados.Habitacion.Limpieza), "bi-stars", "cielo", Estados.Habitacion.Limpieza, estadoSeleccionado, "habitaciones"),
            TarjetaEstado("Mantenimiento", await Contar(Estados.Habitacion.Mantenimiento), "bi-tools", "ambar", Estados.Habitacion.Mantenimiento, estadoSeleccionado, "habitaciones")
        };

        return View(await _obtenerBL.ObtenerAsync(filtro));
    }

    private async Task<List<HabitacionDto>> CargarTodasAsync(string estado)
    {
        var elementos = new List<HabitacionDto>();
        var filtro = new FiltroListado { Estado = estado, TamanoPagina = FiltroListado.TamanoPaginaMaximo };
        while (true)
        {
            var resultado = await _obtenerBL.ObtenerAsync(filtro);
            elementos.AddRange(resultado.Elementos);
            if (!resultado.TienePaginaSiguiente)
            {
                return elementos;
            }

            filtro.Pagina = resultado.PaginaActual + 1;
        }
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
