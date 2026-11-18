namespace SistemaHotelColibri.Abstract.Interfaces.DataAccess.Facturas;

public interface IAnularFacturaDA
{
    Task<bool> AnularAsync(int idFactura, string motivo, DateTime fechaAnulacion);
}
