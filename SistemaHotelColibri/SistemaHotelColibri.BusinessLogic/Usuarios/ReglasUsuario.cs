using System.Text.RegularExpressions;
using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Modelos.Usuarios;

namespace SistemaHotelColibri.BusinessLogic.Usuarios;

internal static partial class ReglasUsuario
{
    public static void Normalizar(UsuarioDto usuario)
    {
        usuario.NombreCompleto = usuario.NombreCompleto.Trim();
        usuario.NombreUsuario = usuario.NombreUsuario.Trim();
        usuario.CorreoElectronico = usuario.CorreoElectronico.Trim().ToLowerInvariant();
    }

    public static string? Validar(UsuarioDto usuario, IEnumerable<string> rolesActivos)
    {
        if (!NombreUsuarioValido().IsMatch(usuario.NombreUsuario))
        {
            return Mensajes.NombreUsuarioInvalido;
        }

        if (!rolesActivos.Contains(usuario.Rol))
        {
            return Mensajes.RolInvalido;
        }

        return null;
    }

    [GeneratedRegex(@"^[A-Za-z0-9._-]+$")]
    private static partial Regex NombreUsuarioValido();
}
