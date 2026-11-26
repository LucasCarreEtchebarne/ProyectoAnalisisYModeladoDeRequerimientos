using SistemaHotelColibri.Abstract.Modelos.Reportes;

namespace SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Reportes;

// REP-005: reservas
public interface IObtenerReporteReservasBL
{
    Task<ReporteReservasDto> ObtenerAsync(FiltroReporte filtro);
}