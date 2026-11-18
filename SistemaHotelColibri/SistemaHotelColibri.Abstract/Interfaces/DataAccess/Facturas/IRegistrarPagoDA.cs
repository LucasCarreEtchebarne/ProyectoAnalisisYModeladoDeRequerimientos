using SistemaHotelColibri.Abstract.Modelos.Facturas;

namespace SistemaHotelColibri.Abstract.Interfaces.DataAccess.Facturas;

public interface IRegistrarPagoDA
{
    Task<int> RegistrarAsync(PagoDto pago, bool facturaQuedaPagada);
}
