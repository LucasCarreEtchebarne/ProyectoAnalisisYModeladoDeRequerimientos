using SistemaHotelColibri.DataAccess.Identidad;

namespace SistemaHotelColibri.DataAccess.Usuarios;

internal sealed class UsuarioConRol
{
    public Usuario Usuario { get; init; } = null!;
    public string? Rol { get; init; }
}

internal static class UsuarioConsultas
{
    public static IQueryable<UsuarioConRol> ConRol(this IdentidadContext contexto) =>
        from u in contexto.Users
        select new UsuarioConRol
        {
            Usuario = u,
            Rol = (from ur in contexto.UserRoles
                   join r in contexto.Roles on ur.RoleId equals r.Id
                   where ur.UserId == u.Id
                   select r.Name).FirstOrDefault()
        };
}
