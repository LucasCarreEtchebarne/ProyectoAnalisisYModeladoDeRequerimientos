using System;
using System.Collections.Generic;

namespace SistemaHotelColibri.DataAccess.Entidades;

public partial class Reserva
{
    public int IdReserva { get; set; }

    public int IdCliente { get; set; }

    public int IdHabitacion { get; set; }

    public DateOnly FechaEntrada { get; set; }

    public DateOnly FechaSalida { get; set; }

    public int CantidadHuespedes { get; set; }

    public decimal PrecioNoche { get; set; }

    public int? CantidadNoches { get; set; }

    public decimal? MontoHospedaje { get; set; }

    public string EstadoReserva { get; set; } = null!;

    public DateTime FechaRegistro { get; set; }

    public string? Observaciones { get; set; }

    public virtual ICollection<DetalleFactura> DetalleFactura { get; set; } = new List<DetalleFactura>();

    public virtual Cliente IdClienteNavigation { get; set; } = null!;

    public virtual Habitacion IdHabitacionNavigation { get; set; } = null!;

    public virtual ICollection<Pedido> Pedido { get; set; } = new List<Pedido>();
}
