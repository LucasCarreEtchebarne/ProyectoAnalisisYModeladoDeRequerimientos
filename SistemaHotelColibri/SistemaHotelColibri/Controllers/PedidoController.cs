using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Clientes;
using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Menus;
using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Mesas;
using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Pedidos;
using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Reservas;
using SistemaHotelColibri.ViewModels.Comun;
using SistemaHotelColibri.ViewModels.Pedidos;

namespace SistemaHotelColibri.Controllers;

[Authorize(Roles = Roles.AccesoMenu)]
public class PedidoController : ControladorBase
{
    private readonly IAgregarPedidoBL _agregarPedidoBL;
    private readonly IEditarPedidoBL _editarPedidoBL;
    private readonly IObtenerPedidoBL _obtenerPedidoBL;
    private readonly IObtenerPedidoPorIdBL _obtenerPedidoPorIdBL;
    private readonly IObtenerMesaBL _obtenerMesaBL;
    private readonly IObtenerReservaBL _obtenerReservaBL;
    private readonly IObtenerClienteBL _obtenerClienteBL;
    private readonly IObtenerMenuBL _obtenerMenuBL;

    public PedidoController(
        IAgregarPedidoBL agregarPedidoBL,
        IEditarPedidoBL editarPedidoBL,
        IObtenerPedidoBL obtenerPedidoBL,
        IObtenerPedidoPorIdBL obtenerPedidoPorIdBL,
        IObtenerMesaBL obtenerMesaBL,
        IObtenerReservaBL obtenerReservaBL,
        IObtenerClienteBL obtenerClienteBL,
        IObtenerMenuBL obtenerMenuBL)
    {
        _agregarPedidoBL = agregarPedidoBL;
        _editarPedidoBL = editarPedidoBL;
        _obtenerPedidoBL = obtenerPedidoBL;
        _obtenerPedidoPorIdBL = obtenerPedidoPorIdBL;
        _obtenerMesaBL = obtenerMesaBL;
        _obtenerReservaBL = obtenerReservaBL;
        _obtenerClienteBL = obtenerClienteBL;
        _obtenerMenuBL = obtenerMenuBL;
    }

    public async Task<IActionResult> Index(
        string? busqueda, string? estado, int pagina = 1)
    {
        var estadoSeleccionado = estado ?? FiltroListado.TodosLosEstados;

        ViewBag.Busqueda = new BarraBusquedaViewModel
        {
            Busqueda = busqueda,
            Placeholder = "Número de pedido, tipo o cliente",
            Filtros =
            [
                FiltroSeleccion.Crear(
                    "estado",
                    "Estado",
                    Estados.Pedido.Todos.Prepend(FiltroListado.TodosLosEstados),
                    estadoSeleccionado)
            ]
        };

        var pedidos = await _obtenerPedidoBL.ObtenerAsync(new FiltroListado
        {
            Busqueda = busqueda,
            Estado = estadoSeleccionado,
            Pagina = pagina
        });

        return View(pedidos);
    }

    public async Task<IActionResult> Detalle(int id)
    {
        var pedido = await _obtenerPedidoPorIdBL.ObtenerPorIdAsync(id);

        if (pedido == null)
        {
            MensajeError("El pedido solicitado no existe.");
            return RedirectToAction(nameof(Index));
        }

        return View(pedido);
    }

    [HttpGet]
    public async Task<IActionResult> Crear()
    {
        var modelo = new PedidoFormViewModel
        {
            Detalles = [new LineaPedidoFormViewModel()]
        };

        await CargarOpcionesAsync(modelo);
        return View(modelo);
    }

    [HttpPost]
    public async Task<IActionResult> Crear(PedidoFormViewModel modelo)
    {
        if (ModelState.IsValid)
        {
            var resultado = await _agregarPedidoBL.AgregarAsync(modelo.ADto(IdUsuarioActual));

            if (resultado.Exito)
            {
                MensajeExito(resultado.Mensaje);
                return RedirectToAction(nameof(Detalle), new { id = resultado.IdGenerado });
            }

            ModelState.AddModelError(string.Empty, resultado.Mensaje);
        }

        if (modelo.Detalles.Count == 0)
        {
            modelo.Detalles.Add(new LineaPedidoFormViewModel());
        }

        await CargarOpcionesAsync(modelo);
        return View(modelo);
    }

