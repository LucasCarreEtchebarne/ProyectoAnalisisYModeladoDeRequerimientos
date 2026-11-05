using SistemaHotelColibri.Abstract.Modelos.TareasHousekeeping;

namespace SistemaHotelColibri.Abstract.Interfaces.DataAccess.TareasHousekeeping;

public interface IAgregarTareaDA
{
    Task<int> AgregarAsync(TareaHousekeepingDto tarea);
}