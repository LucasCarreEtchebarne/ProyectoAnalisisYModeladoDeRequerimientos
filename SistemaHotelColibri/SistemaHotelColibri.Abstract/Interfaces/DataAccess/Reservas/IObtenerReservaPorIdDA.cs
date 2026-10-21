using SistemaHotelColibri.Abstract.Modelos.Reservas;

namespace SistemaHotelColibri.Abstract.Interfaces.DataAccess.Reservas;

public interface IObtenerReservaPorIdDA
{
    Task<ReservaDto?> ObtenerPorIdAsync(int idReserva);
}
