using System.Net;
using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Usuarios;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Correo;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Usuarios;

namespace SistemaHotelColibri.BusinessLogic.Usuarios;

public class RecuperarContrasenaBL : IRecuperarContrasenaBL
{
    private readonly IRecuperarContrasenaDA _recuperarDA;
    private readonly IEnviarCorreoDA _enviarCorreoDA;

    public RecuperarContrasenaBL(IRecuperarContrasenaDA recuperarDA, IEnviarCorreoDA enviarCorreoDA)
    {
        _recuperarDA = recuperarDA;
        _enviarCorreoDA = enviarCorreoDA;
    }

    public async Task SolicitarAsync(string correoElectronico, string urlRestablecer)
    {
        var correo = correoElectronico.Trim();
        if (correo.Length == 0)
        {
            return;
        }

        var solicitud = await _recuperarDA.GenerarTokenAsync(correo);
        if (solicitud == null)
        {
            return;
        }

        var enlace = $"{urlRestablecer}?correo={Uri.EscapeDataString(solicitud.CorreoElectronico)}&token={Uri.EscapeDataString(solicitud.Token)}";
        var cuerpo = $"""
            <p>Hola {WebUtility.HtmlEncode(solicitud.NombreCompleto)}:</p>
            <p>Recibimos una solicitud para restablecer su contraseña del sistema del Hotel El Colibrí.</p>
            <p><a href="{WebUtility.HtmlEncode(enlace)}">Restablecer contraseña</a></p>
            <p>El enlace vence en 2 horas. Si usted no hizo esta solicitud, puede ignorar este correo.</p>
            """;

        await _enviarCorreoDA.EnviarAsync(solicitud.CorreoElectronico, "Restablecer contraseña - Hotel El Colibrí", cuerpo);
    }

    public async Task<ResultadoOperacion> RestablecerAsync(string correoElectronico, string token, string contrasenaNueva)
    {
        if (string.IsNullOrWhiteSpace(correoElectronico) || string.IsNullOrWhiteSpace(token))
        {
            return ResultadoOperacion.Error(Mensajes.EnlaceRecuperacionInvalido);
        }

        var resultado = await _recuperarDA.RestablecerAsync(correoElectronico.Trim(), token, contrasenaNueva);
        return resultado.Exito ? ResultadoOperacion.Ok(Mensajes.ContrasenaRestablecida) : resultado;
    }
}
