using SistemaHotelColibri.Abstract.Modelos.TareasHousekeeping;

namespace SistemaHotelColibri.Abstract.Interfaces.DataAccess.TareasHousekeeping;

public interface IObtenerPersonalHousekeepingDA
{
    Task<List<PersonalHousekeepingDto>> ObtenerAsync();
}