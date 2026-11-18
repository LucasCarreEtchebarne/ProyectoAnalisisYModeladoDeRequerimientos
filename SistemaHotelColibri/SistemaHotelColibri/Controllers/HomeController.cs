using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Habitaciones;
using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Inventarios;
using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Mesas;
using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Pedidos;
using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Reservas;
using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.TareasHousekeeping;
using SistemaHotelColibri.Abstract.Modelos.Reservas;
using SistemaHotelColibri.Models;
using SistemaHotelColibri.ViewModels.Comun;
using SistemaHotelColibri.ViewModels.Inicio;

namespace SistemaHotelColibri.Controllers;

public class HomeController : ControladorBase
{
    private const int MaximoPendientes = 4;

    private readonly IObtenerHabitacionBL _habitacionBL;
    private readonly IObtenerReservaBL _reservaBL;
    private readonly IObtenerTareaBL _tareaBL;
    private readonly IObtenerAlertasInventarioBL _alertasBL;
    private readonly IObtenerMesaBL _mesaBL;
    private readonly IObtenerPedidoBL _pedidoBL;
    private readonly IObtenerOcupacionHabitacionBL _ocupacionBL;

    public HomeController(
        IObtenerOcupacionHabitacionBL ocupacionBL,
        IObtenerHabitacionBL habitacionBL,
        IObtenerReservaBL reservaBL,
        IObtenerTareaBL tareaBL,
        IObtenerAlertasInventarioBL alertasBL,
        IObtenerMesaBL mesaBL,
        IObtenerPedidoBL pedidoBL)
    {
        _ocupacionBL = ocupacionBL;
        _habitacionBL = habitacionBL;
        _reservaBL = reservaBL;
        _tareaBL = tareaBL;
        _alertasBL = alertasBL;
        _mesaBL = mesaBL;
        _pedidoBL = pedidoBL;
    }

