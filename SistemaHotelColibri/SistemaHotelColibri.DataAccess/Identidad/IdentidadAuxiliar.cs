using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using SistemaHotelColibri.Abstract.Comun;

namespace SistemaHotelColibri.DataAccess.Identidad;

internal static class IdentidadAuxiliar
{
    public static ResultadoOperacion AResultado(this IdentityResult resultado, string mensajeExito) =>
        resultado.Succeeded
            ? ResultadoOperacion.Ok(mensajeExito)
            : ResultadoOperacion.Error(string.Join(" ", resultado.Errors.Select(e => e.Description)));

    public static async Task<bool> DebeCambiarContrasenaAsync(this UserManager<Usuario> gestor, Usuario usuario) =>
        (await gestor.GetClaimsAsync(usuario)).Any(c => c.Type == Reclamos.DebeCambiarContrasena);

    public static async Task MarcarCambioContrasenaAsync(this UserManager<Usuario> gestor, Usuario usuario)
    {
        if (!await gestor.DebeCambiarContrasenaAsync(usuario))
        {
            await gestor.AddClaimAsync(usuario, new Claim(Reclamos.DebeCambiarContrasena, "true"));
        }
    }

    public static async Task QuitarCambioContrasenaAsync(this UserManager<Usuario> gestor, Usuario usuario)
    {
        var reclamos = (await gestor.GetClaimsAsync(usuario))
            .Where(c => c.Type == Reclamos.DebeCambiarContrasena)
            .ToList();

        if (reclamos.Count > 0)
        {
            await gestor.RemoveClaimsAsync(usuario, reclamos);
        }
    }
}
