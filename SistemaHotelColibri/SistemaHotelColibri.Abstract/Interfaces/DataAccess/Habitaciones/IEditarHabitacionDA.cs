using SistemaHotelColibri.Abstract.Modelos.Habitaciones;

namespace SistemaHotelColibri.Abstract.Interfaces.DataAccess.Habitaciones;

public interface IEditarHabitacionDA
{
    Task<bool> EditarAsync(HabitacionDto habitacion);
}
