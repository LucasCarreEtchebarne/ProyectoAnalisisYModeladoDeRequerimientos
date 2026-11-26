using SistemaHotelColibri.Abstract.Modelos.Inventarios;

namespace SistemaHotelColibri.Abstract.Modelos.Reportes;

// REP-004: Reporte de inventario (existencias y movimientos del período).
public class ReporteInventarioDto
{
    public const string EstadoAgotado = "Agotado";
    public const string EstadoBajo = "Stock bajo";
    public const string EstadoNormal = "Normal";

    public DateOnly Desde { get; set; }
    public DateOnly Hasta { get; set; }
    public int TotalProductos { get; set; }
    public int ConStockBajo { get; set; }
    public int SinExistencias { get; set; }
    public decimal TotalEntradas { get; set; }
    public decimal TotalSalidas { get; set; }

    public List<InventarioReporteFilaDto> Productos { get; set; } = [];
    public List<InventarioCategoriaReporteDto> PorCategoria { get; set; } = [];
}

// Datos crudos que entrega la capa de datos.
public class DatosInventarioDto
{
    public List<InventarioDto> Productos { get; set; } = [];
    public List<MovimientoReporteDto> Movimientos { get; set; } = [];
}

public class MovimientoReporteDto
{
    public int IdProducto { get; set; }
    public string TipoMovimiento { get; set; } = string.Empty;
    public decimal Cantidad { get; set; }
}

public class InventarioReporteFilaDto
{
    public string Producto { get; set; } = string.Empty;
    public string Categoria { get; set; } = string.Empty;
    public string Area { get; set; } = string.Empty;
    public string? Proveedor { get; set; }
    public string UnidadMedida { get; set; } = string.Empty;
    public decimal Stock { get; set; }
    public decimal StockMinimo { get; set; }
    public decimal Faltante { get; set; }
    public decimal Entradas { get; set; }
    public decimal Salidas { get; set; }
    public decimal Ajustes { get; set; }
    public string Estado { get; set; } = string.Empty;
}

public class InventarioCategoriaReporteDto
{
    public string Categoria { get; set; } = string.Empty;
    public string Area { get; set; } = string.Empty;
    public int Productos { get; set; }
    public int ConStockBajo { get; set; }
}
