using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Habitaciones;
using SistemaHotelColibri.Abstract.Modelos.Habitaciones;
using SistemaHotelColibri.DataAccess.Contexto;

namespace SistemaHotelColibri.DataAccess.Habitaciones;

public class AgregarHabitacionDA : IAgregarHabitacionDA
{
    private readonly HotelColibriContext _contexto;

    public AgregarHabitacionDA(HotelColibriContext contexto)
    {
        _contexto = contexto;
    }

    public async Task<int> AgregarAsync(HabitacionDto habitacion)
    {
        var entidad = habitacion.AEntidad();
        _contexto.Habitacion.Add(entidad);
        await _contexto.SaveChangesAsync();
        return entidad.IdHabitacion;
    }
}
