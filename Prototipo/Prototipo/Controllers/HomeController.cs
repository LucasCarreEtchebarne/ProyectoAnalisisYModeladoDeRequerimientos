using System.Web.Mvc;

namespace Prototipo.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home (Página de inicio)
        public ActionResult Index()
        {
            return View();
        }
    }
}
