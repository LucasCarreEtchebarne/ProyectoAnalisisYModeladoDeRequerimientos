using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace SistemaHotelColibri.DataAccess.Identidad;

public static class SembradorUsuariosDesarrollo
{
    public static async Task SembrarContrasenasAsync(this IServiceProvider servicios, string contrasena)
    {
        using var alcance = servicios.CreateScope();
        var gestorUsuarios = alcance.ServiceProvider.GetRequiredService<UserManager<Usuario>>();
        var registro = alcance.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger(nameof(SembradorUsuariosDesarrollo));

        var sinContrasena = await gestorUsuarios.Users.Where(u => u.PasswordHash == null).ToListAsync();
        foreach (var usuario in sinContrasena)
        {
            var resultado = await gestorUsuarios.AddPasswordAsync(usuario, contrasena);
            if (resultado.Succeeded)
            {
                registro.LogInformation("Contraseña de desarrollo asignada a {Usuario}.", usuario.UserName);
            }
            else
            {
                registro.LogWarning("No se pudo asignar la contraseña de desarrollo a {Usuario}: {Errores}",
                    usuario.UserName, string.Join(" ", resultado.Errors.Select(e => e.Description)));
            }
        }
    }
}
