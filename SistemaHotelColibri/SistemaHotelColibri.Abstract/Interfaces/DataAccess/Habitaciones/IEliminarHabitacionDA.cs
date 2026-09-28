namespace SistemaHotelColibri.Abstract.Interfaces.DataAccess.Habitaciones;

public interface IEliminarHabitacionDA
{
    Task<bool> EliminarAsync(int idHabitacion);
}
