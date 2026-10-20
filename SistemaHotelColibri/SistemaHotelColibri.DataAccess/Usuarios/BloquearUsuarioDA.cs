using Microsoft.AspNetCore.Identity;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Usuarios;
using SistemaHotelColibri.DataAccess.Identidad;

namespace SistemaHotelColibri.DataAccess.Usuarios;

public class BloquearUsuarioDA : IBloquearUsuarioDA
{
    private readonly UserManager<Usuario> _gestorUsuarios;

    public BloquearUsuarioDA(UserManager<Usuario> gestorUsuarios)
    {
        _gestorUsuarios = gestorUsuarios;
    }

    public async Task<bool> BloquearAsync(int idUsuario)
    {
        var usuario = await _gestorUsuarios.FindByIdAsync(idUsuario.ToString());
        if (usuario == null)
        {
            return false;
        }

        await _gestorUsuarios.SetLockoutEnabledAsync(usuario, true);
        var resultado = await _gestorUsuarios.SetLockoutEndDateAsync(usuario, DateTimeOffset.MaxValue);
        if (!resultado.Succeeded)
        {
            return false;
        }

        await _gestorUsuarios.UpdateSecurityStampAsync(usuario);
        return true;
    }

    public async Task<bool> DesbloquearAsync(int idUsuario)
    {
        var usuario = await _gestorUsuarios.FindByIdAsync(idUsuario.ToString());
        if (usuario == null)
        {
            return false;
        }

        var resultado = await _gestorUsuarios.SetLockoutEndDateAsync(usuario, null);
        if (!resultado.Succeeded)
        {
            return false;
        }

        await _gestorUsuarios.ResetAccessFailedCountAsync(usuario);
        return true;
    }
}
