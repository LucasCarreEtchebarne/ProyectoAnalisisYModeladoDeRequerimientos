namespace SistemaHotelColibri.Abstract.Interfaces.DataAccess.Usuarios;

public interface IEliminarUsuarioDA
{
    Task<bool> EliminarAsync(int idUsuario);
}
