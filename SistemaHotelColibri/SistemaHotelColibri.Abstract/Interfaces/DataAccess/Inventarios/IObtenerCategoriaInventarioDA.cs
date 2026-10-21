using SistemaHotelColibri.Abstract.Modelos.Inventarios;

namespace SistemaHotelColibri.Abstract.Interfaces.DataAccess.Inventarios;

public interface IObtenerCategoriaInventarioDA
{
    Task<List<CategoriaInventarioDto>> ObtenerAsync(string? estado);
}
