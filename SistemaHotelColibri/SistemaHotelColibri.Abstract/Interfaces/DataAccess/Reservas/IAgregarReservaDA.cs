using SistemaHotelColibri.Abstract.Modelos.Reservas;

namespace SistemaHotelColibri.Abstract.Interfaces.DataAccess.Reservas;

public interface IAgregarReservaDA
{
    Task<int> AgregarAsync(ReservaDto reserva);
}
