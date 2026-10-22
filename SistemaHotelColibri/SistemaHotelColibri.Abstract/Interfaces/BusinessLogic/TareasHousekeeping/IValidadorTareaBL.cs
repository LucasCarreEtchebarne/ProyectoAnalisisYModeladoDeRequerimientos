using SistemaHotelColibri.Abstract.Modelos.TareasHousekeeping;

namespace SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.TareasHousekeeping;

public interface IValidadorTareaBL
{
    Task<string?> ValidarAsync(TareaHousekeepingDto tarea);
}