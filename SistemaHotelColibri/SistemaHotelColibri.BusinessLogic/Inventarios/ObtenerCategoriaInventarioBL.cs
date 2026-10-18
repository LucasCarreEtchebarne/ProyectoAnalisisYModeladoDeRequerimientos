using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Inventarios;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Inventarios;
using SistemaHotelColibri.Abstract.Modelos.Inventarios;

namespace SistemaHotelColibri.BusinessLogic.Inventarios;

public class ObtenerCategoriaInventarioBL : IObtenerCategoriaInventarioBL
{
    private readonly IObtenerCategoriaInventarioDA _obtenerDA;

    public ObtenerCategoriaInventarioBL(IObtenerCategoriaInventarioDA obtenerDA)
    {
        _obtenerDA = obtenerDA;
    }

    public Task<List<CategoriaInventarioDto>> ObtenerAsync(string? estado)
    {
        return _obtenerDA.ObtenerAsync(estado);
    }
}
