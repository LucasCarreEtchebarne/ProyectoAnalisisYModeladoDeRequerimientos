using Microsoft.EntityFrameworkCore;
using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.TareasHousekeeping;
using SistemaHotelColibri.DataAccess.Contexto;

namespace SistemaHotelColibri.DataAccess.TareasHousekeeping;

public class ExisteTareaActivaDA : IExisteTareaActivaDA
{
    private readonly HotelColibriContext _contexto;

    public ExisteTareaActivaDA(HotelColibriContext contexto)
    {
        _contexto = contexto;
    }

    public async Task<bool> ExisteTareaActivaAsync(int idHabitacion, string tipoTarea, int idExcluir)
    {
        return await _contexto.Housekeeping.AnyAsync(t =>
            t.IdHabitacion == idHabitacion
            && t.TipoTarea == tipoTarea
            && t.IdTarea != idExcluir
            && (t.EstadoTarea == Estados.Tarea.Pendiente || t.EstadoTarea == Estados.Tarea.EnProceso));
    }
}