using Microsoft.EntityFrameworkCore;
using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.TareasHousekeeping;
using SistemaHotelColibri.DataAccess.Contexto;

namespace SistemaHotelColibri.DataAccess.TareasHousekeeping;

public class CambiarEstadoTareaDA : ICambiarEstadoTareaDA
{
    private readonly HotelColibriContext _contexto;

    public CambiarEstadoTareaDA(HotelColibriContext contexto)
    {
        _contexto = contexto;
    }

    public async Task<bool> CambiarEstadoAsync(int idTarea, string nuevoEstado, DateTime? fechaCompletada)
    {
        var entidad = await _contexto.Housekeeping.FindAsync(idTarea);
        if (entidad == null)
        {
            return false;
        }

        entidad.EstadoTarea = nuevoEstado;
        entidad.FechaCompletada = fechaCompletada;

        // Terminada la última tarea pendiente, una habitación en limpieza o mantenimiento vuelve a estar disponible.
        if (nuevoEstado == Estados.Tarea.Completada)
        {
            var quedanTareas = await _contexto.Housekeeping.AnyAsync(t =>
                t.IdHabitacion == entidad.IdHabitacion
                && t.IdTarea != idTarea
                && (t.EstadoTarea == Estados.Tarea.Pendiente || t.EstadoTarea == Estados.Tarea.EnProceso));

            var habitacion = await _contexto.Habitacion.FindAsync(entidad.IdHabitacion);
            if (!quedanTareas && habitacion != null
                && (habitacion.EstadoHabitacion == Estados.Habitacion.Limpieza || habitacion.EstadoHabitacion == Estados.Habitacion.Mantenimiento))
            {
                habitacion.EstadoHabitacion = Estados.Habitacion.Disponible;
            }
        }

        await _contexto.SaveChangesAsync();
        return true;
    }
}