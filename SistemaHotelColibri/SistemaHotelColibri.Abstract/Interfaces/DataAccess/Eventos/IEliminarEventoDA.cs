namespace SistemaHotelColibri.Abstract.Interfaces.DataAccess.Eventos;

public interface IEliminarEventoDA
{
    Task<bool> EliminarAsync(int idEvento);
}
