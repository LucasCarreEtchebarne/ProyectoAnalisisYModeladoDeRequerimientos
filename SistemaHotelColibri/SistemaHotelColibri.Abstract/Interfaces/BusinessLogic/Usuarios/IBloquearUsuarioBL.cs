using SistemaHotelColibri.Abstract.Comun;

namespace SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Usuarios;

public interface IBloquearUsuarioBL
{
    Task<ResultadoOperacion> BloquearAsync(int idUsuario, int idUsuarioActual);
    Task<ResultadoOperacion> DesbloquearAsync(int idUsuario);
}
