using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Habitaciones;
using SistemaHotelColibri.Abstract.Modelos.Habitaciones;
using SistemaHotelColibri.DataAccess.Contexto;

namespace SistemaHotelColibri.DataAccess.Habitaciones;

public class EditarHabitacionDA : IEditarHabitacionDA
{
    private readonly HotelColibriContext _contexto;

    public EditarHabitacionDA(HotelColibriContext contexto)
    {
        _contexto = contexto;
    }

    public async Task<bool> EditarAsync(HabitacionDto habitacion)
    {
        var entidad = await _contexto.Habitacion.FindAsync(habitacion.IdHabitacion);
        if (entidad == null)
        {
            return false;
        }

        _contexto.Entry(entidad).CurrentValues.SetValues(habitacion.AEntidad());
        await _contexto.SaveChangesAsync();
        return true;
    }
}
