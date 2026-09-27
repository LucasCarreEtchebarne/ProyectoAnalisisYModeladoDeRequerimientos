using SistemaHotelColibri.Abstract.Modelos.Inventarios;

namespace SistemaHotelColibri.Abstract.Interfaces.DataAccess.Inventarios;

public interface IObtenerInventarioPorIdDA
{
    Task<InventarioDto?> ObtenerPorIdAsync(int idProducto);
}
