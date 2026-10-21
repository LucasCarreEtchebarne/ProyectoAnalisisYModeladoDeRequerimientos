using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using SistemaHotelColibri.Abstract.Comun;

namespace SistemaHotelColibri.DataAccess.Identidad;

public class FabricaReclamosUsuario : UserClaimsPrincipalFactory<Usuario, Rol>
{
    public FabricaReclamosUsuario(UserManager<Usuario> gestorUsuarios, RoleManager<Rol> gestorRoles, IOptions<IdentityOptions> opciones)
        : base(gestorUsuarios, gestorRoles, opciones)
    {
    }

    protected override async Task<ClaimsIdentity> GenerateClaimsAsync(Usuario usuario)
    {
        var identidad = await base.GenerateClaimsAsync(usuario);
        identidad.AddClaim(new Claim(Reclamos.NombreCompleto, usuario.NombreCompleto));
        return identidad;
    }
}
