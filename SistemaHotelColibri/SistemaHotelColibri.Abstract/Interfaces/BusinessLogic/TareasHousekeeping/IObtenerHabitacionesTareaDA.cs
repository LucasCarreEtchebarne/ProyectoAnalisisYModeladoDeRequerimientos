using SistemaHotelColibri.Abstract.Modelos.Habitaciones;

namespace SistemaHotelColibri.Abstract.Interfaces.DataAccess.TareasHousekeeping;

public interface IObtenerHabitacionesTareaDA
{
    Task<List<HabitacionDto>> ObtenerAsync();
}