    [HttpGet]
    public async Task<IActionResult> Editar(int id)
    {
        var pedido = await _obtenerPedidoPorIdBL.ObtenerPorIdAsync(id);

        if (pedido == null)
        {
            MensajeError(Mensajes.NoEncontrado);
            return RedirectToAction(nameof(Index));
        }

        if (pedido.EstadoPedido != Estados.Pedido.Pendiente)
        {
            MensajeError("Solo se pueden modificar pedidos en estado Pendiente.");
            return RedirectToAction(nameof(Detalle), new { id });
        }

        var modelo = PedidoFormViewModel.DesdeDto(pedido);
        await CargarOpcionesAsync(modelo);
        return View("Crear", modelo);
    }

    [HttpPost]
    public async Task<IActionResult> Editar(int id, PedidoFormViewModel modelo)
    {
        modelo.IdPedido = id;

        if (ModelState.IsValid)
        {
            var resultado = await _editarPedidoBL.EditarAsync(modelo.ADto(IdUsuarioActual));

            if (resultado.Exito)
            {
                MensajeExito(resultado.Mensaje);
                return RedirectToAction(nameof(Detalle), new { id });
            }

            ModelState.AddModelError(string.Empty, resultado.Mensaje);
        }

        if (modelo.Detalles.Count == 0)
        {
            modelo.Detalles.Add(new LineaPedidoFormViewModel());
        }

        await CargarOpcionesAsync(modelo);
        return View("Crear", modelo);
    }

    [HttpPost]
    public async Task<IActionResult> Inactivar(int id)
    {
        MostrarResultado(await _editarPedidoBL.InactivarAsync(id));
        return RedirectToAction(nameof(Detalle), new { id });
    }

    private async Task CargarOpcionesAsync(PedidoFormViewModel modelo)
    {
        var mesas = await CargarTodasAsync(
            _obtenerMesaBL.ObtenerAsync, Estados.Mesa.Activas);

        var reservas = await CargarTodasAsync(
            _obtenerReservaBL.ObtenerAsync, Estados.Reserva.CheckIn);

        var clientes = await _obtenerClienteBL.ObtenerAsync(Estados.Cliente.Activo);

        var productos = await CargarTodasAsync(
            _obtenerMenuBL.ObtenerAsync, Estados.ProductoMenu.Disponible);

        modelo.Mesas = mesas.Select(mesa => new SelectListItem(
            $"Mesa {mesa.NumeroMesa}", mesa.IdMesa.ToString())).ToList();

        modelo.Reservas = reservas.Select(reserva => new SelectListItem(
            $"Habitación {reserva.NumeroHabitacion} · {reserva.NombreCliente} · Reserva #{reserva.IdReserva}",
            reserva.IdReserva.ToString())).ToList();

        modelo.Clientes = clientes.Select(cliente => new SelectListItem(
            cliente.NombreMostrar, cliente.IdCliente.ToString())).ToList();

        modelo.Productos = productos.Select(producto => new SelectListItem(
            $"{producto.NombreProducto} · {Formato.Colones(producto.Precio)}",
            producto.IdProductoMenu.ToString())).ToList();
    }

    private static async Task<List<T>> CargarTodasAsync<T>(
        Func<FiltroListado, Task<ResultadoPaginado<T>>> obtenerAsync,
        string estado)
    {
        var elementos = new List<T>();
        var filtro = new FiltroListado
        {
            Estado = estado,
            TamanoPagina = FiltroListado.TamanoPaginaMaximo
        };

        while (true)
        {
            var resultado = await obtenerAsync(filtro);
            elementos.AddRange(resultado.Elementos);

            if (!resultado.TienePaginaSiguiente)
            {
                return elementos;
            }

            filtro.Pagina = resultado.PaginaActual + 1;
        }
    }
}