using SistemaHotelColibri.Abstract.Modelos.Facturas;

namespace SistemaHotelColibri.Abstract.Interfaces.DataAccess.Facturas;

public interface IRegistrarFacturaDA
{
    Task<int> RegistrarAsync(FacturaDto factura);
}
