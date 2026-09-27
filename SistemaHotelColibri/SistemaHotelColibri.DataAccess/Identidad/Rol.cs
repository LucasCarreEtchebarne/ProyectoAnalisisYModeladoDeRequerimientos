using Microsoft.AspNetCore.Identity;

namespace SistemaHotelColibri.DataAccess.Identidad;

public class Rol : IdentityRole<int>
{
    public string? Descripcion { get; set; }

    public string EstadoRol { get; set; } = "Activo";
}
