using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Inventarios;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Inventarios;
using SistemaHotelColibri.Abstract.Modelos.Inventarios;

namespace SistemaHotelColibri.BusinessLogic.Inventarios;

public class ObtenerInventarioBL : IObtenerInventarioBL
{
    private readonly IObtenerInventarioDA _obtenerDA;

    public ObtenerInventarioBL(IObtenerInventarioDA obtenerDA)
    {
        _obtenerDA = obtenerDA;
    }

    public Task<List<InventarioDto>> ObtenerAsync(string? estado)
    {
        return _obtenerDA.ObtenerAsync(estado);
    }
}
