using System.Web.Mvc;
using Prototipo.Common;
using Prototipo.Infrastructure;
using Prototipo.Services;

namespace Prototipo.Controllers
{
    [Autorizar(Roles = "Administrador,Mesero")]
    public class MesasController : ControladorBase
    {
        private MesaService Servicio
        {
            get { return new MesaService(Db); }
        }

        public ActionResult Mesas(string estado)
        {
            ViewBag.Estado = estado;
            return View(Servicio.Listar(estado));
        }

        public ActionResult Index()
        {
            return RedirectToAction("Mesas");
        }

        public ActionResult DetalleMesa(int id)
        {
            var mesa = Servicio.Obtener(id);
            if (mesa == null)
            {
                MensajeError(Mensajes.NoEncontrado);
                return RedirectToAction("Mesas");
            }

            ViewBag.Pedidos = Servicio.PedidosActivos(id);
            ViewBag.Estados = Estados.Mesa.Todos;
            return View(mesa);
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

            return RedirectToAction("DetalleMesa", new { id });
        }
    }
}
