using System.Web.Mvc;

namespace Prototipo.Controllers
{
    public class ClientesController : Controller
    {
        // GET: Clientes (Lista de clientes)
        public ActionResult Clientes()
        {
            return View();
        }

        // GET: Clientes/Crear
        public ActionResult Crear()
        {
            return View();
        }

        // GET: Clientes/Editar
        public ActionResult Editar()
        {
            return View();
        }

        // GET: Clientes/DetalleCliente
        public ActionResult DetalleCliente()
        {
            return View();
        }
    }
}
