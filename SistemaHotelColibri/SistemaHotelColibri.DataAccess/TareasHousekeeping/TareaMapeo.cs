using Microsoft.EntityFrameworkCore;
using SistemaHotelColibri.Abstract.Modelos.TareasHousekeeping;
using SistemaHotelColibri.DataAccess.Entidades;
using SistemaHotelColibri.DataAccess.Identidad;

namespace SistemaHotelColibri.DataAccess.TareasHousekeeping;

public static class TareaMapeo
{
    public static TareaHousekeepingDto ADto(this Housekeeping entidad, string nombreUsuario) => new()
    {
        IdTarea = entidad.IdTarea,
        IdHabitacion = entidad.IdHabitacion,
        NumeroHabitacion = entidad.IdHabitacionNavigation?.NumeroHabitacion ?? string.Empty,
        IdUsuario = entidad.IdUsuario,
        NombreUsuario = nombreUsuario,
        TipoTarea = entidad.TipoTarea,
        FechaAsignacion = entidad.FechaAsignacion,
        FechaLimite = entidad.FechaLimite,
        FechaCompletada = entidad.FechaCompletada,
        EstadoTarea = entidad.EstadoTarea,
        Observaciones = entidad.Observaciones,
        MotivoCancelacion = entidad.MotivoCancelacion,
        FechaCancelacion = entidad.FechaCancelacion
    };

    public static Housekeeping AEntidad(this TareaHousekeepingDto dto) => new()
    {
        IdTarea = dto.IdTarea,
        IdHabitacion = dto.IdHabitacion,
        IdUsuario = dto.IdUsuario,
        TipoTarea = dto.TipoTarea,
        FechaAsignacion = dto.FechaAsignacion,
        FechaLimite = dto.FechaLimite,
        FechaCompletada = dto.FechaCompletada,
        EstadoTarea = dto.EstadoTarea,
        Observaciones = dto.Observaciones,
        MotivoCancelacion = dto.MotivoCancelacion,
        FechaCancelacion = dto.FechaCancelacion
    };

    // Los usuarios están en el contexto de Identity (AspNetUsers), por eso los nombres se consultan aparte.
    public static async Task<Dictionary<int, string>> ObtenerNombresUsuarioAsync(
        this IdentidadContext identidad, IEnumerable<int> idsUsuario)
    {
        var ids = idsUsuario.Distinct().ToList();
        if (ids.Count == 0)
        {
            return new Dictionary<int, string>();
        }

        return await identidad.Users
            .AsNoTracking()
            .Where(u => ids.Contains(u.Id))
            .ToDictionaryAsync(u => u.Id, u => u.NombreCompleto);
    }
}