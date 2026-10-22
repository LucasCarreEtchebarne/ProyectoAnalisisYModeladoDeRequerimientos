using SistemaHotelColibri.Abstract.Comun;

namespace SistemaHotelColibri.Abstract.Interfaces.DataAccess.Usuarios;

public interface ICambiarContrasenaDA
{
    Task<ResultadoOperacion> CambiarAsync(int idUsuario, string contrasenaActual, string contrasenaNueva);
}
