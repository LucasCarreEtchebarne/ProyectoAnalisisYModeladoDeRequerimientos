using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Prototipo.Common;
using Prototipo.Data;
using Prototipo.Models;

namespace Prototipo.Services
{
    public class HousekeepingService : ServicioBase
    {
        private readonly BitacoraService _bitacora;

        public HousekeepingService(HotelColibriContext db) : base(db)
        {
            _bitacora = new BitacoraService(db);
        }

        public List<TareaHousekeeping> Listar(string estado, int? idUsuario)
        {
            var consulta = Db.Housekeeping
                .Include(t => t.Habitacion)
                .Include(t => t.Usuario)
                .AsQueryable();

            if (!string.IsNullOrEmpty(estado))
            {
                consulta = consulta.Where(t => t.EstadoTarea == estado);
            }

            if (idUsuario.HasValue && idUsuario.Value > 0)
            {
                consulta = consulta.Where(t => t.IdUsuario == idUsuario.Value);
            }

            return consulta
                .OrderBy(t => t.EstadoTarea)
                .ThenBy(t => t.FechaLimite)
                .ToList();
        }

        public TareaHousekeeping Obtener(int id)
        {
            return Db.Housekeeping
                .Include(t => t.Habitacion)
                .Include(t => t.Usuario)
                .FirstOrDefault(t => t.IdTarea == id);
        }

        public ResultadoOperacion Crear(TareaHousekeeping tarea)
        {
            tarea.FechaAsignacion = DateTime.Now;
            if (string.IsNullOrEmpty(tarea.EstadoTarea))
            {
                tarea.EstadoTarea = Estados.Tarea.Pendiente;
            }

            Db.Housekeeping.Add(tarea);
            Db.SaveChanges();

            _bitacora.Registrar(Estados.Modulos.Housekeeping, Estados.Acciones.Crear, tarea.IdTarea,
                "Tarea de housekeeping creada");
            Db.SaveChanges();

            return ResultadoOperacion.Ok(Mensajes.CreadoOk, tarea.IdTarea);
        }

        public ResultadoOperacion Actualizar(TareaHousekeeping datos)
        {
            var tarea = Obtener(datos.IdTarea);
            if (tarea == null)
            {
                return ResultadoOperacion.Error(Mensajes.NoEncontrado);
            }

            tarea.IdHabitacion = datos.IdHabitacion;
            tarea.IdUsuario = datos.IdUsuario;
            tarea.TipoTarea = datos.TipoTarea;
            tarea.FechaLimite = datos.FechaLimite;
            tarea.Observaciones = datos.Observaciones;

            var resultado = AplicarEstado(tarea, datos.EstadoTarea);
            if (!resultado.Exito)
            {
                return resultado;
            }

            _bitacora.Registrar(Estados.Modulos.Housekeeping, Estados.Acciones.Editar, tarea.IdTarea,
                "Tarea de housekeeping actualizada");
            Db.SaveChanges();

            return ResultadoOperacion.Ok(Mensajes.ActualizadoOk, tarea.IdTarea);
        }

        public ResultadoOperacion CambiarEstado(int idTarea, string estado)
        {
            var tarea = Obtener(idTarea);
            if (tarea == null)
            {
                return ResultadoOperacion.Error(Mensajes.NoEncontrado);
            }

            var resultado = AplicarEstado(tarea, estado);
            if (!resultado.Exito)
            {
                return resultado;
            }

            _bitacora.Registrar(Estados.Modulos.Housekeeping, Estados.Acciones.Editar, tarea.IdTarea,
                "Tarea de housekeeping en estado " + estado);
            Db.SaveChanges();

            return ResultadoOperacion.Ok("La tarea quedo en estado " + estado + ".", tarea.IdTarea);
        }

        private ResultadoOperacion AplicarEstado(TareaHousekeeping tarea, string estado)
        {
            if (!Estados.Tarea.Todos.Contains(estado))
            {
                return ResultadoOperacion.Error("El estado indicado no es valido.");
            }

            tarea.EstadoTarea = estado;

            if (estado == Estados.Tarea.Completada)
            {
                tarea.FechaCompletada = tarea.FechaCompletada ?? DateTime.Now;

                var habitacion = Db.Habitaciones.First(h => h.IdHabitacion == tarea.IdHabitacion);
                if (habitacion.EstadoHabitacion == Estados.Habitacion.Limpieza)
                {
                    habitacion.EstadoHabitacion = Estados.Habitacion.Disponible;
                }
            }
            else
            {
                tarea.FechaCompletada = null;
            }

            return ResultadoOperacion.Ok(string.Empty);
        }
    }
}
