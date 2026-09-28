using SistemaHotelColibri.Abstract.Modelos.Habitaciones;

namespace SistemaHotelColibri.Abstract.Interfaces.DataAccess.Habitaciones;

public interface IObtenerHabitacionPorIdDA
{
    Task<HabitacionDto?> ObtenerPorIdAsync(int idHabitacion);
}
