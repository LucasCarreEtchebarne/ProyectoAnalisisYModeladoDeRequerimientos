using SistemaHotelColibri.Abstract.Comun;

namespace SistemaHotelColibri.Abstract.Modelos.Pedidos;

public class PedidoDto
{
    public int IdPedido { get; set; }

    public int? IdCliente { get; set; }

    public int IdUsuario { get; set; }

    public int? IdMesa { get; set; }

    public int? IdHabitacion { get; set; }

    public int? IdReserva { get; set; }

    public string TipoPedido { get; set; } = string.Empty;

    public string EstadoPedido { get; set; } = Estados.Pedido.Pendiente;

    public DateTime FechaHoraPedido { get; set; }

    public string? Observaciones { get; set; }

    public string? NombreCliente { get; set; }

    public int? NumeroMesa { get; set; }

    public string? NumeroHabitacion { get; set; }

    public List<DetallePedidoDto> Detalles { get; set; } = [];

    public decimal Total => Detalles.Sum(detalle => detalle.Subtotal);

    public string Origen => TipoPedido switch
    {
        "Mesa" => NumeroMesa.HasValue ? $"Mesa {NumeroMesa}" : "Mesa",
        "Habitacion" => string.IsNullOrEmpty(NumeroHabitacion) ? "Habitación" : $"Habitación {NumeroHabitacion}",
        _ => "Para llevar"
    };
}