namespace SistemaHotelColibri.Abstract.Modelos.Facturas;

// Datos que llegan al cobrar la cuenta de un pedido.
public class SolicitudCobroDto
{
    public int IdPedido { get; set; }
    public int IdUsuario { get; set; }
    public int? IdCliente { get; set; }
    public string MetodoPago { get; set; } = string.Empty;
    public decimal MontoTotal { get; set; }
    public decimal? MontoRecibido { get; set; }
    public decimal? CambioDevuelto { get; set; }
    public string? TipoTarjeta { get; set; }
    public string? UltimosCuatroDigitos { get; set; }
    public string? NumeroReferencia { get; set; }
}
