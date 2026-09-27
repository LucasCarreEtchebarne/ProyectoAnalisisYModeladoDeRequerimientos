namespace SistemaHotelColibri.Abstract.Modelos.Inventarios;

public class InventarioDto
{
    public int IdProducto { get; set; }
    public string NombreProducto { get; set; } = string.Empty;
    public string CategoriaProducto { get; set; } = string.Empty;
    public string UnidadMedida { get; set; } = string.Empty;
    public decimal Stock { get; set; }
    public decimal StockMinimo { get; set; }
    public DateOnly FechaIngreso { get; set; }
    public string EstadoProducto { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
}
