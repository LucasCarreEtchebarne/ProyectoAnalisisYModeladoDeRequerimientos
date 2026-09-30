using SistemaHotelColibri.Abstract.Modelos.Inventarios;

namespace SistemaHotelColibri.Abstract.Interfaces.DataAccess.Inventarios;

public interface IAgregarInventarioDA
{
    Task<int> AgregarAsync(InventarioDto producto);
}
