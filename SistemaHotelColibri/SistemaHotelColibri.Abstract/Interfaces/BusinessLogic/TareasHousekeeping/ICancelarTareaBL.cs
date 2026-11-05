using SistemaHotelColibri.Abstract.Comun;

namespace SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.TareasHousekeeping;

public interface ICancelarTareaBL
{
    Task<ResultadoOperacion> CancelarAsync(int idTarea, string? motivo);
}