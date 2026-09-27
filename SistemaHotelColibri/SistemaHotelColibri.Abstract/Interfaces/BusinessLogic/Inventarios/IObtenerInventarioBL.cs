using SistemaHotelColibri.Abstract.Modelos.Inventarios;

namespace SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Inventarios;

public interface IObtenerInventarioBL
{
    Task<List<InventarioDto>> ObtenerAsync(string? estado);
}
