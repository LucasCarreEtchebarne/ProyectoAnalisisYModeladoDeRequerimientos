using Microsoft.AspNetCore.Identity;
using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Usuarios;
using SistemaHotelColibri.DataAccess.Identidad;

namespace SistemaHotelColibri.DataAccess.Usuarios;

public class ActivarUsuarioDA : IActivarUsuarioDA
{
    private readonly UserManager<Usuario> _gestorUsuarios;

    public ActivarUsuarioDA(UserManager<Usuario> gestorUsuarios)
    {
        _gestorUsuarios = gestorUsuarios;
    }

    public async Task<bool> ActivarAsync(int idUsuario)
    {
        var usuario = await _gestorUsuarios.FindByIdAsync(idUsuario.ToString());
        if (usuario == null)
        {
            return false;
        }

        usuario.Estado = Estados.Usuario.Activo;
        return (await _gestorUsuarios.UpdateAsync(usuario)).Succeeded;
    }
}
