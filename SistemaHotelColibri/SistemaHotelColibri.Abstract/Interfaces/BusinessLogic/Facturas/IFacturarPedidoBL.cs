using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Modelos.Facturas;

namespace SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Facturas;

public interface IFacturarPedidoBL
{
    // Si sale bien, IdGenerado es el id de la factura.
    Task<ResultadoOperacion> FacturarAsync(SolicitudCobroDto solicitud);
}
