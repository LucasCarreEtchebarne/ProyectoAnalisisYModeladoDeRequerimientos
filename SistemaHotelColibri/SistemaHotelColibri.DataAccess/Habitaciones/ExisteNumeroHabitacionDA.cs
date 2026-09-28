using Microsoft.EntityFrameworkCore;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Habitaciones;
using SistemaHotelColibri.DataAccess.Contexto;

namespace SistemaHotelColibri.DataAccess.Habitaciones;

public class ExisteNumeroHabitacionDA : IExisteNumeroHabitacionDA
{
    private readonly HotelColibriContext _contexto;

    public ExisteNumeroHabitacionDA(HotelColibriContext contexto)
    {
        _contexto = contexto;
    }

    public async Task<bool> ExisteNumeroAsync(string numeroHabitacion, int idExcluir)
    {
        return await _contexto.Habitacion
            .AnyAsync(h => h.NumeroHabitacion == numeroHabitacion && h.IdHabitacion != idExcluir);
    }
}
