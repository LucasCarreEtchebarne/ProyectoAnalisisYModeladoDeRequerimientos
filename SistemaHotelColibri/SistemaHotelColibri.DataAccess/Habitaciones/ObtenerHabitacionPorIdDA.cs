using Microsoft.EntityFrameworkCore;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Habitaciones;
using SistemaHotelColibri.Abstract.Modelos.Habitaciones;
using SistemaHotelColibri.DataAccess.Contexto;

namespace SistemaHotelColibri.DataAccess.Habitaciones;

public class ObtenerHabitacionPorIdDA : IObtenerHabitacionPorIdDA
{
    private readonly HotelColibriContext _contexto;

    public ObtenerHabitacionPorIdDA(HotelColibriContext contexto)
    {
        _contexto = contexto;
    }

    public async Task<HabitacionDto?> ObtenerPorIdAsync(int idHabitacion)
    {
        var habitacion = await _contexto.Habitacion
            .AsNoTracking()
            .FirstOrDefaultAsync(h => h.IdHabitacion == idHabitacion);

        return habitacion?.ADto();
    }
}
