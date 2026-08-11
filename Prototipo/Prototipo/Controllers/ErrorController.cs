using System.Net;
using System.Web.Mvc;

namespace Prototipo.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error/NoEncontrado (404)
        public ActionResult NoEncontrado()
        {
            Response.StatusCode = (int)HttpStatusCode.NotFound;
            Response.TrySkipIisCustomErrors = true;
            return View();
        }

        // GET: Error/AccesoDenegado (403)
        public ActionResult AccesoDenegado()
        {
            Response.StatusCode = (int)HttpStatusCode.Forbidden;
            Response.TrySkipIisCustomErrors = true;
            return View();
        }

        // GET: Error/ErrorServidor (500)
        public ActionResult ErrorServidor()
        {
            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            Response.TrySkipIisCustomErrors = true;
            return View();
        }
    }
}
