using Microsoft.AspNetCore.Identity;
using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Usuarios;
using SistemaHotelColibri.DataAccess.Identidad;

namespace SistemaHotelColibri.DataAccess.Usuarios;

public class EliminarUsuarioDA : IEliminarUsuarioDA
{
    private readonly UserManager<Usuario> _gestorUsuarios;

    public EliminarUsuarioDA(UserManager<Usuario> gestorUsuarios)
    {
        _gestorUsuarios = gestorUsuarios;
    }

    public async Task<bool> EliminarAsync(int idUsuario)
    {
        var usuario = await _gestorUsuarios.FindByIdAsync(idUsuario.ToString());
        if (usuario == null)
        {
            return false;
        }

        usuario.Estado = Estados.Usuario.Inactivo;
        var resultado = await _gestorUsuarios.UpdateAsync(usuario);
        if (!resultado.Succeeded)
        {
            return false;
        }

        await _gestorUsuarios.UpdateSecurityStampAsync(usuario);
        return true;
    }
}
