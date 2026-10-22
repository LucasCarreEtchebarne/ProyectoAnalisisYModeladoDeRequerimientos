namespace SistemaHotelColibri.Abstract.Interfaces.DataAccess.Reservas;

public interface IEliminarReservaDA
{
    Task<bool> EliminarAsync(int idReserva);
}
