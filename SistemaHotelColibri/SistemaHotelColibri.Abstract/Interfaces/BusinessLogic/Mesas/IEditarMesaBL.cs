using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Modelos.Mesas;

namespace SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Mesas;

public interface IEditarMesaBL
{
    Task<ResultadoOperacion> EditarAsync(MesaDto datos);
}
