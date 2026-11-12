using SistemaHotelColibri.Abstract.Comun;

namespace SistemaHotelColibri.Abstract.Modelos.Facturas;

public class FacturaDto
{
    public int IdFactura { get; set; }
    public string NumeroFactura { get; set; } = string.Empty;
    public int? IdCliente { get; set; }
    public string? NombreCliente { get; set; }
    public string? IdentificacionCliente { get; set; }
    public int IdUsuario { get; set; }
    public string? NombreUsuario { get; set; }
    public DateTime FechaEmision { get; set; }
    public string TipoFactura { get; set; } = string.Empty;
    public decimal Subtotal { get; set; }
    public decimal MontoServicio { get; set; }
    public decimal MontoIva { get; set; }
    public decimal MontoTotal { get; set; }
    public string EstadoFactura { get; set; } = Estados.Factura.Pendiente;
    public string? MotivoAnulacion { get; set; }
    public DateTime? FechaAnulacion { get; set; }
    public List<DetalleFacturaDto> Detalles { get; set; } = [];
    public List<PagoDto> Pagos { get; set; } = [];

    public decimal TotalPagado => Pagos.Where(p => p.EstadoPago == Estados.Pago.Aprobado).Sum(p => p.MontoPagado);

    public decimal Saldo => EstadoFactura == Estados.Factura.Anulada ? 0 : Math.Max(0, MontoTotal - TotalPagado);

    public string ClienteMostrado => NombreCliente ?? "Cliente de contado";
}

public class DetalleFacturaDto
{
    public int IdDetalleFactura { get; set; }
    public string TipoConcepto { get; set; } = string.Empty;
    public int? IdReserva { get; set; }
    public int? IdPedido { get; set; }
    public int? IdEvento { get; set; }
    public string Descripcion { get; set; } = string.Empty;
    public decimal Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }

    public decimal Subtotal => Facturacion.Redondear(Cantidad * PrecioUnitario);
}

public class PagoDto
{
    public int IdPago { get; set; }
    public int IdFactura { get; set; }
    public int IdUsuario { get; set; }
    public string MetodoPago { get; set; } = string.Empty;
    public decimal MontoPagado { get; set; }
    public decimal? MontoRecibido { get; set; }
    public decimal? CambioDevuelto { get; set; }
    public string? TipoTarjeta { get; set; }
    public string? UltimosCuatroDigitos { get; set; }
    public string? NumeroReferencia { get; set; }
    public DateTime FechaHoraPago { get; set; }
    public string EstadoPago { get; set; } = Estados.Pago.Aprobado;
}
