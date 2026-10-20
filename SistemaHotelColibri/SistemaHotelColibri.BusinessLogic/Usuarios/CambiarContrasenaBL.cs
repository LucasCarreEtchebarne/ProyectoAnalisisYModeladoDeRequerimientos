using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Usuarios;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Usuarios;

namespace SistemaHotelColibri.BusinessLogic.Usuarios;

public class CambiarContrasenaBL : ICambiarContrasenaBL
{
    private readonly ICambiarContrasenaDA _cambiarDA;

    public CambiarContrasenaBL(ICambiarContrasenaDA cambiarDA)
    {
        _cambiarDA = cambiarDA;
    }

    public async Task<ResultadoOperacion> CambiarAsync(int idUsuario, string contrasenaActual, string contrasenaNueva)
    {
        if (contrasenaActual == contrasenaNueva)
        {
            return ResultadoOperacion.Error(Mensajes.ContrasenaIgualActual);
        }

        var resultado = await _cambiarDA.CambiarAsync(idUsuario, contrasenaActual, contrasenaNueva);
        return resultado.Exito ? ResultadoOperacion.Ok(Mensajes.ContrasenaCambiada, idUsuario) : resultado;
    }
}
