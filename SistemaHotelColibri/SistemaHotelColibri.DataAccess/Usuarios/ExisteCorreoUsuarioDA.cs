using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Usuarios;
using SistemaHotelColibri.DataAccess.Identidad;

namespace SistemaHotelColibri.DataAccess.Usuarios;

public class ExisteCorreoUsuarioDA : IExisteCorreoUsuarioDA
{
    private readonly IdentidadContext _contexto;
    private readonly ILookupNormalizer _normalizador;

    public ExisteCorreoUsuarioDA(IdentidadContext contexto, ILookupNormalizer normalizador)
    {
        _contexto = contexto;
        _normalizador = normalizador;
    }

    public async Task<bool> ExisteCorreoAsync(string correoElectronico, int idExcluir)
    {
        var normalizado = _normalizador.NormalizeEmail(correoElectronico);
        return await _contexto.Users.AnyAsync(u => u.NormalizedEmail == normalizado && u.Id != idExcluir);
    }
}
