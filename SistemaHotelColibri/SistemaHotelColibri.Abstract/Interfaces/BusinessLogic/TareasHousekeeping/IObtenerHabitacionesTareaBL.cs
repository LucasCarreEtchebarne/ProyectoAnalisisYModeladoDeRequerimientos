using SistemaHotelColibri.Abstract.Modelos.Habitaciones;

namespace SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.TareasHousekeeping;

public interface IObtenerHabitacionesTareaBL
{
    Task<List<HabitacionDto>> ObtenerAsync();
}