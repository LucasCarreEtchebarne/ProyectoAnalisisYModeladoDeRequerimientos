using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.TareasHousekeeping;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.TareasHousekeeping;
using SistemaHotelColibri.Abstract.Modelos.TareasHousekeeping;

namespace SistemaHotelColibri.BusinessLogic.TareasHousekeeping;

public class ObtenerPersonalHousekeepingBL : IObtenerPersonalHousekeepingBL
{
    private readonly IObtenerPersonalHousekeepingDA _obtenerPersonalDA;

    public ObtenerPersonalHousekeepingBL(IObtenerPersonalHousekeepingDA obtenerPersonalDA)
    {
        _obtenerPersonalDA = obtenerPersonalDA;
    }

    public Task<List<PersonalHousekeepingDto>> ObtenerAsync()
    {
        return _obtenerPersonalDA.ObtenerAsync();
    }
}