using Microsoft.AspNetCore.Identity;
using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Usuarios;
using SistemaHotelColibri.Abstract.Modelos.Usuarios;
using SistemaHotelColibri.DataAccess.Identidad;

namespace SistemaHotelColibri.DataAccess.Usuarios;

public class AgregarUsuarioDA : IAgregarUsuarioDA
{
    private readonly UserManager<Usuario> _gestorUsuarios;
    private readonly IdentidadContext _contexto;

    public AgregarUsuarioDA(UserManager<Usuario> gestorUsuarios, IdentidadContext contexto)
    {
        _gestorUsuarios = gestorUsuarios;
        _contexto = contexto;
    }

    public async Task<ResultadoOperacion> AgregarAsync(UsuarioDto dto, string contrasena)
    {
        var usuario = new Usuario
        {
            NombreCompleto = dto.NombreCompleto,
            UserName = dto.NombreUsuario,
            Email = dto.CorreoElectronico,
            EmailConfirmed = true,
            Estado = Estados.Usuario.Activo,
            FechaCreacion = DateTime.Now,
            LockoutEnabled = true
        };

        await using var transaccion = await _contexto.Database.BeginTransactionAsync();

        var resultado = await _gestorUsuarios.CreateAsync(usuario, contrasena);
        if (!resultado.Succeeded)
        {
            return resultado.AResultado(string.Empty);
        }

        resultado = await _gestorUsuarios.AddToRoleAsync(usuario, dto.Rol);
        if (!resultado.Succeeded)
        {
            return resultado.AResultado(string.Empty);
        }

        await _gestorUsuarios.MarcarCambioContrasenaAsync(usuario);
        await transaccion.CommitAsync();

        return ResultadoOperacion.Ok(string.Empty, usuario.Id);
    }
}
