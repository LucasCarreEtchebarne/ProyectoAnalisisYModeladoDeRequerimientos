using Microsoft.EntityFrameworkCore;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.TareasHousekeeping;
using SistemaHotelColibri.Abstract.Modelos.Habitaciones;
using SistemaHotelColibri.DataAccess.Contexto;

namespace SistemaHotelColibri.DataAccess.TareasHousekeeping;

public class ObtenerHabitacionesTareaDA : IObtenerHabitacionesTareaDA
{
    private readonly HotelColibriContext _contexto;

    public ObtenerHabitacionesTareaDA(HotelColibriContext contexto)
    {
        _contexto = contexto;
    }

    // Lista completa (sin paginar) para el combo del formulario de tareas.
    public async Task<List<HabitacionDto>> ObtenerAsync()
    {
        return await _contexto.Habitacion
            .AsNoTracking()
            .OrderBy(h => h.NumeroHabitacion)
            .Select(h => new HabitacionDto
            {
                IdHabitacion = h.IdHabitacion,
                NumeroHabitacion = h.NumeroHabitacion,
                TipoHabitacion = h.TipoHabitacion,
                EstadoHabitacion = h.EstadoHabitacion
            })
            .ToListAsync();
    }
}