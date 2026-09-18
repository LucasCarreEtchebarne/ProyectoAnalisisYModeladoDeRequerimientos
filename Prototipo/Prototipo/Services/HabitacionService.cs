using System;
using System.Collections.Generic;
using System.Linq;
using Prototipo.Common;
using Prototipo.Data;
using Prototipo.Models;

namespace Prototipo.Services
{
    public class HabitacionService : ServicioBase
    {
        private readonly BitacoraService _bitacora;

        public HabitacionService(HotelColibriContext db) : base(db)
        {
            _bitacora = new BitacoraService(db);
        }

        public List<Habitacion> Listar(string estado, string tipo)
        {
            var consulta = Db.Habitaciones.AsQueryable();

            if (!string.IsNullOrEmpty(estado))
            {
                consulta = consulta.Where(h => h.EstadoHabitacion == estado);
            }

            if (!string.IsNullOrEmpty(tipo))
            {
                consulta = consulta.Where(h => h.TipoHabitacion == tipo);
            }

            return consulta.OrderBy(h => h.NumeroHabitacion).ToList();
        }

        public Habitacion Obtener(int id)
        {
            return Db.Habitaciones.FirstOrDefault(h => h.IdHabitacion == id);
        }

        public List<string> ListarTipos()
        {
            return Db.Habitaciones.Select(h => h.TipoHabitacion).Distinct().OrderBy(t => t).ToList();
        }

        public List<Habitacion> ConsultarDisponibles(DateTime entrada, DateTime salida, int huespedes)
        {
            var ocupadas = Db.Reservas
                .Where(r => r.EstadoReserva != Estados.Reserva.Cancelada
                         && entrada < r.FechaSalida && salida > r.FechaEntrada)
                .Select(r => r.IdHabitacion);

            return Db.Habitaciones
                .Where(h => h.EstadoHabitacion != Estados.Habitacion.Mantenimiento
                         && h.Capacidad >= huespedes
                         && !ocupadas.Contains(h.IdHabitacion))
                .OrderBy(h => h.NumeroHabitacion)
                .ToList();
        }

        public ResultadoOperacion Crear(Habitacion habitacion)
        {
            if (Db.Habitaciones.Any(h => h.NumeroHabitacion == habitacion.NumeroHabitacion))
            {
                return ResultadoOperacion.Error(Mensajes.HabitacionDuplicada);
            }

            if (string.IsNullOrEmpty(habitacion.EstadoHabitacion))
            {
                habitacion.EstadoHabitacion = Estados.Habitacion.Disponible;
            }

            Db.Habitaciones.Add(habitacion);
            Db.SaveChanges();

            _bitacora.Registrar(Estados.Modulos.Hospedaje, Estados.Acciones.Crear, habitacion.IdHabitacion,
                "Habitacion creada: " + habitacion.NumeroHabitacion);
            Db.SaveChanges();

            return ResultadoOperacion.Ok(Mensajes.CreadoOk, habitacion.IdHabitacion);
        }

        public ResultadoOperacion Actualizar(Habitacion datos)
        {
            var habitacion = Obtener(datos.IdHabitacion);
            if (habitacion == null)
            {
                return ResultadoOperacion.Error(Mensajes.NoEncontrado);
            }

            if (Db.Habitaciones.Any(h => h.IdHabitacion != datos.IdHabitacion
                                      && h.NumeroHabitacion == datos.NumeroHabitacion))
            {
                return ResultadoOperacion.Error(Mensajes.HabitacionDuplicada);
            }

            habitacion.NumeroHabitacion = datos.NumeroHabitacion;
            habitacion.TipoHabitacion = datos.TipoHabitacion;
            habitacion.Capacidad = datos.Capacidad;
            habitacion.Precio = datos.Precio;
            habitacion.EstadoHabitacion = datos.EstadoHabitacion;
            habitacion.Piso = datos.Piso;
            habitacion.Descripcion = datos.Descripcion;

            _bitacora.Registrar(Estados.Modulos.Hospedaje, Estados.Acciones.Editar, habitacion.IdHabitacion,
                "Habitacion actualizada: " + habitacion.NumeroHabitacion);
            Db.SaveChanges();

            return ResultadoOperacion.Ok(Mensajes.ActualizadoOk, habitacion.IdHabitacion);
        }

        public ResultadoOperacion EnviarAMantenimiento(int id)
        {
            var habitacion = Obtener(id);
            if (habitacion == null)
            {
                return ResultadoOperacion.Error(Mensajes.NoEncontrado);
            }

            habitacion.EstadoHabitacion = Estados.Habitacion.Mantenimiento;

            _bitacora.Registrar(Estados.Modulos.Hospedaje, Estados.Acciones.Inactivar, habitacion.IdHabitacion,
                "Habitacion enviada a mantenimiento: " + habitacion.NumeroHabitacion);
            Db.SaveChanges();

            return ResultadoOperacion.Ok("La habitacion paso a mantenimiento.", habitacion.IdHabitacion);
        }
    }
}
