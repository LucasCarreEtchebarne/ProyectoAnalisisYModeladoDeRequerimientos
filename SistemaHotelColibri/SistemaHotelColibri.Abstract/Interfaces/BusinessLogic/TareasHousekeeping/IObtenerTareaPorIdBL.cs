using SistemaHotelColibri.Abstract.Modelos.TareasHousekeeping;

namespace SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.TareasHousekeeping;

public interface IObtenerTareaPorIdBL
{
    Task<TareaHousekeepingDto?> ObtenerPorIdAsync(int idTarea);
}