using System;
using System.Web.Mvc;
using Prototipo.Infrastructure;
using Prototipo.Services;
using Prototipo.ViewModels;

namespace Prototipo.Controllers
{
    [Autorizar(Roles = "Administrador,Recepcionista")]
    public class ReportesController : ControladorBase
    {
        private ReporteService Servicio
        {
            get { return new ReporteService(Db); }
        }

        public ActionResult Reportes()
        {
            ViewBag.Dashboard = Servicio.ObtenerDashboard();
            return View(ReporteViewModel.Tipos);
        }

        public ActionResult Index()
        {
            return RedirectToAction("Reportes");
        }

        public ActionResult Generar(string tipo, DateTime? desde, DateTime? hasta)
        {
            var inicio = desde ?? DateTime.Today.AddDays(-30);
            var fin = hasta ?? DateTime.Today;

            ViewBag.Tipos = ReporteViewModel.Tipos;

            if (string.IsNullOrEmpty(tipo))
            {
                return View(new ReporteViewModel { Desde = inicio, Hasta = fin });
            }

            return View(Servicio.Generar(tipo, inicio, fin));
        }

        public ActionResult DetalleReporte(string tipo, DateTime? desde, DateTime? hasta)
        {
            var inicio = desde ?? DateTime.Today.AddDays(-30);
            var fin = hasta ?? DateTime.Today;
            return View(Servicio.Generar(tipo, inicio, fin));
        }
    }
}
