using System;
using System.Linq;
using System.Web.Mvc;
using Prototipo.Common;
using Prototipo.Infrastructure;
using Prototipo.Services;

namespace Prototipo.Controllers
{
    [Autorizar(Roles = "Administrador")]
    public class BitacoraController : ControladorBase
    {
        private BitacoraService Servicio
        {
            get { return new BitacoraService(Db); }
        }

        public ActionResult Bitacora(string modulo, DateTime? desde, DateTime? hasta, int? idUsuario)
        {
            ViewBag.ModuloFiltro = modulo;
            ViewBag.Desde = desde;
            ViewBag.Hasta = hasta;
            ViewBag.IdUsuario = idUsuario;
            ViewBag.Modulos = Estados.Modulos.Nombres;
            ViewBag.Usuarios = Db.Usuarios.OrderBy(u => u.NombreCompleto).ToList();

            return View(Servicio.Listar(modulo, desde, hasta, idUsuario));
        }

        public ActionResult Index()
        {
            return RedirectToAction("Bitacora");
        }

        public ActionResult DetalleBitacora(long id)
        {
            var registro = Servicio.Obtener(id);
            if (registro == null)
            {
                MensajeError(Mensajes.NoEncontrado);
                return RedirectToAction("Bitacora");
            }

            return View(registro);
        }
    }
}
