using SistemaHotelColibri.Abstract.Modelos.TareasHousekeeping;

namespace SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.TareasHousekeeping;

public interface IObtenerTareaBL
{
    Task<List<TareaHousekeepingDto>> ObtenerAsync(string? estado);
}