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

    public InventarioController(
        IAgregarInventarioBL agregarBL,
        IObtenerInventarioBL obtenerBL,
        IObtenerInventarioPorIdBL obtenerPorIdBL)
    {
        _agregarBL = agregarBL;
        _obtenerBL = obtenerBL;
        _obtenerPorIdBL = obtenerPorIdBL;
    }

    public async Task<IActionResult> Index(string? estado)
    {
        var estadoSeleccionado = estado ?? Estados.ProductoInventario.Activo;
        var filtro = estadoSeleccionado == TodosLosEstados ? null : estadoSeleccionado;

        ViewBag.EstadoSeleccionado = estadoSeleccionado;
        ViewBag.OpcionesEstado = Estados.ProductoInventario.Todos.Append(TodosLosEstados).ToArray();
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
}
