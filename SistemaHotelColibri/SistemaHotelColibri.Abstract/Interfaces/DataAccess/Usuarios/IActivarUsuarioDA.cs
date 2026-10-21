namespace SistemaHotelColibri.Abstract.Interfaces.DataAccess.Usuarios;

public interface IActivarUsuarioDA
{
    Task<bool> ActivarAsync(int idUsuario);
}
