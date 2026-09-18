using System.Web.Mvc;
using Prototipo.Common;
using Prototipo.Infrastructure;
using Prototipo.Models;
using Prototipo.Services;

namespace Prototipo.Controllers
{
    [Autorizar(Roles = "Administrador,Mesero")]
    public class InventarioController : ControladorBase
    {
        private InventarioService Servicio
        {
            get { return new InventarioService(Db); }
        }

        public ActionResult Inventario(string estado, string categoria, string busqueda)
        {
            ViewBag.Estado = estado;
            ViewBag.Categoria = categoria;
            ViewBag.Busqueda = busqueda;
            ViewBag.Categorias = Servicio.ListarCategorias();
            return View(Servicio.Listar(estado, categoria, busqueda));
        }

        public ActionResult Index()
        {
            return RedirectToAction("Inventario");
        }

        public ActionResult Alertas()
        {
            return View(Servicio.ListarAlertas());
        }

        public ActionResult DetalleProducto(int id)
        {
            var producto = Servicio.Obtener(id);
            if (producto == null)
            {
                MensajeError(Mensajes.NoEncontrado);
                return RedirectToAction("Inventario");
            }

            ViewBag.Movimientos = Servicio.ListarMovimientos(id);
            return View(producto);
        }

        public ActionResult Crear()
        {
            return View(new Models.Inventario
            {
                EstadoProducto = Estados.Producto.Activo,
                UnidadMedida = "Unidad"
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Crear(Models.Inventario producto)
        {
            if (!ModelState.IsValid)
            {
                return View(producto);
            }

            var resultado = Servicio.Crear(producto);
            if (!resultado.Exito)
            {
                ModelState.AddModelError(string.Empty, resultado.Mensaje);
                return View(producto);
            }

            MensajeExito(resultado.Mensaje);
            return RedirectToAction("Inventario");
        }

        public ActionResult Editar(int id)
        {
            var producto = Servicio.Obtener(id);
            if (producto == null)
            {
                MensajeError(Mensajes.NoEncontrado);
                return RedirectToAction("Inventario");
            }

            return View(producto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(Models.Inventario producto)
        {
            if (!ModelState.IsValid)
            {
                return View(producto);
            }

            var resultado = Servicio.Actualizar(producto);
            if (!resultado.Exito)
            {
                ModelState.AddModelError(string.Empty, resultado.Mensaje);
                return View(producto);
            }

            MensajeExito(resultado.Mensaje);
            return RedirectToAction("Inventario");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RegistrarEntrada(int id, decimal cantidad, string motivo)
        {
            var resultado = Servicio.RegistrarEntrada(id, cantidad, motivo);
            if (resultado.Exito)
            {
                MensajeExito(resultado.Mensaje);
            }
            else
            {
                MensajeError(resultado.Mensaje);
            }

            return RedirectToAction("DetalleProducto", new { id });
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

            return RedirectToAction("Inventario");
        }
    }
}
