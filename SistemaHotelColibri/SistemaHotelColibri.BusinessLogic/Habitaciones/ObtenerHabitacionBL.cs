using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Habitaciones;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Habitaciones;
using SistemaHotelColibri.Abstract.Modelos.Habitaciones;

namespace SistemaHotelColibri.BusinessLogic.Habitaciones;

public class ObtenerHabitacionBL : IObtenerHabitacionBL
{
    private readonly IObtenerHabitacionDA _obtenerDA;

    public ObtenerHabitacionBL(IObtenerHabitacionDA obtenerDA)
    {
        _obtenerDA = obtenerDA;
    }

    public Task<List<HabitacionDto>> ObtenerAsync(string? estado)
    {
        return _obtenerDA.ObtenerAsync(estado);
    }
}
