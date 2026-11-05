using Microsoft.EntityFrameworkCore;
using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.TareasHousekeeping;
using SistemaHotelColibri.Abstract.Modelos.TareasHousekeeping;
using SistemaHotelColibri.DataAccess.Identidad;

namespace SistemaHotelColibri.DataAccess.TareasHousekeeping;

public class ObtenerPersonalHousekeepingDA : IObtenerPersonalHousekeepingDA
{

    private readonly IdentidadContext _identidad;

    public ObtenerPersonalHousekeepingDA(IdentidadContext identidad)
    {
        _identidad = identidad;
    }

    public async Task<List<PersonalHousekeepingDto>> ObtenerAsync()
    {
        return await (
                from usuario in _identidad.Users.AsNoTracking()
                join usuarioRol in _identidad.UserRoles on usuario.Id equals usuarioRol.UserId
                join rol in _identidad.Roles on usuarioRol.RoleId equals rol.Id
                where rol.Name == Roles.Housekeeping && usuario.Estado == Estados.Usuario.Activo
                orderby usuario.NombreCompleto
                select new PersonalHousekeepingDto
                {
                    IdUsuario = usuario.Id,
                    NombreCompleto = usuario.NombreCompleto
                })
            .ToListAsync();
    }
}