    public async Task<IActionResult> Index()
    {
        var modelo = new InicioViewModel();
        var hoy = DateOnly.FromDateTime(DateTime.Today);

        // Hospedaje: habitaciones y movimiento de huéspedes del día
        if (TieneRol(Roles.AccesoHospedaje))
        {
            async Task<int> ContarHabitaciones(string estado) =>
                (await _habitacionBL.ObtenerAsync(new FiltroListado { Estado = estado, TamanoPagina = 1 })).TotalRegistros;

            var total = $"de {await ContarHabitaciones(Estados.Habitacion.Activas)} habitaciones";
            var reservadasHoy = (await _ocupacionBL.ObtenerAsync()).Values.Count(o => o.Estado == OcupacionHabitacionDto.Reservada);
            var disponibles = Math.Max(0, await ContarHabitaciones(Estados.Habitacion.Disponible) - reservadasHoy);
            modelo.Tarjetas.Add(Tarjeta("Habitaciones disponibles", disponibles,
                "bi-door-open", "verde", "Habitacion", Estados.Habitacion.Disponible,
                reservadasHoy > 0 ? $"{total} · {reservadasHoy} reservada(s) hoy" : total));
            modelo.Tarjetas.Add(Tarjeta("Ocupadas", await ContarHabitaciones(Estados.Habitacion.Ocupada),
                "bi-person-fill", "coral", "Habitacion", Estados.Habitacion.Ocupada, total));
            modelo.Tarjetas.Add(Tarjeta("En limpieza", await ContarHabitaciones(Estados.Habitacion.Limpieza),
                "bi-stars", "cielo", "Habitacion", Estados.Habitacion.Limpieza, "habitaciones"));

            var reservas = new List<ReservaDto>();
            foreach (var estado in new[] { Estados.Reserva.Pendiente, Estados.Reserva.Confirmada, Estados.Reserva.CheckIn })
            {
                var pagina = await _reservaBL.ObtenerAsync(new FiltroListado { Estado = estado, TamanoPagina = FiltroListado.TamanoPaginaMaximo });
                reservas.AddRange(pagina.Elementos);
            }

            modelo.MostrarActividad = true;
            modelo.Llegadas = reservas
                .Where(r => r.FechaEntrada == hoy)
                .OrderBy(r => r.NumeroHabitacion)
                .ToList();
            modelo.Salidas = reservas
                .Where(r => r.EstadoReserva == Estados.Reserva.CheckIn && r.FechaSalida <= hoy)
                .OrderBy(r => r.FechaSalida)
                .ThenBy(r => r.NumeroHabitacion)
                .ToList();
        }

        // Restaurante: mesas y pedidos
        if (TieneRol(Roles.AccesoMesas) && !TieneRol(Roles.AccesoHospedaje))
        {
            async Task<int> ContarMesas(string estado) =>
                (await _mesaBL.ObtenerAsync(new FiltroListado { Estado = estado, TamanoPagina = 1 })).TotalRegistros;

            var total = $"de {await ContarMesas(Estados.Mesa.Activas)} mesas";
            modelo.Tarjetas.Add(Tarjeta("Mesas disponibles", await ContarMesas(Estados.Mesa.Disponible),
                "bi-check2-circle", "verde", "Mesa", Estados.Mesa.Disponible, total));
            modelo.Tarjetas.Add(Tarjeta("Mesas ocupadas", await ContarMesas(Estados.Mesa.Ocupada),
                "bi-people-fill", "coral", "Mesa", Estados.Mesa.Ocupada, total));

            var pedidos = await _pedidoBL.ObtenerAsync(new FiltroListado { Estado = Estados.Pedido.Pendiente, TamanoPagina = 1 });
            modelo.Tarjetas.Add(Tarjeta("Pedidos pendientes", pedidos.TotalRegistros,
                "bi-receipt", "cielo", "Pedido", Estados.Pedido.Pendiente, "por confirmar"));
        }

        // Housekeeping: tareas activas, primero las vencidas
        if (TieneRol(Roles.AccesoHousekeeping))
        {
            var tareas = await _tareaBL.ObtenerAsync(null);
            var activas = tareas.Where(t => t.EstaActiva).ToList();

            modelo.MostrarTareas = true;
            modelo.Tareas = activas
                .OrderByDescending(t => t.EstaVencida)
                .ThenBy(t => t.FechaLimite ?? DateTime.MaxValue)
                .Take(MaximoPendientes)
                .ToList();

            if (!TieneRol(Roles.AccesoHospedaje))
            {
                modelo.Tarjetas.Add(Tarjeta("Tareas pendientes", activas.Count(t => t.EstadoTarea == Estados.Tarea.Pendiente),
                    "bi-hourglass-split", "ambar", "Housekeeping", Estados.Tarea.Pendiente, "por iniciar"));
                modelo.Tarjetas.Add(Tarjeta("En proceso", activas.Count(t => t.EstadoTarea == Estados.Tarea.EnProceso),
                    "bi-arrow-repeat", "cielo", "Housekeeping", Estados.Tarea.EnProceso, "en este momento"));
                modelo.Tarjetas.Add(TarjetaResumenViewModel.Crear("Vencidas", activas.Count(t => t.EstaVencida),
                    "bi-alarm", "coral", Url.Action("Index", "Housekeeping"), false, "pasaron la fecha límite"));
            }
            else if (!TieneRol(Roles.AccesoInventario))
            {
                modelo.Tarjetas.Add(Tarjeta("Tareas de limpieza", activas.Count,
                    "bi-stars", "ambar", "Housekeeping", Estados.Tarea.Pendiente, "pendientes o en proceso"));
            }
        }

        // Inventario: productos con stock bajo
        if (TieneRol(Roles.AccesoInventario))
        {
            var alertas = await _alertasBL.ObtenerAlertasAsync();

            modelo.MostrarAlertas = true;
            modelo.Alertas = alertas
                .OrderBy(p => p.Stock - p.StockMinimo)
                .Take(MaximoPendientes)
                .ToList();
            modelo.Tarjetas.Add(TarjetaResumenViewModel.Crear("Alertas de inventario", alertas.Count,
                "bi-exclamation-triangle", "ambar", Url.Action("Alertas", "Inventario"), false, "productos con stock bajo"));
        }

        return View(modelo);
    }

    [AllowAnonymous]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    private bool TieneRol(string roles) => roles.Split(',').Any(User.IsInRole);

    private TarjetaResumenViewModel Tarjeta(string titulo, int valor, string icono, string color,
        string controlador, string estado, string detalle) =>
        TarjetaResumenViewModel.Crear(titulo, valor, icono, color,
            Url.Action("Index", controlador, new { estado }), false, detalle);
}
