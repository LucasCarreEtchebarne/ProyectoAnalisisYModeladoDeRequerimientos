namespace SistemaHotelColibri.Abstract.Interfaces.DataAccess.Usuarios;

public interface IExisteCorreoUsuarioDA
{
    Task<bool> ExisteCorreoAsync(string correoElectronico, int idExcluir);
}
