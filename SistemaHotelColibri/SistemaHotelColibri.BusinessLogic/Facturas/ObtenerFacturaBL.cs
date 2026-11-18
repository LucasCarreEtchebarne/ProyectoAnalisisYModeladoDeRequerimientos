using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Facturas;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Facturas;
using SistemaHotelColibri.Abstract.Modelos.Facturas;

namespace SistemaHotelColibri.BusinessLogic.Facturas;

public class ObtenerFacturaBL : IObtenerFacturaBL
{
    private readonly IObtenerFacturaDA _obtenerDA;

    public ObtenerFacturaBL(IObtenerFacturaDA obtenerDA)
    {
        _obtenerDA = obtenerDA;
    }

    public Task<ResultadoPaginado<FacturaDto>> ObtenerAsync(FiltroFactura filtro)
    {
        filtro.Busqueda = string.IsNullOrWhiteSpace(filtro.Busqueda) ? null : filtro.Busqueda.Trim();

        if (filtro.Desde.HasValue && filtro.Hasta.HasValue && filtro.Desde > filtro.Hasta)
        {
            (filtro.Desde, filtro.Hasta) = (filtro.Hasta, filtro.Desde);
        }

        return _obtenerDA.ObtenerAsync(filtro);
    }
}
