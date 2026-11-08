using SistemaHotelColibri.Abstract.Modelos.Pedidos;
using SistemaHotelColibri.Abstract.Modelos.Reservas;

namespace SistemaHotelColibri.Abstract.Modelos.Clientes;

public class HistorialClienteDto
{
    public ClienteDto Cliente { get; set; } = new();
    public List<ReservaDto> Reservas { get; set; } = [];
    public List<PedidoDto> Pedidos { get; set; } = [];
    public List<FacturaHistorialDto> Facturas { get; set; } = [];
}

public class FacturaHistorialDto
{
    public int IdFactura { get; set; }
    public string? NumeroFactura { get; set; }
    public DateTime FechaEmision { get; set; }
    public string TipoFactura { get; set; } = string.Empty;
    public decimal MontoTotal { get; set; }
    public string EstadoFactura { get; set; } = string.Empty;
}