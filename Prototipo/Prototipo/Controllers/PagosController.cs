using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Prototipo.Common;
using Prototipo.Infrastructure;
using Prototipo.Models;
using Prototipo.Services;

namespace Prototipo.Controllers
{
    [Autorizar(Roles = "Administrador,Recepcionista,Mesero")]
    public class PagosController : ControladorBase
    {
        private PagoService Servicio
        {
            get { return new PagoService(Db); }
        }

        public ActionResult Pagos(string metodo, string estado, DateTime? desde, DateTime? hasta)
        {
            ViewBag.Metodo = metodo;
            ViewBag.Estado = estado;
            ViewBag.Desde = desde;
            ViewBag.Hasta = hasta;
            return View(Servicio.Listar(metodo, estado, desde, hasta));
        }

        public ActionResult Index()
        {
            return RedirectToAction("Pagos");
        }

        public ActionResult DetallePago(int id)
        {
            var pago = Servicio.Obtener(id);
            if (pago == null)
            {
                MensajeError(Mensajes.NoEncontrado);
                return RedirectToAction("Pagos");
            }

            return View(pago);
        }

        public ActionResult Crear(int? idFactura)
        {
            var facturas = new FacturaService(Db);

            ViewBag.FacturasPendientes = Db.Facturas
                .Include(f => f.Cliente)
                .Where(f => f.EstadoFactura == Estados.Factura.Pendiente)
                .OrderByDescending(f => f.FechaEmision)
                .ToList();

            var pago = new Pago
            {
                MetodoPago = Estados.MetodoPago.Efectivo,
                EstadoPago = Estados.Pago.Aprobado
            };

            if (idFactura.HasValue)
            {
                var factura = facturas.Obtener(idFactura.Value);
                if (factura != null)
                {
                    pago.IdFactura = factura.IdFactura;
                    pago.MontoPagado = facturas.SaldoPendiente(factura);
                    ViewBag.Factura = factura;
                    ViewBag.Saldo = pago.MontoPagado;
                }
            }

            return View(pago);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Crear(Pago pago)
        {
            ModelState.Remove("EstadoPago");
            pago.EstadoPago = Estados.Pago.Aprobado;

            if (!ModelState.IsValid)
            {
                var errores = ModelState.Values
                    .SelectMany(e => e.Errors)
                    .Select(e => string.IsNullOrEmpty(e.ErrorMessage) ? "Hay datos con formato invalido." : e.ErrorMessage)
                    .Distinct();

                MensajeError(string.Join(" ", errores));
                return RedirectToAction("Crear", new { idFactura = pago.IdFactura });
            }

            var resultado = Servicio.Registrar(pago);
            if (!resultado.Exito)
            {
                MensajeError(resultado.Mensaje);
                return RedirectToAction("Crear", new { idFactura = pago.IdFactura });
            }

            MensajeExito(resultado.Mensaje);
            return RedirectToAction("DetallePago", new { id = resultado.IdGenerado });
        }

        public ActionResult Editar(int id)
        {
            var pago = Servicio.Obtener(id);
            if (pago == null)
            {
                MensajeError(Mensajes.NoEncontrado);
                return RedirectToAction("Pagos");
            }

            return View(pago);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Reversar(int id, string motivo)
        {
            var resultado = Servicio.Reversar(id, motivo);
            if (resultado.Exito)
            {
                MensajeExito(resultado.Mensaje);
            }
            else
            {
                MensajeError(resultado.Mensaje);
            }

            return RedirectToAction("DetallePago", new { id });
        }
    }
}
