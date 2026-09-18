using System;
using System.Linq;
using System.Web.Mvc;
using Prototipo.Common;
using Prototipo.Infrastructure;
using Prototipo.Models;
using Prototipo.Services;

namespace Prototipo.Controllers
{
    [Autorizar(Roles = "Administrador,Recepcionista")]
    public class ReservasController : ControladorBase
    {
        private ReservaService Servicio
        {
            get { return new ReservaService(Db); }
        }

        private void CargarCatalogos(Reserva reserva)
        {
            ViewBag.Clientes = new SelectList(new ClienteService(Db).ListarActivos(),
                "IdCliente", "NombreMostrar", reserva == null ? (object)null : reserva.IdCliente);

            var habitaciones = new HabitacionService(Db).Listar(null, null)
                .Select(h => new
                {
                    h.IdHabitacion,
                    Descripcion = h.NumeroHabitacion + " - " + h.TipoHabitacion +
                                  " (" + h.Capacidad + " pax, " + Formato.Colones(h.Precio) + ")"
                })
                .ToList();

            ViewBag.Habitaciones = new SelectList(habitaciones, "IdHabitacion", "Descripcion",
                reserva == null ? (object)null : reserva.IdHabitacion);

            ViewBag.EstadosReserva = Estados.Reserva.Todos;
        }

        public ActionResult Reservas(string estado, DateTime? desde, DateTime? hasta, string busqueda)
        {
            ViewBag.Estado = estado;
            ViewBag.Desde = desde;
            ViewBag.Hasta = hasta;
            ViewBag.Busqueda = busqueda;
            return View(Servicio.Listar(estado, desde, hasta, busqueda));
        }

        public ActionResult Index()
        {
            return RedirectToAction("Reservas");
        }

        public ActionResult DetalleReserva(int id)
        {
            var reserva = Servicio.Obtener(id);
            if (reserva == null)
            {
                MensajeError(Mensajes.NoEncontrado);
                return RedirectToAction("Reservas");
            }

            return View(reserva);
        }

        public ActionResult Crear()
        {
            var reserva = new Reserva
            {
                FechaEntrada = DateTime.Today,
                FechaSalida = DateTime.Today.AddDays(1),
                CantidadHuespedes = 1,
                EstadoReserva = Estados.Reserva.Pendiente
            };

            CargarCatalogos(reserva);
            return View(reserva);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Crear(Reserva reserva)
        {
            if (!ModelState.IsValid)
            {
                CargarCatalogos(reserva);
                return View(reserva);
            }

            var resultado = Servicio.Crear(reserva);
            if (!resultado.Exito)
            {
                ModelState.AddModelError(string.Empty, resultado.Mensaje);
                CargarCatalogos(reserva);
                return View(reserva);
            }

            MensajeExito(resultado.Mensaje);
            return RedirectToAction("Reservas");
        }

        public ActionResult Editar(int id)
        {
            var reserva = Servicio.Obtener(id);
            if (reserva == null)
            {
                MensajeError(Mensajes.NoEncontrado);
                return RedirectToAction("Reservas");
            }

            CargarCatalogos(reserva);
            return View(reserva);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(Reserva reserva)
        {
            if (!ModelState.IsValid)
            {
                CargarCatalogos(reserva);
                return View(reserva);
            }

            var resultado = Servicio.Actualizar(reserva);
            if (!resultado.Exito)
            {
                ModelState.AddModelError(string.Empty, resultado.Mensaje);
                CargarCatalogos(reserva);
                return View(reserva);
            }

            MensajeExito(resultado.Mensaje);
            return RedirectToAction("Reservas");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CheckIn(int id)
        {
            var resultado = Servicio.CheckIn(id);
            Notificar(resultado);
            return RedirectToAction("DetalleReserva", new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CheckOut(int id)
        {
            var encargado = Db.Usuarios
                .FirstOrDefault(u => u.Rol == Estados.Roles.Housekeeping && u.Estado == Estados.Usuario.Activo);

            var idEncargado = encargado != null ? encargado.IdUsuario : SesionActual.IdUsuario;

            var resultado = Servicio.CheckOut(id, idEncargado);
            Notificar(resultado);
            return RedirectToAction("DetalleReserva", new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cancelar(int id)
        {
            var resultado = Servicio.Cancelar(id);
            Notificar(resultado);
            return RedirectToAction("Reservas");
        }

        private void Notificar(ResultadoOperacion resultado)
        {
            if (resultado.Exito)
            {
                MensajeExito(resultado.Mensaje);
            }
            else
            {
                MensajeError(resultado.Mensaje);
            }
        }
    }
}
