using SistemaHotelColibri.Abstract.Modelos.Reportes;

namespace SistemaHotelColibri.Abstract.Interfaces.DataAccess.Reportes;

// REP-003: ingresos totales
public interface IObtenerDatosIngresosDA
{
    Task<DatosIngresosDto> ObtenerAsync(FiltroReporte filtro);
}