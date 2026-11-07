using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Mesas;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Mesas;
using SistemaHotelColibri.Abstract.Modelos.Mesas;

namespace SistemaHotelColibri.BusinessLogic.Mesas;

public class ObtenerMesaPorIdBL : IObtenerMesaPorIdBL
{
    private readonly IObtenerMesaPorIdDA _obtenerPorIdDA;

    public ObtenerMesaPorIdBL(IObtenerMesaPorIdDA obtenerPorIdDA)
    {
        _obtenerPorIdDA = obtenerPorIdDA;
    }

    public Task<MesaDto?> ObtenerPorIdAsync(int idMesa) => _obtenerPorIdDA.ObtenerPorIdAsync(idMesa);
}
