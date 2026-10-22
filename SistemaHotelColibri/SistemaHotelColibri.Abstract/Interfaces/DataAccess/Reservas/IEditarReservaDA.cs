using SistemaHotelColibri.Abstract.Modelos.Reservas;

namespace SistemaHotelColibri.Abstract.Interfaces.DataAccess.Reservas;

public interface IEditarReservaDA
{
    Task<bool> EditarAsync(ReservaDto reserva);
}
