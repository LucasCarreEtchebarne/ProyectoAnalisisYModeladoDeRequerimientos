using SistemaHotelColibri.Abstract.Modelos.Habitaciones;

namespace SistemaHotelColibri.Abstract.Interfaces.DataAccess.Habitaciones;

public interface IObtenerHabitacionDA
{
    Task<List<HabitacionDto>> ObtenerAsync(string? estado);
}
