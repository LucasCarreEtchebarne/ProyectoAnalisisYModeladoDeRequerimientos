using Microsoft.EntityFrameworkCore;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.TareasHousekeeping;
using SistemaHotelColibri.Abstract.Modelos.TareasHousekeeping;
using SistemaHotelColibri.DataAccess.Contexto;
using SistemaHotelColibri.DataAccess.Identidad;

namespace SistemaHotelColibri.DataAccess.TareasHousekeeping;

public class ObtenerTareaPorIdDA : IObtenerTareaPorIdDA
{
    private readonly HotelColibriContext _contexto;
    private readonly IdentidadContext _identidad;

    public ObtenerTareaPorIdDA(HotelColibriContext contexto, IdentidadContext identidad)
    {
        _contexto = contexto;
        _identidad = identidad;
    }

    public async Task<TareaHousekeepingDto?> ObtenerPorIdAsync(int idTarea)
    {
        var tarea = await _contexto.Housekeeping
            .AsNoTracking()
            .Include(t => t.IdHabitacionNavigation)
            .FirstOrDefaultAsync(t => t.IdTarea == idTarea);

        if (tarea == null)
        {
            return null;
        }

        var nombres = await _identidad.ObtenerNombresUsuarioAsync(new[] { tarea.IdUsuario });
        return tarea.ADto(nombres.GetValueOrDefault(tarea.IdUsuario, "Usuario #" + tarea.IdUsuario));
    }
}