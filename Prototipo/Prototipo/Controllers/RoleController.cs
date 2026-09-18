using System.Linq;
using System.Web.Mvc;
using Prototipo.Common;
using Prototipo.Infrastructure;

namespace Prototipo.Controllers
{
    [Autorizar(Roles = "Administrador")]
    public class RoleController : ControladorBase
    {
        public ActionResult Roles()
        {
            var conteos = Db.Usuarios
                .GroupBy(u => u.Rol)
                .Select(g => new { Rol = g.Key, Cantidad = g.Count() })
                .ToDictionary(g => g.Rol, g => g.Cantidad);

            ViewBag.Conteos = conteos;
            return View(Estados.Roles.Todos);
        }

        public ActionResult Index()
        {
            return RedirectToAction("Roles");
        }

        public ActionResult DetalleRol(string rol)
        {
            if (string.IsNullOrEmpty(rol) || !Estados.Roles.Todos.Contains(rol))
            {
                MensajeError("El rol indicado no existe.");
                return RedirectToAction("Roles");
            }

            ViewBag.Rol = rol;
            return View(Db.Usuarios.Where(u => u.Rol == rol).OrderBy(u => u.NombreCompleto).ToList());
        }

        public ActionResult Crear()
        {
            MensajeError("Los roles estan fijados por la base de datos (CHECK de USUARIO.Rol) " +
                         "y no se crean desde la aplicacion.");
            return RedirectToAction("Roles");
        }

        public ActionResult Editar()
        {
            MensajeError("Los roles estan fijados por la base de datos y no se editan desde la aplicacion. " +
                         "Para cambiar el rol de una persona use el modulo de Usuarios.");
            return RedirectToAction("Roles");
        }
    }
}
