namespace SistemaHotelColibri.Abstract.Interfaces.DataAccess.Reservas;

public interface IHabitacionDisponibleReservaDA
{
    Task<bool> EstaDisponibleAsync(int idHabitacion, DateOnly fechaEntrada, DateOnly fechaSalida, int idReservaExcluida);
}
