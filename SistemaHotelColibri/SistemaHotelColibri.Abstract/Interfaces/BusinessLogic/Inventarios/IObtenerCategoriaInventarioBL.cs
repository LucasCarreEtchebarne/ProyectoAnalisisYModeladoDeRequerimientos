using SistemaHotelColibri.Abstract.Modelos.Inventarios;

namespace SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Inventarios;

public interface IObtenerCategoriaInventarioBL
{
    Task<List<CategoriaInventarioDto>> ObtenerAsync(string? estado);
}
