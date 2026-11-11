using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Facturas;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Facturas;
using SistemaHotelColibri.Abstract.Modelos.Facturas;

namespace SistemaHotelColibri.BusinessLogic.Facturas;

public class ObtenerReservaFacturableBL : IObtenerReservaFacturableBL
{
    private readonly IObtenerReservaFacturableDA _obtenerDA;

    public ObtenerReservaFacturableBL(IObtenerReservaFacturableDA obtenerDA)
    {
        _obtenerDA = obtenerDA;
    }

    public Task<List<ReservaFacturableDto>> ListarAsync(string? busqueda) =>
        _obtenerDA.ListarAsync(string.IsNullOrWhiteSpace(busqueda) ? null : busqueda.Trim());

    public Task<ReservaFacturableDto?> ObtenerAsync(int idReserva) => _obtenerDA.ObtenerAsync(idReserva);
}
