using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Modelos.Reservas;

namespace SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Reservas;

public interface IEditarReservaBL
{
    Task<ResultadoOperacion> EditarAsync(ReservaDto reserva);
}
