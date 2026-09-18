using System;
using System.Web.Mvc;
using Prototipo.Common;
using Prototipo.Infrastructure;
using Prototipo.Models;
using Prototipo.Services;

namespace Prototipo.Controllers
{
    [Autorizar(Roles = "Administrador,Recepcionista")]
    public class EventosController : ControladorBase
    {
        private EventoService Servicio
        {
            get { return new EventoService(Db); }
        }

        private void CargarCatalogos(Evento evento)
        {
            ViewBag.Clientes = new SelectList(new ClienteService(Db).ListarActivos(),
                "IdCliente", "NombreMostrar", evento == null ? (object)null : evento.IdCliente);

            ViewBag.Espacios = new SelectList(Servicio.ListarEspacios(),
                "IdEspacio", "NombreEspacio", evento == null ? (object)null : evento.IdEspacio);

            ViewBag.EstadosEvento = Estados.Evento.Todos;
        }

        public ActionResult Eventos(string estado, DateTime? desde, DateTime? hasta, string busqueda)
        {
            ViewBag.Estado = estado;
            ViewBag.Desde = desde;
            ViewBag.Hasta = hasta;
            ViewBag.Busqueda = busqueda;
            return View(Servicio.Listar(estado, desde, hasta, busqueda));
        }

        public ActionResult Index()
        {
            return RedirectToAction("Eventos");
        }

        public ActionResult DetalleEvento(int id)
        {
            var evento = Servicio.Obtener(id);
            if (evento == null)
            {
                MensajeError(Mensajes.NoEncontrado);
                return RedirectToAction("Eventos");
            }

            return View(evento);
        }

        public ActionResult Crear()
        {
            var evento = new Evento
            {
                FechaEvento = DateTime.Today,
                HoraInicio = new TimeSpan(9, 0, 0),
                HoraFin = new TimeSpan(13, 0, 0),
                Participantes = 1,
                EstadoEvento = Estados.Evento.Programado
            };

            CargarCatalogos(evento);
            return View(evento);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Crear(Evento evento)
        {
            if (!ModelState.IsValid)
            {
                CargarCatalogos(evento);
                return View(evento);
            }

            var resultado = Servicio.Crear(evento);
            if (!resultado.Exito)
            {
                ModelState.AddModelError(string.Empty, resultado.Mensaje);
                CargarCatalogos(evento);
                return View(evento);
            }

            MensajeExito(resultado.Mensaje);
            return RedirectToAction("Eventos");
        }

        public ActionResult Editar(int id)
        {
            var evento = Servicio.Obtener(id);
            if (evento == null)
            {
                MensajeError(Mensajes.NoEncontrado);
                return RedirectToAction("Eventos");
            }

            CargarCatalogos(evento);
            return View(evento);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(Evento evento)
        {
            if (!ModelState.IsValid)
            {
                CargarCatalogos(evento);
                return View(evento);
            }

            var resultado = Servicio.Actualizar(evento);
            if (!resultado.Exito)
            {
                ModelState.AddModelError(string.Empty, resultado.Mensaje);
                CargarCatalogos(evento);
                return View(evento);
            }

            MensajeExito(resultado.Mensaje);
            return RedirectToAction("Eventos");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cancelar(int id)
        {
            var resultado = Servicio.Cancelar(id);
            if (resultado.Exito)
            {
                MensajeExito(resultado.Mensaje);
            }
            else
            {
                MensajeError(resultado.Mensaje);
            }

            return RedirectToAction("Eventos");
        }
    }
}
