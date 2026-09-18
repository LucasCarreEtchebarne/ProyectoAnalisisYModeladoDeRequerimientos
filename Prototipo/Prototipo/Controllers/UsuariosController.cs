using System.Web.Mvc;
using Prototipo.Common;
using Prototipo.Infrastructure;
using Prototipo.Models;
using Prototipo.Services;

namespace Prototipo.Controllers
{
    [Autorizar(Roles = "Administrador")]
    public class UsuariosController : ControladorBase
    {
        private UsuarioService Servicio
        {
            get { return new UsuarioService(Db); }
        }

        public ActionResult Usuarios(string estado, string rol, string busqueda)
        {
            ViewBag.Estado = estado;
            ViewBag.Rol = rol;
            ViewBag.Busqueda = busqueda;
            ViewBag.Roles = Estados.Roles.Todos;
            return View(Servicio.Listar(estado, rol, busqueda));
        }

        public ActionResult Index()
        {
            return RedirectToAction("Usuarios");
        }

        public ActionResult DetalleUsuario(int id)
        {
            var usuario = Servicio.Obtener(id);
            if (usuario == null)
            {
                MensajeError(Mensajes.NoEncontrado);
                return RedirectToAction("Usuarios");
            }

            return View(usuario);
        }

        public ActionResult Crear()
        {
            ViewBag.Roles = Estados.Roles.Todos;
            ViewBag.Estados = Estados.Usuario.Todos;
            return View(new Usuario { Estado = Estados.Usuario.Activo, Rol = Estados.Roles.Recepcionista });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Crear(Usuario usuario, string contrasena)
        {
            ModelState.Remove("ContrasenaHash");
            usuario.ContrasenaHash = string.Empty;

            if (!ModelState.IsValid)
            {
                ViewBag.Roles = Estados.Roles.Todos;
                ViewBag.Estados = Estados.Usuario.Todos;
                return View(usuario);
            }

            var resultado = Servicio.Crear(usuario, contrasena);
            if (!resultado.Exito)
            {
                ModelState.AddModelError(string.Empty, resultado.Mensaje);
                ViewBag.Roles = Estados.Roles.Todos;
                ViewBag.Estados = Estados.Usuario.Todos;
                return View(usuario);
            }

            MensajeExito(resultado.Mensaje);
            return RedirectToAction("Usuarios");
        }

        public ActionResult Editar(int id)
        {
            var usuario = Servicio.Obtener(id);
            if (usuario == null)
            {
                MensajeError(Mensajes.NoEncontrado);
                return RedirectToAction("Usuarios");
            }

            ViewBag.Roles = Estados.Roles.Todos;
            ViewBag.Estados = Estados.Usuario.Todos;
            return View(usuario);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(Usuario usuario, string contrasenaNueva)
        {
            ModelState.Remove("ContrasenaHash");
            usuario.ContrasenaHash = string.Empty;

            if (!ModelState.IsValid)
            {
                ViewBag.Roles = Estados.Roles.Todos;
                ViewBag.Estados = Estados.Usuario.Todos;
                return View(usuario);
            }

            var resultado = Servicio.Actualizar(usuario, contrasenaNueva);
            if (!resultado.Exito)
            {
                ModelState.AddModelError(string.Empty, resultado.Mensaje);
                ViewBag.Roles = Estados.Roles.Todos;
                ViewBag.Estados = Estados.Usuario.Todos;
                return View(usuario);
            }

            MensajeExito(resultado.Mensaje);
            return RedirectToAction("Usuarios");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Inactivar(int id)
        {
            var resultado = Servicio.Inactivar(id);
            if (resultado.Exito)
            {
                MensajeExito(resultado.Mensaje);
            }
            else
            {
                MensajeError(resultado.Mensaje);
            }

            return RedirectToAction("Usuarios");
        }
    }
}
