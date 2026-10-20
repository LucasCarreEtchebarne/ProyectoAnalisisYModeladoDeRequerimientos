using Microsoft.AspNetCore.Identity;
using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Usuarios;
using SistemaHotelColibri.DataAccess.Identidad;

namespace SistemaHotelColibri.DataAccess.Usuarios;

public class CambiarContrasenaDA : ICambiarContrasenaDA
{
    private readonly UserManager<Usuario> _gestorUsuarios;
    private readonly SignInManager<Usuario> _gestorSesion;

    public CambiarContrasenaDA(UserManager<Usuario> gestorUsuarios, SignInManager<Usuario> gestorSesion)
    {
        _gestorUsuarios = gestorUsuarios;
        _gestorSesion = gestorSesion;
    }

    public async Task<ResultadoOperacion> CambiarAsync(int idUsuario, string contrasenaActual, string contrasenaNueva)
    {
        var usuario = await _gestorUsuarios.FindByIdAsync(idUsuario.ToString());
        if (usuario == null)
        {
            return ResultadoOperacion.Error(Mensajes.NoEncontrado);
        }

        var resultado = await _gestorUsuarios.ChangePasswordAsync(usuario, contrasenaActual, contrasenaNueva);
        if (!resultado.Succeeded)
        {
            return resultado.AResultado(string.Empty);
        }

        await _gestorUsuarios.QuitarCambioContrasenaAsync(usuario);
        await _gestorSesion.RefreshSignInAsync(usuario);
        return ResultadoOperacion.Ok(string.Empty, usuario.Id);
    }
}
