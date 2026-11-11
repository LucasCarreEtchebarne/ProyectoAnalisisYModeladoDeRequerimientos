using SistemaHotelColibri.Abstract.Comun;

namespace SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Facturas;

public interface IAnularFacturaBL
{
    Task<ResultadoOperacion> AnularAsync(int idFactura, string? motivo);
}
