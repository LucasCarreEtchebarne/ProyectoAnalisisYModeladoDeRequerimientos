using Microsoft.AspNetCore.Mvc;
using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Clientes;
using SistemaHotelColibri.ViewModels.Clientes;

namespace SistemaHotelColibri.Controllers;

public class ClienteController : ControladorBase
{
    private const string TodosLosEstados = "Todos";

    private readonly IAgregarClienteBL _agregarBL;
    private readonly IObtenerClienteBL _obtenerBL;
    private readonly IObtenerClientePorIdBL _obtenerPorIdBL;
    private readonly IEditarClienteBL _editarBL;
    private readonly IEliminarClienteBL _eliminarBL;
    private readonly IBuscarClienteBL _buscarBL;

    public ClienteController(
        IAgregarClienteBL agregarBL,
        IObtenerClienteBL obtenerBL,
        IObtenerClientePorIdBL obtenerPorIdBL,
        IEditarClienteBL editarBL,
        IEliminarClienteBL eliminarBL,
        IBuscarClienteBL buscarBL)
    {
        _agregarBL = agregarBL;
        _obtenerBL = obtenerBL;
        _obtenerPorIdBL = obtenerPorIdBL;
        _editarBL = editarBL;
        _eliminarBL = eliminarBL;
        _buscarBL = buscarBL;
    }

    public async Task<IActionResult> Index(string? estado, string? busqueda)
    {
        var estadoSeleccionado = estado ?? Estados.Cliente.Activo;
        var filtro = estadoSeleccionado == TodosLosEstados ? null : estadoSeleccionado;

        ViewBag.EstadoSeleccionado = estadoSeleccionado;
        ViewBag.OpcionesEstado = Estados.Cliente.Todos.Append(TodosLosEstados).ToArray();
        ViewBag.Busqueda = busqueda?.Trim();

        var clientes = string.IsNullOrWhiteSpace(busqueda)
            ? await _obtenerBL.ObtenerAsync(filtro)
            : await _buscarBL.BuscarAsync(busqueda, filtro);

        return View(clientes);
    }

    public async Task<IActionResult> Detalle(int id)
    {
        var cliente = await _obtenerPorIdBL.ObtenerPorIdAsync(id);
        if (cliente == null)
        {
            MensajeError(Mensajes.NoEncontrado);
            return RedirectToAction(nameof(Index));
        }

        return View(cliente);
    }

    public IActionResult Crear()
    {
        return View(new ClienteFormViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> Crear(ClienteFormViewModel modelo)
    {
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
        var cliente = await _obtenerPorIdBL.ObtenerPorIdAsync(id);
        if (cliente == null)
        {
            MensajeError(Mensajes.NoEncontrado);
            return RedirectToAction(nameof(Index));
        }

        ViewBag.EstadosCliente = Estados.Cliente.Todos;
        return View(ClienteFormViewModel.DesdeDto(cliente));
    }

    [HttpPost]
    public async Task<IActionResult> Editar(int id, ClienteFormViewModel modelo)
    {
        modelo.IdCliente = id;

        if (!ModelState.IsValid)
        {
            ViewBag.EstadosCliente = Estados.Cliente.Todos;
            return View(modelo);
        }

        var resultado = await _editarBL.EditarAsync(modelo.ADto());
        if (!resultado.Exito)
        {
            ModelState.AddModelError(string.Empty, resultado.Mensaje);
            ViewBag.EstadosCliente = Estados.Cliente.Todos;
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