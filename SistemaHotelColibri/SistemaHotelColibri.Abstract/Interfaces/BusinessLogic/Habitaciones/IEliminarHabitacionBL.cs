using SistemaHotelColibri.Abstract.Comun;

namespace SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Habitaciones;

public interface IEliminarHabitacionBL
{
    Task<ResultadoOperacion> EliminarAsync(int idHabitacion);
}
