using SistemaHotelColibri.Abstract.Comun;

namespace SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Facturas;

public interface IRegistrarFacturaRestauranteBL
{
    Task<ResultadoOperacion> RegistrarAsync(int idPedido, int idUsuario);
}
