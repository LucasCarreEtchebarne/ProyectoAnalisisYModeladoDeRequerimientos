using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Modelos.Facturas;

namespace SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Facturas;

public interface IRegistrarPagoBL
{
    Task<ResultadoOperacion> RegistrarAsync(PagoDto pago, int idUsuario);
}
