namespace SistemaHotelColibri.Abstract.Modelos.Facturas;

public class FacturaDto
{
    public int IdFactura { get; set; }
    public string? NumeroFactura { get; set; }
    public int? IdCliente { get; set; }
    public string? NombreCliente { get; set; }
    public string? IdentificacionCliente { get; set; }
    public int IdUsuario { get; set; }
    public string NombreUsuario { get; set; } = string.Empty;
    public DateTime FechaEmision { get; set; }
    public string TipoFactura { get; set; } = string.Empty;
    public decimal MontoTotal { get; set; }
    public string EstadoFactura { get; set; } = string.Empty;
    public int? NumeroMesa { get; set; }
    public string? NumeroHabitacion { get; set; }
    public string? TipoPedido { get; set; }
    public int? IdPedido { get; set; }

    public List<DetalleFacturaDto> Detalles { get; set; } = [];
    public List<PagoDto> Pagos { get; set; } = [];
}

public class DetalleFacturaDto
{
    public string Descripcion { get; set; } = string.Empty;
    public decimal Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
    public decimal Subtotal => Cantidad * PrecioUnitario;
}

public class PagoDto
{
    public string MetodoPago { get; set; } = string.Empty;
    public decimal MontoPagado { get; set; }
    public decimal? MontoRecibido { get; set; }
    public decimal? CambioDevuelto { get; set; }
    public string? TipoTarjeta { get; set; }
    public string? UltimosCuatroDigitos { get; set; }
    public string? NumeroReferencia { get; set; }
    public DateTime FechaHoraPago { get; set; }
}
