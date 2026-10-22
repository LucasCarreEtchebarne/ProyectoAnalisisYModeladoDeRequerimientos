using SistemaHotelColibri.Abstract.Modelos.TareasHousekeeping;

namespace SistemaHotelColibri.Abstract.Interfaces.DataAccess.TareasHousekeeping;

public interface IObtenerTareaDA
{
    Task<List<TareaHousekeepingDto>> ObtenerAsync(string? estado);
}