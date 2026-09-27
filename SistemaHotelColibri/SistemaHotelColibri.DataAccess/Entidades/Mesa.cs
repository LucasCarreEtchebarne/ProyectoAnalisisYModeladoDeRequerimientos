using System;
using System.Collections.Generic;

namespace SistemaHotelColibri.DataAccess.Entidades;

public partial class Mesa
{
    public int IdMesa { get; set; }

    public int NumeroMesa { get; set; }

    public int Capacidad { get; set; }

    public string EstadoMesa { get; set; } = null!;

    public string? Descripcion { get; set; }

    public virtual ICollection<Pedido> Pedido { get; set; } = new List<Pedido>();

    public virtual ICollection<ReservaMesa> ReservaMesa { get; set; } = new List<ReservaMesa>();
}
