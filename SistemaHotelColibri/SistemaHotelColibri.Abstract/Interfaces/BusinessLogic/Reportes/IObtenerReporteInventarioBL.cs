using SistemaHotelColibri.Abstract.Modelos.Reportes;

namespace SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Reportes;

// REP-004: inventario
public interface IObtenerReporteInventarioBL
{
    Task<ReporteInventarioDto> ObtenerAsync(FiltroReporte filtro);
}