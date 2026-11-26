using SistemaHotelColibri.Abstract.Modelos.Reportes;

namespace SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Reportes;

// REP-001: ocupación de habitaciones
public interface IObtenerReporteOcupacionBL
{
    Task<ReporteOcupacionDto> ObtenerAsync(FiltroReporte filtro);
}