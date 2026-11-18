using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Modelos.Facturas;

namespace SistemaHotelColibri.Abstract.Interfaces.DataAccess.Facturas;

public interface IObtenerFacturaDA
{
    Task<ResultadoPaginado<FacturaDto>> ObtenerAsync(FiltroFactura filtro);
}
