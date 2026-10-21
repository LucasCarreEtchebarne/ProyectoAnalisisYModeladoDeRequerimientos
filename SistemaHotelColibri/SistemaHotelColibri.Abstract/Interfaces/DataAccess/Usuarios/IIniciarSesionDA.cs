using SistemaHotelColibri.Abstract.Modelos.Usuarios;

namespace SistemaHotelColibri.Abstract.Interfaces.DataAccess.Usuarios;

public interface IIniciarSesionDA
{
    Task<ResultadoInicioSesion> IniciarSesionAsync(string usuarioOCorreo, string contrasena, bool recordar);
}
