using SistemaHotelColibri.Abstract.Modelos.Reservas;

namespace SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Reservas;

public interface IObtenerReservaPorIdBL
{
    Task<ReservaDto?> ObtenerPorIdAsync(int idReserva);
}
