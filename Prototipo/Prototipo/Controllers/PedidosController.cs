using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Prototipo.Common;
using Prototipo.Infrastructure;
using Prototipo.Models;
using Prototipo.Services;
using Prototipo.ViewModels;

namespace Prototipo.Controllers
{
    [Autorizar(Roles = "Administrador,Mesero,Recepcionista")]
    public class PedidosController : ControladorBase
    {
        private PedidoService Servicio
        {
            get { return new PedidoService(Db); }
        }

        private void CargarCatalogos(PedidoFormViewModel modelo)
        {
            modelo.Clientes = new ClienteService(Db).ListarActivos();
            modelo.Mesas = new MesaService(Db).Listar(null);
            modelo.Menu = new MenuService(Db).ListarDisponibles();

            modelo.HabitacionesOcupadas = Db.Reservas
                .Where(r => r.EstadoReserva == Estados.Reserva.CheckIn)
                .Select(r => r.Habitacion)
                .Distinct()
                .OrderBy(h => h.NumeroHabitacion)
                .ToList();

            ViewBag.TiposPedido = Estados.TipoPedido.Todos;
        }

        public ActionResult Pedidos(string estado, string tipo, DateTime? desde, DateTime? hasta)
        {
            ViewBag.Estado = estado;
            ViewBag.Tipo = tipo;
            ViewBag.Desde = desde;
            ViewBag.Hasta = hasta;
            return View(Servicio.Listar(estado, tipo, desde, hasta));
        }

        public ActionResult Index()
        {
            return RedirectToAction("Pedidos");
        }

        public ActionResult DetallePedido(int id)
        {
            var pedido = Servicio.Obtener(id);
            if (pedido == null)
            {
                MensajeError(Mensajes.NoEncontrado);
                return RedirectToAction("Pedidos");
            }

            return View(pedido);
        }

        public ActionResult Crear(int? idMesa)
        {
            var modelo = new PedidoFormViewModel
            {
                TipoPedido = Estados.TipoPedido.Mesa,
                IdMesa = idMesa
            };

            CargarCatalogos(modelo);
            return View(modelo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Crear(PedidoFormViewModel modelo)
        {
            var detalles = (modelo.Lineas ?? new List<LineaPedidoViewModel>())
                .Where(l => l.Cantidad > 0)
                .Select(l => new DetallePedido { IdProductoMenu = l.IdProductoMenu, Cantidad = l.Cantidad })
                .ToList();

            if (detalles.Count == 0)
            {
                ModelState.AddModelError(string.Empty, Mensajes.PedidoSinDetalle);
            }

            if (!ModelState.IsValid)
            {
                CargarCatalogos(modelo);
                return View(modelo);
            }

            var pedido = new Pedido
            {
                IdCliente = modelo.IdCliente,
                IdUsuario = SesionActual.IdUsuario,
                IdMesa = modelo.IdMesa,
                IdHabitacion = modelo.IdHabitacion,
                TipoPedido = modelo.TipoPedido,
                Observaciones = modelo.Observaciones
            };

            var resultado = Servicio.Crear(pedido, detalles);
            if (!resultado.Exito)
            {
                ModelState.AddModelError(string.Empty, resultado.Mensaje);
                CargarCatalogos(modelo);
                return View(modelo);
            }

            MensajeExito(resultado.Mensaje);
            return RedirectToAction("DetallePedido", new { id = resultado.IdGenerado });
        }

        public ActionResult Editar(int id)
        {
            var pedido = Servicio.Obtener(id);
            if (pedido == null)
            {
                MensajeError(Mensajes.NoEncontrado);
                return RedirectToAction("Pedidos");
            }

            ViewBag.Clientes = new SelectList(new ClienteService(Db).ListarActivos(),
                "IdCliente", "NombreMostrar", pedido.IdCliente);

            return View(pedido);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(int id, int? idCliente, string observaciones)
        {
            var pedido = Servicio.Obtener(id);
            if (pedido == null)
            {
                MensajeError(Mensajes.NoEncontrado);
                return RedirectToAction("Pedidos");
            }

            pedido.IdCliente = idCliente;
            pedido.Observaciones = observaciones;
            Db.SaveChanges();

            MensajeExito(Mensajes.ActualizadoOk);
            return RedirectToAction("DetallePedido", new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Confirmar(int id)
        {
            Notificar(Servicio.Confirmar(id));
            return RedirectToAction("DetallePedido", new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Entregar(int id)
        {
            Notificar(Servicio.Entregar(id));
            return RedirectToAction("DetallePedido", new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cancelar(int id)
        {
            Notificar(Servicio.Cancelar(id));
            return RedirectToAction("Pedidos");
        }

        private void Notificar(ResultadoOperacion resultado)
        {
            if (resultado.Exito)
            {
                MensajeExito(resultado.Mensaje);
            }
            else
            {
                MensajeError(resultado.Mensaje);
            }
        }
    }
}
