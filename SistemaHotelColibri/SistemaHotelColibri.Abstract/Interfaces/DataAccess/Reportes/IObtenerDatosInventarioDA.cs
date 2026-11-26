using SistemaHotelColibri.Abstract.Modelos.Reportes;

namespace SistemaHotelColibri.Abstract.Interfaces.DataAccess.Reportes;

// REP-004: inventario
public interface IObtenerDatosInventarioDA
{
    Task<DatosInventarioDto> ObtenerAsync(FiltroReporte filtro);
}