using SistemaHotelColibri.Abstract.Comun;

namespace SistemaHotelColibri.Abstract.Modelos.Inventarios;

public class InventarioDto
{
    public int IdProducto { get; set; }
    public string NombreProducto { get; set; } = string.Empty;
    public int IdCategoria { get; set; }
    public string NombreCategoria { get; set; } = string.Empty;
    public string AreaCategoria { get; set; } = string.Empty;
    public int? IdProveedor { get; set; }
    public string? NombreProveedor { get; set; }
    public string UnidadMedida { get; set; } = string.Empty;
    public decimal Stock { get; set; }
    public decimal StockMinimo { get; set; }
    public DateOnly FechaIngreso { get; set; }
    public string EstadoProducto { get; set; } = string.Empty;
    public string? Descripcion { get; set; }

    public bool RequiereAlerta => EstadoProducto == Estados.ProductoInventario.Activo && Stock <= StockMinimo;
}
