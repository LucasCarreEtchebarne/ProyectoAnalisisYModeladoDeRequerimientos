using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Prototipo.Controllers
{
    public class EventosController : Controller
    {
        // GET: Eventos
        public ActionResult Eventos()
        {
            return View();
        }

        // GET: Eventos/Crear
        public ActionResult Crear()
        {
            return View();
        }

        // GET: Eventos/Editar
        public ActionResult Editar()
        {
            return View();
        }

        // GET: Eventos/DetalleEvento
        public ActionResult DetalleEvento()
        {
            return View();
        }
    }
}
