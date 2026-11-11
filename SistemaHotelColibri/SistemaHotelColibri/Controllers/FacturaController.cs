using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Facturas;
using SistemaHotelColibri.Abstract.Modelos.Facturas;
using SistemaHotelColibri.ViewModels.Comun;
using SistemaHotelColibri.ViewModels.Facturas;

namespace SistemaHotelColibri.Controllers;

[Authorize(Roles = Roles.AccesoFacturacion)]
public class FacturaController : ControladorBase
{
    private const string TodosLosTipos = "Todos";

    private readonly IObtenerFacturaBL _obtenerBL;
    private readonly IObtenerFacturaPorIdBL _obtenerPorIdBL;
    private readonly IObtenerReservaFacturableBL _reservasFacturablesBL;
    private readonly IObtenerPedidoFacturableBL _pedidosFacturablesBL;
    private readonly IRegistrarFacturaHospedajeBL _facturaHospedajeBL;
    private readonly IRegistrarFacturaRestauranteBL _facturaRestauranteBL;
    private readonly IRegistrarFacturaConsolidadaBL _facturaConsolidadaBL;
    private readonly IAnularFacturaBL _anularBL;
    private readonly IRegistrarPagoBL _registrarPagoBL;

    public FacturaController(
        IObtenerFacturaBL obtenerBL,
        IObtenerFacturaPorIdBL obtenerPorIdBL,
        IObtenerReservaFacturableBL reservasFacturablesBL,
        IObtenerPedidoFacturableBL pedidosFacturablesBL,
        IRegistrarFacturaHospedajeBL facturaHospedajeBL,
        IRegistrarFacturaRestauranteBL facturaRestauranteBL,
        IRegistrarFacturaConsolidadaBL facturaConsolidadaBL,
        IAnularFacturaBL anularBL,
        IRegistrarPagoBL registrarPagoBL)
    {
        _obtenerBL = obtenerBL;
        _obtenerPorIdBL = obtenerPorIdBL;
        _reservasFacturablesBL = reservasFacturablesBL;
        _pedidosFacturablesBL = pedidosFacturablesBL;
        _facturaHospedajeBL = facturaHospedajeBL;
        _facturaRestauranteBL = facturaRestauranteBL;
        _facturaConsolidadaBL = facturaConsolidadaBL;
        _anularBL = anularBL;
        _registrarPagoBL = registrarPagoBL;
    }

    public async Task<IActionResult> Index(string? busqueda, string? estado, string? tipo, DateOnly? desde, DateOnly? hasta, int pagina = 1)
    {
        var estadoSeleccionado = estado ?? FiltroListado.TodosLosEstados;
        var tipoSeleccionado = tipo ?? TodosLosTipos;

        var filtro = new FiltroFactura
        {
            Busqueda = busqueda,
            Estado = estadoSeleccionado == FiltroListado.TodosLosEstados ? null : estadoSeleccionado,
            Tipo = tipoSeleccionado == TodosLosTipos ? null : tipoSeleccionado,
            Desde = desde,
            Hasta = hasta,
            Pagina = pagina
        };

        ViewBag.Busqueda = new BarraBusquedaViewModel
        {
            Busqueda = busqueda,
            Placeholder = "Número de factura, cliente o identificación",
            Filtros =
            [
                FiltroSeleccion.Crear("estado", "Estado", Estados.Factura.Todos.Prepend(FiltroListado.TodosLosEstados), estadoSeleccionado),
                FiltroSeleccion.Crear("tipo", "Tipo", Facturacion.Tipo.Todos.Prepend(TodosLosTipos), tipoSeleccionado)
            ],
            FiltrosFecha =
            [
                FiltroFecha.Crear("desde", "Desde", desde),
                FiltroFecha.Crear("hasta", "Hasta", hasta)
            ]
        };

        return View(await _obtenerBL.ObtenerAsync(filtro));
    }

    public async Task<IActionResult> Detalle(int id)
    {
        var factura = await _obtenerPorIdBL.ObtenerPorIdAsync(id);
        if (factura == null)
        {
            MensajeError(Mensajes.NoEncontrado);
            return RedirectToAction(nameof(Index));
        }

        return View(factura);
    }

    public IActionResult Nueva()
    {
        return View();
    }

    public async Task<IActionResult> Hospedaje(string? busqueda)
    {
        ViewBag.Busqueda = busqueda;
        return View(await _reservasFacturablesBL.ListarAsync(busqueda));
    }

    [HttpPost]
    public async Task<IActionResult> Hospedaje(int idReserva)
    {
        var resultado = await _facturaHospedajeBL.RegistrarAsync(idReserva, IdUsuarioActual);
        return ResultadoFactura(resultado, nameof(Hospedaje));
    }

    public async Task<IActionResult> Restaurante(string? busqueda)
    {
        ViewBag.Busqueda = busqueda;
        return View(await _pedidosFacturablesBL.ListarAsync(busqueda));
    }

