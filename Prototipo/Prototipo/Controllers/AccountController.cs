using System.Web.Mvc;
using Prototipo.Common;
using Prototipo.Infrastructure;
using Prototipo.Services;
using Prototipo.ViewModels;

namespace Prototipo.Controllers
{
    [Autorizar]
    public class AccountController : ControladorBase
    {
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            if (SesionActual.Autenticado)
            {
                return RedirectToAction("Main", "Main");
            }

            var claveSeed = System.Configuration.ConfigurationManager.AppSettings["ContrasenaSeedDesarrollo"];
            if (!string.IsNullOrWhiteSpace(claveSeed))
            {
                new UsuarioService(Db).SembrarContrasenasDesarrollo(claveSeed);
            }

            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel modelo)
        {
            if (!ModelState.IsValid)
            {
                return View(modelo);
            }

            var servicio = new UsuarioService(Db);
            var resultado = servicio.Autenticar(modelo.NombreUsuario, modelo.Contrasena);

            if (!resultado.Exito)
            {
                ModelState.AddModelError(string.Empty, resultado.Mensaje);
                return View(modelo);
            }

            var usuario = servicio.Obtener(resultado.IdGenerado);
            Seguridad.IniciarSesion(usuario.IdUsuario, usuario.NombreUsuario, usuario.NombreCompleto, usuario.Rol);

            if (!string.IsNullOrEmpty(modelo.ReturnUrl) && Url.IsLocalUrl(modelo.ReturnUrl))
            {
                return Redirect(modelo.ReturnUrl);
            }

            return RedirectToAction("Main", "Main");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CerrarSesion()
        {
            new UsuarioService(Db).RegistrarCierreSesion();
            Seguridad.CerrarSesion();
            return RedirectToAction("Login");
        }

        [AllowAnonymous]
        public ActionResult AccesoDenegado()
        {
            return RedirectToAction("AccesoDenegado", "Error");
        }
    }
}
