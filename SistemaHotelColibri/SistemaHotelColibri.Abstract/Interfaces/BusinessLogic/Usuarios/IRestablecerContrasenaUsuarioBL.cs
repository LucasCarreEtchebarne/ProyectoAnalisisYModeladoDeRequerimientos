using SistemaHotelColibri.Abstract.Comun;

namespace SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Usuarios;

public interface IRestablecerContrasenaUsuarioBL
{
    Task<ResultadoOperacion> RestablecerAsync(int idUsuario);
}
