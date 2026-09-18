using System.Web.Mvc;
using Prototipo.Infrastructure;
using Prototipo.Services;

namespace Prototipo.Controllers
{
    public class MainController : ControladorBase
    {
        public ActionResult Main()
        {
            var modelo = new ReporteService(Db).ObtenerDashboard();
            return View(modelo);
        }

        public ActionResult Index()
        {
            return RedirectToAction("Main");
        }
    }
}
