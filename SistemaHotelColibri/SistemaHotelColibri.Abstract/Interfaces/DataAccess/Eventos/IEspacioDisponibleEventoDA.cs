namespace SistemaHotelColibri.Abstract.Interfaces.DataAccess.Eventos;

public interface IEspacioDisponibleEventoDA
{
    Task<bool> EstaDisponibleAsync(int idEspacio, DateOnly fecha, TimeOnly horaInicio, TimeOnly horaFin, int idEventoExcluido);
}
