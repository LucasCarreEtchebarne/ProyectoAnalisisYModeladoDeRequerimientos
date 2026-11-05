using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.TareasHousekeeping;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.TareasHousekeeping;
using SistemaHotelColibri.Abstract.Modelos.TareasHousekeeping;

namespace SistemaHotelColibri.BusinessLogic.TareasHousekeeping;

public class ObtenerTareaPorIdBL : IObtenerTareaPorIdBL
{
    private readonly IObtenerTareaPorIdDA _obtenerPorIdDA;

    public ObtenerTareaPorIdBL(IObtenerTareaPorIdDA obtenerPorIdDA)
    {
        _obtenerPorIdDA = obtenerPorIdDA;
    }

    public Task<TareaHousekeepingDto?> ObtenerPorIdAsync(int idTarea)
    {
        return _obtenerPorIdDA.ObtenerPorIdAsync(idTarea);
    }
}