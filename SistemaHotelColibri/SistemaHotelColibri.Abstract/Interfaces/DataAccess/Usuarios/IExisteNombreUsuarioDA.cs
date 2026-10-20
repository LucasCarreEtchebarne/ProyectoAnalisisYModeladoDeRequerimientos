namespace SistemaHotelColibri.Abstract.Interfaces.DataAccess.Usuarios;

public interface IExisteNombreUsuarioDA
{
    Task<bool> ExisteNombreUsuarioAsync(string nombreUsuario, int idExcluir);
}
