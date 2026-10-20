using Microsoft.AspNetCore.Identity;
using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Usuarios;
using SistemaHotelColibri.DataAccess.Identidad;

namespace SistemaHotelColibri.DataAccess.Usuarios;

public class RestablecerContrasenaUsuarioDA : IRestablecerContrasenaUsuarioDA
{
    private readonly UserManager<Usuario> _gestorUsuarios;

    public RestablecerContrasenaUsuarioDA(UserManager<Usuario> gestorUsuarios)
    {
        _gestorUsuarios = gestorUsuarios;
    }

    public async Task<ResultadoOperacion> EstablecerContrasenaTemporalAsync(int idUsuario, string contrasenaTemporal)
    {
        var usuario = await _gestorUsuarios.FindByIdAsync(idUsuario.ToString());
        if (usuario == null)
        {
            return ResultadoOperacion.Error(Mensajes.NoEncontrado);
        }

        var token = await _gestorUsuarios.GeneratePasswordResetTokenAsync(usuario);
        var resultado = await _gestorUsuarios.ResetPasswordAsync(usuario, token, contrasenaTemporal);
        if (!resultado.Succeeded)
        {
            return resultado.AResultado(string.Empty);
        }

        await _gestorUsuarios.MarcarCambioContrasenaAsync(usuario);
        return ResultadoOperacion.Ok(string.Empty, usuario.Id);
    }
}
