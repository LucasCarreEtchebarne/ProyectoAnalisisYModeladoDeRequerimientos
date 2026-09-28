using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Modelos.Habitaciones;

namespace SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Habitaciones;

public interface IAgregarHabitacionBL
{
    Task<ResultadoOperacion> AgregarAsync(HabitacionDto habitacion);
}
