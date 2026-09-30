using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Modelos.Inventarios;

namespace SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Inventarios;

public interface IAgregarInventarioBL
{
    Task<ResultadoOperacion> AgregarAsync(InventarioDto producto);
}
