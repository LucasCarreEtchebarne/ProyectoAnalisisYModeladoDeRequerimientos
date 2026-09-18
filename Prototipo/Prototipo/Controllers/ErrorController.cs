using System.Net;
using System.Web.Mvc;

namespace Prototipo.Controllers
{
    [AllowAnonymous]
    public class ErrorController : Controller
    {
        public ActionResult NoEncontrado()
        {
            Response.StatusCode = (int)HttpStatusCode.NotFound;
            Response.TrySkipIisCustomErrors = true;
            return View();
        }

        public ActionResult AccesoDenegado()
        {
            Response.StatusCode = (int)HttpStatusCode.Forbidden;
            Response.TrySkipIisCustomErrors = true;
            return View();
        }

        public ActionResult ErrorServidor()
        {
            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            Response.TrySkipIisCustomErrors = true;
            return View();
        }
    }
}
