using SistemaHotelColibri.Abstract.Comun;

namespace SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Reservas;

public interface IEliminarReservaBL
{
    Task<ResultadoOperacion> EliminarAsync(int idReserva);
}
