using SistemaHotelColibri.Abstract.Modelos.Facturas;

namespace SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Facturas;

public interface IObtenerFacturaPorIdBL
{
    Task<FacturaDto?> ObtenerPorIdAsync(int idFactura);
}
