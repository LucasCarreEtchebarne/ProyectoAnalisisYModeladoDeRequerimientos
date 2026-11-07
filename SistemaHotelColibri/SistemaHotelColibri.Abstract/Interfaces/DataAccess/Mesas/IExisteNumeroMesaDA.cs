namespace SistemaHotelColibri.Abstract.Interfaces.DataAccess.Mesas;

public interface IExisteNumeroMesaDA
{
    Task<bool> ExisteNumeroAsync(int numeroMesa, int idExcluir);
}
