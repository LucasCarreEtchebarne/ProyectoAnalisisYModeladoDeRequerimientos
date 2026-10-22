using Microsoft.EntityFrameworkCore;
using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.TareasHousekeeping;
using SistemaHotelColibri.Abstract.Modelos.TareasHousekeeping;
using SistemaHotelColibri.DataAccess.Contexto;
using SistemaHotelColibri.DataAccess.Identidad;

namespace SistemaHotelColibri.DataAccess.TareasHousekeeping;

public class ObtenerTareaDA : IObtenerTareaDA
{
    private readonly HotelColibriContext _contexto;
    private readonly IdentidadContext _identidad;

    public ObtenerTareaDA(HotelColibriContext contexto, IdentidadContext identidad)
    {
        _contexto = contexto;
        _identidad = identidad;
    }

    public async Task<List<TareaHousekeepingDto>> ObtenerAsync(string? estado)
    {
        var consulta = _contexto.Housekeeping
            .AsNoTracking()
            .Include(t => t.IdHabitacionNavigation)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(estado))
        {
            consulta = consulta.Where(t => t.EstadoTarea == estado);
        }

        // Primero las tareas activas, luego las que vencen antes.
        var tareas = await consulta
            .OrderBy(t => t.EstadoTarea == Estados.Tarea.Completada || t.EstadoTarea == Estados.Tarea.Cancelada)
            .ThenBy(t => t.FechaLimite == null)
            .ThenBy(t => t.FechaLimite)
            .ThenByDescending(t => t.IdTarea)
            .ToListAsync();

        var nombres = await _identidad.ObtenerNombresUsuarioAsync(tareas.Select(t => t.IdUsuario));

        return tareas
            .Select(t => t.ADto(nombres.GetValueOrDefault(t.IdUsuario, "Usuario #" + t.IdUsuario)))
            .ToList();
    }
}