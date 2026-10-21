using SistemaHotelColibri.Abstract.Comun;
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

    public Task<ResultadoPaginado<HabitacionDto>> ObtenerAsync(FiltroListado filtro)
    {
        filtro.Busqueda = string.IsNullOrWhiteSpace(filtro.Busqueda) ? null : filtro.Busqueda.Trim();
        return _obtenerDA.ObtenerAsync(filtro);
    }
}
