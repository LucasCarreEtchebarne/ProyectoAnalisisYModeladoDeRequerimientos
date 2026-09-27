using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Inventarios;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Inventarios;
using SistemaHotelColibri.Abstract.Modelos.Inventarios;

namespace SistemaHotelColibri.BusinessLogic.Inventarios;

public class ObtenerInventarioPorIdBL : IObtenerInventarioPorIdBL
{
    private readonly IObtenerInventarioPorIdDA _obtenerPorIdDA;

    public ObtenerInventarioPorIdBL(IObtenerInventarioPorIdDA obtenerPorIdDA)
    {
        _obtenerPorIdDA = obtenerPorIdDA;
    }

    public Task<InventarioDto?> ObtenerPorIdAsync(int idProducto)
    {
        return _obtenerPorIdDA.ObtenerPorIdAsync(idProducto);
    }
}
