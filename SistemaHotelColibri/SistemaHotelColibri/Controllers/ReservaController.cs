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
    private readonly IClienteReservaBL _clienteReservaBL;

    public ReservaController(
        IAgregarReservaBL agregarBL,
        IObtenerReservaBL obtenerBL,
        IObtenerReservaPorIdBL obtenerPorIdBL,
        IEditarReservaBL editarBL,
        IEliminarReservaBL eliminarBL,
        IObtenerOpcionesReservaBL opcionesBL,
        IClienteReservaBL clienteReservaBL)
    {
        _clienteReservaBL = clienteReservaBL;
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
        async Task<int> Contar(string estadoContar) =>
            (await _obtenerBL.ObtenerAsync(new FiltroListado { Estado = estadoContar, TamanoPagina = 1 })).TotalRegistros;

        ViewBag.Resumen = new List<TarjetaResumenViewModel>
        {
            TarjetaEstado("Pendientes", await Contar(Estados.Reserva.Pendiente), "bi-hourglass-split", "ambar", Estados.Reserva.Pendiente, estadoSeleccionado, "por confirmar"),
            TarjetaEstado("Confirmadas", await Contar(Estados.Reserva.Confirmada), "bi-calendar-check", "cielo", Estados.Reserva.Confirmada, estadoSeleccionado, "listas para llegar"),
            TarjetaEstado("En check-in", await Contar(Estados.Reserva.CheckIn), "bi-box-arrow-in-right", "verde", Estados.Reserva.CheckIn, estadoSeleccionado, "huéspedes en el hotel"),
            TarjetaEstado("Canceladas", await Contar(Estados.Reserva.Cancelada), "bi-x-circle", "coral", Estados.Reserva.Cancelada, estadoSeleccionado, "reservas")
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
            var clienteListo = true;

            // Cliente escrito en la reserva: se deja listo para poder reservar.
            if (modelo.ClienteNuevo)
            {
                var cliente = await _clienteReservaBL.PrepararAsync(modelo.ClienteADto());
                if (cliente.Exito)
                {
                    modelo.IdCliente = cliente.IdGenerado;
                }
                else
                {
                    clienteListo = false;
                    ModelState.AddModelError(string.Empty, cliente.Mensaje);
                }
            }

            if (clienteListo)
            {
                var resultado = await _agregarBL.AgregarAsync(modelo.ADto());

                // Si dijeron que no se guarde, el cliente se oculta de Clientes (salga bien o no la reserva).
                if (modelo.ClienteNuevo)
                {
                    await _clienteReservaBL.FinalizarAsync(modelo.IdCliente ?? 0, modelo.GuardarCliente);
                }

                if (resultado.Exito)
                {
                    var mensajeCliente = !modelo.ClienteNuevo ? string.Empty
                        : modelo.GuardarCliente ? " El cliente quedó guardado en Clientes."
                        : " El cliente no se guardó en Clientes.";
                    MensajeExito(resultado.Mensaje + mensajeCliente);
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError(string.Empty, resultado.Mensaje);
            }
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
