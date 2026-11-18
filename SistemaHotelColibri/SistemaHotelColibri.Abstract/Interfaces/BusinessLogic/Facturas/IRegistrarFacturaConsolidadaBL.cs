using SistemaHotelColibri.Abstract.Comun;

namespace SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Facturas;

public interface IRegistrarFacturaConsolidadaBL
{
    Task<ResultadoOperacion> RegistrarAsync(int idReserva, IReadOnlyCollection<int> idPedidos, int idUsuario);
}
