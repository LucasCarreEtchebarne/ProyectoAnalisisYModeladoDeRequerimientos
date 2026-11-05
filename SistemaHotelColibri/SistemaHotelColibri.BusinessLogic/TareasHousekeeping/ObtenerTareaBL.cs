using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.TareasHousekeeping;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.TareasHousekeeping;
using SistemaHotelColibri.Abstract.Modelos.TareasHousekeeping;

namespace SistemaHotelColibri.BusinessLogic.TareasHousekeeping;

public class ObtenerTareaBL : IObtenerTareaBL
{
    private readonly IObtenerTareaDA _obtenerDA;

    public ObtenerTareaBL(IObtenerTareaDA obtenerDA)
    {
        _obtenerDA = obtenerDA;
    }

    public Task<List<TareaHousekeepingDto>> ObtenerAsync(string? estado)
    {
        return _obtenerDA.ObtenerAsync(estado);
    }
}