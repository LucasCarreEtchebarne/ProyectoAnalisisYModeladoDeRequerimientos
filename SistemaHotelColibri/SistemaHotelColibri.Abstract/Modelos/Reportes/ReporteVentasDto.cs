namespace SistemaHotelColibri.Abstract.Modelos.Reportes;

// REP-002: Reporte de ventas del restaurante (pedidos facturados, sin contar facturas anuladas).
public class ReporteVentasDto
{
    public DateOnly Desde { get; set; }
    public DateOnly Hasta { get; set; }
    public int CantidadPedidos { get; set; }
    public int ProductosVendidos { get; set; }
    public decimal Ventas { get; set; }
    public decimal Servicio { get; set; }

    public decimal TicketPromedio => CantidadPedidos == 0 ? 0 : Math.Round(Ventas / CantidadPedidos, 2);

    public List<VentaProductoReporteDto> Productos { get; set; } = [];
    public List<VentaGrupoReporteDto> PorCategoria { get; set; } = [];
    public List<VentaGrupoReporteDto> PorOrigen { get; set; } = [];
    public List<VentaDiaReporteDto> PorDia { get; set; } = [];
}

// Una línea vendida, tal como sale de la base (la usa la capa de datos).
public class LineaVentaDto
{
    public int IdPedido { get; set; }
    public DateTime FechaFactura { get; set; }
    public string TipoPedido { get; set; } = string.Empty;
    public string Producto { get; set; } = string.Empty;
    public string Categoria { get; set; } = string.Empty;
    public int Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
}

public class VentaProductoReporteDto
{
    public string Producto { get; set; } = string.Empty;
    public string Categoria { get; set; } = string.Empty;
    public int Cantidad { get; set; }
    public decimal Monto { get; set; }
    public decimal Porcentaje { get; set; }
}

public class VentaGrupoReporteDto
{
    public string Nombre { get; set; } = string.Empty;
    public int Pedidos { get; set; }
    public int Cantidad { get; set; }
    public decimal Monto { get; set; }
    public decimal Porcentaje { get; set; }
}

public class VentaDiaReporteDto
{
    public DateOnly Fecha { get; set; }
    public int Pedidos { get; set; }
    public decimal Monto { get; set; }
}
