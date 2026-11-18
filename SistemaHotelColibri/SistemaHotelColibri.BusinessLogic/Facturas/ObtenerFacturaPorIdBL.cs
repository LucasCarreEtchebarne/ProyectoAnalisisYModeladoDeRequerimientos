using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Facturas;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Facturas;
using SistemaHotelColibri.Abstract.Modelos.Facturas;

namespace SistemaHotelColibri.BusinessLogic.Facturas;

public class ObtenerFacturaPorIdBL : IObtenerFacturaPorIdBL
{
    private readonly IObtenerFacturaPorIdDA _obtenerDA;

    public ObtenerFacturaPorIdBL(IObtenerFacturaPorIdDA obtenerDA)
    {
        _obtenerDA = obtenerDA;
    }

    public Task<FacturaDto?> ObtenerPorIdAsync(int idFactura) => _obtenerDA.ObtenerPorIdAsync(idFactura);
}
