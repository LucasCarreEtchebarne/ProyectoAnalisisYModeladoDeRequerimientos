using System.Linq;
using System.Web.Mvc;
using Prototipo.Common;
using Prototipo.Infrastructure;

namespace Prototipo.Controllers
{
    [AllowAnonymous]
    public class HomeController : ControladorBase
    {
        public ActionResult Index()
        {
            ViewBag.HabitacionesDisponibles = Db.Habitaciones
                .Count(h => h.EstadoHabitacion == Estados.Habitacion.Disponible);

            return View();
        }
    }
}
