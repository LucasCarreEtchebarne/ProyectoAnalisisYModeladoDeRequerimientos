using System.Web.Mvc;
using Prototipo.Data;

namespace Prototipo.Infrastructure
{
    [Autorizar]
    public abstract class ControladorBase : Controller
    {
        protected readonly HotelColibriContext Db = new HotelColibriContext();

        protected void MensajeExito(string mensaje)
        {
            TempData["MensajeExito"] = mensaje;
        }

        protected void MensajeError(string mensaje)
        {
            TempData["MensajeError"] = mensaje;
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ViewBag.UsuarioNombre = SesionActual.NombreCompleto;
            ViewBag.UsuarioRol = SesionActual.Rol;
            base.OnActionExecuting(filterContext);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Db.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
