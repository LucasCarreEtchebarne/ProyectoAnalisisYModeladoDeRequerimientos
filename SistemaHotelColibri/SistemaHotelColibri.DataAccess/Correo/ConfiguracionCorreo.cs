namespace SistemaHotelColibri.DataAccess.Correo;

public class ConfiguracionCorreo
{
    public const string Seccion = "Correo";

    public string? Servidor { get; set; }
    public int Puerto { get; set; } = 587;
    public bool UsarSsl { get; set; } = true;
    public string? Usuario { get; set; }
    public string? Contrasena { get; set; }
    public string? Remitente { get; set; }
    public string NombreRemitente { get; set; } = "Hotel El Colibrí";

    public bool EstaConfigurado => !string.IsNullOrWhiteSpace(Servidor) && !string.IsNullOrWhiteSpace(Remitente);
}
