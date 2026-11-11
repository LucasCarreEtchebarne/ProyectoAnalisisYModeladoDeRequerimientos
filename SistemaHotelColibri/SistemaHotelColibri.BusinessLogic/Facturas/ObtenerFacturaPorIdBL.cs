using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Facturas;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Facturas;
using SistemaHotelColibri.Abstract.Modelos.Facturas;

namespace SistemaHotelColibri.BusinessLogic.Facturas;

public class ObtenerFacturaPorIdBL : IObtenerFacturaPorIdBL
{
    private readonly IObtenerFacturaPorIdDA _obtenerPorIdDA;

    public ObtenerFacturaPorIdBL(IObtenerFacturaPorIdDA obtenerPorIdDA)
    {
        _obtenerPorIdDA = obtenerPorIdDA;
    }

    public Task<FacturaDto?> ObtenerPorIdAsync(int idFactura) => _obtenerPorIdDA.ObtenerPorIdAsync(idFactura);
}
