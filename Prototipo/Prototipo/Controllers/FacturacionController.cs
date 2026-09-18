using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Prototipo.Common;
using Prototipo.Infrastructure;
using Prototipo.Services;

namespace Prototipo.Controllers
{
    [Autorizar(Roles = "Administrador,Recepcionista")]
    public class FacturacionController : ControladorBase
    {
        private FacturaService Servicio
        {
            get { return new FacturaService(Db); }
        }

        public ActionResult Facturacion(string estado, string tipo, DateTime? desde, DateTime? hasta, string busqueda)
        {
            ViewBag.Estado = estado;
            ViewBag.Tipo = tipo;
            ViewBag.Desde = desde;
            ViewBag.Hasta = hasta;
            ViewBag.Busqueda = busqueda;
            return View(Servicio.Listar(estado, tipo, desde, hasta, busqueda));
        }

        public ActionResult Index()
        {
            return RedirectToAction("Facturacion");
        }

        public ActionResult DetalleFactura(int id)
        {
            var factura = Servicio.Obtener(id);
            if (factura == null)
            {
                MensajeError(Mensajes.NoEncontrado);
                return RedirectToAction("Facturacion");
            }

            ViewBag.TotalPagado = Servicio.TotalPagado(id);
            ViewBag.Saldo = Servicio.SaldoPendiente(factura);
            return View(factura);
        }

        public ActionResult Crear()
        {
            var facturados = Db.DetallesFactura.ToList();

            ViewBag.Pedidos = Db.Pedidos
                .Include(p => p.Cliente)
                .Include(p => p.Detalles)
                .Where(p => p.EstadoPedido != Estados.Pedido.Cancelado && p.IdCliente != null)
                .ToList()
                .Where(p => !facturados.Any(d => d.IdPedido == p.IdPedido))
                .OrderByDescending(p => p.FechaHoraPedido)
                .Take(50)
                .ToList();

            ViewBag.Reservas = Db.Reservas
                .Include(r => r.Cliente)
                .Include(r => r.Habitacion)
                .Where(r => r.EstadoReserva != Estados.Reserva.Cancelada)
                .ToList()
                .Where(r => !facturados.Any(d => d.IdReserva == r.IdReserva))
                .OrderByDescending(r => r.FechaEntrada)
                .Take(50)
                .ToList();

            ViewBag.Eventos = Db.Eventos
                .Include(e => e.Cliente)
                .Include(e => e.Espacio)
                .Where(e => e.EstadoEvento != Estados.Evento.Cancelado)
                .ToList()
                .Where(e => !facturados.Any(d => d.IdEvento == e.IdEvento))
                .OrderByDescending(e => e.FechaEvento)
                .Take(50)
                .ToList();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Crear(string origen, int idOrigen)
        {
            ResultadoOperacion resultado;

            switch (origen)
            {
                case "pedido":
                    resultado = Servicio.CrearDesdePedido(idOrigen);
                    break;
                case "reserva":
                    resultado = Servicio.CrearDesdeReserva(idOrigen);
                    break;
                case "mixta":
                    resultado = Servicio.CrearMixtaDeReserva(idOrigen);
                    break;
                case "evento":
                    resultado = Servicio.CrearDesdeEvento(idOrigen);
                    break;
                default:
                    resultado = ResultadoOperacion.Error("Seleccione un origen valido para la factura.");
                    break;
            }

            if (!resultado.Exito)
            {
                MensajeError(resultado.Mensaje);
                return RedirectToAction("Crear");
            }

            MensajeExito(resultado.Mensaje);
            return RedirectToAction("DetalleFactura", new { id = resultado.IdGenerado });
        }

        public ActionResult Editar(int id)
        {
            var factura = Servicio.Obtener(id);
            if (factura == null)
            {
                MensajeError(Mensajes.NoEncontrado);
                return RedirectToAction("Facturacion");
            }

            return View(factura);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Anular(int id, string motivoAnulacion)
        {
            var resultado = Servicio.Anular(id, motivoAnulacion);
            if (!resultado.Exito)
            {
                MensajeError(resultado.Mensaje);
                return RedirectToAction("Editar", new { id });
            }

            MensajeExito(resultado.Mensaje);
            return RedirectToAction("DetalleFactura", new { id });
        }
    }
}
