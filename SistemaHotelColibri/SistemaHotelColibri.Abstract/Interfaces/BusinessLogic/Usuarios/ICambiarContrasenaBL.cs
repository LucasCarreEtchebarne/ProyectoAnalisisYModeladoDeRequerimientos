using SistemaHotelColibri.Abstract.Comun;

namespace SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Usuarios;

public interface ICambiarContrasenaBL
{
    Task<ResultadoOperacion> CambiarAsync(int idUsuario, string contrasenaActual, string contrasenaNueva);
}
