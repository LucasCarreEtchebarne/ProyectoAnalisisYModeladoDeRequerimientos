using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Modelos.Mesas;

namespace SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Mesas;

public interface IAgregarMesaBL
{
    Task<ResultadoOperacion> AgregarAsync(MesaDto mesa);
}
