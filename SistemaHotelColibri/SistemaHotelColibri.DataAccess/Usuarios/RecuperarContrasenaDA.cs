using Microsoft.AspNetCore.Identity;
using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Usuarios;
using SistemaHotelColibri.Abstract.Modelos.Usuarios;
using SistemaHotelColibri.DataAccess.Identidad;

namespace SistemaHotelColibri.DataAccess.Usuarios;

public class RecuperarContrasenaDA : IRecuperarContrasenaDA
{
    private readonly UserManager<Usuario> _gestorUsuarios;

    public RecuperarContrasenaDA(UserManager<Usuario> gestorUsuarios)
    {
        _gestorUsuarios = gestorUsuarios;
    }

    public async Task<SolicitudRecuperacionDto?> GenerarTokenAsync(string correoElectronico)
    {
        var usuario = await _gestorUsuarios.FindByEmailAsync(correoElectronico);
        if (usuario == null || usuario.Estado != Estados.Usuario.Activo)
        {
            return null;
        }

        return new SolicitudRecuperacionDto
        {
            NombreCompleto = usuario.NombreCompleto,
            CorreoElectronico = usuario.Email ?? correoElectronico,
            Token = await _gestorUsuarios.GeneratePasswordResetTokenAsync(usuario)
        };
    }

    public async Task<ResultadoOperacion> RestablecerAsync(string correoElectronico, string token, string contrasenaNueva)
    {
        var usuario = await _gestorUsuarios.FindByEmailAsync(correoElectronico);
        if (usuario == null || usuario.Estado != Estados.Usuario.Activo)
        {
            return ResultadoOperacion.Error(Mensajes.EnlaceRecuperacionInvalido);
        }

        var resultado = await _gestorUsuarios.ResetPasswordAsync(usuario, token, contrasenaNueva);
        if (!resultado.Succeeded)
        {
            return resultado.Errors.Any(e => e.Code == nameof(IdentityErrorDescriber.InvalidToken))
                ? ResultadoOperacion.Error(Mensajes.EnlaceRecuperacionInvalido)
                : resultado.AResultado(string.Empty);
        }

        await _gestorUsuarios.QuitarCambioContrasenaAsync(usuario);
        await _gestorUsuarios.ResetAccessFailedCountAsync(usuario);
        if (usuario.LockoutEnd.HasValue && usuario.LockoutEnd.Value.Year < 9000)
        {
            await _gestorUsuarios.SetLockoutEndDateAsync(usuario, null);
        }
        return ResultadoOperacion.Ok(string.Empty, usuario.Id);
    }
}
