using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Clientes;
using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Facturas;
using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Habitaciones;
using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Menus;
using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Mesas;
using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Pedidos;
using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Reservas;
using SistemaHotelColibri.Abstract.Modelos.Facturas;
using SistemaHotelColibri.Abstract.Modelos.Pedidos;
using SistemaHotelColibri.Abstract.Modelos.Reservas;
using SistemaHotelColibri.ViewModels.Comun;
using SistemaHotelColibri.ViewModels.Salon;

namespace SistemaHotelColibri.Controllers;

// Pedidos del restaurante: cuadritos de mesas, habitaciones y para llevar,
// la cuenta de cada uno con el menú por categorías y el cobro con factura.
[Authorize(Roles = Roles.AccesoMenu)]
public class SalonController : ControladorBase
{
    private const string Mesa = Catalogos.TipoPedido.Mesa;
    private const string Habitacion = Catalogos.TipoPedido.Habitacion;
    private const string Llevar = Catalogos.TipoPedido.Llevar;

    private readonly IObtenerMesaBL _obtenerMesaBL;
    private readonly IObtenerMesaPorIdBL _obtenerMesaPorIdBL;
    private readonly IObtenerHabitacionBL _obtenerHabitacionBL;
    private readonly IObtenerHabitacionPorIdBL _obtenerHabitacionPorIdBL;
    private readonly IObtenerOcupacionHabitacionBL _ocupacionBL;
    private readonly IObtenerMenuBL _obtenerMenuBL;
    private readonly IObtenerPedidoBL _obtenerPedidoBL;
    private readonly ICuentaPedidoBL _cuentaBL;
    private readonly IRegistrarFacturaRestauranteBL _facturarBL;
    private readonly IRegistrarPagoBL _registrarPagoBL;
    private readonly IObtenerFacturaPorIdBL _obtenerFacturaBL;
    private readonly IObtenerClienteBL _obtenerClienteBL;

    public SalonController(
        IObtenerMesaBL obtenerMesaBL,
        IObtenerMesaPorIdBL obtenerMesaPorIdBL,
        IObtenerHabitacionBL obtenerHabitacionBL,
        IObtenerHabitacionPorIdBL obtenerHabitacionPorIdBL,
        IObtenerOcupacionHabitacionBL ocupacionBL,
        IObtenerMenuBL obtenerMenuBL,
        IObtenerPedidoBL obtenerPedidoBL,
        ICuentaPedidoBL cuentaBL,
        IRegistrarFacturaRestauranteBL facturarBL,
        IRegistrarPagoBL registrarPagoBL,
        IObtenerFacturaPorIdBL obtenerFacturaBL,
        IObtenerClienteBL obtenerClienteBL)
    {
        _obtenerMesaBL = obtenerMesaBL;
        _obtenerMesaPorIdBL = obtenerMesaPorIdBL;
        _obtenerHabitacionBL = obtenerHabitacionBL;
        _obtenerHabitacionPorIdBL = obtenerHabitacionPorIdBL;
        _ocupacionBL = ocupacionBL;
        _obtenerMenuBL = obtenerMenuBL;
        _obtenerPedidoBL = obtenerPedidoBL;
        _cuentaBL = cuentaBL;
        _facturarBL = facturarBL;
        _registrarPagoBL = registrarPagoBL;
        _obtenerFacturaBL = obtenerFacturaBL;
        _obtenerClienteBL = obtenerClienteBL;
    }

