using SistemaHotelColibri.Abstract.Modelos.Usuarios;
using SistemaHotelColibri.DataAccess.Identidad;

namespace SistemaHotelColibri.DataAccess.Usuarios;

public static class UsuarioMapeo
{
    public static UsuarioDto ADto(this Usuario entidad, string? rol) => new()
    {
        IdUsuario = entidad.Id,
        NombreCompleto = entidad.NombreCompleto,
        NombreUsuario = entidad.UserName ?? string.Empty,
        CorreoElectronico = entidad.Email ?? string.Empty,
        Rol = rol ?? string.Empty,
        Estado = entidad.Estado,
        BloqueadoHasta = entidad.LockoutEnd,
        IntentosFallidos = entidad.AccessFailedCount,
        FechaCreacion = entidad.FechaCreacion,
        FechaUltimoAcceso = entidad.FechaUltimoAcceso
    };
}
