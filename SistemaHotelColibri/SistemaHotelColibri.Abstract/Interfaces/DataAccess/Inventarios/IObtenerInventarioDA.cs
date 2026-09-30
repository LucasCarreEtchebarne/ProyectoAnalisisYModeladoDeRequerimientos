using SistemaHotelColibri.Abstract.Modelos.Inventarios;

namespace SistemaHotelColibri.Abstract.Interfaces.DataAccess.Inventarios;

public interface IObtenerInventarioDA
{
    Task<List<InventarioDto>> ObtenerAsync(string? estado);
}