    public async Task<IActionResult> Index(string? vista)
    {
        var abiertas = await _cuentaBL.ObtenerAbiertasAsync();
        var mesas = await CargarTodasAsync(_obtenerMesaBL.ObtenerAsync, Estados.Mesa.Activas);
        var habitaciones = await CargarTodasAsync(_obtenerHabitacionBL.ObtenerAsync, Estados.Habitacion.Activas);
        var ocupacion = await _ocupacionBL.ObtenerAsync();

        var modelo = new SalonViewModel
        {
            Vista = vista is SalonViewModel.VistaHabitaciones or SalonViewModel.VistaLlevar ? vista : SalonViewModel.VistaMesas,

            Mesas = mesas.Select(mesa =>
            {
                var cuenta = abiertas.FirstOrDefault(p => p.TipoPedido == Mesa && p.IdMesa == mesa.IdMesa);
                return new CuadroViewModel
                {
                    Tipo = Mesa,
                    Id = mesa.IdMesa,
                    Etiqueta = "Mesa",
                    Numero = mesa.NumeroMesa.ToString(),
                    Capacidad = mesa.Capacidad,
                    Cuenta = cuenta,
                    Estado = cuenta != null || mesa.EstadoMesa == Estados.Mesa.Ocupada ? "en-uso"
                        : mesa.EstadoMesa == Estados.Mesa.Reservada ? "reservada"
                        : "libre"
                };
            }).ToList(),

            Habitaciones = habitaciones
                .OrderBy(h => int.TryParse(h.NumeroHabitacion, out var numero) ? numero : int.MaxValue)
                .ThenBy(h => h.NumeroHabitacion)
                .Select(habitacion =>
                {
                    var cuenta = abiertas.FirstOrDefault(p => p.TipoPedido == Habitacion && p.IdHabitacion == habitacion.IdHabitacion);
                    var oc = ocupacion.GetValueOrDefault(habitacion.IdHabitacion);
                    return new CuadroViewModel
                    {
                        Tipo = Habitacion,
                        Id = habitacion.IdHabitacion,
                        Etiqueta = "Habitación",
                        Numero = habitacion.NumeroHabitacion,
                        Capacidad = habitacion.Capacidad,
                        Cuenta = cuenta,
                        Nota = oc?.ReservaActual?.NombreCliente,
                        Estado = cuenta != null ? "en-uso"
                            : oc?.Estado == OcupacionHabitacionDto.Hospedada || habitacion.EstadoHabitacion == Estados.Habitacion.Ocupada ? "hospedada"
                            : oc?.Estado == OcupacionHabitacionDto.Reservada ? "reservada"
                            : "libre"
                    };
                }).ToList(),

            ParaLlevar = abiertas
                .Where(p => p.TipoPedido == Llevar)
                .Select(cuenta => new CuadroViewModel
                {
                    Tipo = Llevar,
                    Id = cuenta.IdPedido,
                    Etiqueta = "Para llevar",
                    Numero = "#" + cuenta.IdPedido,
                    Cuenta = cuenta,
                    Nota = cuenta.NombreCliente,
                    Estado = "en-uso"
                }).ToList()
        };

        async Task<int> Contar(string estado) =>
            (await _obtenerPedidoBL.ObtenerAsync(new FiltroListado { Estado = estado, TamanoPagina = 1 })).TotalRegistros;

        modelo.Resumen =
        [
            TarjetaResumenViewModel.Crear("Pendientes", await Contar(Estados.Pedido.Pendiente), "bi-hourglass-split", "ambar", detalle: "cuentas abiertas"),
            TarjetaResumenViewModel.Crear("Confirmados", await Contar(Estados.Pedido.Confirmado), "bi-fire", "cielo", detalle: "en preparación"),
            TarjetaResumenViewModel.Crear("Entregados", await Contar(Estados.Pedido.Entregado), "bi-bag-check", "verde", detalle: "pedidos cobrados"),
            TarjetaResumenViewModel.Crear("Cancelados", await Contar(Estados.Pedido.Cancelado), "bi-x-circle", "coral", detalle: "pedidos")
        ];

        return View(modelo);
    }

    // tipo: Mesa (id = mesa), Habitacion (id = habitación) o Llevar (id = pedido; 0 = nuevo).
    public async Task<IActionResult> Cuenta(string tipo, int id, string? categoria)
    {
        var destino = await ResolverAsync(tipo, id);
        if (destino == null)
        {
            return RedirectToAction(nameof(Index), new { vista = CuentaViewModel.VistaDe(tipo) });
        }

        var productos = await CargarTodasAsync(_obtenerMenuBL.ObtenerAsync, Estados.ProductoMenu.Disponible);

        // Primero las categorías del catálogo (en su orden) y después cualquier otra que tenga productos.
        var categorias = Catalogos.CategoriasMenu
            .Where(c => productos.Any(p => p.CategoriaMenu == c))
            .Concat(productos.Select(p => p.CategoriaMenu).Where(c => !Catalogos.CategoriasMenu.Contains(c)).Distinct().Order())
            .ToList();

        return View(new CuentaViewModel
        {
            Tipo = destino.Tipo,
            Id = destino.Id,
            Titulo = destino.Titulo,
            Subtitulo = destino.Subtitulo,
            VistaRegreso = CuentaViewModel.VistaDe(destino.Tipo),
            Cuenta = destino.Cuenta,
            Productos = productos,
            Categorias = categorias,
            CategoriaSeleccionada = categoria != null && categorias.Contains(categoria) ? categoria : CuentaViewModel.TodasLasCategorias
        });
    }

