namespace SistemaHotelColibri.Abstract.Interfaces.DataAccess.TareasHousekeeping;

public interface ICambiarEstadoTareaDA
{
    Task<bool> CambiarEstadoAsync(int idTarea, string nuevoEstado, DateTime? fechaCompletada);
}