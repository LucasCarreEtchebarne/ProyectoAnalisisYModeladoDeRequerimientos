using SistemaHotelColibri.Abstract.Modelos.Habitaciones;

namespace SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Habitaciones;

public interface IObtenerHabitacionBL
{
    Task<List<HabitacionDto>> ObtenerAsync(string? estado);
}
