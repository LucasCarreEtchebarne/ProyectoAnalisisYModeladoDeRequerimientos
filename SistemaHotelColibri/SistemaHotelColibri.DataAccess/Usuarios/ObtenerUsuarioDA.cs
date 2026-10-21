using Microsoft.EntityFrameworkCore;
using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Usuarios;
using SistemaHotelColibri.Abstract.Modelos.Usuarios;
using SistemaHotelColibri.DataAccess.Comun;
using SistemaHotelColibri.DataAccess.Identidad;

namespace SistemaHotelColibri.DataAccess.Usuarios;

public class ObtenerUsuarioDA : IObtenerUsuarioDA
{
    private readonly IdentidadContext _contexto;

    public ObtenerUsuarioDA(IdentidadContext contexto)
    {
        _contexto = contexto;
    }

    public async Task<ResultadoPaginado<UsuarioDto>> ObtenerAsync(FiltroUsuario filtro)
    {
        var ahora = DateTimeOffset.UtcNow;
        var consulta = _contexto.ConRol().AsNoTracking();

        consulta = filtro.Estado switch
        {
            Estados.Usuario.Activo => consulta.Where(x => x.Usuario.Estado == Estados.Usuario.Activo
                                                       && (x.Usuario.LockoutEnd == null || x.Usuario.LockoutEnd <= ahora)),
            Estados.Usuario.Bloqueado => consulta.Where(x => x.Usuario.Estado == Estados.Usuario.Activo
                                                          && x.Usuario.LockoutEnd > ahora),
            Estados.Usuario.Inactivo => consulta.Where(x => x.Usuario.Estado == Estados.Usuario.Inactivo),
            _ => consulta
        };

        if (!string.IsNullOrWhiteSpace(filtro.Rol))
        {
            consulta = consulta.Where(x => x.Rol == filtro.Rol);
        }

        if (!string.IsNullOrWhiteSpace(filtro.Busqueda))
        {
            var texto = filtro.Busqueda;
            consulta = consulta.Where(x => x.Usuario.NombreCompleto.Contains(texto)
                                        || (x.Usuario.UserName != null && x.Usuario.UserName.Contains(texto))
                                        || (x.Usuario.Email != null && x.Usuario.Email.Contains(texto)));
        }

        var pagina = await consulta
            .OrderBy(x => x.Usuario.NombreCompleto)
            .PaginarAsync(filtro.Pagina, filtro.TamanoPagina);

        return pagina.Mapear(x => x.Usuario.ADto(x.Rol));
    }
}
