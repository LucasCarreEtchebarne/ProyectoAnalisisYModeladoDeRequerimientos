using Microsoft.AspNetCore.Identity;
using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Usuarios;
using SistemaHotelColibri.Abstract.Modelos.Usuarios;
using SistemaHotelColibri.DataAccess.Identidad;

namespace SistemaHotelColibri.DataAccess.Usuarios;

public class EditarUsuarioDA : IEditarUsuarioDA
{
    private readonly UserManager<Usuario> _gestorUsuarios;
    private readonly IdentidadContext _contexto;

    public EditarUsuarioDA(UserManager<Usuario> gestorUsuarios, IdentidadContext contexto)
    {
        _gestorUsuarios = gestorUsuarios;
        _contexto = contexto;
    }

    public async Task<ResultadoOperacion> EditarAsync(UsuarioDto dto)
    {
        var usuario = await _gestorUsuarios.FindByIdAsync(dto.IdUsuario.ToString());
        if (usuario == null)
        {
            return ResultadoOperacion.Error(Mensajes.NoEncontrado);
        }

        await using var transaccion = await _contexto.Database.BeginTransactionAsync();

        usuario.NombreCompleto = dto.NombreCompleto;
        usuario.UserName = dto.NombreUsuario;
        usuario.Email = dto.CorreoElectronico;

        var resultado = await _gestorUsuarios.UpdateAsync(usuario);
        if (!resultado.Succeeded)
        {
            return resultado.AResultado(string.Empty);
        }

        var rolesActuales = await _gestorUsuarios.GetRolesAsync(usuario);
        if (!rolesActuales.SequenceEqual([dto.Rol]))
        {
            resultado = await _gestorUsuarios.RemoveFromRolesAsync(usuario, rolesActuales);
            if (resultado.Succeeded)
            {
                resultado = await _gestorUsuarios.AddToRoleAsync(usuario, dto.Rol);
            }

            if (!resultado.Succeeded)
            {
                return resultado.AResultado(string.Empty);
            }

            await _gestorUsuarios.UpdateSecurityStampAsync(usuario);
        }

        await transaccion.CommitAsync();
        return ResultadoOperacion.Ok(string.Empty, usuario.Id);
    }
}
