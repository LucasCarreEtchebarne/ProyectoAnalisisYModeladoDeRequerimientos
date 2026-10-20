using Microsoft.AspNetCore.Identity;
using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Usuarios;
using SistemaHotelColibri.Abstract.Modelos.Usuarios;
using SistemaHotelColibri.DataAccess.Identidad;

namespace SistemaHotelColibri.DataAccess.Usuarios;

public class IniciarSesionDA : IIniciarSesionDA
{
    private readonly UserManager<Usuario> _gestorUsuarios;
    private readonly SignInManager<Usuario> _gestorSesion;

    public IniciarSesionDA(UserManager<Usuario> gestorUsuarios, SignInManager<Usuario> gestorSesion)
    {
        _gestorUsuarios = gestorUsuarios;
        _gestorSesion = gestorSesion;
    }

    public async Task<ResultadoInicioSesion> IniciarSesionAsync(string usuarioOCorreo, string contrasena, bool recordar)
    {
        var usuario = usuarioOCorreo.Contains('@')
            ? await _gestorUsuarios.FindByEmailAsync(usuarioOCorreo)
            : await _gestorUsuarios.FindByNameAsync(usuarioOCorreo);

        if (usuario == null)
        {
            return ResultadoInicioSesion.CredencialesInvalidas;
        }

        var verificacion = await _gestorSesion.CheckPasswordSignInAsync(usuario, contrasena, lockoutOnFailure: true);
        if (verificacion.IsLockedOut)
        {
            return ResultadoInicioSesion.Bloqueado;
        }

        if (!verificacion.Succeeded)
        {
            return ResultadoInicioSesion.CredencialesInvalidas;
        }

        if (usuario.Estado != Estados.Usuario.Activo)
        {
            return ResultadoInicioSesion.Inactivo;
        }

        usuario.FechaUltimoAcceso = DateTime.Now;
        await _gestorUsuarios.UpdateAsync(usuario);
        await _gestorSesion.SignInAsync(usuario, recordar);

        return await _gestorUsuarios.DebeCambiarContrasenaAsync(usuario)
            ? ResultadoInicioSesion.DebeCambiarContrasena
            : ResultadoInicioSesion.Exitoso;
    }
}
