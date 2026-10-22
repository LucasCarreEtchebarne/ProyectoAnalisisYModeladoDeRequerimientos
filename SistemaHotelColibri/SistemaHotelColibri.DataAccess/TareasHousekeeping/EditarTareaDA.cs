using SistemaHotelColibri.Abstract.Interfaces.DataAccess.TareasHousekeeping;
using SistemaHotelColibri.Abstract.Modelos.TareasHousekeeping;
using SistemaHotelColibri.DataAccess.Contexto;

namespace SistemaHotelColibri.DataAccess.TareasHousekeeping;

public class EditarTareaDA : IEditarTareaDA
{
    private readonly HotelColibriContext _contexto;

    public EditarTareaDA(HotelColibriContext contexto)
    {
        _contexto = contexto;
    }

    public async Task<bool> EditarAsync(TareaHousekeepingDto tarea)
    {
        var entidad = await _contexto.Housekeeping.FindAsync(tarea.IdTarea);
        if (entidad == null)
        {
            return false;
        }

        // Solo los datos editables (HSK-004). El estado se maneja en HSK-005 y HSK-006.
        entidad.IdHabitacion = tarea.IdHabitacion;
        entidad.IdUsuario = tarea.IdUsuario;
        entidad.TipoTarea = tarea.TipoTarea;
        entidad.FechaLimite = tarea.FechaLimite;
        entidad.Observaciones = tarea.Observaciones;

        await _contexto.SaveChangesAsync();
        return true;
    }
}