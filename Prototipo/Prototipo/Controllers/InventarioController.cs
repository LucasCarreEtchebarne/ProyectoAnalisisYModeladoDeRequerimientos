using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Prototipo.Controllers
{
    public class InventarioController : Controller
    {
        // GET: Inventario
        public ActionResult Inventario()
        {
            return View();
        }

        // GET: Inventario/Crear
        public ActionResult Crear()
        {
            return View();
        }

        // GET: Inventario/Editar
        public ActionResult Editar()
        {
            return View();
        }

        // GET: Inventario/DetalleProducto
        public ActionResult DetalleProducto()
        {
            return View();
        }

        // GET: Inventario/Alertas
        public ActionResult Alertas()
        {
            return View();
        }
    }
}
