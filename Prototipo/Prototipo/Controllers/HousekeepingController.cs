using System.Linq;
using System.Web.Mvc;
using Prototipo.Common;
using Prototipo.Infrastructure;
using Prototipo.Models;
using Prototipo.Services;

namespace Prototipo.Controllers
{
    [Autorizar(Roles = "Administrador,Housekeeping,Recepcionista")]
    public class HousekeepingController : ControladorBase
    {
        private HousekeepingService Servicio
        {
            get { return new HousekeepingService(Db); }
        }

        private void CargarCatalogos(TareaHousekeeping tarea)
        {
            var habitaciones = new HabitacionService(Db).Listar(null, null)
                .Select(h => new { h.IdHabitacion, Descripcion = h.NumeroHabitacion + " - " + h.TipoHabitacion })
                .ToList();

            ViewBag.Habitaciones = new SelectList(habitaciones, "IdHabitacion", "Descripcion",
                tarea == null ? (object)null : tarea.IdHabitacion);

            var personal = Db.Usuarios
                .Where(u => u.Estado == Estados.Usuario.Activo)
                .OrderBy(u => u.NombreCompleto)
                .ToList();

            ViewBag.Usuarios = new SelectList(personal, "IdUsuario", "NombreCompleto",
                tarea == null ? (object)null : tarea.IdUsuario);

            ViewBag.EstadosTarea = Estados.Tarea.Todos;
        }

        public ActionResult Housekeeping(string estado, int? idUsuario)
        {
            ViewBag.Estado = estado;
            ViewBag.IdUsuario = idUsuario;
            return View(Servicio.Listar(estado, idUsuario));
        }

        public ActionResult Index()
        {
            return RedirectToAction("Housekeeping");
        }

        public ActionResult DetalleTarea(int id)
        {
            var tarea = Servicio.Obtener(id);
            if (tarea == null)
            {
                MensajeError(Mensajes.NoEncontrado);
                return RedirectToAction("Housekeeping");
            }

            ViewBag.EstadosTarea = Estados.Tarea.Todos;
            return View(tarea);
        }

        public ActionResult Crear()
        {
            var tarea = new TareaHousekeeping { EstadoTarea = Estados.Tarea.Pendiente };
            CargarCatalogos(tarea);
            return View(tarea);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Crear(TareaHousekeeping tarea)
        {
            if (!ModelState.IsValid)
            {
                CargarCatalogos(tarea);
                return View(tarea);
            }

            var resultado = Servicio.Crear(tarea);
            if (!resultado.Exito)
            {
                ModelState.AddModelError(string.Empty, resultado.Mensaje);
                CargarCatalogos(tarea);
                return View(tarea);
            }

            MensajeExito(resultado.Mensaje);
            return RedirectToAction("Housekeeping");
        }

        public ActionResult Editar(int id)
        {
            var tarea = Servicio.Obtener(id);
            if (tarea == null)
            {
                MensajeError(Mensajes.NoEncontrado);
                return RedirectToAction("Housekeeping");
            }

            CargarCatalogos(tarea);
            return View(tarea);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(TareaHousekeeping tarea)
        {
            if (!ModelState.IsValid)
            {
                CargarCatalogos(tarea);
                return View(tarea);
            }

            var resultado = Servicio.Actualizar(tarea);
            if (!resultado.Exito)
            {
                ModelState.AddModelError(string.Empty, resultado.Mensaje);
                CargarCatalogos(tarea);
                return View(tarea);
            }

            MensajeExito(resultado.Mensaje);
            return RedirectToAction("Housekeeping");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CambiarEstado(int id, string estado)
        {
            var resultado = Servicio.CambiarEstado(id, estado);
            if (resultado.Exito)
            {
                MensajeExito(resultado.Mensaje);
            }
            else
            {
                MensajeError(resultado.Mensaje);
            }

            return RedirectToAction("DetalleTarea", new { id });
        }
    }
}
