using SistemaHotelColibri.Abstract.Modelos.Habitaciones;

namespace SistemaHotelColibri.Abstract.Interfaces.DataAccess.Habitaciones;

public interface IAgregarHabitacionDA
{
    Task<int> AgregarAsync(HabitacionDto habitacion);
}
