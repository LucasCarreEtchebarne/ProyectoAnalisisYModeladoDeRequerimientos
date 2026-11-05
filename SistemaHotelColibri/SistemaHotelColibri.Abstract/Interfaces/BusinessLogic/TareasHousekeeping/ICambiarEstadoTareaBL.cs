using SistemaHotelColibri.Abstract.Comun;

namespace SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.TareasHousekeeping;

public interface ICambiarEstadoTareaBL
{
    Task<ResultadoOperacion> CambiarEstadoAsync(int idTarea, string nuevoEstado);
}