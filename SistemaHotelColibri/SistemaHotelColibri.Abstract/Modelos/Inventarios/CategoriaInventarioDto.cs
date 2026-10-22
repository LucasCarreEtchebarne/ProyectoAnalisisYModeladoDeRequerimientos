namespace SistemaHotelColibri.Abstract.Modelos.Inventarios;

public class CategoriaInventarioDto
{
    public int IdCategoria { get; set; }
    public string NombreCategoria { get; set; } = string.Empty;
    public string Area { get; set; } = string.Empty;
    public string EstadoCategoria { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
}
