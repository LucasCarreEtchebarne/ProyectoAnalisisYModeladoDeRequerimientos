using SistemaHotelColibri.Abstract.Modelos.Reservas;

namespace SistemaHotelColibri.Abstract.Interfaces.DataAccess.Reservas;

public interface IObtenerOpcionesReservaDA
{
    Task<OpcionesReservaDto> ObtenerAsync(int idClienteActual, int idHabitacionActual);
}
