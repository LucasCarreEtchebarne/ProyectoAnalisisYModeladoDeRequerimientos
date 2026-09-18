using System;
using System.Web.Mvc;
using Prototipo.Common;
using Prototipo.Infrastructure;
using Prototipo.Models;
using Prototipo.Services;

namespace Prototipo.Controllers
{
    [Autorizar(Roles = "Administrador,Recepcionista")]
    public class HabitacionesController : ControladorBase
    {
        private HabitacionService Servicio
        {
            get { return new HabitacionService(Db); }
        }

        public ActionResult Habitaciones(string estado, string tipo)
        {
            ViewBag.Estado = estado;
            ViewBag.Tipo = tipo;
            ViewBag.Tipos = Servicio.ListarTipos();
            return View(Servicio.Listar(estado, tipo));
        }

        public ActionResult Index()
        {
            return RedirectToAction("Habitaciones");
        }

        public ActionResult Consultar(DateTime? entrada, DateTime? salida, int huespedes = 1)
        {
            ViewBag.Entrada = entrada ?? DateTime.Today;
            ViewBag.Salida = salida ?? DateTime.Today.AddDays(1);
            ViewBag.Huespedes = huespedes;

            if (entrada.HasValue && salida.HasValue)
            {
                if (salida.Value <= entrada.Value)
                {
                    MensajeError(Mensajes.FechasInvalidas);
                    return View(new System.Collections.Generic.List<Habitacion>());
                }

                ViewBag.Consultado = true;
                return View(Servicio.ConsultarDisponibles(entrada.Value, salida.Value, huespedes));
            }

            return View(new System.Collections.Generic.List<Habitacion>());
        }

        public ActionResult DetalleHabitacion(int id)
        {
            var habitacion = Servicio.Obtener(id);
            if (habitacion == null)
            {
                MensajeError(Mensajes.NoEncontrado);
                return RedirectToAction("Habitaciones");
            }

            return View(habitacion);
        }

        public ActionResult Crear()
        {
            return View(new Habitacion { EstadoHabitacion = Estados.Habitacion.Disponible, Capacidad = 2, Piso = 1 });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Crear(Habitacion habitacion)
        {
            if (!ModelState.IsValid)
            {
                return View(habitacion);
            }

            var resultado = Servicio.Crear(habitacion);
            if (!resultado.Exito)
            {
                ModelState.AddModelError(string.Empty, resultado.Mensaje);
                return View(habitacion);
            }

            MensajeExito(resultado.Mensaje);
            return RedirectToAction("Habitaciones");
        }

        public ActionResult Editar(int id)
        {
            var habitacion = Servicio.Obtener(id);
            if (habitacion == null)
            {
                MensajeError(Mensajes.NoEncontrado);
                return RedirectToAction("Habitaciones");
            }

            return View(habitacion);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(Habitacion habitacion)
        {
            if (!ModelState.IsValid)
            {
                return View(habitacion);
            }

            var resultado = Servicio.Actualizar(habitacion);
            if (!resultado.Exito)
            {
                ModelState.AddModelError(string.Empty, resultado.Mensaje);
                return View(habitacion);
            }

            MensajeExito(resultado.Mensaje);
            return RedirectToAction("Habitaciones");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Mantenimiento(int id)
        {
            var resultado = Servicio.EnviarAMantenimiento(id);
            if (resultado.Exito)
            {
                MensajeExito(resultado.Mensaje);
            }
            else
            {
                MensajeError(resultado.Mensaje);
            }

            return RedirectToAction("Habitaciones");
        }
    }
}
