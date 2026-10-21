namespace SistemaHotelColibri.Abstract.Modelos.Usuarios;

public class SolicitudRecuperacionDto
{
    public string NombreCompleto { get; set; } = string.Empty;
    public string CorreoElectronico { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
}
