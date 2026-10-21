using SistemaHotelColibri.Abstract.Modelos.Reservas;

namespace SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Reservas;

public interface IObtenerOpcionesReservaBL
{
    Task<OpcionesReservaDto> ObtenerAsync(int idClienteActual = 0, int idHabitacionActual = 0);
}
