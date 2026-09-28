using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Habitaciones;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Habitaciones;
using SistemaHotelColibri.Abstract.Modelos.Habitaciones;

namespace SistemaHotelColibri.BusinessLogic.Habitaciones;

public class ObtenerHabitacionPorIdBL : IObtenerHabitacionPorIdBL
{
    private readonly IObtenerHabitacionPorIdDA _obtenerPorIdDA;

    public ObtenerHabitacionPorIdBL(IObtenerHabitacionPorIdDA obtenerPorIdDA)
    {
        _obtenerPorIdDA = obtenerPorIdDA;
    }

    public Task<HabitacionDto?> ObtenerPorIdAsync(int idHabitacion)
    {
        return _obtenerPorIdDA.ObtenerPorIdAsync(idHabitacion);
    }
}
