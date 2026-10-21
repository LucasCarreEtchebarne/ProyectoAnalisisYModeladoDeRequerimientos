using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Usuarios;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Usuarios;
using SistemaHotelColibri.Abstract.Modelos.Usuarios;

namespace SistemaHotelColibri.BusinessLogic.Usuarios;

public class IniciarSesionBL : IIniciarSesionBL
{
    private readonly IIniciarSesionDA _iniciarSesionDA;

    public IniciarSesionBL(IIniciarSesionDA iniciarSesionDA)
    {
        _iniciarSesionDA = iniciarSesionDA;
    }

    public Task<ResultadoInicioSesion> IniciarSesionAsync(string usuarioOCorreo, string contrasena, bool recordar)
    {
        var identificador = usuarioOCorreo.Trim();
        if (identificador.Length == 0 || string.IsNullOrEmpty(contrasena))
        {
            return Task.FromResult(ResultadoInicioSesion.CredencialesInvalidas);
        }

        return _iniciarSesionDA.IniciarSesionAsync(identificador, contrasena, recordar);
    }
}
