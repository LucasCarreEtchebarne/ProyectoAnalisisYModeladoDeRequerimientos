using Microsoft.EntityFrameworkCore;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Usuarios;
using SistemaHotelColibri.Abstract.Modelos.Usuarios;
using SistemaHotelColibri.DataAccess.Identidad;

namespace SistemaHotelColibri.DataAccess.Usuarios;

public class ObtenerUsuarioPorIdDA : IObtenerUsuarioPorIdDA
{
    private readonly IdentidadContext _contexto;

    public ObtenerUsuarioPorIdDA(IdentidadContext contexto)
    {
        _contexto = contexto;
    }

    public async Task<UsuarioDto?> ObtenerPorIdAsync(int idUsuario)
    {
        var resultado = await _contexto.ConRol()
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Usuario.Id == idUsuario);

        return resultado?.Usuario.ADto(resultado.Rol);
    }
}