    [HttpPost]
    public async Task<IActionResult> Agregar(string tipo, int id, int idProducto, string? categoria)
    {
        var destino = await ResolverAsync(tipo, id);
        if (destino == null)
        {
            return RedirectToAction(nameof(Index), new { vista = CuentaViewModel.VistaDe(tipo) });
        }

        ResultadoOperacion resultado;
        if (destino.Cuenta != null)
        {
            resultado = await _cuentaBL.AgregarProductoAsync(destino.Cuenta.IdPedido, idProducto);
        }
        else
        {
            resultado = await _cuentaBL.AbrirAsync(destino.NuevaCuenta(IdUsuarioActual), idProducto);

            // Un pedido para llevar nuevo pasa a identificarse por su número de pedido.
            if (resultado.Exito && destino.Tipo == Llevar)
            {
                id = resultado.IdGenerado;
            }
        }

        if (!resultado.Exito)
        {
            MensajeError(resultado.Mensaje);
        }

        return RedirectToAction(nameof(Cuenta), new { tipo = destino.Tipo, id, categoria });
    }

    [HttpPost]
    public async Task<IActionResult> Cantidad(string tipo, int id, int idProducto, int cambio, string? categoria)
    {
        var destino = await ResolverAsync(tipo, id);
        if (destino?.Cuenta == null)
        {
            MensajeError("No hay una cuenta abierta.");
            return RedirectToAction(nameof(Index), new { vista = CuentaViewModel.VistaDe(tipo) });
        }

        var resultado = await _cuentaBL.CambiarCantidadAsync(destino.Cuenta.IdPedido, idProducto, cambio);
        if (!resultado.Exito)
        {
            MensajeError(resultado.Mensaje);
        }

        // Un pedido para llevar al que se le quitó todo ya no existe: se vuelve a la lista.
        if (destino.Tipo == Llevar && resultado.Exito && resultado.IdGenerado == 0)
        {
            MensajeExito(resultado.Mensaje);
            return RedirectToAction(nameof(Index), new { vista = SalonViewModel.VistaLlevar });
        }

        return RedirectToAction(nameof(Cuenta), new { tipo = destino.Tipo, id, categoria });
    }

    [HttpPost]
    public async Task<IActionResult> Nota(string tipo, int id, string? observaciones, string? categoria)
    {
        var destino = await ResolverAsync(tipo, id);
        if (destino?.Cuenta == null)
        {
            MensajeError("Agregá un producto antes de escribir una nota.");
        }
        else
        {
            MostrarResultado(await _cuentaBL.GuardarNotaAsync(destino.Cuenta.IdPedido, observaciones));
        }

        return RedirectToAction(nameof(Cuenta), new { tipo, id, categoria });
    }

