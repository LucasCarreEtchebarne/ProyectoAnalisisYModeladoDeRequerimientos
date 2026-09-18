using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Prototipo.Common;
using Prototipo.Data;
using Prototipo.Models;

namespace Prototipo.Services
{
    public class EventoService : ServicioBase
    {
        private readonly BitacoraService _bitacora;

        public EventoService(HotelColibriContext db) : base(db)
        {
            _bitacora = new BitacoraService(db);
        }

        public List<Evento> Listar(string estado, DateTime? desde, DateTime? hasta, string busqueda)
        {
            var consulta = Db.Eventos
                .Include(e => e.Cliente)
                .Include(e => e.Espacio)
                .AsQueryable();

            if (!string.IsNullOrEmpty(estado))
            {
                consulta = consulta.Where(e => e.EstadoEvento == estado);
            }

            if (desde.HasValue)
            {
                consulta = consulta.Where(e => e.FechaEvento >= desde.Value);
            }

            if (hasta.HasValue)
            {
                consulta = consulta.Where(e => e.FechaEvento <= hasta.Value);
            }

            if (!string.IsNullOrWhiteSpace(busqueda))
            {
                consulta = consulta.Where(e => e.NombreEvento.Contains(busqueda)
                                            || e.Cliente.NombreCompleto.Contains(busqueda)
                                            || e.Cliente.PrimerApellido.Contains(busqueda));
            }

            return consulta.OrderByDescending(e => e.FechaEvento).ToList();
        }

        public Evento Obtener(int id)
        {
            return Db.Eventos
                .Include(e => e.Cliente)
                .Include(e => e.Espacio)
                .FirstOrDefault(e => e.IdEvento == id);
        }

        public List<EspacioEvento> ListarEspacios()
        {
            return Db.EspaciosEvento
                .Where(e => e.EstadoEspacio == Estados.Espacio.Activo)
                .OrderBy(e => e.NombreEspacio)
                .ToList();
        }

        public bool HayTraslape(int idEspacio, DateTime fecha, TimeSpan inicio, TimeSpan fin, int idEventoExcluir = 0)
        {
            return Db.Eventos.Any(e => e.IdEspacio == idEspacio
                                    && e.FechaEvento == fecha
                                    && e.IdEvento != idEventoExcluir
                                    && e.EstadoEvento != Estados.Evento.Cancelado
                                    && inicio < e.HoraFin
                                    && fin > e.HoraInicio);
        }

        private ResultadoOperacion Validar(Evento evento, int idExcluir)
        {
            if (evento.HoraFin <= evento.HoraInicio)
            {
                return ResultadoOperacion.Error(Mensajes.EventoHorasInvalidas);
            }

            var espacio = Db.EspaciosEvento.FirstOrDefault(e => e.IdEspacio == evento.IdEspacio);
            if (espacio == null)
            {
                return ResultadoOperacion.Error(Mensajes.NoEncontrado);
            }

            if (evento.Participantes > espacio.CapacidadMaxima)
            {
                return ResultadoOperacion.Error(Mensajes.EventoCapacidadExcedida +
                    " Capacidad de " + espacio.NombreEspacio + ": " + espacio.CapacidadMaxima + ".");
            }

            if (HayTraslape(evento.IdEspacio, evento.FechaEvento, evento.HoraInicio, evento.HoraFin, idExcluir))
            {
                return ResultadoOperacion.Error(Mensajes.EventoTraslapado);
            }

            return ResultadoOperacion.Ok(string.Empty);
        }

        public ResultadoOperacion Crear(Evento evento)
        {
            var validacion = Validar(evento, 0);
            if (!validacion.Exito)
            {
                return validacion;
            }

            if (string.IsNullOrEmpty(evento.EstadoEvento))
            {
                evento.EstadoEvento = Estados.Evento.Programado;
            }

            Db.Eventos.Add(evento);
            Db.SaveChanges();

            _bitacora.Registrar(Estados.Modulos.Clientes, Estados.Acciones.Crear, evento.IdEvento,
                "Evento creado: " + evento.NombreEvento);
            Db.SaveChanges();

            return ResultadoOperacion.Ok(Mensajes.CreadoOk, evento.IdEvento);
        }

        public ResultadoOperacion Actualizar(Evento datos)
        {
            var evento = Db.Eventos.FirstOrDefault(e => e.IdEvento == datos.IdEvento);
            if (evento == null)
            {
                return ResultadoOperacion.Error(Mensajes.NoEncontrado);
            }

            var validacion = Validar(datos, datos.IdEvento);
            if (!validacion.Exito)
            {
                return validacion;
            }

            evento.IdCliente = datos.IdCliente;
            evento.IdEspacio = datos.IdEspacio;
            evento.NombreEvento = datos.NombreEvento;
            evento.FechaEvento = datos.FechaEvento;
            evento.HoraInicio = datos.HoraInicio;
            evento.HoraFin = datos.HoraFin;
            evento.Participantes = datos.Participantes;
            evento.MontoAcordado = datos.MontoAcordado;
            evento.EstadoEvento = datos.EstadoEvento;
            evento.Descripcion = datos.Descripcion;

            _bitacora.Registrar(Estados.Modulos.Clientes, Estados.Acciones.Editar, evento.IdEvento,
                "Evento actualizado: " + evento.NombreEvento);
            Db.SaveChanges();

            return ResultadoOperacion.Ok(Mensajes.ActualizadoOk, evento.IdEvento);
        }

        public ResultadoOperacion Cancelar(int idEvento)
        {
            var evento = Obtener(idEvento);
            if (evento == null)
            {
                return ResultadoOperacion.Error(Mensajes.NoEncontrado);
            }

            evento.EstadoEvento = Estados.Evento.Cancelado;

            _bitacora.Registrar(Estados.Modulos.Clientes, Estados.Acciones.Anular, evento.IdEvento,
                "Evento cancelado: " + evento.NombreEvento);
            Db.SaveChanges();

            return ResultadoOperacion.Ok("El evento fue cancelado.", evento.IdEvento);
        }
    }
}
