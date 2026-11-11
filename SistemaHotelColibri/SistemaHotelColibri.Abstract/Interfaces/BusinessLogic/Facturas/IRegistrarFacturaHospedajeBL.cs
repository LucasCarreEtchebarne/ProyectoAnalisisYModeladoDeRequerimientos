using SistemaHotelColibri.Abstract.Comun;

namespace SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Facturas;

public interface IRegistrarFacturaHospedajeBL
{
    Task<ResultadoOperacion> RegistrarAsync(int idReserva, int idUsuario);
}
