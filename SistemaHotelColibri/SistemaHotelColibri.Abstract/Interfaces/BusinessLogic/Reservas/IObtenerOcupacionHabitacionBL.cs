using SistemaHotelColibri.Abstract.Modelos.Reservas;

namespace SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Reservas;

public interface IObtenerOcupacionHabitacionBL
{
    // Clave: IdHabitacion. Solo trae las habitaciones que tienen alguna reserva vigente.
    Task<Dictionary<int, OcupacionHabitacionDto>> ObtenerAsync();
}
