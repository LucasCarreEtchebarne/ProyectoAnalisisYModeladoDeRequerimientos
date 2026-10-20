using SistemaHotelColibri.Abstract.Comun;

namespace SistemaHotelColibri.Abstract.Modelos.Usuarios;

public class UsuarioDto
{
    public int IdUsuario { get; set; }
    public string NombreCompleto { get; set; } = string.Empty;
    public string NombreUsuario { get; set; } = string.Empty;
    public string CorreoElectronico { get; set; } = string.Empty;
    public string Rol { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
    public DateTimeOffset? BloqueadoHasta { get; set; }
    public int IntentosFallidos { get; set; }
    public DateTime FechaCreacion { get; set; }
    public DateTime? FechaUltimoAcceso { get; set; }

    public bool EstaBloqueado => BloqueadoHasta.HasValue && BloqueadoHasta.Value > DateTimeOffset.Now;

    public string EstadoMostrado => Estado == Estados.Usuario.Activo && EstaBloqueado ? Estados.Usuario.Bloqueado : Estado;
}
