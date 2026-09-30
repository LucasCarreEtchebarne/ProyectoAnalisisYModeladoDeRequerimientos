using SistemaHotelColibri.Abstract.Modelos.Inventarios;

namespace SistemaHotelColibri.Abstract.Interfaces.DataAccess.Inventarios;

public interface IEditarInventarioDA
{
    Task<bool> EditarAsync(InventarioDto producto);
}
