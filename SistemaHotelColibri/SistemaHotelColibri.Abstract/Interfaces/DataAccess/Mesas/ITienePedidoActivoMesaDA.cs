namespace SistemaHotelColibri.Abstract.Interfaces.DataAccess.Mesas;

public interface ITienePedidoActivoMesaDA
{
    Task<bool> TienePedidoActivoAsync(int idMesa);
}