    [HttpPost]
    public async Task<IActionResult> Restaurante(int idPedido)
    {
        var resultado = await _facturaRestauranteBL.RegistrarAsync(idPedido, IdUsuarioActual);
        return ResultadoFactura(resultado, nameof(Restaurante));
    }

    public async Task<IActionResult> Consolidada(string? busqueda, int? idReserva)
    {
        if (idReserva == null)
        {
            ViewBag.Busqueda = busqueda;
            return View("SeleccionarReserva", await _reservasFacturablesBL.ListarAsync(busqueda));
        }

        var reserva = await _reservasFacturablesBL.ObtenerAsync(idReserva.Value);
        if (reserva == null || reserva.YaFacturada || !Facturacion.EstadosReservaFacturables.Contains(reserva.EstadoReserva))
        {
            MensajeError(reserva == null ? Mensajes.NoEncontrado : reserva.YaFacturada ? Mensajes.ReservaYaFacturada : Mensajes.ReservaNoFacturable);
            return RedirectToAction(nameof(Consolidada));
        }

        return View(new ConsolidadaViewModel
        {
            Reserva = reserva,
            Pedidos = await _pedidosFacturablesBL.ListarParaReservaAsync(reserva.IdReserva)
        });
    }

    [HttpPost]
    public async Task<IActionResult> Consolidada(int idReserva, int[] idPedidos)
    {
        var resultado = await _facturaConsolidadaBL.RegistrarAsync(idReserva, idPedidos, IdUsuarioActual);
        if (!resultado.Exito)
        {
            MensajeError(resultado.Mensaje);
            return RedirectToAction(nameof(Consolidada), new { idReserva });
        }

        MensajeExito(resultado.Mensaje);
        return RedirectToAction(nameof(Detalle), new { id = resultado.IdGenerado });
    }

    public async Task<IActionResult> RegistrarPago(int id)
    {
        var factura = await _obtenerPorIdBL.ObtenerPorIdAsync(id);
        if (factura == null || factura.EstadoFactura != Estados.Factura.Pendiente)
        {
            MensajeError(factura == null ? Mensajes.NoEncontrado : Mensajes.FacturaNoAdmitePagos);
            return factura == null ? RedirectToAction(nameof(Index)) : RedirectToAction(nameof(Detalle), new { id });
        }

        ViewBag.Factura = factura;
        return View(new PagoFormViewModel { MontoPagado = factura.Saldo });
    }

    [HttpPost]
    public async Task<IActionResult> RegistrarPago(int id, PagoFormViewModel modelo)
    {
        if (ModelState.IsValid)
        {
            var resultado = await _registrarPagoBL.RegistrarAsync(modelo.ADto(id), IdUsuarioActual);
            if (resultado.Exito)
            {
                MensajeExito(resultado.Mensaje);
                return RedirectToAction(nameof(Detalle), new { id });
            }

            ModelState.AddModelError(string.Empty, resultado.Mensaje);
        }

        var factura = await _obtenerPorIdBL.ObtenerPorIdAsync(id);
        if (factura == null)
        {
            MensajeError(Mensajes.NoEncontrado);
            return RedirectToAction(nameof(Index));
        }

        ViewBag.Factura = factura;
        return View(modelo);
    }

    public async Task<IActionResult> Anular(int id)
    {
        var factura = await _obtenerPorIdBL.ObtenerPorIdAsync(id);
        if (factura == null || factura.EstadoFactura == Estados.Factura.Anulada)
        {
            MensajeError(factura == null ? Mensajes.NoEncontrado : Mensajes.FacturaYaAnulada);
            return factura == null ? RedirectToAction(nameof(Index)) : RedirectToAction(nameof(Detalle), new { id });
        }

        ViewBag.Factura = factura;
        return View(new AnularFacturaViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> Anular(int id, AnularFacturaViewModel modelo)
    {
        if (ModelState.IsValid)
        {
            var resultado = await _anularBL.AnularAsync(id, modelo.Motivo);
            if (resultado.Exito)
            {
                MensajeExito(resultado.Mensaje);
                return RedirectToAction(nameof(Detalle), new { id });
            }

            ModelState.AddModelError(string.Empty, resultado.Mensaje);
        }

        var factura = await _obtenerPorIdBL.ObtenerPorIdAsync(id);
        if (factura == null)
        {
            MensajeError(Mensajes.NoEncontrado);
            return RedirectToAction(nameof(Index));
        }

        ViewBag.Factura = factura;
        return View(modelo);
    }

    private IActionResult ResultadoFactura(ResultadoOperacion resultado, string accionError)
    {
        if (!resultado.Exito)
        {
            MensajeError(resultado.Mensaje);
            return RedirectToAction(accionError);
        }

        MensajeExito(resultado.Mensaje);
        return RedirectToAction(nameof(Detalle), new { id = resultado.IdGenerado });
    }
}
