using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Modelos.TareasHousekeeping;

namespace SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.TareasHousekeeping;

public interface IAgregarTareaBL
{
    Task<ResultadoOperacion> AgregarAsync(TareaHousekeepingDto tarea);
}