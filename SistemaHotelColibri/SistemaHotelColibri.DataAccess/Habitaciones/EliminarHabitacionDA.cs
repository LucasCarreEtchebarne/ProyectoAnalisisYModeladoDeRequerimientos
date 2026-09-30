using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Habitaciones;
using SistemaHotelColibri.DataAccess.Contexto;

namespace SistemaHotelColibri.DataAccess.Habitaciones;

public class EliminarHabitacionDA : IEliminarHabitacionDA
{
    private readonly HotelColibriContext _contexto;

    public EliminarHabitacionDA(HotelColibriContext contexto)
    {
        _contexto = contexto;
    }

    public async Task<bool> EliminarAsync(int idHabitacion)
    {
        var entidad = await _contexto.Habitacion.FindAsync(idHabitacion);
        if (entidad == null)
        {
            return false;
        }

        _contexto.Habitacion.Remove(entidad);
        await _contexto.SaveChangesAsync();
        return true;
    }
}
