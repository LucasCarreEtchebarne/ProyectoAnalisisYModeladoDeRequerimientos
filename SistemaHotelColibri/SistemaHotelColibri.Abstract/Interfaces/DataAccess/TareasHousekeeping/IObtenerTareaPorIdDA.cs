using SistemaHotelColibri.Abstract.Modelos.TareasHousekeeping;

namespace SistemaHotelColibri.Abstract.Interfaces.DataAccess.TareasHousekeeping;

public interface IObtenerTareaPorIdDA
{
    Task<TareaHousekeepingDto?> ObtenerPorIdAsync(int idTarea);
}