using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Prototipo.Common;
using Prototipo.Data;
using Prototipo.Models;

namespace Prototipo.Services
{
    public class ReservaService : ServicioBase
    {
        private readonly BitacoraService _bitacora;

        public ReservaService(HotelColibriContext db) : base(db)
        {
            _bitacora = new BitacoraService(db);
        }

        public List<Reserva> Listar(string estado, DateTime? desde, DateTime? hasta, string busqueda)
        {
            var consulta = Db.Reservas
                .Include(r => r.Cliente)
                .Include(r => r.Habitacion)
                .AsQueryable();

            if (!string.IsNullOrEmpty(estado))
            {
                consulta = consulta.Where(r => r.EstadoReserva == estado);
            }

            if (desde.HasValue)
            {
                consulta = consulta.Where(r => r.FechaSalida >= desde.Value);
            }

            if (hasta.HasValue)
            {
                consulta = consulta.Where(r => r.FechaEntrada <= hasta.Value);
            }

            if (!string.IsNullOrWhiteSpace(busqueda))
            {
                consulta = consulta.Where(r => r.Cliente.NombreCompleto.Contains(busqueda)
                                            || r.Cliente.PrimerApellido.Contains(busqueda)
                                            || r.Cliente.Identificacion.Contains(busqueda)
                                            || r.Habitacion.NumeroHabitacion.Contains(busqueda));
            }

            return consulta.OrderByDescending(r => r.FechaEntrada).ToList();
        }

        public Reserva Obtener(int id)
        {
            return Db.Reservas
                .Include(r => r.Cliente)
                .Include(r => r.Habitacion)
                .FirstOrDefault(r => r.IdReserva == id);
        }

        public bool HayTraslape(int idHabitacion, DateTime entrada, DateTime salida, int idReservaExcluir = 0)
        {
            return Db.Reservas.Any(r => r.IdHabitacion == idHabitacion
                                     && r.IdReserva != idReservaExcluir
                                     && r.EstadoReserva != Estados.Reserva.Cancelada
                                     && entrada < r.FechaSalida
                                     && salida > r.FechaEntrada);
        }

        private ResultadoOperacion Validar(Reserva reserva, int idReservaExcluir)
        {
            if (reserva.FechaSalida <= reserva.FechaEntrada)
            {
                return ResultadoOperacion.Error(Mensajes.FechasInvalidas);
            }

            var habitacion = Db.Habitaciones.FirstOrDefault(h => h.IdHabitacion == reserva.IdHabitacion);
            if (habitacion == null)
            {
                return ResultadoOperacion.Error(Mensajes.NoEncontrado);
            }

            if (reserva.CantidadHuespedes > habitacion.Capacidad)
            {
                return ResultadoOperacion.Error(Mensajes.CapacidadExcedida);
            }

            if (HayTraslape(reserva.IdHabitacion, reserva.FechaEntrada, reserva.FechaSalida, idReservaExcluir))
            {
                return ResultadoOperacion.Error(Mensajes.HabitacionNoDisponible);
            }

            return ResultadoOperacion.Ok(string.Empty);
        }

        public ResultadoOperacion Crear(Reserva reserva)
        {
            var validacion = Validar(reserva, 0);
            if (!validacion.Exito)
            {
                return validacion;
            }

            var habitacion = Db.Habitaciones.First(h => h.IdHabitacion == reserva.IdHabitacion);
            reserva.PrecioNoche = habitacion.Precio;
            reserva.FechaRegistro = DateTime.Now;

            if (string.IsNullOrEmpty(reserva.EstadoReserva))
            {
                reserva.EstadoReserva = Estados.Reserva.Pendiente;
            }

            Db.Reservas.Add(reserva);
            Db.SaveChanges();

            _bitacora.Registrar(Estados.Modulos.Hospedaje, Estados.Acciones.Crear, reserva.IdReserva,
                "Reserva creada para la habitacion " + habitacion.NumeroHabitacion);
            Db.SaveChanges();

            return ResultadoOperacion.Ok(Mensajes.CreadoOk, reserva.IdReserva);
        }

