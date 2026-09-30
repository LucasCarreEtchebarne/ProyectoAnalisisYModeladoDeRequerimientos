using Microsoft.AspNetCore.Identity;

namespace SistemaHotelColibri.DataAccess.Identidad;

public class Usuario : IdentityUser<int>
{
    public string NombreCompleto { get; set; } = string.Empty;

    public string Estado { get; set; } = "Activo";

    public DateTime FechaCreacion { get; set; }

    public DateTime? FechaUltimoAcceso { get; set; }
}
