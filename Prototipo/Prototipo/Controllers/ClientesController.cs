using System.Web.Mvc;
using Prototipo.Common;
using Prototipo.Infrastructure;
using Prototipo.Models;
using Prototipo.Services;

namespace Prototipo.Controllers
{
    [Autorizar(Roles = "Administrador,Recepcionista")]
    public class ClientesController : ControladorBase
    {
        private ClienteService Servicio
        {
            get { return new ClienteService(Db); }
        }

        public ActionResult Clientes(string estado, string busqueda)
        {
            ViewBag.Estado = estado;
            ViewBag.Busqueda = busqueda;
            return View(Servicio.Listar(estado, busqueda));
        }

        public ActionResult Index()
        {
            return RedirectToAction("Clientes");
        }

        public ActionResult DetalleCliente(int id)
        {
            var cliente = Servicio.Obtener(id);
            if (cliente == null)
            {
                MensajeError(Mensajes.NoEncontrado);
                return RedirectToAction("Clientes");
            }

            return View(cliente);
        }

        public ActionResult Crear()
        {
            return View(new Cliente { EstadoCliente = Estados.Cliente.Activo });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Crear(Cliente cliente)
        {
            if (!ModelState.IsValid)
            {
                return View(cliente);
            }

            var resultado = Servicio.Crear(cliente);
            if (!resultado.Exito)
            {
                ModelState.AddModelError(string.Empty, resultado.Mensaje);
                return View(cliente);
            }

            MensajeExito(resultado.Mensaje);
            return RedirectToAction("Clientes");
        }

        public ActionResult Editar(int id)
        {
            var cliente = Servicio.Obtener(id);
            if (cliente == null)
            {
                MensajeError(Mensajes.NoEncontrado);
                return RedirectToAction("Clientes");
            }

            return View(cliente);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(Cliente cliente)
        {
            if (!ModelState.IsValid)
            {
                return View(cliente);
            }

            var resultado = Servicio.Actualizar(cliente);
            if (!resultado.Exito)
            {
                ModelState.AddModelError(string.Empty, resultado.Mensaje);
                return View(cliente);
            }

            MensajeExito(resultado.Mensaje);
            return RedirectToAction("Clientes");
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

            return RedirectToAction("Clientes");
        }
    }
}
