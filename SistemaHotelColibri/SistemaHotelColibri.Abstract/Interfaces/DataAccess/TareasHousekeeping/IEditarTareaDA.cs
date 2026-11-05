using SistemaHotelColibri.Abstract.Modelos.TareasHousekeeping;

namespace SistemaHotelColibri.Abstract.Interfaces.DataAccess.TareasHousekeeping;

public interface IEditarTareaDA
{
    Task<bool> EditarAsync(TareaHousekeepingDto tarea);
}