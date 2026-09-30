namespace SistemaHotelColibri.Abstract.Interfaces.DataAccess.Habitaciones;

public interface IExisteNumeroHabitacionDA
{
    Task<bool> ExisteNumeroAsync(string numeroHabitacion, int idExcluir);
}
