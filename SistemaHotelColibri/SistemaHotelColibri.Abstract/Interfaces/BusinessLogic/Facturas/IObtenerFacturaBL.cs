using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Modelos.Facturas;

namespace SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Facturas;

public interface IObtenerFacturaBL
{
    Task<ResultadoPaginado<FacturaDto>> ObtenerAsync(FiltroFactura filtro);
}
