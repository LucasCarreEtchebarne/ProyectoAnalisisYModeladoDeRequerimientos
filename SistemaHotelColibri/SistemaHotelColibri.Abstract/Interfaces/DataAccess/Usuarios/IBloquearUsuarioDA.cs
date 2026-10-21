namespace SistemaHotelColibri.Abstract.Interfaces.DataAccess.Usuarios;

public interface IBloquearUsuarioDA
{
    Task<bool> BloquearAsync(int idUsuario);
    Task<bool> DesbloquearAsync(int idUsuario);
}
