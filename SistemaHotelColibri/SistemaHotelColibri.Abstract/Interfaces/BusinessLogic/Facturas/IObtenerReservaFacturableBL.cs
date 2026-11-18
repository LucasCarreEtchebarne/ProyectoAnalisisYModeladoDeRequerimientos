using SistemaHotelColibri.Abstract.Modelos.Facturas;

namespace SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Facturas;

public interface IObtenerReservaFacturableBL
{
    Task<List<ReservaFacturableDto>> ListarAsync(string? busqueda);
    Task<ReservaFacturableDto?> ObtenerAsync(int idReserva);
}
