using SistemaHotelColibri.Abstract.Modelos.Inventarios;

namespace SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Inventarios;

public interface IObtenerInventarioPorIdBL
{
    Task<InventarioDto?> ObtenerPorIdAsync(int idProducto);
}
