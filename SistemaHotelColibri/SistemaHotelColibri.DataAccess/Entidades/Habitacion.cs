using System;
using System.Collections.Generic;

namespace SistemaHotelColibri.DataAccess.Entidades;

public partial class Habitacion
{
    public int IdHabitacion { get; set; }

    public string NumeroHabitacion { get; set; } = null!;

    public string TipoHabitacion { get; set; } = null!;

    public int Capacidad { get; set; }

    public decimal Precio { get; set; }

    public string EstadoHabitacion { get; set; } = null!;

    public int Piso { get; set; }

    public string? Descripcion { get; set; }

    public virtual ICollection<Housekeeping> Housekeeping { get; set; } = new List<Housekeeping>();

    public virtual ICollection<Pedido> Pedido { get; set; } = new List<Pedido>();

    public virtual ICollection<Reserva> Reserva { get; set; } = new List<Reserva>();
}
