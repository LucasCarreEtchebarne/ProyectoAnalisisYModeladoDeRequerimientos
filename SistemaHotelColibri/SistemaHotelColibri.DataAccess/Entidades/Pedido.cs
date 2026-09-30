using System;
using System.Collections.Generic;

namespace SistemaHotelColibri.DataAccess.Entidades;

public partial class Pedido
{
    public int IdPedido { get; set; }

    public int? IdCliente { get; set; }

    public int IdUsuario { get; set; }

    public int? IdMesa { get; set; }

    public int? IdHabitacion { get; set; }

    public int? IdReserva { get; set; }

    public string TipoPedido { get; set; } = null!;

    public string EstadoPedido { get; set; } = null!;

    public DateTime FechaHoraPedido { get; set; }

    public string? Observaciones { get; set; }

    public virtual ICollection<DetalleFactura> DetalleFactura { get; set; } = new List<DetalleFactura>();

    public virtual ICollection<DetallePedido> DetallePedido { get; set; } = new List<DetallePedido>();

    public virtual Cliente? IdClienteNavigation { get; set; }

    public virtual Habitacion? IdHabitacionNavigation { get; set; }

    public virtual Mesa? IdMesaNavigation { get; set; }

    public virtual Reserva? IdReservaNavigation { get; set; }

    public virtual ICollection<MovimientoInventario> MovimientoInventario { get; set; } = new List<MovimientoInventario>();
}
