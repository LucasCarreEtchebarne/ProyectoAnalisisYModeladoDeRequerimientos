using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Modelos.Habitaciones;

namespace SistemaHotelColibri.Abstract.Interfaces.DataAccess.Habitaciones;

public interface IObtenerHabitacionDA
{
    Task<ResultadoPaginado<HabitacionDto>> ObtenerAsync(FiltroListado filtro);
}
