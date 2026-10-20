using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Usuarios;
using SistemaHotelColibri.DataAccess.Identidad;

namespace SistemaHotelColibri.DataAccess.Usuarios;

public class ExisteNombreUsuarioDA : IExisteNombreUsuarioDA
{
    private readonly IdentidadContext _contexto;
    private readonly ILookupNormalizer _normalizador;

    public ExisteNombreUsuarioDA(IdentidadContext contexto, ILookupNormalizer normalizador)
    {
        _contexto = contexto;
        _normalizador = normalizador;
    }

    public async Task<bool> ExisteNombreUsuarioAsync(string nombreUsuario, int idExcluir)
    {
        var normalizado = _normalizador.NormalizeName(nombreUsuario);
        return await _contexto.Users.AnyAsync(u => u.NormalizedUserName == normalizado && u.Id != idExcluir);
    }
}
