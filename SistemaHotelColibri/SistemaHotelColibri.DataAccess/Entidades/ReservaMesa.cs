using System;
using System.Collections.Generic;

namespace SistemaHotelColibri.DataAccess.Entidades;

public partial class ReservaMesa
{
    public int IdReservaMesa { get; set; }

    public int IdMesa { get; set; }

    public int? IdCliente { get; set; }

    public int IdUsuario { get; set; }

    public DateTime FechaHoraReserva { get; set; }

    public int CantidadPersonas { get; set; }

    public string EstadoReservaMesa { get; set; } = null!;

    public string? Observaciones { get; set; }

    public virtual Cliente? IdClienteNavigation { get; set; }

    public virtual Mesa IdMesaNavigation { get; set; } = null!;
}
