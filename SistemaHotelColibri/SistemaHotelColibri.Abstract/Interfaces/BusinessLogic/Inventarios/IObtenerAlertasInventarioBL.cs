using SistemaHotelColibri.Abstract.Modelos.Inventarios;

namespace SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Inventarios;

public interface IObtenerAlertasInventarioBL
{
    Task<List<InventarioDto>> ObtenerAlertasAsync();
}
