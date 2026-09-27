using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Modelos.Inventarios;

namespace SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Inventarios;

public interface IEditarInventarioBL
{
    Task<ResultadoOperacion> EditarAsync(InventarioDto producto);
}
