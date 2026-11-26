using SistemaHotelColibri.Abstract.Modelos.Reservas;
using SistemaHotelColibri.Abstract.Modelos.Reportes;

namespace SistemaHotelColibri.Abstract.Interfaces.DataAccess.Reportes;

// REP-005: reservas
public interface IObtenerDatosReservasDA
{
    Task<List<ReservaDto>> ObtenerAsync(FiltroReporte filtro);
}