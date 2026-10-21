using Microsoft.EntityFrameworkCore;
using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Usuarios;
using SistemaHotelColibri.Abstract.Modelos.Usuarios;
using SistemaHotelColibri.DataAccess.Identidad;

namespace SistemaHotelColibri.DataAccess.Usuarios;

public class ObtenerRolDA : IObtenerRolDA
{
    private readonly IdentidadContext _contexto;

    public ObtenerRolDA(IdentidadContext contexto)
    {
        _contexto = contexto;
    }

    public async Task<List<RolDto>> ObtenerActivosAsync()
    {
        return await _contexto.Roles
            .AsNoTracking()
            .Where(r => r.EstadoRol == Estados.Rol.Activo)
            .OrderBy(r => r.Name)
            .Select(r => new RolDto { IdRol = r.Id, Nombre = r.Name ?? string.Empty, Descripcion = r.Descripcion })
            .ToListAsync();
    }
}
