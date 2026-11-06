namespace SistemaHotelColibri.Abstract.Modelos.Menus;

public class MenuDto
{
    public int IdProductoMenu { get; set; }
    public string NombreProducto { get; set; } = string.Empty;
    public string CategoriaMenu { get; set; } = string.Empty;
    public decimal Precio { get; set; }
    public string EstadoProductoMenu { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
}
