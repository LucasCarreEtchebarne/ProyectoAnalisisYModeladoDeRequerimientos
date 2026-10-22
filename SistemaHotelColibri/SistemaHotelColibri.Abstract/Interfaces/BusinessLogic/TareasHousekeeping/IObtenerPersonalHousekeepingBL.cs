using SistemaHotelColibri.Abstract.Modelos.TareasHousekeeping;

namespace SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.TareasHousekeeping;

public interface IObtenerPersonalHousekeepingBL
{
    Task<List<PersonalHousekeepingDto>> ObtenerAsync();
}