using SistemaHotelColibri.Abstract.Modelos.Reportes;

namespace SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Reportes;

// REP-003: ingresos totales
public interface IObtenerReporteIngresosBL
{
    Task<ReporteIngresosDto> ObtenerAsync(FiltroReporte filtro);
}