using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Modelos.Habitaciones;

namespace SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Habitaciones;

public interface IObtenerHabitacionBL
{
    Task<ResultadoPaginado<HabitacionDto>> ObtenerAsync(FiltroListado filtro);
}
