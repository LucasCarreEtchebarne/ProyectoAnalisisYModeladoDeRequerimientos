using SistemaHotelColibri.Abstract.Modelos.Reportes;

namespace SistemaHotelColibri.Abstract.Interfaces.DataAccess.Reportes;

// REP-002: ventas del restaurante
public interface IObtenerDatosVentasDA
{
    Task<List<LineaVentaDto>> ObtenerAsync(FiltroReporte filtro);
}