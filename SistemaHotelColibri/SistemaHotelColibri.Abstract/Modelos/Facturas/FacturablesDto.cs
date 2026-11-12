using SistemaHotelColibri.Abstract.Comun;

namespace SistemaHotelColibri.Abstract.Modelos.Facturas;

public class ReservaFacturableDto
{
    public int IdReserva { get; set; }
    public int IdCliente { get; set; }
    public string NombreCliente { get; set; } = string.Empty;
    public string IdentificacionCliente { get; set; } = string.Empty;
    public string NumeroHabitacion { get; set; } = string.Empty;
    public DateOnly FechaEntrada { get; set; }
    public DateOnly FechaSalida { get; set; }
    public int CantidadNoches { get; set; }
    public decimal PrecioNoche { get; set; }
    public string EstadoReserva { get; set; } = string.Empty;
    public bool YaFacturada { get; set; }

    public decimal MontoHospedaje => Facturacion.Redondear(CantidadNoches * PrecioNoche);
}

public class PedidoFacturableDto
{
    public int IdPedido { get; set; }
    public int? IdCliente { get; set; }
    public string? NombreCliente { get; set; }
    public int? IdReserva { get; set; }
    public string Origen { get; set; } = string.Empty;
    public DateTime FechaHoraPedido { get; set; }
    public string EstadoPedido { get; set; } = string.Empty;
    public bool YaFacturado { get; set; }
    public List<LineaPedidoFacturableDto> Lineas { get; set; } = [];

    public decimal Total => Lineas.Sum(l => l.Subtotal);
}

public class LineaPedidoFacturableDto
{
    public string NombreProducto { get; set; } = string.Empty;
    public int Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }

    public decimal Subtotal => Facturacion.Redondear(Cantidad * PrecioUnitario);
}
