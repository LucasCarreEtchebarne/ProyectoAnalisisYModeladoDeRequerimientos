namespace SistemaHotelColibri.Abstract.Interfaces.DataAccess.TareasHousekeeping;

public interface ICancelarTareaDA
{
    Task<bool> CancelarAsync(int idTarea, string motivo, DateTime fechaCancelacion);
}