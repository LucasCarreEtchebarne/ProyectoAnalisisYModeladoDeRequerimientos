using SistemaHotelColibri.Abstract.Modelos.Reportes;

namespace SistemaHotelColibri.Abstract.Interfaces.DataAccess.Reportes;

// REP-001: ocupación de habitaciones
public interface IObtenerDatosOcupacionDA
{
    Task<DatosOcupacionDto> ObtenerAsync(FiltroReporte filtro);
}