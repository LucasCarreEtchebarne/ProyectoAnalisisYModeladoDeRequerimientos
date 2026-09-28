using SistemaHotelColibri.Abstract.Modelos.Habitaciones;

namespace SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Habitaciones;

public interface IObtenerHabitacionPorIdBL
{
    Task<HabitacionDto?> ObtenerPorIdAsync(int idHabitacion);
}
