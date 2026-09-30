using Microsoft.AspNetCore.Mvc;
using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Inventarios;
using SistemaHotelColibri.ViewModels.Inventarios;

namespace SistemaHotelColibri.Controllers;

public class InventarioController : ControladorBase
{
    private const string TodosLosEstados = "Todos";

    private readonly IAgregarInventarioBL _agregarBL;
    private readonly IObtenerInventarioBL _obtenerBL;
    private readonly IObtenerInventarioPorIdBL _obtenerPorIdBL;
    private readonly IEditarInventarioBL _editarBL;
    private readonly IEliminarInventarioBL _eliminarBL;
    private readonly IObtenerAlertasInventarioBL _obtenerAlertasBL;

    public InventarioController(
        IAgregarInventarioBL agregarBL,
        IObtenerInventarioBL obtenerBL,
        IObtenerInventarioPorIdBL obtenerPorIdBL,
        IEditarInventarioBL editarBL,
        IEliminarInventarioBL eliminarBL,
        IObtenerAlertasInventarioBL obtenerAlertasBL)
    {
        _agregarBL = agregarBL;
        _obtenerBL = obtenerBL;
        _obtenerPorIdBL = obtenerPorIdBL;
        _editarBL = editarBL;
        _eliminarBL = eliminarBL;
        _obtenerAlertasBL = obtenerAlertasBL;
    }

    public async Task<IActionResult> Index(string? estado)
    {
        var estadoSeleccionado = estado ?? Estados.ProductoInventario.Activo;
        var filtro = estadoSeleccionado == TodosLosEstados ? null : estadoSeleccionado;

        ViewBag.EstadoSeleccionado = estadoSeleccionado;
        ViewBag.OpcionesEstado = Estados.ProductoInventario.Todos.Append(TodosLosEstados).ToArray();
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

    public IActionResult Crear()
    {
        ViewBag.UnidadesMedida = Catalogos.UnidadesMedida;
        return View(new InventarioFormViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> Crear(InventarioFormViewModel modelo)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.UnidadesMedida = Catalogos.UnidadesMedida;
            return View(modelo);
        }

        var resultado = await _agregarBL.AgregarAsync(modelo.ADto());
        if (!resultado.Exito)
        {
            ModelState.AddModelError(string.Empty, resultado.Mensaje);
            ViewBag.UnidadesMedida = Catalogos.UnidadesMedida;
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

        ViewBag.UnidadesMedida = Catalogos.UnidadesMedida;
        return View(InventarioFormViewModel.DesdeDto(producto));
    }

    [HttpPost]
    public async Task<IActionResult> Editar(int id, InventarioFormViewModel modelo)
    {
        modelo.IdProducto = id;

        if (!ModelState.IsValid)
        {
            ViewBag.UnidadesMedida = Catalogos.UnidadesMedida;
            return View(modelo);
        }

        var resultado = await _editarBL.EditarAsync(modelo.ADto());
        if (!resultado.Exito)
        {
            ModelState.AddModelError(string.Empty, resultado.Mensaje);
            ViewBag.UnidadesMedida = Catalogos.UnidadesMedida;
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
