using SistemaHotelColibri.Abstract.Modelos.Reportes;

namespace SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Reportes;

// REP-002: ventas del restaurante
public interface IObtenerReporteVentasBL
{
    Task<ReporteVentasDto> ObtenerAsync(FiltroReporte filtro);
}