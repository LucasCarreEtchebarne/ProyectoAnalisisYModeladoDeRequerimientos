using SistemaHotelColibri.Abstract.Modelos.Reservas;

namespace SistemaHotelColibri.Abstract.Interfaces.DataAccess.Reservas;

// Reservas que todavía cuentan: en check-in, o pendientes/confirmadas que no han terminado.
public interface IObtenerReservasVigentesDA
{
    Task<List<ReservaDto>> ObtenerAsync(DateOnly hoy);
}
