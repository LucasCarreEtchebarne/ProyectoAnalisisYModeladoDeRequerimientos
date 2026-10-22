using SistemaHotelColibri.Abstract.Modelos.Usuarios;

namespace SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Usuarios;

public interface IIniciarSesionBL
{
    Task<ResultadoInicioSesion> IniciarSesionAsync(string usuarioOCorreo, string contrasena, bool recordar);
}
