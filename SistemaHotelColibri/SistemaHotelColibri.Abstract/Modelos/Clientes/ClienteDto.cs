namespace SistemaHotelColibri.Abstract.Modelos.Clientes;

public class ClienteDto
{
    public int IdCliente { get; set; }
    public string Identificacion { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string PrimerApellido { get; set; } = string.Empty;
    public string? SegundoApellido { get; set; }
    public string? Telefono { get; set; }
    public string? CorreoElectronico { get; set; }
    public string? Direccion { get; set; }
    public DateTime FechaRegistro { get; set; }
    public string EstadoCliente { get; set; } = string.Empty;

    public string NombreMostrar => $"{Nombre} {PrimerApellido} {SegundoApellido}".Trim();
}