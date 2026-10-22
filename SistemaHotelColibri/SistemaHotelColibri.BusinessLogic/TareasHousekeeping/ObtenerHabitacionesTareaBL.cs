using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.TareasHousekeeping;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.TareasHousekeeping;
using SistemaHotelColibri.Abstract.Modelos.Habitaciones;

namespace SistemaHotelColibri.BusinessLogic.TareasHousekeeping;

public class ObtenerHabitacionesTareaBL : IObtenerHabitacionesTareaBL
{
    private readonly IObtenerHabitacionesTareaDA _obtenerHabitacionesDA;

    public ObtenerHabitacionesTareaBL(IObtenerHabitacionesTareaDA obtenerHabitacionesDA)
    {
        _obtenerHabitacionesDA = obtenerHabitacionesDA;
    }

    public Task<List<HabitacionDto>> ObtenerAsync()
    {
        return _obtenerHabitacionesDA.ObtenerAsync();
    }
}