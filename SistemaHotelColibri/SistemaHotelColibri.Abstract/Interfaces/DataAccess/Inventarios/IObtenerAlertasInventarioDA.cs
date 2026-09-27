using SistemaHotelColibri.Abstract.Modelos.Inventarios;

namespace SistemaHotelColibri.Abstract.Interfaces.DataAccess.Inventarios;

public interface IObtenerAlertasInventarioDA
{
    Task<List<InventarioDto>> ObtenerAlertasAsync();
}
