using System;
using System.Linq;
using System.Web.Mvc;
using Prototipo.Common;
using Prototipo.Infrastructure;
using Prototipo.Services;

namespace Prototipo.Controllers
{
    public class IAController : ControladorBase
    {
        private ConsultaIAService Servicio
        {
            get { return new ConsultaIAService(Db); }
        }

        public ActionResult IA()
        {
            return View(Servicio.Historial());
        }

        public ActionResult Index()
        {
            return RedirectToAction("IA");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Consultar(string consulta)
        {
            if (string.IsNullOrWhiteSpace(consulta))
            {
                MensajeError("Escriba una consulta.");
                return RedirectToAction("IA");
            }

            var resultado = Servicio.Responder(consulta);
            TempData["UltimaConsulta"] = resultado.ConsultaIngresada;
            TempData["UltimaRespuesta"] = resultado.RespuestaGenerada;
            return RedirectToAction("IA");
        }

        public ActionResult Predicciones()
        {
            var hoy = DateTime.Today;
            var proximaSemana = hoy.AddDays(7);

            ViewBag.ReservasProximaSemana = Db.Reservas
                .Count(r => r.FechaEntrada >= hoy && r.FechaEntrada < proximaSemana
                         && r.EstadoReserva != Estados.Reserva.Cancelada);

            var habitaciones = Db.Habitaciones.Count();
            var nochesComprometidas = Db.Reservas
                .Where(r => r.EstadoReserva != Estados.Reserva.Cancelada
                         && r.FechaEntrada < proximaSemana && r.FechaSalida > hoy)
                .Select(r => (int?)r.CantidadNoches).Sum() ?? 0;

            ViewBag.OcupacionProyectada = habitaciones == 0
                ? 0m
                : decimal.Round(nochesComprometidas * 100m / (habitaciones * 7), 1);

            var inicioMesAnterior = new DateTime(hoy.Year, hoy.Month, 1).AddMonths(-1);
            var inicioMes = new DateTime(hoy.Year, hoy.Month, 1);

            var ingresoMesAnterior = Db.Pagos
                .Where(p => p.EstadoPago == Estados.Pago.Aprobado
                         && p.FechaHoraPago >= inicioMesAnterior && p.FechaHoraPago < inicioMes)
                .Select(p => (decimal?)p.MontoPagado).Sum() ?? 0m;

            var ingresoMesActual = Db.Pagos
                .Where(p => p.EstadoPago == Estados.Pago.Aprobado && p.FechaHoraPago >= inicioMes)
                .Select(p => (decimal?)p.MontoPagado).Sum() ?? 0m;

            ViewBag.IngresoMesAnterior = ingresoMesAnterior;
            ViewBag.IngresoMesActual = ingresoMesActual;

            var diasTranscurridos = hoy.Day;
            var diasDelMes = DateTime.DaysInMonth(hoy.Year, hoy.Month);
            ViewBag.IngresoProyectado = diasTranscurridos == 0
                ? 0m
                : decimal.Round(ingresoMesActual / diasTranscurridos * diasDelMes, 2);

            ViewBag.InsumosCriticos = Db.Inventario
                .Where(p => p.EstadoProducto == Estados.Producto.Activo && p.Stock <= p.StockMinimo)
                .OrderBy(p => p.NombreProducto)
                .ToList();

            return View();
        }
    }
}