    public async Task<IActionResult> Cobrar(string tipo, int id)
    {
        return await MostrarCobroAsync(tipo, id, new CobroViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> Cobrar(string tipo, int id, CobroViewModel modelo)
    {
        var destino = await ResolverAsync(tipo, id);
        if (destino?.Cuenta == null)
        {
            MensajeError("No hay una cuenta abierta para cobrar.");
            return RedirectToAction(nameof(Index), new { vista = CuentaViewModel.VistaDe(tipo) });
        }

        // El total lleva servicio (10 %) e IVA (13 %), igual que en Facturación.
        var totales = TotalesDe(destino.Cuenta);
        var errorPago = ValidarPagoAntesDeFacturar(modelo, totales.Total);
        if (errorPago != null)
        {
            ModelState.AddModelError(string.Empty, errorPago);
        }

        if (ModelState.IsValid)
        {
            var cliente = await _cuentaBL.AsignarClienteAsync(destino.Cuenta.IdPedido, modelo.IdCliente);
            if (!cliente.Exito)
            {
                ModelState.AddModelError(string.Empty, cliente.Mensaje);
                return await MostrarCobroAsync(tipo, id, modelo);
            }

            // 1) La factura se registra con la facturación del módulo de Facturas.
            var factura = await _facturarBL.RegistrarAsync(destino.Cuenta.IdPedido, IdUsuarioActual);
            if (!factura.Exito)
            {
                ModelState.AddModelError(string.Empty, factura.Mensaje);
                return await MostrarCobroAsync(tipo, id, modelo);
            }

            // 2) Se paga completa en el mismo momento.
            var emitida = await _obtenerFacturaBL.ObtenerPorIdAsync(factura.IdGenerado);
            var pago = await _registrarPagoBL.RegistrarAsync(new PagoDto
            {
                IdFactura = factura.IdGenerado,
                MetodoPago = modelo.MetodoPago,
                MontoPagado = emitida?.Saldo ?? totales.Total,
                MontoRecibido = modelo.MontoRecibido,
                TipoTarjeta = modelo.TipoTarjeta,
                UltimosCuatroDigitos = modelo.UltimosCuatroDigitos,
                NumeroReferencia = modelo.NumeroReferencia
            }, IdUsuarioActual);

            if (pago.Exito)
            {
                MensajeExito($"{factura.Mensaje} {pago.Mensaje}");
            }
            else
            {
                MensajeError($"La factura se generó pero el pago no se registró: {pago.Mensaje} Registralo desde Facturación.");
            }

            return RedirectToAction(nameof(Factura), new { id = factura.IdGenerado });
        }

        return await MostrarCobroAsync(tipo, id, modelo);
    }

    // Comprobante de la factura para el personal del restaurante (los meseros no entran a Facturación).
    public async Task<IActionResult> Factura(int id)
    {
        var factura = await _obtenerFacturaBL.ObtenerPorIdAsync(id);
        if (factura == null || factura.TipoFactura != Facturacion.Tipo.Restaurante)
        {
            MensajeError(Mensajes.NoEncontrado);
            return RedirectToAction(nameof(Index));
        }

        return View(factura);
    }

    private static TotalesFactura TotalesDe(PedidoDto cuenta)
    {
        var subtotal = cuenta.Detalles.Sum(d => Facturacion.Redondear(d.Cantidad * d.PrecioUnitario));
        return Facturacion.CalcularTotales(subtotal, subtotal);
    }

    // Se revisa antes de crear la factura para no dejar facturas a medio cobrar.
    private static string? ValidarPagoAntesDeFacturar(CobroViewModel modelo, decimal total)
    {
        if (!Facturacion.MetodoPago.Todos.Contains(modelo.MetodoPago))
        {
            return "Elegí un método de pago válido.";
        }

        return modelo.MetodoPago switch
        {
            Facturacion.MetodoPago.Efectivo when modelo.MontoRecibido is not { } recibido || recibido < total =>
                $"El efectivo recibido debe cubrir el total de {Formato.Colones(total)}.",
            Facturacion.MetodoPago.Tarjeta when modelo.TipoTarjeta == null || !Facturacion.TiposTarjeta.Contains(modelo.TipoTarjeta) =>
                "Elegí el tipo de tarjeta.",
            Facturacion.MetodoPago.Tarjeta when modelo.UltimosCuatroDigitos is not { Length: 4 } digitos || !digitos.All(char.IsAsciiDigit) =>
                "Escribí los últimos 4 dígitos de la tarjeta.",
            Facturacion.MetodoPago.Sinpe or Facturacion.MetodoPago.Transferencia when string.IsNullOrWhiteSpace(modelo.NumeroReferencia) =>
                "Escribí el número de comprobante.",
            _ => null
        };
    }

    private async Task<IActionResult> MostrarCobroAsync(string tipo, int id, CobroViewModel modelo)
    {
        var destino = await ResolverAsync(tipo, id);
        if (destino?.Cuenta == null)
        {
            MensajeError("No hay productos para cobrar.");
            return destino == null
                ? RedirectToAction(nameof(Index), new { vista = CuentaViewModel.VistaDe(tipo) })
                : RedirectToAction(nameof(Cuenta), new { tipo, id });
        }

        var clientes = await _obtenerClienteBL.ObtenerAsync(Estados.Cliente.Activo);

        modelo.Tipo = destino.Tipo;
        modelo.Id = destino.Id;
        modelo.Titulo = destino.Titulo;
        modelo.Cuenta = destino.Cuenta;
        modelo.Totales = TotalesDe(destino.Cuenta);
        modelo.IdCliente ??= destino.Cuenta.IdCliente;
        modelo.Clientes = clientes
            .Select(c => new SelectListItem($"{c.NombreMostrar} · {c.Identificacion}", c.IdCliente.ToString()))
            .ToList();

        return View("Cobrar", modelo);
    }

    // Busca la mesa, habitación o pedido para llevar y su cuenta abierta (si tiene).
    private async Task<Destino?> ResolverAsync(string tipo, int id)
    {
        var abiertas = await _cuentaBL.ObtenerAbiertasAsync();

        switch (tipo)
        {
            case Mesa:
                {
                    var mesa = await _obtenerMesaPorIdBL.ObtenerPorIdAsync(id);
                    if (mesa == null || mesa.EstadoMesa == Estados.Mesa.Inactiva)
                    {
                        MensajeError("La mesa no existe o está inactiva.");
                        return null;
                    }

                    var cuenta = abiertas.FirstOrDefault(p => p.TipoPedido == Mesa && p.IdMesa == id);
                    return new Destino(Mesa, id, $"Mesa {mesa.NumeroMesa}",
                        $"{mesa.Capacidad} personas", cuenta, IdMesa: id);
                }

            case Habitacion:
                {
                    var habitacion = await _obtenerHabitacionPorIdBL.ObtenerPorIdAsync(id);
                    if (habitacion == null || habitacion.EstadoHabitacion == Estados.Habitacion.Inactiva)
                    {
                        MensajeError("La habitación no existe o está inactiva.");
                        return null;
                    }

                    var cuenta = abiertas.FirstOrDefault(p => p.TipoPedido == Habitacion && p.IdHabitacion == id);
                    var oc = (await _ocupacionBL.ObtenerAsync()).GetValueOrDefault(id);
                    var actual = oc?.ReservaActual;
                    var subtitulo = oc?.Estado switch
                    {
                        OcupacionHabitacionDto.Hospedada => $"Huésped: {actual!.NombreCliente}",
                        OcupacionHabitacionDto.Reservada => $"Reservada hoy para {actual!.NombreCliente} (sin check-in)",
                        _ => $"{habitacion.TipoHabitacion} · sin huésped en check-in"
                    };

                    // Solo una reserva en check-in se liga al pedido (así el consumo queda a nombre del huésped).
                    var idReserva = oc?.Estado == OcupacionHabitacionDto.Hospedada ? actual!.IdReserva : (int?)null;

                    return new Destino(Habitacion, id, $"Habitación {habitacion.NumeroHabitacion}",
                        subtitulo, cuenta, IdHabitacion: id, IdReserva: idReserva);
                }

            case Llevar:
                {
                    if (id == 0)
                    {
                        return new Destino(Llevar, 0, "Nuevo pedido para llevar", "Tocá un producto para empezar el pedido", null);
                    }

                    var cuenta = abiertas.FirstOrDefault(p => p.TipoPedido == Llevar && p.IdPedido == id);
                    if (cuenta == null)
                    {
                        MensajeError("Ese pedido para llevar ya se cobró o se cerró.");
                        return null;
                    }

                    return new Destino(Llevar, id, $"Para llevar #{id}",
                        cuenta.NombreCliente ?? "Cliente de paso", cuenta);
                }

            default:
                MensajeError("Tipo de pedido no válido.");
                return null;
        }
    }

    private static async Task<List<T>> CargarTodasAsync<T>(
        Func<FiltroListado, Task<ResultadoPaginado<T>>> obtenerAsync, string estado)
    {
        var elementos = new List<T>();
        var filtro = new FiltroListado { Estado = estado, TamanoPagina = FiltroListado.TamanoPaginaMaximo };

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

    private sealed record Destino(
        string Tipo, int Id, string Titulo, string Subtitulo, PedidoDto? Cuenta,
        int? IdMesa = null, int? IdHabitacion = null, int? IdReserva = null)
    {
        public PedidoDto NuevaCuenta(int idUsuario) => new()
        {
            IdUsuario = idUsuario,
            TipoPedido = Tipo,
            IdMesa = IdMesa,
            IdHabitacion = IdHabitacion,
            IdReserva = IdReserva
        };
    }
}