        public ResultadoOperacion Actualizar(Reserva datos)
        {
            var reserva = Db.Reservas.FirstOrDefault(r => r.IdReserva == datos.IdReserva);
            if (reserva == null)
            {
                return ResultadoOperacion.Error(Mensajes.NoEncontrado);
            }

            var validacion = Validar(datos, datos.IdReserva);
            if (!validacion.Exito)
            {
                return validacion;
            }

            if (reserva.IdHabitacion != datos.IdHabitacion)
            {
                reserva.PrecioNoche = Db.Habitaciones.First(h => h.IdHabitacion == datos.IdHabitacion).Precio;
            }

            reserva.IdCliente = datos.IdCliente;
            reserva.IdHabitacion = datos.IdHabitacion;
            reserva.FechaEntrada = datos.FechaEntrada;
            reserva.FechaSalida = datos.FechaSalida;
            reserva.CantidadHuespedes = datos.CantidadHuespedes;
            reserva.EstadoReserva = datos.EstadoReserva;
            reserva.Observaciones = datos.Observaciones;

            _bitacora.Registrar(Estados.Modulos.Hospedaje, Estados.Acciones.Editar, reserva.IdReserva,
                "Reserva actualizada");
            Db.SaveChanges();

            return ResultadoOperacion.Ok(Mensajes.ActualizadoOk, reserva.IdReserva);
        }

        public ResultadoOperacion CheckIn(int idReserva)
        {
            var reserva = Obtener(idReserva);
            if (reserva == null)
            {
                return ResultadoOperacion.Error(Mensajes.NoEncontrado);
            }

            if (reserva.EstadoReserva != Estados.Reserva.Confirmada)
            {
                return ResultadoOperacion.Error(Mensajes.ReservaNoConfirmada);
            }

            reserva.EstadoReserva = Estados.Reserva.CheckIn;
            reserva.Habitacion.EstadoHabitacion = Estados.Habitacion.Ocupada;

            _bitacora.Registrar(Estados.Modulos.Hospedaje, "CHECKIN", reserva.IdReserva,
                "Check-in de la habitacion " + reserva.Habitacion.NumeroHabitacion);
            Db.SaveChanges();

            return ResultadoOperacion.Ok("Check-in registrado. La habitacion quedo ocupada.", reserva.IdReserva);
        }

        public ResultadoOperacion CheckOut(int idReserva, int idUsuarioAsignado)
        {
            var reserva = Obtener(idReserva);
            if (reserva == null)
            {
                return ResultadoOperacion.Error(Mensajes.NoEncontrado);
            }

            if (reserva.EstadoReserva != Estados.Reserva.CheckIn)
            {
                return ResultadoOperacion.Error(Mensajes.CheckOutInvalido);
            }

            reserva.EstadoReserva = Estados.Reserva.CheckOut;
            reserva.Habitacion.EstadoHabitacion = Estados.Habitacion.Limpieza;

            Db.Housekeeping.Add(new TareaHousekeeping
            {
                IdHabitacion = reserva.IdHabitacion,
                IdUsuario = idUsuarioAsignado,
                TipoTarea = "Limpieza post check-out",
                FechaAsignacion = DateTime.Now,
                FechaLimite = DateTime.Now.AddHours(4),
                EstadoTarea = Estados.Tarea.Pendiente,
                Observaciones = "Generada automaticamente al cerrar la reserva " + reserva.IdReserva
            });

            _bitacora.Registrar(Estados.Modulos.Hospedaje, "CHECKOUT", reserva.IdReserva,
                "Check-out de la habitacion " + reserva.Habitacion.NumeroHabitacion);
            Db.SaveChanges();

            return ResultadoOperacion.Ok("Check-out registrado. Se genero la tarea de limpieza.", reserva.IdReserva);
        }

        public ResultadoOperacion Cancelar(int idReserva)
        {
            var reserva = Obtener(idReserva);
            if (reserva == null)
            {
                return ResultadoOperacion.Error(Mensajes.NoEncontrado);
            }

            if (reserva.EstadoReserva == Estados.Reserva.CheckIn)
            {
                return ResultadoOperacion.Error("No se puede cancelar una reserva con check-in activo.");
            }

            reserva.EstadoReserva = Estados.Reserva.Cancelada;

            _bitacora.Registrar(Estados.Modulos.Hospedaje, Estados.Acciones.Anular, reserva.IdReserva,
                "Reserva cancelada");
            Db.SaveChanges();

            return ResultadoOperacion.Ok("La reserva fue cancelada.", reserva.IdReserva);
        }
    }
}
